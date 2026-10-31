#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNianChallengeHostEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime> _resolve;
    private CoreGameEngineNianChallengeHostEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNianChallengeHostEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime> resolve) => new(resolve);
}
