#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramTurnPhaseRuntime
{
    internal SkillProgramStepOutcome SkipProgramTurnPhases(ProgramSkillFrame frame, IReadOnlyList<SkillProgramTurnPhase> phases)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.TriggerId is null || active.WindowContext is not { } context || context.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnProgressionAccess._currentSeat || active.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnProgressionAccess._currentSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.Window != context.Window || phases.Count == 0 || phases.Distinct().Count() != phases.Count)
            throw new InvalidOperationException("Phase substitution requires the current lifecycle program.");
        var first = context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && phases.All(phase => phase is SkillProgramTurnPhase.Judgment or SkillProgramTurnPhase.Draw or SkillProgramTurnPhase.Play);
        var second = context.Window == SkillProgramTriggerWindow.AfterNormalDraw && phases.Count == 1 && phases[0] == SkillProgramTurnPhase.Play;
        var third = context.Window == SkillProgramTriggerWindow.DiscardPhaseStarting && phases.Count == 1 && phases[0] == SkillProgramTurnPhase.Discard;
        var actualJudgment = context.Window == SkillProgramTriggerWindow.JudgmentPhaseStarting && phases.SequenceEqual([SkillProgramTurnPhase.Judgment]);
        var actualDraw = context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && phases.SequenceEqual([SkillProgramTurnPhase.Draw]);
        if (!first && !second && !third && !actualJudgment && !actualDraw)
            throw new InvalidOperationException("The configured phase substitution does not match its boundary.");
        _coreShanheS5ShensuOperations.RecordShanheS5PhaseSkipCost(active, phases, parent);
        if (actualDraw && parent.ResumeDrawPhaseObligationFrameId is not null)
        {
            _coreAlternativePhaseCostProgramsOperations.RecordAlternativePhaseSkip(active, phases);
            return SkillProgramStepOutcome.Continue;
        }

        foreach (var phase in phases)
            _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= phase switch
            {
                SkillProgramTurnPhase.Judgment => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipJudgmentPhase,
                SkillProgramTurnPhase.Draw => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase,
                SkillProgramTurnPhase.Play => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipPlayPhase,
                SkillProgramTurnPhase.Discard => global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDiscardPhase,
                _ => throw new InvalidOperationException("Unknown turn phase.")};
        _coreAlternativePhaseCostProgramsOperations.RecordAlternativePhaseSkip(active, phases);
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome BeginProgramVirtualCardUse(ProgramSkillFrame frame, int targetSeat, CardKind cardKind, bool ignoreDistance)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var instruction = ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!).GetPausedInstruction(active.InstructionIndex).Effect;
        if (active.TriggerId is null && instruction.Op is not (SkillProgramEffectOp.OfferUnlimitedVirtualSlash or SkillProgramEffectOp.UseVirtualCard) || active.SelectedTargetSeats.Count != 1 || active.SelectedTargetSeats[0] != targetSeat || cardKind != CardKind.Slash || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat))
            throw new InvalidOperationException("Virtual card use requires one current selected Slash target.");
        var source = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var activeUse = active.TriggerId is null && instruction.Op == SkillProgramEffectOp.UseVirtualCard;
        var phaseControlParticipant = _corePhaseStartControlPoliciesOperations.HasPhaseStartCardUseParticipant(source.Seat);
        if ((phaseControlParticipant || activeUse || _coreShanheS5ShensuOperations.IsShanheS5ShensuFrame(active) || _coreShanheXingtianOperations.HasShanheXingtianSeal(source.Seat)) && (_coreCardUseModulesOperations.IsCardUseForbidden(source.Seat, cardKind, CardActionType.Use) || _coreCardUseModulesOperations.HasTurnCardTargetRestriction(source.Seat, SkillProgramCardTargetRestriction.SelfOnly)))
            return SkillProgramStepOutcome.Continue;
        if (active.WindowContext is { Window: SkillProgramTriggerWindow.RoundStarting } round && (!instruction.UseCardActionWindows || round.Facts?.RoundTurnSeatCount != 1 || _coreActualRoundProgressRoundTurnSeatsAccess._roundTurnSeats.Count != 1 || _coreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression.Kind == ActualTurnKind.Extra || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != round.ParentFrameId || parent.Window != SkillProgramTriggerWindow.RoundStarting || parent.Continuation != ProgramLifecycleContinuation.RoundProgramsTurnStart))
            throw new InvalidOperationException("A round-first virtual Slash lost its actual normal-turn boundary and native continuation.");
        if (active.WindowContext?.Window == SkillProgramTriggerWindow.RoundStarting && (_coreCardUseModulesOperations.IsCardUseForbidden(source.Seat, CardKind.Slash, CardActionType.Use) || _coreCardUseModulesOperations.HasTurnCardTargetRestriction(source.Seat, SkillProgramCardTargetRestriction.SelfOnly)))
            return SkillProgramStepOutcome.Continue;
        if (!source.IsAlive || !target.IsAlive || source.Seat == target.Seat || _coreShanheS5ShensuOperations.IsShanheS5ShensuFrame(active) && !_coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(source, target, physicalSuit: Suit.None, physicalCardIds: [], ignoreDistance: ignoreDistance, countsTowardLimit: false) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, targetSeat, cardKind) || _coreNativeDamagePoliciesOperations.IsSlashProhibited(target) || !ignoreDistance && !(_characterCaiWenJiLeOperations.HasBeifenPreUsePrivilege(source) || _characterHuangGaiMouMobileOperations.HasMouZhaJiangPreUsePrivilege(source)) && _coreNativeRuleQueriesOperations.GetSeatDistance(source.Seat, targetSeat) > _coreNativeRuleQueriesOperations.GetAttackRange(source.Seat))
            return SkillProgramStepOutcome.Continue;
        var resolutionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var nativeWindows = instruction.UseCardActionWindows || _coreCurrentSlashFireOffersOperations.HasCommittedSlashFireCapability(source) || _characterLuBuHappyGodOperations.HasHappyLuBuWuqianCapability(source) || _characterLeJiuAnniversaryOperations.HasAnniversaryLeJiuUseObserver(source);
        var action = nativeWindows || _characterHuangYueyingOlMouOperations.HasMouHuangYueyingBingcaiCapability || _characterZhouQunOlOperations.HasZhouQunCapability || _characterKongShuOlOperations.HasKongShuCapability || _characterMaChengOlOperations.HasMaChengCapability || _characterCuiYanOlOperations.HasCuiYanCapability || _characterLiYiOlOperations.HasLiYiCapability || _characterCaiWenJiLeOperations.HasLeCaiWenJiBeifenCapability || _characterLiuYanOlOperations.HasLiuYanCapability || _coreNativeLogicalDiscardsOperations.TracksNativeLogicalDiscards || _characterLiuZanMobileOperations.TracksAlternateActualUseColor || _coreSoleTargetProtectionOperations.HasSoleTargetProtectionParticipant || phaseControlParticipant || _characterSunHaoOlOperations.HasSunHaoCanshiCapability ? _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, source.Seat, source.Seat, null, null, null, cardKind, [targetSeat], [], Array.AsReadOnly(new[] { new CardConversionSource(active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, active.SkillInstanceId) }), effectiveSuit: Suit.None, effectiveRank: 0)) : null;
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(resolutionId, source.Seat, 0, cardKind, Array.AsReadOnly(new[] { targetSeat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>())) { Action = action });
        if (action is not null)
            _corePhaseNamePredictionOperations.ObservePhaseNamePredictionUse(action);
        else
            _corePhaseNamePredictionOperations.RecordPredictionNativeCardUse(resolutionId, source.Seat, cardKind);
        if ((instruction.Op == SkillProgramEffectOp.OfferUnlimitedVirtualSlash || instruction.TargetRestriction == SkillProgramCardTargetRestriction.NormalSlashTarget || _coreCurrentSlashFireOffersOperations.HasCommittedSlashFireCapability(source) || phaseControlParticipant || activeUse || _coreShanheS5ShensuOperations.IsShanheS5ShensuFrame(active)) && action is not null)
        {
            _characterLuMengMouOperations.RecordYingboCardUse(resolutionId, source.Seat, cardKind);
            _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(source.Seat, cardKind);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(source.Seat, cardKind);
            _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        }

        if (action is not null && _corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(resolutionId, 0, cardKind, source.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(resolutionId, Array.AsReadOnly(new[] { targetSeat })));
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(resolutionId, source.Seat, targetSeat, card: null, damageAmount: nativeWindows ? 1 + source.AlcoholDamageBonus + _coreShanheS5ShensuOperations.ShanheS5ShensuBaseDamageBonus(active) : 1, playedCardKind: cardKind, ignoresArmor: nativeWindows && _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(source, target, cardKind), conversionSource: action?.ConversionChain.Single(), programSkillCardUseFrameId: frame.Id);
        if (nativeWindows && action is not null)
        {
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(resolutionId, source);
            source.HasAlcoholEffect = false;
        }

        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, cardKind, source.Seat, targetSeat));
        if (action is not null)
        {
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(resolutionId);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
        }
        else if (!_coreFirstSlashDuelLegacyWindowsOperations.TryBeginFirstSlashDuelLegacyWindow(attack) && !_coreSlashTargetPenaltyWindowsOperations.TryBeginSlashTargetPenalties(attack, legacy: true) && !_coreSlashTargetBenefitWindowsOperations.TryBeginSlashTargetBenefits(attack, legacy: true) && !_coreActualUseTargetProgramsOperations.TryBeginActualUseTargetPrograms(attack, ActualUseTargetReturnKind.LegacyVirtualSlash))
            _coreNativeSlashUseOperations.ContinueSlashAfterResponsePrograms(attack);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
