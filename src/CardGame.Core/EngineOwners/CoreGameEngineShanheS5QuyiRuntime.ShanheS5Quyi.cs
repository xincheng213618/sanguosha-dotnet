#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5QuyiRuntime
{
    internal bool IsShanheS5FuqiResponseProhibited(int sourceSeat, int responderSeat) => sourceSeat != responderSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(responderSeat) && _coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[responderSeat].IsAlive && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[sourceSeat], SkillProgramCardPolicyKind.ProhibitOthersResponse).Any(item => item.Policy.Value == 0 || _coreNativeRuleQueriesOperations.GetCombatDistance(sourceSeat, responderSeat) == 1);
    internal bool ShanheS5FuqiNullificationBanned(int responderSeat)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } window)
            return false;
        // A counterspell responds to the last actual counterspell, rather than
        // retaining the initial trick user's privilege throughout the chain.
        var sourceSeat = window.ChainDepth == 0 ? window.SourceSeat : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(e => e.WindowFrameId == window.Id && e.ResolutionId == window.ParentFrameId && e.ChainDepth == window.ChainDepth)?.ResponderSeat ?? -1;
        return IsShanheS5FuqiResponseProhibited(sourceSeat, responderSeat);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 5724)]
    internal bool ShanheS5FuqiNativeResponseForbidden(ProgramNativeResponseQuery query) => !query.DyingUse && query.Kind == CardKind.Nullification && ShanheS5FuqiNullificationBanned(query.Provider.Seat);
    internal bool IsShanheS5JiaoziQualified(int ownerSeat) => _coreActualRoundProgressRoundNumberAccess._roundNumber >= 1 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != ownerSeat).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Count);
    internal int ShanheS5JiaoziModifierAmount(SkillProgramDamageModifier modifier) => modifier.Condition == SkillProgramDamageModifierCondition.OwnerMaximumHandCurrentRound ? _coreActualRoundProgressRoundNumberAccess._roundNumber : modifier.Amount;
    internal int ShanheS5JiaoziPublicDamageBonus(GameSnapshot view, int sourceSeat, int targetSeat, bool sourceLess, int paidHandCards = 0)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasMaximumHandRoundDamageModifiers)
            return 0;
        // All comparisons are public hand counts; only visible skill instances
        // contribute to the viewer's prediction. Actual payment uses the same
        // qualification and amount helpers in the passive damage pipeline.
        int HandCount(PlayerSnapshot player) => Math.Max(0, player.HandCount - (player.Seat == sourceSeat ? paidHandCards : 0));
        var participants = sourceLess ? new[]
        {
            targetSeat
        }

        : new[]
        {
            sourceSeat,
            targetSeat
        }.Distinct();
        return participants.Where(seat => _coreActualRoundProgressRoundNumberAccess._roundNumber >= 1 && view.Players.Single(p => p.Seat == seat)is { IsAlive: true } owner && view.Players.Where(p => p.IsAlive && p.Seat != seat).All(p => HandCount(p) <= HandCount(owner))).Sum(seat =>
        {
            var visible = CoreGameEnginePublicDamagePredictionRuntime.VisiblePredictionSkillIds(view.Players.Single(p => p.Seat == seat));
            return _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ProgramInstances.Where(instance => visible.Contains(instance.SkillId)).SelectMany(instance => instance.Program.DamageModifiers).Where(modifier => modifier.Condition == SkillProgramDamageModifierCondition.OwnerMaximumHandCurrentRound).Sum(ShanheS5JiaoziModifierAmount);
        });
    }
}
