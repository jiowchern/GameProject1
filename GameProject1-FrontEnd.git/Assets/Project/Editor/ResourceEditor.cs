using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Assets.Project.Editor;

using UnityEditor;

using UnityEngine;

public abstract class ResourceEditor<T , TKey> : EditorWindow
{
    public Expression<Func<T, TKey>> GetKeyExpression;
    private Dictionary<TKey, T> _ItemSet;

    private int _Begin;

    private int _End;

     public T SelectedItem;

    public string DefaultPath = "";

    

    protected ResourceEditor()
    {
        
        // GetKeyExpression = get_key;
        // SelectedItem = first;
        _ItemSet = new Dictionary<TKey, T>();
    }

    
    public void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Load"))
        {
            var path = EditorUtility.OpenFilePanel("select", "", "txt");
            var items = Regulus.Utility.Serialization.Read<T[]>(path);
            _ItemSet = new Dictionary<TKey, T>();
            foreach (var item in items)
            {
                var key = _GetKey(item);
                _ItemSet.Add(key , item);
            }
            
        }
        if (GUILayout.Button("Save"))
        {
            var path = EditorUtility.SaveFilePanel("select", "", DefaultPath , "txt");
            Regulus.Utility.Serialization.Write(_ItemSet.Values.ToArray(), path);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _Begin = EditorGUILayout.IntField("Begin", _Begin);
        _End = EditorGUILayout.IntField("End", _End);
        EditorGUILayout.EndHorizontal();

        var set = _ItemSet.Values.Skip(_Begin).Take(_End - _Begin).ToArray();
        var length = set.Length;
        var fieldLength = 5;
        var lineLength = length / fieldLength;
        EditorGUILayout.BeginVertical();
        for (int i = 0; i <= lineLength; i++)
        {
            int index = i * fieldLength;
            int end = index + fieldLength;
            EditorGUILayout.BeginHorizontal();
            for (; index < end && index < length; index++)
            {
                var item = set[index];
                if (GUILayout.Button(_GetKeyString(item)))
                {                    
                    SelectedItem = item;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        

        EditorGUILayout.BeginHorizontal();
                
        
        if (GUILayout.Button("New"))
        {

            SelectedItem = _Create();
            _ItemSet.Add(_GetKey(SelectedItem) , SelectedItem);
        }
        
        
        
        
        if (GUILayout.Button("Remove"))
        {
            var key = _GetKey(SelectedItem);
            _ItemSet.Remove(key);
        }

        EditorGUILayout.EndHorizontal();
        _DrawDetail(ref SelectedItem);
        EditorGUILayout.EndVertical();
    }

    protected abstract T _Create();    

    protected abstract string _GetKeyString(T item);
    
    private TKey _GetKey(T item)
    {
        return GetKeyExpression.Compile().Invoke(item);
    }

    protected abstract void _DrawDetail(ref T selected_item);

    


     
    
}

