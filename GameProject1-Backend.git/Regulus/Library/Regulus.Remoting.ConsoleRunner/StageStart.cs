﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Regulus.Network;
using Regulus.Network.Rudp;
using Regulus.Utility;
using Console = Regulus.Utility.Console;

namespace Regulus.Remote.Soul.Console
{
    internal class StageStart : IStage
    {
        public event Action<IEntry, IProtocol, int, IListenable> DoneEvent;

        private readonly Command _Command;

        private readonly Regulus.Utility.Console.IViewer _View;

        private readonly string[] _FirstCommand;


        public StageStart(Command command, Regulus.Utility.Console.IViewer view, string[] first_command)
        {
            _View = view;
            _FirstCommand = first_command;
            _Command = command;
        }

        void IStage.Enter()
        {

            _Command.RegisterLambda<StageStart, string>(this, (instance, ini_path) => instance.LaunchIni(ini_path));


            _View.WriteLine("======Ini file format description=====");
            _View.WriteLine("Example.");
            _View.WriteLine("[Launch]");
            _View.WriteLine("port = 12345");
            _View.WriteLine("common_path = path/common.dll");
            _View.WriteLine("common_namespace = YourNamespace.YourProjectCommon");
            _View.WriteLine("project_path = path/project.dll");
            _View.WriteLine("project_entry = YourNamespace.YourProjectClassName");
            _View.WriteLine("rudp = true");
            _View.WriteLine("======================================");


            if (_HasFirstCommand())
            {
                _RunFirstCommand();
            }

        }

        private void _RunFirstCommand()
        {
            var command = _FirstCommand[0];
            var args = _FirstCommand.Skip(1).ToArray();
            var arg = string.Join(" ", args);
            _View.WriteLine(string.Format("First Run Command {0} {1}.", command, arg));
            _Command.Run(command, args);
        }

        private bool _HasFirstCommand()
        {
            return _FirstCommand.Length > 0;
        }


        void IStage.Leave()
        {
            _Command.Unregister("Launch");
            _Command.Unregister("LaunchIni");
        }

        void IStage.Update()
        {
        }

        private void LaunchIni(string path)
        {
            try
            {
                var ini = new Ini(File.ReadAllText(path));
                var port_string = ini.Read("Launch", "port");
                var port = int.Parse(port_string);
                var dllpath = ini.Read("Launch", "project_path");
                var className = ini.Read("Launch", "project_entry");
                var commonPath = ini.Read("Launch", "common_path");
                var rudp = ini.Read("Launch", "rudp");

                Launch(port, dllpath, className, commonPath, rudp == "true");
            }
            catch (Exception ex)
            {
                _View.WriteLine(ex.ToString());
            }
        }



        public void Launch(int port, string project_path, string project_entry_name, string common_path, bool rudp)
        {



            var instance = _CreateProject(project_path, project_entry_name);

            var library = _CreateProtocol(common_path);

            IListenable server = _CreateServer(rudp);

            DoneEvent(instance, library, port, server);
        }

        private IListenable _CreateServer(bool rudp)
        {
            if (rudp)
                return new Regulus.Network.Rudp.Listener(new UdpSocket());

            return new Regulus.Network.Tcp.Listener();
        }

        private static IEntry _CreateProject(string project_path, string project_entry_name)
        {
            var assembly = Assembly.LoadFrom(project_path);
            var instance = assembly.CreateInstance(project_entry_name) as IEntry;
            return instance;
        }

        private IProtocol _CreateProtocol(string common_path)
        {



            var assembly = Assembly.LoadFrom(common_path);
            
            var buidler = new Regulus.Remote.Protocol.AssemblyBuilder(assembly.GetExportedTypes() );
            var asm = buidler.Create();
            
            return Regulus.Remote.Protocol.ProtocolProvider.Create(asm) ;
        }


    }
}

