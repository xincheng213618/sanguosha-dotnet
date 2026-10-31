#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal const string WuLuXunZhangcaiDrawReason = "skill-program.wu-lu-xun.zhangcai.draw";
    internal int? CaptureWuLuXunResponseRank(CharacterState provider, CardKind effectiveKind, IReadOnlyList<CardActionCost> costs, CardConversionSource? source)
    {
        if (!HasWuLuXunZhangcaiCapability || costs is not [var cost])
            return null;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId);
        // Existing payment-aware Juewu/Sirui/Alcohol captures take precedence at the caller.
        // Appearance is otherwise read from the actual one-card conversion, never its output name.
        return _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholResponseRank(provider, effectiveKind, costs) ?? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(provider, card, source).Rank;
    }

    internal static string WuLuXunZhangcaiBinding(SkillProgramTriggerWindow window) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lu_xun_wumiao_WuLuXunZhangcai.WuLuXunZhangcaiBinding(window);
    internal void CaptureWuLuXunZhangcai(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!HasWuLuXunZhangcaiCapability || action.ActionId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardResponseAccepted or SkillProgramTriggerWindow.CardSupplyCompleted) || window == SkillProgramTriggerWindow.CardUseCommitted && action.Type != CardActionType.Use || window == SkillProgramTriggerWindow.CardResponseAccepted && action.Type != CardActionType.Response)
            return;
        IEnumerable<int> owners = window == SkillProgramTriggerWindow.CardResponseAccepted ? new[]
        {
            action.ActorSeat,
            action.ProviderSeat
        }.Distinct() : [action.ActorSeat];
        foreach (var ownerSeat in owners)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
            if (!owner.IsAlive)
                continue;
            var ruxian = WuLuXunActiveRuxian(ownerSeat);
            if (action.EffectiveRank != 8 && ruxian is null)
                continue;
            var paidHand = action.PhysicalCards.Where(c => c.From == CardLocation.Hand(ownerSeat)).Select(c => c.CardId).ToHashSet();
            var remaining = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count(c => !paidHand.Contains(c.Id) && _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(owner, c) == 8);
            foreach (var binding in _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetInstanceTriggers(window).Where(b => b.SkillId == "wumiao:zhangcai" && b.Trigger.Id == WuLuXunZhangcaiBinding(window) && b.Trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveWuLuXunZhangcai }]))
            {
                var source = new CardConversionSource(binding.SkillId, binding.Trigger.Id, ownerSeat, binding.SkillInstanceId);
                if (_coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunZhangcaiQualifiedEvent>().Any(e => e.Qualification.Source == source && e.Qualification.ActionId == action.ActionId))
                    continue;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuXunZhangcaiQualifiedEvent(new(action.ActionId, window, source, binding.Program.GameplayHash, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, action.EffectiveRank, remaining, Math.Max(1, remaining), ruxian?.FrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)));
            }
        }
    }

    internal WuLuXunZhangcaiQualification? WuLuXunZhangcaiQualificationFor(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != "wumiao:zhangcai" || context.CardUse is not { } use || candidate.BindingId != WuLuXunZhangcaiBinding(context.Window))
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunZhangcaiQualifiedEvent>().Select(e => e.Qualification).SingleOrDefault(q => q.ActionId == use.CardActionId && q.Window == context.Window && q.Source.OwnerSeat == candidate.OwnerSeat && q.Source.SkillId == candidate.SkillId && q.Source.BindingId == candidate.BindingId && q.Source.SkillInstanceId == candidate.SkillInstanceId && q.GameplayHash == candidate.GameplayHash);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29002, SkillProgramEffectOp.ResolveWuLuXunZhangcai)]
    internal bool CanOfferWuLuXunZhangcai(ProgramTriggerLegalityContext gate) => gate.Owner.IsAlive && WuLuXunZhangcaiQualificationFor(gate.Candidate, gate.Context)is { } q && !_coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunZhangcaiStartedEvent>().Any(e => e.Source.OwnerSeat == gate.OwnerSeat && e.Source.SkillInstanceId == q.Source.SkillInstanceId && e.ActionId == q.ActionId);
    internal SkillProgramStepOutcome BeginWuLuXunZhangcai(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WuLuXunZhangcai is not null)
            throw new InvalidOperationException("Zhangcai cannot issue the same accepted action twice.");
        if (f.SkillId != "wumiao:zhangcai" || f.TriggerId != f.ActivationId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || WuLuXunZhangcaiQualificationFor(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context)is not { } q || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || !WuLuXunZhangcaiWindowMatches(window, q) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, window) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunZhangcaiStartedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillInstanceId == f.SkillInstanceId && e.ActionId == q.ActionId))
            throw new InvalidOperationException("Zhangcai needs its frozen actual Use/Played payment, eligible point rank and unique original actor/provider opportunity.");
        var r = new WuLuXunZhangcaiReceipt(q, window.Id, window.ParentFrameId, WuLuXunZhangcaiStage.Drawing);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuXunZhangcai = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(WuLuXunZhangcaiStart(f.Id, r));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【彰才】，摸 {(q.DrawCount)} 张牌。", f.OwnerSeat);
        ContinueWuLuXunZhangcai(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static WuLuXunZhangcaiStartedEvent WuLuXunZhangcaiStart(long id, WuLuXunZhangcaiReceipt r) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lu_xun_wumiao_WuLuXunZhangcai.WuLuXunZhangcaiStart(id, r);
    internal bool WuLuXunZhangcaiWindowMatches(ProgramCardTriggerWindowFrame w, WuLuXunZhangcaiQualification q)
    {
        var action = q.Window == SkillProgramTriggerWindow.CardSupplyCompleted && w.CardSupplyCompletion is not null ? global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime.CardSupplyCompletionActorAction(w) : w.Action;
        return global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) == q.Window && action.ActionId == q.ActionId && action.ActorSeat == q.ActorSeat && action.ProviderSeat == q.ProviderSeat && action.EffectiveKind == q.EffectiveKind && action.EffectiveRank == q.EffectiveRank && (q.Source.OwnerSeat == action.ActorSeat || q.Window == SkillProgramTriggerWindow.CardResponseAccepted && q.Source.OwnerSeat == action.ProviderSeat);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveWuLuXunZhangcai)]
    internal void ReturnWuLuXunZhangcaiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.WuLuXunZhangcai is not { Stage: WuLuXunZhangcaiStage.Drawing } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.Qualification.DrawCount || _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunZhangcaiDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Zhangcai cannot replace or repeat its issued native draw request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuXunZhangcai = r with { Stage = WuLuXunZhangcaiStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuXunZhangcaiDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueWuLuXunZhangcai(f.Id);
    }

    internal void ContinueWuLuXunZhangcai(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuXunZhangcai: { } r } f || f.Id != id)
            return;
        if (r.Stage == WuLuXunZhangcaiStage.Drawing)
        {
            if (f.NativeDraw is null)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.Qualification.DrawCount, new(WuLuXunZhangcaiDrawReason));
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuXunZhangcaiCompletedEvent(f.Id, r.Qualification.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { WuLuXunZhangcai = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29002)]
    internal bool ResumeWuLuXunZhangcai(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuXunZhangcai: not null } f || f.Id != id)
            return false;
        ContinueWuLuXunZhangcai(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29002)]
    internal bool ReturnWuLuXunZhangcaiMovement(ProgramSkillFrame f)
    {
        if (f.WuLuXunZhangcai is null || f.PendingMovementContinuation is null)
            return false;
        ContinueWuLuXunZhangcai(f.Id);
        return true;
    }
}
