using UnityEngine;
using System.Collections;

public class ResetLongIdle : MonoBehaviour
{

    public Entity Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Reset()
    {
        Target.ResetLongIdle();
    }
     
}
