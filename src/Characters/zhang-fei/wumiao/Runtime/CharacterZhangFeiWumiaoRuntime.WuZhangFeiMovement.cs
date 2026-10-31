#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiWumiaoRuntime
{
    internal const string WuZishengSkill = "wumiao:zisheng";
    internal const string WuZishengMovementBinding = global::CardGame.Core.RuntimeSkills.RuntimeRules_zhang_fei_wumiao_WuZhangFeiMovement.WuZishengMovementBinding;
    internal bool HasWuZishengMovementCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuZhangFeiMovementDamage);

    internal static bool IsWuZishengMovementTrigger(SkillProgramTrigger t) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhang_fei_wumiao_WuZhangFeiMovement.IsWuZishengMovementTrigger(t);
    internal WuZishengMovementSource[] CaptureWuZishengMovementSources() => !HasWuZishengMovementCapability || !_coreMatchSetupSetupCompleteAccess._setupComplete ? [] : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreNationalSkillsOperations.GetSkillBindingShard(p).ProgramInstances.Where(i => i.SkillId == WuZishengSkill && i.Program.Triggers.Any(IsWuZishengMovementTrigger)).Select(i => new WuZishengMovementSource(new(i.SkillId, WuZishengMovementBinding, p.Seat, i.SkillInstanceId), i.Program.GameplayHash))).OrderBy(s => s.Source.OwnerSeat).ThenBy(s => s.Source.SkillInstanceId, StringComparer.Ordinal).ToArray();
    internal bool ValidWuZishengMovementSource(WuZishengMovementSource s) => s.Source.SkillId == WuZishengSkill && s.Source.BindingId == WuZishengMovementBinding && _coreTableSeatQueriesOperations.IsValidPlayerSeat(s.Source.OwnerSeat) && _contentRegistryOperations._contentRegistry.GetSkill(WuZishengSkill).Program is { } p && p.GameplayHash == s.GameplayHash && p.Triggers.Any(IsWuZishengMovementTrigger);
    internal NativeLogicalDiscardInvocationDraft? AdmitWuZishengNativeDiscard(NativeLogicalDiscardInvocationDraft? draft)
    {
        if (draft is null || !HasWuZishengMovementCapability)
            return draft;
        foreach (var source in CaptureWuZishengMovementSources().Where(s => s.Source.OwnerSeat == draft.ExecutorSeat))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengDiscardAdmittedEvent(draft.InvocationId, draft.ParentFrameId, draft.TurnNumber, draft.InitiatorSeat, draft.ExecutorSeat, draft.SequenceBefore, draft.Reason, source));
        AdmitWuZishengDiscardMaterials(draft);
        return draft;
    }

    internal void AdmitWuZishengDiscardMaterials(NativeLogicalDiscardInvocationDraft draft)
    {
        if (!HasWuZishengMovementCapability || !_coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardAdmittedEvent>().Any(e => e.InvocationId == draft.InvocationId))
            return;
        for (var i = 0; i < draft.Materials.Count; i++)
        {
            var m = draft.Materials[i];
            var existing = _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardMaterialAdmittedEvent>().Where(e => e.InvocationId == draft.InvocationId && e.Index == i).ToArray();
            if (existing is [var prior])
            {
                if (prior.CardId != m.CardId || prior.From != m.From)
                    throw new InvalidOperationException("Zisheng's native primary material changed.");
                continue;
            }

            if (existing.Length != 0)
                throw new InvalidOperationException("Zisheng cannot qualify one primary material twice.");
            var producer = _resolutionStackOperations._resolutionStack.LastOrDefault();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengDiscardMaterialAdmittedEvent(draft.InvocationId, i, m.CardId, _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind, m.From, producer?.Id, (producer as ProgramSkillFrame)?.InstructionIndex));
        }
    }

    internal void CloseWuZishengNativeDiscard(NativeLogicalDiscardInvocationDraft? draft)
    {
        if (draft is null || !HasWuZishengMovementCapability || !_coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardAdmittedEvent>().Any(e => e.InvocationId == draft.InvocationId))
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardClosedEvent>().Any(e => e.InvocationId == draft.InvocationId))
            throw new InvalidOperationException("A Zisheng native discard invoice cannot close twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengDiscardClosedEvent(draft.InvocationId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal CardMovementBatchContext CaptureWuZishengMovementBatch(CardMovementBatchContext batch, IReadOnlyList<WuZishengMovementSource> sources)
    {
        if (!HasWuZishengMovementCapability)
            return batch;
        var qualified = new List<(WuZishengMovementSource Source, int Victim, WuZishengMovementMaterial Material)>();
        foreach (var admission in _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardAdmittedEvent>())
        {
            if (admission.TurnNumber != batch.TurnNumber || !ValidWuZishengMovementSource(admission.Source))
                continue;
            var closed = _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardClosedEvent>().SingleOrDefault(e => e.InvocationId == admission.InvocationId);
            foreach (var material in _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardMaterialAdmittedEvent>().Where(e => e.InvocationId == admission.InvocationId))
            {
                if (material.From.OwnerSeat is not { } victim || victim == admission.ExecutorSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(material.From))
                    continue;
                foreach (var move in batch.Movements.Where(m => m.CardId == material.CardId && m.CardKind == material.PrintedKind && m.Sequence > admission.SequenceBefore && (closed is null || m.Sequence <= closed.SequenceAfter)))
                {
                    var directEntry = move.From == material.From && move.Reason == admission.Reason && batch.ParentFrameId == material.ProducerFrameId;
                    if (directEntry && (move.To == CardLocation.Processing || move.To == CardLocation.DiscardPile || move.To == CardLocation.OutsideGame && move.From.Zone == CardZoneKind.Equipment))
                    {
                        var bound = _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardEntryBoundEvent>().Where(e => e.InvocationId == admission.InvocationId && e.MaterialIndex == material.Index).ToArray();
                        if (bound.Length == 0)
                            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengDiscardEntryBoundEvent(admission.InvocationId, material.Index, batch.Id, move.Sequence, material.ProducerFrameId));
                        else if (bound is not [var first] || first.MovementSequence != move.Sequence)
                            directEntry = false;
                    }

                    var terminal = directEntry && (move.To == CardLocation.DiscardPile || move.To == CardLocation.OutsideGame && move.From.Zone == CardZoneKind.Equipment);
                    if (!terminal && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && (move.Reason == admission.Reason || move.Reason.Value == admission.Reason.Value + "-finished" || move.Reason.Value == admission.Reason.Value + "-discard"))
                    {
                        var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == move.CardId && m.Sequence < move.Sequence);
                        terminal = entry is not null && entry.From == material.From && entry.To == CardLocation.Processing && entry.Reason == admission.Reason && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengDiscardEntryBoundEvent>().Any(e => e.InvocationId == admission.InvocationId && e.MaterialIndex == material.Index && e.MovementSequence == entry.Sequence && e.ProducerFrameId == material.ProducerFrameId) && batch.ParentFrameId == material.ProducerFrameId;
                    }

                    if (terminal)
                        qualified.Add((admission.Source, victim, new(move.CardId, move.CardKind, material.From, move.Sequence, true, admission.InvocationId)));
                }
            }
        }

        foreach (var move in batch.Movements.Where(m => m.To.Zone == CardZoneKind.Hand && m.To.OwnerSeat is not null && m.From != m.To))
        {
            var original = WuZishengObtainOriginalSource(batch, move);
            if (original?.OwnerSeat is not { } victim || victim == move.To.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(victim))
                continue;
            foreach (var source in sources.Where(s => s.Source.OwnerSeat == move.To.OwnerSeat && ValidWuZishengMovementSource(s)))
                qualified.Add((source, victim, new(move.CardId, move.CardKind, original.Value, move.Sequence, false)));
        }

        var list = qualified.GroupBy(e => (e.Source, e.Victim)).OrderBy(g => g.Key.Source.Source.OwnerSeat).ThenBy(g => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, g.Key.Victim)).ThenBy(g => g.Key.Source.Source.SkillInstanceId, StringComparer.Ordinal).Select(g => new WuZishengMovementQualification(g.Key.Source, batch.Id, g.Key.Victim) { Materials = g.Select(e => e.Material).GroupBy(m => m.CardId).Select(g => g.OrderBy(m => m.TerminalSequence).First()).ToArray() }).ToArray();
        foreach (var q in list)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengMovementQualifiedEvent(batch.Id, q));
        return list.Length == 0 ? batch : batch with
        {
            WuZisheng = list
        };
    }

    internal CardLocation? WuZishengObtainOriginalSource(CardMovementBatchContext batch, CardMovementRecord exit)
    {
        if (exit.From.OwnerSeat is not null)
            return exit.From;
        if (exit.From != CardLocation.Processing || batch.ParentFrameId is not { } parent)
            return null;
        var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == exit.CardId && m.Sequence < exit.Sequence);
        if (entry is null || entry.To != CardLocation.Processing || entry.From.OwnerSeat is not { } originalOwner || originalOwner == exit.To.OwnerSeat)
            return null;
        var entryBatch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.SingleOrDefault(b => b.Id != batch.Id && b.Movements.Contains(entry) && b.ParentFrameId == parent && b.ParentBatchId == batch.ParentBatchId && b.TurnNumber == batch.TurnNumber);
        if (entryBatch is null)
            return null;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(parent)is { CardKind: CardKind.Snatch } use && exit.To == CardLocation.Hand(use.SourceSeat) && use.TargetSeats.Contains(originalOwner) && entry.Reason == CardMoveReasons.Snatch && exit.Reason == CardMoveReasons.SnatchFinished && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == parent && e.SourceSeat == use.SourceSeat && e.CardKind == CardKind.Snatch) && _coreProgramEventHistoryOperations.ProgramEventHistory<TargetCardTakenEvent>().Any(e => e.ResolutionId == parent && e.SourceSeat == use.SourceSeat && e.TargetSeat == originalOwner && e.FromZone == entry.From.Zone && (e.PublicCardId is null || e.PublicCardId == exit.CardId)))
            return entry.From;
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == parent)is not ProgramSkillFrame program || program.InstructionIndex < 1 || batch.OriginOwnerSeat != program.OwnerSeat || batch.OriginSkillId != program.SkillId || batch.OriginSkillInstanceId != program.SkillInstanceId || entryBatch.OriginOwnerSeat != program.OwnerSeat || entryBatch.OriginSkillId != program.SkillId || entryBatch.OriginSkillInstanceId != program.SkillInstanceId || _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program?.GameplayHash != program.GameplayHash)
            return null;
        var effect = ProgramInstructionResolver.Default.Resolve(program, _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program!).GetPausedInstruction(program.InstructionIndex).Effect;
        return effect is { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.SelectedTargetHand, CardOwnerRef: { } ownerRef, TargetReference: { } targetRef } && entry.Reason == exit.Reason && effect.Zones.Contains(entry.From.Zone) && _coreProgramCardPaymentsOperations.ResolveProgramParticipant(program, ownerRef) == originalOwner && exit.To == CardLocation.Hand(_coreProgramCardPaymentsOperations.ResolveProgramParticipant(program, targetRef)) ? entry.From : null;
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectWuZishengMovementCandidates(CardMovementBatchContext batch)
    {
        for (var i = 0; i < (batch.WuZisheng?.Count ?? 0); i++)
        {
            var q = batch.WuZisheng![i];
            if (!ValidWuZishengQualification(q, batch) || !_coreMatchParticipantsPlayersAccess._players[q.Source.Source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[q.VictimSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[q.VictimSeat].HasHpValue || _coreMatchParticipantsPlayersAccess._players[q.VictimSeat].Hp <= 0)
                continue;
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(WuZishengSkill).Program!.Triggers.Single(IsWuZishengMovementTrigger);
            yield return new(q.Source.Source.OwnerSeat, WuZishengSkill, WuZishengMovementBinding, q.Source.Source.SkillInstanceId, q.Source.GameplayHash, trigger.Priority, i);
        }
    }

    internal bool ValidWuZishengQualification(WuZishengMovementQualification q, CardMovementBatchContext batch) => q.BatchId == batch.Id && ValidWuZishengMovementSource(q.Source) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(q.VictimSeat) && q.VictimSeat != q.Source.Source.OwnerSeat && q.Materials.Count > 0 && q.Materials.Select(m => m.CardId).Distinct().Count() == q.Materials.Count && q.Materials.All(m => m.OriginalFrom.OwnerSeat == q.VictimSeat && batch.Movements.Any(e => e.CardId == m.CardId && e.CardKind == m.PrintedKind && e.Sequence == m.TerminalSequence)) && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengMovementQualifiedEvent>().Count(e => e.BatchId == batch.Id && e.Qualification.Source == q.Source && e.Qualification.VictimSeat == q.VictimSeat && e.Qualification.Materials.SequenceEqual(q.Materials)) == 1;
    internal ProgramSkillWindowContext CreateWuZishengMovementContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate candidate)
    {
        var q = parent.Batch.WuZisheng![candidate.OccurrenceIndex];
        if (!ValidWuZishengQualification(q, parent.Batch) || q.Source.Source != new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId))
            throw new InvalidOperationException("Zisheng requires its exact causal victim and physical movement batch.");
        return new(SkillProgramTriggerWindow.CardsMoved, parent.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: q.VictimSeat, Amount: q.Materials.Count, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])with { MovedCardCount = q.Materials.Count }, MovementBatch: parent.Batch);
    }

    [ProgramIssuedTrigger(230)]
    internal bool IsIssuedWuZishengMovementCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != WuZishengSkill || candidate.BindingId != WuZishengMovementBinding || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || context.Window != SkillProgramTriggerWindow.CardsMoved || context.MovementBatch is not { } batch || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not CardsMovedTriggerWindowFrame parent || parent.Batch.Id != batch.Id || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != candidate || candidate.OccurrenceIndex < 0 || candidate.OccurrenceIndex >= (batch.WuZisheng?.Count ?? 0))
            return false;
        var q = batch.WuZisheng![candidate.OccurrenceIndex];
        return ValidWuZishengQualification(q, batch) && q.Source.Source == new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) && q.Source.GameplayHash == candidate.GameplayHash && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == candidate.OwnerSeat && context.TargetSeat == q.VictimSeat && context.Amount == q.Materials.Count && _coreMatchParticipantsPlayersAccess._players[q.VictimSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[q.VictimSeat].HasHpValue && _coreMatchParticipantsPlayersAccess._players[q.VictimSeat].Hp > 0;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29062, SkillProgramEffectOp.ResolveWuZhangFeiMovementDamage)]
    internal bool CanOfferWuZishengMovement(ProgramTriggerLegalityContext gate) => IsIssuedWuZishengMovementCandidate(gate.Candidate, gate.Context);
    internal bool ExactWuZishengMovementParent(ProgramSkillFrame f, out WuZishengMovementQualification q)
    {
        q = null !;
        if (f.SkillId != WuZishengSkill || f.TriggerId != WuZishengMovementBinding || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } c || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not CardsMovedTriggerWindowFrame parent || parent.Batch.Id != batch.Id || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        var i = parent.Candidates[parent.CandidateIndex].OccurrenceIndex;
        if (i < 0 || i >= (batch.WuZisheng?.Count ?? 0))
            return false;
        q = batch.WuZisheng![i];
        return ValidWuZishengQualification(q, batch) && q.Source.Source == new CardConversionSource(f.SkillId, f.TriggerId, f.OwnerSeat, f.SkillInstanceId) && q.Source.GameplayHash == f.GameplayHash && c.SourceSeat == f.OwnerSeat && c.TargetSeat == q.VictimSeat && c.Amount == q.Materials.Count;
    }

    SkillProgramStepOutcome global::CardGame.Core.IWuZhangFeiMovementHost.ResolveMovementDamage(ProgramSkillFrame frame) => ResolveWuZishengMovement(frame);
    internal SkillProgramStepOutcome ResolveWuZishengMovement(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!ExactWuZishengMovementParent(f, out var q) || f.WuZishengMovement is not null)
            throw new InvalidOperationException("Zisheng damage lost its original causal movement mother.");
        var victim = _coreMatchParticipantsPlayersAccess._players[q.VictimSeat];
        var amount = victim.IsAlive && victim.HasHpValue && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? Math.Min(q.Materials.Count, Math.Max(0, victim.Hp)) : 0;
        var context = f.WindowContext!;
        var suspended = amount > 0 ? CaptureWuZishengAncestorDying(f) : null;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZishengMovement = new(context.ParentFrameId, q, amount, victim.Hp, amount > 0) { SuspendedDying = suspended } });
        if (suspended is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengSuspendedDyingIssuedEvent(f.Id, suspended));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengMovementDamageIssuedEvent(f.Id, context.ParentFrameId, q.BatchId, q.Source.Source, q.Source.GameplayHash, q.VictimSeat, q.Materials.Count, victim.Hp, amount));
        if (amount == 0)
        {
            CompleteWuZishengMovement(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, q.VictimSeat, amount);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29062)]
    internal bool ResumeWuZishengMovement(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuZishengMovement: not null } f || f.Id != id)
            return false;
        if (f.AttackAttempt is not null)
        {
            _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(id));
            return true;
        }

        CompleteWuZishengMovement(f);
        return true;
    }

    internal void CompleteWuZishengMovement(ProgramSkillFrame f)
    {
        if (!ExactWuZishengMovementParent(f, out _) || f.WuZishengMovement is not { } r)
            throw new InvalidOperationException("Zisheng lost its paid damage return.");
        if (r.SuspendedDying is { } suspended)
        {
            if (!WuZishengSuspendsDying(f, suspended) || _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId != suspended.AttackOwnerFrameId)
                throw new InvalidOperationException("Zisheng cannot return to a changed original dying or attack cursor.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengSuspendedDyingReturnedEvent(f.Id, suspended.DyingFrameId, suspended.AttackOwnerFrameId));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZishengMovementCompletedEvent(f.Id, r.Qualification.BatchId, r.Qualification.VictimSeat, r.RequestedDamage));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZishengMovement = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29062)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29062)]
    internal bool ContinueIssuedWuZishengMovement(ProgramSkillFrame f) => f.InstructionIndex is 0 or 1 && ExactWuZishengMovementParent(f with { InstructionIndex = 1 }, out _);
    internal bool WuZishengMovementFirstChild(ProgramSkillFrame f, ResolutionFrame child) => f.WuZishengMovement is { Issued: true } && ExactWuZishengMovementParent(f, out _) && f.AttackAttempt is not null && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
    internal bool WuZhangFeiMovementObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _resolutionStackOperations2._resolutionStack()[child - 1] is ProgramSkillFrame parent && (_characterZhangFeiWumiaoOperations.WuZishengMovementFirstChild(parent, _resolutionStackOperations2._resolutionStack()[child]) || _characterZhangFeiWumiaoOperations.WuHaoxianFirstChild(parent, _resolutionStackOperations2._resolutionStack()[child])) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal ProgramSkillFrame? WuZhangFeiMovementObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhangFeiWumiaoOperations.WuZishengMovementFirstChild(f, child) || _characterZhangFeiWumiaoOperations.WuHaoxianFirstChild(f, child), (index) => _characterZhangFeiWumiaoOperations.WuZhangFeiMovementObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 770)]
    internal bool IsWuZhangFeiMovementDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && WuZhangFeiMovementObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29062)]
    internal bool AdvanceWuZhangFeiObserverChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuZhangFeiMovementObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);

    internal static string WuZishengCursorHash(ResolutionFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhang_fei_wumiao_WuZhangFeiMovement.WuZishengCursorHash(frame);
    internal WuZishengSuspendedDying? CaptureWuZishengAncestorDying(ProgramSkillFrame f)
    {
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not { } dying)
            return null;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == dying.ParentFrameId);
        if (parent is null || !WuZishengDyingPrefix(f, dying) || !_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, dying))
            throw new InvalidOperationException("Zisheng requires the exact native Dying-to-movement producer ancestry before nesting damage.");
        var attackOwner = _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack ? _resolutionStackOperations._resolutionStack.Single(x => x.Id == attack.ResolutionId) : null;
        return new(dying.Id, parent.Id, WuZishengCursorHash(dying), WuZishengCursorHash(parent), attackOwner?.Id, attackOwner is null ? null : WuZishengCursorHash(attackOwner));
    }

    internal bool WuZishengDyingPrefix(ProgramSkillFrame f, DyingFrame dying)
    {
        if (!ExactWuZishengMovementParent(f, out _) || f.WindowContext is not { } context)
            return false;
        var di = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id);
        var fi = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (di < 1 || fi <= di + 1 || _resolutionStackOperations._resolutionStack[di - 1].Id != dying.ParentFrameId || _resolutionStackOperations._resolutionStack[fi - 1].Id != context.ParentFrameId || _resolutionStackOperations._resolutionStack.Skip(di + 1).Take(fi - di - 1).Any(x => x is DyingFrame) || _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == dying.VictimSeat && e.KillerSeat == dying.KillerSeat) != 1)
            return false;
        for (var i = di + 1; i < fi; i++)
            if (!WuZishengSuspensionEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]))
                return false;
        return true;
    }

    internal bool WuZishengSuspensionEdge(ResolutionFrame parent, ResolutionFrame child) => _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    internal bool WuZishengSuspendsDying(ProgramSkillFrame f, WuZishengSuspendedDying s)
    {
        if (f.WuZishengMovement is not { Issued: true, SuspendedDying: { } claimed } r || claimed != s || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == s.DyingFrameId)is not DyingFrame dying || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == s.ParentFrameId)is not { } parent || parent.Id != dying.ParentFrameId || !WuZishengDyingPrefix(f, dying) || WuZishengCursorHash(dying) != s.DyingHash || WuZishengCursorHash(parent) != s.ParentHash || s.AttackOwnerFrameId is { } owner && (_resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == owner)is not { } attackOwner || WuZishengCursorHash(attackOwner) != s.AttackOwnerHash) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengSuspendedDyingIssuedEvent>().Count(e => e.FrameId == f.Id && e.Suspension == s) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WuZishengMovementDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.ParentWindowId == r.ParentWindowId && e.BatchId == r.Qualification.BatchId && e.Source == r.Qualification.Source.Source && e.RequestedDamage == r.RequestedDamage) != 1)
            return false;
        var fi = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        for (var i = fi + 1; i < _resolutionStackOperations._resolutionStack.Count; i++)
            if (!WuZishengSuspensionEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]))
                return false;
        return true;
    }

    internal bool IsOriginalDyingSuspendedByWuZishengMovement(DyingFrame dying) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.WuZishengMovement?.SuspendedDying is { } suspended && suspended.DyingFrameId == dying.Id && WuZishengSuspendsDying(f, suspended));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 4100)]
    internal void CollectWindow_CollectWuZishengMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectWuZishengMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1300)]
    internal bool ExcludeOrdinaryMovement_IsWuZishengMovementTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsWuZishengMovementTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1400)]
    internal ProgramSkillWindowContext? TryCreateWuZishengMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsWuZishengMovementTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateWuZishengMovementContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 300)]
    internal void CaptureMovementBatchStartWuZishengSources(CardMovementBatchBuilder batch) => batch.WuZishengSources = CaptureWuZishengMovementSources();
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 5400)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureWuZishengMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureWuZishengMovementBatch(completed, batch.WuZishengSources);
}
