using System;
using System.Collections.Generic;
using System.Linq;


using Regulus.BehaviourTree;
using Regulus.Utility;

using Regulus.Project.GameProject1.Data;



using Vector2 = Regulus.Utility.Vector2;




namespace Regulus.Project.GameProject1.Game.Play
{
    internal class StandardBehavior : Behavior
    {
        private readonly Entity _Entiry;

        private readonly List<IVisible> _FieldOfVision;


        private readonly ActorMind _ActorMind;

        private readonly Regulus.Utility.IRandom _Random;
        private IMoveController _MoveController;

        private float _ScanCycle;

        private readonly float _DecisionTime;

        private readonly Regulus.Utility.FPSCounter _FPS;

        private IEmotion _Emotion;

        private INormalSkill _NormalSkill;

        private ICastSkill _CastSkill;

        private readonly List<ACTOR_STATUS_TYPE> _UsableSkills;

        private IBattleSkill _BattleSkill;

        private bool _IsCollide;

        private bool _Investigate;

        private IMakeSkill _MakeSkill;

        private ItemFormulaLite[] _Formulas;

        private readonly InventoryProxy _Bag;

        private readonly InventoryProxy _Equipment;

        private readonly Dictionary<ENTITY , float> _EntityImperils;

        private readonly Queue<string> _Messages;

        private int _DebugLeftCount;
        private int _DebugRealLeftCount;

        private int _DebugJoinCount;

        private readonly ITicker _Node;

        private int _DebugRejoinCount;

        private IInventoryController _InventoryController;

        public StandardBehavior(Entity entiry)
        {
            _Equipment = new InventoryProxy();
            _Bag = new InventoryProxy();
            var type  = entiry.GetVisible().EntityType;

            _Messages = new Queue<string>();

            _FPS = new FPSCounter();
            
            _UsableSkills = new List<ACTOR_STATUS_TYPE>();

            _EntityImperils = _InitialImperil(type);
            _ActorMind = new ActorMind(type);

            _DecisionTime = 0.5f;
                        
            _FieldOfVision = new List<IVisible>();
                        
            _Entiry = entiry;

            _Random = Regulus.Utility.Random.Instance;

            //asin(2 / sqrt(2 ^ 2 + 10 ^ 2))

            _Node = _BuildNode();
        }

        private Dictionary<ENTITY, float> _InitialImperil(ENTITY type)
        {
            if (type == ENTITY.ACTOR2)
            {                
                return new Dictionary<ENTITY, float>
                {
                    {ENTITY.ACTOR2, -5},
                    {ENTITY.ACTOR3, 0},
                    {ENTITY.ACTOR4, 0},
                    {ENTITY.ACTOR5, 0}
                }; 
            }
            else if (type == ENTITY.ACTOR3)
            {
                return new Dictionary<ENTITY, float>
                {
                    {ENTITY.ACTOR1, 0},
                    {ENTITY.ACTOR3, -10},
                    {ENTITY.ACTOR4, 1},
                    {ENTITY.ACTOR5, 3}
                };
            }
            else if (type == ENTITY.ACTOR4)
            {
                return new Dictionary<ENTITY, float>
                {
                    {ENTITY.ACTOR1, 0},
                    {ENTITY.ACTOR3, 3},
                    {ENTITY.ACTOR4, -10},
                    {ENTITY.ACTOR5, 1}
                };
            }
            else if (type == ENTITY.ACTOR5)
            {
                return new Dictionary<ENTITY, float>
                {
                    {ENTITY.ACTOR1, 0},
                    {ENTITY.ACTOR3, 1},
                    {ENTITY.ACTOR4, 3},
                    {ENTITY.ACTOR5, -10}
                };
            }

            return new Dictionary<ENTITY, float>();
        }

        protected override Regulus.BehaviourTree.ITicker _Launch()
        {
            foreach (var visible in _FieldOfVision.ToList())
            {
                _ClearIndividual(visible);
            }
            _FieldOfVision.Clear();

            _Entiry.GetVisible().StatusEvent += _OnStatus;

            _Entiry.InjuredEvent += _OnInjured;
            _Entiry.CollideTargets.JoinEvent += _HaveCollide;

            _Bind();

            _Node.Reset();
            return _Node;
        }

     

        private ITicker _BuildNode()
        {
            Guid actor = Guid.Empty;

            var builder = new Regulus.BehaviourTree.Builder();
            var node = builder
                .Selector()
                    .Sub(_ListenCommandStrategy())
                    .Sequence()
                        .Action((delta) => _DetectActor(_Entiry.GetViewLength(), _GetOffsetDirection(120), out actor))
                        .Action((delta) => _NotSeen(actor))
                        .Action((delta) => _Remember(actor))
                    .End()
                    .Sub(_CollisionWayfindingStrategy())
                    .Sub(_AttackStrategy())
                    .Sub(_InvestigateStrategy())
              
                    .Sequence()
                        .Action(_NotEnemy)
                        .Selector()
                            .Sequence()
                                .Action(_InBattle)
                                .Action(_ToNormal)
                            .End()

                            .Sub(_RescueCompanion())
                            .Sub(_LootStrategy())
                            .Sub(_ResourceObtaionStrategy())
                            .Sub(_MakeItemStrategy())
                            .Sub(_EquipItemStrategy())
                            .Sub(_DiscardItemStrategy())
                        .End()
                        
                    .End()

                    .Sub(_DistanceWayfindingStrategy())

                    // 沒事就前進
                    .Sequence()
                        .Action(_MoveForward)
                    .End()
                .End()
                .Build();
            return node;
        }

        

        private ITicker _RescueCompanion()
        {
            var trace = new TraceStrategy(this);
            Guid target = Guid.Empty;
            var builder = new Regulus.BehaviourTree.Builder();
            return builder
                    .Sequence()
                        .Action((delta) => _CheckItemAmount("AidKit", (count) => count >= 1))
                        .Action((delta) => _FindWounded(out target))
                        .Action((delta) => trace.Reset(target , 1))
                        .Action(trace.Tick)
                        .Action(_StopMove)
                        .Action((dt) => _UseItem("AidKit") )
                    .End()
                .Build();
        }

        private TICKRESULT _FindWounded(out Guid target)
        {
            target = Guid.Empty;
            var wonded = _ActorMind.FindWounded(_Entiry.GetVisible().EntityType, _FieldOfVision);
            if (wonded != null)
            {
                target = wonded.Id;
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private ITicker _ListenCommandStrategy()
        {
            var builder = new Regulus.BehaviourTree.Builder();
            return builder
                    .Sequence()
                        .Action(_HandleCommandAction)
                    .End()
                .Build();
        }

        private TICKRESULT _HandleCommandAction(float arg)
        {
            if (_Messages.Count > 0)
            {
                var message = _Messages.Dequeue();
                if (message == "fps")
                {
                    if(_Emotion != null)
                        _Emotion.Talk(string.Format("FPS:{0} Delta:{1} mind:{2} msg:{3}" , _FPS.Value , LastDelta , _ActorMind.GetActorCount() , _Messages.Count));

                }
                else if(message == "fov")
                {
                    if (_Emotion != null)
                        _Emotion.Talk(string.Format("view:{2} join:{0}({5}) - left:{1}({4}) = {3}", _DebugJoinCount , _DebugLeftCount, _FieldOfVision.Count , _DebugJoinCount - _DebugLeftCount , _DebugRealLeftCount , _DebugRejoinCount));
                }


                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private ITicker _EquipItemStrategy()
        {
            var timeTrigger = new TimeTriggerStrategy(1f);
            var builder = new Regulus.BehaviourTree.Builder();
            var node = builder
                    .Sequence()
                        .Action(timeTrigger.Tick)
                        .Action((delta) => timeTrigger.Reset(1))                                                
                        .Selector()
                            .Action((delta) => _Equip(EQUIP_PART.RIGHT_HAND))
                            .Action((delta) => _Equip(EQUIP_PART.FIXTURES))                            
                        .End()
                        
                    .End()
                .Build();
            return node;
        }

        private TICKRESULT _Equip(EQUIP_PART part)
        {
            
            if (_InventoryController == null)
                return TICKRESULT.FAILURE;

            _Unequip(part);

            var items = _Bag.FindByPart(part);
            var item = items.Concat(new Item[] {null}).Shuffle().FirstOrDefault();
            if (item != null)
            {
                _InventoryController.Equip(item.Id);
                return TICKRESULT.SUCCESS;
            }

            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _Unequip(EQUIP_PART part)
        {
            var item = _Equipment.FindByPart(part).FirstOrDefault();
            if (item != null)
            {
                _InventoryController.Unequip(item.Id);
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _UseItem(string item)
        {
            if(_InventoryController == null)
                return TICKRESULT.FAILURE;
            var id = _Bag.FindIdByName(item);
            _InventoryController.Use(id);
           return TICKRESULT.SUCCESS;
        }


        private ITicker _DiscardItemStrategy()
        {
            var builder = new Regulus.BehaviourTree.Builder();

            var timeTrigger = new TimeTriggerStrategy(10f);

            var node = builder
                    .Sequence()
                        .Action(timeTrigger.Tick)
                        .Action((delta) => timeTrigger.Reset(10f))                        
                        .Selector()
                            .Sequence()
                                .Action((delta) => _CheckItemAmount("Axe1", (count) => count >= 2))
                                .Action((delta) => _DiscardItem("Axe1" , 1))
                                .Action((delta) => _Talk("Discard axe."))
                            .End()
                            .Sequence()
                                .Action((delta) => _CheckItemAmount("Sword1", (count) => count >= 2))
                                .Action((delta) => _DiscardItem("Sword1", 1))
                                .Action((delta) => _Talk("Discard sword."))
                            .End()

                            .Sequence()
                                .Action((delta) => _CheckItemAmount("Lamp", (count) => count >= 2))
                                .Action((delta) => _DiscardItem("Lamp", 1))
                                .Action((delta) => _Talk("Discard lamp."))
                            .End()

                            .Sequence()
                                .Action((delta) => _CheckItemAmount("AidKit", (count) => count >= 2))
                                .Action((delta) => _DiscardItem("AidKit", 1))
                                .Action((delta) => _Talk("Discard AidKit."))
                            .End()                            
                        .End()

                    .End()
                .Build();
            return node;
        }

        private TICKRESULT _DiscardItem(string item_name, int count)
        {
            if(_InventoryController == null)
                return TICKRESULT.FAILURE;
            var item = _Bag.FindIdByName(item_name);
            _InventoryController.Discard(item);

            return TICKRESULT.SUCCESS;            

        }

        private ITicker _MakeItemStrategy()
        {
            var index = 0;
            var builder = new Regulus.BehaviourTree.Builder();

            var timeTrigger = new TimeTriggerStrategy(10f);

            var node = builder
                    .Sequence()            
                        .Action(timeTrigger.Tick)
                        .Action((delta) => timeTrigger.Reset(10f))
                        .Action(_ToMake)      
                        .Action(()=>new WaitSecondStrategy(3), t => t.Tick, t => t.Start, t => t.End)   
                        .Action((delta) => _GetRandomIndex(0, 4 , out index))
                        .Selector()
                            .Sequence()
                                .Action((delta) => _If( index == 0) )
                                .Action((delta) => _CheckItemAmount("Axe1" , (count) => count < 2 ))
                                .Action((delta) => _MakeItem("Axe1"))
                                .Action((delta) => _Talk("Produced axe."))
                            .End()
                            .Sequence()
                                .Action((delta) => _If(index == 1))
                                .Action((delta) => _CheckItemAmount("Sword1", (count) => count < 2))
                                .Action((delta) => _MakeItem("Sword1"))
                                .Action((delta) => _Talk("Produced sword."))
                            .End()

                            .Sequence()
                                .Action((delta) => _If(index == 2))
                                .Action((delta) => _CheckItemAmount("Lamp", (count) => count < 2))
                                .Action((delta) => _MakeItem("Lamp"))
                                .Action((delta) => _Talk("Produced lamp."))
                            .End()

                            .Sequence()
                                .Action((delta) => _If(index == 3))
                                .Action((delta) => _CheckItemAmount("AidKit", (count) => count < 2))
                                .Action((delta) => _MakeItem("AidKit"))
                                .Action((delta) => _Talk("Produced aid."))
                            .End()
                            .Sequence()
                                .Action((delta) => _Talk("Nothing maked."))                                
                            .End()
                            
                        .End()
                        .Action(_ReleaseMake)
                    .End()
                .Build();
            return node;
        }

        private TICKRESULT _If(bool b)
        {
            if(b)
                return TICKRESULT.SUCCESS;
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _GetRandomIndex(int min, int max, out int index)
        {
            index = _Random.NextInt(min, max);
            return TICKRESULT.SUCCESS;
        }

        private TICKRESULT _ReleaseMake(float arg)
        {
            if (_MakeSkill != null)
            {
                _MakeSkill.Exit();
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;

        }

        private TICKRESULT _MakeItem(string item_name)
        {
            if (_MakeSkill == null)
            {
                return TICKRESULT.FAILURE;
            }

            if (_MakeSkill != null && _Formulas == null)
            {
                return TICKRESULT.RUNNING;
            }
            var formula = _Formulas.FirstOrDefault((f) => f.Id == item_name);
            if (formula != null)
            {
                bool enough = true;
                foreach (var needItem in formula.NeedItems)
                {

                    var itemAmount = _Entiry.Bag.GetItemAmount(needItem.Item);
                    if (itemAmount < needItem.Min)
                    {
                        enough = false;
                        break;
                    }
                }

                if (enough)
                {
                    _MakeSkill.Create(formula.Item, (from items in formula.NeedItems select items.Min).ToArray());                    
                    return TICKRESULT.SUCCESS;
                }
            }            
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _CheckItemAmount(string item, Func<int, bool> func)
        {
            var amount = _Bag.GetAmount(item);
            if (func(amount))
            {
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _MakeItem()
        {
            if (_MakeSkill == null)
            {
                return TICKRESULT.FAILURE;
            }

            if (_MakeSkill != null && _Formulas == null)
            {
                return TICKRESULT.RUNNING;
            }
            var random = Regulus.Utility.Random.Instance;
            foreach (var formula in _Formulas.OrderBy(i => random.NextDouble()))
            {
                
                bool enough = true;
                foreach (var needItem in formula.NeedItems)
                {

                    var itemAmount = _Entiry.Bag.GetItemAmount(needItem.Item);
                    if (itemAmount < needItem.Min)
                    {
                        enough = false;
                        break;
                    }
                }

                if (enough)
                {                    
                    _MakeSkill.Create(formula.Item , (from items in formula.NeedItems select  items.Min).ToArray() );
                    _MakeSkill.Exit();
                    return TICKRESULT.SUCCESS;
                }
            }
            _MakeSkill.Exit();
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _ToMake(float arg)
        {
            if(_MakeSkill != null)
                return TICKRESULT.SUCCESS;

            if (_NormalSkill != null)
            {
                _NormalSkill.Make();
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private ITicker _InvestigateStrategy()
        {
            var th = new TurnHandler(this);
            var builder = new Regulus.BehaviourTree.Builder();
            var node = builder
                    .Sequence()
                        .Action(_NotEnemy)
                        .Action(_NeedInvestigate )      
                        
                        .Action(
                            (delta) =>
                            {
                                var angel = Regulus.Utility.Random.Instance.NextFloat(135, 225);
                                th.Input(angel);
                                return TICKRESULT.SUCCESS;
                            })
                        .Action((delta) =>
                        {
                            if (GetTrunSpeed() > 0)
                            {                                
                                return th.Run(delta);
                            }
                            return TICKRESULT.FAILURE;
                        })
                        .Action((delta) => _Talk("?"))
                        .Action(_DoneInvestigate)
                    .End()
                .Build();
            return node;
        }

        private TICKRESULT _DoneInvestigate(float arg)
        {
            _Investigate = false;
            return TICKRESULT.SUCCESS;
        }

        private TICKRESULT _NeedInvestigate(float arg)
        {            
            if (_Investigate)
            {
                return TICKRESULT.SUCCESS;
            }

            return TICKRESULT.FAILURE;

        }
        private ITicker _ResourceObtaionStrategy()
        {
            
            var th = new TurnHandler(this);


            var traceStrategy = new TraceStrategy(this);
            Guid target = Guid.Empty;
            var builder = new Regulus.BehaviourTree.Builder();
            var distance = 1f;
            var node = builder
                    .Sequence()
                        .Action(_NotEnemy)
                        .Action((delta) => _FindResourceTarget(out target))
                        .Action((delta) => _GetDistance(target , out distance))
                        //.Action((delta) => _Talk(string.Format("Mining Target. Distance:{0}", distance)))
                        .Sequence()
                                .Action((delta) => traceStrategy.Reset(target, distance))
                                .Action((delta) => traceStrategy.Tick(delta))
                                //.Action((delta) => _Talk("Begin Mining ."))
                                .Action((delta) => _Loot(target))
                                //.Action((delta) => _Talk("Mining Done."))
                        .End()
                    .End()
                .Build();
            return node;
        }

        private TICKRESULT _GetDistance(Guid target, out float distance)
        {
            distance = 0f;
            var entity = _FieldOfVision.Find((e) => e.Id == target);
            if(entity == null)
                return TICKRESULT.FAILURE;

            var mesh = _BuildMesh(entity);

            float dis;
            Vector2 point;
            Vector2 normal;

            if (RayPolygonIntersect(
                _Entiry.GetPosition(),
                Vector2.AngleToVector(_Entiry.Direction),
                mesh.Points,
                out dis,
                out point,
                out normal))
            {
                distance = dis - _Entiry.GetDetectionRange();
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _FindResourceTarget(out Guid target_id)
        {
            if (_FieldOfVision.Any(e => e.EntityType == ENTITY.POOL))
            {
                var target = _ActorMind.FindResourceTarget(_FieldOfVision);
                if (target != null)
                {
                    target_id = target.Id;
                    return TICKRESULT.SUCCESS;
                }
            }
            

            target_id = Guid.Empty;
            return TICKRESULT.FAILURE;
        }

        private ITicker _LootStrategy()
        {            
            var th = new TurnHandler(this);


            var traceStrategy = new TraceStrategy(this);
            Guid target = Guid.Empty;
            var builder = new Regulus.BehaviourTree.Builder();
            var distance = 1f;
            var node = builder
                    .Sequence()
                        .Action(_NotEnemy)
                        .Action((delta) => _FindLootTarget(out target) )
                        .Action((delta) => traceStrategy.Reset(target , distance ) )
                        .Action((delta) => traceStrategy.Tick(delta))                               
                        .Action((delta) => _Loot(target))
                    .End()
                .Build();
            return node;
        }

        private TICKRESULT _Loot(Guid target)
        {
            if (_NormalSkill != null)
            {
                _NormalSkill.Explore(target);
                _ActorMind.Loot(target);
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _FindLootTarget(out Guid target_id)
        {
            var target = _ActorMind.FindLootTarget(_FieldOfVision);
            if (target != null)
            {
                target_id = target.Id;
                return TICKRESULT.SUCCESS;
            }
                
            target_id = Guid.Empty;
            return TICKRESULT.FAILURE;
        }

        private ITicker _CollisionWayfindingStrategy()
        {
            var th = new TurnHandler(this);
            
            var builder = new Regulus.BehaviourTree.Builder();
            return builder
                    .Sequence()                        
                        
                        .Action(
                            (delta) =>
                            {
                                var result = _CheckCollide();
                                th.Input(Regulus.Utility.Random.Instance.NextFloat(1, 360));
                                return result;
                            } )
                        
                        .Action((delta) => th.Run(delta))
                        
                        .Action(_MoveForward)
                        
                        .Action(()=>new WaitSecondStrategy(0.5f), t => t.Tick, t => t.Start, t => t.End)
                        
                    .End()   
                .Build();
        }

        private TICKRESULT _CheckCollide()
        {            
            if (_IsCollide)
            {
                _IsCollide = false;
                return TICKRESULT.SUCCESS;
            }                
            return TICKRESULT.FAILURE;
        }

        private ITicker _DistanceWayfindingStrategy()
        {
            var builder = new Regulus.BehaviourTree.Builder();            

            var th = new TurnHandler(this);
            var od = new ObstacleDetector();
            od.OutputEvent += th.Input;
            return builder
                .Sequence()
                    // 是否碰到障礙物
                    .Action((delta) => _DetectObstacle(delta, _Entiry.GetViewLength() / 2, _GetOffsetDirection(10.31f * 2)))

                    // 停止移動
                    //.Action(_StopMove)

                    // 檢查周遭障礙物
                    .Action((delta) => od.Detect(delta , _DecisionTime, _Entiry, this, _Entiry.GetViewLength(), 180) )

                    // 旋轉至出口
                    .Action((delta) => th.Run(delta) )
                .End().Build();
        }
    

        private ITicker _ChangeToBattle()
        {
            var builder = new Regulus.BehaviourTree.Builder();
            return builder.Sequence()
                   .Action(_InNormal)
                   .Action(_ToBattle)
                   .End().Build();
        }

        private ITicker _AttackStrategy()
        {
            Guid enemy = Guid.Empty;
            
            var skill = ACTOR_STATUS_TYPE.NORMAL_IDLE;
            float distance = 0;                        
            var builder = new Regulus.BehaviourTree.Builder();
            var traceStrategy = new TraceStrategy(this);
            return builder
                .Sequence()
                    .Action((delta) => _FindEnemy(out enemy))

                    .Selector()
                        .Sub(_ChangeToBattle())
                        .Sequence()
                            .Action((delta) => _FindSkill(ref skill, ref distance))
                            .Action((delta) => traceStrategy.Reset(enemy, distance))
                            .Action((delta) => traceStrategy.Tick(delta))
                            .Action((delta) => _UseSkill(skill))                            
                        .End()

                    .End()

                    
                .End()
                .Build();

        }



        public TICKRESULT CheckAngle(float angle)
        {
            return _CheckAngle(angle);
        }
        private TICKRESULT _CheckAngle(float angle)
        {
            if (Math.Abs(angle ) < 45)
            {
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _UseSkill(ACTOR_STATUS_TYPE skill)
        {
            if (_CastSkill != null)
            {
                _CastSkill.Cast(skill);
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        public TICKRESULT CheckDistance(Guid enemy, float distance)
        {
            return _CheckDistance(enemy , distance);
        }
        private TICKRESULT _CheckDistance(Guid enemy, float distance)
        {
            var target = _Find(enemy);

            if(target == null)
                return TICKRESULT.FAILURE;

            if(target.Position.Value.DistanceTo(_Entiry.GetPosition()) <= distance || distance <= 0f)
                return TICKRESULT.SUCCESS;

            return TICKRESULT.FAILURE;
        }


        public TICKRESULT GetTargetAngle(Guid id, ref float angle)
        {
            return _GetTargetAngle(id , ref angle);
        }

        private TICKRESULT _GetTargetAngle(Guid id , ref float angle)
        {
            var target = _Find(id);
            if (target == null)
            {
                return TICKRESULT.FAILURE;
            }
            var position = _Entiry.GetPosition();
            var diff = target.Position - position;
            var a = Vector2.VectorToAngle(diff.GetNormalized());
            a += 360;
            a %= 360;

            angle = a - _Entiry.Direction;

#if UNITY_EDITOR
            var distance = target.Position.DistanceTo(position);
            var trunForce = Vector2.AngleToVector(a);
            var forcePos = position + trunForce * (distance);
            UnityEngine.Debug.DrawLine(new UnityEngine.Vector3(position.X, 0, position.Y), new UnityEngine.Vector3(forcePos.X, 0, forcePos.Y), UnityEngine.Color.green , _DecisionTime);
            //UnityEngine.Debug.Log("TurnDirection = " + _GoblinWisdom.TurnDirection);

#endif
            return TICKRESULT.SUCCESS;
        }

        private IVisible _Find(Guid id)
        {
            return (from v in _FieldOfVision where v.Id == id select v).FirstOrDefault();
        }

        private TICKRESULT _FindSkill(ref ACTOR_STATUS_TYPE skill, ref float distance)
        {
            var strength = _Entiry.GetStrength();
            if (strength < 0)
                return TICKRESULT.FAILURE;
            var len = _UsableSkills.Count;
            if(len == 0)
                return TICKRESULT.FAILURE;
            var index = Regulus.Utility.Random.Instance.NextInt(0, len);

            skill = _UsableSkills[index];
            distance = 1f;
            if (skill == ACTOR_STATUS_TYPE.BATTLE_AXE_ATTACK1 ||
                skill == ACTOR_STATUS_TYPE.BATTLE_AXE_ATTACK2 
                )
            {
                distance = 1.3f;
            }
            else if (skill == ACTOR_STATUS_TYPE.BATTLE_AXE_BLOCK)
            {
                distance = 3f;
            }
            else if (skill == ACTOR_STATUS_TYPE.CLAYMORE_ATTACK1)
            {
                distance = 3f;
            }



            return TICKRESULT.SUCCESS;
        }

        private TICKRESULT _ToBattle(float arg)
        {
            if (_NormalSkill != null)
            {
                _NormalSkill.Battle();
                return TICKRESULT.SUCCESS;
            }

            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _InNormal(float arg)
        {
            if(_NormalSkill != null)
                return TICKRESULT.SUCCESS;
            return TICKRESULT.FAILURE;            
        }

        private void _OnInjured(Guid target, float damage)
        {
            if (_ActorMind.Hate(target, damage) == false)
            {
                _Investigate = true;
            }
        }

        private TICKRESULT _FindEnemy(out Guid enemy)
        {
            enemy = Guid.Empty;
            var result = _ActorMind.FindEnemy(_FieldOfVision);
            if (result != null)
            {                
                enemy = result.Id;
                return TICKRESULT.SUCCESS;
            }
                
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _ToNormal(float arg)
        {
            if (_BattleSkill != null)
            {
                _BattleSkill.Disarm();
                return TICKRESULT.SUCCESS;
            }
                
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _InBattle(float arg)
        {
            if (_CastSkill != null)
            {
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }


        private TICKRESULT _NotEnemy(float arg)
        {
            var actor = _ActorMind.FindEnemy(_FieldOfVision);
            if(actor == null)
                return TICKRESULT.SUCCESS;
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _Talk(string message)
        {
            if (_Emotion != null)
            {
                _Emotion.Talk(message);
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }

        private TICKRESULT _Remember(Guid id)
        {

            var actor = _FieldOfVision.FindAll((a) => a.Id == id).FirstOrDefault();
            if (actor != null)
            {
                
                _ActorMind.Add(id , _GetImperil(actor.EntityType));
                return TICKRESULT.SUCCESS;
            }

            return TICKRESULT.FAILURE;

        }

        private float _GetImperil(ENTITY entity_type)
        {
            float imperil ;
            _EntityImperils.TryGetValue(entity_type, out imperil);
            return imperil;
        }

        private TICKRESULT _NotSeen(Guid actor)
        {

            
            if (_ActorMind.Have(actor))
            {
                return TICKRESULT.FAILURE;
                
            }
            return TICKRESULT.SUCCESS;
        }

        

        private TICKRESULT _StopMove(float delta)
        {
            if (_MoveController != null)
            {
                
                _MoveController.StopMove();
                return TICKRESULT.SUCCESS;
            }
            
            return TICKRESULT.FAILURE;
        }
        private TICKRESULT _DetectActor(float distance, float angle, out Guid actor)
        {
            
            var target = _Detect(angle + _Entiry.Direction, distance);
            if (target.Visible != null )
            {
                actor = target.Visible.Id;
                return TICKRESULT.SUCCESS;
            }
            actor = Guid.Empty;
            return TICKRESULT.FAILURE;
        }

        

        private TICKRESULT _DetectObstacle(float delta, float distance , float angle)
        {
            
            var target = _Detect(angle + _Entiry.Direction, distance);

            if (target.Visible != null && 
                IsWall(target.Visible.EntityType) )
            {
                
                return TICKRESULT.SUCCESS;
            }
            
            return TICKRESULT.FAILURE;
        }

        private bool _InvalidResource(IVisible visible)
        {
            if (EntityData.IsResource(visible.EntityType))
                return _ActorMind.IsLooted(visible.Id) ;

            return false;
        }

        public TICKRESULT MoveForward(float arg1)
        {
            return _MoveForward(arg1);
        }
        private TICKRESULT _MoveForward(float arg1)
        {
            if (_MoveController != null)
            {
                _MoveController.Forward();
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
                
        }

        

       
        private void _Bind()
        {
            Transponder.Query<IInventoryController>().Supply += _SetInventoryController;
            Transponder.Query<IInventoryController>().Unsupply += _ClearInventoryController;

            Transponder.Query<IEquipmentNotifier>().Supply += _SetEquipment;
            Transponder.Query<IEquipmentNotifier>().Unsupply += _ClearEquipment;

            Transponder.Query<IBagNotifier>().Supply += _SetBag;
            Transponder.Query<IBagNotifier>().Unsupply += _ClearBag;

            Transponder.Query<IMakeSkill>().Supply += _SetMake;
            Transponder.Query<IMakeSkill>().Unsupply += _ClearMake;

            Transponder.Query<IEmotion>().Supply += _SetEmotion;
            Transponder.Query<IEmotion>().Unsupply += _ClearEmotion;
            Transponder.Query<IVisible>().Supply += _SetIndividual;
            Transponder.Query<IVisible>().Unsupply += _ClearIndividual;
            Transponder.Query<IMoveController>().Supply += _SetMoveController;
            Transponder.Query<IMoveController>().Unsupply += _ClearMoveController;

            Transponder.Query<INormalSkill>().Supply += _SetNormalSkill;
            Transponder.Query<INormalSkill>().Unsupply += _ClearNormalSkill;

            Transponder.Query<ICastSkill>().Supply += _SetCastSkill;
            Transponder.Query<ICastSkill>().Unsupply += _ClearCastSkill;

            Transponder.Query<IBattleSkill>().Supply += _SetBattleSkill;
            Transponder.Query<IBattleSkill>().Unsupply += _ClearBattleSkill;

            

        }

        private void _Unbind()
        {

            Transponder.Query<IEmotion>().Supply -= _SetEmotion;
            Transponder.Query<IEmotion>().Unsupply -= _ClearEmotion;
            Transponder.Query<IMoveController>().Supply -= _SetMoveController;
            Transponder.Query<IMoveController>().Unsupply -= _ClearMoveController;
            Transponder.Query<IVisible>().Supply -= _SetIndividual;
            Transponder.Query<IVisible>().Unsupply -= _ClearIndividual;

            Transponder.Query<INormalSkill>().Supply -= _SetNormalSkill;
            Transponder.Query<INormalSkill>().Unsupply -= _ClearNormalSkill;

            Transponder.Query<ICastSkill>().Supply -= _SetCastSkill;
            Transponder.Query<ICastSkill>().Unsupply -= _ClearCastSkill;

            Transponder.Query<IBattleSkill>().Supply -= _SetBattleSkill;
            Transponder.Query<IBattleSkill>().Unsupply -= _ClearBattleSkill;

            Transponder.Query<IMakeSkill>().Supply -= _SetMake;
            Transponder.Query<IMakeSkill>().Unsupply -= _ClearMake;

            Transponder.Query<IBagNotifier>().Supply -= _SetBag;
            Transponder.Query<IBagNotifier>().Unsupply -= _ClearBag;

            Transponder.Query<IEquipmentNotifier>().Supply -= _SetEquipment;
            Transponder.Query<IEquipmentNotifier>().Unsupply -= _ClearEquipment;


            Transponder.Query<IInventoryController>().Supply -= _SetInventoryController;
            Transponder.Query<IInventoryController>().Unsupply -= _ClearInventoryController;
        }

        private void _ClearInventoryController(IInventoryController obj)
        {
            _InventoryController.BagItemsEvent -= _RefreshBag;
            _InventoryController.EquipItemsEvent -= _RefreshEquip;
            _InventoryController = null;
            
        }

        private void _SetInventoryController(IInventoryController obj)
        {
            _InventoryController = obj;
            _InventoryController.BagItemsEvent += _RefreshBag;
            _InventoryController.EquipItemsEvent += _RefreshEquip;
            _InventoryController.Refresh();
        }

        private void _RefreshEquip(Item[] obj)
        {
            _Equipment.Refresh(obj);
        }

        private void _RefreshBag(Item[] obj)
        {
            _Bag.Refresh(obj);
        }

        private void _ClearEquipment(IEquipmentNotifier obj)
        {
            _Equipment.Clear();
        }

        private void _SetEquipment(IEquipmentNotifier obj)
        {
            _Equipment.Set(obj);
            if (_InventoryController != null)
                _InventoryController.Refresh();
        }

        private void _ClearBag(IBagNotifier obj)
        {
            _Bag.Clear();

        }

        private void _SetBag(IBagNotifier obj)
        {
            _Bag.Set(obj);

            if(_InventoryController != null)
                _InventoryController.Refresh();
            
        }
        

        private void _ClearMake(IMakeSkill obj)
        {
            _MakeSkill.FormulasEvent -= _SetFormulas;
            _MakeSkill = null;
            _Formulas = null;
        }

        private void _SetMake(IMakeSkill obj)
        {
            _MakeSkill = obj;
            _MakeSkill.FormulasEvent += _SetFormulas;
            _MakeSkill.QueryFormula();
        }

        private void _SetFormulas(ItemFormulaLite[] obj)
        {
            _Formulas = obj;
        }

        private void _ClearBattleSkill(IBattleSkill obj)
        {
            _BattleSkill = null;
        }

        private void _SetBattleSkill(IBattleSkill obj)
        {
            _BattleSkill = obj;
        }

        private void _ClearCastSkill(ICastSkill obj)
        {
            _CastSkill.HitNextsEvent -= _AddSkills;
            _CastSkill = null;

            _UsableSkills.Clear();
        }

        private void _SetCastSkill(ICastSkill obj)
        {
            _CastSkill = obj;
            _CastSkill.HitNextsEvent += _AddSkills;
            _UsableSkills.AddRange(_CastSkill.Skills.Value);
        }

        private void _AddSkills(ACTOR_STATUS_TYPE[] obj)
        {
            _UsableSkills.AddRange(obj);
        }

        private void _ClearNormalSkill(INormalSkill obj)
        {
            _NormalSkill = null;
        }

        private void _SetNormalSkill(INormalSkill obj)
        {
            _NormalSkill = obj;
        }

        private void _ClearEmotion(IEmotion obj)
        {
            _Emotion = null;
        }
        private void _SetEmotion(IEmotion obj)
        {
            _Emotion = obj;
        }

        private void _ClearIndividual(IVisible obj)
        {

            _DebugLeftCount ++;
            _FieldOfVision.RemoveAll(
                i =>
                {
                    if (i.Id == obj.Id)
                    {
                        _DebugRealLeftCount ++;
                        obj.TalkMessageEvent -= _TalkMessage;
                        return true;
                    }
                    return false;
                }
            );
        }

        

        private void _SetIndividual(IVisible obj)
        {

            _DebugJoinCount++;
            if (_FieldOfVision.FindAll((v) => v.Id == obj.Id).Any())
            {
                _DebugRejoinCount++;
                return;
            }
            _FieldOfVision.Add(obj);
            obj.TalkMessageEvent += _TalkMessage;
        }

        
        private void _TalkMessage(string message)
        {
            _Messages.Enqueue(message);
        }

        private void _ClearMoveController(IMoveController obj)
        {
            _MoveController = null;

        }

        private void _SetMoveController(IMoveController obj)
        {
            _MoveController = obj;

        }

        public struct Hit
        {
            public float Distance { get; set; }

            public IVisible Visible { get; set; }

            public Vector2 HitPoint { get; set; }
        }
        private Hit _Detect(float scan_angle, float max_distance)
        {
            scan_angle += 360f;
            scan_angle %= 360f;
            var pos = _Entiry.GetPosition();
            var view = Vector2.AngleToVector(scan_angle);
            //Unity Debug Code
#if UNITY_EDITOR
            UnityEngine.Debug.DrawRay(new UnityEngine.Vector3(pos.X, 0, pos.Y), new UnityEngine.Vector3(view.X, 0, view.Y), UnityEngine.Color.blue, 0.5f);
#endif

            var hit = new Hit();
            hit.HitPoint = pos + view * max_distance;
            hit.Distance = max_distance;
            foreach (var visible in _FieldOfVision)
            {
                var mesh = StandardBehavior._BuildMesh(visible);
                float dis;
                Vector2 normal;
                Vector2 point;
                if (RayPolygonIntersect(pos, view, mesh.Points, out dis, out point, out normal))
                {
                    if (dis < hit.Distance)
                    {
                        hit.Visible = visible;
                        hit.HitPoint = point;
                        hit.Distance = dis;
                    }
                }
            }


            return hit;
        }

        private static Polygon _BuildMesh(IVisible visible)
        {
            var data = Resource.Instance.FindEntity(visible.EntityType);

            var mesh = data.Mesh.Clone();

            mesh.Offset(visible.Position);
            if (data.CollisionRotation)
            {
                mesh.RotationByDegree(visible.Direction);
            }
            return mesh;
        }

        private float _GetOffsetDirection(float angle)
        {
            var x = Math.PI * _ScanCycle / _DecisionTime;
            var y = (float)Math.Sin(x);
            return angle * y - (angle / 2);
        }

        protected override void _Update(float delta)
        {
            _FPS.Update();
            _ScanCycle += delta;
            _ScanCycle %= _DecisionTime;

            _ActorMind.Forget(delta);
        }


        protected override void _Shutdown()
        {
            
            _ActorMind.Release();
            _Entiry.GetVisible().StatusEvent -= _OnStatus;
            _Entiry.CollideTargets.JoinEvent -= _HaveCollide;

            _Entiry.InjuredEvent -= _OnInjured;
            _Unbind();            
        }

        private void _OnStatus(VisibleStatus obj)
        {
            
        }

        private void _HaveCollide(IEnumerable<IIndividual> instances)
        {
            _IsCollide = true;
        }

        public static bool RayPolygonIntersect(Vector2 ro, Vector2 rd, Vector2[] polygon, out float t, out Vector2 point, out Vector2 normal)
        {
            t = float.MaxValue;
            point = ro;
            normal = rd;

            // Comparison variables.
            float distance;
            int crossings = 0;

            for (int j = polygon.Length - 1, i = 0; i < polygon.Length; j = i, i++)
            {
                if (RayLineIntersect(ro, rd, polygon[j], polygon[i], out distance))
                {
                    crossings++;

                    // If new intersection is closer, update variables.
                    if (distance < t)
                    {
                        t = distance;
                        point = ro + (rd * t);

                        Vector2 edge = polygon[j] - polygon[i];
                        normal = new Vector2(-edge.Y, edge.X).GetNormalized();
                    }
                }
            }

            return crossings > 0 && crossings % 2 == 0;
        }
        public static bool RayLineIntersect(Vector2 ro, Vector2 rd, Vector2 l1, Vector2 l2, out float t)
        {
            Vector2 seg = l2 - l1;
            Vector2 segPerp = new Vector2(seg.Y, -seg.X);


            float perpDotd = rd.DotProduct(segPerp);

            // If lines are parallel, return false.
            if (Math.Abs(perpDotd) <= float.Epsilon)
            {
                t = float.MaxValue;
                return false;
            }

            Vector2 d = l1 - ro;


            t = segPerp.DotProduct(d) / perpDotd;
            float s = new Vector2(rd.Y, -rd.X).DotProduct(d) / perpDotd;

            // If intersect is in right direction and in segment bounds, return true.
            return t >= 0.0f && s >= 0.0f && s <= 1.0f;
        }

        

        public bool IsWall(ENTITY entity_type)
        {
            return EntityData.IsWall(entity_type);
        }

        public Hit Detect(float f, float view)
        {
            return _Detect(f , view);
        }

        public void MoveLeft()
        {
            if(_MoveController != null)
                _MoveController.TrunLeft();
        }

        public void MoveRight()
        {
            if (_MoveController != null)
                _MoveController.TrunRight();
        }

        public void StopTrun()
        {
            if (_MoveController != null)
                _MoveController.StopTrun();
        }

        public float GetTrunSpeed()
        {
            if (_MoveController != null)
            {
                var status = _Entiry.GetStatus();
                var skill = Resource.Instance.FindSkill(status);
                var caster = new SkillCaster(skill, new Determination(skill));

                return caster.GetTurnRight();
            }

            return 0;
        }

        public TICKRESULT StopMove(float delta)
        {
            return _StopMove(delta);
        }
    }

    internal struct Exit
    {
        public float Direction { get; set; }

        public float Distance
        { get; set; }
        
    }


}