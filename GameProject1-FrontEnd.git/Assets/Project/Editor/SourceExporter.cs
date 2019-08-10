using UnityEngine;
using System.Collections;
using System.Linq;

using UnityEditor;

public class SourceExporter : MonoBehaviour
{
    [MenuItem("Regulus/GameProject1/ItemExport")]
    static public void ItemExport()
    {        
        var path = EditorUtility.SaveFilePanel("select", "", "items.txt", "txt");

        var itemSources = FindObjectsOfType<ItemSource>();

        var itemPropertys = (from i in itemSources select i.Item);

        Regulus.Utility.Serialization.Write(itemPropertys.ToArray(), path);

        foreach (var itemSource in itemSources)
        {
            PrefabUtility.CreatePrefab(ItemSource.GetModelPath(itemSource.Item), itemSource.Model);
        }
        
    }

}
