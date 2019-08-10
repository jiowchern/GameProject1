using UnityEngine;
using System.Collections;

public class SkillButton : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
        
        var y = Screen.height / 4;
	    var rect = GetComponent<RectTransform>();
	    var pos = rect.localPosition;
	    pos.y = y;
	    rect.localPosition = pos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
