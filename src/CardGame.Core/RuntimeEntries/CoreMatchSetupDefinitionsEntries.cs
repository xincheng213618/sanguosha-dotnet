#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed partial class CoreMatchSetupDefinitionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _resolve;
    private CoreMatchSetupDefinitionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchSetupDefinitionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> resolve) => new(resolve);
}
