#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangGaiMouMobileRuntime
{
    internal const string MouKurouSkill = "mou-mobile:kurou";
    internal const string MouKurouGiftReason = "skill-program.mou-mobile.kurou.give";
    internal static CardConversionSource MouKurouSource(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_huang_gai_mou_mobile_MouHuangGaiKurou.MouKurouSource(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29050, SkillProgramEffectOp.ResolveMouHuangGaiKurouGift)]
    internal bool CanOfferMouKurouGift(ProgramTriggerLegalityContext gate) => gate.Candidate.SkillId == MouKurouSkill && gate.Candidate.BindingId == "give-before-loss" && gate.Context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && gate.Owner.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && gate.Owner.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(gate.Owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(gate.Owner)).Any() && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != gate.Owner.Seat);
    internal bool ExactMouKurouGiftParent(ProgramSkillFrame frame)
    {
        if (frame.SkillId != MouKurouSkill || frame.TriggerId != "give-before-loss" || frame.ActivationId != frame.TriggerId || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } context || context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != frame.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || _coreProgramLifecycleOperations.GetProgramTrigger(frame).Effects is not [{ Op: SkillProgramEffectOp.ResolveMouHuangGaiKurouGift }] || _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(parent => parent.Id == context.ParentFrameId)is not { } starting || starting.OwnerSeat != frame.OwnerSeat || starting.ItemIndex < 0 || starting.ItemIndex >= starting.Items.Count || starting.Items[starting.ItemIndex].Candidate is not { } candidate || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, candidate))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == starting.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(fact => fact.FrameId == frame.Id && fact.OwnerSeat == frame.OwnerSeat && fact.SkillId == frame.SkillId && fact.BindingId == frame.TriggerId && fact.SkillInstanceId == frame.SkillInstanceId && fact.Window == context.Window) == 1;
    }

    internal SkillProgramStepOutcome BeginMouKurouGift(ProgramSkillFrame input)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (!ExactMouKurouGiftParent(frame) || frame.MouHuangGaiKurou is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangGaiKurouStartedEvent>().Any(fact => fact.FrameId == frame.Id))
            throw new InvalidOperationException("KuRou requires its exact unpaid own Play-start binding.");
        var receipt = new MouHuangGaiKurouReceipt(MouKurouSource(frame), frame.GameplayHash, frame.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, MouHuangGaiKurouStage.Choosing);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MouHuangGaiKurou = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangGaiKurouStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.OriginalParentId, receipt.ActualTurnNumber, receipt.ActualTurnOwnerSeat));
        var choices = MouKurouChoices(frame);
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || choices.Count == 0)
        {
            CompleteMouKurou(frame);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "苦肉：交给一名其他角色一张牌，然后失去体力。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouKurouChoices(ProgramSkillFrame frame)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).ToArray();
        var recipients = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat).ToArray();
        return Array.AsReadOnly(cards.SelectMany(card => recipients.Select(recipient => new PromptChoice(new ChoiceId($"mou-kurou.{(frame.Id)}.{(card.Id)}.{(recipient.Seat)}"), $"将【{(card.DisplayName)}】交给 {(recipient.Name)}。", [card.Id], [recipient.Seat], new Dictionary<string, string> { ["program-action"] = "mou-huang-gai-kurou", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["source-zone"] = _cardZonesOperations._cardZones.GetLocation(card.Id).Zone.ToString() }))).ToArray());
    }

    [ProgramAnswer("mou-huang-gai-kurou")]
    internal void AnswerMouKurouGift(PromptChoice selected)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (!ValidMouKurou(frame) || frame.MouHuangGaiKurou is not { Stage: MouHuangGaiKurouStage.Choosing } || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouKurouChoices(frame).Where(choice => choice.Id == selected.Id).ToArray()) || selected.Cards is not [var cardId] || selected.Targets is not [var recipient])
            throw new InvalidOperationException("KuRou must name its exact currently owned entity and living recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            CompleteMouKurou(frame);
            return;
        }

        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        if (from.OwnerSeat != frame.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || recipient == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            throw new InvalidOperationException("KuRou cannot substitute a foreign entity or a dead recipient.");
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(item => item.Id == cardId);
        var kind = MouKurouGiftKind(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card, from);
        var loss = MouKurouGiftLoss(kind);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MouHuangGaiKurou = frame.MouHuangGaiKurou!with { Stage = MouHuangGaiKurouStage.GiftChildren }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(recipient), new(MouKurouGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, frame.OwnerSeat, recipient, [cardId], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, movement =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var paid = new MouHuangGaiKurouGift(cardId, card.Kind, kind, from, movement.To, recipient, movement.Sequence, loss);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouHuangGaiKurou = current.MouHuangGaiKurou!with { Gift = paid, GiftSequenceAfter = movement.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangGaiKurouGiftPaidEvent(frame.Id, MouKurouSource(frame), frame.GameplayHash, paid));
        }));
        // Native equipment removal can queue recovery and spill real Ox grain.
        // Merge into the current owning frame after all physical removal hooks.
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MouHuangGaiKurou = frame.MouHuangGaiKurou!with { GiftSequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueMouKurou(frame.Id);
    }

    internal bool AwaitMouKurouChildren(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        return false;
    }

    internal void ContinueMouKurou(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (!ValidMouKurou(frame))
            throw new InvalidOperationException("KuRou lost its actual gift, HP-loss payment or original phase owner.");
        var receipt = frame.MouHuangGaiKurou!;
        if (receipt.Stage == MouHuangGaiKurouStage.Choosing)
            return;
        if (AwaitMouKurouChildren(id))
            return;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        receipt = frame.MouHuangGaiKurou!;
        if (receipt.Stage == MouHuangGaiKurouStage.GiftChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CompleteMouKurou(frame);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MouHuangGaiKurou = receipt with { Stage = MouHuangGaiKurouStage.LossRequested } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangGaiKurouLossRequestedEvent(frame.Id, receipt.Source, receipt.Gift!.RequestedHpLoss));
            var result = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, frame.OwnerSeat, receipt.Gift.RequestedHpLoss);
            if (result == SkillProgramStepOutcome.Continue && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == id)
                ContinueMouKurou(id);
            return;
        }

        if (receipt.Stage == MouHuangGaiKurouStage.LossRequested)
        {
            // Native concealed identities prevent this deduction and return via
            // the same program cursor, without creating a fictitious HP loss.
            var appeared = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<IdentityGeneralAppearedEvent>().Where(fact => fact.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && fact.Receipt.ParentFrameId == id && fact.Receipt.OwnerSeat == frame.OwnerSeat && fact.Receipt.RequestedDeduction == receipt.Gift!.RequestedHpLoss && fact.Receipt.OriginalInstructionIndex == frame.InstructionIndex).ToArray();
            if (appeared is not [var identity] || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<IdentityAppearanceCompletedEvent>().Any(fact => fact.AppearanceFrameId == identity.AppearanceFrameId))
                throw new InvalidOperationException("KuRou cannot finish an unreturned native HP deduction.");
            var hp = identity.Receipt.RestoredHp;
            var loss = new MouHuangGaiKurouLoss(0, hp, hp, receipt.Gift!.RequestedHpLoss, 0, true);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MouHuangGaiKurou = receipt with { Stage = MouHuangGaiKurouStage.LossChildren, Loss = loss } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangGaiKurouHpLossPaidEvent(id, receipt.Source, receipt.GameplayHash, loss));
        }

        CompleteMouKurou(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void CaptureMouKurouNativeLoss(HpChangeContext change)
    {
        if (change.Kind != HpChangeKind.Loss || change.Amount <= 0 || change.ParentFrameId is not { } id || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != id || frame.MouHuangGaiKurou is not { Stage: MouHuangGaiKurouStage.LossRequested, Gift: { } gift } receipt)
            return;
        if (!ValidMouKurou(frame) || change.TargetSeat != frame.OwnerSeat || change.SourceSeat is not null || (long)change.HpBefore - change.HpAfter != change.Amount || change.Amount != gift.RequestedHpLoss)
            throw new InvalidOperationException("KuRou HP loss must be its exact once-issued native owner deduction.");
        var loss = new MouHuangGaiKurouLoss(change.Id, change.HpBefore, change.HpAfter, gift.RequestedHpLoss, change.Amount);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MouHuangGaiKurou = receipt with { Stage = MouHuangGaiKurouStage.LossChildren, Loss = loss } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangGaiKurouHpLossPaidEvent(id, receipt.Source, receipt.GameplayHash, loss));
    }

    internal void CompleteMouKurou(ProgramSkillFrame frame)
    {
        var receipt = frame.MouHuangGaiKurou!;
        if (receipt.Stage == MouHuangGaiKurouStage.Complete)
            throw new InvalidOperationException("KuRou cannot finish twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MouHuangGaiKurou = receipt with { Stage = MouHuangGaiKurouStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangGaiKurouCompletedEvent(frame.Id, receipt.Source, receipt.Gift?.MovementSequence, receipt.Loss?.ActualLost ?? 0));
        _coreSkillProgramsOperations.FinishProgramSkill(frame, receipt.Gift is not null);
    }

    internal bool ValidMouKurou(ProgramSkillFrame frame)
    {
        if (frame.MouHuangGaiKurou is not { } receipt || !ExactMouKurouGiftParent(frame) || !Enum.IsDefined(receipt.Stage) || receipt.Source != MouKurouSource(frame) || receipt.GameplayHash != frame.GameplayHash || receipt.OriginalParentId != frame.WindowContext!.ParentFrameId || receipt.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || receipt.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || receipt.ActualTurnOwnerSeat != frame.OwnerSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangGaiKurouStartedEvent>().Where(fact => fact.FrameId == frame.Id).ToArray()is not [var start] || start != new MouHuangGaiKurouStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.OriginalParentId, receipt.ActualTurnNumber, receipt.ActualTurnOwnerSeat))
            return false;
        var gifts = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangGaiKurouGiftPaidEvent>().Where(fact => fact.FrameId == frame.Id).ToArray();
        var requested = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangGaiKurouLossRequestedEvent>().Where(fact => fact.FrameId == frame.Id).ToArray();
        var losses = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangGaiKurouHpLossPaidEvent>().Where(fact => fact.FrameId == frame.Id).ToArray();
        if (receipt.Gift is not { } gift)
            return receipt.Stage is MouHuangGaiKurouStage.Choosing or MouHuangGaiKurouStage.Complete && gifts.Length == 0 && requested.Length == 0 && losses.Length == 0 && receipt.GiftSequenceAfter == 0 && receipt.Loss is null;
        if (gifts is not [var paid] || paid != new MouHuangGaiKurouGiftPaidEvent(frame.Id, receipt.Source, receipt.GameplayHash, gift) || gift.CardId <= 0 || gift.From.OwnerSeat != frame.OwnerSeat || gift.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(gift.RecipientSeat) || gift.RecipientSeat == frame.OwnerSeat || gift.RequestedHpLoss != (gift.EffectiveKind is CardKind.Peach or CardKind.Alcohol ? 2 : 1) || receipt.GiftSequenceAfter < gift.MovementSequence || receipt.GiftSequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence == gift.MovementSequence).ToArray()is not [var movement] || movement.CardId != gift.CardId || movement.CardKind != gift.PrintedKind || movement.From != gift.From || movement.To != gift.To || movement.Reason.Value != MouKurouGiftReason || gift.To != CardLocation.Hand(gift.RecipientSeat) && !(gift.To == CardLocation.OutsideGame && (_coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(movement) || gift.From.Zone == CardZoneKind.Equipment && (gift.PrintedKind == CardKind.GeneralWeapon || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(gift.CardId), gift.From, CardLocation.Hand(gift.RecipientSeat))))))
            return false;
        if (receipt.Stage == MouHuangGaiKurouStage.GiftChildren)
            return requested.Length == 0 && losses.Length == 0 && receipt.Loss is null;
        if (receipt.Stage == MouHuangGaiKurouStage.Complete && requested.Length == 0)
            return receipt.Loss is null && losses.Length == 0 && (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None);
        if (requested is not [var request] || request != new MouHuangGaiKurouLossRequestedEvent(frame.Id, receipt.Source, gift.RequestedHpLoss))
            return false;
        if (receipt.Stage == MouHuangGaiKurouStage.LossRequested)
            return receipt.Loss is null && losses.Length == 0;
        if (receipt.Loss is not { } loss || losses is not [var lossPaid] || lossPaid != new MouHuangGaiKurouHpLossPaidEvent(frame.Id, receipt.Source, receipt.GameplayHash, loss) || loss.RequestedHpLoss != gift.RequestedHpLoss || loss.ActualLost < 0 || loss.ActualLost > loss.RequestedHpLoss || (long)loss.HpBefore - loss.HpAfter != loss.ActualLost)
            return false;
        return loss.IdentityPrevented ? loss.HpChangeId == 0 && loss.ActualLost == 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<IdentityGeneralAppearedEvent>().Any(fact => fact.Receipt.ParentFrameId == frame.Id && fact.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && fact.Receipt.OwnerSeat == frame.OwnerSeat && fact.Receipt.RequestedDeduction == gift.RequestedHpLoss) : loss.HpChangeId > 0 && loss.ActualLost == gift.RequestedHpLoss && loss.ActualLost > 0 && (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(fact => fact.FrameId == frame.Id && fact.SkillId == frame.SkillId && fact.TargetSeat == frame.OwnerSeat && fact.Amount == loss.ActualLost && fact.RemainingHp == loss.HpAfter) || _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Any(change => change.Id == loss.HpChangeId && change.ParentFrameId == frame.Id && change.Kind == HpChangeKind.Loss && change.TargetSeat == frame.OwnerSeat && change.HpBefore == loss.HpBefore && change.HpAfter == loss.HpAfter));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29050)]
    internal bool ResumeMouKurou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouHuangGaiKurou: not null } frame || frame.Id != id)
            return false;
        ContinueMouKurou(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29050)]
    internal bool ReturnMouKurouMovement(ProgramSkillFrame frame)
    {
        if (frame.MouHuangGaiKurou is null || frame.PendingMovementContinuation is null)
            return false;
        ContinueMouKurou(frame.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29050)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29050)]
    internal bool PermitPaidMouKurou(ProgramSkillFrame frame) => frame.MouHuangGaiKurou?.Gift is not null && ValidMouKurou(frame);
    internal bool MouKurouFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.MouHuangGaiKurou is not { Gift: { } gift } receipt || !ValidMouKurou(frame))
            return false;
        if (child is DyingFrame dying)
            return receipt.Stage == MouHuangGaiKurouStage.LossChildren && receipt.Loss is { ActualLost: > 0, HpAfter: <= 0 } && dying.ParentFrameId == frame.Id && dying.VictimSeat == frame.OwnerSeat && dying.KillerSeat is null && dying.Continuation == DyingContinuationKind.ProgramSkill;
        if (child is IdentityAppearanceFrame identity)
            return receipt.Stage == MouHuangGaiKurouStage.LossRequested && identity.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && identity.Receipt.ParentFrameId == frame.Id && identity.Receipt.OwnerSeat == frame.OwnerSeat && identity.Receipt.RequestedDeduction == gift.RequestedHpLoss;
        if (frame.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.ParentFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence >= gift.MovementSequence && move.Sequence <= receipt.GiftSequenceAfter);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == frame.Id && hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.TargetSeat == frame.OwnerSeat && (receipt.Stage == MouHuangGaiKurouStage.LossChildren && receipt.Loss?.HpChangeId == hp.Change.Id && hp.Change.Kind == HpChangeKind.Loss || receipt.Stage == MouHuangGaiKurouStage.GiftChildren && gift.From == CardLocation.Equipment(frame.OwnerSeat) && gift.PrintedKind == CardKind.SilverLion && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1);
        return child is RecoveryReplacementFrame recovery && receipt.Stage == MouHuangGaiKurouStage.GiftChildren && gift.From == CardLocation.Equipment(frame.OwnerSeat) && gift.PrintedKind == CardKind.SilverLion && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouKurouGiftReason;
    }

    internal bool MouKurouObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame original && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, original) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _resolutionStackOperations2._resolutionStack()[child - 1] is ProgramSkillFrame parent && this.MouKurouFirstChild(parent, _resolutionStackOperations2._resolutionStack()[child]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal ProgramSkillFrame? MouKurouObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => this.MouKurouFirstChild(frame, child), (index) => this.MouKurouObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 730)]
    internal bool IsMouKurouDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && MouKurouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 540)]
    internal bool IsMouKurouRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery)
    {
        if (frame.MouHuangGaiKurou is null || recovery.ParentFrameId == frame.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouKurouFirstChild(frame, _resolutionStackOperations._resolutionStack[parent + 1]) && MouKurouObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29050)]
    internal bool AdvanceMouKurouObserverChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouKurouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal bool ExactMouKurouArmorFrame(ProgramSkillFrame frame)
    {
        if (frame.SkillId != MouKurouSkill || frame.TriggerId != "loss-armor" || frame.ActivationId != frame.TriggerId || frame.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHpLost, HpChange: { Kind: HpChangeKind.Loss, Amount: > 0 } change } context || change.TargetSeat != frame.OwnerSeat || context.OwnerSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat || context.ParentFrameId != change.Id || context.Amount != change.Amount || context.OccurrenceIndex < 0 || context.OccurrenceIndex >= change.Amount || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || _coreProgramLifecycleOperations.GetProgramTrigger(frame)is not { HpChangeOccurrence: SkillProgramHpChangeOccurrence.PerPoint, Effects: [{ Op: SkillProgramEffectOp.GrantMouHuangGaiLossArmor }] } || _resolutionStackOperations._resolutionStack.OfType<HpChangedTriggerWindowFrame>().SingleOrDefault(parent => parent.Id == change.Id)is not { } parent || parent.Change != change || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]) || parent.Contexts[parent.CandidateIndex] != context)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == parent.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(fact => fact.FrameId == frame.Id && fact.OwnerSeat == frame.OwnerSeat && fact.SkillId == frame.SkillId && fact.BindingId == frame.TriggerId && fact.SkillInstanceId == frame.SkillInstanceId && fact.Window == context.Window) == 1;
    }

    internal void GrantMouKurouLossArmor(ProgramSkillFrame input)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (!ExactMouKurouArmorFrame(frame) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangGaiKurouArmorGrantedEvent>().Any(fact => fact.FrameId == frame.Id))
            throw new InvalidOperationException("KuRou armor needs one actual positive Loss point and its original candidate.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (!owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
            return;
        var before = owner.Armor;
        const int actual = 2;
        owner.Armor = checked(before + actual);
        var source = MouKurouSource(frame);
        var context = frame.WindowContext!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangGaiKurouArmorGrantedEvent(frame.Id, source, frame.GameplayHash, context.HpChange!.Id, context.OccurrenceIndex, 2, actual, before, owner.Armor, null));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorGrantedEvent(frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), frame.OwnerSeat, actual, before, owner.Armor));
    }

    internal CardKind MouKurouGiftKind(CharacterState owner, Card card, CardLocation from) => from.Zone == CardZoneKind.Hand ? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(owner, card) : card.Kind;
    internal static int MouKurouGiftLoss(CardKind kind) => RuntimeSkills.RuntimeRules_huang_gai_mou_mobile_MouHuangGaiKurou.MouKurouGiftLoss(kind);
    internal bool MouKurouRecoveryIsPubliclyCertain(GameSnapshot snapshot, int ownerSeat)
    {
        foreach (var other in snapshot.Players.Where(player => player.IsAlive && player.Seat != ownerSeat))
        {
            // Unknown generals can have acquired sources that are active while
            // their skill lists remain hidden. Never query their runtime shard.
            if (!other.IsGeneralPublic || other.IdentityConcealed || other.Skills is null || _coreNativeDamageHandlesOperations.Value.IsDualGeneralMode && (!other.IsSecondaryGeneralPublic || other.SecondarySkills is null))
                return false;
            // A published policy is enough to make recovery uncertain; do not
            // inspect its private qualification, role or source-instance state.
            if (CoreGameEnginePublicDamagePredictionRuntime.VisiblePredictionSkillIds(other).Any(skillId => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program?.CardPolicies.Any(policy => policy.Kind == SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery) == true))
                return false;
        }

        return true;
    }

    internal IReadOnlyList<GameEngine.MouKurouAiCandidate> RankMouKurouAiCandidates(CharacterState owner, ProgramAiPublicContext publicContext, IReadOnlyList<PromptChoice>? publishedChoices = null)
    {
        if (!owner.IsAlive)
            return[];
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
        var recoveryCertain = MouKurouRecoveryIsPubliclyCertain(snapshot, owner.Seat);
        var condition = new SkillProgramCondition(SkillProgramConditionKind.Always, 0, []);
        var candidates = new List<GameEngine.MouKurouAiCandidate>();
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)))
        {
            var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
            var loss = MouKurouGiftLoss(MouKurouGiftKind(owner, card, from));
            // Only a currently effective, publicly known removal recovery can
            // precede payment. A possible recovery redirection is not a rescue.
            var recover = _coreNativeDamageHandlesOperations.Value.UsesFormalSilverLion && card.Kind == CardKind.SilverLion && from == CardLocation.Equipment(owner.Seat) && owner.HasHpValue && owner.Hp < owner.MaxHp && !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(owner) && recoveryCertain ? 1 : 0;
            var health = PublicDamageSequencePrediction.Calculate(owner.Hp, owner.MaxHp, owner.Armor, [new(PublicHealthEffectKind.Recover, recover), new(PublicHealthEffectKind.HpLoss, loss)]);
            var lethal = owner.HasHpValue && health.IsLethal;
            var armor = !lethal && owner.HasHpValue ? checked(health.DirectHpLossAmount * 2) : 0;
            var effects = new[]
            {
                new SkillProgramEffect(SkillProgramEffectOp.Recover, SkillProgramEffectTarget.Owner, recover, condition),
                new SkillProgramEffect(SkillProgramEffectOp.LoseHp, SkillProgramEffectTarget.Owner, loss, condition),
                new SkillProgramEffect(SkillProgramEffectOp.GrantArmor, SkillProgramEffectTarget.Owner, armor, condition)
            };
            var estimate = ProgramCompositionAi.Estimate(effects, _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner), publicContext: publicContext);
            var cardCost = CoreNativeAiDecisionsRuntime.GetKeepValue(card, owner) * 0.35d;
            foreach (var recipient in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat))
            {
                if (publishedChoices is not null && !publishedChoices.Any(choice => choice.Cards is [var id] && id == card.Id && choice.Targets is [var target] && target == recipient.Seat))
                    continue;
                var intended = CardLocation.Hand(recipient.Seat);
                var destination = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, intended) ? CardLocation.OutsideGame : intended);
                var delivers = destination == intended;
                var hint = estimate.Hint with
                {
                    TargetDraw = delivers ? 1 : 0,
                    GivesSelected = delivers
                };
                var targetValue = _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ScoreProgramTarget(snapshot, recipient.Seat, hint);
                candidates.Add(new(card.Id, recipient.Seat, estimate with { Hint = hint with { ValueAdjustment = hint.ValueAdjustment - cardCost }, Score = estimate.Score + targetValue - cardCost, IsSelfLethal = lethal, Approximation = "苦肉按自己实际牌的有效牌名扣血；确定的银狮离装先回复；每实际失去1点体力固定增加2点护甲；不预支濒死救援。" }));
            }
        }

        return candidates.OrderBy(candidate => candidate.Estimate.IsSelfLethal).ThenByDescending(candidate => candidate.Estimate.Score).ThenBy(candidate => candidate.CardId).ThenBy(candidate => candidate.RecipientSeat).ToArray();
    }

    internal (ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouKurouGiftForAi(CharacterState owner, ProgramAiPublicContext publicContext)
    {
        if (RankMouKurouAiCandidates(owner, publicContext).FirstOrDefault()is { } best)
            return (best.Estimate, best.RecipientSeat);
        return (new(new(0, 0, 0, 0, 0, 0, false, false), 0d, false, "苦肉没有可交付的自有牌或存活接收者。"), null);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouHuangGaiKurouGift)]
    internal PromptChoice SelectAiMouKurouGift(PendingDecision decision, ProgramSkillFrame frame)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var best = RankMouKurouAiCandidates(owner, _coreProgramLifecycleOperations.CreateProgramAiPublicContext(owner), decision.Choices).First();
        return decision.Choices.Single(choice => choice.Cards is [var id] && id == best.CardId && choice.Targets is [var recipient] && recipient == best.RecipientSeat);
    }

    SkillProgramStepOutcome IMouHuangGaiKurouHost.GiveBeforeLoss(ProgramSkillFrame frame) => BeginMouKurouGift(frame);
    void IMouHuangGaiKurouHost.GrantLossArmor(ProgramSkillFrame frame) => GrantMouKurouLossArmor(frame);
}
