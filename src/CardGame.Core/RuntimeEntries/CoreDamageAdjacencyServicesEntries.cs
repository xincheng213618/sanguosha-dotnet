#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime))]
internal sealed partial class CoreDamageAdjacencyServicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> _resolve;
    private CoreDamageAdjacencyServicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDamageAdjacencyServicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> resolve) => new(resolve);
}
