#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed partial class CorePhysicalDrawFacadesEntries
{
    private readonly Func<EngineOwners.CorePhysicalDrawFacadesRuntime> _resolve;
    private CorePhysicalDrawFacadesEntries(Func<EngineOwners.CorePhysicalDrawFacadesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePhysicalDrawFacadesEntries Create(Func<EngineOwners.CorePhysicalDrawFacadesRuntime> resolve) => new(resolve);
}
