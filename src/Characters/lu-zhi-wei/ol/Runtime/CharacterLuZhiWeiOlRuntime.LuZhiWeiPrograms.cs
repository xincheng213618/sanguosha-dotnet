#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiWeiOlRuntime
{
    internal const string QingzhongDrawReason = "program.qingzhong.draw", QingzhongSwapReason = "program.qingzhong.swap";
    internal QingzhongPlayPhaseKey? CaptureQingzhongStartingPhase(CharacterState owner) => _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(i => i.Program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawAndArmQingzhongActualPlayDebt))) ? new(_coreActualTurnProgressTurnNumberAccess._turnNumber, owner.Seat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    internal QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(CharacterState owner) => owner.QingzhongDebts.Any(g => g.Phase.ActualTurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && g.Phase.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) ? new(_coreActualTurnProgressTurnNumberAccess._turnNumber, owner.Seat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    internal ProgramSkillWindowContext CreateQingzhongStartingContext(PlayPhaseStartingBoundaryFrame boundary, ProgramSkillWindowContext context) => context with
    {
        QingzhongPhase = boundary.QingzhongStartingPhase
    };
    internal IEnumerable<ProgramTriggerCandidate> IssuedQingzhongEndingCandidates(CharacterState owner) => owner.QingzhongDebts.Where(g => g.Phase.ActualTurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && g.Phase.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId).Select(g => new ProgramTriggerCandidate(owner.Seat, g.Source.SkillId, g.EndingBindingId, g.Source.SkillInstanceId, g.GameplayHash, 0));
    internal ProgramSkillWindowContext CreateQingzhongEndingContext(ProgramLifecycleTriggerWindowFrame f, ProgramTriggerCandidate c, ProgramSkillWindowContext context) => context with
    {
        QingzhongDebt = f.QingzhongEndingPhase is { } phase ? _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].QingzhongDebts.SingleOrDefault(g => g.Phase == phase && g.Source.SkillId == c.SkillId && g.Source.SkillInstanceId == c.SkillInstanceId && g.EndingBindingId == c.BindingId && g.GameplayHash == c.GameplayHash) : null
    };
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 80)]
    internal bool HasIssuedQingzhongDebtCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.PlayEnding && w.SourceSeat == c.OwnerSeat && w.QingzhongDebt is { } g && g.Source.SkillId == c.SkillId && g.Source.SkillInstanceId == c.SkillInstanceId && g.EndingBindingId == c.BindingId && g.GameplayHash == c.GameplayHash && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].QingzhongDebts.Contains(g);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 570)]
    internal void ObserveQingzhongDebtTermination(IGameEvent payload)
    {
        if (payload is not (TurnEndedEvent or PlayerDiedEvent))
            return;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
            foreach (var g in owner.QingzhongDebts.Where(g => payload is PlayerDiedEvent d && d.VictimSeat == owner.Seat || payload is TurnEndedEvent t && t.ActorSeat == owner.Seat && t.TurnNumber == g.Phase.ActualTurnNumber).ToArray())
            {
                owner.ConsumeQingzhongDebt(g);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingzhongDebtConsumedEvent(g.ProgramFrameId, g, false));
            }
    }

    internal SkillProgramStepOutcome DrawAndArmQingzhongActualPlayDebt(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.Qingzhong is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, QingzhongPhase: { } phase } w || phase.OwnerSeat != f.OwnerSeat || w.SourceSeat != f.OwnerSeat || phase.ActualTurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || f.InstructionIndex != 1)
            throw new InvalidOperationException("Qingzhong requires its original unpaid own Play-start boundary.");
        var ending = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.SettleQingzhongActualPlayDebt }]);
        var debt = new QingzhongPlayDebt(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, w.ParentFrameId, phase, ending.Id, f.InstructionIndex);
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ArmQingzhongDebt(debt);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingzhong = new(debt, QingzhongStage.DrawChildren, f.InstructionIndex, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, 0), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingzhongDebtArmedEvent(debt));
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 2, null, null, SkillProgramCardSetVisibility.Private, new(QingzhongDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingzhong = f.Qingzhong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.Reason.Value == QingzhongDrawReason && m.To == CardLocation.Hand(f.OwnerSeat)) } });
        IssueQingzhongMovement(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome SettleQingzhongActualPlayDebt(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var debt = f.WindowContext?.QingzhongDebt ?? throw new InvalidOperationException("Qingzhong ending needs its issued actual phase debt.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingzhong = new(debt, QingzhongStage.ChooseMinimum, f.InstructionIndex, 0, 0, 0) });
        PublishQingzhongMinimum(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int[] QingzhongMinimumSeats()
    {
        var living = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToArray();
        if (living.Length == 0)
            return[];
        var count = living.Min(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count);
        return living.Where(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count == count).Select(p => p.Seat).ToArray();
    }

    internal void PublishQingzhongMinimum(ProgramSkillFrame f)
    {
        var seats = QingzhongMinimumSeats();
        var choices = seats.Select(s => new PromptChoice(new ChoiceId($"qingzhong.{(f.Id)}.{(s)}"), $"与{(_coreMatchParticipantsPlayersAccess._players[s].Name)}交换手牌", [], [s], new Dictionary<string, string> { ["program-action"] = "qingzhong-minimum", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray();
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "清忠：选择当前手牌最少的角色。", [], seats, f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("qingzhong-minimum")]
    internal void ResolveQingzhongMinimumChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Qingzhong lost its owning chooser.");
        RequireIssuedLuZhiWei(f);
        if (f.Qingzhong is not { Stage: QingzhongStage.ChooseMinimum } p || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Targets is not [var target] || !QingzhongMinimumSeats().Contains(target))
            throw new InvalidOperationException("Qingzhong choice is not a current minimum living hand seat.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var ownerIds = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => c.Id).ToArray();
        var targetIds = target == f.OwnerSeat ? [] : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ConsumeQingzhongDebt(p.Debt);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingzhongDebtConsumedEvent(f.Id, p.Debt, true));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingzhong = p with { Stage = QingzhongStage.SwapChildren, TargetSeat = target, Before = before, After = before, ActualCount = target == f.OwnerSeat ? 0 : ownerIds.Length + targetIds.Length, OwnerHandCount = ownerIds.Length, TargetHandCount = targetIds.Length }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (target != f.OwnerSeat)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ownerIds.Concat(targetIds).ToArray(), CardLocation.Hand(target), new(QingzhongSwapReason), destinationForCard: (_, location) => CardLocation.Hand(location.OwnerSeat == f.OwnerSeat ? target : f.OwnerSeat));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingzhong = f.Qingzhong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        IssueQingzhongMovement(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void IssueQingzhongMovement(ProgramSkillFrame f)
    {
        var p = f.Qingzhong!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingzhongMovementIssuedEvent(f.Id, p.Stage, p.Before, p.After, p.ActualCount, p.TargetSeat, p.OwnerHandCount, p.TargetHandCount));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 23400)]
    internal bool ResumeQingzhong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Qingzhong: { } p } f || f.Id != id)
            return false;
        RequireIssuedLuZhiWei(f);
        if (p.Stage == QingzhongStage.ChooseMinimum)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishQingzhong(f);
                return true;
            }

            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishQingzhongMinimum(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        FinishQingzhong(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 23400)]
    internal bool ReturnQingzhongMovement(ProgramSkillFrame f) => f.Qingzhong is not null && ResumeQingzhong(f.Id);
    internal void FinishQingzhong(ProgramSkillFrame f)
    {
        var p = f.Qingzhong!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingzhongCompletedEvent(f.Id, p.Debt, p.Stage, p.TargetSeat, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingzhong = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome global::CardGame.Core.ILuZhiWeiProgramHost.DrawAndArmQingzhongActualPlayDebt(ProgramSkillFrame f) => DrawAndArmQingzhongActualPlayDebt(f);
    SkillProgramStepOutcome global::CardGame.Core.ILuZhiWeiProgramHost.SettleQingzhongActualPlayDebt(ProgramSkillFrame f) => SettleQingzhongActualPlayDebt(f);
}
