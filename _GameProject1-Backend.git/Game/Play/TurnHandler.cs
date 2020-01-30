using System;
using System.Collections.Generic;

using Regulus.BehaviourTree;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class TurnHandler 
    {
        private readonly StandardBehavior _StandardBehavior;

        private float _Angle;

        private float _TimeCounter;

        private float _Delta;

        private IEnumerator<TICKRESULT> _Iterator;

        public TurnHandler(StandardBehavior standard_behavior )
        {
            _StandardBehavior = standard_behavior;            
        }

        public void Input(float angle)
        {
            _Angle = angle;
            _Iterator = _Run().GetEnumerator();
        }

        TICKRESULT _Tick(float delta)
        {
            _TimeCounter -= delta;
            if (_TimeCounter > 0)
            {                           
                return TICKRESULT.RUNNING;                    
            }
#if UNITY_EDITOR
            //UnityEngine.Debug.Log("Done TurnTimeCounter = " + _TimeCounter);
#endif
            _StandardBehavior.StopTrun();
            return TICKRESULT.SUCCESS;
        }

        void _Start()
        {

            var turnSpeed = _StandardBehavior.GetTrunSpeed();
            if (turnSpeed <= 0)
                return ;
            var angle = _Angle;

            angle %= 360;
            angle += 360;
            angle %= 360;
            if(angle > 180)
                _TimeCounter = (180 - (angle % 180)) / turnSpeed;
            else
            {
                _TimeCounter = angle / turnSpeed;
            }


            if (angle > 180)
                _StandardBehavior.MoveLeft();
            else if (angle <= 180)
                _StandardBehavior.MoveRight();

#if UNITY_EDITOR
            // UnityEngine.Debug.Log("TurnTimeCounter = " + _TimeCounter);
            //UnityEngine.Debug.Log("Turn angle = " + angle);
#endif

            
        }

        void _End()
        {

            
        }

        public TICKRESULT Run(float delta)
        {
            _Delta = delta;
            _Iterator.MoveNext();
            var result = _Iterator.Current;
            return result;
        }

        IEnumerable<TICKRESULT> _Run()
        {
            while (true)
            {
                

                _Start();

                TICKRESULT result;
                do
                {
                    result = _Tick(_Delta);
                    yield return result;
                }
                while (result == TICKRESULT.RUNNING);

                _End();
                yield return result;
            }
        }
    }
}