using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

public class ResourceOwner : MonoBehaviour {

    public TextAsset EntitySource;
    public TextAsset SkillSource;
    public TextAsset ItemSource;
    public TextAsset ItemFormulaSource;

    public TextAsset[] EntityGroupLayoutSources;

    public bool LoadOnStart;
    // Use this for initialization
    void Start () {
        if (LoadOnStart)
        {
            Load();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Load()
    {
        var formula = new ItemFormula();
        Debug.Log(formula.GetHashCode());
        Regulus.Project.GameProject1.Data.Resource.Instance.Entitys = _ReadEntitys();
        Regulus.Project.GameProject1.Data.Resource.Instance.SkillDatas = _ReadSkills();
        Regulus.Project.GameProject1.Data.Resource.Instance.Items = _ReadItems();
        Regulus.Project.GameProject1.Data.Resource.Instance.Formulas = _ReadItemFormulas();

        Regulus.Project.GameProject1.Data.Resource.Instance.EntityGroupLayouts = _ReadEntityGroupLayouts();
    }

    private EntityGroupLayout[] _ReadEntityGroupLayouts()
    {
        var groups = new List<EntityGroupLayout>();
        foreach (var entityGroupLayoutSource in EntityGroupLayoutSources)
        {
            groups.AddRange(Regulus.Utility.Serialization.Read<EntityGroupLayout[]>(entityGroupLayoutSource.bytes));
        }
        return groups.ToArray();
    }

    private ItemFormula[] _ReadItemFormulas()
    {
        return Regulus.Utility.Serialization.Read<ItemFormula[]>(ItemFormulaSource.bytes);
    }

    private ItemPrototype[] _ReadItems()
    {
        return Regulus.Utility.Serialization.Read<ItemPrototype[]>(ItemSource.bytes);
    }

    private SkillData[] _ReadSkills()
    {
        return Regulus.Utility.Serialization.Read<SkillData[]>(SkillSource.bytes);
    }

    private EntityData[] _ReadEntitys()
    {
        return Regulus.Utility.Serialization.Read<EntityData[]>(EntitySource.bytes);
    }
}
