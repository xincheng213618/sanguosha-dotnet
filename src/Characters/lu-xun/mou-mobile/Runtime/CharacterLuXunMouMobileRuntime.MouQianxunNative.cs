#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunMouMobileRuntime
{
    internal IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> MouLuXunUseOptions(ProgramSkillFrame f)
    {
        if (f.MouLuXun is not { Kind: { } kind, Paid: true } || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []);
        return _coreNextActualUseTargetAdjustmentsOperations.NextActualUseOrdinaryTrickOptions(actor, options, Suit.None, null, Suit.None, false);
    }

    internal bool IsMouLuXunVirtualParent(ProgramSkillFrame f) => f.MouLuXun is { Operation: SkillProgramEffectOp.UseMouLuXunRecordedTrick, Paid: true, Kind: { } kind, Plan: not null } r && r.Stage is MouLuXunStage.IssuingUse or MouLuXunStage.NativeUse or MouLuXunStage.NativeReturned && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && ValidMouLuXun(f) && f.WindowContext?.Window == SkillProgramTriggerWindow.PlayPhaseStarting && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuXunNameRemovedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Kind == kind) == 1;
    internal SkillProgramEffect MouLuXunVirtualInstruction(ProgramSkillFrame f)
    {
        if (!IsMouLuXunVirtualParent(f))
            throw new InvalidOperationException("Qianxun lost its paid named-record native-use parent.");
        return new(SkillProgramEffectOp.UseVirtualOrdinaryTrick, SkillProgramEffectTarget.Owner, 0, new(SkillProgramConditionKind.Always, 0, []), outputKind: f.MouLuXun!.Kind, actorReference: new(ProgramParticipantRef.Owner), targetReference: new(ProgramParticipantRef.Owner));
    }

    internal void IssueMouLuXunUse(ProgramSkillFrame f, int index)
    {
        var r = f.MouLuXun!;
        var options = MouLuXunUseOptions(f);
        if (!ValidMouLuXun(f) || r.Stage != MouLuXunStage.UseChoice || index < 0 || index >= options.Count)
            throw new InvalidOperationException("Qianxun must consume its one record and use a published native plan.");
        var plan = options[index];
        var kind = r.Kind!.Value;
        var targets = _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(kind, f.OwnerSeat, plan.TargetSeats);
        var selected = new MouLuXunUsePlan(plan.ActionKind, plan.TargetCardId, plan.RequiredCardKind, plan.NextActualUseAdjusted)
        {
            Targets = targets
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Stage = MouLuXunStage.IssuingUse, Plan = selected } });
        if (selected.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = targets;
        var origin = new ProgramVirtualOrdinaryTrickOrigin(f.Id, f.InstructionIndex, r.WindowId, f.OwnerSeat, f.OwnerSeat, f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, r.ActualTurn, kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, kind, Suit.None, 0);
        var useId = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, targets, kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!;
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { MouLuXun = f.MouLuXun!with { Stage = MouLuXunStage.NativeUse, UseId = useId, ActionId = use.Action!.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunNativeIssuedEvent(f.Id, useId, use.Action!.ActionId, f.OwnerSeat, kind));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, f.OwnerSeat, use.TargetSeats, selected.ActionKind, selected.TargetCardId, selected.RequiredKind, kind);
    }

    internal bool ReturnMouLuXunNativeUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { } origin || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuXun: { } r } f || origin.ParentProgramFrameId != f.Id || r.Operation != SkillProgramEffectOp.UseMouLuXunRecordedTrick)
            return false;
        if (!IsMouLuXunVirtualParent(f) || r.Stage != MouLuXunStage.NativeUse || r.UseId != use.Id || use.Step != ResolutionFrameStep.Completed || use.Action?.ActionId != r.ActionId || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == 0 && e.CardKind == r.Kind) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuXunNativeReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Qianxun's record cannot pay or return a second native use.");
        var attack = use.CardAttack is { Active: true };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Stage = MouLuXunStage.NativeReturned } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunNativeReturnedEvent(f.Id, use.Id, attack));
        if (!attack)
            FinishMouLuXun(f);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 32801)]
    internal bool ResumeMouLuXunNativeAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuXun: { Stage: MouLuXunStage.NativeReturned } r } f)
            return false;
        if (!ValidMouLuXun(f) || _resolutionStackOperations._resolutionStack.Any(x => x.Id == r.UseId) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuXunNativeReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.HadAttack) != 1)
            throw new InvalidOperationException("Qianxun's native attack must clean up before its original Play-start returns.");
        FinishMouLuXun(f);
        return true;
    }
}
