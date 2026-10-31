#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed partial class CoreNativeLegalActionsEntries
{
    private readonly Func<EngineOwners.CoreNativeLegalActionsRuntime> _resolve;
    private CoreNativeLegalActionsEntries(Func<EngineOwners.CoreNativeLegalActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeLegalActionsEntries Create(Func<EngineOwners.CoreNativeLegalActionsRuntime> resolve) => new(resolve);
}
