#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeMultiTargetActionsRuntime))]
internal sealed partial class CoreNativeMultiTargetActionsEntries
{
    private readonly Func<EngineOwners.CoreNativeMultiTargetActionsRuntime> _resolve;
    private CoreNativeMultiTargetActionsEntries(Func<EngineOwners.CoreNativeMultiTargetActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeMultiTargetActionsEntries Create(Func<EngineOwners.CoreNativeMultiTargetActionsRuntime> resolve) => new(resolve);
}
