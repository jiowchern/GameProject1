using System;

using UnityEngine;
using System.Collections;

using Regulus.Utility;

using UnityEngine.SceneManagement;

public class ClientInitial : MonoBehaviour
{
    
	
	// Use this for initialization
	void Start () {		
        
	}
	
	// Update is called once per frame
	void Update () 
	{
        
		if (Client.Instance != null && Client.Instance.User != null)
		{
            SceneChanger.Instance.ToLogin();
        }
	}


    public void Online()
    {
        SceneChanger.Instance.Initial(Client.MODE.REMOTING);
        
    }

    public void Offline()
    {
        SceneChanger.Instance.Initial(Client.MODE.STANDALONE);
        
    }
}
