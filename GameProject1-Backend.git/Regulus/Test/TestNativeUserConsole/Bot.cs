﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TestNativeUserConsole
{
    partial class Bot : Regulus.Utility.IUpdatable
    {
        private string _Name;

        public event Action ExitEvent;

        Regulus.Utility.StageMachine _Machine;

        Regulus.Utility.Console.IViewer _View;
        bool Regulus.Utility.IUpdatable.Update()
        {
            _Machine.Update();
            return true;
        }

        void Regulus.Framework.ILaunched.Launch()
        {
            _ToConnect();
        }

        private void _ToConnect()
        {
            var stage = new ConnectStage(User,_Ip , _Port);
            stage.ConnectResultEvent += stage_ConnectResultEvent;
            _Machine.Push(stage);
        }

        void stage_ConnectResultEvent(bool connect_result)
        {
            if (connect_result)
            {
                _ToSend();
            }
            else
            {
                ExitEvent();
            }
        }

        private void _ToSend()
        {
            var stage = new SendStage(User, _View);
            stage.ContinueEvent += (continue_send) => 
            {
                if (continue_send)
                {
                    _ToSend();
                }
                else
                {
                    ExitEvent();
                }
            };
            _Machine.Push(stage);
        }

        void Regulus.Framework.ILaunched.Shutdown()
        {
            
        }

        public TestNativeUser.IUser User { get; private set; }
        string _Ip;
        int _Port;
        

        public Bot(TestNativeUser.IUser user, string name,Regulus.Utility.Console.IViewer view,string ip,int port)
        {
            _Ip = ip; ;
            _Port = port;
            // TODO: Complete member initialization
            this.User = user;
            this._Name = name;
            _Machine = new Regulus.Utility.StageMachine();
            _View = view;
        }
    }

    partial class Bot : Regulus.Utility.IUpdatable
    {
        class ConnectStage : Regulus.Utility.IStage
        {
            string _Ip;
            int _Port;
            TestNativeUser.IUser _User;
            public event Action<bool> ConnectResultEvent;
            public ConnectStage(TestNativeUser.IUser user , string ip , int port)
            {
                _Ip = ip;
                _Port = port;
                _User = user;
            }
            void Regulus.Utility.IStage.Enter()
            {
                _User.ConnectProvider.Supply += ConnectProvider_Supply;
            }

            void ConnectProvider_Supply(TestNativeGameCore.IConnect obj)
            {
                var val = obj.Connect(_Ip, _Port);
                val.OnValue += val_OnValue;
            }

            void val_OnValue(bool obj)
            {
                ConnectResultEvent(obj);
            }

            void Regulus.Utility.IStage.Leave()
            {
                _User.ConnectProvider.Supply -= ConnectProvider_Supply;
            }

            void Regulus.Utility.IStage.Update()
            {
                
            }
        }
    }

    partial class Bot : Regulus.Utility.IUpdatable
    {
        class SendStage : Regulus.Utility.IStage
        {
            TestNativeUser.IUser _User;
            Regulus.Utility.Console.IViewer _View;
            public event Action<bool> ContinueEvent;
            public SendStage(TestNativeUser.IUser user, Regulus.Utility.Console.IViewer view)
            {
                _User = user;
                _View = view;
            }
            void Regulus.Utility.IStage.Enter()
            {
                _User.MessagerProvider.Supply += MessagerProvider_Supply;
            }

            void MessagerProvider_Supply(TestNativeGameCore.IMessager obj)
            {
                var val = obj.Send(_BuildString());
                val.OnValue += val_OnValue;
            }

            void val_OnValue(string respunse)
            {
                _View.WriteLine(respunse);
                ContinueEvent( Regulus.Utility.Random.Next(0,2) == 0 );
            }

            private string _BuildString()
            {
                //return "1";
                return System.DateTime.Now.ToString();
            }

            void Regulus.Utility.IStage.Leave()
            {
                
            }

            void Regulus.Utility.IStage.Update()
            {
                
            }
        }
    }
}
