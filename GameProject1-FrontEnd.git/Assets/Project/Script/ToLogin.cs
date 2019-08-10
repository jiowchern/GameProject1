using UnityEngine;
using System.Collections;

public class ToLogin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToMenu()
    {
        SceneChanger.Instance.ToLogin();
    }
}
