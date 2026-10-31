#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengesRuntime))]
internal sealed partial class CoreGameEngineAllHandChallengesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengesRuntime> _resolve;
    private CoreGameEngineAllHandChallengesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAllHandChallengesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengesRuntime> resolve) => new(resolve);
}
