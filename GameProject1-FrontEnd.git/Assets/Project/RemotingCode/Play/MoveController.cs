using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class MoveController : IMoveController
    {
        private readonly Entity _Player;

        public float Backward;
        public float Forward;
        public float RunForward;
        public float TurnLeft;
        public float TurnRight;
        public MoveController(Entity player)
        {
            _Player = player;
            Backward = -1;
            Forward = 1;
            RunForward = 3;

            TurnLeft = 500  ;
            TurnRight = 500;
        }

        void IMoveController.Forward()
        {
            _Player.Move(0, Forward);
        }

        void IMoveController.Backward()
        {
            _Player.Back(Backward);
        }

        void IMoveController.StopMove()
        {
            _Player.Stop();
        }

        void IMoveController.TrunLeft()
        {
            _Player.Trun(-TurnLeft);
        }

        void IMoveController.TrunRight()
        {
            _Player.Trun(TurnRight);
        }

        void IMoveController.StopTrun()
        {
            _Player.Trun(0);
        }

        void IMoveController.RunForward()
        {
            _Player.Move(0, RunForward);
        }
    }
}