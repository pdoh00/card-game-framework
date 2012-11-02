using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.TableCommands
{
    public class CreateNewTableCommand : ICommand
    {
        public readonly string TableType;
        public CreateNewTableCommand(string tableType)
        {
            TableType = tableType;
        }
    }
}
