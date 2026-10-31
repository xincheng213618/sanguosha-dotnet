#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed partial class CoreGameEngineUnrespondableCounterspellsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _resolve;
    private CoreGameEngineUnrespondableCounterspellsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnrespondableCounterspellsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> resolve) => new(resolve);
}
