using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Regulus.Project.GameProject1.Data;

using UnityEditor;

using UnityEngine;

namespace Assets.Project.Editor
{
    public class ItemEditor : EditorWindow
    {
        private ItemPrototypeSet _ItemSet;
                

        private int _Begin = 0;

        private int _End = 20;

        private string _ItemName;
        

        private string _Description;

        private EQUIP_PART _EquipPart;

        [MenuItem("Regulus/GameProject1/ItemEditor")]
        public static void Open()
        {
            var wnd = EditorWindow.GetWindow(typeof(ItemEditor));
            wnd.Show();
        }

        public ItemEditor()
        {
            _ItemSet = new ItemPrototypeSet();
        }
        public void OnGUI()
        {

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
                        
            if (GUILayout.Button("Load"))
            {
                var path = EditorUtility.OpenFilePanel("select", "", "txt");
                _ItemSet = new ItemPrototypeSet(Regulus.Utility.Serialization.Read<ItemPrototype[]>(path));
            }
            if (GUILayout.Button("Save"))
            {
                var path = EditorUtility.SaveFilePanel("select", "", "items.txt", "txt");
                Regulus.Utility.Serialization.Write(_ItemSet.GetItems().ToArray() , path);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _Begin = EditorGUILayout.IntField("Begin", _Begin);
            _End = EditorGUILayout.IntField("End", _End);            
            EditorGUILayout.EndHorizontal();
            
            var set = _ItemSet.GetItems().Skip(_Begin).Take(_End - _Begin).ToArray();
            var length = set.Length;
            var fieldLength = 5;
            var lineLength = length  / fieldLength ;
            EditorGUILayout.BeginVertical();
            for (int i = 0; i <= lineLength ; i++)
            {
                int index = i * fieldLength;
                int end = index + fieldLength ;
                EditorGUILayout.BeginHorizontal();
                for (; index < end && index < length; index++)
                {
                    EditorGUILayout.SelectableLabel(set[index].Id);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();



            EditorGUILayout.BeginHorizontal();
            
            _ItemName = EditorGUILayout.TextField("Name", _ItemName);


            _EquipPart = (EQUIP_PART)EditorGUILayout.EnumPopup("EquipPart", _EquipPart);
            
            _Description = EditorGUILayout.TextField("Description", _Description);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set/Add"))
            {
                
                _ItemSet.Add(new ItemPrototype()
                {
                    Id = _ItemName,
                    
                    EquipPart = _EquipPart

                } );
            }
            if (GUILayout.Button("Load"))
            {
                var item = _ItemSet.Find(_ItemName);                
                _Description = item.Description;
                _EquipPart = item.EquipPart;
            }
            if (GUILayout.Button("Remove"))
            {
                _ItemSet.Remove(_ItemName);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

        }
    }
}
