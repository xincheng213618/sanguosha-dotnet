#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime))]
internal sealed partial class CoreGameEngineNianChallengeHostEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime> _resolve;
    private CoreGameEngineNianChallengeHostEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNianChallengeHostEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime> resolve) => new(resolve);
}
