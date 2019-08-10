using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

public class EnteranceLayoutMark : MonoBehaviour , IMarkToLayout<EnteranceLayout>
{
    public EntityLayoutMark Entity;

    public ENTITY[] Kinds;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerable<EnteranceLayout> ToLayouts()
    {
        yield return new EnteranceLayout()
        {
            Owner = Entity.GetId(),
            Kinds = Kinds
        };

    }
}
