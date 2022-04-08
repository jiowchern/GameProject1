using System;

using Regulus.Utility;

namespace Regulus.Project.GameProject1.Data
{
    public interface IVisible
    {
        Regulus.Remote.Property<ENTITY> EntityType { get; }
        Regulus.Remote.Property<Guid> Id { get; }

        Regulus.Remote.Property<string> Name { get; }

        Regulus.Remote.Property<float> View { get; }

        Regulus.Remote.Property<float> Direction { get; }

        Regulus.Remote.Property<ACTOR_STATUS_TYPE> Status { get; }

        event Action<EquipStatus[]> EquipEvent;

        
        event Action<VisibleStatus> StatusEvent;

        event Action<Energy> EnergyEvent;

        Regulus.Remote.Property< Vector2 >Position { get; }

        void QueryStatus();

        event Action<string> TalkMessageEvent;
    }
}