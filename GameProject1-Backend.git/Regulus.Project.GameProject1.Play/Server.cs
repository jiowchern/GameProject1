
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

    public class Server : Remote.IEntry , System.IDisposable
    {
        private readonly Utility.LogFileRecorder _LogRecorder;

        

        private readonly Utility.Updater _Updater;
        private readonly System.Threading.Tasks.Task _Loop;
        volatile bool _EnableLoop;
        private readonly Center _Center;

        

        public Server()
        {
        
            this._LogRecorder = new Utility.LogFileRecorder("Play");
            
            this._Updater = new Utility.Updater();
            var df =  new Regulus.Project.GameProject1.Game.DummyFrature();
            _Center = new Center(df, df);
            _Updater.Add(_Center);
            _Loop = new System.Threading.Tasks.Task(_Update);
            _Launch();
        }

        void Remote.IBinderProvider.AssignBinder(Remote.IBinder binder)
        {
            _Join(binder);
        }

        private void _Join(IBinder binder)
        {

            this._Center.Join(binder);
        }

        void _Update()
        {

            while (_EnableLoop)
            {
                this._Updater.Working();                
            }

        }



        void _Shutdown()
        {
            _EnableLoop = false;
            _Loop.Wait();
            this._Updater.Shutdown();
            
            
            Utility.Singleton<Utility.Log>.Instance.RecordEvent -= this._LogRecorder.Record;

            AppDomain.CurrentDomain.UnhandledException -= this.CurrentDomain_UnhandledException;
        }

        void _Launch()
        {

            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
            Utility.Singleton<Utility.Log>.Instance.RecordEvent += this._LogRecorder.Record;

          
            this._BuildUser();
            this._LoadData();
            
            

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
            Singleton<Resource>.Instance.SkillDatas = skillDatas;

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

        

        private void _BuildUser()
        {
            var center = new Game.Storage.Center(new Game.DummyFrature());
            this._Updater.Add(center);
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            this._LogRecorder.Record(ex.ToString());
            this._LogRecorder.Save();
        }

   
        void IDisposable.Dispose()
        {
            _Shutdown();
        }
    }
}
