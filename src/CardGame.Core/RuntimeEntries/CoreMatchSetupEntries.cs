#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime))]
internal sealed partial class CoreMatchSetupEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime> _resolve;
    private CoreMatchSetupEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchSetupEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime> resolve) => new(resolve);
}
