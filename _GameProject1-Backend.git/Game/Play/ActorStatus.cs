using Regulus.Project.GameProject1.Data;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    abstract internal class ActorStatus : IStage
    {        

        public abstract void Enter();

        public abstract void Leave();

        public abstract void Update();
    }
}