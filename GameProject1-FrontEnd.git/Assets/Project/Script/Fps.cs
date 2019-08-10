using UnityEngine;
using System.Collections;

using Regulus.Utility;

public class Fps : MonoBehaviour
{

    public UnityEngine.UI.Text Text;

    private readonly Regulus.Utility.FPSCounter _Fps;

    private int _Last;

    public Fps()
    {
        _Last = 0;
        _Fps = new FPSCounter();
    }
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        _Fps.Update();
	    var val = _Fps.Value;
	    if (_Last != val)
	    {
	        _Last = val;
            Text.text = val.ToString();
        }	    
	}
}
