using System;

namespace Regulus.Project.GameProject1.Data
{
    public interface ICastSkill
    {
        ACTOR_STATUS_TYPE Id { get; }
        ACTOR_STATUS_TYPE[] Skills { get; }
        void Cast(ACTOR_STATUS_TYPE skill);

        event Action<ACTOR_STATUS_TYPE[]> HitNextsEvent;
    }
}