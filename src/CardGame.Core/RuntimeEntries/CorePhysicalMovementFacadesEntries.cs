#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed partial class CorePhysicalMovementFacadesEntries
{
    private readonly Func<EngineOwners.CorePhysicalMovementFacadesRuntime> _resolve;
    private CorePhysicalMovementFacadesEntries(Func<EngineOwners.CorePhysicalMovementFacadesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePhysicalMovementFacadesEntries Create(Func<EngineOwners.CorePhysicalMovementFacadesRuntime> resolve) => new(resolve);
}
