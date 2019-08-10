using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

public class FieldLayoutMark : MonoBehaviour ,IMarkToLayout<FieldLayout>
{
    public EntityLayoutMark Entity;

    public ENTITY[] Kinds;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerable<FieldLayout> IMarkToLayout<FieldLayout>.ToLayouts()
    {
        yield return new FieldLayout()
        {
            Owner = Entity.GetId(),
            Kinds = Kinds
        };
    }
}
