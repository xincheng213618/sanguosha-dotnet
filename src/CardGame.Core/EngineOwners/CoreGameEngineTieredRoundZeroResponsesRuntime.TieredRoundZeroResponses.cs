#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTieredRoundZeroResponsesRuntime
{
    internal IReadOnlyList<PromptChoice> TieredRoundZeroResponseChoices(CharacterState actor, CardKind kind, bool dyingUse = false)
    {
        var dying = _coreDyingCursorRuntimeOperations.Value.ActiveDying;
        if (dyingUse)
        {
            if (dying is null || dying.ResponderSeat != actor.Seat || !_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp > 0 || kind == CardKind.Peach && !_coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(actor.Seat, dying.VictimSeat) || kind == CardKind.Alcohol && (dying.VictimSeat != actor.Seat || _coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, dying.VictimSeat, Suit.None))
                return[];
        }
        else if (kind == CardKind.Dodge && !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, kind))
            return[];
        else if (kind == CardKind.Nullification && (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } n || n.CandidateIndex >= n.CandidateSeats.Count || n.CandidateSeats[n.CandidateIndex] != actor.Seat || n.ChainDepth == 0 && _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(n.SourceSeat, actor.Seat, n.EffectCardKind, n.TargetSeats)))
            return[];
        var choices = new List<PromptChoice>();
        foreach (var(source, _, _)in _coreTieredRoundZeroUsesOperations.TieredRoundZeroRules(actor, kind, responseUse: true, dyingUse: dyingUse))
        {
            var parameters = new Dictionary<string, string>
            {
                ["response"] = "tiered-round-zero-use",
                ["output-kind"] = kind.ToString(),
                ["dying-use"] = dyingUse ? "true" : "false"
            };
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
            choices.Add(new(new ChoiceId($"tiered-zero.{(kind)}.{(source.SkillId)}.{(source.BindingId)}.{(source.SkillInstanceId)}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，视为使用【{(CardCatalog.Get(kind).DisplayName)}】。", [], [], parameters));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal bool HasTieredRoundZeroDyingResponse(CharacterState responder) => TieredRoundZeroResponseChoices(responder, CardKind.Peach, true).Count > 0 || TieredRoundZeroResponseChoices(responder, CardKind.Alcohol, true).Count > 0;
    internal void ResolveTieredRoundZeroResponse(PromptChoice choice, bool advance)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || !prompt.Choices.Any(c => c.Id == choice.Id && CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || choice.Cards.Count != 0 || choice.Targets.Count != 0 || choice.Parameters.GetValueOrDefault("response") != "tiered-round-zero-use" || !Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("output-kind"), out var kind))
            throw new InvalidOperationException("A zero-material response must be the current published exact choice.");
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice);
        var dyingUse = choice.Parameters.GetValueOrDefault("dying-use") == "true";
        if (!TieredRoundZeroResponseChoices(actor, kind, dyingUse).Any(c => c.Id == choice.Id && CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || dyingUse && prompt.Kind != DecisionKind.RescueDying || !dyingUse && prompt.Kind != (kind == CardKind.Nullification ? DecisionKind.Nullification : DecisionKind.RespondDodge))
            throw new InvalidOperationException("A zero-material response lost its use-qualified producer and allowance before acceptance.");
        if (_characterWoLongFengChuOlOperations.TryBeginEquipmentSlotNamedResponse(actor, source, choice, dyingUse))
            return;
        if (dyingUse)
        {
            var dying = _coreDyingCursorRuntimeOperations.Value.ActiveDying!;
            var card = new Card(0, kind, Suit.None, 0);
            var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.FrameId, ResolutionFrameStep.ResolvingEffect);
            var policies = kind == CardKind.Peach && actor.Seat != victim.Seat ? _coreProgramCardPoliciesOperations.CardPolicies(victim, SkillProgramCardPolicyKind.RescueRecoveryBonus, CardKind.Peach).Where(p => string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(actor), p.Policy.FactionId, StringComparison.Ordinal)).ToArray() : [];
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, victim, card, kind == CardKind.Peach ? "桃" : "酒", kind, 1 + _corePlayCardHistoryOperations.ViewAsRule(source)!.RecoveryBonus + policies.Sum(p => p.Policy.Value), recoveryPolicySources: policies.Select(p => (p.Source.SkillId, p.Policy.Id)).ToArray(), conversionSource: source, dyingResponse: new(dying.FrameId, actor.Seat, kind == CardKind.Peach, kind == CardKind.Peach ? 0 : null, kind == CardKind.Alcohol, kind == CardKind.Alcohol ? 0 : null), physicalCards: []);
        }
        else if (kind == CardKind.Nullification)
            ResolveTieredRoundZeroCounterspell(actor, source);
        else
            ResolveTieredRoundZeroDodge(actor, source);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advance)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal TieredRoundConversionUseReceipt IssueTieredRoundResponse(ResolutionFrame owner, CardActionContext action, CardConversionSource source)
    {
        var rule = _corePlayCardHistoryOperations.ViewAsRule(source)!;
        var policy = rule.TieredRoundConversion!;
        var tier = _coreProgramTieredRoundConversionOperations.TieredRoundTier(source.OwnerSeat, policy.StateId);
        if (action.Type != CardActionType.Response || action.PhysicalCards.Count != rule.InputCount || rule.InputCount != (tier == 2 ? 0 : 1) || action.ActorSeat != source.OwnerSeat || action.ProviderSeat != source.OwnerSeat || action.ResponderSeat != source.OwnerSeat || action.RequesterSeat is not null || action.ConversionChain is not [var actual] || actual != source || tier is < 0 or > 2 || action.EffectiveKind != rule.OutputKind)
            throw new InvalidOperationException("A tiered response changed its accepted source/actor/provider/material tuple.");
        var receipt = new TieredRoundConversionUseReceipt(source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, policy.StateId, policy.UsageId, tier, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, action.ActionId, owner.Id, action.ActorSeat, action.EffectiveKind, action.PhysicalCards.Count);
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner switch
        {
            NullificationWindowFrame n => n with { TieredRoundResponseUse = receipt },
            CardUseFrame use => use with { TieredRoundResponseUse = receipt },
            _ => throw new InvalidOperationException("A zero response has no original typed response owner.")});
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TieredRoundConversionUseIssuedEvent(receipt));
        return receipt;
    }

    internal void ResolveTieredRoundZeroCounterspell(CharacterState actor, CardConversionSource source)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow!;
        var policy = _coreUnrespondableCounterspellsOperations.FreezeUnrespondableCounterspellSource(actor, []);
        _coreProgramTieredRoundConversionOperations.ConsumeTieredRoundConversion(source);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        // Policy preparation attaches its response admission to the live frame.
        pending = (NullificationWindowFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == pending.Id);
        pending = _characterChengYuMouOperations.HasMouCyMatchingName(actor.Seat, CardKind.Nullification) || _characterHuangZhongAnniversaryGodOperations.HasAnniversaryHzPendingInvalidation(actor.Seat) ? _coreNamedAmbushInvalidationOperations.PrepareNamedAmbushCounterspell(pending, actor.Seat, 0) : _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { EffectNullified = !pending.EffectNullified, ChainDepth = pending.ChainDepth + 1, CandidateSeats = _coreNativeNullificationOperations.BuildNullificationCandidateSeats(actor.Seat), CandidateIndex = 0, Step = ResolutionFrameStep.AwaitingResponse });
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _coreDelayedNullificationOperations.Value.NullificationParentActionId(pending), CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, pending.SourceSeat, CardKind.Nullification, [], [], [source], effectiveSuit: Suit.None, effectiveRank: 0));
        IssueTieredRoundResponse(pending, action, source);
        if (pending.NamedAmbushResponse is not null)
            _coreNamedAmbushInvalidationOperations.BindNamedAmbushCounterspell(pending.Id, action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(0, actor.Seat, pending.SourceSeat, CardKind.Nullification));
        if (pending.NamedAmbushResponse is null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NullificationRespondedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, actor.Seat, 0, pending.EffectNullified, pending.ChainDepth) { WindowFrameId = pending.Id, TargetSeat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) });
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        if (pending.NamedAmbushResponse is null)
            _characterCaoXiOlOperations.CaptureCaoXiNativeCounterspellOffset(pending, action, invalidated: false);
        pending = (NullificationWindowFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == pending.Id);
        if (_coreUnrespondableCounterspellsOperations.TryBeginPolicyCounterspellPayment(pending, action, policy, []))
            return;
        if (_coreCommittedResponseUsesOperations.Value.TryBeginCommittedResponseUsePrograms(null, action, ProgramCardContinuation.NullificationResponse))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardResponseAccepted, [], ProgramCardContinuation.NullificationResponse))
            return;
        _coreResponseUseCompletionOperations.ContinueNullificationAfterResponseUse(action);
    }

    internal void ResolveTieredRoundZeroDodge(CharacterState actor, CardConversionSource source)
    {
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("A zero Dodge use has no original Slash.");
        if (!_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, CardKind.Dodge) || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null)
            throw new InvalidOperationException("A zero Dodge is a real own Slash-defense use, never a faction or response-only group card.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreProgramTieredRoundConversionOperations.ConsumeTieredRoundConversion(source);
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, use.Action?.ActionId, CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, attack.SourceSeat, CardKind.Dodge, [], [], [source], effectiveSuit: Suit.None, effectiveRank: 0));
        IssueTieredRoundResponse(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!, action, source);
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

    internal void TryIssueTieredRoundResponse(ResolutionFrame owner, CardActionContext action)
    {
        if (action.ConversionChain.SingleOrDefault(s => _corePlayCardHistoryOperations.ViewAsRule(s)?.TieredRoundConversion is not null)is { } source)
            IssueTieredRoundResponse(owner, action, source);
    }

    internal bool IsIssuedTieredRoundZeroResponse(ResolutionFrame owner, CardActionContext action) => IsIssuedTieredRoundResponse(owner, action, true);
    internal bool IsIssuedTieredRoundResponse(ResolutionFrame owner, CardActionContext action, bool zeroOnly = false)
    {
        var r = owner switch
        {
            NullificationWindowFrame n => n.TieredRoundResponseUse,
            CardUseFrame u => u.TieredRoundResponseUse,
            _ => null
        };
        if (r is null || r.OwnerFrameId != owner.Id || r.CardActionId != action.ActionId || r.FrozenTier is < 0 or > 2 || r.MaterialCount != (r.FrozenTier == 2 ? 0 : 1) || zeroOnly && r.FrozenTier != 2 || action.Type != CardActionType.Response || action.PhysicalCards.Count != r.MaterialCount || action.ActorSeat != r.ActorSeat || action.ProviderSeat != r.ActorSeat || action.ResponderSeat != r.ActorSeat || action.RequesterSeat is not null || action.EffectiveKind != r.EffectiveKind || action.ConversionChain is not [var source] || source != r.Source || source.OwnerSeat != r.ActorSeat || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program is not { } definition || definition.GameplayHash != r.GameplayHash || _coreActivityOrdinaryTricksOperations.ActivityRuntimeProgram(definition).ViewAs.SingleOrDefault(v => v.Id == source.BindingId)is not { UseOnly: true, TieredRoundConversion: { } policy } rule || rule.InputCount != r.MaterialCount || policy.StateId != r.StateId || policy.UsageId != r.UsageId || rule.OutputKind != r.EffectiveKind || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(r.ActorSeat, source.SkillId, CoreProgramTieredRoundConversionRuntime.TieredRoundUsage(policy), r.FrozenTier == 0 ? SkillUsageScope.Phase : SkillUsageScope.Round) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TieredRoundConversionUseIssuedEvent>().Count(e => e.Receipt == r) != 1)
            return false;
        return owner switch
        {
            NullificationWindowFrame n => r.EffectiveKind == CardKind.Nullification && _coreDelayedNullificationOperations.Value.HasNullificationEffectOwner(n) && action.ParentActionId == _coreDelayedNullificationOperations.Value.NullificationParentActionId(n) && action.OpponentSeat == n.SourceSeat,
            CardUseFrame use => r.EffectiveKind == CardKind.Dodge && use.CardAttack is { } attack && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && action.ParentActionId == use.Action?.ActionId && action.ActorSeat == attack.TargetSeat && action.OpponentSeat == attack.SourceSeat,
            _ => false
        };
    }

    internal static TieredRoundConversionUseReceipt? TieredRoundResponseReceipt(ResolutionFrame owner) => RuntimeSkills.RuntimeRules_Core_TieredRoundZeroResponses.TieredRoundResponseReceipt(owner);
    internal CardActionContext RequireTieredRoundIssuedResponseAction(ResolutionFrame owner)
    {
        var receipt = TieredRoundResponseReceipt(owner) ?? throw new InvalidOperationException("A tiered response lost its issued receipt.");
        var accepted = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == receipt.CardActionId).ToArray();
        if (accepted is not [var fact] || !IsIssuedTieredRoundResponse(owner, fact.Action))
            throw new InvalidOperationException("A tiered response lost its unique accepted action, typed parent, original source or paid allowance.");
        return fact.Action;
    }

    internal void ClearTieredRoundDodgeResponse(NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { TieredRoundResponseUse: not null } use)
            return;
        var action = RequireTieredRoundIssuedResponseAction(use);
        if (action.EffectiveKind != CardKind.Dodge || action.ActorSeat != attack.TargetSeat || action.OpponentSeat != attack.SourceSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Action.ActionId == action.ActionId))
            throw new InvalidOperationException("A tiered response cannot clear before its original Dodge children return.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { TieredRoundResponseUse = null });
    }

    internal NullificationWindowFrame ClearTieredRoundCounterspellResponse(NullificationWindowFrame input)
    {
        var current = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().Single(f => f.Id == input.Id);
        if (current.TieredRoundResponseUse is null)
            return current;
        var action = RequireTieredRoundIssuedResponseAction(current);
        if (action.EffectiveKind != CardKind.Nullification || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != current.Id || current.CounterspellPayment is not null || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Action.ActionId == action.ActionId))
            throw new InvalidOperationException("A tiered response cannot clear before its original counterspell children return.");
        return _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(current with { TieredRoundResponseUse = null });
    }

    internal bool TryResolveTieredRoundZeroAiCounterspell()
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } n || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.Nullification } prompt)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var choice = TieredRoundZeroResponseChoices(actor, CardKind.Nullification).FirstOrDefault();
        if (choice is null)
            return false;
        var(use, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseTieredZeroCounterspell(_corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat), n.EffectCardKind, n.SourceSeat, n.TargetSeats, n.EffectNullified, n.ChainDepth, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        if (!use)
            return false;
        ResolveTieredRoundZeroResponse(choice, false);
        return true;
    }

    internal bool TryResolveTieredRoundZeroAiDying()
    {
        if (_coreDyingCursorRuntimeOperations.Value.ActiveDying is not { } dying || _coreMatchParticipantsPlayersAccess._players[dying.ResponderSeat].IsHuman)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[dying.ResponderSeat];
        var choices = TieredRoundZeroResponseChoices(actor, CardKind.Peach, true).Concat(TieredRoundZeroResponseChoices(actor, CardKind.Alcohol, true)).ToArray();
        if (choices.Length == 0)
            return false;
        var(use, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseProgramDyingRescue(_corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat), dying.VictimSeat, _coreCardConversionsOperations.ProgramConversionName(CoreGameEngineCardConversionsRuntime.RequireConversionSource(choices[0])), ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        if (!use)
            return false;
        // Use the same actual response prompt contract as the human path. No
        // private fake cards, direct HP writes or fabricated Accepted facts.
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.RescueDying, actor.Seat, "选择已发布的零材料救援。", [], [], dying.VictimSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = dying.VictimSeat,
            Choices = Array.AsReadOnly(choices)
        };
        ResolveTieredRoundZeroResponse(choices[0], false);
        return true;
    }

    internal bool TryResolveTieredRoundZeroAiDodge()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondDodge } prompt || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat].IsHuman)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var choice = TieredRoundZeroResponseChoices(actor, CardKind.Dodge).FirstOrDefault();
        if (choice is null || !prompt.Choices.Any(c => c.Id == choice.Id))
            return false;
        var(use, _, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseDodgeResponse(_corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat), attack.SourceSeat, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence, attack.IgnoresArmor, true);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        if (!use)
            return false;
        ResolveTieredRoundZeroResponse(choice, false);
        return true;
    }
}
