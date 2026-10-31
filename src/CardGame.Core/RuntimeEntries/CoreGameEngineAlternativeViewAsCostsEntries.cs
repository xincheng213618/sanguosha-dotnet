#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternativeViewAsCostsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _resolve;
    private CoreGameEngineAlternativeViewAsCostsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlternativeViewAsCostsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> resolve) => new(resolve);
}
