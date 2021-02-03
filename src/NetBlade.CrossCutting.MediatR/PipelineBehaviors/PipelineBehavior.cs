using MediatR;
using NetBlade.Core;
using NetBlade.Core.EventContexts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.MediatR.PipelineBehaviors
{
    public class PipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly EventContext _eventContext;

        public PipelineBehavior(EventContext eventContext)
        {
            this._eventContext = eventContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse nextResponse = await next();
            if (nextResponse is IResponse response && !response.Success)
            {
                return nextResponse;
            }

            try
            {
                await this._eventContext.RaiseEvents();
            }
            catch (Exception e)
            {
                if (nextResponse is IResponse response2)
                {
                    response2.SetException(e);
                }
                else
                {
                    throw;
                }
            }

            return nextResponse;
        }
    }
}
