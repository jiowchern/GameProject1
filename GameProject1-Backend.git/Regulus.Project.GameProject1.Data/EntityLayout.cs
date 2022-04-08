using System;

using Regulus.Utility;

namespace Regulus.Project.GameProject1.Data
{
    

    [Serializable]
    public class EntityLayout
    {
        public Guid Id;
        public ENTITY Type;
        public Vector2 Position;
        public float Direction;
    }

    


}