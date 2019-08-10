using UnityEngine;
using System.Collections;

public class MMController : MonoBehaviour {


    protected string CurAnim = "mm_idle";

	void Start () {
        GetComponent<Animation>()["mm_idle"].wrapMode = WrapMode.Loop;
        GetComponent<Animation>()["face_talk"].wrapMode = WrapMode.Loop;
	}
	
	void Update () 
    {
        GetComponent<Animation>().CrossFade(CurAnim, 0.3f, PlayMode.StopAll);
        GetComponent<Animation>().Blend("face_talk");


        if (Input.GetMouseButton(1))
        {
            float curRotY = transform.rotation.eulerAngles.y;
            float nextRotY = curRotY + Input.GetAxis("Mouse X") * 300.0f * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, nextRotY, 0);
        }

	}
}
