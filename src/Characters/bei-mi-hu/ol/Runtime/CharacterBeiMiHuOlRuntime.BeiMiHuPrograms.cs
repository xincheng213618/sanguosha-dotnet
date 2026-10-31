#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBeiMiHuOlRuntime
{
    internal const string BeiMiHuDrawReason = "program.bei-mi-hu.guju-draw";
    internal static bool IsBeiMiHuOp(SkillProgramEffectOp op) => global::CardGame.Core.RuntimeSkills.RuntimeRules_bei_mi_hu_ol_BeiMiHuPrograms.IsBeiMiHuOp(op);
    internal static SkillProgramEffectOp? BeiMiHuOperation(SkillProgramTrigger t) => global::CardGame.Core.RuntimeSkills.RuntimeRules_bei_mi_hu_ol_BeiMiHuPrograms.BeiMiHuOperation(t);
    internal CardConversionSource BeiMiHuSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int BeiMiHuRound => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundStartedEvent>().LastOrDefault()?.RoundNumber ?? _coreActualRoundProgressRoundNumberAccess._roundNumber;

    internal int BeiMiHuReceiptTurn(ProgramSkillFrame f) => f.WindowContext?.BeforeTurnPreludeProof?.UpcomingTurnNumber ?? _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 390)]
    internal bool CanOfferBeiMiHuLifecycle(int owner, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var op = BeiMiHuOperation(trigger);
        if (op is null)
            return true;
        if (op == SkillProgramEffectOp.MarkUnmarkedOtherBeforeActualTurn)
            return context.Window == SkillProgramTriggerWindow.TurnBeforeStart && context.BeforeTurnPreludeProof is { } proof && proof.UpcomingOwnerSeat == owner && ValidBeiMiHuBeforeTurnPrelude(proof) && BeiMiHuUnmarkedOthers(owner).Length > 0;
        if (op == SkillProgramEffectOp.MarkLowestHpUnmarkedOtherAtRoundStart)
            return context.Window == SkillProgramTriggerWindow.RoundStarting && context.BeforeTurnPreludeProof is { } roundProof && ValidBeiMiHuBeforeTurnPrelude(roundProof) && BeiMiHuRound > 0 && BeiMiHuLowestUnmarkedOthers(owner).Length > 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<BeiMiHuKuiPaidEvent>().Any(e => e.Source.OwnerSeat == owner && e.Kind == "round" && e.Round == BeiMiHuRound);
        if (op == SkillProgramEffectOp.AwakenFromActualGujuCards)
            return context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && owner == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[owner].HasHpValue && (BeiMiHuActualCards(owner) >= 7 || _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(owner, "ol:baijia", trigger.Id));
        return true;
    }

    internal bool ExactBeiMiHuLifecycleParent(ProgramSkillFrame f)
    {
        var c = f.WindowContext;
        return f.TriggerId is not null && c is not null && f.InstructionIndex == 1 && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is { } p && p.Window == c.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && (c.Window is not (SkillProgramTriggerWindow.TurnBeforeStart or SkillProgramTriggerWindow.RoundStarting) || c.BeforeTurnPreludeProof is { } proof && proof == p.BeforeTurnPreludeProof && (c.Window != SkillProgramTriggerWindow.TurnBeforeStart || proof.UpcomingOwnerSeat == f.OwnerSeat) && ValidBeiMiHuBeforeTurnPrelude(proof));
    }

    internal BeiMiHuReceipt NewBeiMiHuReceipt(ProgramSkillFrame f, string stage) => new()
    {
        Source = BeiMiHuSource(f),
        GameplayHash = f.GameplayHash,
        ParentFrameId = f.WindowContext!.ParentFrameId,
        Turn = BeiMiHuReceiptTurn(f),
        Round = f.WindowContext.BeforeTurnPreludeProof?.ActualRoundNumber ?? BeiMiHuRound,
        InstructionIndex = f.InstructionIndex,
        Stage = stage
    };
    internal SkillProgramStepOutcome BeginBeiMiHuOperation(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.BeiMiHu is not null || f.InstructionIndex != 1 || !IsBeiMiHuOp(op))
            throw new InvalidOperationException("Bei Mi Hu lost its original unpaid instruction.");
        return op switch
        {
            SkillProgramEffectOp.MarkUnmarkedOtherBeforeActualTurn or SkillProgramEffectOp.MarkLowestHpUnmarkedOtherAtRoundStart => BeginBeiMiHuMark(f, op),
            SkillProgramEffectOp.DrawAfterActualKuiDamage => BeginBeiMiHuGuju(f),
            SkillProgramEffectOp.AwakenFromActualGujuCards => BeginBeiMiHuAwakening(f),
            _ => BeginBeiMiHuCardTargets(f, op)};
    }

    internal void PayBeiMiHuKui(ProgramSkillFrame f, int target, int delta, string kind)
    {
        var r = f.BeiMiHu!;
        var old = _coreMatchParticipantsPlayersAccess._players[target].Markers.GetValueOrDefault(PlayerMarkerKind.Kui);
        if (delta == 1 ? old != 0 : old != 1)
            throw new InvalidOperationException("The accepted public binary Kui payment changed.");
        _coreStrategicProgramsOperations.SetStrategicMarker(target, PlayerMarkerKind.Kui, f.OwnerSeat, old + delta, f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeiMiHuKuiPaidEvent(f.Id, r.Source, r.GameplayHash, r.ParentFrameId, r.Turn, r.Round, kind, target, delta, old + delta));
    }

    internal IReadOnlyList<PromptChoice> BeiMiHuChoices(ProgramSkillFrame f)
    {
        var r = f.BeiMiHu!;
        Dictionary<string, string> Args(string branch) => new()
        {
            ["program-action"] = "bei-mi-hu",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        if (r.Stage is "round-choice" or "before-turn-choice")
            return Array.AsReadOnly(r.Candidates.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreMatchParticipantsPlayersAccess._players[s].Markers.GetValueOrDefault(PlayerMarkerKind.Kui) == 0).Select(s => new PromptChoice(new($"bei-mi-hu.{(f.Id)}.mark-{(s)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[s].Name)} 获得傀", [], [s], Args("mark"))).ToArray());
        if (r.Stage == "append-choice")
        {
            var use = RequireBeiMiHuCardParent(f, out _);
            var choices = r.Choices.Where(c => c.Targets.Count > 0 && !r.SelectedTargets.Where((_, i) => use.CardKind != CardKind.BorrowedSword || i % 2 == 0).Contains(c.Targets[0]) && BeiMiHuTargetGroupLegal(use, c.Targets)).ToList();
            choices.Add(new(new($"bei-mi-hu.{(f.Id)}.finish"), "完成本次追加目标", [], [], Args("finish")));
            return Array.AsReadOnly(choices.ToArray());
        }

        return[];
    }

    internal void PublishBeiMiHuChoice(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishBeiMiHu(f);
            return;
        }

        var choices = BeiMiHuChoices(f);
        if (choices.Count == 0)
        {
            FinishBeiMiHu(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "卑弥呼：选择本次真实结算的角色。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("bei-mi-hu")]
    internal void ResolveBeiMiHuChoice(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BeiMiHu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], BeiMiHuChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Bei Mi Hu lost its actual chooser and frozen public candidates.");
        AssertBeiMiHu(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters["branch"] == "mark")
        {
            var target = selected.Targets.Single();
            PayBeiMiHuKui(f, target, 1, r.Stage == "round-choice" ? "round" : "before-turn");
            FinishBeiMiHu(f);
            return;
        }

        if (selected.Parameters["branch"] == "append")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BeiMiHu = r with { SelectedTargets = r.SelectedTargets.Concat(selected.Targets).ToArray() } });
            PublishBeiMiHuChoice(f);
            return;
        }

        CompleteBeiMiHuTargetAddition(f);
    }

    internal SkillProgramEffect? BeiMiHuPausedEffect(ProgramSkillFrame f) => f.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal bool DrainBeiMiHuChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 23200)]
    internal bool ResumeBeiMiHu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BeiMiHu: { } r } f || f.Id != id)
            return false;
        AssertBeiMiHu(f);
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd())
            return true;
        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnBeiMiHuMovement(f);
            return true;
        }

        if (DrainBeiMiHuChildren(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.BeiMiHu!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishBeiMiHu(f);
            return true;
        }

        if (r.Stage is "round-choice" or "before-turn-choice" or "append-choice")
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBeiMiHuChoice(f);
            return true;
        }

        if (r.Stage == "maximum-children")
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var requested = owner.HasHpValue ? Math.Min(1, Math.Max(0, owner.MaxHp - owner.Hp)) : 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BeiMiHu = r with { Stage = "recovery-children", RecoveryRequested = requested } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeiMiHuRecoveryIssuedEvent(f.Id, f.OwnerSeat, requested));
            if (requested > 0)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, requested, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        if (r.Stage == "recovery-children")
        {
            var targets = BeiMiHuUnmarkedOthers(f.OwnerSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BeiMiHu = r with { Stage = "grants-children", SelectedTargets = targets } });
            foreach (var target in targets)
                PayBeiMiHuKui(f, target, 1, "awakening");
            _coreProgramSkillChangesOperations.GrantProgramSkills(f, ["ol:canshi"]);
            foreach (var grant in _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == "ol:guju").ToArray())
                _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.RemoveGrant(grant.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeiMiHuAwakeningCompletedEvent(f.Id, f.OwnerSeat, targets, "ol:guju", "ol:canshi"));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        FinishBeiMiHu(f);
        return true;
    }

    internal void FinishBeiMiHu(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BeiMiHu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome global::CardGame.Core.IBeiMiHuProgramHost.BeginBeiMiHuOperation(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginBeiMiHuOperation(f, op);
}
