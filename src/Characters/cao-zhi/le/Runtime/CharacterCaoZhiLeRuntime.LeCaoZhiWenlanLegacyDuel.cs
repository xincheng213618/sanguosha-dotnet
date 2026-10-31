#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhiLeRuntime
{
    // StartVirtualDuel deliberately owns its attack on the producing Program
    // frame. Its real instruction is the identity; it has no CardUse/Action to
    // manufacture. This adapter returns only after that native attack finishes.
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureVirtualDuelLegacy, 1)]
    internal void CaptureWenlanDirectDuel(ProgramSkillFrame supplied, int source, int target)
    {
        if (!HasLeCaoZhiWenlanCapability || CurrentWenlanSource(source, SkillProgramTriggerWindow.CardUseCompleted)is not { } owned)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || f.LeCaoZhiWenlanDirectDuel is not null || _coreProgramVirtualDuelOperations.FrozenProgramDuelInstructionIndex(f.Id) != f.InstructionIndex || f.CardAttack is not { Active: true, EffectiveCardKind: CardKind.Duel, CardId: null, PhysicalCardIds.Count: 0 } attack || attack.ProgramSkillFrameId != f.Id || attack.SourceSeat != source || attack.TargetSeat != target || f.Continuations.Duel is not { Active: true } || WenlanEntries(source).Any(e => e.NativeFrameId == f.Id && e.ProgramInstructionIndex == f.InstructionIndex))
            throw new InvalidOperationException("Wenlan direct-Duel acceptance requires the exact newly issued native instruction.");
        var producer = new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var entry = new WenlanActionAcceptedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, source, NextWenlanOrdinal(source), 0, f.Id, CardActionType.Use, source, CardKind.Duel, new(owned, 0, null))
        {
            ProgramInstructionIndex = f.InstructionIndex
        };
        var receipt = new WenlanDirectDuelReceipt(f.Id, f.InstructionIndex, producer, f.GameplayHash, source, target, entry.EntryId, WenlanDirectDuelStage.Running);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { LeCaoZhiWenlanDirectDuel = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(entry);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanDirectDuelAcceptedEvent(f.Id, f.InstructionIndex, entry.EntryId, source, target, producer, f.GameplayHash));
    }

    internal bool WenlanDirectProducerMatches(ProgramSkillFrame parent, WenlanDirectDuelReceipt r)
    {
        if (parent.Id != r.ProducerFrameId || parent.InstructionIndex != r.InstructionIndex || parent.GameplayHash != r.ProducerGameplayHash || r.ProducerSource != new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } program || program.GameplayHash != parent.GameplayHash || _coreProgramVirtualDuelOperations.FrozenProgramDuelInstructionIndex(parent.Id) != r.InstructionIndex || parent.CardAttack is not { EffectiveCardKind: CardKind.Duel, CardId: null, PhysicalCardIds.Count: 0 } state || state.ProgramSkillFrameId != parent.Id || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.SourceSeat == r.TargetSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanDirectDuelAcceptedEvent>().Count(e => e == new WenlanDirectDuelAcceptedEvent(parent.Id, r.InstructionIndex, r.EntryId, r.SourceSeat, r.TargetSeat, r.ProducerSource, r.ProducerGameplayHash)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanActionAcceptedEvent>().Where(e => e.EntryId == r.EntryId).ToArray()is not [var entry] || entry.OwnerSeat != r.SourceSeat || entry.NativeFrameId != parent.Id || entry.ProgramInstructionIndex != r.InstructionIndex || entry.ActionId != 0 || entry.EffectiveKind != CardKind.Duel || entry.ActionType != CardActionType.Use || entry.ProviderSeat != r.SourceSeat || entry.Capture.MaterialCount != 0 || entry.Capture.FuEntity is not null || entry.Capture.Source?.Source.OwnerSeat != r.SourceSeat)
            return false;
        if (parent.TriggerId is null)
            return parent.SelectedTargetSeats.SequenceEqual([r.SourceSeat, r.TargetSeat]);
        return parent.WindowContext?.Window == SkillProgramTriggerWindow.PlayPhaseStarting && parent.OwnerSeat == r.SourceSeat && parent.SelectedTargetSeats.SequenceEqual([r.TargetSeat]);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.BeforeParentResume, 29081)]
    internal bool ReturnWenlanDirectDuel(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ProgramFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LeCaoZhiWenlanDirectDuel: { } r } parent || parent.Id != parentId)
            return false;
        if (completion.ResolutionId != parent.Id)
            return false;
        if (r.Stage != WenlanDirectDuelStage.Running || !WenlanDirectProducerMatches(parent, r) || completion.ResolutionId != parent.Id || completion.DelayedTurn || completion.DelayedTurnSeat is not null || completion.RequestedSlashChild || parent.CardAttack is not { Active: false } state || state.TargetSeat != completion.FinalTargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(completion.FinalTargetSeat) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || _coreDyingCursorRuntimeOperations.ActiveDying is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanDirectDuelReturnedEvent>().Any(e => e.EntryId == r.EntryId))
            throw new InvalidOperationException("Wenlan direct-Duel return lost its completely resolved native attack and original instruction.");
        r = r with
        {
            Stage = WenlanDirectDuelStage.Returned,
            DamageApplied = completion.DamageWasApplied,
            FinalTargetSeat = completion.FinalTargetSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent = parent with { LeCaoZhiWenlanDirectDuel = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanDirectDuelReturnedEvent(parent.Id, r.InstructionIndex, r.EntryId, completion.DamageWasApplied, completion.FinalTargetSeat));
        var entries = WenlanEntries(r.SourceSeat);
        var second = WenlanEntry(r.EntryId);
        var first = entries.SingleOrDefault(e => e.Ordinal == second.Ordinal - 1);
        if (second.Ordinal % 2 != 0 || first is null || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || CurrentWenlanSource(r.SourceSeat, SkillProgramTriggerWindow.CardUseCompleted)is not { } source || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Any(e => e.Qualification.SecondEntryId == second.EntryId))
        {
            CloseWenlanDirectDuelParent(parent, r, null);
            return false;
        }

        var q = new WenlanPairQualification(source, first.EntryId, second.EntryId, 0, SkillProgramTriggerWindow.CardUseCompleted)
        {
            DirectDuelEntryId = second.EntryId,
            SecondOrdinal = second.Ordinal
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanPairQualifiedEvent(q));
        var candidate = new ProgramTriggerCandidate(r.SourceSeat, WenlanSkill, WenlanUseBinding, source.Source.SkillInstanceId, source.GameplayHash, 0);
        var windowId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.CardUseCompleted, windowId, r.SourceSeat, SourceSeat: r.SourceSeat, TargetSeat: r.TargetSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[r.SourceSeat]));
        r = r with
        {
            Stage = WenlanDirectDuelStage.WenlanWindow,
            WindowId = windowId
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { LeCaoZhiWenlanDirectDuel = r });
        var window = new WenlanDirectDuelWindowFrame(windowId, parent.Id, r, candidate, context, q);
        _coreRuntimeOperations.PushRuntimeFrame(window);
        // This source has one mandatory instruction. No synthetic optional prompt
        // or broad card-action window is needed around the native direct return.
        if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(candidate, context))
        {
            _coreRuntimeOperations.PopResolutionFrame(window.Id, ResolutionFrameKind.LeCaoZhiWenlanDirectDuelWindow);
            CloseWenlanDirectDuelParent(_coreProgramLifecycleOperations.GetActiveProgramFrame(parent.Id), r, false);
            return false;
        }

        _coreProgramLifecycleOperations.BeginProgramBinding(candidate, context);
        return true;
    }

    internal void CloseWenlanDirectDuelParent(ProgramSkillFrame parent, WenlanDirectDuelReceipt r, bool? completed)
    {
        if (!WenlanDirectProducerMatches(parent, r) || parent.LeCaoZhiWenlanDirectDuel != r || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanDirectDuelReturnedEvent>().Count(e => e == new WenlanDirectDuelReturnedEvent(parent.Id, r.InstructionIndex, r.EntryId, r.DamageApplied, r.FinalTargetSeat ?? -1)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanDirectDuelResumedEvent>().Any(e => e.EntryId == r.EntryId))
            throw new InvalidOperationException("Wenlan cannot resume another or already returned direct-Duel instruction.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { LeCaoZhiWenlanDirectDuel = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanDirectDuelResumedEvent(parent.Id, r.InstructionIndex, r.EntryId, completed));
    }

    internal bool WenlanDirectWindowMatches(WenlanDirectDuelWindowFrame w)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == w.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || parent.Id != w.ParentFrameId || parent.LeCaoZhiWenlanDirectDuel != w.Duel || !WenlanDirectProducerMatches(parent, w.Duel) || parent.CardAttack is not { Active: false } || w.Duel.Stage != WenlanDirectDuelStage.WenlanWindow || w.Duel.WindowId != w.Id || w.Qualification.DirectDuelEntryId != w.Duel.EntryId || w.Qualification.SecondEntryId != w.Duel.EntryId || w.Qualification.ActionId != 0 || w.Qualification.Window != SkillProgramTriggerWindow.CardUseCompleted || w.Context != new ProgramSkillWindowContext(SkillProgramTriggerWindow.CardUseCompleted, w.Id, w.Duel.SourceSeat, SourceSeat: w.Duel.SourceSeat, TargetSeat: w.Duel.TargetSeat, Facts: w.Context.Facts) || w.Candidate != new ProgramTriggerCandidate(w.Duel.SourceSeat, WenlanSkill, WenlanUseBinding, w.Qualification.Source.Source.SkillInstanceId, w.Qualification.Source.GameplayHash, 0) || w.Qualification.Source != new WenlanSource(new(WenlanSkill, WenlanUseBinding, w.Duel.SourceSeat, w.Candidate.SkillInstanceId), w.Candidate.GameplayHash) || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanPairQualifiedEvent>().Count(e => e.Qualification == w.Qualification) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanDirectDuelReturnedEvent>().Count(e => e == new WenlanDirectDuelReturnedEvent(parent.Id, w.Duel.InstructionIndex, w.Duel.EntryId, w.Duel.DamageApplied, w.Duel.FinalTargetSeat ?? -1)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanDirectDuelResumedEvent>().Any(e => e.EntryId == w.Duel.EntryId))
            return false;
        var first = WenlanEntry(w.Qualification.FirstEntryId);
        var second = WenlanEntry(w.Duel.EntryId);
        return first.OwnerSeat == w.Duel.SourceSeat && second.OwnerSeat == w.Duel.SourceSeat && second.Ordinal % 2 == 0 && first.Ordinal + 1 == second.Ordinal;
    }

    internal WenlanPairQualification? WenlanDirectQualificationFor(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is WenlanDirectDuelWindowFrame w && WenlanDirectWindowMatches(w) && w.Candidate == c && w.Context == context ? w.Qualification : null;
    internal bool CanRunWenlanDirectDuelCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.CardUse is not null || context.Window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.LastOrDefault()is not WenlanDirectDuelWindowFrame window || window.Id != context.ParentFrameId || WenlanDirectQualificationFor(candidate, context)is not { } qualification || qualification.SecondOrdinal < 2 || qualification.SecondOrdinal % 2 != 0 || qualification.SecondOrdinal != WenlanEntry(qualification.SecondEntryId).Ordinal || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || CurrentWenlanSource(candidate.OwnerSeat, SkillProgramTriggerWindow.CardUseCompleted) != qualification.Source || _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program is not { } program || !LeCaoZhiComposition.IsWenlanProgram(program) || program.GameplayHash != candidate.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Any(e => e.Qualification.SecondEntryId == qualification.SecondEntryId))
            return false;
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        return trigger.Id == WenlanUseBinding && trigger.Window == context.Window && trigger.Priority == candidate.Priority && !trigger.Optional && trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveLeCaoZhiWenlan }];
    }

    internal bool TryGetWenlanDirectWindow(ProgramSkillFrame f, out WenlanDirectDuelWindowFrame window)
    {
        window = null !;
        if (f.WindowContext is not { CardUse: null } context || _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == context.ParentFrameId)is not WenlanDirectDuelWindowFrame w || !WenlanDirectWindowMatches(w) || w.Context != context || w.Candidate.OwnerSeat != f.OwnerSeat || w.Candidate.SkillId != f.SkillId || w.Candidate.BindingId != f.TriggerId || w.Candidate.SkillInstanceId != f.SkillInstanceId || w.Candidate.GameplayHash != f.GameplayHash)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != w.Id)
            return false;
        window = w;
        return true;
    }

    internal bool WenlanNativeParentMatches(ProgramSkillFrame f, WenlanReceipt r, ProgramSkillWindowContext context)
    {
        if (r.NativeParentKind == WenlanNativeParentKind.DirectDuel)
            return TryGetWenlanDirectWindow(f, out var w) && w.Id == r.WindowId && w.ParentFrameId == r.NativeParentId && w.Qualification == r.Qualification;
        return r.NativeParentKind == WenlanNativeParentKind.CardAction && _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == r.WindowId)is ProgramCardTriggerWindowFrame window && window.ParentFrameId == r.NativeParentId && WenlanWindowMatches(window, r.Qualification) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, window);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 2)]
    internal bool TryReturnWenlanDirectDuelBinding(ProgramSkillFrame f, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not WenlanDirectDuelWindowFrame w || f.WindowContext?.ParentFrameId != w.Id)
            return false;
        if (!WenlanDirectWindowMatches(w) || f.WindowContext != w.Context || f.SkillId != WenlanSkill || f.TriggerId != WenlanUseBinding || f.OwnerSeat != w.Candidate.OwnerSeat || f.SkillInstanceId != w.Candidate.SkillInstanceId || f.GameplayHash != w.Candidate.GameplayHash || completed && _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanCompletedEvent>().Count(e => e.FrameId == f.Id && e.SecondEntryId == w.Duel.EntryId) != 1)
            throw new InvalidOperationException("Wenlan direct-Duel child lost its single typed binding return.");
        _coreRuntimeOperations.PopResolutionFrame(w.Id, ResolutionFrameKind.LeCaoZhiWenlanDirectDuelWindow);
        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(w.ParentFrameId);
        CloseWenlanDirectDuelParent(parent, w.Duel, completed);
        _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        return true;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.LegacyWindow, 20, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveWenlanLegacy(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolutionStackOperations._resolutionStack.OfType<WenlanDirectDuelWindowFrame>().Any(window => window.Id == context.ParentFrameId) ? CanRunWenlanDirectDuelCandidate(candidate, context) : null;
}
