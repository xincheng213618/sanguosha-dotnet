#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime))]
internal sealed partial class CoreGameEngineRoundGainedTrickTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime> _resolve;
    private CoreGameEngineRoundGainedTrickTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundGainedTrickTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime> resolve) => new(resolve);
}
