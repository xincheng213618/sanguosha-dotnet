#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed partial class CoreGameEngineCurrentCardEnhancementsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _resolve;
    private CoreGameEngineCurrentCardEnhancementsEntries(Func<EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentCardEnhancementsEntries Create(Func<EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> resolve) => new(resolve);
}
