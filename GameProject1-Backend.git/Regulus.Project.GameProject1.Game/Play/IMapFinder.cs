using Regulus.CustomType;

namespace Regulus.Project.GameProject1.Game.Play
{
    public interface IMapFinder
    {
        IIndividual[] Find(Rect bound);
    }
}