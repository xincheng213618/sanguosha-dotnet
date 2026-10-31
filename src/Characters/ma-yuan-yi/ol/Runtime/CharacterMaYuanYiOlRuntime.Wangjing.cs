#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaYuanYiOlRuntime
{
    internal (MaYuanYiBingActionEvent Fact, int Opponent)? WangjingQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window is not (SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardResponseAccepted) || context.CardUse is not { } card)
            return null;
        var fact = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MaYuanYiBingActionEvent>().SingleOrDefault(e => e.ActionId == card.CardActionId && e.ProviderSeat == c.OwnerSeat);
        if (fact is null || !ValidBingAction(fact))
            return null;
        var opposite = fact.ActorSeat != fact.ProviderSeat ? fact.RequesterSeat : fact.Type == CardActionType.Response ? fact.OpponentSeat : context.TargetSeat;
        if (opposite is not { } seat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[seat].HasHpValue || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WangjingDrawIssuedEvent>().Any(e => e.ActionId == fact.ActionId && e.OwnerSeat == c.OwnerSeat && e.OpponentSeat == seat) || _coreMatchParticipantsPlayersAccess._players[seat].Hp != _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue).Max(p => p.Hp))
            return null;
        return (fact, seat);
    }
}
