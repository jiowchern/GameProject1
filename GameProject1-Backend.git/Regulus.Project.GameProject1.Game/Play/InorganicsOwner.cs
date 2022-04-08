
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class InorganicsOwner : IUpdatable
    {
        private readonly Entity[] _Entitys;

        private readonly IMapGate _Gate;

        private readonly Behavior _Behavior;

        private readonly Regulus.Utility.Updater _Updater;

        public InorganicsOwner(Entity[] entitys, IMapGate gate , Behavior behavior)
        {
            _Behavior = behavior;
            _Entitys = entitys;
            _Gate = gate;
            _Updater = new Updater();
        }

        void IBootable.Launch()
        {
            _Updater.Add(_Behavior);
            foreach (var entity in _Entitys)
                _Gate.Join(entity);
        }

        void IBootable.Shutdown()
        {
            foreach (var entity in _Entitys)
                _Gate.Left(entity);
        }

        bool IUpdatable.Update()
        {
            _Updater.Working();
            return true;
        }
    }
}