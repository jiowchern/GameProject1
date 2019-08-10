using UnityEngine;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

public class ChestLayoutMark : MonoBehaviour , IMarkToLayout<ChestLayout>
{
    
    public EntityLayoutMark Owner;

    public EntityLayoutMark Debirs;
    public EntityLayoutMark Gate;
    public EntityLayoutMark Exit;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    IEnumerable<ChestLayout> IMarkToLayout<ChestLayout>.ToLayouts()
    {
        yield return new ChestLayout()
        {
            Owner = Owner.GetId(),
            Debirs =Debirs.GetId(),
            Gate = Gate.GetId(),
            Exit = Exit.GetId(),

        };
    }
}