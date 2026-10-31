#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDiscardChallengesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime> _resolve;
    private CoreGameEngineProgramDiscardChallengesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDiscardChallengesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime> resolve) => new(resolve);
}
