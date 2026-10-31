#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNianChallengeHostRuntime))]
internal sealed partial class CoreGameEngineNianChallengeHostEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNianChallengeHostRuntime> _resolve;
    private CoreGameEngineNianChallengeHostEntries(Func<EngineOwners.CoreGameEngineNianChallengeHostRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNianChallengeHostEntries Create(Func<EngineOwners.CoreGameEngineNianChallengeHostRuntime> resolve) => new(resolve);
}
