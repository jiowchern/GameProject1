using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Regulus.Project.GameProject1.Data;


public class MapPlayer : MonoBehaviour
{

    struct Entity
    {
        public IVisible Visible;

        public MapEntity Avatar;
    }
    public Map Map;

    private Client _Client;

    private readonly Dictionary<System.Guid, Entity> _Walls;
    private readonly Dictionary<System.Guid, Entity> _Actors;

    public MapPlayer()
    {
        _Actors = new Dictionary<Guid, Entity>();
        _Walls = new Dictionary<System.Guid, Entity>();
    }
    // Use this for initialization
    void Start ()
	{
	    _Client = Client.Instance;
	    if (_Client != null)
	    {
	        _Client.User.VisibleProvider.Supply += _Add;
            _Client.User.VisibleProvider.Unsupply += _Remove;
        }
	}

    void OnDestroy()
    {
        if (_Client != null)
        {
            _Client.User.VisibleProvider.Supply -= _Add;
            _Client.User.VisibleProvider.Unsupply -= _Remove;
        }
    }

    private void _Remove(IVisible visible)
    {
        Entity actor;
        if (_Actors.TryGetValue(visible.Id , out actor))
        {
            GameObject.Destroy(actor.Avatar.gameObject);
            _Actors.Remove(visible.Id);
        }
    }

    private void _Add(IVisible visible)
    {
        if (_Walls.ContainsKey(visible.Id) == false)
        {
            
            if (EntityData.IsWall(visible.EntityType))
            {
                var obj = this.Map.Add(visible);
                _Walls.Add(visible.Id, new Entity() { Visible = visible, Avatar = obj });
            }

            if (EntityData.IsActor(visible.EntityType))
            {
                var obj = this.Map.Add(visible);
                _Actors.Add(visible.Id, new Entity() { Visible = visible, Avatar = obj });
            }
        }        
    }

    // Update is called once per frame
	void Update ()
	{
	    Entity mainPlayer;
	    if (_Actors.TryGetValue(EntityController.MainEntityId, out mainPlayer))
	    {
	        this.Map.SetPosision(mainPlayer.Visible.Position);
	    }

	    foreach (var actor in _Actors)
	    {
	        var avatar = actor.Value.Avatar;
	        var visible = actor.Value.Visible;
            
            avatar.UpdatePosition(new Vector2(visible.Position.X , visible.Position.Y));
	        avatar.UpdateDirection(visible.Direction);
	    }
	}
}
