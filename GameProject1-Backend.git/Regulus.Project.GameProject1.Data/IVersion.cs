using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.GameProject1.Data
{
    public interface IVersion
    {
        Regulus.Remote.Property<string> Number { get; }
    }
}
