#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuHuanMobileRuntime
{
    internal const string MobileZhuHuanPingkouSkill = "mobile:pingkou", MobileZhuHuanPingkouAction = "mobile-zhu-huan-pingkou";
    internal static CardConversionSource MobileZhuHuanPingkouSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IMobileZhuHuanPingkouHost.Begin(ProgramSkillFrame frame) => BeginMobileZhuHuanPingkou(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 72500)]
    internal bool CanOfferMobileZhuHuanPingkou(ProgramTriggerLegalityContext gate) => gate.SkillId != MobileZhuHuanPingkouSkill || gate.Owner.Seat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _characterZhuHuanBoundaryMobileOperations.BoundaryPingkouMaximum(gate.Owner.Seat) > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != gate.Owner.Seat);
    internal SkillProgramStepOutcome BeginMobileZhuHuanPingkou(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MobileZhuHuanPingkouSkill || f.TriggerId != "actual-ended-skips" || f.InstructionIndex != 1 || f.MobileZhuHuanPingkou is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().Any(p => p.Id == context.ParentFrameId && p.OwnerSeat == f.OwnerSeat && p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber) || _characterZhuHuanBoundaryMobileOperations.BoundaryPingkouMaximum(f.OwnerSeat) < 1)
            throw new InvalidOperationException("Mobile Pingkou starts on its owner's genuine ended turn and actual native skipped-phase history.");
        var r = new MobileZhuHuanPingkouReceipt(MobileZhuHuanPingkouSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, context.ParentFrameId, _characterZhuHuanBoundaryMobileOperations.BoundaryPingkouMaximum(f.OwnerSeat));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouStartedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, r.ActualTurn, r.EndedFrame, r.Maximum));
        PublishMobileZhuHuanPingkou(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MobileZhuHuanPingkouChoices(ProgramSkillFrame f)
    {
        var r = f.MobileZhuHuanPingkou!;
        var choices = new List<PromptChoice>();
        if (r.Sealed)
            return[];
        if (r.Targets.Count < r.Maximum)
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && !r.Targets.Contains(p.Seat)).Select(p => new PromptChoice(new($"mobile-pingkou.{(f.Id)}.target-{(p.Seat)}"), "选择 " + p.Name, [], [p.Seat], new Dictionary<string, string> { ["program-action"] = MobileZhuHuanPingkouAction, ["branch"] = "target" })));
        choices.Add(new(new($"mobile-pingkou.{(f.Id)}.finish"), $"依次造成伤害（已选{(r.Targets.Count)}/{(r.Maximum)}）", [], [], new Dictionary<string, string> { ["program-action"] = MobileZhuHuanPingkouAction, ["branch"] = "finish" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMobileZhuHuanPingkou(ProgramSkillFrame f)
    {
        var choices = MobileZhuHuanPingkouChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"平寇：选择至多{(f.MobileZhuHuanPingkou!.Maximum)}名其他角色。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(MobileZhuHuanPingkouAction)]
    internal void AnswerMobileZhuHuanPingkou(PromptChoice chosen)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileZhuHuanPingkou: { Sealed: false } r } f || !ValidMobileZhuHuanPingkou(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([chosen], MobileZhuHuanPingkouChoices(f).Where(c => c.Id == chosen.Id).ToArray()))
            throw new InvalidOperationException("Mobile Pingkou requires its exact original owner and a current distinct living target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (chosen.Parameters["branch"] == "target")
        {
            var target = chosen.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r with { Targets = r.Targets.Append(target).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouTargetEvent(f.Id, target));
            PublishMobileZhuHuanPingkou(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r with { Sealed = true, Targets = r.Targets.OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, seat)).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouSealedEvent(f.Id, r.Targets.Count));
        IssueMobileZhuHuanPingkouDamage(f);
    }

    internal void IssueMobileZhuHuanPingkouDamage(ProgramSkillFrame f)
    {
        var r = f.MobileZhuHuanPingkou!;
        while (r.Index < r.Targets.Count && !_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Index]].IsAlive)
            r = r with
            {
                Index = r.Index + 1
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r });
        if (r.Index >= r.Targets.Count)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouCompletedEvent(f.Id));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileZhuHuanPingkou = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouDamageIssuedEvent(f.Id, r.Index, r.Targets[r.Index]));
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Targets[r.Index], 1);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 72500)]
    internal bool ResumeMobileZhuHuanPingkou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileZhuHuanPingkou: { } r } f || f.Id != id)
            return false;
        if (f.AttackAttempt is not null)
            return true;
        if (!r.Sealed)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishMobileZhuHuanPingkou(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MobileZhuHuanPingkou!;
        if (r.Index >= r.Targets.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileZhuHuanPingkouDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Index && e.Target == r.Targets[r.Index]) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileZhuHuanPingkouDamageReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Index))
            throw new InvalidOperationException("Mobile Pingkou accepts only its original native damage return once.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouDamageReturnedEvent(f.Id, r.Index, r.Targets[r.Index]));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r with { Index = r.Index + 1 } });
        IssueMobileZhuHuanPingkouDamage(f);
        return true;
    }

    internal bool ValidMobileZhuHuanPingkou(ProgramSkillFrame f) => f.MobileZhuHuanPingkou is { } r && f.SkillId == MobileZhuHuanPingkouSkill && f.InstructionIndex == 1 && r.Source == MobileZhuHuanPingkouSource(f) && r.GameplayHash == f.GameplayHash && r.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.EndedFrame == f.WindowContext?.ParentFrameId && f.WindowContext?.Window == SkillProgramTriggerWindow.AfterTurnEnded && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().Any(p => p.Id == r.EndedFrame && p.OwnerSeat == f.OwnerSeat && p.TurnNumber == r.ActualTurn) && r.Maximum > 0 && r.Maximum == _characterZhuHuanBoundaryMobileOperations.BoundaryPingkouMaximum(f.OwnerSeat) && r.Targets.Count <= r.Maximum && r.Targets.Distinct().Count() == r.Targets.Count && r.Targets.All(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && seat != f.OwnerSeat) && r.Index >= 0 && r.Index <= r.Targets.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileZhuHuanPingkouStartedEvent>().Count(e => e == new MobileZhuHuanPingkouStartedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, r.ActualTurn, r.EndedFrame, r.Maximum)) == 1 && r.Targets.OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, seat)).SequenceEqual(_coreProgramEventHistoryOperations.ProgramEventHistory<MobileZhuHuanPingkouTargetEvent>().Where(e => e.FrameId == f.Id).Select(e => e.Target).OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, seat))) && (!r.Sealed || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileZhuHuanPingkouSealedEvent>().Count(e => e.FrameId == f.Id && e.Count == r.Targets.Count) == 1);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 72500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 72500)]
    internal bool KeepMobileZhuHuanPingkouIssued(ProgramSkillFrame f) => f.MobileZhuHuanPingkou is { Sealed: true } && ValidMobileZhuHuanPingkou(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileZhuHuanPingkou)]
    internal PromptChoice MobileZhuHuanPingkouAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Targets is [var target] && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target])) ?? p.Choices.Single(c => c.Parameters["branch"] == "finish");
}
