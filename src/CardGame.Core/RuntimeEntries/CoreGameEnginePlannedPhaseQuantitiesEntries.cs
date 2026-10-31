#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime))]
internal sealed partial class CoreGameEnginePlannedPhaseQuantitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> _resolve;
    private CoreGameEnginePlannedPhaseQuantitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlannedPhaseQuantitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> resolve) => new(resolve);
}
