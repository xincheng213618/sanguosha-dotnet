#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineZhuijiProgramsRuntime
{
    internal SkillProgramStepOutcome ArmZhuijiActualPlay(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } actor } || actor != f.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != f.OwnerSeat || f.ZhuijiInitialPaid is not null)
            throw new InvalidOperationException("Zhuiji needs its unpaid actual own Play-start.");
        var option = f.ChoiceBindings.Single(b => b.Name == "zhuiji-choice").OptionId;
        if (option is not ("recover" or "draw"))
            throw new InvalidOperationException("Zhuiji lost its actual native choice.");
        var ending = _contentRegistryOperations._contentRegistry!.GetSkill(f.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.SettleZhuijiActualPlayDebt }]);
        var grant = new ZhuijiPlayGrant(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, option == "recover", ending.Id);
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ZhuijiPlayDebts.Add(grant);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhuijiInitialPaid = grant });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuijiPlayGrantIssuedEvent(grant));
        return SkillProgramStepOutcome.Continue;
    }

    internal IEnumerable<ZhuijiPlayGrant> UnsettledZhuijiGrants()
    {
        return _coreMatchParticipantsPlayersAccess._players.SelectMany(p => p.ZhuijiPlayDebts);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 590)]
    internal void ObserveZhuijiDebtTermination(IGameEvent payload)
    {
        var ended = payload as TurnEndedEvent;
        var died = payload as PlayerDiedEvent;
        if (ended is null && died is null)
            return;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.ZhuijiPlayDebts.Count > 0))
            foreach (var grant in owner.ZhuijiPlayDebts.Where(g => died?.VictimSeat == owner.Seat || ended is not null && ended.ActorSeat == owner.Seat && ended.TurnNumber == g.ActualTurnNumber).ToArray())
            {
                owner.ZhuijiPlayDebts.Remove(grant);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuijiPlayDebtConsumedEvent(grant.ProgramFrameId, grant, false));
            }
    }

    internal IEnumerable<ProgramTriggerCandidate> IssuedZhuijiEndingCandidates(CharacterState owner) => UnsettledZhuijiGrants().Where(g => g.Source.OwnerSeat == owner.Seat && g.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && g.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId).Select(g => new ProgramTriggerCandidate(owner.Seat, g.Source.SkillId, g.EndingBindingId, g.Source.SkillInstanceId, g.GameplayHash, 0));
    internal ZhuijiPlayGrant? MatchingZhuijiGrant(ProgramTriggerCandidate c) => UnsettledZhuijiGrants().SingleOrDefault(g => g.Source.OwnerSeat == c.OwnerSeat && g.Source.SkillId == c.SkillId && g.Source.SkillInstanceId == c.SkillInstanceId && g.GameplayHash == c.GameplayHash && g.EndingBindingId == c.BindingId && g.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && g.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    internal ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(CharacterState owner) => owner.ZhuijiPlayDebts.Any(g => g.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && g.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) ? new(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, owner.Seat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    internal ZhuijiPlayGrant? MatchingZhuijiGrant(ProgramTriggerCandidate c, ZhuijiPlayPhaseKey phase) => UnsettledZhuijiGrants().SingleOrDefault(g => g.Source.OwnerSeat == c.OwnerSeat && g.Source.SkillId == c.SkillId && g.Source.SkillInstanceId == c.SkillInstanceId && g.GameplayHash == c.GameplayHash && g.EndingBindingId == c.BindingId && g.ActualTurnNumber == phase.ActualTurnNumber && g.Source.OwnerSeat == phase.OwnerSeat && g.PhaseInstanceId == phase.PhaseInstanceId);
    internal ProgramSkillWindowContext CreateZhuijiEndingContext(ProgramLifecycleTriggerWindowFrame f, ProgramTriggerCandidate c, ProgramSkillWindowContext context) => f.ZhuijiEndingPhase is { } phase ? context with
    {
        ZhuijiGrant = MatchingZhuijiGrant(c, phase)
    }

    : context;
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 70)]
    internal bool HasIssuedZhuijiDebtCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.PlayEnding && w.SourceSeat == c.OwnerSeat && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && w.ZhuijiGrant is { } g && g.Source.SkillId == c.SkillId && g.Source.SkillInstanceId == c.SkillInstanceId && g.EndingBindingId == c.BindingId && g.GameplayHash == c.GameplayHash && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].ZhuijiPlayDebts.Contains(g);
    internal SkillProgramStepOutcome SettleZhuijiActualPlayDebt(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var grant = f.WindowContext?.ZhuijiGrant ?? MatchingZhuijiGrant(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0));
        if (grant is null)
            return SkillProgramStepOutcome.Continue;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuijiPlayDebtConsumedEvent(f.Id, grant, false));
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhuijiDebt = new(grant, f.InstructionIndex, grant.RecoverThenDiscard ? ZhuijiDebtStage.ChoosingDiscard : ZhuijiDebtStage.HpChildren, 0, 0, 0), PendingMovementContinuation = grant.RecoverThenDiscard ? null : new(f.OwnerSeat, 0, null) });
        if (grant.RecoverThenDiscard)
        {
            PublishZhuijiDiscardPrompt(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuijiPlayDebtConsumedEvent(f.Id, grant, true));
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ZhuijiPlayDebts.Remove(grant);
        var outcome = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, 1);
        if (outcome == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PublishZhuijiDiscardPrompt(ProgramSkillFrame f)
    {
        var available = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Where(c => !c.IsGeneralWeapon).OrderBy(c => c.Id).ToArray();
        var count = Math.Min(2, available.Length);
        var choices = new List<PromptChoice>();
        var sets = count < 2 ? new[]
        {
            available.Select(c => c.Id).ToArray()
        }

        : available.SelectMany((a, i) => available.Skip(i + 1).Select(b => new[] { a.Id, b.Id })).ToArray();
        foreach (var ids in sets)
            choices.Add(new(new ChoiceId($"zhuiji.discard.{(f.Id)}.{(string.Join('-', ids))}"), $"弃置{(ids.Length)}张牌", ids, [], new Dictionary<string, string> { ["program-action"] = "zhuiji-discard", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) }));
        var s = _contentRegistryOperations._contentRegistry!.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "追姬：本次出牌阶段结束，弃置两张牌。", available.Select(c => c.Id).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices.AsReadOnly(),
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("zhuiji-discard")]
    internal void ResolveZhuijiDiscardChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Zhuiji lost its actual ending frame.");
        if (f.ZhuijiDebt is not { Stage: ZhuijiDebtStage.ChoosingDiscard } r || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture))
            throw new InvalidOperationException("Zhuiji discard belongs to a different already-paid stage.");
        var available = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Where(c => !c.IsGeneralWeapon).ToArray();
        if (choice.Cards.Count != Math.Min(2, available.Length) || choice.Cards.Distinct().Count() != choice.Cards.Count || choice.Cards.Any(id => !available.Any(c => c.Id == id)))
            throw new InvalidOperationException("Zhuiji discard lost an actual owned card.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhuijiDebt = r with { Stage = ZhuijiDebtStage.DiscardChildren, Before = before, After = before, ActualCount = choice.Cards.Count }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuijiPlayDebtConsumedEvent(f.Id, r.Grant, true));
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ZhuijiPlayDebts.Remove(r.Grant);
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, choice.Cards, new("program.zhuiji.ending-discard"));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ZhuijiDebt = f.ZhuijiDebt!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuijiDiscardPaidEvent(f.Id, r.Grant, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, choice.Cards.Count));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 22201)]
    internal bool ResumeZhuijiDebt(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhuijiDebt: { } r } f || f.Id != id)
            return false;
        if (r.Stage == ZhuijiDebtStage.ChoosingDiscard)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishZhuijiDiscardPrompt(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhuijiDebt = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    internal bool ValidZhuijiIssued(ProgramSkillFrame f)
    {
        var grant = f.ZhuijiInitialPaid ?? f.ZhuijiDebt?.Grant ?? f.WindowContext?.ZhuijiGrant;
        if (grant is null)
            return false;
        return grant.Source.OwnerSeat == f.OwnerSeat && grant.Source.SkillId == f.SkillId && grant.Source.SkillInstanceId == f.SkillInstanceId && grant.GameplayHash == f.GameplayHash && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhuijiPlayGrantIssuedEvent>().Count(e => e.Grant == grant) == 1 && (f.ZhuijiInitialPaid is not null ? grant.ProgramFrameId == f.Id && grant.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && grant.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId : f.WindowContext?.Window == SkillProgramTriggerWindow.PlayEnding && f.TriggerId == grant.EndingBindingId && (f.ZhuijiDebt is { } debt ? debt.InstructionIndex == f.InstructionIndex : f.InstructionIndex is 0 or 1 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ZhuijiPlayDebts.Contains(grant)));
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 390)]
    internal bool IsZhuijiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ZhuijiDebt is { } r && e?.Op == SkillProgramEffectOp.SettleZhuijiActualPlayDebt && p.SubjectSeat == f.OwnerSeat && p.CoverageResultBind is null && ValidZhuijiIssued(f) && (r.Stage == ZhuijiDebtStage.HpChildren ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSkillHpLostEvent>().Count(x => x.FrameId == f.Id && x.SkillId == f.SkillId && x.TargetSeat == f.OwnerSeat && x.Amount == 1) == 1 : r.Stage == ZhuijiDebtStage.DiscardChildren && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhuijiDiscardPaidEvent>().Any(x => x.ProgramFrameId == f.Id && x.Grant == r.Grant && x.Before == r.Before && x.After == r.After && x.ActualCount == r.ActualCount) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From.OwnerSeat == f.OwnerSeat && m.To == CardLocation.DiscardPile && m.Reason.Value == "program.zhuiji.ending-discard") == r.ActualCount);
}
