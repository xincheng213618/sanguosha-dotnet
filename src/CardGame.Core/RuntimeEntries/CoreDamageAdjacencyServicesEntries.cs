#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreDamageAdjacencyServicesRuntime))]
internal sealed partial class CoreDamageAdjacencyServicesEntries
{
    private readonly Func<EngineOwners.CoreDamageAdjacencyServicesRuntime> _resolve;
    private CoreDamageAdjacencyServicesEntries(Func<EngineOwners.CoreDamageAdjacencyServicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDamageAdjacencyServicesEntries Create(Func<EngineOwners.CoreDamageAdjacencyServicesRuntime> resolve) => new(resolve);
}
