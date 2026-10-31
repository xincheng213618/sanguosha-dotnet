#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramHoldCardsRuntime))]
internal sealed partial class CoreGameEngineProgramHoldCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramHoldCardsRuntime> _resolve;
    private CoreGameEngineProgramHoldCardsEntries(Func<EngineOwners.CoreGameEngineProgramHoldCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramHoldCardsEntries Create(Func<EngineOwners.CoreGameEngineProgramHoldCardsRuntime> resolve) => new(resolve);
}
