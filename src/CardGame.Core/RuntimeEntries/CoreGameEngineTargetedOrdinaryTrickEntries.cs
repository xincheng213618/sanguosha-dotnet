#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime))]
internal sealed partial class CoreGameEngineTargetedOrdinaryTrickEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> _resolve;
    private CoreGameEngineTargetedOrdinaryTrickEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetedOrdinaryTrickEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> resolve) => new(resolve);
}
