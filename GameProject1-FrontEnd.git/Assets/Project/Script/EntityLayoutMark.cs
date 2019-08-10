using System;

using UnityEngine;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

using RegulusVector2 = Regulus.CustomType.Vector2;
public class EntityLayoutMark : MonoBehaviour, IMarkToLayout<EntityLayout>
{
    
    public Regulus.Project.GameProject1.Data.ENTITY Name;


    public string Id;


    public EntityLayoutMark()
    {
        Id = Guid.NewGuid().ToString();
    }

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public RegulusVector2 GetPosition(UnityEngine.Vector3 parent)
    {
        var position = gameObject.transform.position - parent;
        return new RegulusVector2(position.x, position.z);
    }

    public float GetDirection(float parent)
    {
        return gameObject.transform.rotation.eulerAngles.y - parent;
    }

    public Guid GetId()
    {
        return new Guid(Id);
    }

    IEnumerable<EntityLayout> IMarkToLayout<EntityLayout>.ToLayouts()
    {
        var el = new EntityLayout
        {
            Id = new Guid(Id),
            Type = Name,
            Position = GetPosition(gameObject.transform.position),
            Direction = GetDirection(gameObject.transform.rotation.eulerAngles.y)
        };
        yield return el;
    }
}
