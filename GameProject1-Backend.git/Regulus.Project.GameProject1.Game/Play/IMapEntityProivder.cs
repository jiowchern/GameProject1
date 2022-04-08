using System.Collections.Generic;

using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    public interface IMapEntityProivder
    {                
        
        IEnumerable<IIndividual> Supply(string name, Vector2 center, float direction);
    }
}