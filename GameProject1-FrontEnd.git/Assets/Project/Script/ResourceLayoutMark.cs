using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

public class ResourceLayoutMark : MonoBehaviour ,IMarkToLayout<ResourceLayout>
{
    
    public EntityLayoutMark Entity;

    public ResourceItem[] Items;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerable<ResourceLayout> IMarkToLayout<ResourceLayout>.ToLayouts()
    {
        yield return new ResourceLayout()
        {
           Owner  = Entity.GetId(),Items = Items

        };
    }
}
