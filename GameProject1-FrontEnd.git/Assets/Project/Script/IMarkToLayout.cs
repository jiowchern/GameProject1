using System.Collections.Generic;

public interface IMarkToLayout<TLayout>
{
    IEnumerable<TLayout> ToLayouts();
}