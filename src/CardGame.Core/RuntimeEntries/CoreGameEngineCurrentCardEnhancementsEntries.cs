#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed partial class CoreGameEngineCurrentCardEnhancementsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _resolve;
    private CoreGameEngineCurrentCardEnhancementsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentCardEnhancementsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> resolve) => new(resolve);
}
