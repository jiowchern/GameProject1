using UnityEngine;
using System.Collections;

public class DisplaySwitch : MonoBehaviour {


    public KeyCode Key;
    public bool Show;
    public MonoBehaviour Target;
	// Use this for initialization
	void Start () 
    {
        Target.enabled = Show;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKeyUp(Key))
        {
            Show = !Show;
        }
        if (Target.enabled != Show)
            Target.enabled = Show;
	}

    public void Trigger()
    {
        Show = !Show;
    }
}
