#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiWumiaoRuntime
{
    internal const string WuZhangFeiZisheng = "wumiao:zisheng";
    internal const string WuZhangFeiSampleReason = "skill-program.wu-zhang-fei.zisheng.obtain";
    internal bool HasWuZhangFeiUseCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuZhangFeiUseSample);

    internal static CardConversionSource WuZhangFeiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_zhang_fei_wumiao_WuZhangFeiUse.WuZhangFeiSource(f);
    internal CardActionContext CaptureWuZhangFeiUseRankAction(CardActionContext action)
    {
        if (!HasWuZhangFeiUseCapability)
            return action;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseRankCapturedEvent>().Any(e => e.ActionId == action.ActionId))
            return action;
        var rank = action.ConversionChain.Any(_characterGuanHaiOlOperations.IsXieweiSource) ? 0 : action.PhysicalCards.Count == 1 ? action.EffectiveRank ?? _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[action.ProviderSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(action.PhysicalCards[0].CardId)) : action.PhysicalCards.Sum(cost => _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[action.ProviderSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId)));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiUseRankCapturedEvent(action.ActionId, action.ActorSeat, action.ProviderSeat, action.PhysicalCards.Count, rank));
        return action;
    }

    internal int? CaptureWuZhangFeiResponseRank(CharacterState provider, CardKind kind, IReadOnlyList<CardActionCost> costs, CardConversionSource? source)
    {
        if (!HasWuZhangFeiUseCapability || costs is not [var cost])
            return null;
        var declared = _resolutionStackOperations._resolutionStack.Select(frame => frame.AcceptedDeclarationPayment).SingleOrDefault(paid => paid is { Claimed: true } && paid.ProviderSeat == provider.Seat && paid.Cost == cost && paid.Source == source && paid.DeclaredKind == kind);
        return declared is null ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(provider, _coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId), source).Rank : declared.OriginalRank is >= 0 and <= 13 ? declared.OriginalRank : throw new InvalidOperationException("A declared response lost its original owned-entity rank.");
    }

    internal void PrepareWuZhangFeiResponseRanks(CharacterState provider, CardKind kind, IReadOnlyList<Card> cards, CardConversionSource? source, int? actorSeat = null)
    {
        if (!HasWuZhangFeiUseCapability || cards.Count == 0 || source is not null && _characterGuanHaiOlOperations.IsXieweiSource(source) || actorSeat is { } actor && actor != provider.Seat || kind != CardKind.Nullification && !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(provider, kind))
            return;
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault() ?? throw new InvalidOperationException("A response rank payment requires its native owning frame.");
        if (parent.WuZhangFeiResponseRanks is not null)
            throw new InvalidOperationException("A native response cannot pay another unbound rank invoice.");
        var materials = cards.Select(card =>
        {
            var declared = _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(provider.Seat, card.Id);
            if (declared is not null && (cards.Count != 1 || declared.Source != source || declared.DeclaredKind != kind || declared.Cost.CardKind != card.Kind || declared.OriginalRank is not (>= 0 and <= 13) || declared.ReturnPurpose != (kind == CardKind.Nullification ? CardDeclarationPurpose.Counterspell : CardDeclarationPurpose.Dodge)))
                throw new InvalidOperationException("A declared response rank requires its exact original declaration receipt.");
            return new WuZhangFeiResponseMaterialRank(card.Id, card.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(provider, card), declared?.OriginalRank ?? _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(provider, card));
        }).ToArray();
        var payment = new WuZhangFeiResponseRankPayment(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.Id, provider.Seat, kind, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, materials.Sum(card => card.Rank))
        {
            Materials = materials
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WuZhangFeiResponseRanks = payment });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiResponseRankPreparedEvent(payment.CaptureId, parent.Id, provider.Seat, kind, payment.Before, materials.Length, payment.RankSum));
        for (var i = 0; i < materials.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiResponseMaterialRankEvent(payment.CaptureId, i, materials[i]));
    }

    internal void BindWuZhangFeiResponseRanks(CardActionContext action)
    {
        if (!HasWuZhangFeiUseCapability || action.Type != CardActionType.Response)
            return;
        var matches = _resolutionStackOperations._resolutionStack.Where(frame => frame.WuZhangFeiResponseRanks is { } p && p.ProviderSeat == action.ProviderSeat && p.EffectiveKind == action.EffectiveKind && p.Materials.Select(card => card.CardId).SequenceEqual(action.PhysicalCards.Select(card => card.CardId))).ToArray();
        if (matches.Length == 0)
            return;
        if (matches is not [var parent] || parent.WuZhangFeiResponseRanks is not { } payment || parent.Id != payment.NativeParentId || action.ActorSeat != action.ProviderSeat || action.RequesterSeat is not null || payment.RankSum != payment.Materials.Sum(card => card.Rank) || payment.Materials.Any(card => card.Rank is < 0 or > 13) || !payment.Materials.Zip(action.PhysicalCards).All(pair => pair.First.From == pair.Second.From && pair.First.PrintedKind == pair.Second.CardKind) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiResponseRankPreparedEvent>().Count(e => e == new WuZhangFeiResponseRankPreparedEvent(payment.CaptureId, parent.Id, action.ProviderSeat, action.EffectiveKind, payment.Before, payment.Materials.Count, payment.RankSum)) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiResponseMaterialRankEvent>().Where(e => e.CaptureId == payment.CaptureId).OrderBy(e => e.Index).Select(e => e.Material).SequenceEqual(payment.Materials) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiResponseRankBoundEvent>().Any(e => e.CaptureId == payment.CaptureId))
            throw new InvalidOperationException("Zisheng response ranks must bind once to their exact paid native materials and original owner.");
        if (payment.Materials.Any(card => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence > payment.Before && move.CardId == card.CardId && move.From == card.From && move.CardKind == card.PrintedKind && (move.Reason == CardMoveReasons.Respond || move.Reason == CardMoveReasons.Nullification)) && !IsWuZhangFeiDeclaredResponsePayment(parent, payment, action, card)))
            throw new InvalidOperationException("Zisheng cannot bind an unpaid original response rank. " + System.Text.Json.JsonSerializer.Serialize(new { InvoiceOwner = parent.Id, payment.Before, payment.Materials, Action = action, Declarations = _resolutionStackOperations._resolutionStack.Where(f => f.AcceptedDeclarationPayment is { } paid && action.PhysicalCards.Any(c => c.CardId == paid.Cost.CardId)).Take(4).Select(f => new { Frame = f.Id, Payment = f.AcceptedDeclarationPayment, Targets = (f as NullificationWindowFrame)?.TargetSeats, Entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == f.AcceptedDeclarationPayment!.BianFuRenPaymentSequence), Committed = _coreProgramEventHistoryOperations.ProgramEventHistory<CardDeclarationCommittedEvent>().Where(e => e.DeclarationId == f.AcceptedDeclarationPayment!.DeclarationId).ToArray() }).ToArray() }));
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WuZhangFeiResponseRanks = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiResponseRankBoundEvent(payment.CaptureId, parent.Id, action.ActionId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiUseRankCapturedEvent(action.ActionId, action.ActorSeat, action.ProviderSeat, action.PhysicalCards.Count, payment.RankSum));
    }

    internal bool IsWuZhangFeiDeclaredResponsePayment(ResolutionFrame invoiceOwner, WuZhangFeiResponseRankPayment invoice, CardActionContext action, WuZhangFeiResponseMaterialRank material)
    {
        if (action.PhysicalCards is not [var cost] || cost.CardId != material.CardId || cost.CardKind != material.PrintedKind || cost.From != material.From)
            return false;
        var owners = _resolutionStackOperations._resolutionStack.Where(frame => frame.AcceptedDeclarationPayment is { Claimed: true } paid && paid.ProviderSeat == action.ProviderSeat && paid.Cost == cost && action.ConversionChain.Contains(paid.Source)).ToArray();
        if (owners is not [var owner] || owner.AcceptedDeclarationPayment is not { } declaration || declaration.OwnerFrameId != owner.Id || declaration.OriginalRank != material.Rank || declaration.BianFuRenPaymentSequence is not { } sequence || sequence > invoice.Before || invoiceOwner.Id != owner.Id && !(invoiceOwner is ResponseWindowFrame response && response.ParentFrameId == owner.Id))
            return false;
        var continuation = action.EffectiveKind == CardKind.Nullification ? ProgramCardContinuation.NullificationResponse : ProgramCardContinuation.Dodge;
        if (declaration.ReturnPurpose != (action.EffectiveKind == CardKind.Nullification ? CardDeclarationPurpose.Counterspell : CardDeclarationPurpose.Dodge))
            return false;
        var original = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(move => move.Sequence == sequence);
        return original is not null && _coreResponseCompletionOperations.IsResponseCompletionEntry(action, continuation, owner.Id, cost, original);
    }

    internal bool IsWuZhangFeiTrueUse(CardActionContext action)
    {
        if (action.Type == CardActionType.Use)
            return true;
        if (action.EffectiveKind == CardKind.Nullification)
            return _coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.NullificationResponse);
        if (!_coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.Dodge))
            return false;
        return _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ParentActionId)is { CardAttack: { EffectiveCardKind: { } incoming } attack } && CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) && attack.TargetSeat == action.ActorSeat;
    }

    internal void CaptureWuZhangFeiUseQualification(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!HasWuZhangFeiUseCapability || window != SkillProgramTriggerWindow.CardUseCommitted || !IsWuZhangFeiTrueUse(action) || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive)
            return;
        var rank = _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseRankCapturedEvent>().SingleOrDefault(e => e.ActionId == action.ActionId);
        // Native response-use ranks are captured by their original paid-response
        // constructor. They do not pass through CaptureCardUseAction.
        if (rank is null && action.Type == CardActionType.Response)
        {
            rank = new(action.ActionId, action.ActorSeat, action.ProviderSeat, action.PhysicalCards.Count, action.EffectiveRank ?? 0);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(rank);
        }

        if (rank is null || rank.ActorSeat != action.ActorSeat || rank.ProviderSeat != action.ProviderSeat || rank.MaterialCount != action.PhysicalCards.Count || rank.RankSum <= 3 || rank.RankSum % 3 != 0)
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        foreach (var binding in _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetInstanceTriggers(window).Where(b => b.SkillId == WuZhangFeiZisheng && b.Trigger.Id == "rank-use-sample" && b.Trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveWuZhangFeiUseSample }]))
        {
            var source = new CardConversionSource(binding.SkillId, binding.Trigger.Id, owner.Seat, binding.SkillInstanceId);
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseQualifiedEvent>().Any(e => e.Qualification.Source == source && e.Qualification.ActionId == action.ActionId))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiUseQualifiedEvent(new(action.ActionId, source, binding.Program.GameplayHash, action.ProviderSeat, rank.MaterialCount, rank.RankSum)));
        }
    }

    internal WuZhangFeiUseQualification? WuZhangFeiQualification(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId != WuZhangFeiZisheng || candidate.BindingId != "rank-use-sample" || context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.CardUse is not { } use ? null : _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseQualifiedEvent>().Select(e => e.Qualification).SingleOrDefault(q => q.ActionId == use.CardActionId && q.Source == new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) && q.GameplayHash == candidate.GameplayHash);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29060, SkillProgramEffectOp.ResolveWuZhangFeiUseSample)]
    internal bool CanOfferWuZhangFeiUse(ProgramTriggerLegalityContext gate) => gate.Owner.IsAlive && WuZhangFeiQualification(gate.Candidate, gate.Context)is { } q && !_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseStartedEvent>().Any(e => e.Qualification.Source == q.Source && e.Qualification.ActionId == q.ActionId);
    internal SkillProgramStepOutcome BeginWuZhangFeiUse(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.WuZhangFeiUse is not null || f.TriggerId != "rank-use-sample" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || WuZhangFeiQualification(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context)is not { } q || _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || !WuZhangFeiUseWindowMatches(window, q) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, window) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseStartedEvent>().Any(e => e.Qualification.Source == q.Source && e.Qualification.ActionId == q.ActionId))
            throw new InvalidOperationException("Zisheng needs its exact native true Use, original unbounded rank sum and source-bound commitment.");
        var sample = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => card.Rank == 3).Select(card => card.Id).ToList();
        var beforeRandom = _randomOperations._random.State;
        _randomOperations._random.Shuffle(sample);
        var receipt = new WuZhangFeiUseReceipt(q, window.Id, window.ParentFrameId, WuZhangFeiUseStage.Selecting, beforeRandom, _randomOperations._random.State)
        {
            Sample = sample.Take(3).ToArray(),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhangFeiUse = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiUseStartedEvent(f.Id, q, window.Id, window.ParentFrameId, beforeRandom, _randomOperations._random.State, receipt.Sample.Count));
        for (var i = 0; i < receipt.Sample.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiSampleEntityEvent(f.Id, i, receipt.Sample[i]));
        if (receipt.Sample.Count == 0)
        {
            FinishWuZhangFeiUse(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = WuZhangFeiSampleChoices(f);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "恣胜：获得一张点数为3的牌。", receipt.Sample, [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool WuZhangFeiUseWindowMatches(ProgramCardTriggerWindowFrame window, WuZhangFeiUseQualification q) => CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) == SkillProgramTriggerWindow.CardUseCommitted && window.Action.ActionId == q.ActionId && window.Action.ActorSeat == q.Source.OwnerSeat && window.Action.ProviderSeat == q.ProviderSeat && IsWuZhangFeiTrueUse(window.Action) && window.Action.PhysicalCards.Count == q.MaterialCount && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseRankCapturedEvent>().Count(e => e.ActionId == q.ActionId && e.ActorSeat == q.Source.OwnerSeat && e.ProviderSeat == q.ProviderSeat && e.RankSum == q.RankSum && e.MaterialCount == q.MaterialCount) == 1;

    internal IReadOnlyList<PromptChoice> WuZhangFeiSampleChoices(ProgramSkillFrame f) => f.WuZhangFeiUse!.Sample.Select(id => new PromptChoice(new($"wu-zhang-fei.sample.{(f.Id)}.{(id)}"), $"获得【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】。", [id], [], new Dictionary<string, string> { ["program-action"] = "wu-zhang-fei-sample", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray();
    [ProgramAnswer("wu-zhang-fei-sample")]
    internal void AnswerWuZhangFeiSample(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (f.WuZhangFeiUse is not { Stage: WuZhangFeiUseStage.Selecting } r || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], WuZhangFeiSampleChoices(f).Where(item => item.Id == choice.Id).ToArray()) || choice.Cards is not [var id] || choice.Targets.Count != 0 || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.DrawPile)
            throw new InvalidOperationException("Zisheng sample must be one of its original real private deck entities.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishWuZhangFeiUse(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuZhangFeiUse = r with { Stage = WuZhangFeiUseStage.MovementChildren, ObtainedCardId = id, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(WuZhangFeiSampleReason), movement =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { WuZhangFeiUse = current.WuZhangFeiUse!with { After = movement.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiSampleObtainedEvent(f.Id, id, movement.Sequence));
        });
        ContinueWuZhangFeiOperation(f.Id);
    }

    internal void FinishWuZhangFeiUse(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhangFeiUseCompletedEvent(f.Id, f.WuZhangFeiUse!.Qualification.ActionId, f.WuZhangFeiUse.ObtainedCardId));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveWuZhangFeiUseSample)]
    internal PromptChoice SelectAiWuZhangFeiSample(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices.OrderByDescending(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])).ThenBy(choice => choice.Cards.Single()).First();
    internal bool WuZhangFeiOperationRootMatches(ProgramSkillFrame f)
    {
        if ((f.WuZhangFeiUse is null) == (f.WuZhangFeiXianlue is null) || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        if (f.WuZhangFeiUse is { } sample)
        {
            var q = sample.Qualification;
            if (f.SkillId != WuZhangFeiZisheng || f.TriggerId != "rank-use-sample" || f.ActivationId != f.TriggerId || f.SelectedTargetSeats.Count != 0 || q.Source != WuZhangFeiSource(f) || q.GameplayHash != f.GameplayHash || q.RankSum <= 3 || q.RankSum % 3 != 0 || f.WindowContext is not { } context || context.ParentFrameId != sample.WindowId || _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == sample.WindowId)is not ProgramCardTriggerWindowFrame window || window.ParentFrameId != sample.NativeParentId || !WuZhangFeiUseWindowMatches(window, q) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, window))
                return false;
            var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
            return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == window.Id && sample.Sample.Count <= 3 && sample.Sample.Distinct().Count() == sample.Sample.Count && sample.Sample.All(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank == 3) && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseQualifiedEvent>().Count(e => e.Qualification == q) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiUseStartedEvent>().Count(e => e == new WuZhangFeiUseStartedEvent(f.Id, q, window.Id, window.ParentFrameId, sample.RandomBefore, sample.RandomAfter, sample.Sample.Count)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiSampleEntityEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.CardId).SequenceEqual(sample.Sample);
        }

        if (f.WuZhangFeiXianlue is not { } observed || f.SkillId != WuZhangFeiXianlue || f.TriggerId is not null || f.ActivationId != "observe-ranks" || f.WindowContext is not null || f.SelectedTargetSeats is not [var target] || target != observed.TargetSeat || observed.Source != WuZhangFeiSource(f) || observed.GameplayHash != f.GameplayHash || observed.OriginCycleId < 1 || observed.BeforeMask < 0 || observed.BeforeMask > 8191 || observed.ObservedMask < 0 || observed.ObservedMask > 8191 || observed.EarnedGates < 0 || observed.EarnedGates > 7 || (observed.ReturnedGates & ~observed.EarnedGates) != 0 || observed.Hand.Select(card => card.CardId).Distinct().Count() != observed.Hand.Count || observed.Hand.Any(card => card.CardId <= 0 || _coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId).Kind != card.PrintedKind || card.Rank is < 0 or > 13))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiXianlueStartedEvent>().Count(e => e == new WuZhangFeiXianlueStartedEvent(f.Id, observed.Source, observed.GameplayHash, target, observed.ActualTurn, observed.PhaseInstance, observed.OriginCycleId, observed.BeforeMask, observed.Hand.Count)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhangFeiXianlueObservedEvent>().Count(e => e == new WuZhangFeiXianlueObservedEvent(f.Id, f.OwnerSeat, target, observed.OriginCycleId, observed.BeforeMask, observed.ObservedMask & ~observed.BeforeMask, observed.EarnedGates)) == 1;
    }

    internal bool WuZhangFeiOperationMovement(ProgramSkillFrame f, CardMovementRecord move)
    {
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move))
            return false;
        if (f.WuZhangFeiUse is { Stage: WuZhangFeiUseStage.MovementChildren, ObtainedCardId: { } id } sample)
            return move.Sequence > sample.Before && move.Sequence <= sample.After && move.CardId == id && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(f.OwnerSeat) && move.Reason.Value == WuZhangFeiSampleReason;
        if (f.WuZhangFeiXianlue is not { Stage: WuZhangFeiXianlueStage.DrawChildren, DrawOutcome: { } draw } || move.Sequence <= draw.SequenceBefore || move.Sequence > draw.SequenceAfter)
            return false;
        return draw.ActualDrawIds.Contains(move.CardId) && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(f.OwnerSeat) && move.Reason.Value == WuZhangFeiXianlueDrawReason || draw.ReplacementGainedIds.Contains(move.CardId) && move.To == CardLocation.Hand(f.OwnerSeat) || move.From == CardLocation.DiscardPile && move.To == CardLocation.DrawPile && move.Reason == CardMoveReasons.Reshuffle;
    }
    internal bool AwaitWuZhangFeiChildren(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        return false;
    }

    internal void ContinueWuZhangFeiOperation(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.WuZhangFeiXianlue is not null)
        {
            ContinueWuZhangFeiXianlue(id);
            return;
        }

        if (f.WuZhangFeiUse?.Stage != WuZhangFeiUseStage.MovementChildren || AwaitWuZhangFeiChildren(id))
            return;
        FinishWuZhangFeiUse(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29060)]
    internal bool ResumeWuZhangFeiOperation(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.WuZhangFeiUse is null && f.WuZhangFeiXianlue is null)
            return false;
        ContinueWuZhangFeiOperation(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29060)]
    internal bool ReturnWuZhangFeiMovement(ProgramSkillFrame f)
    {
        if (f.WuZhangFeiUse is null && f.WuZhangFeiXianlue is null || f.PendingMovementContinuation is null)
            return false;
        ContinueWuZhangFeiOperation(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29060)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29060)]
    internal bool PermitPaidWuZhangFei(ProgramSkillFrame f)
    {
        if (f.WuZhangFeiXianlue is null && f.WuZhangFeiUse is not { ObtainedCardId: not null })
            return false;
        return true;
    }

    internal bool WuZhangFeiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if ((f.WuZhangFeiUse is null && f.WuZhangFeiXianlue is null) || !WuZhangFeiOperationRootMatches(f))
            return false;
        if (f.WuZhangFeiXianlue is { Stage: WuZhangFeiXianlueStage.Drawing } && f.NativeDraw is not null)
            return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(move => WuZhangFeiOperationMovement(f, move));
    }

    internal bool WuZhangFeiObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _resolutionStackOperations2._resolutionStack()[child - 1] is ProgramSkillFrame parent && (this.WuZhangFeiFirstChild(parent, _resolutionStackOperations2._resolutionStack()[child]) || parent.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(parent, _resolutionStackOperations2._resolutionStack()[child])) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal ProgramSkillFrame? WuZhangFeiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.WuZhangFeiFirstChild(f, child), (index) => this.WuZhangFeiObserverSuffix(index));
    internal bool IsWuZhangFeiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.WuZhangFeiUse is null && f.WuZhangFeiXianlue is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && WuZhangFeiFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && WuZhangFeiObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29060)]
    internal bool AdvanceWuZhangFeiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuZhangFeiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    SkillProgramStepOutcome IWuZhangFeiUseHost.Sample(ProgramSkillFrame frame) => BeginWuZhangFeiUse(frame);
}
