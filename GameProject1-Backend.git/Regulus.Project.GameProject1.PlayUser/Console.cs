using Regulus.Remote.Ghost;

namespace Regulus.Project.GameProject1.PlayUser
{
    class Console : Regulus.Utility.WindowConsole
    {
        private readonly IAgent _Agent;
        
        public Console(IAgent agent)
        {
        
            this._Agent = agent;
        }

        protected override void _Launch()
        {
            
        }

        protected override void _Shutdown()
        {

        }

        protected override void _Update()
        {
            _Agent.Update();
        }
    }
}
