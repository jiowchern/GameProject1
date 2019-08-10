using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using Regulus.Framework;
using Regulus.Project.GameProject1;
using Regulus.Project.GameProject1.Data;

using Regulus.Utility;
using System.Linq;
using Regulus.Network.Rudp;
using Regulus.Remote;
using Regulus.Project.GameProject1.Game.Play;

public class Client : MonoBehaviour , IEntry
{
    
	public static Client Instance {
		get { return GameObject.FindObjectOfType<Client>();  }
	}
	public enum MODE
	{
		STANDALONE,REMOTING
	}

	
	public ResourceOwner Resource;
	public MODE Mode;
	public Console Console;

   
	private Client<IUser> _Client;
	private readonly Regulus.Utility.Updater _Updater;

	private IOnline _Online;

    private Center _Center;
    
    public Client()
	{
    
        var feature = new Regulus.Project.GameProject1.Game.DummyFrature();
        _Center= new Center(feature, feature);
        _Updater = new Updater();
		
	}
	// Use this for initialization
	void Start ()
	{
		Mode = SceneChanger.Mode;
		
        Regulus.Utility.Log.Instance.RecordEvent += _RegulusLog;
        
        var client = new Regulus.Framework.Client<Regulus.Project.GameProject1.IUser>(Console, Console.Command );
		client.ModeSelectorEvent += _ToMode;

		
		_Client = client;
		_Updater.Add(_Client);
		Debug.Log("Started .");

        

    }

    private void _RegulusLog(string message)
    {
        Debug.Log(message);
    }

    

	private void _SupplyOnline(IOnline obj)
	{
		_Online = obj;
	}

	private void _ToMode(GameModeSelector<IUser> selector)
	{
        var asms = System.AppDomain.CurrentDomain.GetAssemblies();
        var asm = asms.Where(a => a.ManifestModule.Name == "Regulus.Project.GameProject1.Protocol.dll").First();
        
        UserProvider<IUser> provider;
		if (Mode == MODE.REMOTING)
		{
			selector.AddFactoty("r", new RemoteUserFactory(asm));
			provider = selector.CreateUserProvider("r");
		}
		else
		{

            this.Resource.Load();
			
			
			_Updater.Add(_Center);
			selector.AddFactoty("s", new StandaloneUserFactory(this, asm));
			provider = selector.CreateUserProvider("s");
		}

		var user = provider.Spawn("user");
		provider.Select("user");

		User = user;
		User.Remote.OnlineProvider.Supply += _SupplyOnline;
		User.Remote.OnlineProvider.Unsupply += _UnsupplyOnline;
		User.JumpMapProvider.Supply += _JumpMap;
	}

	private void _JumpMap(IJumpMap obj)
	{
		
		SceneChanger.Instance.ToRealm(obj.Realm);
		obj.Ready();
	}
    
	public IUser User { get; private set; }

	public float Ping
	{
		get
		{
			if (_Online != null)
				return (float)_Online.Ping;
			return 0.0f;
		}
	}

	// Update is called once per frame
    void IBootable.Launch()
    {
        
    }

    

    void IBootable.Shutdown()
    {
        
    }

    void Update () {

	    try
	    {
	        _Updater.Working();
	    }
	    catch (Regulus.DeserializeException de)
	    {
            Debug.Log(de);
        }
		/*catch(Exception e)
		{
			Debug.Log(e);
			
		}*/
		
	}

	void OnDestroy()
	{
		User.JumpMapProvider.Supply -= _JumpMap;
		User.Remote.OnlineProvider.Supply -= _SupplyOnline;
		User.Remote.OnlineProvider.Unsupply -= _UnsupplyOnline;
		_Updater.Shutdown();
	    

    }

	private void _UnsupplyOnline(IOnline obj)
	{
		_Online = null;
	}

    void IBinderProvider.AssignBinder(ISoulBinder binder)
    {
        _Center.AssignBinder(binder);
    }
}
