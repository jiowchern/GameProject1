using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Regulus.Project.GameProject1.Data;


using UnityEditor;

using UnityEngine;


public class ItemFormulaEditor1 : ResourceEditor<ItemFormula , string>
{
    [MenuItem("Regulus/GameProject1/ItemFormulaEditor")]
    public static void Open()
    {
        var wnd = EditorWindow.GetWindow(typeof(ItemFormulaEditor1));
        wnd.Show();
    }
    

    private int _ItemEffectCount;

    private float _Quality;

    private int[] _TestCount;

    public ItemFormulaEditor1() 
    {
        //new ItemFormula() { Id = "default"} , formula => formula.Id 
        _TestCount = new int[0];
        DefaultPath = "ItemFormula.txt";
        SelectedItem = new ItemFormula()
        {
            Id = "default"
        };
        GetKeyExpression = formula => formula.Id;
    }

    protected override ItemFormula _Create()
    {
        return new ItemFormula() { Id = Guid.NewGuid().ToString()};
    }

    protected override string _GetKeyString(ItemFormula item)
    {
        return item.Id;
    }

    protected override void _DrawDetail(ref ItemFormula key)
    {
        key.Id = EditorGUILayout.TextField("Formula", key.Id);
        key.Item = EditorGUILayout.TextField("Item", key.Item);
        key.NeedLimit = EditorGUILayout.IntField("NeedLimit", key.NeedLimit);
        _DrawNeeds(key);

        _DrawEffects(key);

        _DrawSpreadsheet(key);
    }

    private void _DrawSpreadsheet(ItemFormula key)
    {
        EditorGUILayout.BeginHorizontal();        
        for (int i = 0; i < key.NeedItems.Length && i < _TestCount.Length; i++)
        {
            _TestCount[i] = EditorGUILayout.IntField(_TestCount[i]);
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Test"))
        {
            var items = key.NeedItems;
            
            var total1 = items.Sum(i => i.Max);
            var itemScales1 = (from i in items
                             select new
                             {
                                 Item = i,
                                 Value = i.Max,
                                 Scale = i.Max / (float)total1
                             }).ToArray();

            var total2 = _TestCount.Sum();
            var itemScales2 = (from i in _TestCount
                               select new
                              {
                                  Value = i,
                                  Scale = i / (float)total2
                              }).ToArray();

            var maxScale = 0.0f; 
            for (int i = 0; i < itemScales2.Length && i < itemScales1.Length; i++)
            {
                var scale1 = itemScales1[i].Scale;
                var scale2 = itemScales2[i].Scale;
                var ms = scale2 / scale1;
                if (ms > maxScale)
                    maxScale = ms;
            }

            Debug.Log("Max Scale" + maxScale);
            _Quality = 0.0f;
            for (int i = 0; i < itemScales2.Length && i < itemScales1.Length; i++)
            {
                _Quality += itemScales1[i].Scale * (itemScales2[i].Scale / itemScales1[i].Scale / maxScale);
            }

        }

        EditorGUILayout.FloatField("Quality", _Quality);
    }

    private void _DrawEffects(ItemFormula key)
    {
        EditorGUILayout.BeginHorizontal();
        _ItemEffectCount = EditorGUILayout.IntField("ItemEffectCount", _ItemEffectCount);

        if (GUILayout.Button("ResetEffectCount"))
        {
            var effects = key.Effects;
            key.Effects = new ItemEffect[_ItemEffectCount];

            int i = 0;
            for (; i < key.Effects.Length && i < effects.Length; i++)
            {
                key.Effects[i] = effects[i];                
            }
            for (; i < key.Effects.Length; i++)
            {
                key.Effects[i] = new ItemEffect();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();

        for (int i = 0; i < key.Effects.Length; ++i)
        {
            EditorGUILayout.BeginHorizontal();

            key.Effects[i].Quality = EditorGUILayout.FloatField("Quality", key.Effects[i].Quality);

            _DrawEffects(ref key.Effects[i].Effects);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void _DrawEffects(ref Effect[] effects)
    {
        EditorGUILayout.BeginHorizontal();
        var len = EditorGUILayout.IntField("EffectCount", effects.Length);

        if (len != effects.Length)
        {
            var tmp = effects;
            effects = new Effect[len];

            for (int i = 0; i < effects.Length && i < tmp.Length; i++)
            {
                effects[i] = tmp[i];
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();

        for (int i = 0; i < effects.Length; ++i)
        {
            EditorGUILayout.BeginHorizontal();

            effects[i].Type = (EFFECT_TYPE)EditorGUILayout.EnumPopup("Type", effects[i].Type);
            effects[i].Value = EditorGUILayout.FloatField("Value", effects[i].Value);

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void _DrawNeeds(ItemFormula key)
    {
        

        EditorGUILayout.BeginHorizontal();
        var needCount = key.NeedItems.Length;
        needCount = EditorGUILayout.IntField("NeedCount", needCount);
        if (key.NeedItems.Length != needCount)
        {
            var needs = key.NeedItems;
            key.NeedItems = new ItemFormulaNeed[needCount];

            _TestCount = new int[needCount];

            for (int i = 0; i < key.NeedItems.Length && i < needs.Length; i++)
            {
                key.NeedItems[i].Item = needs[i].Item;
                key.NeedItems[i].Min = needs[i].Min;
                key.NeedItems[i].Max = needs[i].Max;
            }
        }

        
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();

        for (int i = 0; i < key.NeedItems.Length; ++i)
        {
            EditorGUILayout.BeginHorizontal();

            key.NeedItems[i].Item = EditorGUILayout.TextField("Item", key.NeedItems[i].Item);
            key.NeedItems[i].Max = EditorGUILayout.IntField("Max", key.NeedItems[i].Max);
            key.NeedItems[i].Min = EditorGUILayout.IntField("Min", key.NeedItems[i].Min);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    
}

