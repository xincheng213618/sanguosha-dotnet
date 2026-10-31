#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime))]
internal sealed partial class CoreGameEngineProgramOwnedCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> _resolve;
    private CoreGameEngineProgramOwnedCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnedCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> resolve) => new(resolve);
}
