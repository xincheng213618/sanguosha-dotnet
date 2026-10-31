#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAllHandChallengesRuntime))]
internal sealed partial class CoreGameEngineAllHandChallengesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAllHandChallengesRuntime> _resolve;
    private CoreGameEngineAllHandChallengesEntries(Func<EngineOwners.CoreGameEngineAllHandChallengesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAllHandChallengesEntries Create(Func<EngineOwners.CoreGameEngineAllHandChallengesRuntime> resolve) => new(resolve);
}
