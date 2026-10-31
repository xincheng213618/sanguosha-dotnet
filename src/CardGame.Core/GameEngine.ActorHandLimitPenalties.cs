namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Issued rule contributions, not pending resolution state. Command replay
    // recreates them from the same native use and actual-turn boundary facts.
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ActorHandLimitPenalty> _actorHandLimitPenalties => ref _actorHandLimitPenalties2.ReferenceToActorHandLimitPenalties;
    private ref long _actorHandLimitPenaltySequence => ref _actorHandLimitPenalties2.ReferenceToActorHandLimitPenaltySequence;
}
