//  ============================================================
//  Name:       Jiggle Bone v.1.0
//  Author:     Michael Cook (Fishypants)
//  Date:       9-25-2011
//  License:    Free to use. Any credit would be nice :)
//
//  To Use:
//      Drag this script onto a bone. (ideally bones at the end)
//      Set the boneAxis to be the front facing axis of the bone.
//      Done! Now you have bones with jiggle dynamics.
//
//  ============================================================

//Fixed by shallway. 2012.3.21

using UnityEngine;
using System.Collections;

public class jiggleBone : MonoBehaviour {
    // Target and dynamic positions
    Vector3 targetPos = new Vector3();
    Vector3 dynamicPos = new Vector3();

    // Bone settings
    public Vector3 boneAxis = new Vector3(0,0,1);
    public float targetDistance = 5f;
	
    // Dynamics settings
    public float bStiffness = 0.6f;
    public float bMass = 1f;
    public float bDamping = 0.75f;
    public float bGravity = 0.75f;

    // Dynamics variables
    Vector3 force = new Vector3();
    Vector3 acc = new Vector3();
    Vector3 vel = new Vector3();

    // Squash and stretch variables
    public bool SquashAndStretch = false;
    public float StretchForce = 1f;
	
	public Transform FakeTrans;
	
	
    void Awake(){
        // Set targetPos and dynamicPos at startup
        targetPos = FakeTrans.position + FakeTrans.forward*targetDistance;
        dynamicPos = targetPos;
    }

    void LateUpdate(){
	
		// Calculate Watchtarget position
		targetPos = FakeTrans.position + FakeTrans.forward*targetDistance;
		
		
		Vector3 diff = targetPos - dynamicPos;
		
		//if (diff.magnitude < 0.1f)
			//transform.rotation = FakeTrans.rotation;
	
		force = diff * bStiffness;
		acc = force / bMass;
		vel += acc * (1 - bDamping);
		
        // Update dynamic postion
        dynamicPos += (vel*0.1f + force*0.1f);

        // Set bone rotation to look at dynamicPos
        transform.LookAt(dynamicPos,FakeTrans.up);

        if(SquashAndStretch){
            // Create a vector from Watchtarget position to dynamic position
            // We will measure the magnitude of the vector to determine
            // how much squash and stretch we will apply
            Vector3 dynamicVec = dynamicPos - targetPos;
            float stretchMag = dynamicVec.magnitude;
			
			Vector3 forwardScale = Vector3.zero;
			forwardScale.x = Mathf.Abs(transform.forward.x);
			forwardScale.y = Mathf.Abs(transform.forward.y);
			forwardScale.z = Mathf.Abs(transform.forward.z);
			Vector3 stretchScale = Vector3.one + forwardScale*stretchMag * StretchForce;
			
			Vector3 upScale = Vector3.zero;
			upScale.x = Mathf.Abs(transform.up.x);
			upScale.y = Mathf.Abs(transform.up.y);
			upScale.z = Mathf.Abs(transform.up.z);
			stretchScale -= upScale*stretchMag * StretchForce;
			
			Vector3 rightScale = Vector3.zero;
			rightScale.x = Mathf.Abs(transform.right.x);
			rightScale.y = Mathf.Abs(transform.right.y);
			rightScale.z = Mathf.Abs(transform.right.z);
			stretchScale -= rightScale*stretchMag * StretchForce;
			
			transform.localScale = stretchScale;
        }
    }
}
