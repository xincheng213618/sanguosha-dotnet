#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseGiftBasicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseGiftBasicRuntime> _resolve;
    private CoreGameEnginePhaseGiftBasicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseGiftBasicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseGiftBasicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseGiftBasicRuntime> resolve) => new(resolve);
}
