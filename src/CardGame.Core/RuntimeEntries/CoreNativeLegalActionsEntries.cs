#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeLegalActionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _resolve;
    private CoreNativeLegalActionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeLegalActionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> resolve) => new(resolve);
}
