#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime))]
internal sealed partial class CoreGameEngineRoundGainedSourceUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> _resolve;
    private CoreGameEngineRoundGainedSourceUsesEntries(Func<EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundGainedSourceUsesEntries Create(Func<EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> resolve) => new(resolve);
}
