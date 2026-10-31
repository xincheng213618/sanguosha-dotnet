#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFanChouOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 80)]
    internal bool FanChouFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasFanChouCapability;
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 90)]
    internal bool FanChouNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasFanChouCapability;
    internal const string FanChouDiscardReason = RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.FanChouDiscardReason;
    internal const string FanChouDrawReason = "skill-program.xingluan.fallback-draw", FanChouFieldReason = "skill-program.xingluan.field-gain", FanChouDeckReason = "skill-program.xingluan.deck-gain", FanChouGiftReason = "skill-program.xingluan.recipient-gift";
    internal bool HasFanChouCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseXingluanRankSixBenefit);

    internal static CardConversionSource FanChouSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_fan_chou_ol_FanChouPrograms.FanChouSource(f);
    internal bool FanChouSourceCurrent(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    internal bool TryFanChouUse(int owner, ProgramSkillWindowContext context, out FanChouUseProof proof)
    {
        proof = null !;
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber < 1 || owner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseCompleted || window.Action is not { } a || a.ActionId != identity.CardActionId || a.ActorSeat != owner)
            return false;
        if (a.Type == CardActionType.Use)
        {
            if (window.CompletedResponseReturn is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Step: ResolutionFrameStep.Completed, Action: { } native } use || !CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(native, a) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, a) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Any(e => e.ResolutionId == use.Id && e.CardId == use.CardId && e.CardKind == use.CardKind))
                return false;
        }
        else if (window.CompletedResponseReturn is not { IsCommitted: false } ret || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(a, ret.Kind) || !_coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(window) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(e.Action, a)))
            return false;
        proof = new(window.Id, window.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, a, window.CompletedResponseReturn?.Kind);
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 460)]
    internal bool CanOfferFanChou(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => !ProgramInstructionResolver.Default.Features(t).HasOperation(SkillProgramEffectOp.ChooseXingluanRankSixBenefit) || TryFanChouUse(c.OwnerSeat, w, out _);
    internal SkillProgramStepOutcome BeginFanChou(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.FanChou is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w || !TryFanChouUse(f.OwnerSeat, w, out var use) || !FanChouSourceCurrent(f))
            throw new InvalidOperationException("Xingluan lost its unpaid actual completed own-Play use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FanChou = new(FanChouSource(f), f.GameplayHash, f.InstructionIndex, use, FanChouStage.Branch) });
        PublishFanChou(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<Card> FanChouFieldCards(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => new[] { CardLocation.Equipment(p.Seat), CardLocation.Judgment(p.Seat) }).SelectMany(_cardZonesOperations._cardZones.CardsAt).Where(c => !c.IsGeneralWeapon && _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[_cardZonesOperations._cardZones.GetLocation(c.Id).OwnerSeat!.Value], c) == 6 && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Obtain)).ToArray();
    internal PromptChoice FanChouChoice(ProgramSkillFrame f, string step, string branch, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, string? label = null) => new(new($"xingluan.{(f.Id)}.{(step)}.{(branch)}." + string.Join('-', cards ?? []) + "." + string.Join('-', targets ?? [])), label ?? branch, cards ?? [], targets ?? [], new Dictionary<string, string> { { "program-action", "xingluan" }, { "step", step }, { "branch", branch } });
    internal IReadOnlyList<PromptChoice> FanChouChoices(ProgramSkillFrame f)
    {
        var r = f.FanChou!;
        var result = new List<PromptChoice>();
        if (r.Stage == FanChouStage.Branch)
        {
            if (FanChouFieldCards(f.OwnerSeat).Count > 0)
                result.Add(FanChouChoice(f, "branch", "field", label: "获得场上一张点数6的牌"));
            result.Add(FanChouChoice(f, "branch", "deck", label: "选择牌堆中的点数6牌，没有则摸一张"));
            if (_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat))
                result.Add(FanChouChoice(f, "branch", "other", label: "令其他角色弃点数6牌或交牌"));
        }
        else if (r.Stage == FanChouStage.Field)
            result.AddRange(FanChouFieldCards(f.OwnerSeat).Select(c => FanChouChoice(f, "field", "take", [c.Id], label: c.DisplayName)));
        else if (r.Stage == FanChouStage.Deck)
            result.AddRange(r.DeckCandidates.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank == 6).Select(id => FanChouChoice(f, "deck", "take", [id], label: _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)));
        else if (r.Stage == FanChouStage.Other)
            result.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => FanChouChoice(f, "other", "target", targets: [p.Seat], label: p.Name)));
        else if (r.Stage == FanChouStage.OtherPayment && r.Recipient is { } seat && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
        {
            result.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, seat, seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !c.IsGeneralWeapon && _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[seat], c) == 6).Select(c => FanChouChoice(f, "payment", "discard", c.Cards, label: "弃置 " + c.Label)));
            result.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, seat, seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (_, c) => !c.IsGeneralWeapon).Select(c => FanChouChoice(f, "payment", "give", c.Cards, label: "交给兴乱拥有者 " + c.Label)));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishFanChou(ProgramSkillFrame f)
    {
        var r = f.FanChou!;
        var choices = FanChouChoices(f);
        if (choices.Count == 0)
        {
            FinishFanChou(f);
            return;
        }

        var seat = r.Stage == FanChouStage.OtherPayment ? r.Recipient!.Value : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, "选择兴乱的实际结算。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.Recipient,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseXingluanRankSixBenefit)]
    internal PromptChoice FanChouAiChoice(ProgramSkillFrame f) => FanChouChoices(f).First();
    [ProgramAnswer("xingluan")]
    internal void ResolveFanChouChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        RequireFanChou(f);
        var r = f.FanChou!;
        var seat = r.Stage == FanChouStage.OtherPayment ? r.Recipient!.Value : f.OwnerSeat;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != seat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], FanChouChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Xingluan choice changed its actual payer, private candidate or entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!FanChouSourceCurrent(f))
        {
            FinishFanChou(f);
            return;
        }

        var branch = choice.Parameters["branch"];
        if (r.Stage == FanChouStage.Branch)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingluanBranchChosenEvent(f.Id, r.Source, r.GameplayHash, r.Use.ActualTurn, r.Use.WindowId, r.Use.Action.ActionId, branch));
            if (branch == "deck")
            {
                var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Where(c => c.Rank == 6).Take(2).Select(c => c.Id).ToArray();
                r = r with
                {
                    Branch = branch,
                    Stage = FanChouStage.Deck,
                    DeckCandidates = candidates
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FanChou = r });
                if (candidates.Length == 0)
                {
                    PayFanChouDraw(f);
                    return;
                }
            }
            else
            {
                r = r with
                {
                    Branch = branch,
                    Stage = branch == "field" ? FanChouStage.Field : FanChouStage.Other
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FanChou = r });
            }

            PublishFanChou(f);
            return;
        }

        if (r.Stage == FanChouStage.Other)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FanChou = r with { Recipient = choice.Targets.Single(), Stage = FanChouStage.OtherPayment } });
            PublishFanChou(f);
            return;
        }

        var id = choice.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var payment = r.Stage == FanChouStage.OtherPayment;
        var actualBranch = payment ? "other-" + branch : r.Branch!;
        var subject = payment && branch == "discard" ? seat : f.OwnerSeat;
        var to = payment && branch == "discard" ? CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat);
        var reason = actualBranch switch
        {
            "field" => new CardMoveReason(FanChouFieldReason),
            "deck" => new CardMoveReason(FanChouDeckReason),
            "other-give" => new CardMoveReason(FanChouGiftReason),
            _ => new CardMoveReason(FanChouDiscardReason)};
        if ((payment && (from.OwnerSeat != seat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))) || r.Stage == FanChouStage.Field && !FanChouFieldCards(f.OwnerSeat).Any(c => c.Id == id) || r.Stage == FanChouStage.Deck && (!r.DeckCandidates.Contains(id) || from != CardLocation.DrawPile))
            throw new InvalidOperationException("Xingluan cannot substitute an owned zone or private deck entity.");
        r = r with
        {
            Branch = actualBranch,
            Stage = FanChouStage.MovementChildren,
            SelectedId = id,
            SelectedRank = from.OwnerSeat is { } originalOwner ? _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[originalOwner], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)) : _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank,
            From = from,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActualCount = 1
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { FanChou = r, PendingMovementContinuation = new(subject, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var logicalDiscard = payment && branch == "discard" ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, seat, [id], reason, f.Id) : null;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], to, reason, (batchId, moves) =>
        {
            var paid = r with
            {
                After = moves.Single().Sequence,
                PaidBatchId = batchId
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { FanChou = paid });
            IssueFanChouPaid(f.Id, paid, subject);
        });
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void PayFanChouDraw(ProgramSkillFrame f)
    {
        var r = f.FanChou!with
        {
            Stage = FanChouStage.DrawChildren,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActualCount = 0
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { FanChou = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var cards = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(FanChouDrawReason));
        r = r with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActualCount = cards.Count
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { FanChou = r });
        IssueFanChouPaid(f.Id, r, f.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void IssueFanChouPaid(long id, FanChouReceipt r, int subject) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingluanSegmentPaidEvent(id, r.Source, r.GameplayHash, r.Use.WindowId, r.Use.Action.ActionId, r.Branch!, subject, r.Before, r.After, r.ActualCount) { PaidBatchId = r.PaidBatchId });
    internal void FinishFanChou(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FanChou = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24200)]
    internal bool ResumeFanChou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FanChou: { Stage: FanChouStage.MovementChildren or FanChouStage.DrawChildren } } f || f.Id != id)
            return false;
        RequireFanChou(f);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return true;
        FinishFanChou(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 24200)]
    internal bool ReturnFanChouMovement(ProgramSkillFrame f) => f.FanChou is not null && ResumeFanChou(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 240)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 240)]
    internal bool CanContinueFanChou(ProgramSkillFrame f)
    {
        if (f.FanChou is not { Stage: FanChouStage.MovementChildren or FanChouStage.DrawChildren })
            return false;
        RequireFanChou(f);
        return true;
    }
    internal void RequireFanChou(ProgramSkillFrame f)
    {
        if (f.FanChou is not { } r || r.Source != FanChouSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.Use.WindowId != f.WindowContext?.ParentFrameId || r.Use.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.Use.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || r.Use.Action.ActorSeat != f.OwnerSeat || r.DeckCandidates.Count > 2 || r.DeckCandidates.Distinct().Count() != r.DeckCandidates.Count || !Enum.IsDefined(r.Stage) || f.WindowContext is not { } originalContext || !TryFanChouUse(f.OwnerSeat, originalContext, out var originalUse) || originalUse.WindowId != r.Use.WindowId || originalUse.NativeParentId != r.Use.NativeParentId || originalUse.ActualTurn != r.Use.ActualTurn || originalUse.PhaseInstance != r.Use.PhaseInstance || originalUse.ResponseKind != r.Use.ResponseKind || !CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(originalUse.Action, r.Use.Action))
            throw new InvalidOperationException("Xingluan lost its exact source, phase, native action or private candidates.");
        if (r.Stage is not (FanChouStage.MovementChildren or FanChouStage.DrawChildren))
        {
            if (f.PendingMovementContinuation is not null || r.Before != 0 || r.After != 0 || r.PaidBatchId is not null)
                throw new InvalidOperationException("Unpaid Xingluan cannot borrow a movement invoice.");
            return;
        }

        var subject = r.Branch == "other-discard" ? r.Recipient!.Value : f.OwnerSeat;
        if (r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualCount is < 0 or > 1 || f.PendingMovementContinuation is not { SubjectSeat: var actor, BeforeCount: 0, CoverageResultBind: null } || actor != subject || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XingluanSegmentPaidEvent>().Count(e => e == new XingluanSegmentPaidEvent(f.Id, r.Source, r.GameplayHash, r.Use.WindowId, r.Use.Action.ActionId, r.Branch!, subject, r.Before, r.After, r.ActualCount) { PaidBatchId = r.PaidBatchId }) != 1)
            throw new InvalidOperationException("Xingluan cannot resume an unissued or changed payment.");
        if (r.Stage == FanChouStage.DrawChildren)
        {
            if (r.Branch != "deck" || r.DeckCandidates.Count != 0 || r.PaidBatchId is not null || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == FanChouDrawReason) != r.ActualCount)
                throw new InvalidOperationException("Xingluan fallback draw was not actually paid once.");
            return;
        }

        var to = r.Branch == "other-discard" ? CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat);
        var reason = r.Branch switch
        {
            "field" => new CardMoveReason(FanChouFieldReason),
            "deck" => new CardMoveReason(FanChouDeckReason),
            "other-give" => new CardMoveReason(FanChouGiftReason),
            _ => new CardMoveReason(FanChouDiscardReason)};
        if (r.Branch is not ("field" or "deck" or "other-give" or "other-discard") || r.PaidBatchId is not > 0 || r.SelectedId is not { } id || r.From is not { } from || r.ActualCount != 1 || (r.Branch is "field" or "deck" or "other-discard") && r.SelectedRank != 6 || r.After <= r.Before || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == from && m.To == to && m.Reason == reason) != 1 || r.Branch == "field" && (from.Zone is not (CardZoneKind.Equipment or CardZoneKind.Judgment) || _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank != 6) || r.Branch == "deck" && (from != CardLocation.DrawPile || !r.DeckCandidates.Contains(id) || _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank != 6) || r.Branch is "other-discard" or "other-give" && (from.OwnerSeat != r.Recipient || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            throw new InvalidOperationException("Xingluan payment lost its exact real entity and original legal zone.");
    }

    internal IReadOnlyList<CardSnapshot> FanChouPrivateDeckCards(int viewer) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { OwnerSeat: var owner, FanChou: { Stage: FanChouStage.Deck } r } && owner == viewer ? Array.AsReadOnly(r.DeckCandidates.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile).Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray()) : [];
    SkillProgramStepOutcome IFanChouProgramHost.BeginFanChou(ProgramSkillFrame f, SkillProgramEffect e) => BeginFanChou(f, e);
}
