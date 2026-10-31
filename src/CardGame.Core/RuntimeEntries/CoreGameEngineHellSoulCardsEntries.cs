#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed partial class CoreGameEngineHellSoulCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _resolve;
    private CoreGameEngineHellSoulCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHellSoulCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> resolve) => new(resolve);
}
