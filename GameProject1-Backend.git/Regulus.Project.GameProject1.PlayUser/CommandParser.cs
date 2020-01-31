using Regulus.Framework;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;


using Regulus.Project.GameProject1.Play.User;
using System;

namespace Regulus.Project.GameProject1
{
	public class CommandParser : ICommandParsable<IUser>
	{
		private readonly Command _Command;

		private readonly IUser _User;

		private readonly Utility.Console.IViewer _View;

		public CommandParser(Command command, Utility.Console.IViewer view, IUser user)
		{
		    this._Command = command;
		    this._View = view;
		    this._User = user;
		}

		void ICommandParsable<IUser>.Clear()
		{
		    this._DestroySystem();
		}

		void ICommandParsable<IUser>.Setup(IGPIBinderFactory factory)
		{
		    this._CreateSystem();

		    this._CreateConnect(factory);

		    this._CreateOnline(factory);

		    this._CreateVerify(factory);

		    this._CreateControll(factory);

            this._CreateDevelopActor(factory);

            this._CreateVisible(factory);


            this._CreateJumpMap(factory);

            this._CreatePlayerPropertys(factory);

            this._CreateInventoryController(factory);
            

        }

	    private void _CreateInventoryController(IGPIBinderFactory factory)
	    {
	        
            
            var binder = factory.Create(this._User.InventoryControllerProvider);
            binder.Bind( (IInventoryController gpi ,string id) => gpi.Equip(id));
            binder.Bind(gpi=> gpi.Refresh() );
	        binder.SupplyEvent += _InventoryControllerSupply;
            binder.UnsupplyEvent += _InventoryControllerUnsupply;

        }

	    private void _InventoryControllerUnsupply(IInventoryController source)
	    {
            source.BagItemsEvent -= _ShowItem;
        }

	    private void _InventoryControllerSupply(IInventoryController source)
	    {
	        source.BagItemsEvent += _ShowItem;
	    }

	    private void _ShowItem(Item[] items)
	    {
	        foreach (var item in items)
	        {
	            _View.WriteLine(string.Format("item : {0}" , item.Id));
	        }
	    }

	    private void _CreatePlayerPropertys(IGPIBinderFactory factory)
	    {
            var binder = factory.Create(this._User.PlayerProperysProvider);
	        binder.SupplyEvent += _PlayerProperysSupply;
            binder.UnsupplyEvent += _PlayerProperysUnsupply;
        }

	    private void _PlayerProperysUnsupply(IPlayerProperys source)
	    {
	        _View.WriteLine(string.Format("Unsupply  Player Property {0}", source.Id));
	    }

	    private void _PlayerProperysSupply(IPlayerProperys source)
	    {
            _View.WriteLine(string.Format("Supply Player Property {0}", source.Id));
        }

	    private void _CreateJumpMap(IGPIBinderFactory factory)
	    {
            var binder = factory.Create(this._User.JumpMapProvider);
            binder.Bind( gpi=>gpi.Ready());
			

		}

	    private void _CreateVisible(IGPIBinderFactory factory)
	    {
            var binder = factory.Create(this._User.VisibleProvider);

	        binder.SupplyEvent += _VisibleSupply;
            binder.UnsupplyEvent += _VisibleUnsupply;
            
        }

	    private void _VisibleUnsupply(IVisible source)
	    {
	        _View.WriteLine(string.Format("leave actor id:{0} name:{1}", source.Id , source.Name));    
	    }

	    private void _VisibleSupply(IVisible source)
	    {
            _View.WriteLine(string.Format("enter actor id:{0} name:{1}", source.Id, source.Name));
        }

	    private void _DestroySystem()
		{

		}

		private void _ConnectResult(Regulus.Remote.Value<bool> val)
		{
		    val.OnValue += (result) => { this._View.WriteLine(string.Format("Connect result {0}", result)); };

		}

		


		private void _CreateSystem()
		{
		}



        private void _CreateControll(IGPIBinderFactory factory)
        {
            
            var controller = factory.Create(this._User.MoveControllerProvider);
            
        }

        private void _CreateVerify(IGPIBinderFactory factory)
		{
			var verify = factory.Create(this._User.VerifyProvider);
            verify.Bind<string,string, Regulus.Remote.Value<bool>>( (gpi ,account , password) => gpi.Login(account , password) , _VerifyResult);
            
		}

		private void _CreateOnline(IGPIBinderFactory factory)
		{
			var online = factory.Create(this._User.Remote.OnlineProvider);
		    online.Bind(gpi => gpi.Ping , _ShowPing ) ;
            
			online.Bind(gpi => gpi.Disconnect());
		}

	    private void _ShowPing(double obj)
	    {
	        _View.WriteLine("Ping " + obj);
	    }

	    private void _CreateConnect(IGPIBinderFactory factory)
		{
			var connect = factory.Create(this._User.Remote.ConnectProvider);

                        
			connect.BindStatic<string, int, Regulus.Remote.Value<bool>>((gpi, ip, port) => this._Connect(gpi, ip, port), _ConnectResult);

		}

		private Regulus.Remote.Value<bool> _Connect(IConnect gpi, string ip, int port)
		{
			return gpi.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), port));
		}

		private void _CreateDevelopActor(IGPIBinderFactory factory)
        {
            var binder = factory.Create(this._User.DevelopActorProvider);
            binder.Bind<float>( (gpi , view ) => gpi.SetBaseView(view) );
            binder.Bind<float>((gpi, speed) => gpi.SetSpeed(speed));
            binder.Bind<string , int>((gpi, name , count) => gpi.CreateItem(name , count));
            binder.Bind<string, float>((gpi, name, quality) => gpi.MakeItem(name, quality));
            binder.Bind<float, float>((gpi, x, y) => gpi.SetPosition(x,y));
            binder.Bind<string>((gpi, realm) => gpi.SetRealm(realm));
        }

        private void _VerifyResult(Regulus.Remote.Value<bool> val)
		{
            val.OnValue += (result)=>
            {
                this._View.WriteLine(string.Format("Verify result {0}", result));
            };
		}
	}
}


