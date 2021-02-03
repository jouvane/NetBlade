using FluentValidation;
using NetBlade.Core.Exceptions;
using NetBlade.Core.Transaction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.Core.Commands
{
    public abstract class CommandHandlerBase<TCommand, TCommandResponse> : CommandHandlerBase
        where TCommand : ICommand
        where TCommandResponse : ICommandResponse
    {
        private readonly ITransactionManager _transactionManager;

        protected CommandHandlerBase(ITransactionManager transactionManager)
        {
            this._transactionManager = transactionManager;
        }

        protected CommandHandlerBase(ITransactionManager transactionManager, IValidator<TCommand> validator)
            : this(transactionManager)
        {
            this.Validator = validator;
        }

        protected virtual ITransactionManager TransactionManager
        {
            get => this._transactionManager;
        }

        protected virtual IValidator<TCommand> Validator { get; set; }

        public virtual async Task<ICommandResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (this.Validator != null)
                {
                    this.ValidationResult = await this.Validator.ValidateAsync(command, cancellationToken);
                }

                if (this.ValidationResult == null || this.ValidationResult.IsValid)
                {
                    ICommandResponse commandResponse = await this.TransactionScope(async () => await this.HandleAsync(command));
                    return commandResponse;
                }

                DomainException ex = this.HandleDomainException();
                return this.CommandResponseFail<TCommandResponse>().SetException<TCommandResponse>(ex);
            }
            catch (Exception ex)
            {
                return this.CommandResponseFail<TCommandResponse>().SetException<TCommandResponse>(ex);
            }
        }

        protected abstract Task<TCommandResponse> HandleAsync(TCommand command);

        protected virtual async Task<ICommandResponse> TransactionScope(TransactionHandlerDelegate<ICommandResponse> next)
        {
            if (this is INotCreateTransaction)
            {
                return await next();
            }
            else
            {
                bool commit = !this.TransactionManager.Started;
                try
                {
                    if (commit)
                    {
                        this.TransactionManager.Begin();
                    }

                    ICommandResponse nextResponse = await next();
                    if (nextResponse is IResponse response && response.Success && commit)
                    {
                        if (response.Success)
                        {
                            this.TransactionManager.Commit();
                        }
                        else
                        {
                            if (response?.Exception != null && response.Exception is INotRollbackTransaction)
                            {
                                this.TransactionManager.Commit();
                            }
                            else
                            {
                                this.TransactionManager.Rollback();
                            }
                        }
                    }

                    return nextResponse;
                }
                catch (DomainException ex)
                {
                    if (commit)
                    {
                        if (ex is INotRollbackTransaction)
                        {
                            this.TransactionManager.Commit();
                        }
                        else
                        {
                            this.TransactionManager.Rollback();
                        }
                    }

                    throw;
                }
                catch
                {
                    if (commit)
                    {
                        this.TransactionManager.Rollback();
                    }

                    throw;
                }
            }
        }
    }
}