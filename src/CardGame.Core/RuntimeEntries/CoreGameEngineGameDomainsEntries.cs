#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGameDomainsRuntime))]
internal sealed partial class CoreGameEngineGameDomainsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGameDomainsRuntime> _resolve;
    private CoreGameEngineGameDomainsEntries(Func<EngineOwners.CoreGameEngineGameDomainsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGameDomainsEntries Create(Func<EngineOwners.CoreGameEngineGameDomainsRuntime> resolve) => new(resolve);
}
