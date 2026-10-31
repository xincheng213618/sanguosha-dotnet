#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime))]
internal sealed partial class CoreGameEngineNextCardTargetAdjustmentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> _resolve;
    private CoreGameEngineNextCardTargetAdjustmentsEntries(Func<EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextCardTargetAdjustmentsEntries Create(Func<EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> resolve) => new(resolve);
}
