using Regulus.Project.GameProject1.Game.Play;
using System;
using System.Linq;
using Regulus.Network.Rudp;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;
using System.Threading.Tasks;

namespace Regulus.Project.GameProject1.Play
{
    public class Server : Remote.IEntry
    {
        private readonly Utility.LogFileRecorder _LogRecorder;

        private readonly Utility.StatusMachine _Machine;

        private readonly Utility.Updater _Updater;
        private readonly System.Threading.Tasks.Task _Loop;
        volatile bool _EnableLoop;
        private Center _Center;

        private Storage.User.Proxy _Storage;

        private Storage.User.IUser _StorageUser;

        private CustomType.Verify _StorageVerifyData;

        
        private readonly Regulus.Network.Rudp.ConnectProvider _Provider;

        public Server()
        {
            _Provider = new ConnectProvider(new UdpSocket());
            this._LogRecorder = new Utility.LogFileRecorder("Play");

            this._StorageVerifyData = new CustomType.Verify();

            this._Machine = new Utility.StatusMachine();
            this._Updater = new Utility.Updater();

            _Loop = new System.Threading.Tasks.Task(_Update);
            
        }

        void Remote.IBinderProvider.AssignBinder(Remote.IBinder binder)
        {
            if (_Center != null)
            {
                _Join(binder);
            }

        }

        private void _Join(IBinder binder)
        {
            
            this._Center.Join(binder);
        }

        void _Update()
        {          
            
            while(_EnableLoop)
            {
                this._Updater.Working();
                this._Machine.Update();
            }
                
        }

        

        void Framework.IBootable.Shutdown()
        {
            _EnableLoop = false;
            _Loop.Wait();
            this._Updater.Shutdown();
            _Machine.Termination();
            _Provider.Shutdown();
            Utility.Singleton<Utility.Log>.Instance.RecordEvent -= this._LogRecorder.Record;
            
            AppDomain.CurrentDomain.UnhandledException -= this.CurrentDomain_UnhandledException;
        }

        void Framework.IBootable.Launch()
        {
            
            _Provider.Launch();
            

            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

            this._BuildParams();
            this._BuildUser();
            this._LoadData();
            Utility.Singleton<Utility.Log>.Instance.RecordEvent += this._LogRecorder.Record;
            this._Updater.Add(this._Storage);
            this._ToConnectStorage(this._StorageUser);

            _EnableLoop = true;
            _Loop.Start();
        }

        private void _LoadData()
        {
            Server._BuildGroup();

            var buffer = System.IO.File.ReadAllBytes("entitys.txt");
            var entitys = Utility.Serialization.Read<EntityData[]>(buffer);
            Singleton<Resource>.Instance.Entitys = entitys;

            var skillBuffer = System.IO.File.ReadAllBytes("skills.txt");
            var skillDatas = Utility.Serialization.Read<SkillData[]>(skillBuffer);
            Singleton<Resource>.Instance.SkillDatas= skillDatas;

            var itemsBuffer = System.IO.File.ReadAllBytes("items.txt");
            var items = Utility.Serialization.Read<ItemPrototype[]>(itemsBuffer);
            Singleton<Resource>.Instance.Items = items;

            var itemFormulasBuffer = System.IO.File.ReadAllBytes("itemformulas.txt");
            var itemFormulas = Utility.Serialization.Read<ItemFormula[]>(itemFormulasBuffer);
            Singleton<Resource>.Instance.Formulas = itemFormulas;
        }

        private static void _BuildGroup()
        {
            var entityGroupLayoutBuffer1 = System.IO.File.ReadAllBytes("entitygrouplayout.txt");
            var entityGroupLayouts = Utility.Serialization.Read<EntityGroupLayout[]>(entityGroupLayoutBuffer1);

            var entityGroupLayoutBuffer2 = System.IO.File.ReadAllBytes("town1.txt");
            var town1 = Utility.Serialization.Read<EntityGroupLayout[]>(entityGroupLayoutBuffer2);

            var entityGroupLayoutBuffer3 = System.IO.File.ReadAllBytes("town2.txt");
            var town2 = Utility.Serialization.Read<EntityGroupLayout[]>(entityGroupLayoutBuffer3);

            Singleton<Resource>.Instance.EntityGroupLayouts = entityGroupLayouts.Union(town1).Union(town2).ToArray();
        }

        private void _BuildParams()
        {
            var config = new Utility.Ini(this._ReadConfig());

            this._StorageVerifyData.IPAddress = config.Read("Storage", "ipaddr");
            this._StorageVerifyData.Port = int.Parse(config.Read("Storage", "port"));
            this._StorageVerifyData.Account = config.Read("Storage", "account");
            this._StorageVerifyData.Password = config.Read("Storage", "password");

            
        }

        private void _BuildUser()
        {                        
            if (this._IsIpAddress(this._StorageVerifyData.IPAddress))
            {
                throw new NotImplementedException();
                //this._Storage = new Storage.User.Proxy(new Storage.User.RemoteFactory(_Protocol,_Provider));
                //this._StorageUser = this._Storage.SpawnUser("user");
            }
            else
            {
                _StorageVerifyData.IPAddress = "127.0.0.1";
                var center = new Game.Storage.Center(new Game.DummyFrature());
                this._Updater.Add(center);


                var types = new System.Collections.Generic.List<System.Type>();
                string dataNamesapce = "Regulus.Project.GameProject1.Data";
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.IsDynamic)
                        continue;
                    Regulus.Utility.Log.Instance.WriteInfo($"Storage find common ,{assembly.GetName().Name }");
                    if (assembly.GetName().Name != dataNamesapce)
                        continue;
                    
                    var ab = new Regulus.Remote.Protocol.AssemblyBuilder(Remote.Protocol.Essential.CreateFromDomain(assembly));
                    var protocolAsm = ab.Create();
                    var protocol = Regulus.Remote.Protocol.ProtocolProvider.Create(protocolAsm);
                    var factory = new Storage.User.StandaloneFactory(center, protocol);
                    this._Storage = new Storage.User.Proxy(factory);
                    this._StorageUser = this._Storage.SpawnUser("user");
                    break;
                }
                
            }
        }

        private bool _IsIpAddress(string ip)
        {
            System.Net.IPAddress ipaddr;
            return System.Net.IPAddress.TryParse(ip, out ipaddr);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            this._LogRecorder.Record(ex.ToString());
            this._LogRecorder.Save();
        }

        private void _ToConnectStorage(Storage.User.IUser user)
        {
            var stage = new Storage.User.ConnectStorageStage(user, this._StorageVerifyData.IPAddress, this._StorageVerifyData.Port);
            stage.OnDoneEvent += this._ConnectResult;
            this._Machine.Push(stage);
        }

        private void _ConnectResult(bool result)
        {
            if (result)
            {
                this._ToVerifyStorage(this._StorageUser);
            }
            else
            {
                throw new SystemException("stroage connect fail");
            }
        }

        private void _ToVerifyStorage(Storage.User.IUser user)
        {
            var stage = new Storage.User.VerifyStorageStage(user, this._StorageVerifyData.Account, this._StorageVerifyData.Password);
            stage.OnDoneEvent += this._VerifyResult;
            this._Machine.Push(stage);
        }

        private void _VerifyResult(bool verify_result)
        {
            if (verify_result)
            {
                this._ToBuildClient();
            }
            else
            {
                throw new SystemException("stroage verify fail");
            }
        }

       

        

        private void _ToBuildClient()
        {
            var stage = new BuildCenterStage(this._StorageUser);

            stage.OnBuiledEvent += this._Play;

            this._Machine.Push(stage);
        }

        private void _Play(BuildCenterStage.ExternalFeature features)
        {
            this._Center = new Center(
                features.AccountFinder,                
                features.GameRecorder
                );

            this._Updater.Add(this._Center);
        }

        
        private string _ReadConfig()
        {
            return System.IO.File.ReadAllText("config.ini");
        }
    }
}
