#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed partial class CoreGameEngineAlternativeViewAsCostsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _resolve;
    private CoreGameEngineAlternativeViewAsCostsEntries(Func<EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlternativeViewAsCostsEntries Create(Func<EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> resolve) => new(resolve);
}
