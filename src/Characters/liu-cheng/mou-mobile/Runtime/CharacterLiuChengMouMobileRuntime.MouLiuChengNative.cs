#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuChengMouMobileRuntime
{
    internal IReadOnlyList<MouLiuChengUsePlan> MouLiuChengNativePlans(CharacterState actor, CardKind kind, CardConversionSource source)
    {
        if (kind is not (CardKind.Slash or CardKind.Dismantlement) || !actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actor.Seat, kind, [], source)) || _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, [], null) || _coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor.Seat, SkillProgramCardTargetRestriction.SelfOnly))
            return[];
        var result = new List<MouLiuChengUsePlan>();
        void Add(LegalActionKind action, IReadOnlyList<int> targets, string text, int? card = null, CardKind? required = null, bool adjusted = false) => result.Add(new(kind, action, card, required, text, adjusted) { Targets = targets });
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
        {
            var options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: false, conversionSource: source).Where(p => p.ActionKind != LegalActionKind.Recast).ToArray();
            var ordinary = options.Select(p => new LegalAction(p.ActionKind, 0, p.TargetSeats.Count == 1 ? p.TargetSeats[0] : null, p.Description, kind, p.TargetCardId, p.TargetSeats) { ConversionSource = source }).ToList();
            // Use the same genuine zero-material adjustment producers as
            // native actions. Both existing grant types must publish their
            // legal ordered targets before the choice is accepted and paid.
            _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(ordinary, actor);
            _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(ordinary, actor);
            foreach (var a in ordinary)
                Add(a.Kind, a.TargetSeats, a.Description, a.TargetCardId, options.FirstOrDefault()?.RequiredCardKind, _coreRedAdditionalTargetsOperations.IsTargetAdjustmentAction(actor, a));
            return Array.AsReadOnly(result.ToArray());
        }

        var neutral = new Card(0, kind, Suit.None, 0);
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var actions = new List<LegalAction>();
            var targets = _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind, physicalCardIds: [], physicalSuit: Suit.None, effectiveColor: null, effectiveRank: 0, conversionSource: source, countsTowardLimit: false)).ToArray();
            actions.AddRange(targets.Select(t => new LegalAction(LegalActionKind.Slash, 0, t.Seat, $"视为对{(t.Name)}使用【{(neutral.DisplayName)}】", kind) { ConversionSource = source }));
            _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, neutral, targets, neutral.DisplayName, kind, source, null);
            _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(actions, actor);
            _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(actions, actor);
            foreach (var a in actions)
                Add(a.Kind, a.TargetSeats, a.Description, adjusted: a.ProgramActivationId is not null);
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void DispatchMouLiuCheng(ProgramSkillFrame f, MouLiuChengUsePlan p)
    {
        var r = f.MouLiuChengCompletion!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!r.Paid || r.Stage != MouLiuChengStage.UseChoice || p.Kind != r.OutputKind || !MouLiuChengNativePlans(actor, r.OutputKind, MouLiuChengSource(f)).Any(o => SameMouLiuChengPlan(o, p)))
            throw new InvalidOperationException("MouLiuCheng cannot issue another card name or bypass current native legality.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLiuChengCompletion = r with { Stage = MouLiuChengStage.NativeIssuing, Plan = p } });
        r = f.MouLiuChengCompletion!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengPlanAcceptedEvent(f.Id, p.Kind, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Adjusted, p.Targets.Count));
        for (var i = 0; i < p.Targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengTargetAcceptedEvent(f.Id, i, p.Targets[i]));
        var card = new Card(0, p.Kind, Suit.None, 0);
        if (p.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = p.Targets;
        var source = MouLiuChengSource(f);
        var useId = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, p.Targets, p.Kind, physicalCardIds: [], conversionSource: source);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.MouLiuChengOrigin is null)
            throw new InvalidOperationException("MouLiuCheng must bind its distinct zero-material issuance before native effects.");
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind))
        {
            var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!.TargetSeats;
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actor.Seat, targets[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: p.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], p.Kind), programSkillCardUseFrameId: f.Id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, p.Kind);
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(useId, actor);
            actor.HasAlcoholEffect = false;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, p.Kind, actor.Seat, targets[0], IgnoresArmor: attack.IgnoresArmor));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!.TargetSeats, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Kind);
    }

    internal static bool SameMouLiuChengPlan(MouLiuChengUsePlan a, MouLiuChengUsePlan b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_liu_cheng_mou_mobile_MouLiuChengNative.SameMouLiuChengPlan(a, b);
    internal CardActionContext PrepareMouLiuChengAction(CardActionContext action)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLiuChengCompletion: { Stage: MouLiuChengStage.NativeIssuing } r } f)
            return action;
        if (!r.Paid || r.Plan is not { } plan || !MouLiuChengRootMatches(f) || action.Type != CardActionType.Use || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.RequesterSeat is not null || action.PhysicalCards.Count != 0 || action.EffectiveKind != plan.Kind || !action.ConversionChain.SequenceEqual([MouLiuChengSource(f)]))
            throw new InvalidOperationException("MouLiuCheng cannot normalize another producer's action or replace any genuine material appearance.");
        // Generic CaptureCardUseAction has no rank witness for an empty
        // material set and may leave suit/rank unspecified. Like the other
        // actual zero-material producers, MouLiuCheng explicitly signs its neutral
        // appearance before policies, accepted facts and native issuance.
        return _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: null));
    }

    internal void BindMouLiuChengNativeUse(CardUseFrame use)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { MouLiuChengCompletion: { Stage: MouLiuChengStage.NativeIssuing } r } f)
            return;
        if (!MouLiuChengRootMatches(f) || !r.Paid || r.Plan is not { } plan || use.MouLiuChengOrigin is not null || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || a.PhysicalCards.Count != 0 || use.PhysicalCardIds is not { Count: 0 } || a.EffectiveKind != r.OutputKind || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || a.EffectiveIsRed is not null || !a.ConversionChain.SequenceEqual([MouLiuChengSource(f)]) || use.CardId != 0 || !a.TargetSeats.SequenceEqual(use.TargetSeats) || !a.TargetSeats.SequenceEqual(_coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(plan.Kind, f.OwnerSeat, plan.Targets)))
            throw new InvalidOperationException("MouLiuCheng lost its exact separate zero-material native Use and source-bound completion.");
        var origin = new MouLiuChengNativeOrigin(f.Id, MouLiuChengSource(f), f.GameplayHash, use.Id, a.ActionId, f.OwnerSeat, r.OutputKind);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouLiuChengOrigin = origin });
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouLiuChengCompletion = r with { Stage = MouLiuChengStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengNativeIssuedEvent(f.Id, use.Id, a.ActionId, f.OwnerSeat, r.OutputKind));
    }

    internal bool IsIssuedMouLiuChengUse(CardUseFrame use)
    {
        if (use.MouLiuChengOrigin is not { } o || use.Action is not { Type: CardActionType.Use } a || o.UseId != use.Id || o.ActionId != a.ActionId || o.Kind != use.CardKind || a.EffectiveKind != use.CardKind || use.SourceSeat != a.ActorSeat || use.CardId != 0 || a.ProviderSeat != o.ActorSeat || a.RequesterSeat is not null || a.PhysicalCards.Count != 0 || use.PhysicalCardIds is not { Count: 0 } || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || a.EffectiveIsRed is not null || !a.ConversionChain.SequenceEqual([o.Source]) || !_characterZhouYuOlMouOperations.MouGuquActorLineageMatches(use.Id, a.ActionId, o.ActorSeat, a.ActorSeat, a.ProviderSeat, a.Type) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.ProgramFrameId)is not ProgramSkillFrame { MouLiuChengCompletion: { } r } f || !MouLiuChengRootMatches(f) || r.UseId != use.Id || r.ActionId != a.ActionId || o.Source != MouLiuChengSource(f) || o.GameplayHash != f.GameplayHash)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouLiuChengNativeIssuedEvent>().Count(e => e == new MouLiuChengNativeIssuedEvent(f.Id, use.Id, a.ActionId, o.ActorSeat, o.Kind)) == 1 && use.CardId == 0;
    }

    internal bool MatchesMouLiuChengNativeAction(long id, CardActionContext action, int cardId) => cardId == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { } current } use && IsIssuedMouLiuChengUse(use) && action.ActionId == current.ActionId && action.Type == current.Type && action.ActorSeat == current.ActorSeat && action.ProviderSeat == current.ProviderSeat && action.EffectiveKind == current.EffectiveKind && action.PhysicalCards.Count == 0 && action.ConversionChain.SequenceEqual(current.ConversionChain);
    internal bool IsMouLiuChengUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedMouLiuChengUse(use);
    internal bool IsMouLiuChengZeroUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0 } use && IsIssuedMouLiuChengUse(use);
    internal Card MouLiuChengRepresentation(long id) => IsMouLiuChengZeroUse(id) ? new(0, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind, Suit.None, 0) : throw new InvalidOperationException("MouLiuCheng lost its zero-material native card representation.");
    internal Card ReadMouLiuChengUseAppearance(long id, CardAppearanceReference appearance)
    {
        if (!IsMouLiuChengZeroUse(id))
            return _characterXiaoQiaoMouOlOperations.ReadMouXqJueyanAppearance(id, appearance);
        if (appearance.Id != 0 || appearance.Kind != _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind || appearance.Suit != Suit.None || appearance.Rank != 0)
            throw new InvalidOperationException("MouLiuCheng's native continuation changed its exact neutral zero-material appearance.");
        return MouLiuChengRepresentation(id);
    }

    internal bool SkipMouLiuChengFinishedMovement(long id, Card c) => c.Id == 0 && IsMouLiuChengZeroUse(id) && c.Kind == _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind;
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 20)]
    internal bool FinishMouLiuChengAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsMouLiuChengUse(attack.ResolutionId) || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    internal bool IsMouLiuChengAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouLiuChengOrigin: { } o } use || !IsIssuedMouLiuChengUse(use) || attack.Card is not null || attack.PhysicalCards.Count != 0 || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || attack.SourceSeat != use.SourceSeat || attack.EffectiveCardKind != use.CardKind || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && attack.ProgramSkillCardUseFrameId != o.ProgramFrameId || attack.ProgramSkillCardUseFrameId is { } issuingProgram && issuingProgram != o.ProgramFrameId)
            return false;
        var ancestors = MouLiuChengNativeAncestorProcessing(use.Id);
        return ancestors is not null && processing.Select(c => c.Id).Distinct().Order().SequenceEqual(ancestors.Order());
    }

    [NativeUseValidation(29401)]
    internal void ValidateMouLiuChengNativeUse(CardUseFrame use)
    {
        if (use.MouLiuChengOrigin is not null && (!IsIssuedMouLiuChengUse(use) || use.Step != ResolutionFrameStep.Completed || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null))
            throw new InvalidOperationException("MouLiuCheng cannot return before its genuine whole native Use and native effects.");
    }

    [NativeUseReturn(29401)]
    internal void ReturnMouLiuChengNativeUse(CardUseFrame use)
    {
        if (use.MouLiuChengOrigin is not { } o)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLiuChengCompletion: { } r } f || f.Id != o.ProgramFrameId || r.UseId != use.Id || r.ActionId != o.ActionId || r.Returned || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLiuChengNativeReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("MouLiuCheng can return only once to its original paid native parent.");
        var batches = SignMouLiuChengNativeBatches(f, use);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLiuChengCompletion = r with { Stage = MouLiuChengStage.NativeChildren, Returned = true, NativeBatchIds = batches }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengNativeReturnedEvent(f.Id, use.Id, o.ActionId));
    }

    internal IReadOnlyList<long> SignMouLiuChengNativeBatches(ProgramSkillFrame f, CardUseFrame use)
    {
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat).ToArray();
        foreach (var b in batches)
        {
            if (b.Movements.Count == 0 || b.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= f.MouLiuChengCompletion!.PaidSequence || m.Sequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) || b.NativeDrawInvocation is { } draw && !_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw))
                throw new InvalidOperationException("MouLiuCheng's retired native Use lost its exact original movement and draw invoices.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengNativeBatchSignedEvent(f.Id, use.Id, b.Id, b.ParentBatchId, b.Movements.Count));
            foreach (var m in b.Movements)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengNativeMovementSignedEvent(f.Id, use.Id, b.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
        }

        return Array.AsReadOnly(batches.Select(b => b.Id).ToArray());
    }

    internal bool IsMouLiuChengNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.MouLiuChengCompletion is { Stage: MouLiuChengStage.NativeChildren, Returned: true } r && MouLiuChengRootMatches(f) && b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.Movements.Count > 0 && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat && r.NativeBatchIds.Contains(b.Id) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLiuChengNativeBatchSignedEvent>().Count(e => e == new MouLiuChengNativeBatchSignedEvent(f.Id, r.UseId, b.Id, b.ParentBatchId, b.Movements.Count)) == 1 && b.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLiuChengNativeMovementSignedEvent>().Count(e => e == new MouLiuChengNativeMovementSignedEvent(f.Id, r.UseId, b.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 900)]
    internal bool WindowSettlement_IsMouLiuChengNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsMouLiuChengNativeSettlementBatch(frame, batch);
}
