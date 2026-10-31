#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref long _actorHandLimitPenaltySequence { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ActorHandLimitPenalty> _actorHandLimitPenalties { get; }
}
