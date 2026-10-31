#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLiejieSourceDiscardsRuntime
{
    internal long LiejieMoveSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static string LiejieDiscardReason(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_Core_LiejieSourceDiscards.LiejieDiscardReason(frame);
    internal bool IsLiejieDiscardDestination(CardMovementRecord movement) => movement.To == CardLocation.DiscardPile || movement.From.Zone == CardZoneKind.Equipment && movement.To == CardLocation.OutsideGame && (_coreCardAttackStateOperations.GetAttackCard(movement.CardId).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(movement));
    internal bool IsAtomicLiejieCostMove(ProgramSkillFrame frame, string sourceBind)
    {
        if (frame.TriggerId is null)
            return false;
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(frame);
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.LiejieSourceDiscard && e.SourceBind == sourceBind))
            return false;
        LiejieSourceDiscardContract.ValidateTrigger(frame.SkillId, trigger);
        var binding = frame.CardSetBindings.Single(b => b.Name == sourceBind);
        return frame.InstructionIndex == 2 && ExactLiejieParent(frame) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramLiejieCostColorFrozenEvent>().Count(e => e.FrameId == frame.Id && e.SourceBind == sourceBind && e.SkillInstanceId == frame.SkillInstanceId && e.GameplayHash == frame.GameplayHash && e.BeforeMoveSequence == LiejieMoveSequence && e.Count == binding.CardIds.Count) == 1;
    }

    internal void FreezeLiejieDiscardColors(ProgramSkillFrame frame, string sourceBind, IReadOnlyList<int> selectedIds, IReadOnlyList<CardLocation> selectedLocations)
    {
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (!plan.Instructions.Any(e => e.Op == SkillProgramEffectOp.LiejieSourceDiscard && e.SourceBind == sourceBind))
            return;
        LiejieSourceDiscardContract.ValidateTrigger(frame.SkillId, plan.Trigger!);
        var bound = frame.CardSetBindings.SingleOrDefault(b => b.Name == sourceBind);
        if (frame.InstructionIndex != 2 || !ExactLiejieParent(frame) || bound is null || !selectedIds.SequenceEqual(bound.CardIds) || !selectedLocations.SequenceEqual(bound.SourceLocations) || selectedIds.Count is < 1 or > 3 || selectedLocations.Count != selectedIds.Count || selectedIds.Distinct().Count() != selectedIds.Count || selectedLocations.Any(l => l.OwnerSeat != frame.OwnerSeat || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramLiejieCostColorFrozenEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Liejie colors require the exact unpaid owner HE binding.");
        var red = selectedIds.Select((id, i) =>
        {
            if (_cardZonesOperations._cardZones.GetLocation(id) != selectedLocations[i])
                throw new InvalidOperationException("Liejie cost left its original HE source.");
            return CoreNativeCardClassificationsRuntime.IsRedSuit(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _cardZonesOperations._cardZones.CardsAt(selectedLocations[i]).Single(c => c.Id == id)));
        }).Count(value => value);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramLiejieCostColorFrozenEvent(frame.Id, sourceBind, frame.SkillInstanceId, frame.GameplayHash, selectedIds.Count, red, LiejieMoveSequence));
    }

    internal bool ExactLiejieParent(ProgramSkillFrame frame)
    {
        if (frame.TriggerId is null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } context || context.OwnerSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || frame.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.TriggerWindow != context.Window || parent.TargetSeat != frame.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(f => f.Id == parent.ParentFrameId)is not { } damage || damage.Id != context.DamageFrameId || damage.SourceSeat != parent.SourceSeat || damage.TargetSeat != frame.OwnerSeat || damage.Amount != context.Amount)
            return false;
        // Native children may own the current attack. The original applied damage
        // stays tied to its immutable requested fact and live outer damage frame.
        var requests = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Where(e => e.ResolutionId == damage.Id).ToArray();
        if (requests is not [var request] || request.SourceSeat != damage.SourceSeat || request.TargetSeat != damage.TargetSeat || request.Amount != damage.Amount || request.Nature != damage.Nature || context.SourceSeat != (request.SourceLess ? null : damage.SourceSeat))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.BindingId == frame.TriggerId && e.SkillInstanceId == frame.SkillInstanceId && e.OwnerSeat == frame.OwnerSeat && e.Window == context.Window) == 1;
    }

    internal ProgramLiejieCostColorFrozenEvent ExactLiejiePaidColors(ProgramSkillFrame frame, string bind)
    {
        var bound = frame.CardSetBindings.Single(b => b.Name == bind);
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramLiejieCostColorFrozenEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (facts is not [var colors] || colors.SourceBind != bind || colors.SkillInstanceId != frame.SkillInstanceId || colors.GameplayHash != frame.GameplayHash || colors.Count != bound.CardIds.Count || colors.Count is < 1 or > 3 || colors.RedCount < 0 || colors.RedCount > colors.Count || bound.SourceLocations.Count != colors.Count)
            throw new InvalidOperationException("Liejie lost its frozen first-clause color receipt.");
        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > colors.BeforeMoveSequence && m.Reason.Value == $"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.MoveBoundCards)}" && bound.CardIds.Contains(m.CardId)).ToArray();
        if (moves.Length != colors.Count || moves.Select(m => m.CardId).Distinct().Count() != colors.Count || moves.Any(m => m.From != bound.SourceLocations[bound.CardIds.ToList().IndexOf(m.CardId)] || m.From.OwnerSeat != frame.OwnerSeat || !IsLiejieDiscardDestination(m)))
            throw new InvalidOperationException("Liejie colors require its completed original HE discard invoice.");
        return colors;
    }

    internal SkillProgramStepOutcome BeginLiejieSourceDiscard(ProgramSkillFrame input, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (!ExactLiejieParent(frame) || frame.InstructionIndex != 4 || effect.Op != SkillProgramEffectOp.LiejieSourceDiscard || frame.LiejieDiscardPending is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramLiejieSelectionStartedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Liejie requires its exact unpaid damage-source selection.");
        var colors = ExactLiejiePaidColors(frame, effect.SourceBind!);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || colors.RedCount == 0 || frame.WindowContext!.SourceSeat is not { } source || source == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var cards = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, source, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(c => ResolveLiejieSlot(source, c)).ToArray();
        if (cards.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var draft = new LiejieDiscardPendingState(source, Math.Min(colors.RedCount, cards.Length))
        {
            InstructionIndex = frame.InstructionIndex,
            ParentFrameId = frame.WindowContext.ParentFrameId,
            FrozenRedCount = colors.RedCount,
            CandidateIds = cards.Select(c => c.Id).ToArray(),
            CandidateLocations = cards.Select(c => _cardZonesOperations._cardZones.GetLocation(c.Id)).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LiejieDiscardPending = draft });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramLiejieSelectionStartedEvent(frame.Id, draft.InstructionIndex, draft.ParentFrameId, source, draft.FrozenRedCount, draft.MaximumCount));
        PublishLiejieDiscard(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Card ResolveLiejieSlot(int source, PromptChoice choice)
    {
        if (choice.Parameters.GetValueOrDefault("card-owner-seat") != source.ToString(System.Globalization.CultureInfo.InvariantCulture) || !Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone) || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), out var slot))
            throw new InvalidOperationException("Liejie source slot lost its actual HE owner.");
        var cards = zone == CardZoneKind.Hand ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[source]) : _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[source]);
        if (slot < 0 || slot >= cards.Count)
            throw new InvalidOperationException("Liejie source slot is no longer available.");
        return cards[slot];
    }

    internal IReadOnlyList<PromptChoice> LiejieDiscardChoices(ProgramSkillFrame frame)
    {
        var draft = frame.LiejieDiscardPending!;
        var choices = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, draft.SourceSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c =>
        {
            var id = ResolveLiejieSlot(draft.SourceSeat, c).Id;
            return draft.CandidateIds.Contains(id) && !draft.SelectedIds.Contains(id);
        }).Select(c => c with { Id = new($"liejie-pick.{(frame.Id)}.{(draft.SelectedIds.Count)}.{(c.Id.Value)}"), Parameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(c.Parameters.ToDictionary(k => k.Key, v => v.Key == "program-action" ? "liejie-pick" : v.Value)) }).ToList();
        choices.Add(new(new($"liejie-finish.{(frame.Id)}.{(draft.SelectedIds.Count)}"), $"完成选择（弃置{(draft.SelectedIds.Count)}张）。", [], [], new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string> { ["program-action"] = draft.SelectedIds.Count == 0 ? "liejie-decline" : "liejie-finish", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishLiejieDiscard(ProgramSkillFrame frame)
    {
        var choices = LiejieDiscardChoices(frame);
        var draft = frame.LiejieDiscardPending!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"【{(skill.Name)}】弃置伤害来源至多{(draft.MaximumCount)}张牌（已选{(draft.SelectedIds.Count)}张）。", choices.SelectMany(c => c.Cards).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.SourceSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 弃置伤害来源", skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveLiejieDiscardChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Liejie lost its program frame.");
        AssertLiejieSourceDiscard(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect);
        var draft = frame.LiejieDiscardPending!;
        if (draft.Paid || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { PlayerSeat: var chooser, IsPrivate: true } || chooser != frame.OwnerSeat || !LiejieDiscardChoices(frame).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(selected.Parameters.OrderBy(p => p.Key))))
            throw new InvalidOperationException("Liejie requires its exact published private HE choice.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.SourceSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            throw new InvalidOperationException("The unpaid Liejie participants or source skill changed.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters["program-action"] == "liejie-pick")
        {
            var card = ResolveLiejieSlot(draft.SourceSeat, selected);
            draft = draft with
            {
                SelectedIds = draft.SelectedIds.Append(card.Id).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LiejieDiscardPending = draft });
            if (draft.SelectedIds.Count < draft.MaximumCount)
            {
                PublishLiejieDiscard(frame);
                return;
            }
        }

        if (draft.SelectedIds.Count == 0)
        {
            CompleteLiejieSourceDiscard(frame, 0);
            return;
        }

        var ids = draft.SelectedIds.ToArray();
        if (ids.Any(id => !draft.CandidateIds.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != draft.CandidateLocations[draft.CandidateIds.ToList().IndexOf(id)] || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id), _cardZonesOperations._cardZones.GetLocation(id), OwnedCardMoveIntent.Discard)))
            throw new InvalidOperationException("Liejie selected HE payment is no longer legal.");
        var before = LiejieMoveSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { LiejieDiscardPending = draft with { Paid = true, Before = before, After = before }, PendingMovementContinuation = new(draft.SourceSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, ids, new(LiejieDiscardReason(frame)), (batch, records) =>
        {
            foreach (var movement in records)
                _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, frame.OwnerSeat, frame.Id);
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { LiejieDiscardPending = active.LiejieDiscardPending!with { BatchId = batch, After = LiejieMoveSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramLiejieSourceDiscardEvent(frame.Id, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, draft.SourceSeat, records.Count, Array.AsReadOnly(ids)));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void CompleteLiejieSourceDiscard(ProgramSkillFrame frame, int count)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramLiejieSourceDiscardResolvedEvent(frame.Id, count));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LiejieDiscardPending = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 690)]
    internal bool ResumeLiejieSourceDiscard(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != id || frame.LiejieDiscardPending is not { Paid: true })
            return false;
        AssertLiejieSourceDiscard(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        CompleteLiejieSourceDiscard(frame, frame.LiejieDiscardPending.SelectedIds.Count);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 170)]
    internal bool ReturnLiejieSourceDiscardMovement(ProgramSkillFrame frame)
    {
        if (frame.LiejieDiscardPending is not { Paid: true })
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    internal void AssertLiejieSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var starts = history.OfType<ProgramLiejieSelectionStartedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (frame.LiejieDiscardPending is not { } draft)
        {
            if (starts.Length > 0 && !history.OfType<ProgramLiejieSourceDiscardResolvedEvent>().Any(e => e.FrameId == frame.Id))
                throw new InvalidOperationException("An unfinished Liejie source discard lost its owning receipt.");
            return;
        }

        var colors = ExactLiejiePaidColors(frame, paused.SourceBind!);
        if (paused.Op != SkillProgramEffectOp.LiejieSourceDiscard || frame.InstructionIndex != 4 || !ExactLiejieParent(frame) || draft.InstructionIndex != frame.InstructionIndex || draft.ParentFrameId != frame.WindowContext!.ParentFrameId || draft.SourceSeat != frame.WindowContext.SourceSeat || draft.SourceSeat == frame.OwnerSeat || draft.FrozenRedCount != colors.RedCount || draft.MaximumCount is < 1 or > 3 || draft.MaximumCount != Math.Min(draft.FrozenRedCount, draft.CandidateIds.Count) || draft.CandidateIds.Count != draft.CandidateLocations.Count || draft.CandidateIds.Distinct().Count() != draft.CandidateIds.Count || draft.CandidateLocations.Any(l => l.OwnerSeat != draft.SourceSeat || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || draft.SelectedIds.Count > draft.MaximumCount || draft.SelectedIds.Distinct().Count() != draft.SelectedIds.Count || draft.SelectedIds.Any(id => !draft.CandidateIds.Contains(id)) || starts is not [var started] || started != new ProgramLiejieSelectionStartedEvent(frame.Id, draft.InstructionIndex, draft.ParentFrameId, draft.SourceSeat, draft.FrozenRedCount, draft.MaximumCount) || history.OfType<ProgramLiejieSourceDiscardResolvedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Liejie source discard lost its exact owning instruction or frozen quota.");
        if (!draft.Paid)
        {
            if (frame.PendingMovementContinuation is not null || draft.Before != 0 || draft.After != 0 || draft.BatchId is not null || draft.CandidateIds.Where((id, i) => _cardZonesOperations._cardZones.GetLocation(id) != draft.CandidateLocations[i]).Any() || history.OfType<ProgramLiejieSourceDiscardEvent>().Any(e => e.FrameId == frame.Id))
                throw new InvalidOperationException("Unpaid Liejie candidates changed before selection completed.");
            if (ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat || !prompt.Choices.Select(c => c.Id).SequenceEqual(LiejieDiscardChoices(frame).Select(c => c.Id))))
                throw new InvalidOperationException("Liejie lost its exact private chooser prompt.");
            return;
        }

        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > draft.Before && m.Sequence <= draft.After && m.Reason.Value == LiejieDiscardReason(frame)).ToArray();
        if (draft.SelectedIds.Count == 0 || draft.BatchId is null || draft.After <= draft.Before || draft.After > LiejieMoveSequence || frame.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != draft.SourceSeat || moves.Length != draft.SelectedIds.Count || !moves.Select(m => m.CardId).Order().SequenceEqual(draft.SelectedIds.Order()) || moves.Any(m => m.From != draft.CandidateLocations[draft.CandidateIds.ToList().IndexOf(m.CardId)] || !IsLiejieDiscardDestination(m)) || history.OfType<ProgramLiejieSourceDiscardEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var paid] || paid.SourceSeat != draft.SourceSeat || paid.OwnerSeat != frame.OwnerSeat || paid.SkillId != frame.SkillId || paid.BindingId != CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame) || paid.DiscardCount != moves.Length || !paid.DiscardedCardIds.SequenceEqual(draft.SelectedIds))
            throw new InvalidOperationException("Paid Liejie continuation lost its original physical HE invoice.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index + 1 < _resolutionStackOperations._resolutionStack.Count && !LiejieSourceDiscardFirstChild(frame, _resolutionStackOperations._resolutionStack[index + 1]))
            throw new InvalidOperationException("Paid Liejie continuation lost its exact native first child.");
    }
    internal bool LiejieSourceDiscardFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (LiejieInitialClauseFirstChild(frame, child))
            return true;
        if (frame.LiejieDiscardPending is not { Paid: true } draft || !ExactLiejieParent(frame))
            return false;
        bool EquipmentPayment(CardKind kind) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > draft.Before && m.Sequence <= draft.After && draft.SelectedIds.Contains(m.CardId) && m.CardKind == kind && m.From == CardLocation.Equipment(draft.SourceSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == LiejieDiscardReason(frame));
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId == frame.Id && moved.Batch.AwaitingProgramFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > draft.Before && m.Sequence <= draft.After && m.Reason.Value == LiejieDiscardReason(frame) && moved.Batch.Id == draft.BatchId && IsLiejieDiscardDestination(m) || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(draft.SourceSeat) && m.To == CardLocation.DiscardPile && EquipmentPayment(CardKind.WoodenOx)));
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (!EquipmentPayment(CardKind.SilverLion))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == frame.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == draft.SourceSeat && hp.Change.TargetSeat == draft.SourceSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == draft.SourceSeat && recovery.Attempt.TargetSeat == draft.SourceSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == LiejieDiscardReason(frame);
    }

    internal bool LiejieInitialClauseFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.InstructionIndex is not (2 or 3) || frame.LiejieDiscardPending is not null || frame.PendingMovementContinuation is not null || !ExactLiejieParent(frame))
            return false;
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(frame);
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.LiejieSourceDiscard))
            return false;
        LiejieSourceDiscardContract.ValidateTrigger(frame.SkillId, trigger);
        var colors = ExactLiejiePaidColors(frame, trigger.Effects[^1].SourceBind!);
        var costReason = $"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.MoveBoundCards)}";
        var reason = frame.InstructionIndex == 2 ? costReason : $"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.Draw)}";
        var bound = frame.CardSetBindings.Single(b => b.Name == colors.SourceBind);
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        bool EquipmentPayment(CardKind kind) => frame.InstructionIndex == 2 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > colors.BeforeMoveSequence && bound.CardIds.Contains(m.CardId) && m.CardKind == kind && m.From == CardLocation.Equipment(frame.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == costReason);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId == frame.Id && moved.Batch.AwaitingProgramFrameId is null && moved.Batch.ParentFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > colors.BeforeMoveSequence && (m.Reason.Value == reason && (frame.InstructionIndex == 2 ? bound.CardIds.Contains(m.CardId) && m.From.OwnerSeat == frame.OwnerSeat && IsLiejieDiscardDestination(m) : m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat)) || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(frame.OwnerSeat) && m.To == CardLocation.DiscardPile && EquipmentPayment(CardKind.WoodenOx)));
        if (!EquipmentPayment(CardKind.SilverLion))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == frame.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == frame.OwnerSeat && hp.Change.TargetSeat == frame.OwnerSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.Program && recovery.Attempt.SourceSeat == frame.OwnerSeat && recovery.Attempt.TargetSeat == frame.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == costReason;
    }

    internal ProgramSkillFrame? LiejieSourceDiscardObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.LiejieSourceDiscardFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasPaidDamageObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 290)]
    internal bool IsLiejieSourceDiscardProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && LiejieSourceDiscardObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    internal PromptChoice SelectAiLiejieDiscardChoice(PendingDecision decision) => decision.Choices.OrderBy(c => c.Parameters.GetValueOrDefault("program-action") == "liejie-pick" ? 0 : 1).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
