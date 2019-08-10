using System;
using System.Collections.Generic;
using System.Linq;
using Regulus.Extension;
using Regulus.CustomType;
using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class Mover
    {

        private Entity _Entity;

        private IIndividual _Individual { get { return this._Entity; } }

        public Mover(Entity entity)
        {
            this._Entity = entity;
        }
        public Rect GetOrbit(Vector2 velocity)
        {
            return new Orbit(this._Individual.Mesh, velocity).Vision;
        }

        public class Orbit
        {
            private readonly Rect _Rect;

            public Orbit(Polygon body, Vector2 velocity)
            {
                List<Vector2> points = new List<Vector2>();
                points.AddRange(body.Points);

                var polygon = body.Clone();
                polygon.Offset(velocity);

                points.AddRange(polygon.Points);
                _Rect = points.ToRect();
            }

            public Rect Vision { get { return this._Rect; } }
        }

        public void Set(Vector2 velocity)
        {
            this._Entity.UpdatePosition(velocity);
        }

        public IEnumerable<IIndividual> Move(Vector2 velocity, IEnumerable<IIndividual> entitys)
        {
            var polygon = this._GetThroughRange(velocity);

            //var polygon = _Individual.Mesh;

            var targets = from x in entitys
                          let result = this._Collide(x, polygon, new Vector2())
                          where EntityData.IsWall(x.EntityType) && result.Intersect
                          //&& result.WillIntersect
                          select x;
            if (targets.Any() == false)
            {
                this.Set(velocity);
            }
            return targets;
        }

        
        private Polygon _GetThroughRange(Vector2 velocity)
        {
            var after = this._Individual.Mesh.Clone();
            after.Offset(velocity);

            Vector2[] points = new Vector2[8];
            points[0] = _Individual.Mesh.Points[0];
            points[1] = _Individual.Mesh.Points[1];
            points[2] = _Individual.Mesh.Points[2];
            points[3] = _Individual.Mesh.Points[3];

            points[4] = after.Points[0];
            points[5] = after.Points[1];
            points[6] = after.Points[2];
            points[7] = after.Points[3];

            

            var polygon = new Polygon(points);
            
            return polygon;

        }

        private Regulus.CustomType.Polygon.CollisionResult _Collide(IIndividual individual, Polygon polygon, Vector2 velocity)
        {
            var result = Polygon.Collision(polygon, individual.Mesh, velocity);
            return result;
        }
    }
}