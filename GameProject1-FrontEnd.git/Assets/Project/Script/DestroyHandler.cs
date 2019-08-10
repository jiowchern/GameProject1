using UnityEngine;
using System.Collections;

public class DestroyHandler : MonoBehaviour {

	public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
