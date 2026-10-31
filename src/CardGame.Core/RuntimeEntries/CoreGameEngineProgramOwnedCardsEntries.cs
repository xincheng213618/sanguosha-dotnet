#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramOwnedCardsRuntime))]
internal sealed partial class CoreGameEngineProgramOwnedCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> _resolve;
    private CoreGameEngineProgramOwnedCardsEntries(Func<EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnedCardsEntries Create(Func<EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> resolve) => new(resolve);
}
