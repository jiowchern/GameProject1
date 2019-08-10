using System;

using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;

using UnityEngine.UI;

public class PadController : MonoBehaviour
{
    private Regulus.Project.GameProject1.Data.IMoveController _MoveController;
    private Client _Client;

    

    // Use this for initialization
	void Start ()
	{

        gameObject.SetActive(false);
#if UNITY_ANDROID
        _Client = Client.Instance;
	    if (_Client != null)
	    {
            _Client.User.MoveControllerProvider.Supply += MoveControllerProvider_Supply;
	        _Client.User.MoveControllerProvider.Unsupply += MoveControllerProviderOnUnsupply;
	    }
#endif
    }

    

    void OnDestroy()
    {
        if (_Client != null)
        {
            _Client.User.MoveControllerProvider.Unsupply -= MoveControllerProviderOnUnsupply;
            _Client.User.MoveControllerProvider.Supply -= MoveControllerProvider_Supply;
        }
    }
    // Update is called once per frame
    void Update ()
    {
	
	}
    private void MoveControllerProviderOnUnsupply(IMoveController move_controller)
    {
        gameObject.SetActive(false);
    }
    private void MoveControllerProvider_Supply(Regulus.Project.GameProject1.Data.IMoveController obj)
    {
        _MoveController = obj;
        gameObject.SetActive(true);
    }

    public void StopTrun()
    {
        _MoveController.StopTrun();
    }
    public void StopMove()
    {
        _MoveController.StopMove();
    }
    public void Right()
    {
        _MoveController.TrunRight();
    }
    public void Left()
    {
        _MoveController.TrunLeft();
    }
    public void Forward()
    {
        _MoveController.RunForward();
    }

    public void Back()
    {
        _MoveController.Backward();
    }
}
