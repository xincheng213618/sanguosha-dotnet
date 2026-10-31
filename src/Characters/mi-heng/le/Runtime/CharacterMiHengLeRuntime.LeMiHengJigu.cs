#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMiHengLeRuntime
{
    internal const string JiguDrawReason = "skill-program.mi-heng.jigu.draw";
    internal bool HasLeMiHengJiguCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeJiguInitialEntities) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw);

    internal static CardConversionSource JiguSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_mi_heng_le_LeMiHengJigu.JiguSource(f);
    internal static bool IsJiguDamageTrigger(SkillProgramTrigger t) => global::CardGame.Core.RuntimeSkills.RuntimeRules_mi_heng_le_LeMiHengJigu.IsJiguDamageTrigger(t);
    // Registration only; the query still belongs to legacy GameEngine.LeMiHengJigu.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 100)]
    internal void ProjectJiguCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasJiguEntityMark(cardId) ? "激鼓" : null);
    }

    internal bool HasJiguEntityMark(int cardId) => HasLeMiHengJiguCapability && _coreProgramEventHistoryOperations.ProgramEventHistory<JiguInitialEntityEvent>().Any(e => e.CardId == cardId);
    internal bool HasJiguEntityMark(int ownerSeat, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(ownerSeat) && HasJiguEntityMark(cardId);
    internal bool IsJiguHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:jigu") && HasJiguEntityMark(owner.Seat, cardId);
    internal int JiguMarkedHandCount(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count(c => HasJiguEntityMark(owner, c.Id));
    internal int JiguStartedSeatCount => _coreProgramEventHistoryOperations.ProgramEventHistory<ActualDiscardRecoveryPhaseStartedEvent>().Where(e => e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Preparation && e.Phase.ActorSeat == e.Phase.ActualTurnOwnerSeat).Select(e => e.Phase.ActorSeat).Distinct().Count();

    internal int JiguEmptySlotCount(CharacterState owner) => Enum.GetValues<EquipmentSlot>().Sum(slot => Math.Max(0, owner.EquipmentSlotCapacity(slot) - _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == slot)));
    internal int JiguUseCount(int owner, int round) => _coreProgramEventHistoryOperations.ProgramEventHistory<JiguActivationPaidEvent>().Count(e => e.Source.OwnerSeat == owner && e.Source.SkillId == "le:jigu" && e.RoundNumber == round);
    internal bool JiguRoundDisabled(int owner, int round) => _coreProgramEventHistoryOperations.ProgramEventHistory<JiguActivationPaidEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == "le:jigu" && e.RoundNumber == round && e.DisabledForRound);
    internal ProgramLifecycleTriggerWindowFrame CaptureJiguInitialEntities(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!HasLeMiHengJiguCapability || window.Window != SkillProgramTriggerWindow.GameStarting || window.Continuation != ProgramLifecycleContinuation.CompleteGameStarting)
            return window;
        if (window.JiguInitialEntities is not null)
            throw new InvalidOperationException("Jigu's actual opening Hand can only be frozen once.");
        var owners = window.Candidates.Where(c => _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects is [{ Op: SkillProgramEffectOp.InitializeJiguInitialEntities }]).Select(c => c.OwnerSeat).Distinct().Order();
        var entities = owners.SelectMany(owner => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Select(c =>
        {
            var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == c.Id && m.To == CardLocation.Hand(owner));
            if (paid is null || paid.CardKind != c.Kind)
                throw new InvalidOperationException("A Jigu initial entity requires its actual opening Hand entry.");
            return new JiguInitialEntity(owner, c.Id, c.Kind, paid.Sequence);
        })).ToArray();
        return window with
        {
            JiguInitialEntities = entities
        };
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28210, SkillProgramEffectOp.InitializeJiguInitialEntities, SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw)]
    internal bool CanOfferJiguGate(ProgramTriggerLegalityContext gate) => CanOfferJigu(gate.Candidate, gate.Trigger, gate.Context);
    internal bool CanOfferJigu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.InitializeJiguInitialEntities }])
            return context.Window == SkillProgramTriggerWindow.GameStarting && !_coreProgramEventHistoryOperations.ProgramEventHistory<JiguInitializedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat);
        if (!IsJiguDamageTrigger(trigger))
            return true;
        if (context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.OwnerSeat != candidate.OwnerSeat || context.OccurrenceIndex != 0 || candidate.OccurrenceIndex != 0 || context.Amount <= 0 || context.DamageFrameId is not { } damageId || context.TargetSeat != candidate.OwnerSeat && context.SourceSeat != candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || JiguRoundDisabled(candidate.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber) || JiguMarkedHandCount(candidate.OwnerSeat) != _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count || _coreProgramEventHistoryOperations.ProgramEventHistory<JiguActivationPaidEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.DamageFrameId == damageId) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not DamageTriggerWindowFrame w || w.TriggerWindow != context.Window || w.ParentFrameId != damageId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex].ToProgramCandidate() != candidate || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == damageId)is not DamageFrame d || d.TargetSeat != context.TargetSeat || d.Amount != context.Amount)
            return false;
        return _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(w)is { } attack && d.ParentFrameId == attack.ResolutionId && (attack.IsSourceLess ? context.SourceSeat is null : context.SourceSeat == attack.SourceSeat);
    }

    SkillProgramStepOutcome global::CardGame.Core.ILeMiHengJiguHost.Initialize(ProgramSkillFrame frame) => InitializeJigu(frame);
    SkillProgramStepOutcome global::CardGame.Core.ILeMiHengJiguHost.Draw(ProgramSkillFrame frame) => BeginJiguDraw(frame);
    internal SkillProgramStepOutcome InitializeJigu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "le:jigu" || f.TriggerId != "mark-initial-entities" || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.GameStarting) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreProgramEventHistoryOperations.ProgramEventHistory<JiguInitializedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Jigu initialization requires its original game-start binding exactly once.");
        // The native game-start mother freezes the final opening Hand before
        // any game-start skill can move it, including real opening replacements.
        var parent = (ProgramLifecycleTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(x => x.Id == f.WindowContext!.ParentFrameId);
        var entities = parent.JiguInitialEntities?.Where(e => e.OwnerSeat == f.OwnerSeat).ToArray() ?? throw new InvalidOperationException("Jigu requires the native mother's frozen initial entities.");
        if (entities.Select(e => e.CardId).Distinct().Count() != entities.Length || entities.Any(e => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.DealSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.To == CardLocation.Hand(e.OwnerSeat))))
            throw new InvalidOperationException("Jigu cannot mark an entity without its exact opening-Hand provenance.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiguInitializedEvent(f.Id, JiguSource(f), f.GameplayHash, entities.Length));
        foreach (var e in entities)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiguInitialEntityEvent(f.Id, f.OwnerSeat, e.CardId, e.PrintedKind, e.DealSequence));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome BeginJiguDraw(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Jigu is not null || f.SkillId != "le:jigu" || f.TriggerId != "damage-equipment-draw" || f.InstructionIndex != 1 || f.WindowContext is not { } c || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanOfferJigu(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Jigu requires one unspent native source-or-target damage occurrence.");
        var threshold = JiguStartedSeatCount;
        var use = JiguUseCount(f.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber) + 1;
        var r = new JiguReceipt(JiguSource(f), f.GameplayHash, c.DamageFrameId!.Value, c.ParentFrameId, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, JiguMarkedHandCount(f.OwnerSeat), _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count, JiguEmptySlotCount(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]), threshold, use, use >= threshold, JiguStage.Drawing)
        {
            Paid = true
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Jigu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(JiguPayment(f.Id, r));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【激鼓】，摸 {(r.EmptySlotCount)} 张牌。", f.OwnerSeat);
        ContinueJigu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static JiguActivationPaidEvent JiguPayment(long id, JiguReceipt r) => global::CardGame.Core.RuntimeSkills.RuntimeRules_mi_heng_le_LeMiHengJigu.JiguPayment(id, r);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw)]
    internal void ReturnJiguDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.Jigu is not { Paid: true, Stage: JiguStage.Drawing } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.EmptySlotCount || _coreProgramEventHistoryOperations.ProgramEventHistory<JiguDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Jigu cannot repeat or replace its paid native draw request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Jigu = r with { Stage = JiguStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiguDrawReturnedEvent(f.Id, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueJigu(f.Id);
    }

    internal bool DrainJigu(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueJigu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Jigu: { } r } f || f.Id != id)
            return;
        AssertJigu(f);
        if (r.Stage == JiguStage.Drawing)
        {
            if (f.NativeDraw is null)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.EmptySlotCount, new(JiguDrawReason));
            return;
        }

        if (DrainJigu(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiguCompletedEvent(f.Id, r.DamageFrameId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Jigu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28211)]
    internal bool ResumeJigu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Jigu: not null } f || f.Id != id)
            return false;
        ContinueJigu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28211)]
    internal bool ReturnJiguMovement(ProgramSkillFrame f)
    {
        if (f.Jigu is null)
            return false;
        ContinueJigu(f.Id);
        return true;
    }
}
