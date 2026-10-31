#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeLegalActionsRuntime
{
    internal IReadOnlyList<LegalAction> GetLegalActions(int actorSeat)
    {
        _coreTableSeatQueriesOperations.ValidatePlayerSeat(actorSeat, "actorSeat");
        if (_coreProgramParticipantPromptsOperations.GetPendingDecisionForActor(actorSeat)is not { } pending)
            return[];
        return pending.Kind == DecisionKind.PlayCard ? BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[actorSeat]) : pending.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash ? _coreNationalGeneralRevealOperations.BuildNationalRevealActions(_coreMatchParticipantsPlayersAccess._players[actorSeat]) : [];
    }
}
