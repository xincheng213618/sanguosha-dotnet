#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed partial class CoreGameEngineUnrespondableCounterspellsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _resolve;
    private CoreGameEngineUnrespondableCounterspellsEntries(Func<EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnrespondableCounterspellsEntries Create(Func<EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> resolve) => new(resolve);
}
