using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;

public class ItemSource : MonoBehaviour
{

    const string _AssetsResourcePaths = "Assets/project/resources/Item/Model"; 

    [SerializeField]
    public ItemPrototype Item;

    public GameObject Model;

    public static string GetModelPath(ItemPrototype item)
    {
        return string.Format("{0}/{1}.prefab" , ItemSource._AssetsResourcePaths , item.Id);
    }

    public static string GetResourcePath(string item)
    {
        return string.Format("Item/Model/{0}", item);
    }
}
