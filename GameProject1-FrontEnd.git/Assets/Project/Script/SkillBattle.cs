using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;

public class SkillBattle : MonoBehaviour {
    private Client _Client;

    public GameObject CastSkillObject;
    public GameObject DisarmObject;
    
    private IBattleSkill _Battle;

    private ICastSkill _CastSkill;

    void OnDestroy()
    {
        
        if (_Client != null)
        {
            
            _Client.User.BattleControllerProvider.Unsupply -= _UnsupplyBattle;
            _Client.User.BattleControllerProvider.Supply -= _SupplyBattle;
            _Client.User.BattleCastControllerProvider.Unsupply -= _UnsupplyCastBattle;
            _Client.User.BattleCastControllerProvider.Supply -= _SupplyCastBattle;
        }
    }

    

    // Use this for initialization
    void Start () {
        _Client = Client.Instance;
        if (_Client != null)
        {
            _Client.User.BattleCastControllerProvider.Unsupply += _UnsupplyCastBattle;
            _Client.User.BattleCastControllerProvider.Supply += _SupplyCastBattle;
            _Client.User.BattleControllerProvider.Supply += _SupplyBattle;
            _Client.User.BattleControllerProvider.Unsupply += _UnsupplyBattle;
        }
        _UnsupplyBattle(null);
        _UnsupplyCastBattle(null);
    }
    private void _UnsupplyBattle(IBattleSkill obj)
    {
        _Battle = null;
        
        DisarmObject.SetActive(false);
    }

    private void _SupplyBattle(IBattleSkill obj)
    {
        _Battle = obj;
        DisarmObject.SetActive(true);        
    }

    private void _SupplyCastBattle(ICastSkill obj)
    {
        _CastSkill = obj;
        CastSkillObject.SetActive(true);
        
    }

    private void _UnsupplyCastBattle(ICastSkill obj)
    {
        _CastSkill = null;
        CastSkillObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update () {
        if(_Battle != null)
        {
            if(Input.GetKeyUp(KeyCode.R))
            {
                _Battle.Disarm();
            }
        }
    }

    public void Disarm()
    {
        if (_Battle != null)
        {
            _Battle.Disarm();
        }
    }

    /*public void CastBlock()
    {
        if (_CastSkill != null)
        {
            _CastSkill.Cast(ACTOR_STATUS_TYPE.BATTLE_AXE_BLOCK);
        }
    }
    public void CastAttack1()
    {
        if (_CastSkill != null)
        {
            _CastSkill.Cast(ACTOR_STATUS_TYPE.BATTLE_AXE_ATTACK2);
        }
    }
    public void CastAttack2()
    {
        if (_CastSkill != null)
        {
            _CastSkill.Cast(ACTOR_STATUS_TYPE.BATTLE_AXE_ATTACK1);
        }
    }*/
}
