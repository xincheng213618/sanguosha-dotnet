#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameActivationAwakeningEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationAwakeningRuntime> _resolve;
    private CoreGameEngineGameActivationAwakeningEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationAwakeningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGameActivationAwakeningEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationAwakeningRuntime> resolve) => new(resolve);
}
