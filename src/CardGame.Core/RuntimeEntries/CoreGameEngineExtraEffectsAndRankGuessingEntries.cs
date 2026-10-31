#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime> _resolve;
    private CoreGameEngineExtraEffectsAndRankGuessingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraEffectsAndRankGuessingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime> resolve) => new(resolve);
}
