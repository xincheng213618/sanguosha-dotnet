#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePhysicalMovementFacadesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _resolve;
    private CorePhysicalMovementFacadesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePhysicalMovementFacadesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> resolve) => new(resolve);
}
