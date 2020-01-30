using System;

namespace Regulus.Project.GameProject1.Data
{
    
    public class Energy
    {
        public enum TYPE
        {
            HEALTH_DECREASE,
        }
    
        public TYPE Type;
    
        public float Value;
    }
}