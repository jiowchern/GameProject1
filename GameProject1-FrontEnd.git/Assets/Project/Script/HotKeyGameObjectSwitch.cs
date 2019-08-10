using UnityEngine;
using System.Collections;

public class HotKeyGameObjectSwitch : MonoBehaviour {

    public GameObject Target;
    public KeyCode Key;

    public bool Default;
	// Use this for initialization
	void Start () {
        Target.SetActive(Default);
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyUp(Key))
	    {
	        Switch();
	    }
	}


    public void Switch()
    {
        Target.SetActive(!Target.activeSelf);
    }
}
