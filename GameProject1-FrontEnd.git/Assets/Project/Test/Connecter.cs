using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connecter : MonoBehaviour
{


    //public ItisNotAGame.Agent Agent;
	// Use this for initialization
	void Start () {
      //  Agent.Connect("127.0.0.1" , 47536);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Result(bool success)
    {
        Debug.Log("result" + success);
    }
}
