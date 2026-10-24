namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasShiZrCapability =>
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiZrFuan);

    private sealed record ShiZrFuanRecordInfo(Suit Suit, bool ViaRelaunch);

    // The record is derived from the committed ledger: a later Recorded replaces
    // the previous one, an explicit Cleared empties it.
    private ShiZrFuanRecordInfo? ShiZrFuanRecord(int seat)
    {
        ShiZrFuanRecordInfo? record = null;
        foreach (var payload in CompleteProgramEventHistory())
        {
            if (payload is ShiZrFuanRecordedEvent { OwnerSeat: var owner } rec && owner == seat)
                record = new(rec.Suit, rec.ViaRelaunch);
            else if (payload is ShiZrFuanClearedEvent { OwnerSeat: var cleared } && cleared == seat)
                record = null;
        }
        return record;
    }

    // Armed events both zero the range and reset the accrued YinXian bonus.
    private int ShiZrYinXianBonus(int seat)
    {
        var bonus = 0;
        foreach (var payload in CompleteProgramEventHistory())
        {
            if (payload is ShiZrFuanArmedEvent { OwnerSeat: var owner } && owner == seat)
                bonus = 0;
            else if (payload is ShiZrYinXianBonusGainedEvent { OwnerSeat: var gained } gain && gained == seat)
                bonus = gain.Bonus;
        }
        return bonus;
    }

    private bool ShiZrFuanZeroed(int seat) => HasShiZrCapability &&
        CompleteProgramEventHistory().OfType<ShiZrFuanArmedEvent>().Any(armed => armed.OwnerSeat == seat);

    private int ShiZrYinXianRangeBonus(CharacterState player) =>
        HasShiZrCapability ? ShiZrYinXianBonus(player.Seat) : 0;

    private bool ShiZrFuanBaseZeroed(CharacterState player) =>
        HasShiZrCapability && ShiZrFuanZeroed(player.Seat);

    // 引弦①: +1 Slash damage against the farthest in-range characters.
    private bool ShiZrYinXianFarthest(int user, int target)
    {
        if (!HasShiZrCapability || _winner != Winner.None || !_players[user].IsAlive ||
            !HasRuntimeSkill(_players[user], ShiZhangRenComposition.YinXianSkill)) return false;
        var range = GetAttackRange(user);
        if (range <= 0) return false;
        var farthest = -1;
        for (var seat = 0; seat < _players.Count; seat++)
        {
            if (seat == user || !_players[seat].IsAlive) continue;
            var distance = GetCombatDistance(user, seat);
            if (distance >= 1 && distance <= range && distance > farthest) farthest = distance;
        }
        return farthest >= 1 && GetCombatDistance(user, target) == farthest;
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 68200)]
    private int ShiZrYinXianDamageBonus(IDamageAttempt attack) =>
        attack is CardAttackHandle card && card.CardUserSeat is { } user && user >= 0 &&
        attack.EffectiveCardKind is { } kind && IsSlashCard(kind) &&
        attack.TargetSeat is { } target && target != user &&
        ShiZrYinXianFarthest(user, target) ? 1 : 0;

    // The strike is a cardless virtual use; its declared use id exempts every
    // distance stage during resolution. The child attack commits its target back
    // onto the owning trigger frame, which the cursor invariant accepts through
    // this exact issued-strike receipt.
    private bool IsShiZrFuanStrikeUse(long useFrameId) =>
        ProgramEventHistory<ShiZrFuanStrikeIssuedEvent>().Any(entry => entry.UseId == useFrameId);

    private bool HasExactShiZrFuanStrikeTarget(ProgramSkillFrame frame) =>
        frame.TriggerId == ShiZhangRenComposition.FuanStrikeTrigger && frame.SelectedCardIds.Count == 0 &&
        frame.SelectedTargetSeats is [var target] &&
        ProgramEventHistory<ShiZrFuanStrikeIssuedEvent>().Any(entry => entry.FrameId == frame.Id && entry.TargetSeat == target);

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 68200)]
    private bool ShiZrFuanUseDistance(ProgramCardDistanceQuery query) =>
        query.CardUseFrameId is { } id && IsShiZrFuanStrikeUse(id);

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 68200)]
    private bool ShiZrFuanSlashDistance(ProgramSlashDistanceQuery query) =>
        query.ExistingUseFrameId is { } id && IsShiZrFuanStrikeUse(id);

    // Observes completed engine facts only: discards during an armed record arm
    // the foreign turn-ending strike.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68200)]
    private void ObserveShiZrFuanDiscards(IGameEvent payload)
    {
        if (!HasShiZrCapability || _winner != Winner.None ||
            payload is not CardMovedEvent { CardId: var cardId, To: { Zone: CardZoneKind.DiscardPile } }) return;
        var suit = GetAdvancedCard(cardId).Suit;
        for (var seat = 0; seat < _players.Count; seat++)
        {
            if (!_players[seat].IsAlive || ShiZrFuanRecord(seat) is not { } record || record.Suit != suit) continue;
            AdvanceEventRulesAndQueueFact(new ShiZrFuanSuitDiscardedEvent(_turnNumber, seat, cardId, suit));
        }
    }

    // 伏暗②'s seal: the target's non-locked skills fail until the end of their
    // next turn. Event-derived facts survive replay without a restore list.
    private bool ShiZrFuanSealActive(int target) =>
        ProgramEventHistory<ShiZrFuanSealedEvent>().Any(stamped => stamped.TargetSeat == target &&
            !ProgramEventHistory<ShiZrFuanSealExpiredEvent>().Any(expired => expired.FrameId == stamped.FrameId));

    private bool ShiZrFuanGrantQualified(CharacterState owner, SkillGrant grant) => !HasShiZrCapability ||
        grant.IsModeRule || _contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked) ||
        !ShiZrFuanSealActive(owner.Seat);

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68201)]
    private void ExpireShiZrFuanSeals(IGameEvent payload)
    {
        if (!HasShiZrCapability || payload is not TurnEndedEvent turn) return;
        foreach (var sealedEntry in ProgramEventHistory<ShiZrFuanSealedEvent>()
                     .Where(entry => entry.TargetSeat == turn.ActorSeat && turn.TurnNumber > entry.GrantedTurn &&
                         !ProgramEventHistory<ShiZrFuanSealExpiredEvent>().Any(expired => expired.FrameId == entry.FrameId))
                     .ToArray())
        {
            AdvanceEventRulesAndQueueFact(new ShiZrFuanSealExpiredEvent(sealedEntry.FrameId, sealedEntry.TargetSeat, turn.TurnNumber));
            _currentTurnSkillSuppressionRevision++;
        }
    }
}
