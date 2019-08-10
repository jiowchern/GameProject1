using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.GameProject1.Data
{
    public interface IJumpMap
    {
        string Realm { get; }
        void Ready();
    }
}
