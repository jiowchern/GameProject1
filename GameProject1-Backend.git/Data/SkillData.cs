using System;

using Regulus.CustomType;

namespace Regulus.Project.GameProject1.Data
{
    [Serializable]
    public class SkillData
    {
        public ACTOR_STATUS_TYPE Id;

        public Vector2[] Lefts;

        public Vector2[] Rights;

        public Translate[] Roots;

        public float Total;

        public float Begin;

        public float End;

        public ACTOR_STATUS_TYPE[] Nexts;

        public ACTOR_STATUS_TYPE[] HitNexts;

        public Effect[] Effects;

        public float StrengthCost ;
        

        public SkillData()
        {
            Lefts = new Vector2[0];
            Rights = new Vector2[0];
            Nexts = new ACTOR_STATUS_TYPE[0];
            Roots = new Translate[0];
            Effects = new Effect[0];
            HitNexts = new ACTOR_STATUS_TYPE[0];
        }

        
    }
}
