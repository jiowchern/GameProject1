using System;

using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;

public class AvatarEquipment : MonoBehaviour
{

    public EQUIP_PART Part;

    public bool Battle;

    private IVisible _Visible;

    private GameObject _EquipAvatar;

    public void SetId(IVisible visible )
    {
        _Visible = visible;
        _Visible.EquipEvent += _UpdateAvatar;

    }
    void OnDestroy()
    {
        if (_Visible != null)
        {
            _Visible.EquipEvent -= _UpdateAvatar;
        }

        
    }
    // Use this for initialization
	void Start ()
    {
        
    }
    

    private void _UpdateAvatar(EquipStatus[] equip_statuses)
    {
        bool found =false;
        foreach (var statuse in equip_statuses)
        {
            if(statuse.Part != Part)
                continue;

            if ((!_IsBattle() || !Battle) && (_IsBattle() != false || Battle != false))
            {
                continue;
            }
            
            var equipAvater = (GameObject)GameObject.Instantiate(Resources.Load(ItemSource.GetResourcePath(statuse.Item), typeof(GameObject)));
            
            equipAvater.transform.SetParent(gameObject.transform);
            equipAvater.transform.localPosition = Vector3.zero;            
            equipAvater.transform.localRotation = Quaternion.identity;

            if (_EquipAvatar != null)
            {
                Destroy(_EquipAvatar);
            }
            _EquipAvatar = equipAvater;
            found = true;
            break;
        }

        if (found == false && _EquipAvatar != null)
        {
            Destroy(_EquipAvatar);
        }
    }

    private bool _IsBattle()
    {
        return true;
    }

    // Update is called once per frame
	void Update ()
    {
	
	}
}
