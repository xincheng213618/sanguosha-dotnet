#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextActualUseTargetAdjustmentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _resolve;
    private CoreGameEngineNextActualUseTargetAdjustmentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextActualUseTargetAdjustmentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> resolve) => new(resolve);
}
