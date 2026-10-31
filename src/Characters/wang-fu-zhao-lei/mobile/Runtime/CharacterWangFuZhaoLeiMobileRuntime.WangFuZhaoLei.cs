#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangFuZhaoLeiMobileRuntime
{
    internal static CardConversionSource XunyiSource(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_wang_fu_zhao_lei_mobile_WangFuZhaoLei.XunyiSource(frame);
    internal bool TracksXunyi => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.SettleXunyiDamagePoint) == true;

    internal IEnumerable<XunyiRelationGrant> XunyiRelations() => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XunyiRelationChangedEvent>().GroupBy(e => (e.Relation.Source.OwnerSeat, e.Relation.Source.SkillId, e.Relation.Source.SkillInstanceId)).Select(g => g.Last().Relation);
    internal XunyiRelationGrant? XunyiRelation(int owner, string skill, string instance) => XunyiRelations().SingleOrDefault(g => g.Source.OwnerSeat == owner && g.Source.SkillId == skill && g.Source.SkillInstanceId == instance);
    internal XunyiDamageReceipt? XunyiQualification(int owner, string skill, string instance, long damage) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XunyiDamageQualifiedEvent>().Select(e => e.Receipt).SingleOrDefault(r => r.DamageFrameId == damage && r.Relation.Source.OwnerSeat == owner && r.Relation.Source.SkillId == skill && r.Relation.Source.SkillInstanceId == instance);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26320)]
    internal void CaptureXunyiActualDamage(IGameEvent payload)
    {
        if (!TracksXunyi || payload is not DamageAppliedEvent { Amount: > 0 } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || !attack.DamageWasApplied || attack.TargetSeat != applied.TargetSeat || attack.DamageAmount != applied.Amount)
            return;
        var damage = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault(d => d.ParentFrameId == attack.ResolutionId && d.TargetSeat == applied.TargetSeat && d.Amount == applied.Amount) ?? throw new InvalidOperationException("Xunyi requires the actual native damage frame.");
        var source = attack.IsSourceLess || attack.IsDelayedJudgmentDamage ? (int? )null : applied.SourceSeat;
        foreach (var relation in XunyiRelations().Where(g => _coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat].IsAlive).ToArray())
        {
            var owner = relation.Source.OwnerSeat;
            var other = relation.HolderSeat;
            if (!_coreMatchParticipantsPlayersAccess._players[other].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[owner], relation.Source.SkillId, relation.Source.SkillInstanceId))
                continue;
            int? discard = applied.TargetSeat == owner && source != other ? other : applied.TargetSeat == other && source != owner ? owner : null;
            int? draw = source == owner && applied.TargetSeat != other ? other : source == other && applied.TargetSeat != owner ? owner : null;
            if (discard is null && draw is null)
                continue;
            if (XunyiQualification(owner, relation.Source.SkillId, relation.Source.SkillInstanceId, damage.Id)is not null)
                throw new InvalidOperationException("Xunyi cannot qualify the same actual damage twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XunyiDamageQualifiedEvent(new(relation, damage.Id, attack.ResolutionId, source, applied.TargetSeat, applied.Amount, discard, draw)));
        }
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26320)]
    internal bool CanOfferXunyi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ArmXunyiRelation))
            return context.Window == SkillProgramTriggerWindow.GameStarting && XunyiRelation(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId)is null && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat);
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.MoveXunyiAfterPartnerDeath))
            return context.Window == SkillProgramTriggerWindow.CharacterDied && context.TargetSeat is { } victim && !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive && XunyiRelation(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId)?.HolderSeat == victim && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat);
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.SettleXunyiDamagePoint))
            return true;
        return context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.DamageFrameId is { } damage && XunyiQualification(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, damage)is { } receipt && receipt.ActualAmount == context.Amount && receipt.ActualTargetSeat == context.TargetSeat && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XunyiDamageIssuedEvent>().Any(e => e.Receipt == receipt);
    }

    internal SkillProgramStepOutcome ExecuteXunyi(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (operation != SkillProgramEffectOp.SettleXunyiDamagePoint)
        {
            ChangeXunyiRelation(frame, operation == SkillProgramEffectOp.MoveXunyiAfterPartnerDeath);
            return SkillProgramStepOutcome.Continue;
        }

        if (frame.Xunyi is not null || frame.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, DamageFrameId: { } damage } context || !CanOfferXunyi(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(frame), context))
            throw new InvalidOperationException("Xunyi requires its exact unissued after-damage candidate.");
        var receipt = XunyiQualification(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, damage)!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Xunyi = new(receipt, frame.InstructionIndex, 0, XunyiStage.DiscardChoice) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XunyiDamageIssuedEvent(frame.Id, receipt, frame.InstructionIndex));
        BeginNextXunyiPoint(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ChangeXunyiRelation(ProgramSkillFrame frame, bool transfer)
    {
        if (frame.SelectedTargetSeats is not [var target] || target == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || frame.WindowContext is not { } context || frame.InstructionIndex != 2 || !CanOfferXunyi(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(frame), context))
            throw new InvalidOperationException("Xunyi assignment requires its exact native selection and relation.");
        var old = XunyiRelation(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId);
        if (transfer != (old is not null))
            throw new InvalidOperationException("Xunyi cannot invent an initial or transferred relation.");
        var relation = old is null ? new XunyiRelationGrant(frame.Id, XunyiSource(frame), frame.GameplayHash, target, context.ParentFrameId) : old with
        {
            FrameId = frame.Id,
            HolderSeat = target
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XunyiRelationChangedEvent(relation, old?.HolderSeat));
        if (old is not null)
            SynchronizeXunyiMarker(frame, old.HolderSeat);
        SynchronizeXunyiMarker(frame, target);
    }

    internal void SynchronizeXunyiMarker(ProgramSkillFrame frame, int holder)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[holder];
        var marker = PlayerMarkerKind.XunyiYi;
        var before = player.Markers.GetValueOrDefault(marker);
        foreach (var key in player.MarkerSourceCounts.Keys.Where(k => k.Marker == marker).ToArray())
            player.MarkerSourceCounts.Remove(key);
        var counts = XunyiRelations().Where(g => g.HolderSeat == holder).GroupBy(g => g.Source.OwnerSeat).ToArray();
        foreach (var count in counts)
            player.MarkerSourceCounts[(marker, count.Key)] = count.Count();
        var total = counts.Sum(c => c.Count());
        if (total == 0)
            player.Markers.Remove(marker);
        else
            player.Markers[marker] = total;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame.Id, holder, marker, total - before, total, frame.OwnerSeat, "skill-program.xunyi.marker"));
    }

    internal static int XunyiBranches(XunyiDamageReceipt receipt) => RuntimeSkills.RuntimeRules_wang_fu_zhao_lei_mobile_WangFuZhaoLei.XunyiBranches(receipt);
    internal static int XunyiStepCount(XunyiDamageReceipt receipt) => RuntimeSkills.RuntimeRules_wang_fu_zhao_lei_mobile_WangFuZhaoLei.XunyiStepCount(receipt);
    internal static (int Seat, bool Draw) XunyiStep(XunyiDamagePending pending) => RuntimeSkills.RuntimeRules_wang_fu_zhao_lei_mobile_WangFuZhaoLei.XunyiStep(pending);
    internal IReadOnlyList<PromptChoice> XunyiChoices(ProgramSkillFrame frame)
    {
        var seat = XunyiStep(frame.Xunyi!).Seat;
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, seat, seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, card, new(zone, seat), OwnedCardMoveIntent.Discard)).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "xunyi-discard" } }).ToArray());
    }

    internal void BeginNextXunyiPoint(ProgramSkillFrame frame)
    {
        var pending = frame.Xunyi!;
        if (pending.StepIndex >= XunyiStepCount(pending.Receipt))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XunyiDamageCompletedEvent(frame.Id, pending.Receipt.DamageFrameId, pending.StepIndex));
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Xunyi = null, PendingMovementContinuation = null });
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return;
        }

        var(seat, draw) = XunyiStep(pending);
        if (draw || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || XunyiChoices(frame).Count == 0)
        {
            PayXunyiPoint(frame, null, null);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Xunyi = pending with { Stage = XunyiStage.DiscardChoice }, Step = ResolutionFrameStep.AwaitingResponse });
        var choices = XunyiChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, "殉义：弃置一张牌。", choices.SelectMany(c => c.Cards).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.SettleXunyiDamagePoint)]
    internal PromptChoice XunyiAiChoice(ProgramSkillFrame frame) => XunyiChoices(frame).First();
    [ProgramAnswer("xunyi-discard")]
    internal void ResolveXunyiDiscard(PromptChoice choice)
    {
        var frame = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var seat = XunyiStep(frame.Xunyi!).Seat;
        if (frame.Xunyi!.Stage != XunyiStage.DiscardChoice || XunyiStep(frame.Xunyi).Draw || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != seat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], XunyiChoices(frame).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Xunyi discard lost its actual recipient authority.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        PayXunyiPoint(frame, card, from);
    }

    internal void PayXunyiPoint(ProgramSkillFrame frame, Card? card, CardLocation? from)
    {
        var pending = frame.Xunyi!;
        var(seat, draw) = XunyiStep(pending);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        pending = pending with
        {
            Stage = XunyiStage.Children,
            Before = before,
            After = before,
            Count = 0,
            CardId = card?.Id,
            From = from
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Xunyi = pending, PendingMovementContinuation = new(seat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var count = 0;
        if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
        {
            if (draw)
                count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 1, true, new("skill-program.xunyi.draw")).Count;
            else if (card is not null && from is not null)
            {
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, seat, card, from.Value, new("skill-program.xunyi.discard"));
                count = 1;
            }
        }

        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Xunyi = pending with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XunyiPointPaidEvent(frame.Id, pending.Receipt.DamageFrameId, pending.StepIndex, seat, draw, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count, card?.Id, from));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26320)]
    internal bool ResumeXunyi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Xunyi: { Stage: XunyiStage.Children } pending } frame || frame.Id != id)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Xunyi = pending with { StepIndex = pending.StepIndex + 1, Stage = XunyiStage.DiscardChoice, Before = 0, After = 0, Count = 0, CardId = null, From = null }, PendingMovementContinuation = null });
        BeginNextXunyiPoint(frame);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26320)]
    internal bool ReturnXunyiMovement(ProgramSkillFrame frame) => frame.Xunyi is not null && ResumeXunyi(frame.Id);
    SkillProgramStepOutcome IWangFuZhaoLeiProgramHost.ExecuteXunyi(ProgramSkillFrame frame, SkillProgramEffectOp operation) => ExecuteXunyi(frame, operation);
}
