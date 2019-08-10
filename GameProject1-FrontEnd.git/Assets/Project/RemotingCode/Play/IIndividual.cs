using System;
using System.Collections.Generic;

using Regulus.CustomType;
using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    public interface IIndividual : IVisible
    {
        Rect Bounds { get; }        

        Polygon Mesh { get; }

        event Action BoundsEvent;

        void SetPosition(float x, float y);

        IEnumerable<Item> Stolen(Guid id);

        void AttachHit(Guid target, HitForce force);

        bool IsBlock();

    

        void AddDirection(float dir);

        void SetPosition(Vector2 position);

        event Action<Guid,IEnumerable<Item>> TheftEvent;

        void Transmit(string target_realm);
    }
}