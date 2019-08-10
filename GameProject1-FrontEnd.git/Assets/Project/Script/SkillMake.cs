using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


using Regulus.Collection;
using Regulus.Project.GameProject1.Data;

public class SkillMake : MonoBehaviour {

    public GameObject MakeObject;
    public GameObject ItemFormulaViewObject;

    public GameObject ItemFormulaSource;

    private Client _Client;

    private IMakeSkill _MakeSkill;

    private readonly Regulus.Collection.DifferenceNoticer<ItemFormulaLite> _ItemFormulaLites;

    public SkillMake()
    {
        _ItemFormulaLites = new DifferenceNoticer<ItemFormulaLite>(new ItemFormulaLiteComparer());
        _ItemFormulaLites.JoinEvent += _AddFormula;
        _ItemFormulaLites.LeftEvent += _RemoveFormula;
    }

    

    void OnDestroy()
    {
        if (_Client != null)
        {
            _Client.User.MakeControllerProvider.Supply -= _MakeSupply;
            _Client.User.MakeControllerProvider.Unsupply -= _MakeUnsupply;
        }
    }
    

    // Use this for initialization
    void Start () {
        _Client = Client.Instance;

        if (_Client != null)
        {
            _Client.User.MakeControllerProvider.Unsupply += _MakeUnsupply;
            _Client.User.MakeControllerProvider.Supply += _MakeSupply;            
        }
    }

    private void _MakeSupply(IMakeSkill obj)
    {
        
        MakeObject.SetActive(true);
        _MakeSkill = obj;
        _MakeSkill.FormulasEvent += _AddFormulas;
        _MakeSkill.QueryFormula();
        
    }

    private void _AddFormulas(ItemFormulaLite[] obj)
    {
        //_RemoveFormulaAll();
        _ItemFormulaLites.Set(obj);
    }
    private void _MakeUnsupply(IMakeSkill obj)
    {
        MakeObject.SetActive(false);
        _MakeSkill.FormulasEvent -= _AddFormulas;
        _MakeSkill = null;
        
    }

    void _RemoveFormulaAll()
    {
        var formulas = ItemFormulaViewObject.GetComponentsInChildren<UIItemFormula>();
        foreach (var formula in formulas)
        {            
            GameObject.Destroy(formula.gameObject);
        }
    }
    private void _RemoveFormula(IEnumerable<ItemFormulaLite> instances)
    {
        var formulas = ItemFormulaViewObject.GetComponentsInChildren<UIItemFormula>();
        foreach(var formula in formulas)
        {
            if(instances.Any( i => i.Id == formula.Name.text))            
                GameObject.Destroy(formula.gameObject);
        }

    }
    private void _AddFormula(IEnumerable<ItemFormulaLite> instances)
    {
        foreach(var formulaLite in instances)
        {
            var slot = GameObject.Instantiate(ItemFormulaSource);

            var rectTransform = slot.GetComponent<RectTransform>();
            rectTransform.SetParent(ItemFormulaViewObject.transform);
            rectTransform.localScale = new Vector3(1, 1, 1);
            var ui = slot.GetComponent<UIItemFormula>();
            ui.Set(formulaLite);
            ui.MakeEvent += _Make;
        }
        
    }

    private void _Make(string arg1, int[] arg2)
    {
        _MakeSkill.Create(arg1 , arg2);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Exit()
    {
        if(_MakeSkill != null)
            _MakeSkill.Exit();
    }
}