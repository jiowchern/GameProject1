using System;



using Regulus.CustomType;

namespace Regulus.Project.GameProject1.Data
{
    
    [Serializable]
    public struct EntityData
    {
        
        public ENTITY Name;
        
        public Polygon Mesh;

        public bool CollisionRotation;


        public static bool IsActor(ENTITY e)
        {
            return e == ENTITY.ACTOR1 || e == ENTITY.ACTOR2 || e == ENTITY.ACTOR3 || e == ENTITY.ACTOR4 || e == ENTITY.ACTOR5;
        }

        public static bool IsWall(ENTITY e)
        {
            return e == Data.ENTITY.WALL_EAST || e == Data.ENTITY.WALL_EAST_AISLE 
                || e == Data.ENTITY.WALL_NORTH|| e == Data.ENTITY.WALL_NORTH_AISLE
                || e == Data.ENTITY.WALL_SOUTH|| e == Data.ENTITY.WALL_SOUTH_AISLE
                || e == Data.ENTITY.WALL_WESTERN|| e == Data.ENTITY.WALL_WESTERN_AISLE
                || e == Data.ENTITY.WALL_GATE || e == Data.ENTITY.CHEST_GATE || e == Data.ENTITY.STATIC;
        }

        public static bool IsResource(ENTITY entity_type)
        {            
            return entity_type == ENTITY.POOL;
        }
    }
}