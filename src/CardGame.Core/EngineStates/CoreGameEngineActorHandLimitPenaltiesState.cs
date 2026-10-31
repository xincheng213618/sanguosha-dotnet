#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineActorHandLimitPenaltiesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess
{
    // Issued rule contributions, not pending resolution state. Command replay
    // recreates them from the same native use and actual-turn boundary facts.
    private global::System.Collections.Generic.List<global::CardGame.Core.ActorHandLimitPenalty> _actorHandLimitPenalties = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ActorHandLimitPenalty> ReferenceToActorHandLimitPenalties => ref _actorHandLimitPenalties;

    private long _actorHandLimitPenaltySequence = default !;
    internal ref long ReferenceToActorHandLimitPenaltySequence => ref _actorHandLimitPenaltySequence;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenaltySequence => ref _actorHandLimitPenaltySequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ActorHandLimitPenalty> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess._actorHandLimitPenalties => ref _actorHandLimitPenalties;
}
