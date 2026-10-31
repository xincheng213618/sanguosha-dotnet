#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime))]
internal sealed partial class CoreGameEngineWholeActualTurnUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> _resolve;
    private CoreGameEngineWholeActualTurnUsesEntries(Func<EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWholeActualTurnUsesEntries Create(Func<EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> resolve) => new(resolve);
}
