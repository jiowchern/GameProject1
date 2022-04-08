
using System;

using NSubstitute;

using Regulus.Utility;
using Regulus.Project.GameProject1.Game.Play;
using Regulus.Utility;
using Regulus.Project.GameProject1.Data;
using NUnit.Framework;

namespace Regulus.Project.GameProject1.Test
{
    
    public class UnitTest
    {
        [NUnit.Framework.Test]
        public void TestBuildItemEffect()
        {
            var ip = new ItemProvider();
            var item = new Item() ;
            var effects = new ItemEffect[]
            {
                new ItemEffect() { Quality = 100 , Effects = new []
                {
                    new Effect() { Type = EFFECT_TYPE.ATTACK_ADD , Value = 10},
                    new Effect() { Type = EFFECT_TYPE.ATTACK_ADD , Value = 1}
                }},
                new ItemEffect() { Quality = 80 , Effects = new []
                {
                    new Effect() { Type = EFFECT_TYPE.ATTACK_ADD , Value = 8},
                    new Effect() { Type = EFFECT_TYPE.ATTACK_ADD , Value = 2}
                }},
                new ItemEffect() { Quality = 0 , Effects = new []
                {
                    new Effect() { Type = EFFECT_TYPE.ATTACK_ADD , Value = 7},
                    new Effect() { Type = EFFECT_TYPE.ATTACK_ADD , Value = 3}
                }}

            };
            item = ip.BuildItem(80, item, effects);

            Assert.AreEqual(EFFECT_TYPE.ATTACK_ADD , item.Effects[0].Type );
            Assert.AreEqual(20, item.Effects[0].Value);
        }
        [NUnit.Framework.Test]

        public void TestExtendingPolygon()
        {
            var polygon = new Polygon( new []
            {
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(-1,0),
                new Vector2(0,-1),
            });

            var extendPolygon = new ExtendPolygon(polygon , 1.0f);
            var result = extendPolygon.Result;

            Assert.AreEqual(new Vector2(0, 2), result.Points[0] );
            Assert.AreEqual(new Vector2(2,0), result.Points[1] );
            Assert.AreEqual(new Vector2(-2,0),result.Points[2] );
            Assert.AreEqual(new Vector2(0,-2),result.Points[3] );
        }
        [NUnit.Framework.Test]

        public void TestInventoryRemove()
        {
            var inventory = new Bag();
            Item item1 = new Item();
            item1.Id = Guid.NewGuid();
            item1.Weight = 10;
            item1.Effects = new Effect[0];
            inventory.Add(item1);
            
            Item item2 = new Item();
            item2.Id = Guid.NewGuid();
            item2.Weight = 10;
            item2.Effects = new Effect[0];
            inventory.Add(item2);

            inventory.Remove(item1.Id);

            var weight = inventory.GetWeight();

            Assert.AreEqual(10, weight);
        }

        [NUnit.Framework.Test]

        public void TestInventoryGetSum1()
        {
            var inventory = new Bag();
            Item item1 = new Item();
            item1.Id = Guid.NewGuid();
            item1.Name = "a";
            item1.Weight = 10;
            item1.Count = 9;
            item1.Effects = new Effect[0];
            inventory.Add(item1);

            Item item2 = new Item();
            item2.Id = Guid.NewGuid();
            item2.Name = "a";
            item2.Weight = 10;
            item2.Count = 11;
            item2.Effects = new Effect[0];
            inventory.Add(item2);

            var amount = inventory.GetItemAmount("a");

            Assert.AreEqual(20, amount);
        }

        [NUnit.Framework.Test]
        public void TestInventoryGetSum2()
        {
            var inventory = new Bag();
            Item item1 = new Item();
            item1.Id = Guid.NewGuid();
            item1.Name = "a";
            item1.Weight = 10;
            item1.Count = 9;
            item1.Effects = new Effect[0];
            inventory.Add(item1);

            Item item2 = new Item();
            item2.Id = Guid.NewGuid();
            item2.Name = "a";
            item2.Weight = 10;
            item2.Count = 11;
            item2.Effects = new Effect[0];
            inventory.Add(item2);

            inventory.Remove("a", 10);

            var amount = inventory.GetItemAmount("a");

            Assert.AreEqual(10, amount);
        }

        [NUnit.Framework.Test]
        public void TestSkillDataSer()
        {
            
            var datas = new SkillData[]
            {
                new SkillData()
                {
                    Roots    = new Translate[]
                    {
                        new Translate() { Position = new Vector2(111,222) , Rotation = 123 },
                        new Translate() { Position = new Vector2(111,222) , Rotation = 123 }
                    }
                }   
            };
            

            Regulus.Utility.Serialization.Write<SkillData[]>(datas , "123.txt");
            var result1 = Regulus.Utility.Serialization.WriteXml<SkillData[]>(datas);

        }
        [NUnit.Framework.Test]
        public void TestInventoryGetSum3()
        {
            var inventory = new Bag();
            Item item1 = new Item();
            item1.Id = Guid.NewGuid();
            item1.Name = "a";
            item1.Weight = 10;
            item1.Count = 1;
            item1.Effects = new Effect[] { new Effect() { Type = EFFECT_TYPE.ATTACK_ADD } };
            inventory.Add(item1);

            Item item2 = new Item();
            item2.Id = Guid.NewGuid();
            item2.Name = "a";
            item2.Weight = 10;
            item2.Count = 1;
            item2.Effects = new Effect[] { new Effect() { Type = EFFECT_TYPE.ATTACK_ADD } };
            inventory.Add(item2);

            var amount = inventory.GetItemAmount("a");

            Assert.AreEqual(2, amount);
        }

        [NUnit.Framework.Test]
        public void TestInventoryGetSum4()
        {
            var inventory = new Bag();
            Item item1 = new Item();
            item1.Id = Guid.NewGuid();
            item1.Name = "a";
            item1.Weight = 10;
            item1.Count = 1;
            item1.Effects = new Effect[] { new Effect() { Type = EFFECT_TYPE.ATTACK_ADD } };
            inventory.Add(item1);

            Item item2 = new Item();
            item2.Id = Guid.NewGuid();
            item2.Name = "a";
            item2.Weight = 10;
            item2.Count = 1;
            item2.Effects = new Effect[] { new Effect() { Type = EFFECT_TYPE.ATTACK_ADD } };
            inventory.Add(item2);

            inventory.Remove("a", 1);

            var amount = inventory.GetItemAmount("a");

            Assert.AreEqual(1, amount);
        }

        [NUnit.Framework.Test]
        public void TestInventoryGetSum5()
        {
            var inventory = new Bag();
            Item item1 = new Item();
            item1.Id = Guid.NewGuid();
            item1.Name = "a";
            item1.Weight = 10;
            item1.Count = 1;
            item1.Effects = new Effect[] { new Effect() { Type = EFFECT_TYPE.ATTACK_ADD } };
            inventory.Add(item1);

            Item item2 = new Item();
            item2.Id = Guid.NewGuid();
            item2.Name = "a";
            item2.Weight = 10;
            item2.Count = 1;
            item2.Effects = new Effect[] { new Effect() { Type = EFFECT_TYPE.ATTACK_ADD } };
            inventory.Add(item2);

            inventory.Remove("a", 2);

            var amount = inventory.GetItemAmount("a");

            Assert.AreEqual(0, amount);
        }


        [NUnit.Framework.Test]
        public void TestInventoryGetSum6()
        {
            var inventory = new Bag();
            Item item1 = new Item();
            item1.Id = Guid.NewGuid();
            item1.Name = "a";
            item1.Weight = 10;
            item1.Count = 10;
            item1.Effects = new Effect[] { };
            inventory.Add(item1);

            Item item2 = new Item();
            item2.Id = Guid.NewGuid();
            item2.Name = "a";
            item2.Weight = 10;
            item2.Count = 10;
            item2.Effects = new Effect[] {  };
            inventory.Add(item2);

            inventory.Remove("a", 15);

            var amount = inventory.GetItemAmount("a");

            Assert.AreEqual(5, amount);
        }


        [NUnit.Framework.Test]
        public void TestInventoryWeight1()
        {
            var inventory = new Bag();
            Item item1 = new Item();
            item1.Weight = 10;
            item1.Effects = new Effect[0];
            inventory.Add(item1);
            Item item2 = new Item();

            item2.Weight = 10;
            item2.Effects = new Effect[0];
            inventory.Add(item2);
            var weight = inventory.GetWeight();

            Assert.AreEqual(20 , weight);
        }
        

        [NUnit.Framework.Test]
        public void TestCenter()
        {
            var binder = Substitute.For<Remote.IBinder>();
            var finder = Substitute.For<IAccountFinder>();
            var record = Substitute.For<IGameRecorder>();

            var center = new Center(finder, record);

            Updater updater = new Updater();
            center.Join(binder);
            updater.Add(center);
            updater.Working();
            updater.Shutdown();
        }

        [NUnit.Framework.Test]
        public void TestMapFind()
        {
            Map map = new Map();

            Rect rect = new Rect(0.5f ,0.5f,1,10);

            Polygon meshs1 = new Polygon(new[]{
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 2),
                new Vector2(1, 2)
            });
            Polygon meshs2 = new Polygon(new[]{
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 2),
                new Vector2(1, 2)
            });
            Polygon meshs3 = new Polygon(new[]{
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 2),
                new Vector2(1, 2)
            });
            Polygon meshs4 = new Polygon(new[]{
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 2),
                new Vector2(1, 2)
            });
            Polygon meshs5 = new Polygon(new[]{
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 2),
                new Vector2(1, 2)
            });
           
            meshs2.Offset(0,2);
            
            meshs3.Offset(0, 4);
            
            meshs4.Offset(0, 6);
            
            meshs5.Offset(0, 8);

            IIndividual[] visables = {
                new Entity(meshs1), 
                new Entity(meshs2), 
                new Entity(meshs3), 
                new Entity(meshs4), 
                new Entity(meshs5)
            };

            foreach (var visable in visables)
            {
                map.JoinStaff(visable);
            }
            IMapFinder finder = map;
            var results = finder.Find(rect);

            foreach (var visable in visables)
            {
                map.Left(visable);
            }

            Assert.AreEqual(5, results.Length);

        }

        
    }

    
}

