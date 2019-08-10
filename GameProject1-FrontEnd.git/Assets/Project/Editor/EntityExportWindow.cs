using System;

using UnityEngine;
using System.Linq;

using System.IO;
using System.Xml.Serialization;

using Regulus.Project.GameProject1.Data;
using Regulus.Utility;
using UnityEditor;

public class EntityExportWindow : EditorWindow
{

    [MenuItem("Regulus/GameProject1/ExportEntity")]
    static public void ExportFromScene()
    {
        var marks = GameObject.FindObjectsOfType<EntityExportMark>();        
        var entitys = (from mark in marks select mark.BuildEntity()).ToArray();

        var path = EditorUtility.SaveFilePanel("select", "", "entitys.txt", "txt");
        Serialization.Write(entitys , path);
    }

    [MenuItem("Regulus/GameProject1/ExportEntityGroupLayout")]
    static public void ExportEntityGroupLayout()
    {
        var marks = GameObject.FindObjectsOfType<EntityGroupLayoutMark>();
        var egls = new EntityGroupLayout[marks.Length];
        for(int i = 0; i < egls.Length ; ++i)
        {
            egls[i] = new EntityGroupLayout
            {
                Id = marks[i].Id,
                Entitys = marks[i].GetMarks().ToArray(),
                //Entitys = marks[i].GetLayouts<EntityLayoutMark, EntityLayout>().ToArray(),
                Protals = marks[i].GetLayouts<ProtalLayoutMark, ProtalLayout>().ToArray(),
                Chests = marks[i].GetLayouts<ChestLayoutMark, ChestLayout>().ToArray(),
                Statics = marks[i].GetLayouts<StaticLayoutMark, StaticLayout>().ToArray(),
                Walls = marks[i].GetLayouts<WallsLayoutMark, WallLayout>().ToArray(),
                Resources = marks[i].GetLayouts<ResourceLayoutMark, ResourceLayout>().ToArray(),
                Enterances = marks[i].GetLayouts<EnteranceLayoutMark, EnteranceLayout>().ToArray(),
                Strongholds = marks[i].GetLayouts<StrongholdLayoutMark, StrongholdLayout>().ToArray(),
                Fields = marks[i].GetLayouts<FieldLayoutMark, FieldLayout>().ToArray()
            };


        }

        var path = EditorUtility.SaveFilePanel("select", "", "entitygrouplayout.txt", "txt");
        if(path.Length > 0)
            Serialization.Write(egls, path);

    }
    [MenuItem("Regulus/GameProject1/ExportSkill")]
    static public void ExportSkillFromScene()
    {
        var marks = GameObject.FindObjectsOfType<SkillExportMark>();
        var entitys = (from mark in marks select mark.Data).ToArray();

        

        try
        {
            


            var path = EditorUtility.SaveFilePanel("select", "", "skills.txt", "txt");
            Serialization.Write(entitys, path);
        }
        catch (InvalidCastException e)
        {
            Debug.Log(e.InnerException.Message.ToString());
            throw;
        }
        
    }
    

}
