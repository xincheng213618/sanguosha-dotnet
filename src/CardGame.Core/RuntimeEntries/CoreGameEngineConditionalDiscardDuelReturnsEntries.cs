#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime))]
internal sealed partial class CoreGameEngineConditionalDiscardDuelReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime> _resolve;
    private CoreGameEngineConditionalDiscardDuelReturnsEntries(Func<EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConditionalDiscardDuelReturnsEntries Create(Func<EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime> resolve) => new(resolve);
}
