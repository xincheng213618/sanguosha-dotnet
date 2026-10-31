#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDamageAdjacencyServicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> _resolve;
    private CoreDamageAdjacencyServicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDamageAdjacencyServicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> resolve) => new(resolve);
}
