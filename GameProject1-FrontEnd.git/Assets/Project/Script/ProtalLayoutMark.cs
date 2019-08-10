using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

public class ProtalLayoutMark : MonoBehaviour , IMarkToLayout<ProtalLayout>
{
    public EntityLayoutMark Owner;

    public ENTITY[] Pass;

    public string Realm;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerable<ProtalLayout> IMarkToLayout<ProtalLayout>.ToLayouts()
    {
        yield return new ProtalLayout
        {
            Owner = Owner.GetId(),
            TargetRealm = Realm,
            PassEntity = Pass
        };
    }
}
