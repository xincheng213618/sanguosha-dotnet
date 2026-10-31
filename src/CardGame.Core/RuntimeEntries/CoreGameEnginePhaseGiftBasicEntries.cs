#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseGiftBasicEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseGiftBasicRuntime> _resolve;
    private CoreGameEnginePhaseGiftBasicEntries(Func<EngineOwners.CoreGameEnginePhaseGiftBasicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseGiftBasicEntries Create(Func<EngineOwners.CoreGameEnginePhaseGiftBasicRuntime> resolve) => new(resolve);
}
