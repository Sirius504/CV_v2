using System.Collections.Generic;
using System.Linq;

public class Initializer : SystemBase<Initializer, IInitializable>
{
    public override SystemsStartOrder ResolutionOrder => SystemsStartOrder.Initializer;
    public InitOrder InitOrder => InitOrder.Initializer;


    protected override void RegisterMany(IEnumerable<IInitializable> initializables)
    {
        if (!initializables.Any())
        {
            return;
        }
        foreach (var request in initializables.OrderBy(req => req.InitOrder))
        {
            request.Init();
        }
    }
}