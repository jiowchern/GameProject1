using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Regulus.Project.GameProject1.Data;

public class EntityOrigin : MonoBehaviour
{
    public GameObject[] Prefabs;
    private Dictionary<ENTITY, GameObject> _EntitySources;
    private Client _Client;

    void OnDestroy()
    {
        if (_Client != null)
        {
            _Client.User.VisibleProvider.Supply -= _CreateEntity;
            _Client.User.VisibleProvider.Unsupply -= _DestroyEntity;
        }
    }
    // Use this for initialization
	void Start ()
	{
        _EntitySources = new Dictionary<ENTITY, GameObject>();
        foreach (var data in  (from prefab in Prefabs
	        let mark = prefab.GetComponent<EntityExportMark>()
	        where mark != null
	        select new {Name = mark.Name, Prefab = prefab}))
	    {
            _EntitySources.Add(data.Name , data.Prefab );
        }

        
        StartCoroutine(_WaitSceneLoadDone());
	}

    private IEnumerator _WaitSceneLoadDone()
    {
        yield return new WaitWhile( ()=>gameObject.scene.isLoaded == false);
        _Client = Client.Instance;
        if (_Client != null)
        {
            _Client.User.VisibleProvider.Supply += _CreateEntity;
            _Client.User.VisibleProvider.Unsupply += _DestroyEntity;
        }
    }

    private void _DestroyEntity(IVisible obj)
    {
        
    }

    private void _CreateEntity(IVisible obj)
    {
        GameObject source;
        if (_EntitySources.TryGetValue(obj.EntityType, out source))
        {
            var entityObject = GameObject.Instantiate(source);
            var entity = entityObject.GetComponent<Entity>();
            entity.SetVisible(obj);
            
        }
    }

    // Update is called once per frame
	void Update () {
	
	}
}
