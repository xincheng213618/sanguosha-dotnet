#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime))]
internal sealed partial class CoreGameEngineRoundGainedSourceUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> _resolve;
    private CoreGameEngineRoundGainedSourceUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundGainedSourceUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> resolve) => new(resolve);
}
