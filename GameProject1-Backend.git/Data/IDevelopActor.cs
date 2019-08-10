using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.GameProject1.Data
{
    public interface IDevelopActor
    {

        void SetBaseView(float range);
        void SetSpeed(float speed);

        void MakeItem(string name, float quality );

        void CreateItem(string name, int count);

        void SetPosition(float x , float y);

        void SetRealm(string realm);
    }
}
