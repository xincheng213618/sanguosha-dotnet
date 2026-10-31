#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYingOlRuntime
{
    internal CardConversionSource CaoYingSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    internal bool IsMobileCaoYing(ProgramSkillFrame frame) => _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.CaoYingPolicy == CaoYingRulePolicy.MobileOrdinary;
    internal bool CaoYingSourceCurrent(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 330)]
    internal bool CanOfferCaoYing(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c)
    {
        if (trigger.Effects is not [{ Op: SkillProgramEffectOp.GuessTargetCategoriesAndArmTurnDamage }])
            return true;
        var use = c.CardUse is { } action ? _coreCardUseLifecycleOperations.LifecycleCardUse(action.ParentCardUseFrameId) : null;
        if (trigger.Effects[0].CaoYingPolicy == CaoYingRulePolicy.MobileOrdinary && (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != candidate.OwnerSeat))
            return false;
        return use is { Action: { Type: CardActionType.Use } a } && a.ActorSeat == candidate.OwnerSeat && a.ActionId == c.CardUse!.CardActionId && CaoYingDamageKind(a.EffectiveKind) && use.TargetSeats.Any(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive);
    }

    internal SkillProgramStepOutcome BeginCaoYing(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.CaoYing is not null || f.WindowContext is not { } context || !CaoYingSourceCurrent(f))
            throw new InvalidOperationException("Cao Ying requires an unpaid current source.");
        if (effect.Op == SkillProgramEffectOp.GuessTargetCategoriesAndArmTurnDamage)
        {
            if (!CanOfferCaoYing(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
                throw new InvalidOperationException("Lingren lost its real finalized actor use.");
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(context.CardUse!.ParentCardUseFrameId)!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYing = new(f.InstructionIndex, context.ParentFrameId, use.Id, use.Action!.ActionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, effect.CaoYingPolicy == CaoYingRulePolicy.MobileOrdinary ? _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat : _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, CaoYingStage.Target) { Source = CaoYingSource(f), GameplayHash = f.GameplayHash, Policy = effect.CaoYingPolicy, PhaseInstanceId = effect.CaoYingPolicy is null ? null : _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId } });
        }
        else
        {
            if (context.Window is not (SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.TurnEnding) || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
                throw new InvalidOperationException("Fujian requires its own preparation or ending.");
            var living = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToArray();
            var mobile = effect.CaoYingPolicy == CaoYingRulePolicy.MobileOrdinary;
            var minimum = living.Min(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count);
            if (mobile && minimum == 0)
                return SkillProgramStepOutcome.Continue;
            var maximum = living.Max(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count);
            var uniqueMaximum = living.Count(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count == maximum) == 1;
            var candidates = living.Where(p => p.Seat != f.OwnerSeat && (mobile || !uniqueMaximum || _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count != maximum)).ToArray();
            if (candidates.Length == 0)
                return SkillProgramStepOutcome.Continue;
            var target = candidates[_randomOperations._random.Next(candidates.Length)];
            var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(target).ToArray();
            if (mobile)
                for (var index = 0; index < minimum; index++)
                {
                    var picked = index + _randomOperations._random.Next(cards.Length - index);
                    (cards[index], cards[picked]) = (cards[picked], cards[index]);
                }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYing = new(f.InstructionIndex, context.ParentFrameId, null, null, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, CaoYingStage.Inspection, target.Seat) { Cards = (mobile ? cards.Take(minimum) : cards).Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray(), Source = CaoYingSource(f), GameplayHash = f.GameplayHash, Policy = effect.CaoYingPolicy, InspectionMinimum = mobile ? minimum : null } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYingInspectionIssuedEvent(f.Id, f.OwnerSeat, target.Seat, f.CaoYing.Cards.Count));
        }

        PublishCaoYingChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice CaoYingChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? targets = null) => new(new($"cao-ying.{(f.Id)}.{(f.CaoYing!.Stage)}.{(branch)}"), label, [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "cao-ying", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> CaoYingChoices(ProgramSkillFrame f)
    {
        var r = f.CaoYing!;
        if (r.Stage == CaoYingStage.Target)
            return Array.AsReadOnly(_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId!.Value)!.TargetSeats.Where(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive).Distinct().Select(t => CaoYingChoice(f, $"target-{(t)}", $"猜测{(_coreMatchParticipantsPlayersAccess._players[t].Name)}的手牌类型", [t])).ToArray());
        if (r.Stage == CaoYingStage.Inspection)
            return[CaoYingChoice(f, "done", "观看完毕")];
        if (r.Stage != CaoYingStage.Guess)
            return[];
        return Array.AsReadOnly(Enumerable.Range(0, 8).Select(mask => CaoYingChoice(f, $"guess-{(mask)}", $"基本牌：{(((mask & 1) != 0 ? "有" : "无"))}；锦囊牌：{(((mask & 2) != 0 ? "有" : "无"))}；装备牌：{(((mask & 4) != 0 ? "有" : "无"))}")).ToArray());
    }

    internal void PublishCaoYingChoice(ProgramSkillFrame f)
    {
        var choices = CaoYingChoices(f);
        if (choices.Count == 0)
        {
            FinishCaoYing(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.CaoYing!.Stage == CaoYingStage.Inspection ? (IsMobileCaoYing(f) ? $"观看随机角色的{(f.CaoYing.Cards.Count)}张手牌。" : "观看随机角色的全部手牌。") : "请选择目标及三类手牌的有无。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.CaoYing.Target,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("cao-ying")]
    internal void ResolveCaoYingChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Cao Ying lost its owning frame.");
        RequireCaoYing(f);
        var r = f.CaoYing!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], CaoYingChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Cao Ying selection changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!CaoYingSourceCurrent(f))
        {
            FinishCaoYing(f);
            return;
        }

        if (r.Stage == CaoYingStage.Inspection)
        {
            FinishCaoYing(f);
            return;
        }

        if (r.Stage == CaoYingStage.Target)
        {
            var target = choice.Targets.Single();
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]);
            var mask = (hand.Any(c => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(c.Kind, SkillProgramCardCategory.Basic)) ? 1 : 0) | (hand.Any(c => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(c.Kind, SkillProgramCardCategory.Trick)) ? 2 : 0) | (hand.Any(c => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(c.Kind, SkillProgramCardCategory.Equipment)) ? 4 : 0);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYing = r with { Stage = CaoYingStage.Guess, Target = target, FrozenCategories = mask } });
            PublishCaoYingChoice(f);
            return;
        }

        var guess = int.Parse(choice.Parameters["branch"][6..], CultureInfo.InvariantCulture);
        var correct = 3 - System.Numerics.BitOperations.PopCount((uint)(guess ^ r.FrozenCategories));
        _coreProgramLifecycleOperations.ConsumeProgramTriggerUsage(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority));
        var mobileRules = IsMobileCaoYing(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYing = r with { Guess = guess, Correct = correct, Stage = CaoYingStage.DrawChildren, MovementBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYingGuessResolvedEvent(f.Id, CaoYingSource(f), r.TurnNumber, r.TurnOwner, r.UseId!.Value, r.ActionId, r.Target!.Value, correct) { Policy = r.Policy, PhaseInstanceId = r.PhaseInstanceId });
        if (correct >= 2 && !mobileRules)
            _coreCardUseDebitsOperations.RefundProgramCardUseDebit(f);
        // The original OL policy grants before drawing. Mobile grants only after its draw children return.
        if (correct >= 3 && !mobileRules)
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], $"cao-ying:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(r.TurnNumber)}:{(f.Id)}", _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single().SkillIds);
        var drawn = correct >= 2 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 2, true, new("skill-program.cao-ying.draw")).Count : 0;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYing = f.CaoYing!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawCount = drawn } });
        if (!DrainCaoYingChildren(f))
            FinishCaoYing(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal bool DrainCaoYingChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void FinishCaoYing(ProgramSkillFrame f)
    {
        if (f.CaoYing is { Stage: CaoYingStage.DrawChildren, Policy: CaoYingRulePolicy.MobileOrdinary, Correct: 3 } r && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            RequireCaoYing(f);
            // All original draw, HP, movement and skill-change children have drained before this tail.
            // Advancing the owning stage before mutation prevents a resumed grant child from granting twice.
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYing = r with { Stage = CaoYingStage.GrantSkills }, PendingMovementContinuation = null });
            _coreProgramSkillChangesOperations.CommitProgramParticipantSkillGrants(f, f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single().SkillIds, $"turn:{(r.TurnNumber)}:{(r.TurnOwner)}:mobile-cao-ying:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:frame:{(f.Id)}", null, new SkillGrantNextActualTurnStartExpiry(r.TurnNumber, r.TurnOwner, f.OwnerSeat));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (DrainCaoYingChildren(f))
                return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYing = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 21800)]
    internal bool ResumeCaoYing(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoYing: { Stage: CaoYingStage.DrawChildren or CaoYingStage.GrantSkills } } f || f.Id != id)
            return false;
        RequireCaoYing(f);
        if (!DrainCaoYingChildren(f))
            FinishCaoYing(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 21800)]
    internal bool ReturnCaoYingMovement(ProgramSkillFrame f)
    {
        if (f.CaoYing?.Stage is not (CaoYingStage.DrawChildren or CaoYingStage.GrantSkills))
            return false;
        RequireCaoYing(f);
        if (!DrainCaoYingChildren(f))
            FinishCaoYing(f);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 50)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 50)]
    internal bool CanContinueCaoYing(ProgramSkillFrame f) => f.CaoYing is { Stage: CaoYingStage.DrawChildren or CaoYingStage.GrantSkills, Correct: >= 0 } && ValidCaoYing(f);
    internal bool ValidCaoYing(ProgramSkillFrame f)
    {
        var r = f.CaoYing!;
        return r.Source == CaoYingSource(f) && r.GameplayHash == f.GameplayHash && r.InstructionIndex == f.InstructionIndex && r.WindowId == f.WindowContext?.ParentFrameId && r.UseId == f.WindowContext?.CardUse?.ParentCardUseFrameId && r.ActionId == f.WindowContext?.CardUse?.CardActionId && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.GuessTargetCategoriesAndArmTurnDamage && r.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && r.TurnOwner == (r.Policy == CaoYingRulePolicy.MobileOrdinary ? _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat : _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat) && (r.Stage == CaoYingStage.GrantSkills ? r.Policy == CaoYingRulePolicy.MobileOrdinary && r.Correct == 3 && (f.PendingMovementContinuation is null || f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } granted && granted.SubjectSeat == f.OwnerSeat) && ValidMobileCaoYingGrantTail(f) : f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == f.OwnerSeat) && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId!.Value)is { Action: { } action } use && action.ActionId == r.ActionId && action.ActorSeat == f.OwnerSeat && use.TargetSeats.Contains(r.Target!.Value) && r.MovementBefore <= r.MovementAfter && r.MovementAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && r.DrawCount is >= 0 and <= 2 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoYingGuessResolvedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.TurnNumber == r.TurnNumber && e.TurnOwner == r.TurnOwner && e.UseId == r.UseId && e.ActionId == r.ActionId && e.TargetSeat == r.Target && e.CorrectCount == r.Correct && e.Policy == r.Policy && e.PhaseInstanceId == r.PhaseInstanceId) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.Reason.Value == "skill-program.cao-ying.draw" && m.To == CardLocation.Hand(f.OwnerSeat)) == r.DrawCount;
    }

    internal bool ValidMobileCaoYingGrantTail(ProgramSkillFrame f)
    {
        var r = f.CaoYing!;
        var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramTurnSkillsGrantedEvent>().Where(e => e.FrameId == f.Id && e.SkillId == f.SkillId).ToArray();
        return (f.PendingMovementContinuation is null ? grants.Length <= 1 : grants.Length == 1) && grants.All(grant => grant.OwnerSeat == f.OwnerSeat && grant.RecipientSeat == f.OwnerSeat && grant.BindingId == f.TriggerId && grant.GrantSourceId == $"turn:{(r.TurnNumber)}:{(r.TurnOwner)}:mobile-cao-ying:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:frame:{(f.Id)}" && grant.TurnExpiry is null && grant.NextActualTurnStartExpiry == new SkillGrantNextActualTurnStartExpiry(r.TurnNumber, r.TurnOwner, f.OwnerSeat) && grant.GrantedSkillIds.SequenceEqual(_coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single().SkillIds));
    }

    internal void RequireCaoYing(ProgramSkillFrame f)
    {
        if (f.CaoYing is not { } r || r.Source != CaoYingSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.TriggerId is null || r.WindowId != f.WindowContext?.ParentFrameId || r.FrozenCategories is < 0 or > 7 || r.Policy != _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.CaoYingPolicy || r.Policy == CaoYingRulePolicy.MobileOrdinary && r.Stage != CaoYingStage.Inspection && r.PhaseInstanceId is not > 0 || r.Stage is (CaoYingStage.DrawChildren or CaoYingStage.GrantSkills) && !ValidCaoYing(f) || (r.Stage is CaoYingStage.DrawChildren or CaoYingStage.GrantSkills ? r.Guess is < 0 or > 7 || r.Correct != 3 - System.Numerics.BitOperations.PopCount((uint)(r.Guess ^ r.FrozenCategories)) : r.Guess != -1 || r.Correct != -1) || (r.Stage == CaoYingStage.Inspection ? r.Target is null || r.Target == f.OwnerSeat || r.UseId is not null || r.Policy == CaoYingRulePolicy.MobileOrdinary && (r.InspectionMinimum is not > 0 || r.Cards.Count != r.InspectionMinimum || r.Cards.Select(c => c.Id).Distinct().Count() != r.Cards.Count) : r.UseId != f.WindowContext?.CardUse?.ParentCardUseFrameId || r.ActionId != f.WindowContext?.CardUse?.CardActionId))
            throw new InvalidOperationException("Cao Ying receipt lost its frozen instruction, action or private category decision.");
    }

    internal bool CaoYingFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueCaoYing(root) || root.CaoYing is not { } r)
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return r.Correct == 3 && skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == "skill-program.cao-ying.draw");
    }

    internal ProgramSkillFrame? CaoYingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.CaoYingFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 110)]
    internal bool TryAdvanceCaoYingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaoYingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    SkillProgramStepOutcome ICaoYingProgramHost.BeginCaoYing(ProgramSkillFrame f, SkillProgramEffect e) => BeginCaoYing(f, e);
}
