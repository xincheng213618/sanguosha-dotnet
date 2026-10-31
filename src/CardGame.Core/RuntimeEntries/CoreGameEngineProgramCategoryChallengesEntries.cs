#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCategoryChallengesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryChallengesRuntime> _resolve;
    private CoreGameEngineProgramCategoryChallengesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryChallengesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCategoryChallengesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryChallengesRuntime> resolve) => new(resolve);
}
