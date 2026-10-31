#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess : IProgramRuntimeOperations
{
    ref long _actorHandLimitPenaltySequence { get; }

    ref readonly List<ActorHandLimitPenalty> _actorHandLimitPenalties { get; }
}
