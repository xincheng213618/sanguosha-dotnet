#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineActorHandLimitPenaltiesState : Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess
{
    // Issued rule contributions, not pending resolution state. Command replay
    // recreates them from the same native use and actual-turn boundary facts.
    private List<ActorHandLimitPenalty> _actorHandLimitPenalties = [];
    internal ref readonly List<ActorHandLimitPenalty> ReferenceToActorHandLimitPenalties => ref _actorHandLimitPenalties;

    private long _actorHandLimitPenaltySequence = default !;
    internal ref long ReferenceToActorHandLimitPenaltySequence => ref _actorHandLimitPenaltySequence;

    ref long Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenaltySequence => ref _actorHandLimitPenaltySequence;

    ref readonly List<ActorHandLimitPenalty> Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties => ref _actorHandLimitPenalties;
}
