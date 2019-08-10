using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


using UnityEditor;

public class UnityChanPartHandler : EditorWindow
{
    private Object _CharactorObject;

    
    [MenuItem("Regulus/GameProject1/UnityChanPart")]
    public static void Open()
    {
        var wnd = EditorWindow.GetWindow(typeof(UnityChanPartHandler));
        wnd.Show();
    }

    public UnityChanPartHandler()
    {        
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _CharactorObject = EditorGUILayout.ObjectField("unity chan", _CharactorObject, typeof (Transform), true);


        if (GUILayout.Button("Extract"))
        {
            _Extract(_CharactorObject as Transform);
        }
        


        EditorGUILayout.EndVertical();
    }

    private void _Extract(Transform root)
    {
        
        var skinnedMeshRenders = root.GetComponentsInChildren<SkinnedMeshRenderer>(true);        

        foreach (var skinnedMeshRenderer in skinnedMeshRenders)
        {
            PrefabUtility.CreatePrefab(
                string.Format("Assets/project/resources/avatar/parts/{0}_{1}.prefab", root.name , skinnedMeshRenderer.name) , skinnedMeshRenderer.gameObject );
        }
        _SaveBone(root.gameObject);

    }

    private void _SaveBone(GameObject game_object)
    {
        

        foreach (var componentsInChild in game_object.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            var holder = ScriptableObject.CreateInstance<StringHolder>();
            holder.Values = (from t in componentsInChild.bones select t.name).ToArray();

            AssetDatabase.CreateAsset(holder, string.Format("Assets/project/resources/avatar/parts/{0}_{1}_bone.asset" , game_object.name , componentsInChild.name));
        }
        
    }
}
