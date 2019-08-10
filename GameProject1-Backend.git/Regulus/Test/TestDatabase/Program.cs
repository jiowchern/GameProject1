﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDatabase
{
    [Serializable]
    public enum LoginResult
    {
        RepeatLogin,
        Error,
        Success
    }
}
namespace Regulus.Project.Keys.Serializable
{
    [Serializable]
    public class AccountInfomation
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid Id { get; set; }
    }

    [Serializable]
    public class EntityLookInfomation
    {
        public string Name { get; set; }
    }

    [Serializable]
    public class EntityPropertyInfomation
    {
        public EntityPropertyInfomation()
        {
            Position = new CustomType.Vector2();
        }

        public Guid Id { get; set; }
        public Regulus.CustomType.Vector2 Position { get; set; }
        public int Vision { get; set; }
		public float Speed { get; set; }
    }

    [Serializable]
    public class EntityInfomation
    {
        public EntityInfomation()
        {
            Position = new CustomType.Vector2();
        }
        public Guid Id { get; set; }
        public Regulus.CustomType.Vector2 Position { get; set; }
		public float Speed { get; set; }
    }


    [Serializable]
    public class DBEntityInfomation
    {
        public DBEntityInfomation()
        {
            Look = new EntityLookInfomation();
            Property = new EntityPropertyInfomation();
        }
        public EntityPropertyInfomation Property { get; set; }
        public EntityLookInfomation Look { get; set; }
        public Guid Owner { get; set; }
    }

	class PlayerMoverAbility 
	{
		private DBEntityInfomation _DBActorInfomation;

		public PlayerMoverAbility(Serializable.DBEntityInfomation dB_actor_infomation)
		{			
			_DBActorInfomation = dB_actor_infomation;
		}
		CustomType.Vector2 _Start;
		CustomType.Vector2 _End;
		CustomType.Vector2 _Vector = new CustomType.Vector2();
		long _BeginTime;
		double _TotalTime;
		double _Distance ; 
		public void Move(CustomType.Vector2 end, long time)
		{
			_Start = Regulus.Utility.ValueHelper.DeepCopy(_DBActorInfomation.Property.Position);
			_End = end;
			_BeginTime = time;

			float x = System.Math.Abs(_End.X - _Start.X);
			float y = System.Math.Abs(_End.Y - _Start.Y);
			_Distance = Math.Sqrt(x * x + y * y);

			_Vector.X = x ;
			_Vector.Y = y ;
			_TotalTime = _Distance / _DBActorInfomation.Property.Speed;
		}

		public void Update(long time)
		{
			System.DateTime begin = new DateTime(_BeginTime);
			System.DateTime current = new DateTime(time);
			var seconds = (current - begin).TotalSeconds;
			float newDistPer = (float)(seconds / _TotalTime);
			if (newDistPer < 1.0)
			{
                var position = new CustomType.Vector2();
                position.X = _Start.X + _Vector.X * newDistPer;
                position.Y = _Start.Y + _Vector.Y * newDistPer;
                _DBActorInfomation.Property.Position = position;				
			}
		}
	}
    class Program
    {
        static void Main(string[] args)
        {
			var db3 = new DBEntityInfomation();
			db3.Property.Speed = 1;
            var position = new CustomType.Vector2();
            position.X = 4530;
            position.Y = 5430;
            db3.Property.Position = position;
			

			var db4 = new DBEntityInfomation();
            position.X = 100;
            position.Y = 33220;
            db4.Property.Position = position;			


			System.TimeSpan time = new TimeSpan(0, 0, 0);
			PlayerMoverAbility pma = new PlayerMoverAbility(db3);
			pma.Move(db4.Property.Position, time.Ticks);

			pma.Update(time.Add(new TimeSpan(0,0,50)).Ticks);

			pma.Update(time.Add(new TimeSpan(0, 0, 99)).Ticks);
			


            DBEntityInfomation db1 = new DBEntityInfomation();
            db1.Property.Position = position;
            DBEntityInfomation db2 = new DBEntityInfomation();
            db2.Property.Position = position;
            db2.Look.Name = "dff";
            var ret = Regulus.Utility.ValueHelper.DeepEqual(db1, db2);
            Regulus.NoSQL.Database db = new Regulus.NoSQL.Database();
            db.Launch("mongodb://127.0.0.1:27017", "Keys");
            db.Shutdown();
        }
    }
}
