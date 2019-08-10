using UnityEngine;
using System.Collections;
using Regulus.Project.GameProject1.Data;
using System;

using UnityEngine.SceneManagement;

public class KickDetector : MonoBehaviour {

    private Client _Client;
    
    private IAccountStatus _AccountStatus;

    
    // Use this for initialization
    void Start ()
    {
    
        _Client = Client.Instance;
        if (_Client != null)
        {
            
            _Client.User.AccountStatusProvider.Supply += _AccountStatusProvider;
            _Client.User.AccountStatusProvider.Unsupply += _ToLeave;
            _Client.User.JumpMapProvider.Supply += _JumpMap;
        }
        
    }

    private void _ToLeave(IAccountStatus obj)
    {
        _ToLogin();
    }

    private void _AccountStatusProvider(IAccountStatus obj)
    {
        obj.KickEvent += _ToLogin;
        _AccountStatus = obj;
    }

    private void _ToLogin()
    {        

        if (SceneChanger.Instance != null)
            SceneChanger.Instance.ToLogin();

        
    }

    void OnDestroy()
    {
        if(_Client != null)
        {
            _Client.User.AccountStatusProvider.Supply -= _AccountStatusProvider;
            _Client.User.AccountStatusProvider.Unsupply -= _ToLeave;
            _Client.User.JumpMapProvider.Supply -= _JumpMap;
        }
        if (_AccountStatus != null)
            _AccountStatus.KickEvent -= _ToLogin;
    }

    private void _JumpMap(IJumpMap obj)
    {
        //SceneChanger.Instance.ToJumpMap();
    }

    // Update is called once per frame
	void Update () {
	
	}
}
