using UnityEngine;
using System.Collections;

public class DeterminationExportMark : MonoBehaviour
{

    public enum PART
    {
        LEFT,
        RIGHT,
        ROOT
    }

    public PART Part;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector2 Position { get
        {
            return new Vector2(transform.position.x,  transform.position.z);
        } }

    public bool Enable
    {
        get { return gameObject.activeInHierarchy; }
    }
}
