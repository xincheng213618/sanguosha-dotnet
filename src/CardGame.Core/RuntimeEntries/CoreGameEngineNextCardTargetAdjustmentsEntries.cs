#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime))]
internal sealed partial class CoreGameEngineNextCardTargetAdjustmentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> _resolve;
    private CoreGameEngineNextCardTargetAdjustmentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextCardTargetAdjustmentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> resolve) => new(resolve);
}
