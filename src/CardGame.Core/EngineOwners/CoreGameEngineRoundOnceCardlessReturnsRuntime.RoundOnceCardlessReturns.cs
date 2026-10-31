#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundOnceCardlessReturnsRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 120)]
    internal void ObserveRoundOnceCardlessRequestedOwnerDamage(DamageAppliedEvent damage)
    {
        if (damage.Amount <= 0 || damage.SourceLess || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { RoundOnceCardlessProgramRequest: { } receipt } use || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == receipt.ProgramFrameId)is not ProgramSkillFrame parent || receipt.RequestOp != SkillProgramEffectOp.RequestSlashByTarget || damage.TargetSeat != parent.OwnerSeat || damage.SourceSeat != attack.SourceSeat || damage.TargetSeat != attack.TargetSeat || !_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramRequestRidesOn(use, parent))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundOnceCardlessRequestedOwnerDamagedEvent(parent.Id, use.Id, receipt.NativeActionId, damage.SourceSeat, damage.TargetSeat, damage.Amount));
    }

    internal bool RoundOnceCardlessRequestedOwnerWasDamaged(ProgramSkillFrame parent, GameEngine.AttackCompletionReceipt completion)
    {
        if (parent.RoundOnceCardlessProgramRequest is not { NativeReturned: true, Receipt: { } receipt } || receipt.NativeUseId != completion.ResolutionId || receipt.RequestOp != SkillProgramEffectOp.RequestSlashByTarget || !_coreRoundOnceCardlessProgramRequestsOperations.CanContinueIssuedRoundOnceCardlessProgramRequest(parent))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessRequestedOwnerDamagedEvent>().Any(fact => fact.ProgramFrameId == parent.Id && fact.NativeUseId == receipt.NativeUseId && fact.NativeActionId == receipt.NativeActionId && fact.TargetSeat == parent.OwnerSeat && fact.Amount > 0);
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 100)]
    internal bool FinishRoundOnceCardlessAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !_coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(attack.ResolutionId) || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
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

    internal bool TryResolveRoundOnceCardlessAiDodge()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondDodge } prompt || _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat].IsHuman)
            return false;
        var choice = _coreRoundOnceCardlessNativeResponsesOperations.RoundOnceCardlessResponseChoices(_coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat], CardKind.Dodge).FirstOrDefault();
        if (choice is null || !prompt.Choices.Any(candidate => candidate.Id == choice.Id) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack)
            return false;
        var(use, _, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[prompt.PlayerSeat].ChooseDodgeResponse(_corePlayerViewProjectionOperations.CreateSnapshot(prompt.PlayerSeat), attack.SourceSeat, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence, attack.IgnoresArmor, true);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        if (!use)
            return false;
        return _coreRoundOnceCardlessNativeResponsesOperations.TryResolveRoundOnceCardlessResponse(choice, false);
    }
}
