using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.GameProject1.Data
{
    public interface IPlayerProperys
    {

        Regulus.Remote.Property<string> Realm { get; }
        Regulus.Remote.Property<Guid> Id { get; }

        Regulus.Remote.Property<float> Strength { get; }

        Regulus.Remote.Property<float> Health { get; }
    }
}
