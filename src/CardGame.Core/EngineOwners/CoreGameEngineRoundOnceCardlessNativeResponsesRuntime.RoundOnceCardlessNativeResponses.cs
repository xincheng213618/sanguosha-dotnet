#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeResponsesRuntime
{
    internal bool IsRoundOnceCardlessResponseCardUse(CharacterState actor, CardKind kind)
    {
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || kind != CardKind.Dodge || _coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(actor.Seat) || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return false;
        return _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is null && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, kind) && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } slash && slash.TargetSeat == actor.Seat && slash.EffectiveCardKind is { } incoming && CoreNativeCardClassificationsRuntime.IsSlashCard(incoming);
    }

    internal IReadOnlyList<PromptChoice> RoundOnceCardlessResponseChoices(CharacterState actor, CardKind kind)
    {
        if (!IsRoundOnceCardlessResponseCardUse(actor, kind))
            return[];
        var originalId = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack!.ResolutionId;
        return Array.AsReadOnly(_coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(actor, kind, true).Select(entry =>
        {
            var source = entry.Source;
            var parameters = new Dictionary<string, string>
            {
                ["response"] = "round-once-cardless-basic",
                ["output-kind"] = kind.ToString(),
                ["original-frame-id"] = originalId.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
            return new PromptChoice(new ChoiceId($"round-cardless.{(originalId)}.{(kind)}.{(source.SkillId)}.{(source.BindingId)}.{(source.SkillInstanceId)}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，视为使用【{(CardCatalog.Get(kind).DisplayName)}】。", [], [], parameters);
        }).ToArray());
    }

    internal bool TryResolveRoundOnceCardlessResponse(PromptChoice choice, bool advance)
    {
        if (choice.Parameters.GetValueOrDefault("response") != "round-once-cardless-basic")
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || !prompt.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || choice.Cards.Count != 0 || choice.Targets.Count != 0 || !Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("output-kind"), out var kind) || kind != CardKind.Dodge || prompt.Kind != DecisionKind.RespondDodge)
            throw new InvalidOperationException("Cardless response changed its current native published producer or zero-material tuple.");
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice);
        if (!RoundOnceCardlessResponseChoices(actor, kind).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Cardless response lost its exact current native use qualification or unpaid shared round allowance.");
        _coreRoundOnceCardlessBasicOperations.ConsumeRoundOnceCardlessBasic(source, true);
        ResolveRoundOnceCardlessDodge(actor, source);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advance)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        return true;
    }

    internal ResolutionFrame RoundOnceCardlessResponseOwner(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id && (f is CardUseFrame or ProgramSkillFrame)) ?? throw new InvalidOperationException("Cardless response has no actual typed incoming Slash owner.");
    internal RoundOnceCardlessBasicUseReceipt StoreRoundOnceCardlessResponse(ResolutionFrame owner, CardActionContext action, CardConversionSource source)
    {
        if (RoundOnceCardlessResponseReceipt(owner)is not null)
            throw new InvalidOperationException("A native response owner already holds an unresolved cardless basic receipt.");
        var receipt = _coreRoundOnceCardlessBasicOperations.IssueRoundOnceCardlessBasicUse(owner, action, source);
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner switch
        {
            CardUseFrame use => use with { RoundOnceCardlessBasicResponse = receipt },
            ProgramSkillFrame program => program with { RoundOnceCardlessBasicResponse = receipt },
            _ => throw new InvalidOperationException("Unexpected cardless response owner.")});
        return receipt;
    }

    internal void ResolveRoundOnceCardlessDodge(CharacterState actor, CardConversionSource source)
    {
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("Cardless Dodge lost its original native Slash.");
        var owner = RoundOnceCardlessResponseOwner(attack.ResolutionId);
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeResponseWindowsOperations.SetResponseParentStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, (owner as CardUseFrame)?.Action?.ActionId, CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, attack.SourceSeat, CardKind.Dodge, [], [], [source], effectiveSuit: Suit.None, effectiveRank: 0));
        StoreRoundOnceCardlessResponse(RoundOnceCardlessResponseOwner(owner.Id), action, source);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Dodge);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(0, actor.Seat, attack.SourceSeat, CardKind.Dodge));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        _coreFriendEventsOperations.CaptureFriendMinimumHandUse(action);
        _coreOffTurnUsedCardsOperations.CaptureOffTurnResponseUseOrigin(attack.ResolutionId, action);
        if (_coreCommittedResponseUsesOperations.Value.TryBeginCommittedResponseUsePrograms(attack, action, ProgramCardContinuation.Dodge))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardResponseAccepted, [attack.SourceSeat], ProgramCardContinuation.Dodge))
            return;
        if (_coreResponseUseCompletionOperations.HasResponseUseCompletionObserver(action, ProgramCardContinuation.Dodge) || _coreResponseCompletionOperations.HasCardResponseCompletedObserver(action, ProgramCardContinuation.Dodge))
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, action, ProgramCardContinuation.Dodge);
        else
            _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
    }

    internal static RoundOnceCardlessBasicUseReceipt? RoundOnceCardlessResponseReceipt(ResolutionFrame owner) => RuntimeSkills.RuntimeRules_Core_RoundOnceCardlessNativeResponses.RoundOnceCardlessResponseReceipt(owner);
    internal bool IsIssuedRoundOnceCardlessResponse(ResolutionFrame owner, CardActionContext action)
    {
        if (!_coreRoundOnceCardlessBasicOperations.IsIssuedRoundOnceCardlessBasicUse(RoundOnceCardlessResponseReceipt(owner), owner, action) || action.Type != CardActionType.Response || action.TargetSeats.Count != 0 || action.ActorSeat != action.ResponderSeat)
            return false;
        var parentAction = (owner as CardUseFrame)?.Action?.ActionId;
        if (action.ParentActionId != parentAction)
            return false;
        var attack = owner switch
        {
            CardUseFrame use => use.CardAttack,
            ProgramSkillFrame program => program.CardAttack,
            _ => null
        };
        if (attack is null)
            return false;
        return action.EffectiveKind == CardKind.Dodge && attack.EffectiveCardKind is { } incoming && CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) && attack.TargetSeat == action.ActorSeat && attack.SourceSeat == action.OpponentSeat;
    }

    internal CardActionContext RequireRoundOnceCardlessResponseAction(ResolutionFrame owner)
    {
        var receipt = RoundOnceCardlessResponseReceipt(owner) ?? throw new InvalidOperationException("Cardless response lost its actual paid receipt.");
        var accepted = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == receipt.CardActionId).ToArray();
        if (accepted is not [var fact] || !IsIssuedRoundOnceCardlessResponse(owner, fact.Action))
            throw new InvalidOperationException("Cardless response lost its once accepted native action, original typed producer or shared quota payment.");
        return fact.Action;
    }

    internal bool IsRoundOnceCardlessResponseUse(CardActionContext action, ProgramCardContinuation continuation)
    {
        if (continuation != ProgramCardContinuation.Dodge || action.Type != CardActionType.Response || action.PhysicalCards.Count != 0 || action.ConversionChain is not [var source] || _corePlayCardHistoryOperations.ViewAsRule(source)?.RoundOnceCardlessBasic is null)
            return false;
        var owner = _resolutionStackOperations._resolutionStack.FirstOrDefault(f => RoundOnceCardlessResponseReceipt(f)?.CardActionId == action.ActionId);
        return owner is not null && IsIssuedRoundOnceCardlessResponse(owner, action) && action.EffectiveKind == CardKind.Dodge;
    }

    internal void ClearRoundOnceCardlessResponse(ResolutionFrame owner)
    {
        if (RoundOnceCardlessResponseReceipt(owner)is null)
            return;
        var action = RequireRoundOnceCardlessResponseAction(owner);
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Action.ActionId == action.ActionId))
            throw new InvalidOperationException("Cardless response cannot clear before its accepted native children return.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner switch
        {
            CardUseFrame use => use with { RoundOnceCardlessBasicResponse = null },
            ProgramSkillFrame program => program with { RoundOnceCardlessBasicResponse = null },
            _ => throw new InvalidOperationException("Unexpected cardless response owner.")});
    }

    internal void ClearRoundOnceCardlessDodgeResponse(NativeOwners.CardAttackHandle attack)
    {
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == attack.ResolutionId)is { } owner && RoundOnceCardlessResponseReceipt(owner)is not null)
        {
            var action = RequireRoundOnceCardlessResponseAction(owner);
            if (action.EffectiveKind != CardKind.Dodge || action.ActorSeat != attack.TargetSeat || action.OpponentSeat != attack.SourceSeat)
                throw new InvalidOperationException("Cardless Dodge changed its original successful native attack.");
            ClearRoundOnceCardlessResponse(owner);
        }
    }
}
