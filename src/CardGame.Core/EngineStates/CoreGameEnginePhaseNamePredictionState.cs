#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEnginePhaseNamePredictionState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.PhaseNamePredictionPolicy> _phaseNamePredictions = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.PhaseNamePredictionPolicy> ReferenceToPhaseNamePredictions => ref _phaseNamePredictions;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.PhaseNamePredictionPolicy> global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions => ref _phaseNamePredictions;
}
