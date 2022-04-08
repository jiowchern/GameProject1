using System;
using System.Collections.Generic;
using System.Linq;

using Regulus.BehaviourTree;



using Vector2 = Regulus.Utility.Vector2;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class ObstacleDetector 
    {
        

        private float _TimeCounter;

        public event Action<float> OutputEvent;
        
        

        

        private readonly List<Exit> _Nears;

        
        public ObstacleDetector( )
        {
            _Nears = new List<Exit>();            
        }

        

#if UNITY_EDITOR

        private void _UnityDrawLine(float hitAngle, Vector2 pos , UnityEngine.Color color , float distance , float decision_time)
        {
            var trunForce = Vector2.AngleToVector(hitAngle);
            var forcePos = pos + trunForce * (distance);
            UnityEngine.Debug.DrawLine(
                new UnityEngine.Vector3(pos.X, 0, pos.Y),
                new UnityEngine.Vector3(forcePos.X, 0, forcePos.Y),
                color,
                decision_time);
        }
#endif
        

        public TICKRESULT Detect(float delta, float decision_time, Entity entiry, StandardBehavior standard_behavior, float view_distance, int scan_angle)
        {
            var decisionTime = decision_time;
            var _Entiry = entiry;
            if (_Entiry == null)
            {
                throw new ArgumentNullException("_Entiry");
            }
            var goblinWisdom = standard_behavior;
            var distance = view_distance;
            var scanAngle = scan_angle;

            var pos = _Entiry.GetPosition();
            _TimeCounter += delta;
            if (_TimeCounter < decisionTime)
            {
                var view = (float)distance;
                var x = Math.PI * _TimeCounter / decisionTime;
                var y = (float)Math.Sin(x);
                var a = scanAngle * y - (scanAngle / 2);


                var target = goblinWisdom.Detect(a + _Entiry.Direction, view);

                if (target.Visible == null || (target.Visible != null && goblinWisdom.IsWall(target.Visible.EntityType)))
                {


                    var hitDistnace = target.HitPoint.DistanceTo(pos);
                    hitDistnace = (float)Math.Floor(hitDistnace);
                    var vector = target.HitPoint - pos;
                    var hitAngle = Vector2.VectorToAngle(vector.GetNormalized());
                    hitAngle += 360;
                    hitAngle %= 360;
                    //Unity Debug Code
#if UNITY_EDITOR
                    _UnityDrawLine(hitAngle, pos, UnityEngine.Color.red , view_distance , decisionTime);
#endif

                    _Nears.Add(new Exit() { Distance = hitDistnace, Direction = hitAngle });

                }


                return TICKRESULT.RUNNING;
            }


            var sortedDirections = from e in _Nears
                                   let diff = Math.Abs(e.Direction - _Entiry.Direction)
                                   where diff > 0.0f
                                   orderby diff
                                   select e;
            var soteds = from e in sortedDirections orderby e.Distance descending select e;
            var first = soteds.FirstOrDefault();

            OutputEvent(first.Direction - _Entiry.Direction);            
            _Nears.Clear();
            _TimeCounter = 0.0f;

#if UNITY_EDITOR
            var trunForce = Vector2.AngleToVector(first.Direction);
            var forcePos = pos + trunForce * (distance);
            UnityEngine.Debug.DrawLine(new UnityEngine.Vector3(pos.X, 0, pos.Y), new UnityEngine.Vector3(forcePos.X, 0, forcePos.Y), UnityEngine.Color.yellow, decisionTime);
            //UnityEngine.Debug.Log("TurnDirection = " + _StandardBehavior.TurnDirection);

#endif
            return TICKRESULT.SUCCESS;
        }
    }
}