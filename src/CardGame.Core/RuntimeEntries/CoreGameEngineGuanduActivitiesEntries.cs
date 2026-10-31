#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduActivitiesRuntime))]
internal sealed partial class CoreGameEngineGuanduActivitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduActivitiesRuntime> _resolve;
    private CoreGameEngineGuanduActivitiesEntries(Func<EngineOwners.CoreGameEngineGuanduActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduActivitiesEntries Create(Func<EngineOwners.CoreGameEngineGuanduActivitiesRuntime> resolve) => new(resolve);
}
