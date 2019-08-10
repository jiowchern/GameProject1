

using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using System;
using System.Collections.Generic;
namespace Regulus.Project.GameProject1.Game
{
    public class DummyFrature :  IStorage
    {
        private readonly List<Account> _Accounts;

        private readonly List<GamePlayerRecord> _Records;

        public DummyFrature()
        {
            this._Records = new List<GamePlayerRecord>();

            this._Accounts = new List<Account>
			{
				new Account
				{
					Id = Guid.NewGuid(), 
					Password = "pw", 
					Name = "name", 
					Competnces = Account.AllCompetnce()
				}, 
				new Account
				{
					Id = Guid.NewGuid(), 
					Password = "20150815", 
					Name = "itisnotagame", 
					Competnces = Account.AllCompetnce()
				}, 
				new Account
				{
					Id = Guid.NewGuid(), 
					Password = "user", 
					Name = "user1", 
					Competnces = Account.AllCompetnce()
				},
                new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "1",
                    Competnces = Account.AllCompetnce()
                },
                new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "2",
                    Competnces = Account.AllCompetnce()
                },
                new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "3",
                    Competnces = Account.AllCompetnce()
                },
                new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "4",
                    Competnces = Account.AllCompetnce()
                },
                new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "5",
                    Competnces = Account.AllCompetnce()
                },
                new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "6",
                    Competnces = Account.AllCompetnce()
                },
                new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "7",
                    Competnces = Account.AllCompetnce()
                },new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "8",
                    Competnces = Account.AllCompetnce()
                },new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "9",
                    Competnces = Account.AllCompetnce()
                },new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "10",
                    Competnces = Account.AllCompetnce()
                },new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "11",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "12",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "13",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "14",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "15",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "16",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "17",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "18",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "19",
                    Competnces = Account.AllCompetnce()
                }
                ,new Account
                {
                    Id = Guid.NewGuid(),
                    Password = "1",
                    Name = "20",
                    Competnces = Account.AllCompetnce()
                }



            };
        }

        Value<Account> IAccountFinder.FindAccountByName(string id)
        {
            return this._Accounts.Find(a => a.Name == id);
        }

        Value<Account> IAccountFinder.FindAccountById(Guid account_id)
        {
            return this._Accounts.Find(a => a.Id == account_id);
        }

        

        Value<Account[]> IAccountManager.QueryAllAccount()
        {
            return this._Accounts.ToArray();
        }

        

        Value<ACCOUNT_REQUEST_RESULT> IAccountManager.Delete(string account)
        {
            this._Accounts.RemoveAll(a => a.Name == account);
            return ACCOUNT_REQUEST_RESULT.OK;
        }

        Value<ACCOUNT_REQUEST_RESULT> IAccountManager.Update(Account account)
        {
            if (this._Accounts.RemoveAll(a => a.Id == account.Id) > 0)
            {
                this._Accounts.Add(account);
                return ACCOUNT_REQUEST_RESULT.OK;
            }

            return ACCOUNT_REQUEST_RESULT.NOTFOUND;
        }

        Value<GamePlayerRecord> IGameRecorder.Load(Guid account_id)
        {
            var account = this._Accounts.Find(a => a.Id == account_id);
            if (account.IsPlayer())
            {
                var record = this._Records.Find(r => r.Owner == account.Id);
                if (record == null)
                {
                    record = new GamePlayerRecord
                    {
                        Id = Guid.NewGuid(),
        
                        Owner = account_id,
                        Items = new Item[]
                        {                            
                        }

                        
                    };
                }

                return record;
            }

            return null;
        }

        void IGameRecorder.Save(GamePlayerRecord game_player_record)
        {
            var account = this._Accounts.Find(a => a.Id == game_player_record.Owner);
            if (account.IsPlayer())
            {
                var old = this._Records.Find(r => r.Owner == account.Id);
                this._Records.Remove(old);
                this._Records.Add(game_player_record);
            }
        }



        Value<ACCOUNT_REQUEST_RESULT> IAccountManager.Create(Account account)
        {
            this._Accounts.Add(account);
            return ACCOUNT_REQUEST_RESULT.OK;
        }
    }
}