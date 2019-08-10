using System;

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Regulus.Project.GameProject1.Data;

public class EntityGroupLayoutMark : MonoBehaviour 
{
    public string Id;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    

    public IEnumerable<TData> GetLayouts<TMark , TData>()
        where TMark : IMarkToLayout<TData>
    {
        var marks = gameObject.GetComponentsInChildren<TMark>();
        return marks.SelectMany(mark => mark.ToLayouts());
    }

    public IEnumerable<EntityLayout> GetMarks()
    {
        var marks = gameObject.GetComponentsInChildren<EntityLayoutMark>();
        foreach (var entityExportMark in marks)
        {
            var el = new EntityLayout();
            el.Id = entityExportMark.GetId();
            el.Type = entityExportMark.Name;
            el.Position = entityExportMark.GetPosition(gameObject.transform.position);
            el.Direction = entityExportMark.GetDirection(gameObject.transform.rotation.eulerAngles.y);
            yield return el;
        }
    }

}

