using System;
using System.Linq;





using Regulus.CustomType;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Data
{

    public class Account
    {
        public enum COMPETENCE
        {
            [EnumDescription("帳號管理")]
            ACCOUNT_MANAGER,

            [EnumDescription("遊戲體驗")]
            GAME_PLAYER,

            [EnumDescription("帳號查詢")]
            ACCOUNT_FINDER
        };


        public Guid Id;


        public string Name;


        public string Password;


        public Flag<COMPETENCE> Competnces { get; set; }

        public Account()
        {
            this.Id = Guid.NewGuid();
            this.Name = this.Id.ToString();
            this.Password = this.Id.ToString(); 
        }

        public bool IsPassword(string password)
        {
            return this.Password == password;
        }


        public bool IsPlayer()
        {
            return this._HasCompetence(COMPETENCE.GAME_PLAYER);
        }
        private bool _HasCompetence(COMPETENCE competence)
        {
            return this.Competnces[competence];
        }
        public bool HasCompetnce(COMPETENCE cOMPETENCE)
        {
            return this._HasCompetence(cOMPETENCE);
        }
        public static Flag<COMPETENCE> AllCompetnce()
        {
            var flags = EnumHelper.GetEnums<COMPETENCE>().ToArray();            
            return new Flag<COMPETENCE>(flags);
        }
    }
}
