using System;

using UnityEngine;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

using UnityEngine.UI;

public class SkillSelector : MonoBehaviour
{
    private readonly List<GameObject> _Childs;
    public RMF_RadialMenu Menu;

    public GameObject SkillButtomSource;

    private Client _Client;

    private ICastSkill _CastSkill;

    public SkillSelector()
    {
        _Childs = new List<GameObject>();
    }
    void OnDisable()
    {
        if (_Client != null)
        {
            _Client.User.BattleCastControllerProvider.Supply -= BattleCastControllerProviderOnSupply;
            _Client.User.BattleCastControllerProvider.Unsupply -= BattleCastControllerProviderOnUnsupply; 
        }
    }

    

    void OnEnable()
    {
        _Client = Client.Instance;

        if (_Client != null)
        {
            _Client.User.BattleCastControllerProvider.Supply += BattleCastControllerProviderOnSupply;
            _Client.User.BattleCastControllerProvider.Unsupply += BattleCastControllerProviderOnUnsupply;
        }
    }

    private void BattleCastControllerProviderOnSupply(ICastSkill cast_skill)
    {
        Debug.Log("CastSkill id : " + cast_skill.Id);
        _CastSkill = cast_skill;
        _CastSkill.HitNextsEvent += _AddHitSkills;

        foreach (var child in _Childs)
        {
            Destroy(child);
        }
        Menu.elements.Clear();
        foreach (var actorStatusType in _CastSkill.Skills)
        {
            _AddSkill(actorStatusType);
        }
        Menu.arrangeElements();
    }

    private void _AddHitSkills(ACTOR_STATUS_TYPE[] skills)
    {
        foreach (var actorStatusType in skills)
        {
            _AddSkill(actorStatusType);
        }
        Menu.arrangeElements();
    }

    private void _AddSkill(ACTOR_STATUS_TYPE actorStatusType)
    {
        //Resources.Load("UI/Skill/" + actorStatusType.ToString())
        var obj = GameObject.Instantiate(SkillButtomSource);
        var image = obj.GetComponentInChildren<UnityEngine.UI.Image>();
        image.sprite = (Sprite)Resources.Load("Icon/Skill/" + actorStatusType.ToString(), typeof (Sprite));
        var button = obj.GetComponentInChildren<UnityEngine.UI.Button>();
        var tmp = actorStatusType;
        button.onClick.AddListener(
            () => { _Cast(tmp); });
        _Childs.Add(obj);
        var rect = obj.GetComponent<RectTransform>();
        rect.SetParent(GetComponent<RectTransform>(), false);

        var element = obj.GetComponentInChildren<RMF_RadialMenuElement>();
        Menu.elements.Add(element);
    }

    private void _Cast(ACTOR_STATUS_TYPE actor_status_type)
    {
        if(_CastSkill != null)
            _CastSkill.Cast(actor_status_type);
    }

    private void BattleCastControllerProviderOnUnsupply(ICastSkill cast_skill)
    {
        _CastSkill.HitNextsEvent -= _AddHitSkills;
        _CastSkill = null;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
