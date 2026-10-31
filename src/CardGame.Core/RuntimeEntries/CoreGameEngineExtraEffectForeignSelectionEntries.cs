#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectForeignSelectionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> _resolve;
    private CoreGameEngineExtraEffectForeignSelectionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraEffectForeignSelectionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> resolve) => new(resolve);
}
