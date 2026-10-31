#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime))]
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessingEntries
{
    private readonly Func<EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime> _resolve;
    private CoreGameEngineExtraEffectsAndRankGuessingEntries(Func<EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraEffectsAndRankGuessingEntries Create(Func<EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime> resolve) => new(resolve);
}
