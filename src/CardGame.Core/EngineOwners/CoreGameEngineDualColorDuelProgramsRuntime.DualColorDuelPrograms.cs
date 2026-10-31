#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDualColorDuelProgramsRuntime
{
    SkillProgramStepOutcome IDualColorDuelProgramHost.ChoosePrivateColorsDiscardAndDuel(ProgramSkillFrame frame) => BeginDualColorDuel(frame);
    internal bool DualColorSourceCurrent(ProgramSkillFrame frame) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == frame.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId);
    internal bool DualColorDraftMatches(ProgramSkillFrame frame)
    {
        if (frame.DualColorDuel is not { } draft || frame.TriggerId is not null || frame.WindowContext is not null || frame.InstructionIndex != draft.InstructionIndex || draft.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats is not [var other] || other != draft.OtherSeat || other == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(other) || draft.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || string.IsNullOrWhiteSpace(frame.SkillId) || string.IsNullOrWhiteSpace(frame.ActivationId) || string.IsNullOrWhiteSpace(frame.SkillInstanceId) || string.IsNullOrWhiteSpace(frame.GameplayHash))
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        return plan.Instructions is [{ Op: SkillProgramEffectOp.ChoosePrivateColorsDiscardAndDuel }] && plan.Activation is { MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.OtherLiving, UsesPerPhase: 1, UsesPerTurn: null };
    }

    internal SkillProgramStepOutcome BeginDualColorDuel(ProgramSkillFrame input)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (frame.DualColorDuel is not null || frame.SelectedTargetSeats is not [var other])
            throw new InvalidOperationException("Private color Duel lost its single fresh activation target.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DualColorDuel = new(frame.InstructionIndex, other, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, ProgramDualColorDuelStage.ChoosingOwner, null, null, Array.AsReadOnly(Array.Empty<int>()), Array.AsReadOnly(Array.Empty<int>())) });
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (!DualColorDraftMatches(frame))
            throw new InvalidOperationException("Private color Duel requires its exact standalone phase-limited owner.");
        if (!DualColorSourceCurrent(frame) || !_coreMatchParticipantsPlayersAccess._players[other].IsAlive)
        {
            CancelDualColorDuel(frame);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishDualColorChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int DualColorChooser(ProgramSkillFrame frame) => frame.DualColorDuel!.Stage switch
    {
        ProgramDualColorDuelStage.ChoosingOwner => frame.OwnerSeat,
        ProgramDualColorDuelStage.ChoosingOther => frame.DualColorDuel.OtherSeat,
        _ => throw new InvalidOperationException("Private color selection has already been committed.")};
    internal void PublishDualColorChoice(ProgramSkillFrame frame)
    {
        var chooser = DualColorChooser(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var choices = Array.AsReadOnly(new[] { true, false }.Select(red => new PromptChoice(new ChoiceId($"dual-color.frame-{(frame.Id)}.seat-{(chooser)}.{((red ? "red" : "black"))}"), red ? "红色" : "黑色", [], [], new Dictionary<string, string> { ["program-action"] = "dual-color-choice", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["chooser-seat"] = chooser.ToString(CultureInfo.InvariantCulture), ["color"] = red ? "red" : "black" })).ToArray());
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】私密选择要弃置的手牌颜色。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name + " · 选择颜色", skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("dual-color-choice")]
    internal void ResolveDualColorChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Private color selection lost its owning program.");
        if (!DualColorDraftMatches(frame) || frame.DualColorDuel is not { } draft || draft.Stage is not (ProgramDualColorDuelStage.ChoosingOwner or ProgramDualColorDuelStage.ChoosingOther) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true, Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != DualColorChooser(frame) || !prompt.Choices.Any(choice => choice.Id == selected.Id) || selected.Cards.Count != 0 || selected.Targets.Count != 0 || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("chooser-seat") != prompt.PlayerSeat.ToString(CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("color")is not ("red" or "black"))
            throw new InvalidOperationException("Invalid private color commitment.");
        if (!DualColorSourceCurrent(frame) || !_coreMatchParticipantsPlayersAccess._players[draft.OtherSeat].IsAlive)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            CancelDualColorDuel(frame);
            return;
        }

        var red = selected.Parameters["color"] == "red";
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (draft.Stage == ProgramDualColorDuelStage.ChoosingOwner)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DualColorDuel = draft with { OwnerIsRed = red, Stage = ProgramDualColorDuelStage.ChoosingOther } });
            // No public option fact or log exposes the first commitment.
            PublishDualColorChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        var ownerCards = DualColorHandIds(frame.OwnerSeat, draft.OwnerIsRed!.Value);
        var otherCards = DualColorHandIds(draft.OtherSeat, red);
        draft = draft with
        {
            OtherIsRed = red,
            OwnerCardIds = ownerCards,
            OtherCardIds = otherCards,
            Stage = ProgramDualColorDuelStage.PaymentChildren,
            PaymentStartSequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DualColorDuel = draft });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDualColorsCommittedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, draft.OtherSeat, draft.OwnerIsRed.Value, red));
        PayDualColorHands(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DualColorDuel = frame.DualColorDuel!with { PaymentEndSequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDualColorCardsDiscardedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, draft.OtherSeat, ownerCards.Count, otherCards.Count));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal IReadOnlyList<int> DualColorHandIds(int seat, bool red) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Where(card => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card)) == red).Select(card => card.Id).ToArray());
    internal void PayDualColorHands(ProgramSkillFrame frame)
    {
        var draft = frame.DualColorDuel!;
        var ownerFrom = CardLocation.Hand(frame.OwnerSeat);
        var otherFrom = CardLocation.Hand(draft.OtherSeat);
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch([ownerFrom, otherFrom], [CardLocation.DiscardPile]);
        var movements = new List<CardMovementRecord>();
        var committed = false;
        try
        {
            // Both sets were frozen before either hand changes. Move both sets
            // before emitting rules facts, and expose one mixed-owner batch.
            // This producer accepts Hand -> Discard only. Equipment removal,
            // judgment cleanup and discard-phase collection cannot apply here.
            var ownerCards = _cardZonesOperations._cardZones.MoveMany(draft.OwnerCardIds, ownerFrom, CardLocation.DiscardPile);
            var otherCards = _cardZonesOperations._cardZones.MoveMany(draft.OtherCardIds, otherFrom, CardLocation.DiscardPile);
            foreach (var card in ownerCards)
                movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(card, ownerFrom, CardLocation.DiscardPile, CoreGameEngineDualColorDuelProgramsDefinitions.DualColorDiscardReason));
            foreach (var card in otherCards)
                movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(card, otherFrom, CardLocation.DiscardPile, CoreGameEngineDualColorDuelProgramsDefinitions.DualColorDiscardReason));
            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, movements, committed);
        }
    }

    internal bool DualColorPaymentMatches(ProgramSkillFrame frame)
    {
        var draft = frame.DualColorDuel!;
        if (draft.OwnerIsRed is null || draft.OtherIsRed is null || draft.PaymentStartSequence < 0 || draft.PaymentEndSequence < draft.PaymentStartSequence || draft.OwnerCardIds.Distinct().Count() != draft.OwnerCardIds.Count || draft.OtherCardIds.Distinct().Count() != draft.OtherCardIds.Count || draft.OwnerCardIds.Intersect(draft.OtherCardIds).Any())
            return false;
        bool Exact(int seat, IReadOnlyList<int> ids) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > draft.PaymentStartSequence && move.Sequence <= draft.PaymentEndSequence && move.From == CardLocation.Hand(seat) && move.To == CardLocation.DiscardPile && move.Reason == CoreGameEngineDualColorDuelProgramsDefinitions.DualColorDiscardReason).Select(move => move.CardId).Order().SequenceEqual(ids.Order());
        return Exact(frame.OwnerSeat, draft.OwnerCardIds) && Exact(draft.OtherSeat, draft.OtherCardIds);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 660)]
    internal bool ResumeDualColorDuel(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.DualColorDuel is not { } draft)
            return false;
        if (!DualColorDraftMatches(frame))
            throw new InvalidOperationException("Private color Duel lost its exact owning instruction.");
        if (draft.Stage is ProgramDualColorDuelStage.ChoosingOwner or ProgramDualColorDuelStage.ChoosingOther)
            return true;
        if (draft.Stage == ProgramDualColorDuelStage.DuelIssued)
            throw new InvalidOperationException("An issued private color Duel requires its typed card-use or attack return.");
        if (!DualColorPaymentMatches(frame))
            throw new InvalidOperationException("Private color Duel lost its exact once-only hand payment ledger.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return true;
        if (!DualColorSourceCurrent(frame) || !_coreMatchParticipantsPlayersAccess._players[draft.OtherSeat].IsAlive)
        {
            CancelDualColorDuel(frame);
            return true;
        }

        if (draft.OwnerCardIds.Count == draft.OtherCardIds.Count)
        {
            FinishDualColorDuel(frame);
            return true;
        }

        var actor = draft.OwnerCardIds.Count > draft.OtherCardIds.Count ? frame.OwnerSeat : draft.OtherSeat;
        var target = actor == frame.OwnerSeat ? draft.OtherSeat : frame.OwnerSeat;
        if (!_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(target, actor))
        {
            FinishDualColorDuel(frame);
            return true;
        }

        var useId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        var origin = new ProgramDualColorDuelOrigin(frame.Id, draft.InstructionIndex, useId, frame.OwnerSeat, draft.OtherSeat, actor, target, frame.SkillId, frame.ActivationId, frame.SkillInstanceId, frame.GameplayHash, draft.TurnNumber, draft.OwnerCardIds.Count, draft.OtherCardIds.Count, draft.PaymentStartSequence, draft.PaymentEndSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DualColorDuel = draft with { Stage = ProgramDualColorDuelStage.DuelIssued, CardUseFrameId = useId } });
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var actualUseId = _coreNativeCardUseOperations.BeginCardUse(card, actor, [target], CardKind.Duel, physicalCardIds: [], dualColorDuelOrigin: origin);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDualColorDuelIssuedEvent(frame.Id, useId, frame.SkillId, frame.OwnerSeat, actor, target, draft.OwnerCardIds.Count, draft.OtherCardIds.Count));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, actor, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
        return true;
    }

    internal bool IsDualColorDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { DualColorDuelOrigin: not null, SelectedActorDuelOrigin: null, DamageTargetDuelOrigin: null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    internal bool MatchesDualColorDuelParent(ProgramSkillFrame frame, ProgramDualColorDuelOrigin origin) => DualColorDraftMatches(frame) && DualColorPaymentMatches(frame) && frame.DualColorDuel is { Stage: ProgramDualColorDuelStage.DuelIssued, CardUseFrameId: { } id } draft && id == origin.CardUseFrameId && frame.Id == origin.ParentProgramFrameId && frame.InstructionIndex == origin.InstructionIndex && frame.OwnerSeat == origin.OwnerSeat && draft.OtherSeat == origin.OtherSeat && frame.SkillId == origin.SkillId && frame.ActivationId == origin.BindingId && frame.SkillInstanceId == origin.SkillInstanceId && frame.GameplayHash == origin.GameplayHash && draft.TurnNumber == origin.TurnNumber && draft.OwnerCardIds.Count == origin.OwnerDiscardCount && draft.OtherCardIds.Count == origin.OtherDiscardCount && draft.PaymentStartSequence == origin.PaymentStartSequence && draft.PaymentEndSequence == origin.PaymentEndSequence && origin.OwnerDiscardCount != origin.OtherDiscardCount && origin.InitialActorSeat == (origin.OwnerDiscardCount > origin.OtherDiscardCount ? frame.OwnerSeat : draft.OtherSeat) && origin.InitialTargetSeat == (origin.InitialActorSeat == frame.OwnerSeat ? draft.OtherSeat : frame.OwnerSeat);
    [NativeUseReturn(240)]
    internal void ReturnDualColorDuel(CardUseFrame use)
    {
        if (use.DualColorDuelOrigin is not { AttackStarted: false } origin)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || use.Id != origin.CardUseFrameId || !MatchesDualColorDuelParent(frame, origin))
            throw new InvalidOperationException("Private color Duel lost its exact no-attack typed return.");
        FinishDualColorDuel(frame);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 110)]
    internal bool CompleteIssuedDualColorDuelAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ProgramFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != parentId || frame.DualColorDuel is not { Stage: ProgramDualColorDuelStage.DuelIssued })
            return false;
        CompleteDualColorDuelAttackReturn(frame, completion);
        return true;
    }

    internal void CompleteDualColorDuelAttackReturn(ProgramSkillFrame frame, GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.DualColorDuelReturn is not { AttackStarted: true } origin || completion.ProgramFrameId != frame.Id || completion.ResolutionId != origin.CardUseFrameId || !MatchesDualColorDuelParent(frame, origin))
            throw new InvalidOperationException("Private color Duel lost its exact completed attack return.");
        FinishDualColorDuel(frame);
    }

    internal void FinishDualColorDuel(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DualColorDuel = null });
        if (!DualColorSourceCurrent(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)))
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "选色弃牌已结算，技能来源或游戏已结束。");
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void CancelDualColorDuel(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DualColorDuel = null });
        _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "选色结算的参与者或技能来源已失效，未发行的决斗取消。");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChoosePrivateColorsDiscardAndDuel)]
    internal PromptChoice SelectAiDualColorChoice(PendingDecision decision, ProgramSkillFrame frame)
    {
        var chooser = decision.PlayerSeat;
        var other = chooser == frame.OwnerSeat ? frame.DualColorDuel!.OtherSeat : frame.OwnerSeat;
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(chooser);
        var hostility = _coreAiBrainsAiBrainsAccess._aiBrains[chooser].ScoreProgramTarget(snapshot, other, new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false));
        var expectedOtherCount = snapshot.Players.Single(player => player.Seat == other).HandCount / 2d;
        var own = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[chooser]);
        return decision.Choices.OrderByDescending(choice =>
        {
            var red = choice.Parameters["color"] == "red";
            var count = own.Count(card => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[chooser], card)) == red);
            var retainedSlash = own.Count(card => CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) && CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[chooser], card)) != red);
            return -count * 1.4d + (hostility > 0 ? count > expectedOtherCount ? 5d : -2d : 0d) + Math.Min(1, retainedSlash) * 2d;
        }).ThenBy(choice => choice.Parameters["color"], StringComparer.Ordinal).First();
    }
}
