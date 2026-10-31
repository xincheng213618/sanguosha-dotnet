#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchSetupEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime> _resolve;
    private CoreMatchSetupEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchSetupEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime> resolve) => new(resolve);
}
