#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed partial class CoreGameEngineNextActualUseTargetAdjustmentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _resolve;
    private CoreGameEngineNextActualUseTargetAdjustmentsEntries(Func<EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextActualUseTargetAdjustmentsEntries Create(Func<EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> resolve) => new(resolve);
}
