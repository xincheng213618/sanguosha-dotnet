#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess : IProgramRuntimeOperations
{
    ref readonly List<PhaseNamePredictionPolicy> _phaseNamePredictions { get; }
}
