using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MemoryCommandBus : ICommandBus, ICommandHandlerRegistry
    {
        private List<ICommandHandler> _handlers = new List<ICommandHandler>();
        private List<ICommand> _commands = new List<ICommand>();

        public MemoryCommandBus(params ICommandHandler[] handlers)
        {
            this._handlers.AddRange(handlers);
        }

        public void Register(ICommandHandler handler)
        {
            this._handlers.Add(handler);
        }

        public void Send(ICommand command)
        {
            this._commands.Add(command);

            //Task.Factory.StartNew(() =>
            //{
                var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

                foreach (dynamic handler in this._handlers
                    .Where(x => handlerType.IsAssignableFrom(x.GetType())))
                {
                    handler.Handle((dynamic)command);
                }
            //});
        }

        public void Send(IEnumerable<ICommand> commands)
        {
            foreach (var @event in commands)
            {
                this.Send(@event);
            }
        }

        public IEnumerable<ICommand> Events { get { return this._commands; } }
    }
}
