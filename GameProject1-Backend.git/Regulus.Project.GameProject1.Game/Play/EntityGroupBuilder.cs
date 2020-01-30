using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

using Regulus.CustomType;
using Regulus.Project.GameProject1.Data;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class EntityGroupBuilder
    {
        private readonly string _Id;
        private readonly IMapFinder _Finder;
        private readonly IMapGate _Gate;


        internal EntityGroupBuilder(string id , IMapFinder finder , IMapGate gate)
        {
            _Id = id;
            _Finder = finder;
            _Gate = gate;
        }

        internal IEnumerable<IUpdatable> Create(float degree, Vector2 center )
        {            
            return _CreateFromGroup(_Id, degree, center);            
        }        
        
        private IEnumerable<IUpdatable> _CreateFromGroup(string id, float degree, Vector2 center)
        {
            
            var layout = Resource.Instance.FindEntityGroupLayout(id);
            var buildInfos = (from e in layout.Entitys
                        let radians = degree * (float) System.Math.PI/180f
                        let position = Polygon.RotatePoint(e.Position, new Vector2(), radians)
                        select new EntityCreateParameter
                        {
                            Id = e.Id,
                            Entity = EntityProvider.Create(e.Type , position + center , e.Direction + degree)                            
                        }).ToArray();
            
            foreach (var updatable in _CreateChests(layout.Chests, buildInfos))
            {
                yield return updatable;
            }

            foreach (var updatable in _CreateEnterances(layout.Enterances, buildInfos))
            {
                yield return updatable;
            }
            
            foreach (var updatable in _CreateStrongholds(layout.Strongholds, buildInfos))
            {
                yield return updatable;
            }

            foreach (var updatable in _CreateFields(layout.Fields, buildInfos))
            {
                yield return updatable;
            }

            foreach (var updatable in _CreateProtals(layout.Protals, buildInfos))
            {
                yield return updatable;
            }

            var inorganics = new List<Entity>();
            inorganics.AddRange(_CreateResources(layout.Resources, buildInfos));
            inorganics.AddRange(_CreateStatic(layout.Statics, buildInfos));
            inorganics.AddRange(_CreateWall(layout.Walls, buildInfos));
            yield return new InorganicsWisdom(  inorganics , _Gate);

        }

        private IEnumerable<IUpdatable> _CreateProtals(ProtalLayout[] protals, EntityCreateParameter[] build_infos)
        {
            foreach (var layout in protals)
            {
                var owner = _Find(build_infos, layout.Owner);
                yield return new PortalBehavior(owner , layout.TargetRealm , layout.PassEntity , _Gate ,_Finder);
            }
        }

        private IEnumerable<Entity> _CreateWall(WallLayout[] walls, EntityCreateParameter[] build_infos)
        {
            foreach (var layout in walls)
            {
                var owner = _Find(build_infos, layout.Owner);                
                yield return owner;
            }
        }

        private IEnumerable<IUpdatable> _CreateFields(FieldLayout[] fields, EntityCreateParameter[] build_infos)
        {
            foreach (var layout in fields)
            {
                var owner = _Find(build_infos, layout.Owner);
                yield return new FieldBehavior(layout.Kinds, owner, _Gate, _Finder);
            }
        }

        private IEnumerable<IUpdatable> _CreateStrongholds(StrongholdLayout[] strongholds, EntityCreateParameter[] build_infos)
        {
            foreach (var layout in strongholds)
            {
                var owner = _Find(build_infos, layout.Owner);
                yield return new StrongholdBehavior(layout.Kinds, owner, _Gate , _Finder);
            }
        }

        private IEnumerable<IUpdatable> _CreateEnterances(EnteranceLayout[] enterances, EntityCreateParameter[] build_infos)
        {
            foreach (var layout in enterances)
            {
                var owner = _Find(build_infos, layout.Owner);
                yield return new EnteranceBehavior(layout.Kinds , owner , _Gate);
            }
        }

        private IEnumerable<Entity> _CreateResources(ResourceLayout[] resources, IEnumerable<EntityCreateParameter> build_infos)
        {
            foreach (var layout in resources)
            {
                var owner = _Find(build_infos, layout.Owner);
                var bag = new ResourceBag(layout.Items);
                owner.SetBag(bag);
                yield return owner;
            }
        }

        private IEnumerable<Entity> _CreateStatic(StaticLayout[] statics, IEnumerable<EntityCreateParameter> build_infos)
        {
            foreach (var layout in statics)
            {
                var owner = _Find(build_infos, layout.Owner);
                owner.SetBody(layout.Body);

                yield return owner;
            }
        }

        private IEnumerable<IUpdatable> _CreateChests(ChestLayout[] chests, IEnumerable<EntityCreateParameter> build_infos)
        {
            foreach (var chestLayout in chests)
            {
                var owner = _Find(build_infos, chestLayout.Owner);
                var exit = _Find(build_infos, chestLayout.Exit);
                var debirs = _Find(build_infos, chestLayout.Debirs);
                var gate = _Find(build_infos, chestLayout.Gate);

                var chest = new ChestBehavior(owner , exit , debirs , gate , _Finder , _Gate);
                yield return chest;
            }
        }

        private Entity _Find(IEnumerable<EntityCreateParameter> build_infos, Guid owner)
        {
            return (from build_info in build_infos where build_info.Id == owner select build_info.Entity).Single();
        }
    }
}


