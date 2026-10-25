namespace CardGame.Core;
public sealed partial class GameEngine
{
    private const string MobileLianzhu = "mobile:lianzhu", MobileXiahui = "mobile:xiahui";
    private SkillGrantPhaseExpiry? MobileLianzhuPhase(int owner) => _phase == TurnPhase.Play && _currentSeat == owner && _turnNumber > 0 &&
        _cardUseDebitPhaseInstanceId > 0 ? new(_turnNumber, owner, _cardUseDebitPhaseInstanceId) : null;
    private bool ValidMobileLianzhuIssued(MobileLianzhuIssuedEvent e) => e.Source is { SkillId: MobileLianzhu, BindingId: "reveal-and-give" } &&
        IsValidPlayerSeat(e.Source.OwnerSeat) && IsValidPlayerSeat(e.Recipient) && e.Source.OwnerSeat != e.Recipient &&
        e.Phase.PhaseActorSeat == e.Source.OwnerSeat && e.Hash == _contentRegistry.GetSkill(MobileLianzhu).Program?.GameplayHash &&
        ProgramEventHistory<ProgramSkillStartedEvent>().Count(s => s.FrameId == e.FrameId && s.SkillId == MobileLianzhu && s.ActivationId == e.Source.BindingId && s.OwnerSeat == e.Source.OwnerSeat) == 1;
    private bool MobileLianzhuGiftPaid(MobileLianzhuIssuedEvent e) => ValidMobileLianzhuIssued(e) && _cardMovements.Count(m => m.CardId == e.CardId &&
        m.From == e.From && m.To == CardLocation.Hand(e.Recipient) && m.Sequence > e.Before && m.Reason.Value == "skill-program.mobile:lianzhu.InitialGiftChildren") == 1;
    private bool MobileLianzhuGiftMoved(MobileLianzhuIssuedEvent e) => ValidMobileLianzhuIssued(e) && _cardMovements.Any(m => m.CardId == e.CardId &&
        m.From == e.From && m.Sequence > e.Before && m.Reason.Value == "skill-program.mobile:lianzhu.InitialGiftChildren");
    private MobileLianzhuIssuedEvent? ActiveMobileLianzhu(int owner) => MobileLianzhuPhase(owner) is { } phase && _players[owner].IsAlive
        ? ProgramEventHistory<MobileLianzhuIssuedEvent>().SingleOrDefault(e => e.Source.OwnerSeat == owner && e.Phase == phase && MobileLianzhuGiftPaid(e)) : null;
    private bool MobileLianzhuHolder(int owner, int target) => ActiveMobileLianzhu(owner) is { } e &&
        _cardZones.GetLocation(e.CardId) is { OwnerSeat: { } holder, Zone: CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment } && holder == target;
    [ProgramCapability] private sealed class MobileDongBaiHost(GameEngine engine) : IMobileDongBaiHost
    {
        public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffect effect) => effect.Op switch
        {
            SkillProgramEffectOp.PrepareMobileDongBaiLianzhu => engine.PrepareMobileLianzhu(f, effect.SourceBind),
            SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay => engine.BeginMobileLianzhuRelay(f),
            _ => engine.RecoverMobileLianzhu(f)
        };
    }
    private SkillProgramStepOutcome PrepareMobileLianzhu(ProgramSkillFrame f, string? sourceBind)
    {
        if (f.InstructionIndex == 4 && sourceBind == "lianzhu-card" && ProgramEventHistory<MobileLianzhuIssuedEvent>().SingleOrDefault(e => e.FrameId == f.Id) is { } prepared &&
            ValidMobileLianzhuIssued(prepared) && prepared.Hash == f.GameplayHash && f.SelectedCardIds.SequenceEqual([prepared.CardId]) &&
            f.SelectedTargetSeats.SequenceEqual([prepared.Recipient]))
        {
            if (f.CardSetBindings.SingleOrDefault(b => b.Name == sourceBind) is not
                { Visibility: SkillProgramCardSetVisibility.Public, CardIds: [var bound], SourceLocations: [var boundFrom] } || bound != prepared.CardId || boundFrom != prepared.From)
                throw new InvalidOperationException("Mobile Lianzhu delivery must consume its exact once-captured publicly revealed selected entity and original source.");
            if (_cardZones.GetLocation(prepared.CardId) != prepared.From || !_players[prepared.Recipient].IsAlive || MobileLianzhuPhase(f.OwnerSeat) != prepared.Phase)
                return SkillProgramStepOutcome.Continue;
            ReplaceRuntimeTop(f = f with { MobileLianzhu = new(prepared.FrameId, prepared.CardId, f.OwnerSeat, 0, MobileLianzhuStage.InitialGiftChildren) });
            MoveMobileLianzhuEntity(f, MobileLianzhuStage.InitialGiftChildren, prepared.CardId, prepared.From, prepared.Recipient);
            return SkillProgramStepOutcome.AwaitChild;
        }
        if (f.InstructionIndex != 1 || sourceBind is not null || f.TriggerId is not null || f.SelectedCardIds is not [var card] || f.SelectedTargetSeats is not [var target] ||
            MobileLianzhuPhase(f.OwnerSeat) is not { } phase || _cardZones.GetLocation(card) is not { OwnerSeat: { } owner, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } from ||
            owner != f.OwnerSeat || target == owner || !_players[target].IsAlive || ProgramEventHistory<MobileLianzhuIssuedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Mobile Lianzhu requires its exact original owned HE entity, other recipient and actual Play.");
        AdvanceEventRulesAndQueueFact(new MobileLianzhuIssuedEvent(f.Id, new(f.SkillId, f.ActivationId, owner, f.SkillInstanceId), f.GameplayHash, phase, card, from, target, _movementSequence));
        return SkillProgramStepOutcome.Continue;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.PaidInitialSelection, 79100)]
    private bool KeepMobileLianzhuGift(ProgramSkillFrame f) => f.SkillId == MobileLianzhu && f.ActivationId == "reveal-and-give" && f.TriggerId is null &&
        ProgramEventHistory<MobileLianzhuIssuedEvent>().SingleOrDefault(e => e.FrameId == f.Id) is { } issued && ValidMobileLianzhuIssued(issued) &&
        _cardMovements.Any(m => m.CardId == issued.CardId && m.From == issued.From && m.Sequence > issued.Before && m.Reason.Value == "skill-program.mobile:lianzhu.InitialGiftChildren") &&
        f.GameplayHash == issued.Hash && f.SelectedCardIds.SequenceEqual([issued.CardId]) && f.SelectedTargetSeats.SequenceEqual([issued.Recipient]);

    [ProgramTargetRule(ProgramTargetRulePoint.QuotaAllowance, 79100)]
    private bool MobileLianzhuQuota(ProgramTargetQuotaQuery q) => MobileLianzhuHolder(q.ActorSeat, q.TargetSeat);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 79100)]
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 79100)]
    private bool MobileLianzhuSlashQuota(ProgramSlashQuotaQuery q) => MobileLianzhuHolder(q.Actor.Seat, q.Target.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 79100)]
    private bool MobileLianzhuDistance(ProgramCardDistanceQuery q) => MobileLianzhuHolder(q.Actor.Seat, q.Target.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 79100)] [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 79100)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 79100)] [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 79100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 79100)] [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 79100)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 79100)]
    private bool MobileLianzhuSlashDistance(ProgramSlashDistanceQuery q) => MobileLianzhuHolder(q.Actor.Seat, q.Target.Seat);

    private bool IsMobileLianzhuGain(MobileLianzhuIssuedEvent issued, CardMovementRecord m) => m.CardId == issued.CardId && m.From != m.To &&
        m.To is { Zone: CardZoneKind.Hand, OwnerSeat: { } holder } && _players[holder].IsAlive && m.Sequence > issued.Before &&
        !ProgramEventHistory<MobileLianzhuGainProcessedEvent>().Any(e => e.IssuedFrameId == issued.FrameId && e.MovementSequence == m.Sequence);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 79100)]
    private void CollectMobileLianzhuGains(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates)
    {
        candidates.RemoveAll(c => c.SkillId == MobileLianzhu && c.BindingId == "gained-relay");
        foreach (var owner in _players.Where(p => p.IsAlive))
        {
            if (ActiveMobileLianzhu(owner.Seat) is not { } issued) continue;
            for (var index = 0; index < batch.Movements.Count; index++)
                if (IsMobileLianzhuGain(issued, batch.Movements[index]))
                    candidates.Add(new(owner.Seat, MobileLianzhu, "gained-relay", issued.Source.SkillInstanceId, issued.Hash, 0, index));
        }
    }
    private bool TryMobileLianzhuGain(int owner, ProgramSkillWindowContext c, out MobileLianzhuIssuedEvent issued, out CardMovementRecord movement)
    {
        issued = ActiveMobileLianzhu(owner)!; movement = null!;
        if (issued is null || c.Window != SkillProgramTriggerWindow.CardsGained || c.MovementBatch is not { } batch || c.MovementIndex is not { } index ||
            index < 0 || index >= batch.Movements.Count || !IsMobileLianzhuGain(issued, batch.Movements[index])) return false;
        movement = batch.Movements[index]; return _cardZones.GetLocation(movement.CardId) == movement.To;
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 79100, SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay)]
    private bool QualifyMobileLianzhuGain(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => TryMobileLianzhuGain(candidate.OwnerSeat, context, out _, out _);
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 79100)]
    private bool AdmitIssuedMobileLianzhu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == MobileLianzhu &&
        ActiveMobileLianzhu(c.OwnerSeat) is { } issued && c.SkillInstanceId == issued.Source.SkillInstanceId && c.GameplayHash == issued.Hash &&
        (c.BindingId == "gained-relay" && TryMobileLianzhuGain(c.OwnerSeat, context, out _, out _) ||
         c.BindingId == "damage-recover" && QualifyMobileLianzhuRecovery(c, context));
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 79100)]
    private IReadOnlyList<ProgramTriggerCandidate> IncludeIssuedMobileLianzhuDamage(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> candidates)
    {
        if (window != SkillProgramTriggerWindow.AfterDamageApplied || ActiveMobileLianzhu(owner.Seat) is not { } issued ||
            candidates.Any(c => c.SkillId == MobileLianzhu && c.BindingId == "damage-recover" && c.SkillInstanceId == issued.Source.SkillInstanceId)) return candidates;
        return Array.AsReadOnly(candidates.Append(new(owner.Seat, MobileLianzhu, "damage-recover", issued.Source.SkillInstanceId, issued.Hash, 0)).ToArray());
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 79100)]
    private void LockMobileLianzhuEntity(CardMovedEvent fact)
    {
        if (fact.To is not { Zone: CardZoneKind.Hand, OwnerSeat: { } recipient } || fact.From == fact.To || !_players[recipient].IsAlive) return;
        foreach (var owner in _players.Where(p => p.IsAlive && p.Seat != recipient))
        {
            if (ActiveMobileLianzhu(owner.Seat) is not { } issued || issued.CardId != fact.CardId) continue;
            foreach (var policy in CardPolicies(owner, SkillProgramCardPolicyKind.XiaHuiBlackHandAndGainedEntityRestrictions).Where(p => p.Source.SkillId == MobileXiahui))
            {
                var movement = _cardMovements.Last(m => m.CardId == fact.CardId && m.From == fact.From && m.To == fact.To && m.Reason == fact.Reason);
                var source = new CardConversionSource(MobileXiahui, policy.Policy.Id, owner.Seat, policy.Source.SkillInstanceId);
                var restriction = new XiaHuiCardRestriction(source, _contentRegistry.GetSkill(MobileXiahui).Program!.GameplayHash, recipient, fact.CardId,
                    movement.Sequence, EffectiveSuit(movement.From.OwnerSeat is { } original ? _players[original] : owner, GetAdvancedCard(fact.CardId)));
                var old = _players[recipient].XiaHuiRestrictions ?? [];
                if (old.Any(r => r.Source == source && r.CardId == fact.CardId && r.MovementSequence == movement.Sequence)) continue;
                _players[recipient].XiaHuiRestrictions = Array.AsReadOnly(old.Append(restriction).ToArray());
                AdvanceEventRulesAndQueueFact(new XiaHuiRestrictionGrantedEvent(restriction));
            }
        }
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 79101, SkillProgramEffectOp.RecoverMobileDongBaiLianzhu)]
    private bool QualifyMobileLianzhuRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => w is
        { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, OccurrenceIndex: 0, TargetSeat: { } target } &&
        w.SourceSeat == c.OwnerSeat && MobileLianzhuHolder(c.OwnerSeat, target) && _players[target].IsAlive;
}
