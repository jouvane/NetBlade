using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NetBlade.Core.Commands;
using NetBlade.Core.Domain.Test.Events.Model;
using NetBlade.Core.Domain.Test.Events.Model.Commands;
using NetBlade.Core.Domain.Test.Events.Model.DomainEvents;
using NetBlade.Core.Domain.Test.Events.Model.Entitys;
using NetBlade.Core.Domain.Test.Events.Model.EventHandlers;
using NetBlade.Core.Enum;
using NetBlade.Core.EventContexts;
using NetBlade.Core.Events;
using NetBlade.Core.Mediator;
using NetBlade.Core.Transaction;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetBlade.Core.Domain.Test.Events
{
    public class EventHandlersTest : IClassFixture<Startup>
    {
        public EventHandlersTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
            this._logger = this._serviceProvider.GetService<ILogger<EventHandlersTest>>();
            this._eventContext = this._serviceProvider.GetService<EventContext>();
            this._mediatorHandler = this._serviceProvider.GetService<IMediatorHandler>();
            this._sendMsgTest = this._serviceProvider.GetService<ISendMsgTest>();
            this._transactionManager = this._serviceProvider.GetService<ITransactionManager>();
        }

        public readonly ILogger<EventHandlersTest> _logger;
        private readonly EventContext _eventContext;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ISendMsgTest _sendMsgTest;
        private readonly ServiceProvider _serviceProvider;
        private readonly ITransactionManager _transactionManager;

        [Fact]
        public async Task RegisterDeleteEventTest()
        {
            ProdutoModel p = new ProdutoModel { ID = 1 };
            p.RegisterDeleteEvent();

            Assert.Single(p.GetEvents());
            Assert.IsType<DomainEvent<ProdutoModel>>(p.GetEvents().First());
            Assert.Equal(OperationType.Delete, (p.GetEvents().First() as DomainEvent<ProdutoModel>).OperationType);
            await Task.CompletedTask;
        }

        [Fact]
        public async Task RegisterInsertEventTest()
        {
            ProdutoModel p = new ProdutoModel { ID = 1 };
            p.RegisterInsertEvent();

            Assert.Single(p.GetEvents());
            Assert.IsType<DomainEvent<ProdutoModel>>(p.GetEvents().First());
            Assert.Equal(OperationType.Insert, (p.GetEvents().First() as DomainEvent<ProdutoModel>).OperationType);
            await Task.CompletedTask;
        }

        [Fact]
        public async Task RegisterUpdateEventTest()
        {
            ProdutoModel p = new ProdutoModel { ID = 1 };
            p.RegisterUpdateEvent();

            Assert.Single(p.GetEvents());
            Assert.IsType<DomainEvent<ProdutoModel>>(p.GetEvents().First());
            Assert.Equal(OperationType.Update, (p.GetEvents().First() as DomainEvent<ProdutoModel>).OperationType);
            await Task.CompletedTask;
        }

        [Fact]
        public async Task EntityRegisterEventUpdateTest()
        {
            ProdutoModel p = new ProdutoModel
            {
                ID = 1
            };

            double oldValue = 1.5d;
            p.RegisterEvent<AtualizarValorUnitarioEvent>(oldValue);

            Assert.Single(p.GetEvents());
            await Task.CompletedTask;
        }


        [Fact]
        public async Task EventHandlersTestDoisEventHandlerTest()
        {
            Mock<ITransactionManager> mockTransactionManager = Mock.Get(this._transactionManager);
            Mock<ISendMsgTest> mockSendMsgTest = Mock.Get(this._sendMsgTest);

            mockTransactionManager
               .Setup(v => v.Begin(It.IsAny<IsolationLevel>()))
               .Callback(() => { mockTransactionManager.Setup(s => s.Started).Returns(true); });

            mockTransactionManager
               .Setup(v => v.Commit())
               .Callback(() => { mockTransactionManager.Setup(s => s.Started).Returns(false); });

            int qtd = 0;
            mockSendMsgTest.Setup(s => s.SendMsg(It.IsAny<string>()))
               .Callback<string>(msg =>
                {
                    if (string.Format(PromocaoEmailEventHandler.MSG, "Sabao", 100, 10.5).Equals(msg) || string.Format(MensagemChatEventHandler.MSG, "Sabao", 100, 10.5).Equals(msg))
                    {
                        qtd++;
                    }
                    else
                    {
                        Assert.Null(msg);
                    }
                });

            AtualizarValorUnitarioCommand cmd = new AtualizarValorUnitarioCommand
            {
                Codigo = 1010,
                Valor = 10.5
            };

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);
            Assert.True(resp.Success);

            mockSendMsgTest
               .Verify(v => v.SendMsg(It.IsAny<string>()), Times.Exactly(2));

            mockTransactionManager.Reset();
            mockSendMsgTest.Reset();

            Assert.Equal(2, qtd);
        }

        [Fact]
        public async Task EventHandlersTestSemEventHandlerTest()
        {
            Mock<ITransactionManager> mockTransactionManager = Mock.Get(this._transactionManager);
            Mock<ISendMsgTest> mockSendMsgTest = Mock.Get(this._sendMsgTest);

            mockTransactionManager
               .Setup(v => v.Begin(It.IsAny<IsolationLevel>()))
               .Callback(() => { mockTransactionManager.Setup(s => s.Started).Returns(true); });

            mockTransactionManager
               .Setup(v => v.Commit())
               .Callback(() => { mockTransactionManager.Setup(s => s.Started).Returns(false); });

            AtualizarValorUnitarioCommand cmd = new AtualizarValorUnitarioCommand
            {
                Codigo = 1010,
                Valor = 2000
            };

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);

            mockSendMsgTest
               .Verify(v => v.SendMsg(It.IsAny<string>()), Times.Exactly(0));

            mockTransactionManager.Reset();
            mockSendMsgTest.Reset();

            Assert.True(resp.Success);
        }
    }
}
