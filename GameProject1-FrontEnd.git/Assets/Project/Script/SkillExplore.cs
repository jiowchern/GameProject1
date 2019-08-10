using System;

using UnityEngine;
using System.Collections;
using System.Linq;

using Regulus.Project.GameProject1.Data;

public class SkillExplore : MonoBehaviour {
    private Client _Client;

    public GameObject ExploreObject;
    

    public GameObject BattleObject;

    public GameObject MakeObject;


    private INormalSkill _SkillController;

    // Use this for initialization
	void Start () {
        _Disable();
	    _Client = Client.Instance;
        if(_Client != null)
        {
            _Client.User.NormalControllerProvider.Supply += _Supply;
            _Client.User.NormalControllerProvider.Unsupply += _Unsupply;
        }

        
    }

    private void _Disable()
    {
        ExploreObject.SetActive(false);
        BattleObject.SetActive(false);
        MakeObject.SetActive(false);
        _SkillController = null;

        
    }

    void OnDestroy()
    {
        if (_Client != null)
        {
            _Client.User.NormalControllerProvider.Supply -= _Supply;
            _Client.User.NormalControllerProvider.Unsupply -= _Unsupply;
        }
    }

    private void _Unsupply(INormalSkill obj)
    {
        _Disable();        
        
    }

    private void _Supply(INormalSkill obj)
    {
        _Enable(obj);
    }

    private void _Enable(INormalSkill skill_controller)
    {
        MakeObject.SetActive(true);
        ExploreObject.SetActive(true);
        BattleObject.SetActive(true);
        _SkillController = skill_controller;
        
    }

    // Update is called once per frame
	void Update ()
    {
	    if (_SkillController != null)
	    {
            if (Input.GetKeyUp(KeyCode.E))
            {
                Explore();
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                Battle();
            }
	        if (Input.GetKeyUp(KeyCode.M))
	        {
	            Make();
	        }
        }
	        
	}

    public void Make()
    {
        _SkillController.Make();
    }

    public void Battle()
    {
        var entity = _FindEntity();
        if (entity != null)
        {
            _SkillController.Battle();
        }
    }

    public void Explore()
    {
        var entity = _FindEntity();
        if (entity != null)
        {
            var id = entity.GetExploreTarget();
            if (id != Guid.Empty)
            {
                _SkillController.Explore(id);
            }
        }
    }

    private Entity _FindEntity()
    {
        if (_Client != null)
        {
            return  GameObject.FindObjectsOfType<Entity>().FirstOrDefault(e => e.Id == EntityController.MainEntityId);


        }
        return null;
    }
}
