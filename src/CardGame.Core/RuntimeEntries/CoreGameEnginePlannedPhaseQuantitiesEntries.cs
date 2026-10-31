#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime))]
internal sealed partial class CoreGameEnginePlannedPhaseQuantitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> _resolve;
    private CoreGameEnginePlannedPhaseQuantitiesEntries(Func<EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlannedPhaseQuantitiesEntries Create(Func<EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime> resolve) => new(resolve);
}
