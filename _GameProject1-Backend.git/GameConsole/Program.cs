using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Regulus.Project.GameProject1;
using Regulus.Project.GameProject1.Game;
using Regulus.Remote;
using Regulus.Utility;
using Regulus.Utility.WindowConsoleAppliction;

namespace GameConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            



            var console = new ClientConsole();


            console.Run();




        }
    }

    internal class ClientConsole : WindowConsole
    {
        private Regulus.Utility.Updater _Updater;
        public ClientConsole()
        {
            _Updater = new Updater();
        }
        protected override void _Launch()
        {
            
        }

        protected override void _Update()
        {
            _Updater.Working();
        }

        protected override void _Shutdown()
        {
            _Updater.Shutdown();
        }
    }
}
