using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Regulus.Project.GameProject1.Data;

public class WallsLayoutMark : MonoBehaviour , IMarkToLayout<WallLayout>
{

    public EntityLayoutMark[] Walls;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    

    IEnumerable<WallLayout> IMarkToLayout<WallLayout>.ToLayouts()
    {
        return Walls.Select(wall => new WallLayout()
        {
            Owner = wall.GetId()
        });
    }
}
