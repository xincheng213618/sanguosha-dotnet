#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongBaiMobileRuntime
{
    internal const string MobileLianzhuDrawReason = "skill-program.mobile:lianzhu.relay-draw";
    internal SkillProgramStepOutcome BeginMobileLianzhuRelay(ProgramSkillFrame f)
    {
        if (f.InstructionIndex != 1 || f.MobileLianzhu is not null || f.WindowContext is not { } c || !TryMobileLianzhuGain(f.OwnerSeat, c, out var issued, out var movement) || movement.To.OwnerSeat is not { } holder)
            throw new InvalidOperationException("Mobile Lianzhu relay requires the exact actual gained entity and original movement index.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLianzhu = new(issued.FrameId, issued.CardId, holder, movement.Sequence, MobileLianzhuStage.Choice) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLianzhuGainProcessedEvent(f.Id, issued.FrameId, movement.Sequence, holder));
        PublishMobileLianzhuChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome RecoverMobileLianzhu(ProgramSkillFrame f)
    {
        if (f.InstructionIndex != 1 || f.MobileLianzhu is not null || ActiveMobileLianzhu(f.OwnerSeat)is not { } issued || f.WindowContext is not { TargetSeat: { } holder, SourceSeat: { } source, Amount: > 0 } || source != f.OwnerSeat || !MobileLianzhuHolder(f.OwnerSeat, holder))
            throw new InvalidOperationException("Mobile Lianzhu recovery requires its original outgoing damage and current entity holder.");
        var from = _cardZonesOperations._cardZones.GetLocation(issued.CardId);
        if (from == CardLocation.Hand(f.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLianzhu = new(issued.FrameId, issued.CardId, holder, 0, MobileLianzhuStage.RecoverChildren) });
        MoveMobileLianzhuEntity(f, MobileLianzhuStage.RecoverChildren, issued.CardId, from, f.OwnerSeat);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> MobileLianzhuGiftCards(ProgramSkillFrame f) => f.MobileLianzhu!.Holder == f.OwnerSeat ? [] : _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.MobileLianzhu.Holder, f.MobileLianzhu.Holder, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (_, card) => card.Id != f.MobileLianzhu.CardId, destination: SkillProgramCardDestination.OwnerHand, destinationSeat: f.OwnerSeat);
    internal PromptChoice MobileLianzhuChoice(ProgramSkillFrame f, string branch, string text, IReadOnlyList<int>? cards = null) => new(new($"mobile-lianzhu.{(f.Id)}.{(f.MobileLianzhu!.Stage)}.{(branch)}"), text, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "mobile-lianzhu-relay", ["branch"] = branch, ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> MobileLianzhuChoices(ProgramSkillFrame f)
    {
        if (f.MobileLianzhu!.Stage == MobileLianzhuStage.SelectingGift)
            return Array.AsReadOnly(MobileLianzhuGiftCards(f).Select(c => MobileLianzhuChoice(f, "card-" + c.Cards.Single(), c.Label, c.Cards)).ToArray());
        var choices = new List<PromptChoice>
        {
            MobileLianzhuChoice(f, "draw", "令董白摸两张牌")
        };
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && MobileLianzhuGiftCards(f).Count > 0)
            choices.Add(MobileLianzhuChoice(f, "give", "交给董白一张其他牌，再将连诛牌交给自己的上家"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMobileLianzhuChoice(ProgramSkillFrame f)
    {
        var r = f.MobileLianzhu!;
        var choices = MobileLianzhuChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.Holder, "请选择连诛结算。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.Holder,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[r.Holder].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay)]
    internal PromptChoice ChooseMobileLianzhuRelay(ProgramSkillFrame f) => MobileLianzhuChoices(f).First();
    [ProgramAnswer("mobile-lianzhu-relay")]
    internal void AnswerMobileLianzhuRelay(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        RequireMobileLianzhuReceipt(f);
        var r = f.MobileLianzhu!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.Holder || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MobileLianzhuChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Mobile Lianzhu lost the real holder's private authority or exact current choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive || ActiveMobileLianzhu(f.OwnerSeat)?.FrameId != r.IssuedFrameId)
        {
            FinishMobileLianzhu(f);
            return;
        }

        if (choice.Parameters["branch"] == "draw")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLianzhu = r with { Stage = MobileLianzhuStage.Drawing }, Step = ResolutionFrameStep.ResolvingEffect });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(MobileLianzhuDrawReason));
            return;
        }

        if (choice.Parameters["branch"] == "give")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLianzhu = r with { Stage = MobileLianzhuStage.SelectingGift } });
            PublishMobileLianzhuChoice(f);
            return;
        }

        var card = choice.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(card);
        if (card == r.CardId || from.OwnerSeat != r.Holder || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("Mobile Lianzhu cannot pay its gift with the designated entity or a changed original source.");
        MoveMobileLianzhuEntity(f, MobileLianzhuStage.GiftChildren, card, from, f.OwnerSeat);
    }

    internal void MoveMobileLianzhuEntity(ProgramSkillFrame f, MobileLianzhuStage stage, int card, CardLocation from, int recipient)
    {
        var r = f.MobileLianzhu!with
        {
            Stage = stage,
            PaymentCardId = card,
            PaymentFrom = from,
            Recipient = recipient,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActualTo = null
        };
        var actor = stage == MobileLianzhuStage.RecoverChildren ? f.OwnerSeat : r.Holder;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLianzhu = r, PendingMovementContinuation = new(actor, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var reason = new CardMoveReason("skill-program.mobile:lianzhu." + stage);
        void Paid(long batchId, IReadOnlyList<CardMovementRecord> records)
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = r with
            {
                After = records.Max(m => m.Sequence),
                ActualTo = records.Single(m => m.CardId == card).To
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { MobileLianzhu = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLianzhuSegmentPaidEvent(f.Id, stage, actor, card, from, recipient, paid.Before, paid.After, paid.ActualTo));
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card], CardLocation.Hand(recipient), reason, stage == MobileLianzhuStage.RecoverChildren ? Paid : _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, r.Holder, recipient, [card], r.Before, Paid));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay)]
    internal void ReturnMobileLianzhuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.MobileLianzhu is not { Stage: MobileLianzhuStage.Drawing, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 2 || outcome.SequenceBefore < 0 || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == f.Id && e.RecipientSeat == f.OwnerSeat && e.RequestedCount == 2 && e.Replaced == (outcome.Replacement is not null)) != 1)
            throw new InvalidOperationException("Mobile Lianzhu draw must return its exact original requested native two-card draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLianzhu = r with { Stage = MobileLianzhuStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLianzhuSegmentPaidEvent(f.Id, MobileLianzhuStage.DrawChildren, f.OwnerSeat, null, null, f.OwnerSeat, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueMobileLianzhu(f.Id);
    }

    internal void FinishMobileLianzhu(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLianzhu = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 79100)]
    internal bool ContinueMobileLianzhu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileLianzhu: { } r } f || f.Id != id || r.Stage is MobileLianzhuStage.Choice or MobileLianzhuStage.SelectingGift or MobileLianzhuStage.Drawing)
            return false;
        RequireMobileLianzhuReceipt(f);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return true;
        if (r.Stage == MobileLianzhuStage.GiftChildren && r.ActualTo == CardLocation.Hand(f.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive && ActiveMobileLianzhu(f.OwnerSeat)?.FrameId == r.IssuedFrameId && _cardZonesOperations._cardZones.GetLocation(r.CardId)is { OwnerSeat: { } holder, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } from && holder == r.Holder)
        {
            var previous = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != holder).OrderBy(p => (holder - p.Seat + _coreMatchParticipantsPlayersAccess._players.Count) % _coreMatchParticipantsPlayersAccess._players.Count).FirstOrDefault();
            if (previous is not null)
            {
                MoveMobileLianzhuEntity(f, MobileLianzhuStage.PassChildren, r.CardId, from, previous.Seat);
                return true;
            }
        }

        FinishMobileLianzhu(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 79100)]
    internal bool ReturnMobileLianzhuMovement(ProgramSkillFrame f) => f.MobileLianzhu is not null && ContinueMobileLianzhu(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 79100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 79100)]
    internal bool KeepMobileLianzhuRelay(ProgramSkillFrame f)
    {
        if (f.MobileLianzhu is not null)
        {
            RequireMobileLianzhuReceipt(f);
            return true;
        }

        if (KeepMobileLianzhuGift(f))
            return true;
        return f.SkillId == MobileLianzhu && f.TriggerId is { } trigger && f.InstructionIndex == 0 && f.WindowContext is { } context && AdmitIssuedMobileLianzhu(new(f.OwnerSeat, f.SkillId, trigger, f.SkillInstanceId, f.GameplayHash, 0), context);
    }
    internal void RequireMobileLianzhuReceipt(ProgramSkillFrame f)
    {
        if (f.MobileLianzhu is not { } r || f.SkillId != MobileLianzhu || (r.Stage == MobileLianzhuStage.InitialGiftChildren ? f.TriggerId is not null || f.ActivationId != "reveal-and-give" || f.InstructionIndex != 4 : f.TriggerId is not ("gained-relay" or "damage-recover") || f.InstructionIndex != 1) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileLianzhuIssuedEvent>().SingleOrDefault(e => e.FrameId == r.IssuedFrameId)is not { } issued || !(r.Stage == MobileLianzhuStage.InitialGiftChildren ? MobileLianzhuGiftMoved(issued) : MobileLianzhuGiftPaid(issued)) || issued.Source.OwnerSeat != f.OwnerSeat || issued.Source.SkillInstanceId != f.SkillInstanceId || issued.Hash != f.GameplayHash || issued.CardId != r.CardId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Holder) || !Enum.IsDefined(r.Stage) || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Mobile Lianzhu lost its original issued phase, entity, skill source or owning relay frame.");
        if (f.TriggerId == "gained-relay" && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileLianzhuGainProcessedEvent>().Count(e => e.FrameId == f.Id && e.IssuedFrameId == issued.FrameId && e.MovementSequence == r.GainSequence && e.Holder == r.Holder) != 1)
            throw new InvalidOperationException("Mobile Lianzhu relay needs its exact once-consumed gain occurrence.");
        if (r.Stage is MobileLianzhuStage.Choice or MobileLianzhuStage.SelectingGift)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } p && (p.PlayerSeat != r.Holder || !p.IsPrivate || p.SkillPrompt?.SkillId != MobileLianzhu || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, MobileLianzhuChoices(f))))
                throw new InvalidOperationException("Mobile Lianzhu lost the exact original holder choice after its real gain.");
            return;
        }

        if (r.Stage == MobileLianzhuStage.Drawing)
            return;
        var actor = r.Stage is MobileLianzhuStage.RecoverChildren or MobileLianzhuStage.DrawChildren ? f.OwnerSeat : r.Holder;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MobileLianzhuSegmentPaidEvent>().Count(e => e == new MobileLianzhuSegmentPaidEvent(f.Id, r.Stage, actor, r.Stage == MobileLianzhuStage.DrawChildren ? null : r.PaymentCardId, r.Stage == MobileLianzhuStage.DrawChildren ? null : r.PaymentFrom, r.Stage == MobileLianzhuStage.DrawChildren ? f.OwnerSeat : r.Recipient, r.Before, r.After, r.Stage == MobileLianzhuStage.DrawChildren ? null : r.ActualTo)) != 1 || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != actor)
            throw new InvalidOperationException("Mobile Lianzhu must retain its exact paid native segment until its children return.");
        if (r.Stage != MobileLianzhuStage.DrawChildren && (r.PaymentCardId is not { } card || r.PaymentFrom is not { } from || r.Recipient is null || r.ActualTo is null || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == card && m.From == from && m.To == r.ActualTo && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == "skill-program.mobile:lianzhu." + r.Stage) != 1))
            throw new InvalidOperationException("Mobile Lianzhu cannot repeat or replace its paid original entity movement.");
    }

    internal bool MobileLianzhuFirstChild(ProgramSkillFrame root, ResolutionFrame child) => root.MobileLianzhu is { } r && r.Stage is MobileLianzhuStage.GiftChildren or MobileLianzhuStage.PassChildren or MobileLianzhuStage.DrawChildren or MobileLianzhuStage.RecoverChildren or MobileLianzhuStage.InitialGiftChildren && child is CardsMovedTriggerWindowFrame moved && moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    internal ProgramSkillFrame? MobileLianzhuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.MobileLianzhuFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 79100)]
    internal bool AdvanceMobileLianzhuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileLianzhuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
