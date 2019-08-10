using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using NUnit.Framework;

using UnityEditor;
public class AvatarSuit : EditorWindow
{
    private Object _TargetObject;

    private Object _PartObject;

    private Object _BoneObject;

    [MenuItem("Regulus/GameProject1/AvatarSuit")]
    public static void Open()
    {
        var wnd = EditorWindow.GetWindow(typeof(AvatarSuit));
        wnd.Show();
    }



    public void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _TargetObject = EditorGUILayout.ObjectField("target", _TargetObject, typeof(SkinnedMeshRenderer), true);
        _PartObject = EditorGUILayout.ObjectField("source", _PartObject, typeof(SkinnedMeshRenderer) , true);
        _BoneObject = EditorGUILayout.ObjectField("bone", _BoneObject, typeof(StringHolder), true);



        if (GUILayout.Button("Change"))
        {
            var part = _PartObject as SkinnedMeshRenderer;

            var bones = _BoneObject as StringHolder;
            _Change(_TargetObject as SkinnedMeshRenderer, part , bones.Values);
        }

        EditorGUILayout.EndVertical();
    }

    private void _Change(SkinnedMeshRenderer target_object, SkinnedMeshRenderer part_object , string[] bone_names)
    {

        List<CombineInstance> combineInstances = new List<CombineInstance>();
        

        for (int i = 0; i < part_object.sharedMesh.subMeshCount ; i++)
        {
            var ci = new CombineInstance
            {
                mesh = part_object.sharedMesh,
                subMeshIndex = i
            };
            combineInstances.Add(ci);
        }
        var bones = new List<Transform>();

        var allBones = target_object.gameObject.transform.root.GetComponentsInChildren<Transform>();
        /*foreach (var bone in allBones)
        {
            if (bone_names.Any(name=> name == bone.name))
            {
                bones.Add(bone);
            }
        }*/

        foreach (var boneName in bone_names)
        {
            var bone = allBones.FirstOrDefault(b => b.name == boneName);
            if (bone != null)
            {
                bones.Add(bone);
            }
            else
            {
                Debug.LogWarning("not bone " + boneName);
            }
        }


        target_object.sharedMesh = new Mesh();
        target_object.sharedMesh.CombineMeshes(combineInstances.ToArray(), false, false);
        target_object.bones = bones.ToArray();
        target_object.sharedMaterials = part_object.sharedMaterials.ToArray();
    }
}
