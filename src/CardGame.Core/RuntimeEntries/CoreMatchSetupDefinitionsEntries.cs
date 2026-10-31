#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed partial class CoreMatchSetupDefinitionsEntries
{
    private readonly Func<EngineOwners.CoreMatchSetupDefinitionsRuntime> _resolve;
    private CoreMatchSetupDefinitionsEntries(Func<EngineOwners.CoreMatchSetupDefinitionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchSetupDefinitionsEntries Create(Func<EngineOwners.CoreMatchSetupDefinitionsRuntime> resolve) => new(resolve);
}
