using UnityEngine;
using System.Collections;
using System.Globalization;

public class Ping : MonoBehaviour
{

    public UnityEngine.UI.Text Text;

    private Client _Client;

    private float _Last;
    // Use this for initialization
	void Start ()
	{
	    _Last = 0;
        _Client = Client.Instance;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (_Client != null)
	    {
	        var val = _Client.Ping;
	        if (val != _Last)
	        {
	            _Last = val;
                Text.text = val.ToString();
            }
            
        }
	    
	}
}
