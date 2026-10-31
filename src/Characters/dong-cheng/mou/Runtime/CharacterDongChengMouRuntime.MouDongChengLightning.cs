#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongChengMouRuntime
{
    internal bool MouDcNativeLightning(JudgmentFrame j) => j.Continuation == JudgmentContinuationKind.Lightning && j.ParentAttackId is null && j.ParentFrameId == 0 && j.Reason == JudgmentReasons.Lightning && j.SourceSeat == j.TargetSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentRequestedEvent>().Count(e => e.ResolutionId == j.Id && e.TargetSeat == j.TargetSeat && e.SourceCard == CardKind.Lightning && e.Reason == JudgmentReasons.Lightning) == 1 && (j.DelayedCardId is { } card && j.OriginalUseLightning is null && _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(card)).Any(c => c.Id == card) || j.DelayedCardId is null && j.OriginalUseLightning is { Holder: var holder } && holder == j.TargetSeat);
    internal JudgmentFrame? MouDcDelayedDamage(MouDcOccurrence o) => o.UseId == 0 && _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == o.AttackId)is { } j && MouDcNativeLightning(j) && j.CardAttack is { IsDelayedJudgmentDamage: true } && (j.DelayedCardId ?? 0) == o.CardId ? j : null;
    internal bool MouDcDamageParent(MouDcOccurrence o) => o.UseId > 0 ? _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)?.Action is not null : MouDcDelayedDamage(o)is not null;
    internal bool MouDcHasPaidClaim(ProgramSkillFrame f, MouDcOccurrence o, long beforeId)
    {
        var claims = o.UseId > 0 ? _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)?.MouDongChengClaims : MouDcDelayedDamage(o)?.MouDongChengClaims;
        return claims?.Any(p => p.ProtectionFrameId == f.Id && p.Source == f.MouDongCheng!.Source && p.BeforeWindowId == beforeId) == true;
    }

    MouDcDelayedCause? IMouDcLightningFacts.DyingCause(PlayerDyingEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DyingFrame { Continuation: DyingContinuationKind.Damage } dying || dying.Id != fact.ResolutionId || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(f => f.Id == dying.ParentFrameId)is not { Amount: > 0 } damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not NativeOwners.CardAttackHandle { IsDelayedJudgmentDamage: true, IsSourceLess: true, DamageWasApplied: true } attack || attack.ResolutionId != damage.ParentFrameId || attack.TargetSeat != fact.VictimSeat || _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == attack.ResolutionId)is not { OriginalUseLightning: { } a } j || !MouDcNativeLightning(j) || a.Origin.Source is not { SkillId: MouDongChengRuntime.Douwei, BindingId: "discard-damage-use" } source || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Program?.GameplayHash != a.Origin.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MouDcUseIssuedEvent>().Count(x => x.UseId == a.Origin.UseId && x.ActionId == a.Origin.ActionId && x.Owner == source.OwnerSeat && x.Kind == CardKind.Lightning) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(x => x.ResolutionId == a.Origin.UseId) != 1)
            return null;
        return new(a.Origin.UseId, a.Origin.ActionId, source, a.Origin.GameplayHash, a.Id, j.Id, damage.Id, fact.VictimSeat);
    }

    MouDcSettlement? IMouDcLightningFacts.NextSettlement(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not JudgmentFrame { NativeDelayedAttackCompletion: { } ret } j || j.Id != id || ret.ResolutionId != id || !ret.DelayedTurn || ret.ProgramFrameId is not null || !MouDcNativeLightning(j))
            return null;
        var claim = j.MouDongChengClaims?.FirstOrDefault(p => !p.Settled && _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat].IsAlive);
        if (claim is not null)
            return new(id, claim.Source, claim.GameplayHash, false, claim.ProtectionFrameId, 0, 0);
        return j.MouDongChengReward is { Settled: false } reward && _coreMatchParticipantsPlayersAccess._players[reward.Cause.Source.OwnerSeat].IsAlive ? new(id, reward.Cause.Source, reward.Cause.GameplayHash, true, 0, reward.DyingId, reward.Cause.OriginalUseId) : null;
    }

    bool IMouDcLightningFacts.SettlementParent(ProgramSkillFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (f.MouDongChengSettlement is not { } s || f.ActivationId != "native-delayed-settlement" || f.TriggerId is not null || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.OwnerSeat != s.OriginalSource.OwnerSeat || f.SkillId != s.OriginalSource.SkillId || f.SkillInstanceId != s.OriginalSource.SkillInstanceId || f.GameplayHash != s.OriginalHash || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not JudgmentFrame { NativeDelayedAttackCompletion: { } ret } j || j.Id != s.JudgmentId || ret.ResolutionId != j.Id || !ret.DelayedTurn || ret.ProgramFrameId is not null || !MouDcNativeLightning(j) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouDcSettlementIssuedEvent>().Count(x => x.FrameId == f.Id && x.Return == s) != 1)
            return false;
        return s.Reward ? j.MouDongChengReward is { Settled: true } reward && reward.Cause.Source == s.OriginalSource && reward.Cause.GameplayHash == s.OriginalHash && reward.DyingId == s.DyingId && reward.Cause.OriginalUseId == s.OriginalUseId && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDcLightningRewardEvent>().Count(x => x.Cause == reward.Cause && x.DyingId == reward.DyingId) == 1 : j.MouDongChengClaims?.Any(p => p.ProtectionFrameId == s.ProtectionId && p.Source == s.OriginalSource && p.GameplayHash == s.OriginalHash) == true;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 37401, SkillProgramEffectOp.ResolveMouDongChengDelayedSettlement)]
    internal bool MouDcInternalSettlementIsNotCommand(CharacterState owner) => false;
    ProgramSkillFrame IMouDcLightningNative.BindSettlement(ProgramSkillFrame child, long judgment) => _coreOriginalUseLightningOperations.BindNativeDelayedAttackChild(child, judgment);
    void IMouDcLightningNative.MarkReward(MouDcDelayedCause cause, long dying)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == cause.JudgmentId)is not { OriginalUseLightning: { } a } j || a.Id != cause.AttachmentId || a.Origin.UseId != cause.OriginalUseId || a.Origin.Source != cause.Source || a.Origin.GameplayHash != cause.GameplayHash || j.MouDongChengReward is not null)
            throw new InvalidOperationException("Only the first causal native Lightning dying may earn its original whole-Use reward.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(j.Id, j with { MouDongChengReward = new(cause, dying, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber) });
        _coreMatchParticipantsPlayersAccess._players[cause.Source.OwnerSeat].MouDongChengDisabledTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;
    }

    void IMouDcLightningNative.ResumeCleanup(long judgment) => _coreOriginalUseLightningOperations.ResumeNativeDelayedAttackCompletion(judgment);
    void IMouDcLightningNative.MarkSettlement(MouDcSettlement s)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == s.JudgmentId)is not { NativeDelayedAttackCompletion: not null } j)
            throw new InvalidOperationException("Settlement must retain its complete native delayed return.");
        if (s.Reward)
        {
            if (j.MouDongChengReward is not { Settled: false } reward || reward.DyingId != s.DyingId)
                throw new InvalidOperationException("A delayed reward cannot settle twice.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(j.Id, j with { MouDongChengReward = reward with { Settled = true } });
        }
    }

    [NativeAttackReturn(NativeAttackReturnPhase.BeforeCleanup, 37400)]
    internal bool BeforeMouDcDelayedCleanup(GameEngine.AttackCompletionReceipt completion)
    {
        if (!completion.DelayedTurn || completion.ProgramFrameId is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not JudgmentFrame j || j.Id != completion.ResolutionId || j.MouDongChengReward is null && j.MouDongChengClaims is null)
            return false;
        if (j.NativeDelayedAttackCompletion != completion)
            throw new InvalidOperationException("Baojia and Douwei require the retained complete native delayed attack return.");
        return MouDongCheng.BeginDelayedSettlement(j.Id);
    }

    internal void ClaimMouDcDelayedCard(ProgramSkillFrame f)
    {
        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.ClaimChildren } r || f.MouDongChengSettlement is not { Reward: false } s || _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == s.JudgmentId)is not { NativeDelayedAttackCompletion: not null, MouDongChengClaims: { } claims } j || claims.SingleOrDefault(p => p.ProtectionFrameId == s.ProtectionId)is not { Settled: false } claim)
            throw new InvalidOperationException("Baojia requires its one paid delayed card claim after native resolution.");
        var card = j.DelayedCardId;
        var ids = card is { } id && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DiscardPile ? new[]
        {
            id
        }

        : Array.Empty<int>();
        _coreRuntimeOperations.ReplaceRuntimeFrame(j.Id, j with { MouDongChengClaims = claims.Select(p => p.ProtectionFrameId == claim.ProtectionFrameId ? p with { Settled = true, Before = r.Before, After = r.Before, Cards = ids } : p).ToArray() });
        if (ids.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new("skill-program.mou:baojia.delayed-claim"), (_, moves) =>
            {
                if (moves is not [var m] || m.CardId != ids[0] || m.From != CardLocation.DiscardPile || m.Sequence <= r.Before || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame)
                    throw new InvalidOperationException("Baojia's genuine delayed entity claim invoice changed.");
                var own = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, own with { MouDongCheng = own.MouDongCheng!with { After = m.Sequence, Actual = m.To == CardLocation.Hand(f.OwnerSeat) ? 1 : 0 } });
                var actual = (JudgmentFrame)_resolutionStackOperations._resolutionStack.Single(x => x.Id == j.Id);
                _coreRuntimeOperations.ReplaceRuntimeFrame(j.Id, actual with { MouDongChengClaims = actual.MouDongChengClaims!.Select(p => p.ProtectionFrameId == claim.ProtectionFrameId ? p with { After = m.Sequence } : p).ToArray() });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDcDelayedClaimSettledEvent(f.Id, j.Id, claim.ProtectionFrameId, ids[0], m.To == CardLocation.Hand(f.OwnerSeat) ? 1 : 0, r.Before, m.Sequence));
            });
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDcDelayedClaimSettledEvent(f.Id, j.Id, claim.ProtectionFrameId, card ?? 0, 0, r.Before, r.Before));
    }
}
