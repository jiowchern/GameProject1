using System;

namespace Regulus.Project.GameProject1.Data
{
    public interface ICastSkill
    {
        Regulus.Remote.Property<ACTOR_STATUS_TYPE> Id { get; }
        Regulus.Remote.Property<ACTOR_STATUS_TYPE[]> Skills { get; }
        void Cast(ACTOR_STATUS_TYPE skill);

        event Action<ACTOR_STATUS_TYPE[]> HitNextsEvent;
    }
}