#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime))]
internal sealed partial class CoreGameEngineTargetedOrdinaryTrickEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> _resolve;
    private CoreGameEngineTargetedOrdinaryTrickEntries(Func<EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetedOrdinaryTrickEntries Create(Func<EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> resolve) => new(resolve);
}
