using System;
using System.Linq;

using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;

public class PopupEnergyLauncher: MonoBehaviour
{
    public Transform Root;

    public EnergyPrefab[] Prefabs;
    [Serializable]
    public struct EnergyPrefab
    {
        public Energy.TYPE Type;

        public GameObject Prefab;
    }
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void Launch(Transform transform1, Energy energy)
    {
        var instance = GameObject.FindObjectOfType<PopupEnergyLauncher>();
        if (instance != null)
        {
            instance._Launch(transform1 , energy);
        }

    }

    private void _Launch(Transform transform1, Energy energy)
    {
        var screen = Camera.main.WorldToScreenPoint(transform1.position);
        GameObject prefab = _GetPrefab(energy.Type);
        var popup = GameObject.Instantiate(prefab);
        var rect = popup.GetComponent<RectTransform>();
        rect.position = screen;
        rect.SetParent(Root);
        
        var text = popup.GetComponentInChildren<UnityEngine.UI.Text>();
        text.text = energy.Value.ToString();
    }

    private GameObject _GetPrefab(Energy.TYPE type)
    {
        return Prefabs.First(p => p.Type == type).Prefab;
    }
}
