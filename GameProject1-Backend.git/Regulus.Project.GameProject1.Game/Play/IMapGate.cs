using System;

using Regulus.Utility;
using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    public interface IMapGate
    {

        string Name { get; }
        void Left(Entity player);

        void Join(Entity player);        

        void Pass(Vector2 position, ENTITY[] entitys);

        void Pass(Vector2 position, Guid id);

        Guid SpawnEnterance(ENTITY type);
        Guid[] SpawnField(ENTITY[] types);

        Guid Spawn(ENTITY type);

        void Exit(Guid contestant);

        event Action<Guid> WaitEvent;
    }
}