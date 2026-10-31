#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicResponsesRuntime
{
    internal void ResolveDrawFundedDistinctBasicDodge(DrawFundedDistinctBasicPayment payment)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[payment.Source.OwnerSeat];
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("A paid Dodge lost its incoming Slash.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        if (!_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(payment) || payment.Intent != DrawFundedDistinctBasicIntent.OwnSlashDodge || payment.ParentFrameId != use.Id || payment.ParentActionId != use.Action?.ActionId || payment.Cursor != attack.SuccessfulDodgeResponses || actor.Seat != attack.TargetSeat || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null || !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, CardKind.Dodge))
            throw new InvalidOperationException("A paid Dodge changed its original own Slash-defense direction.");
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, use.Action?.ActionId, CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, attack.SourceSeat, CardKind.Dodge, [], [], [payment.Source], effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: false));
        var receipt = new DrawFundedDistinctBasicUseReceipt(payment, use.Id, action.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!with { DrawFundedDistinctBasicResponse = receipt });
        _coreDrawFundedDistinctBasicUsesOperations.EmitDrawFundedDistinctBasicIssued(receipt);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Dodge);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(0, actor.Seat, attack.SourceSeat, CardKind.Dodge));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        if (_coreCommittedResponseUsesOperations.Value.TryBeginCommittedResponseUsePrograms(attack, action, ProgramCardContinuation.Dodge))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardResponseAccepted, [attack.SourceSeat], ProgramCardContinuation.Dodge))
            return;
        if (_coreResponseUseCompletionOperations.HasResponseUseCompletionObserver(action, ProgramCardContinuation.Dodge) || _coreResponseCompletionOperations.HasCardResponseCompletedObserver(action, ProgramCardContinuation.Dodge))
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, action, ProgramCardContinuation.Dodge);
        else
            _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
    }

    internal CardActionContext RequireDrawFundedDistinctBasicResponseAction(CardUseFrame use)
    {
        var receipt = use.DrawFundedDistinctBasicResponse ?? throw new InvalidOperationException("A paid Dodge lost its incoming-use receipt.");
        var p = receipt.Payment;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == receipt.CardActionId).ToArray();
        if (facts is not [var fact] || !_coreDrawFundedDistinctBasicUsesOperations.IsDrawFundedDistinctBasicIssued(receipt) || receipt.OwnerFrameId != use.Id || p.ParentFrameId != use.Id || p.Intent != DrawFundedDistinctBasicIntent.OwnSlashDodge || p.EffectiveKind != CardKind.Dodge || use.CardAttack is not { } attack || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || attack.TargetSeat != p.Source.OwnerSeat || attack.SuccessfulDodgeResponses != p.Cursor || p.TargetSeat != attack.SourceSeat || fact.Action is not { Type: CardActionType.Response, PhysicalCards.Count: 0, EffectiveKind: CardKind.Dodge, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: false } action || action.ParentActionId != use.Action?.ActionId || action.ParentActionId != p.ParentActionId || action.ActorSeat != p.Source.OwnerSeat || action.ProviderSeat != action.ActorSeat || action.ResponderSeat != action.ActorSeat || action.RequesterSeat is not null || action.OpponentSeat != attack.SourceSeat || action.ConversionChain is not [var source] || source != p.Source || action.TargetSeats.Count != 0)
            throw new InvalidOperationException("A paid Dodge changed its unique accepted action, native owner or once-paid original cursor.");
        return fact.Action;
    }

    internal void ClearDrawFundedDistinctBasicDodgeResponse(NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { DrawFundedDistinctBasicResponse: not null } use)
            return;
        var action = RequireDrawFundedDistinctBasicResponseAction(use);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Action.ActionId == action.ActionId) || use.RecoveryPaidContinuation is not null || use.PendingRecoveryAttempts is { Count: > 0 })
            throw new InvalidOperationException("A paid Dodge cannot clear while its response or recovery child is in flight.");
        _coreDrawFundedDistinctBasicReturnsOperations.EmitDrawFundedDistinctBasicReturned(use.DrawFundedDistinctBasicResponse!);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { DrawFundedDistinctBasicResponse = null });
    }

    internal bool TryAdvanceDrawFundedDistinctBasicAi()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat].IsHuman)
            return false;
        var choices = prompt.Choices.Where(CoreGameEngineDrawFundedDistinctBasicOffersRuntime.IsDrawFundedDistinctBasicAnswer).ToArray();
        if (choices.Length == 0)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var use = true;
        if (prompt.Kind == DecisionKind.RespondDodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack)
        {
            var decision = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseDodgeResponse(_corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat), attack.SourceSeat, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence, attack.IgnoresArmor, true);
            use = decision.Item1;
            _coreNativeAiDecisionsOperations.AddThought(decision.Item3);
        }
        else if (prompt.Kind == DecisionKind.RescueDying && _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying)
        {
            var decision = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseProgramDyingRescue(_corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat), dying.VictimSeat, _coreCardConversionsOperations.ProgramConversionName(CoreGameEngineCardConversionsRuntime.RequireConversionSource(choices[0])), ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
            use = decision.UseRescue;
            _coreNativeAiDecisionsOperations.AddThought(decision.Thought);
        }

        if (!use || _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicContext(prompt)is not { } need)
            return false;
        var choice = choices.FirstOrDefault(c => _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicResponseChoices(prompt, need).Any(current => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([current], [c])));
        if (choice is null || !Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("output-kind"), out var kind) || !int.TryParse(choice.Parameters.GetValueOrDefault("native-target-seat"), out var target))
            return false;
        _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasic(actor, CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice), kind, (need.Intent is DrawFundedDistinctBasicIntent.OwnSlashDodge or DrawFundedDistinctBasicIntent.Dying) ? [target] : choice.Targets, prompt, need);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    internal bool TryResolveDrawFundedDistinctBasicAiDodge() => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.RespondDodge && TryAdvanceDrawFundedDistinctBasicAi();
    internal bool TryResolveDrawFundedDistinctBasicAiForcedSlash() => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind is DecisionKind.RespondSlash or DecisionKind.QinglongCrescentBlade && TryAdvanceDrawFundedDistinctBasicAi();
    internal bool TryResolveDrawFundedDistinctBasicAiDying()
    {
        if (_coreDyingCursorRuntimeOperations.Value.ActiveDying is not { } dying || _coreMatchParticipantsPlayersAccess._players[dying.ResponderSeat].IsHuman || !_coreDrawFundedDistinctBasicOffersOperations.HasDrawFundedDistinctBasicDyingResponse(_coreMatchParticipantsPlayersAccess._players[dying.ResponderSeat]))
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return _corePendingDecisionPolicyPipelineOperations._pendingDecision.Kind == DecisionKind.RescueDying && TryAdvanceDrawFundedDistinctBasicAi();
        // Publish the same original need before consulting AI. No fabricated hand card/action is involved.
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RescueDying, dying.ResponderSeat, "选择本次真实濒死救援。", [], [], dying.VictimSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = dying.VictimSeat
        };
        if (TryAdvanceDrawFundedDistinctBasicAi())
            return true;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        return false;
    }
}
