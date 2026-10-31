#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed partial class CoreGameEngineWenheCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _resolve;
    private CoreGameEngineWenheCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenheCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> resolve) => new(resolve);
}
