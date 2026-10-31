#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime))]
internal sealed partial class CoreGameEngineProgramOrdinaryTrickUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> _resolve;
    private CoreGameEngineProgramOrdinaryTrickUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOrdinaryTrickUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> resolve) => new(resolve);
}
