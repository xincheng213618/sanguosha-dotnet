#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFenAnniversaryRuntime
{
    internal void IssueDagongCheSlash(ProgramSkillFrame f, IReadOnlyList<int> targets)
    {
        var r = f.ZhangFen!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (r.Stage != ZhangFenStage.Targets || !DagongTargetSets(DagongLegalTargets(f.OwnerSeat, r.IgnoreRangeArmor), r.Count).Any(s => s.SequenceEqual(targets)))
            throw new InvalidOperationException("Dagong Che lost its exact original legal native Slash targets.");
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, CardKind.Slash, targets, [], [], effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: false));
        var origin = new DagongCheSlashOrigin(f.Id, f.InstructionIndex, r.Source, r.Hash, r.CartId, useId, action.ActionId, r.Turn, 1 + DagongUpgrades(r.CartId).Count(e => e.Choice == "discard"), r.IgnoreRangeArmor);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangFen = r with { Stage = ZhangFenStage.NativeUse, Targets = targets, UseId = useId, ActionId = action.ActionId }, VirtualBasicDraft = new(f.InstructionIndex, useId) });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash, targets, IgnoresArmor: r.IgnoreRangeArmor, PhysicalCardIds: []) { Action = action, VirtualBasicReturn = new(f.Id, f.InstructionIndex), UnlimitedUse = true, DagongCheSlash = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DagongCheSlashIssuedEvent(origin));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor.Seat, r.IgnoreRangeArmor));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, targets));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actor.Seat, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actor.Seat, targets[0], null, 1 + actor.AlcoholDamageBonus, CardKind.Slash, ignoresArmor: r.IgnoreRangeArmor || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(useId, actor);
        actor.HasAlcoholEffect = false;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targets[0], IgnoresArmor: attack.IgnoresArmor));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }
    internal bool ExactDagongCheUse(CardUseFrame use, ProgramSkillFrame f) => use.DagongCheSlash is { } o && f.ZhangFen is { } r && ValidZhangFen(f) && o.FrameId == f.Id && o.Instruction == f.InstructionIndex && o.Source == r.Source && o.Hash == f.GameplayHash && o.CartId == r.CartId && o.Turn == r.Turn && r.Stage == ZhangFenStage.NativeUse && r.UseId == use.Id && o.UseId == use.Id && o.ActionId == r.ActionId && use.CardId == 0 && use.CardKind == CardKind.Slash && use.PhysicalCardIds is { Count: 0 } && use.VirtualBasicReturn == new ProgramVirtualBasicReturn(f.Id, f.InstructionIndex) && use.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, PhysicalCards.Count: 0, ConversionChain.Count: 0 } a && a.ActionId == o.ActionId && a.ActorSeat == f.OwnerSeat && a.ProviderSeat == f.OwnerSeat && a.RequesterSeat is null && _coreProgramEventHistoryOperations.ProgramEventHistory<DagongCheSlashIssuedEvent>().Count(e => e.Origin == o) == 1;
    [NativeUseValidation(43500)]
    internal void ValidateDagongCheUseReturn(CardUseFrame use)
    {
        if (use.DagongCheSlash is not { } o)
            return;
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.FrameId)is not { } parent || use.Step != ResolutionFrameStep.Completed || !ExactDagongCheUse(use, parent))
            throw new InvalidOperationException("Dagong Che must return its exact completed original zero-material Slash.");
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 43500)]
    internal bool CompleteDagongCheAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { DagongCheSlash: { } o } use || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == o.FrameId)is not { } f || !ExactDagongCheUse(use, f))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 43500)]
    internal bool ReturnDagongCheAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangFen: { Stage: ZhangFenStage.NativeReturned } r } f || completion.ResolutionId != r.UseId)
            return false;
        if (completion.ProgramFrameId != f.Id || f.VirtualBasicDraft is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId && e.CardId == 0 && e.CardKind == CardKind.Slash) != 1)
            throw new InvalidOperationException("Dagong Che's completed native attack must return to its exact issued program parent.");
        FinishZhangFen(f);
        return true;
    }
}
