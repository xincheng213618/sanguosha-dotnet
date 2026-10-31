#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed partial class CoreNativeLegalActionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _resolve;
    private CoreNativeLegalActionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeLegalActionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> resolve) => new(resolve);
}
