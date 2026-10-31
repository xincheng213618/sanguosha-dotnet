#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaoSanNiangOlRuntime
{
    internal bool ValidBao2025(ProgramSkillFrame f)
    {
        if (f.Bao2025 is not { } r || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } c || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [var e] || e.Op != r.Operation || !IsBaoOperation(e.Op) || r.Source != BaoSource(f) || r.GameplayHash != f.GameplayHash || r.WindowId != c.ParentFrameId || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !Enum.IsDefined(r.Stage) || r.Cards.Count != r.From.Count || r.Cards.Distinct().Count() != r.Cards.Count || r.SelectedTargets.Distinct().Count() != r.SelectedTargets.Count || r.SelectedTargets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t)) || r.MovementBefore < 0 || r.MovementAfter < r.MovementBefore || r.MovementAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.OutcomeCursor < 0 || r.OutcomeCursor > r.SelectedTargets.Count)
            return false;
        var started = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBao2025StartedEvent>().Where(x => x.FrameId == f.Id).ToArray();
        if (started is not [var start] || start.Receipt.Operation != r.Operation || start.Receipt.Source != r.Source || start.Receipt.GameplayHash != r.GameplayHash || start.Receipt.WindowId != r.WindowId || start.Receipt.OriginalUseId != r.OriginalUseId || start.Receipt.OriginalActionId != r.OriginalActionId || !start.Receipt.Targets.SequenceEqual(r.Targets) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBao2025CompletedEvent>().Any(x => x.FrameId == f.Id))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1)
            return false;
        if (r.Operation == SkillProgramEffectOp.TakeConvertedSlashTargetAndResetLimitedSkill)
        {
            if (_resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame window || window.Id != c.ParentFrameId || c.Window != SkillProgramTriggerWindow.CardUseCompleted || window.ParentFrameId != r.OriginalUseId || window.Action.ActionId != r.OriginalActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.OriginalUseId!.Value)is not { Step: ResolutionFrameStep.Completed } use || !_coreNonBasicSlashProfileOperations.HasIssuedNonBasicSlashDistance(use.Id) || !use.TargetSeats.SequenceEqual(r.Targets))
                return false;
            if (r.GainedCardId is { } gained)
            {
                if (r.GainedFrom is not { } from || from.OwnerSeat != r.Recipient || r.GainedCardKind is null || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.CardId == gained && m.CardKind == r.GainedCardKind && m.From == from && m.Reason == CharacterBaoSanNiangOlDefinitions.BaoGainReason && (m.To == CardLocation.Hand(f.OwnerSeat) || m.To == CardLocation.OutsideGame)) != 1 || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBao2025MovementIssuedEvent>().Any(x => x.FrameId == f.Id && x.Receipt.GainedCardId == gained && x.Receipt.MovementBefore == r.MovementBefore && x.Receipt.MovementAfter == r.MovementAfter))
                    return false;
            }

            return r.Stage is ProgramBao2025Stage.TakeTarget or ProgramBao2025Stage.TakeCard or ProgramBao2025Stage.GainChildren or ProgramBao2025Stage.ResetChoice or ProgramBao2025Stage.ResetHpChildren;
        }

        if (_resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame lifecycle || lifecycle.Id != c.ParentFrameId || lifecycle.Window != c.Window || lifecycle.CandidateIndex < 0 || lifecycle.CandidateIndex >= lifecycle.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, lifecycle.Candidates[lifecycle.CandidateIndex]))
            return false;
        if (r.Operation == SkillProgramEffectOp.PayTwoForChosenAssaultAndApplyTargetOutcomes)
        {
            if (c.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || c.OwnerSeat != f.OwnerSeat || r.Cards.Count > 2)
                return false;
            var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBao2025MovementIssuedEvent>().Where(x => x.FrameId == f.Id).ToArray();
            if (r.Stage is ProgramBao2025Stage.AssaultCost or ProgramBao2025Stage.AssaultTargets)
                return paid.Length == 0;
            if (paid is not [var payment] || payment.Receipt.Cards.Count != 2 || !payment.Receipt.Cards.SequenceEqual(r.Cards) || !payment.Receipt.From.SequenceEqual(r.From) || payment.Receipt.MovementAfter < payment.Receipt.MovementBefore || r.Cards.Select((id, i) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.From == r.From[i] && m.To == CardLocation.DiscardPile && m.Reason == CharacterBaoSanNiangOlDefinitions.BaoCostReason && m.Sequence > payment.Receipt.MovementBefore && m.Sequence <= payment.Receipt.MovementAfter) != 1).Any(v => v))
                return false;
            if (r.AssaultUseId is { } useId)
            {
                if (r.AssaultActionId is not { } action || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChosenAssaultUseIssuedEvent>().Count(x => x.FrameId == f.Id && x.Source == r.Source && x.UseId == useId && x.ActionId == action && x.SelectedTargets.SequenceEqual(r.SelectedTargets)) != 1)
                    return false;
                var damage = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChosenAssaultActualDamageObservedEvent>().Where(x => x.ProgramFrameId == f.Id).ToArray();
                if (damage.Any(x => x.UseId != useId || x.SourceSeat != f.OwnerSeat || x.Amount <= 0) || !damage.Select(x => x.TargetSeat).Distinct().Order().SequenceEqual(r.DamagedTargets.Order()))
                    return false;
            }

            return r.Stage is ProgramBao2025Stage.AssaultCostChildren or ProgramBao2025Stage.AssaultUse or ProgramBao2025Stage.AssaultOutcomes or ProgramBao2025Stage.AssaultOutcomeChildren;
        }

        if (c.Window != SkillProgramTriggerWindow.DyingEntering || c.TargetSeat != f.OwnerSeat || !_resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any(d => d.VictimSeat == f.OwnerSeat))
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DyingRecipientSkillChoiceEvent>().Where(x => x.FrameId == f.Id).ToArray();
        return r.GrantedSkillId is null ? facts.Length == 0 : facts is [var chosen] && chosen.Source == r.Source && chosen.RecipientSeat == r.Recipient && chosen.Replace == r.ReplacedSkills && chosen.GrantedSkillId == r.GrantedSkillId && chosen.LostGrantIds.SequenceEqual(r.LostGrantIds);
    }

    internal void RequireBao2025(ProgramSkillFrame f)
    {
        if (!ValidBao2025(f))
            throw new InvalidOperationException("The native converted Slash, paid assault or Dying recipient lost its exact owner, candidate, once-paid invoice or typed return.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40)]
    internal bool CanContinueBao2025(ProgramSkillFrame f) => f.Bao2025 is { } r && ValidBao2025(f) && (r.GainedCardId is not null || r.Stage is ProgramBao2025Stage.AssaultCostChildren or ProgramBao2025Stage.AssaultUse or ProgramBao2025Stage.AssaultOutcomes or ProgramBao2025Stage.AssaultOutcomeChildren or ProgramBao2025Stage.DyingRecipientChoice or ProgramBao2025Stage.DyingSkillChildren or ProgramBao2025Stage.DyingRecoveryChildren);
    internal bool IsChosenAssaultUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ChosenAssault: { } ret, CardId: 0, CardKind: CardKind.BarbarianAssault, PhysicalCardIds.Count: 0, Action: { Type: CardActionType.Use, EffectiveKind: CardKind.BarbarianAssault, PhysicalCards.Count: 0, ConversionChain.Count: 0 } action } use && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == ret.ProgramFrameId)is { Bao2025: { Stage: ProgramBao2025Stage.AssaultUse } r } parent && parent.InstructionIndex == ret.InstructionIndex && r.AssaultUseId == id && r.AssaultActionId == action.ActionId && action.ProviderSeat == parent.OwnerSeat && use.SourceSeat == action.ActorSeat && action.TargetSeats.SequenceEqual(use.TargetSeats) && ValidBao2025(parent);
    internal bool MatchesChosenAssaultUseAction(long id, CardActionContext action, int cardId) => cardId == 0 && IsChosenAssaultUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!.ActionId == action.ActionId;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 400)]
    internal void ObserveChosenAssaultDamage(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent { Amount: > 0, SourceLess: false } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ChosenAssault: { } ret } use || !IsChosenAssaultUse(use.Id) || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault(d => d.ParentFrameId == use.Id)is not { } damage || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat)
            return;
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == ret.ProgramFrameId && p.InstructionIndex == ret.InstructionIndex);
        if (applied.SourceSeat != f.OwnerSeat)
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChosenAssaultActualDamageObservedEvent>().Any(x => x.DamageFrameId == damage.Id))
            throw new InvalidOperationException("One native chosen-assault damage cannot be observed twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { Bao2025 = f.Bao2025!with { DamagedTargets = f.Bao2025.DamagedTargets.Append(applied.TargetSeat).Distinct().ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChosenAssaultActualDamageObservedEvent(f.Id, use.Id, damage.Id, applied.SourceSeat, applied.TargetSeat, applied.Amount));
    }

    internal bool BaoFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.Bao2025 is not { } r || !CanContinueBao2025(root))
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && (m.Reason == CharacterBaoSanNiangOlDefinitions.BaoGainReason || m.Reason == CharacterBaoSanNiangOlDefinitions.BaoCostReason || m.Reason == CharacterBaoSanNiangOlDefinitions.BaoOutcomeReason));
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && (r.Stage == ProgramBao2025Stage.ResetHpChildren && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1 || r.Stage == ProgramBao2025Stage.DyingRecoveryChildren && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == root.OwnerSeat || hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && HasBaoSilverLionPayment(root, hp.Change.TargetSeat));
        if (child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root))
            return recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && (r.Stage == ProgramBao2025Stage.DyingRecoveryChildren && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program || recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && HasBaoSilverLionPayment(root, recovery.Attempt.TargetSeat));
        if (child is DyingFrame dying)
            return r.Stage == ProgramBao2025Stage.ResetHpChildren && dying.VictimSeat == root.OwnerSeat && dying.ParentFrameId == root.Id;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id);
    }

    internal bool HasBaoSilverLionPayment(ProgramSkillFrame f, int seat) => f.Bao2025 is { } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.From == CardLocation.Equipment(seat) && m.CardKind == CardKind.SilverLion && (m.Reason == CharacterBaoSanNiangOlDefinitions.BaoGainReason || m.Reason == CharacterBaoSanNiangOlDefinitions.BaoCostReason || m.Reason == CharacterBaoSanNiangOlDefinitions.BaoOutcomeReason));
    internal ProgramSkillFrame? BaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.BaoFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 450)]
    internal bool TryAdvanceBaoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramAiChoice(SkillProgramEffectOp.TakeConvertedSlashTargetAndResetLimitedSkill, SkillProgramEffectOp.PayTwoForChosenAssaultAndApplyTargetOutcomes, SkillProgramEffectOp.DyingRecipientChooseReplaceOrGrantThenRecover)]
    internal PromptChoice SelectAiBaoChoice(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.Bao2025!;
        if (r.Stage == ProgramBao2025Stage.ResetChoice)
            return decision.Choices.Single(c => c.Parameters["branch"] == (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp > 1 && BaoHasSpentFriendlyResetSkill(f) ? "reset" : "decline"));
        if (r.Stage == ProgramBao2025Stage.DyingRecipientChoice)
            return decision.Choices.Single(c => c.Parameters["branch"] == "grant");
        if (r.Stage == ProgramBao2025Stage.AssaultTargets)
        {
            var enemy = decision.Choices.FirstOrDefault(c => c.Parameters["branch"].StartsWith("add-", StringComparison.Ordinal) && !CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]));
            return enemy ?? decision.Choices.Single(c => c.Parameters["branch"] == "confirm");
        }

        if (r.Stage == ProgramBao2025Stage.AssaultCost)
            return decision.Choices.Where(c => c.Cards.Count == 2).OrderBy(c => c.Cards.Sum(id => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(id), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))).First();
        if (r.Stage is ProgramBao2025Stage.TakeTarget or ProgramBao2025Stage.DyingTarget)
            return decision.Choices.FirstOrDefault(c => c.Targets.Count == 1 && CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]) == (r.Stage == ProgramBao2025Stage.DyingTarget)) ?? decision.Choices.First(c => c.Targets.Count == 1);
        return decision.Choices.First();
    }

    internal bool BaoHasSpentFriendlyResetSkill(ProgramSkillFrame f)
    {
        var resetId = _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single().SourceBind!;
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (p.Seat == f.OwnerSeat || CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], p))).Any(p => _coreNationalSkillsOperations.GetSkillBindingShard(p).ProgramInstances.Where(i => i.SkillId == resetId).Any(i => i.Program.Triggers.Any(t => t.UsageScope == SkillUsageScope.Game && t.UsageLimit == 1 && t.Effects is [{ Op: SkillProgramEffectOp.PayTwoForChosenAssaultAndApplyTargetOutcomes }] && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(p.Seat, resetId, _coreProgramLifecycleOperations.ProgramTriggerUsageId(new(p.Seat, resetId, t.Id, i.SkillInstanceId, i.Program.GameplayHash, t.Priority)), SkillUsageScope.Game) > 0)));
    }
}
