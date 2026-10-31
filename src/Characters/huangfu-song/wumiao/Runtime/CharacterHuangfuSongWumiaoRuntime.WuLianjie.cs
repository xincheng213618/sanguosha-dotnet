#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangfuSongWumiaoRuntime
{
    internal bool WuHfVictimAvailable(int owner, int target) => _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count > 0 && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuHuangFuSongBottomPaidEvent>().Any(e => e.OwnerSeat == owner && e.TargetSeat == target && e.Turn == WuHfTurn());
    internal CardUseFrame? WuHfLianjieUse(int owner, ProgramSkillWindowContext c)
    {
        if (c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.CardUse is not { } identity || identity.ActorSeat != owner || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { WuHuangFuSongUse: { HandUse: true, EffectiveRank: { } rank } r, Action: { } a } use || use.Id != r.UseId || a.ActionId != identity.CardActionId || a.ActorSeat != owner || a.ProviderSeat != owner || !WuHfTurnOpen(r.Turn) || a.EffectiveDesignatedTargetSeats.Count == 0 || !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(card => !a.PhysicalCards.Any(cost => cost.CardId == card.Id)).All(card => rank <= _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[owner], card)))
            return null;
        return _coreMatchParticipantsPlayersAccess._players.Any(p => WuHfVictimAvailable(owner, p.Seat)) ? use : null;
    }
}
