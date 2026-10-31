#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreMatchSetupRuntime))]
internal sealed partial class CoreMatchSetupEntries
{
    private readonly Func<EngineOwners.CoreMatchSetupRuntime> _resolve;
    private CoreMatchSetupEntries(Func<EngineOwners.CoreMatchSetupRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchSetupEntries Create(Func<EngineOwners.CoreMatchSetupRuntime> resolve) => new(resolve);
}
