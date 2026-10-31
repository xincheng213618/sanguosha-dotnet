#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime))]
internal sealed partial class CoreGameEngineRoundGainedTrickTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime> _resolve;
    private CoreGameEngineRoundGainedTrickTargetsEntries(Func<EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundGainedTrickTargetsEntries Create(Func<EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime> resolve) => new(resolve);
}
