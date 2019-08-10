using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Regulus.CustomType;
using Regulus.Extension;
using Regulus.Project.GameProject1.Data;

public class StaticLayoutMark : MonoBehaviour , IMarkToLayout<StaticLayout>
{

    public EntityLayoutMark Static;

    public Vector3[] Polygon;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerable<StaticLayout> IMarkToLayout<StaticLayout>.ToLayouts()
    {
        
        var body = new Polygon((from v3 in Polygon select new Regulus.CustomType.Vector2(v3.x, v3.z)).FindHull().ToArray());
        yield return new StaticLayout()
        {
            Owner = Static.GetId(), Body = body

        };
    }
}
