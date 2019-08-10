using UnityEngine;
using System.Collections;

public class DontRemoveObjects : MonoBehaviour 
{
	public Object[] Targets;

	void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);

		foreach (var tar in Targets)
		{
			GameObject.DontDestroyOnLoad(tar);
		}
	}
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
