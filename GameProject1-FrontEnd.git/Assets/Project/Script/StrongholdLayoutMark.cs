using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

public class StrongholdLayoutMark : MonoBehaviour, IMarkToLayout<StrongholdLayout>
{
    public EntityLayoutMark Entity;

    public ENTITY[] Kinds;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerable<StrongholdLayout> IMarkToLayout<StrongholdLayout>.ToLayouts()
    {
        yield return new StrongholdLayout()
        {
            Owner = Entity.GetId(),
            Kinds = Kinds
        };
    }
}
