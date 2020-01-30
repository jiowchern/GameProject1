using System;

using Regulus.CustomType;
using Regulus.Project.GameProject1.Data;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class EntityProvider 
    {
                

        public static Entity Create(ENTITY type)
        {
            var data = Singleton<Resource>.Instance.FindEntity(type);
            return new Entity(data);
        }
        

        public static Entity Create(ENTITY entity_type, Vector2 position, float direction)
        {
            var entity = Create(entity_type);
            IIndividual individual = entity;
            individual.SetPosition(position);
            individual.AddDirection(direction);
            return entity;
        }
    }
}