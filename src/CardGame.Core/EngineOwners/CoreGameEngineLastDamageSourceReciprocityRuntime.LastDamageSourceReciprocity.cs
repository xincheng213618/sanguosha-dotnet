#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityRuntime
{
    internal static string LastDamageSourceReason(ProgramSkillFrame f, bool draw) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_LastDamageSourceReciprocity.LastDamageSourceReason(f, draw);
    internal long LastDamageSourceMovementSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static bool IsLastDamageSourceTrigger(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_LastDamageSourceReciprocity.IsLastDamageSourceTrigger(trigger);
    internal static LastDamageSourceReciprocityDirection LastDamageSourceDirection(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_LastDamageSourceReciprocity.LastDamageSourceDirection(trigger);
    internal int? RecordedLastDamageSource(int owner, string skillId, string stateId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LastDamageSourceRecordedEvent>().LastOrDefault(e => e.OwnerSeat == owner && e.SkillId == skillId && e.StateId == stateId)?.SourceSeat;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 610)]
    internal void ObserveLastDamageSourceReciprocity(IGameEvent payload)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.LastDamageSourceReciprocity) || payload is not DamageAppliedEvent { Amount: > 0, SourceLess: false } applied || applied.SourceSeat == applied.TargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(applied.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(applied.TargetSeat) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || !attack.DamageWasApplied || attack.IsSourceLess || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || damage.Nature != applied.Nature || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat || attack.DamageAmount != applied.Amount || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Any(e => e.ResolutionId == damage.Id && e.SourceSeat == applied.SourceSeat && e.TargetSeat == applied.TargetSeat && e.Amount == applied.Amount && e.Nature == applied.Nature && !e.SourceLess && e.SourceCard == attack.EffectiveCardKind))
            return;
        foreach (var ownerSeat in new[]
        {
            applied.TargetSeat,
            applied.SourceSeat
        }

        )
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
            if (!owner.IsAlive)
                continue;
            var bindings = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.SelectMany(instance => instance.Program.Triggers.Where(IsLastDamageSourceTrigger).Select(trigger => (Instance: instance, Trigger: trigger))).GroupBy(x => (x.Instance.SkillId, StateId: x.Trigger.Effects[0].StateId!)).ToArray();
            foreach (var group in bindings)
            {
                var previous = RecordedLastDamageSource(ownerSeat, group.Key.SkillId, group.Key.StateId);
                var incoming = ownerSeat == applied.TargetSeat;
                if (incoming && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LastDamageSourceRecordedEvent>().Any(e => e.OwnerSeat == ownerSeat && e.SkillId == group.Key.SkillId && e.StateId == group.Key.StateId && e.DamageFrameId == damage.Id))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LastDamageSourceRecordedEvent(ownerSeat, group.Key.SkillId, group.Key.StateId, applied.SourceSeat, previous, damage.Id, damage.ParentFrameId, applied.Amount, applied.Nature, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
                if (!incoming && previous != applied.TargetSeat)
                    continue;
                var subject = incoming ? SkillProgramTriggerSubject.Owner : SkillProgramTriggerSubject.DamageSource;
                var binding = group.Where(x => x.Trigger.Subject == subject).OrderBy(x => x.Instance.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
                if (binding.Instance is null)
                    continue;
                var direction = LastDamageSourceDirection(binding.Trigger);
                if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LastDamageSourceReciprocityEligibleEvent>().Any(e => e.OwnerSeat == ownerSeat && e.SkillId == group.Key.SkillId && e.StateId == group.Key.StateId && e.DamageFrameId == damage.Id && e.Direction == direction))
                    continue;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LastDamageSourceReciprocityEligibleEvent(ownerSeat, group.Key.SkillId, group.Key.StateId, binding.Instance.SkillInstanceId, binding.Instance.Program.GameplayHash, binding.Trigger.Id, direction, damage.Id, damage.ParentFrameId, applied.SourceSeat, applied.TargetSeat, applied.Amount, applied.Nature, incoming ? applied.SourceSeat : previous!.Value));
            }
        }
    }

    internal LastDamageSourceReciprocityEligibleEvent? LastDamageSourceEligibility(int owner, string skillId, SkillProgramTrigger trigger, long damageId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LastDamageSourceReciprocityEligibleEvent>().SingleOrDefault(e => e.OwnerSeat == owner && e.SkillId == skillId && e.StateId == trigger.Effects[0].StateId && e.BindingId == trigger.Id && e.DamageFrameId == damageId && e.Direction == LastDamageSourceDirection(trigger));
    internal bool LastDamageSourceAlreadyIssued(LastDamageSourceReciprocityEligibleEvent e) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LastDamageSourceReciprocityStartedEvent>().Any(s => s.Source.OwnerSeat == e.OwnerSeat && s.Source.SkillId == e.SkillId && s.StateId == e.StateId && s.Direction == e.Direction && s.DamageFrameId == e.DamageFrameId);
    internal bool CanOfferLastDamageSourceReciprocity(int ownerSeat, string skillId, SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!IsLastDamageSourceTrigger(trigger))
            return true;
        if (attack.IsSourceLess || !attack.DamageWasApplied || attack.DamageAmount <= 0 || attack.SourceSeat == attack.TargetSeat || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.ParentFrameId != attack.ResolutionId)
            return false;
        return CanOfferLastDamageSourceEligibility(LastDamageSourceEligibility(ownerSeat, skillId, trigger, damage.Id));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1080)]
    internal bool CanOfferLastDamageSourceReciprocity(int ownerSeat, string skillId, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!IsLastDamageSourceTrigger(trigger))
            return true;
        if (context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, DamageFrameId: { } damageId, SourceSeat: { } source, TargetSeat: { } target } || source == target || context.OwnerSeat != ownerSeat)
            return false;
        var e = LastDamageSourceEligibility(ownerSeat, skillId, trigger, damageId);
        return e is not null && e.SourceSeat == source && e.TargetSeat == target && e.Amount == context.Amount && CanOfferLastDamageSourceEligibility(e);
    }

    internal bool CanOfferLastDamageSourceEligibility(LastDamageSourceReciprocityEligibleEvent? e) => e is not null && !LastDamageSourceAlreadyIssued(e) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(e.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[e.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[e.OwnerSeat], e.SkillId, e.SkillInstanceId) && (e.Direction == LastDamageSourceReciprocityDirection.DrawOwner || _coreMatchParticipantsPlayersAccess._players[e.SourceSeat].IsAlive && LastDamageSourceLegalMaterials(e.SourceSeat).Length > 0);
    internal LastDamageSourceReciprocityMaterial[] LastDamageSourceLegalMaterials(int seat) => new[]
    {
        CardLocation.Hand(seat),
        CardLocation.Equipment(seat)
    }.SelectMany(from => _cardZonesOperations._cardZones.CardsAt(from).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, card, from, OwnedCardMoveIntent.Discard)).Select(card => new LastDamageSourceReciprocityMaterial(card.Id, card.Kind, from, card.IsGeneralWeapon))).ToArray();
    internal SkillProgramStepOutcome BeginLastDamageSourceReciprocity(ProgramSkillFrame supplied, string stateId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        var trigger = plan.Trigger;
        if (trigger is null || !IsLastDamageSourceTrigger(trigger) || f.InstructionIndex != 1 || f.LastDamageSourceReciprocity is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || trigger.Effects[0].StateId != stateId || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, DamageFrameId: { } damageId } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.ParentFrameId != damageId || window.TriggerWindow != context.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("Last-source reciprocity lost its exact applied-damage candidate or first instruction.");
        var e = LastDamageSourceEligibility(f.OwnerSeat, f.SkillId, trigger, damageId);
        if (e is null || e.SkillInstanceId != f.SkillInstanceId || !CanOfferLastDamageSourceReciprocity(f.OwnerSeat, f.SkillId, trigger, context))
            return SkillProgramStepOutcome.Continue;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        if (attack.ResolutionId != e.AttackFrameId || attack.IsSourceLess || !attack.DamageWasApplied || attack.SourceSeat != e.SourceSeat || attack.TargetSeat != e.TargetSeat || attack.DamageAmount != e.Amount || _coreNativeCardClassificationsOperations.GetDamageNature(attack) != e.Nature)
            throw new InvalidOperationException("Last-source reciprocity lost its real positive damage producer.");
        var participant = e.Direction == LastDamageSourceReciprocityDirection.DrawOwner ? f.OwnerSeat : e.SourceSeat;
        var r = new ProgramLastDamageSourceReceipt
        {
            InstructionIndex = f.InstructionIndex,
            Source = new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            StateId = stateId,
            Direction = e.Direction,
            DamageWindowId = window.Id,
            DamageFrameId = damageId,
            AttackFrameId = e.AttackFrameId,
            SourceSeat = e.SourceSeat,
            TargetSeat = e.TargetSeat,
            Amount = e.Amount,
            Nature = e.Nature,
            ParticipantSeat = participant,
            RecordedSourceSeat = e.RecordedSourceSeat,
            Stage = e.Direction == LastDamageSourceReciprocityDirection.DrawOwner ? LastDamageSourceReciprocityStage.MovementChildren : LastDamageSourceReciprocityStage.ChoosingDiscard,
            EligibleMaterials = e.Direction == LastDamageSourceReciprocityDirection.DiscardSource ? LastDamageSourceLegalMaterials(participant) : []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LastDamageSourceReciprocity = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LastDamageSourceReciprocityStartedEvent(f.Id, r.Source, r.GameplayHash, stateId, r.Direction, r.DamageWindowId, r.DamageFrameId, r.AttackFrameId, r.SourceSeat, r.TargetSeat, r.Amount, r.Nature, participant, r.RecordedSourceSeat));
        if (r.Direction == LastDamageSourceReciprocityDirection.DiscardSource)
        {
            PublishLastDamageSourceDiscard(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var before = LastDamageSourceMovementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LastDamageSourceReciprocity = r with { MovementIssued = true, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(participant, 0, null) });
        var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[participant], 1, true, new(LastDamageSourceReason(f, true))).Count;
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { LastDamageSourceReciprocity = current.LastDamageSourceReciprocity!with { ActualCount = actual, SequenceAfter = LastDamageSourceMovementSequence } });
        QueueLastDamageSourceMovementIssued(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> LastDamageSourceDiscardChoices(ProgramSkillFrame f) => Array.AsReadOnly(f.LastDamageSourceReciprocity!.EligibleMaterials.Select(m => new PromptChoice(new($"last-damage-source.{(f.Id)}.{(m.CardId)}"), $"弃置【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】", [m.CardId], [], new Dictionary<string, string> { ["program-action"] = "last-damage-source-discard", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishLastDamageSourceDiscard(ProgramSkillFrame f) => _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.LastDamageSourceReciprocity!.ParticipantSeat, LastDamageSourceDiscardChoices(f), "选择并弃置一张自己的手牌或装备牌。", f.LastDamageSourceReciprocity.ParticipantSeat);
    [ProgramAnswer("last-damage-source-discard")]
    internal void ResolveLastDamageSourceReciprocityChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Last-source discard lost its owning program.");
        _coreLastDamageSourceReciprocityObserversOperations.AssertLastDamageSourceReciprocity(f);
        var r = f.LastDamageSourceReciprocity!;
        if (r.Stage != LastDamageSourceReciprocityStage.ChoosingDiscard || r.MovementIssued || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != r.ParticipantSeat || choice.Parameters.GetValueOrDefault("program-action") != "last-damage-source-discard" || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Cards is not [var id] || choice.Targets.Count != 0 || !LastDamageSourceDiscardChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(choice.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Last-source discard changed its private chooser, frozen entity or owning frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.ParticipantSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "伤害后弃牌在付款前失去原技能资格或实际参与者。");
            return;
        }

        var material = r.EligibleMaterials.Single(m => m.CardId == id);
        if (!LastDamageSourceLegalMaterials(r.ParticipantSeat).Contains(material))
            throw new InvalidOperationException("Last-source discard must pay the exact still-owned eligible entity.");
        var before = LastDamageSourceMovementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LastDamageSourceReciprocity = r with { Stage = LastDamageSourceReciprocityStage.MovementChildren, MovementIssued = true, SequenceBefore = before, SequenceAfter = before, PaidMaterial = material }, PendingMovementContinuation = new(r.ParticipantSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.ParticipantSeat, [id], new(LastDamageSourceReason(f, false)), (batch, records) =>
        {
            if (records is not [var paid] || paid.CardId != material.CardId || paid.CardKind != material.PrintedKind || paid.From != material.From || paid.To != (material.IsGeneralWeapon && material.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile))
                throw new InvalidOperationException("Last-source discard did not produce its single actual payment.");
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { LastDamageSourceReciprocity = current.LastDamageSourceReciprocity!with { ActualCount = 1, SequenceAfter = paid.Sequence, BatchId = batch } });
            QueueLastDamageSourceMovementIssued(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void QueueLastDamageSourceMovementIssued(ProgramSkillFrame f)
    {
        var r = f.LastDamageSourceReciprocity!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LastDamageSourceReciprocityMovementIssuedEvent(f.Id, r.ParticipantSeat, r.Direction, r.ActualCount, r.SequenceBefore, r.SequenceAfter, r.BatchId));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 90)]
    internal bool ResumeLastDamageSourceReciprocity(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.LastDamageSourceReciprocity is not { } r)
            return false;
        _coreLastDamageSourceReciprocityObserversOperations.AssertLastDamageSourceReciprocity(f);
        if (r.Stage == LastDamageSourceReciprocityStage.ChoosingDiscard)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishLastDamageSourceDiscard(f);
            return true;
        }

        if (r.Stage == LastDamageSourceReciprocityStage.MovementChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LastDamageSourceReciprocity = r with { Stage = LastDamageSourceReciprocityStage.Complete }, PendingMovementContinuation = null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LastDamageSourceReciprocityCompletedEvent(f.Id, r.ActualCount));
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 70)]
    internal bool ReturnLastDamageSourceReciprocityMovement(ProgramSkillFrame f)
    {
        if (f.LastDamageSourceReciprocity is not { MovementIssued: true } || f.PendingMovementContinuation is null)
            return false;
        _coreLastDamageSourceReciprocityObserversOperations.AssertLastDamageSourceReciprocity(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 100)]
    internal bool IsLastDamageSourceReciprocityMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.LastDamageSourceReciprocity is { } receipt && effect is { Op: SkillProgramEffectOp.LastDamageSourceReciprocity } && effect.StateId == receipt.StateId && f.PendingMovementContinuation == pending && pending.SubjectSeat == receipt.ParticipantSeat && _coreLastDamageSourceReciprocityObserversOperations.ValidLastDamageSourceReceipt(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 640)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 660)]
    internal bool CanContinueLastDamageSourceReciprocity(ProgramSkillFrame f) => f.LastDamageSourceReciprocity is { MovementIssued: true } && _coreLastDamageSourceReciprocityObserversOperations.ValidLastDamageSourceReceipt(f);
    [ProgramAiChoice(SkillProgramEffectOp.LastDamageSourceReciprocity)]
    internal PromptChoice SelectAiLastDamageSourceReciprocity(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.LastDamageSourceReciprocity!.ParticipantSeat])).ThenBy(c => c.Cards.Single()).First();
    SkillProgramStepOutcome global::CardGame.Core.ILastDamageSourceReciprocityProgramHost.LastDamageSourceReciprocity(ProgramSkillFrame f, string stateId) => BeginLastDamageSourceReciprocity(f, stateId);
}
