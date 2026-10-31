#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageCardColorHealthRuntime
{
    internal bool HasRecipientDamageCardColorHealth(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).GetInstanceTriggers(SkillProgramTriggerWindow.BeforeDamageApplied).Any(binding => ResolveDamageCardColorHealthDescriptor.IsTrigger(binding.Trigger));
    internal bool TryGetDamageHealthCard(GameEngine.IDamageAttempt attack, out CardKind kind, out Suit suit, out bool red, out CardUseFrame? use)
    {
        use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        var action = use?.Action;
        kind = action?.EffectiveKind ?? attack.EffectiveCardKind ?? attack.Card?.Kind ?? default;
        suit = action?.EffectiveSuit ?? attack.Card?.Suit ?? Suit.None;
        red = action?.EffectiveIsRed ?? (suit is Suit.Heart or Suit.Diamond);
        if (use is { } nativeUse)
            return nativeUse.CardKind == kind && (action is null || action.Type == CardActionType.Use && action.EffectiveKind == kind) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == nativeUse.Id && e.CardId == nativeUse.CardId) == 1;
        if (attack.Card is not null)
            return true;
        return attack is NativeOwners.CardAttackHandle cardAttack && kind == CardKind.Duel && _coreProgramVirtualDuelOperations.IsProgramVirtualDuelAttackConsistent(cardAttack, Array.Empty<Card>());
    }

    internal bool ExactDamageColorHealthParent(ProgramSkillFrame f, out BeforeDamageProgramWindowFrame window, out GameEngine.IDamageAttempt attack)
    {
        window = null !;
        attack = null !;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0 } context || _resolutionStackOperations._resolutionStack[index - 1] is not BeforeDamageProgramWindowFrame before || before.Id != context.ParentFrameId || before.TargetSeat != f.OwnerSeat || context.OwnerSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || before.Amount != context.Amount || before.Prevented || before.RedirectedTargetSeat is not null || before.CandidateIndex < 0 || before.CandidateIndex >= before.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, before.Candidates[before.CandidateIndex].Candidate) || !ResolveDamageCardColorHealthDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)))
            return false;
        var native = _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(before.ContinuationAttackResolutionId ?? before.ParentFrameId);
        if (native is null || native.TargetSeat != before.TargetSeat || native.SourceSeat != before.SourceSeat || context.SourceSeat != (native.IsSourceLess ? null : native.SourceSeat) || _coreNativeCardClassificationsOperations.GetDamageNature(native) != before.Nature)
            return false;
        window = before;
        attack = native;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 2403, SkillProgramEffectOp.ResolveDamageCardColorHealth)]
    internal bool CanOfferDamageCardColorHealth(ProgramTriggerLegalityContext c) => c.WindowContext is { Amount: > 0, TargetSeat: { } target } && target == c.Owner.Seat && c.Owner.HasHpValue && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == c.Context.ParentFrameId)is { } before && _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(before.ContinuationAttackResolutionId ?? before.ParentFrameId)is { } attack && TryGetDamageHealthCard(attack, out _, out _, out _, out _);
    internal SkillProgramStepOutcome ResolveDamageCardColorHealth(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.DamageCardColorHealth is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || !ExactDamageColorHealthParent(f, out var before, out var attack) || !TryGetDamageHealthCard(attack, out var kind, out var suit, out var red, out var use))
            throw new InvalidOperationException("Damage-card color health lost its exact positive native before-damage candidate.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!owner.IsAlive || !owner.HasHpValue || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return SkillProgramStepOutcome.Continue;
        var receipt = new ProgramDamageCardColorHealthReceipt(f.Id, f.InstructionIndex, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, before.Id, attack.ResolutionId, use?.Id, use?.Action?.ActionId, attack.Card?.Id, f.OwnerSeat, attack.IsSourceLess ? null : attack.SourceSeat, before.Amount, kind, suit, red, owner.Hp, owner.MaxHp, red ? checked(owner.MaxHp + before.Amount) : owner.MaxHp);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageCardColorHealth = receipt });
        if (red)
            _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, receipt.Amount);
        else
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, receipt.Amount, null, null);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageCardColorHealthPaidEvent(receipt));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ValidDamageCardColorHealth(ProgramSkillFrame f)
    {
        if (f.DamageCardColorHealth is not { } r || r.ProgramFrameId != f.Id || r.InstructionIndex != f.InstructionIndex || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.PendingMovementContinuation is not null || !ExactDamageColorHealthParent(f, out var before, out var attack) || !TryGetDamageHealthCard(attack, out var kind, out var suit, out var red, out var use))
            return false;
        return r.BeforeDamageWindowId == before.Id && r.AttackResolutionId == attack.ResolutionId && r.CardUseFrameId == use?.Id && r.CardActionId == use?.Action?.ActionId && r.PhysicalCardId == attack.Card?.Id && r.OwnerSeat == f.OwnerSeat && r.DamageSourceSeat == (attack.IsSourceLess ? null : attack.SourceSeat) && r.Amount == before.Amount && r.Amount > 0 && r.EffectiveCardKind == kind && r.EffectiveSuit == suit && r.IsRed == red && r.MaxHpBefore > 0 && r.MaxHpAfter == (red ? checked(r.MaxHpBefore + r.Amount) : r.MaxHpBefore) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramDamageCardColorHealthPaidEvent>().Count(e => e.Receipt == r) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == SkillProgramTriggerWindow.BeforeDamageApplied) == 1;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 2403)]
    internal bool ResumeDamageCardColorHealth(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.DamageCardColorHealth is null)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageCardColorHealthCompletedEvent(id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 2403)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 2403)]
    internal bool CanContinueDamageCardColorHealth(ProgramSkillFrame f) => f.DamageCardColorHealth is not null && ValidDamageCardColorHealth(f);

    internal bool DamageCardColorHealthFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!ValidDamageCardColorHealth(root))
            return false;
        var r = root.DamageCardColorHealth!;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (child is RecoveryReplacementFrame recovery)
            return !r.IsRed && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(root, recovery, history) && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Amount == (_coreRecoveryAmountBonusesOperations.Value.TracksRecoveryAmountBonuses ? r.Amount : Math.Min(r.Amount, r.MaxHpBefore - r.HpBefore));
        if (child is HpChangedTriggerWindowFrame hp)
            return !r.IsRed && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(root, hp, history) && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.HpBefore == r.HpBefore && hp.Change.Amount == Math.Min(r.Amount, r.MaxHpBefore - r.HpBefore);
        return _coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(root, child);
    }

    internal bool DamageCardColorHealthObserverEdge(int index)
    {
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        if (parent is ProgramSkillFrame root && root.DamageCardColorHealth is not null)
            return DamageCardColorHealthFirstChild(root, child);
        if (parent is RecoveryReplacementFrame recovery)
        {
            var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
            if (child is HpChangedTriggerWindowFrame hp)
                return _coreTypedRecoveryDamageCursorOperations.TypedRecoveryHpReturnMatches(recovery, hp, history);
            if (child is CardsMovedTriggerWindowFrame moved)
                return _coreTypedRecoveryDamageCursorOperations.TypedRecoveryRewardReturnMatches(recovery, moved, history);
        }

        return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) || _coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(parent, child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(index) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index);
    }

    internal ProgramSkillFrame? DamageCardColorHealthObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => root.DamageCardColorHealth is not null && this.DamageCardColorHealthFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => this.DamageCardColorHealthObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 30)]
    internal bool IsDamageCardColorHealthRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery)
    {
        if (frame.DamageCardColorHealth is null || !ValidDamageCardColorHealth(frame) || recovery.ParentFrameId == frame.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && DamageCardColorHealthFirstChild(frame, _resolutionStackOperations._resolutionStack[parent + 1]) && _coreObserverSubtreesOperations.HasObserverSuffix(parent, (index) => this.DamageCardColorHealthObserverEdge(index), (index, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index, dying)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 2403)]
    internal bool AdvanceDamageCardColorHealthChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && DamageCardColorHealthObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    SkillProgramStepOutcome IDamageCardColorHealthHost.ResolveDamageCardColorHealth(ProgramSkillFrame frame) => ResolveDamageCardColorHealth(frame);
}
