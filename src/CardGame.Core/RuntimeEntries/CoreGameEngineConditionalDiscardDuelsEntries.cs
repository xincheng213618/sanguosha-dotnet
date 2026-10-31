#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineConditionalDiscardDuelsRuntime))]
internal sealed partial class CoreGameEngineConditionalDiscardDuelsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineConditionalDiscardDuelsRuntime> _resolve;
    private CoreGameEngineConditionalDiscardDuelsEntries(Func<EngineOwners.CoreGameEngineConditionalDiscardDuelsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConditionalDiscardDuelsEntries Create(Func<EngineOwners.CoreGameEngineConditionalDiscardDuelsRuntime> resolve) => new(resolve);
}
