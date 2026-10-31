#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEnginePhaseNamePredictionState : Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess
{
    private List<PhaseNamePredictionPolicy> _phaseNamePredictions = [];
    internal ref readonly List<PhaseNamePredictionPolicy> ReferenceToPhaseNamePredictions => ref _phaseNamePredictions;

    ref readonly List<PhaseNamePredictionPolicy> Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions => ref _phaseNamePredictions;
}
