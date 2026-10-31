#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePhysicalDrawFacadesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _resolve;
    private CorePhysicalDrawFacadesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePhysicalDrawFacadesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> resolve) => new(resolve);
}
