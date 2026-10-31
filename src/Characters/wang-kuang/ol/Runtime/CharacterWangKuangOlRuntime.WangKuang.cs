#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangKuangOlRuntime
{
    internal const string RenxiaDrawReason = "skill-program.wang-kuang.renxia.draw", RenxiaDiscardReason = "skill-program.wang-kuang.renxia.discard";
    internal bool HasWangKuangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveRenxiaRepeatedBranch);

    internal static CardConversionSource WangKuangSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_wang_kuang_ol_WangKuang.WangKuangSource(f);
    internal WangKuangRenxiaPhaseKey WangKuangCurrentPhase(int owner) => new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, owner, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    internal static bool IsWangKuangDamageCardKind(CardKind kind) => RuntimeSkills.RuntimeRules_wang_kuang_ol_WangKuang.IsWangKuangDamageCardKind(kind);
    internal bool WangKuangHandHasDamage(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Any(c => IsWangKuangDamageCardKind(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], c)));
    internal IReadOnlyList<WangKuangRenxiaCard> WangKuangDiscardable(int owner, string instance) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(owner)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, "ol:renxia", instance, c)).Select(c => new WangKuangRenxiaCard(c.Id, _cardZonesOperations._cardZones.GetLocation(c.Id))).ToArray());
    SkillProgramStepOutcome IWangKuangProgramHost.Resolve(ProgramSkillFrame f, bool ending) => BeginWangKuang(f, ending);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 26910)]
    internal bool CollectWangKuang(SkillProgramTrigger t) => t.Effects is not [{ Op: SkillProgramEffectOp.ResolveRenxiaOppositeAtPlayEnding }];
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26910)]
    internal bool QualifyWangKuang(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is not [{ Op: SkillProgramEffectOp.ResolveRenxiaOppositeAtPlayEnding }] || IsIssuedWangKuangRenxiaCandidate(c, context);
    internal bool WangKuangObligationConsumed(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<WangKuangRenxiaEndingConsumedEvent>().Any(e => e.ActiveFrameId == id);
    internal IEnumerable<WangKuangRenxiaObligationIssuedEvent> WangKuangOutstanding(WangKuangRenxiaPhaseKey phase) => !HasWangKuangCapability ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<WangKuangRenxiaObligationIssuedEvent>().Where(e => e.Phase == phase && !WangKuangObligationConsumed(e.ActiveFrameId));
    internal WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(CharacterState owner) => owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && owner.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && WangKuangOutstanding(WangKuangCurrentPhase(owner.Seat)).Any() ? WangKuangCurrentPhase(owner.Seat) : null;
    internal IEnumerable<ProgramTriggerCandidate> IssuedWangKuangRenxiaEndingCandidates(CharacterState owner) => CaptureWangKuangRenxiaEndingPhase(owner)is { } phase ? WangKuangOutstanding(phase).Select(e => new ProgramTriggerCandidate(owner.Seat, e.Source.SkillId, "issued-opposite-at-play-end", e.Source.SkillInstanceId, e.GameplayHash, 0)) : [];
    internal bool IsIssuedWangKuangRenxiaCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!HasWangKuangCapability || c.SkillId != "ol:renxia" || c.BindingId != "issued-opposite-at-play-end" || context.Window != SkillProgramTriggerWindow.PlayEnding || context.OwnerSeat != c.OwnerSeat || c.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { WangKuangRenxiaEndingPhase: { } phase } parent || phase.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || phase.OwnerSeat != c.OwnerSeat || parent.OwnerSeat != c.OwnerSeat || parent.Window != context.Window || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WangKuangRenxiaObligationIssuedEvent>().Any(e => e.Phase == phase && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.Source.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash && ValidWangKuangObligation(e) && (!WangKuangObligationConsumed(e.ActiveFrameId) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.WangKuangRenxia is { Ending: true } r && r.ObligationId == e.ActiveFrameId && r.Phase == phase && f.WindowContext == context && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c))));
    }

    internal SkillProgramStepOutcome BeginWangKuang(ProgramSkillFrame input, bool ending)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.WangKuangRenxia is not null || f.SkillId != "ol:renxia" || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Renxia requires its exact zero-input original Play activation or issued same-phase ending.");
        WangKuangRenxiaObligationIssuedEvent? promise = null;
        if (ending)
        {
            var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, f.WindowContext!.OccurrenceIndex);
            if (!IsIssuedWangKuangRenxiaCandidate(c, f.WindowContext))
                throw new InvalidOperationException("Renxia lost the exact issued PlayEnding candidate.");
            var phase = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(w => w.Id == f.WindowContext.ParentFrameId).WangKuangRenxiaEndingPhase!;
            promise = WangKuangOutstanding(phase).Single(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash);
        }
        else if (f.WindowContext is not null || f.TriggerId is not null || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("An unpaid Renxia activation cannot borrow an ending entitlement or lost source.");
        var receipt = new WangKuangRenxiaReceipt(WangKuangSource(f), f.GameplayHash, f.InstructionIndex, promise?.Phase ?? WangKuangCurrentPhase(f.OwnerSeat), ending, promise?.ActiveFrameId, WangKuangRenxiaStage.BranchChoice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaStartedEvent(f.Id, receipt.Source, receipt.GameplayHash, receipt.Phase, ending, receipt.ObligationId));
        if (promise is not null)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaEndingConsumedEvent(promise.ActiveFrameId, f.Id, f.WindowContext!.ParentFrameId, promise.Phase));
            ChooseWangKuangBranch(f, promise.FirstBranch == WangKuangRenxiaBranch.Draw ? WangKuangRenxiaBranch.Discard : WangKuangRenxiaBranch.Draw);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishWangKuang(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> WangKuangChoices(ProgramSkillFrame f)
    {
        var r = f.WangKuangRenxia!;
        PromptChoice Choice(string branch, string label, int[]? cards = null) => new(new($"renxia.{(f.Id)}.{(r.Cursor)}.{(branch)}.{(string.Join('.', cards ?? []))}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "wang-kuang-renxia", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == WangKuangRenxiaStage.BranchChoice)
        {
            var choices = new List<PromptChoice>
            {
                Choice("draw", "摸两张牌，重复至手牌中有杀或伤害锦囊；本阶段结束时执行弃牌项")
            };
            if (WangKuangDiscardable(f.OwnerSeat, f.SkillInstanceId).Count >= 2)
                choices.Add(Choice("discard", "弃置两张牌，重复至手牌中没有杀或伤害锦囊；本阶段结束时执行摸牌项"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage != WangKuangRenxiaStage.DiscardChoice)
            return[];
        var legal = WangKuangDiscardable(f.OwnerSeat, f.SkillInstanceId);
        return Array.AsReadOnly(r.Candidates.Where(c => legal.Contains(c) && !r.Selected.Contains(c)).Select(c => Choice("card", $"弃置【{(_coreCardAttackStateOperations.GetAttackCard(c.CardId).DisplayName)}】（{(r.Selected.Count + 1)}/{(r.RequiredCount)}）", [c.CardId])).ToArray());
    }

    internal void PublishWangKuang(ProgramSkillFrame f)
    {
        var choices = WangKuangChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("Renxia lost its frozen legal private choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.WangKuangRenxia!.Stage == WangKuangRenxiaStage.BranchChoice ? "任侠：选择先执行的一项。" : "任侠：选择本次一起弃置的牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("wang-kuang-renxia")]
    internal void AnswerWangKuang(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WangKuangRenxia: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], WangKuangChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Renxia lost its original private chooser.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == WangKuangRenxiaStage.BranchChoice)
        {
            ChooseWangKuangBranch(f, choice.Parameters["branch"] == "draw" ? WangKuangRenxiaBranch.Draw : WangKuangRenxiaBranch.Discard);
            return;
        }

        var card = r.Candidates.Single(c => c.CardId == choice.Cards.Single());
        var selected = r.Selected.Append(card).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = r with { Selected = selected } });
        if (selected.Length < r.RequiredCount)
        {
            PublishWangKuang(f);
            return;
        }

        PayWangKuangDiscard(f);
        ContinueWangKuang(f.Id);
    }

    internal void ChooseWangKuangBranch(ProgramSkillFrame f, WangKuangRenxiaBranch branch)
    {
        var r = f.WangKuangRenxia!;
        if (r.Branch is not null || !r.Ending && branch == WangKuangRenxiaBranch.Discard && WangKuangDiscardable(f.OwnerSeat, f.SkillInstanceId).Count < 2)
            throw new InvalidOperationException("The initial voluntary discard requires two genuinely discardable owned cards.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = r with { Branch = branch } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaBranchChosenEvent(f.Id, branch));
        if (branch == WangKuangRenxiaBranch.Draw)
        {
            PayWangKuangDraw(f);
            ContinueWangKuang(f.Id);
        }
        else
            PrepareWangKuangDiscard(f);
    }

    internal void PrepareWangKuangDiscard(ProgramSkillFrame f)
    {
        var r = f.WangKuangRenxia!;
        var cards = WangKuangDiscardable(f.OwnerSeat, f.SkillInstanceId);
        var count = Math.Min(2, cards.Count);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = r with { Stage = WangKuangRenxiaStage.DiscardChoice, Candidates = cards, Selected = [], RequiredCount = count, WoodenOxGrainIds = [] } });
        if (count == 0)
        {
            PayWangKuangDiscard(f);
            ContinueWangKuang(f.Id);
        }
        else
            PublishWangKuang(f);
    }

    internal void RecordWangKuangPayment(ProgramSkillFrame f)
    {
        var r = f.WangKuangRenxia!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaIterationPaidEvent(f.Id, r.Branch!.Value, r.Cursor, r.Before, r.After, r.ActualCount));
        if (!r.Ending && r.Cursor == 1)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaObligationIssuedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.Branch.Value));
    }

    internal void PayWangKuangDraw(ProgramSkillFrame f)
    {
        var r = f.WangKuangRenxia!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = r with { Stage = WangKuangRenxiaStage.MovementChildren, Cursor = r.Cursor + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = 0, Candidates = [], Selected = [], WoodenOxGrainIds = [], RequiredCount = 0 } });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 2, true, new(RenxiaDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = f.WangKuangRenxia!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = drawn.Count } });
        RecordWangKuangPayment(f);
    }

    internal void PayWangKuangDiscard(ProgramSkillFrame f)
    {
        var r = f.WangKuangRenxia!;
        var legal = WangKuangDiscardable(f.OwnerSeat, f.SkillInstanceId);
        if (r.Selected.Count != r.RequiredCount || r.Selected.Any(c => !legal.Contains(c)))
            throw new InvalidOperationException("Renxia changed a frozen unpaid physical discard set.");
        var grain = r.Selected.Any(c => c.From == CardLocation.Equipment(f.OwnerSeat) && _coreCardAttackStateOperations.GetAttackCard(c.CardId).Kind == CardKind.WoodenOx) ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => c.Id).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = r with { Stage = WangKuangRenxiaStage.MovementChildren, Cursor = r.Cursor + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = r.Selected.Count, WoodenOxGrainIds = grain } });
        if (r.Selected.Count == 0)
        {
            RecordWangKuangPayment(f);
            return;
        }

        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected.Select(c => c.CardId).ToArray(), new(RenxiaDiscardReason), (_, movements) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { WangKuangRenxia = active.WangKuangRenxia!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            foreach (var m in movements)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaDiscardEntityEvent(active.Id, active.WangKuangRenxia!.Cursor, m.CardId, m.From, m.Sequence));
            RecordWangKuangPayment(active);
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = f.WangKuangRenxia!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaTailClosedEvent(f.Id, f.WangKuangRenxia!.Cursor, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal bool DrainWangKuangChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueWangKuang(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { WangKuangRenxia: { } r } f && f.Id == id)
        {
            if (r.Stage is WangKuangRenxiaStage.BranchChoice or WangKuangRenxiaStage.DiscardChoice)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishWangKuang(f);
                return;
            }

            if (DrainWangKuangChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.WangKuangRenxia!;
            var has = WangKuangHandHasDamage(f.OwnerSeat);
            var alive = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
            var repeat = alive && r.ActualCount > 0 && (r.Branch == WangKuangRenxiaBranch.Draw ? !has : has && WangKuangDiscardable(f.OwnerSeat, f.SkillInstanceId).Count > 0);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaComparedEvent(f.Id, r.Cursor, has, repeat));
            if (!repeat)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangKuangRenxiaCompletedEvent(f.Id, r.Ending, r.Branch, r.Cursor, !alive ? "owner-dead-or-game-ended" : r.ActualCount == 0 ? "no-payable-card" : "condition-satisfied-or-no-legal-payment"));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangKuangRenxia = null });
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return;
            }

            if (r.Branch == WangKuangRenxiaBranch.Draw)
                PayWangKuangDraw(f);
            else
            {
                PrepareWangKuangDiscard(f);
                return;
            }
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26910)]
    internal bool ResumeWangKuang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WangKuangRenxia: not null } f || f.Id != id)
            return false;
        ContinueWangKuang(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26910)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26910)]
    internal bool ContinuePaidWangKuang(ProgramSkillFrame f) => f.WangKuangRenxia is { } r ? (r.Ending || r.Cursor > 0) && ValidWangKuang(f) : f.InstructionIndex is 0 or 1 && f.TriggerId == "issued-opposite-at-play-end" && f.WindowContext is { } context && IsIssuedWangKuangRenxiaCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveRenxiaRepeatedBranch, SkillProgramEffectOp.ResolveRenxiaOppositeAtPlayEnding)]
    internal PromptChoice ChooseWangKuangAi(PendingDecision p, ProgramSkillFrame f) => f.WangKuangRenxia?.Stage == WangKuangRenxiaStage.BranchChoice ? p.Choices.First(c => c.Parameters["branch"] == "draw") : p.Choices.OrderBy(c => IsWangKuangDamageCardKind(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreCardAttackStateOperations.GetAttackCard(c.Cards.Single()))) ? 0 : 1).ThenBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();
    [ProgramIssuedTrigger(660, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger660(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(candidate.SkillId)?.Program is { } renxiaProgram && ProgramInstructionResolver.Default.FindTrigger(renxiaProgram, candidate.BindingId)?.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveRenxiaOppositeAtPlayEnding) == true ? IsIssuedWangKuangRenxiaCandidate(candidate, context) : null;
}
