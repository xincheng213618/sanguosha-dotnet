#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlannedPhaseQuantitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> _resolve;
    private CoreGameEnginePlannedPhaseQuantitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlannedPhaseQuantitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> resolve) => new(resolve);
}
