#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime))]
internal sealed partial class CoreGameEngineAllHandChallengeReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime> _resolve;
    private CoreGameEngineAllHandChallengeReceiptsEntries(Func<EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAllHandChallengeReceiptsEntries Create(Func<EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime> resolve) => new(resolve);
}
