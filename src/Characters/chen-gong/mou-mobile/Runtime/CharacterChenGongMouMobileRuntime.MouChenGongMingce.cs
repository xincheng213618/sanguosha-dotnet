#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenGongMouMobileRuntime
{
    internal const string MouChenGongGiftReason = "skill-program.mou-mobile:mingce.give", MouChenGongDrawReason = "skill-program.mou-mobile:mingce.Draw";
    internal bool HasMouChenGongCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouChenGongMingceGift) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouChenGongMingceRelease) || _contentRegistryOperations._contentRegistry.Skills.ContainsKey(MouChenGongComposition.Zhichi);

    internal static CardConversionSource MouChenGongSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_chen_gong_mou_mobile_MouChenGongMingce.MouChenGongSource(f);
    internal bool MouChenGongPhaseSpent(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongMingceStartedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == MouChenGongComposition.Mingce && e.Phase == phase);
    [ProgramPhaseUsedProjection(30)]
    internal int MouChenGongProjectedPhaseUses(int owner, string skill, int fallback) => skill == MouChenGongComposition.Mingce && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongMingceStartedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Phase == phase) : fallback;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 29600, SkillProgramEffectOp.ResolveMouChenGongMingceGift)]
    internal bool CanStartMouChenGongGift(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && !MouChenGongPhaseSpent(owner.Seat, phase) && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Any() && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat);
    internal bool MouChenGongGiftRoot(ProgramSkillFrame f) => f.MouChenGongGift is { } r && f.SkillId == MouChenGongComposition.Mingce && f.TriggerId is null && f.ActivationId == "gift-and-plan" && f.InstructionIndex == 1 && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.SequenceEqual([r.RecipientSeat]) && r.Source == MouChenGongSource(f) && r.GameplayHash == f.GameplayHash && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.RecipientSeat) && r.RecipientSeat != f.OwnerSeat && r.Phase.ActorSeat == f.OwnerSeat && r.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is { } p && p.GameplayHash == f.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenGongMingceStartedEvent>().Count(e => e == new MouChenGongMingceStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.RecipientSeat)) == 1;
    internal SkillProgramStepOutcome BeginMouChenGongGift(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouChenGongGift is not null || f.SkillId != MouChenGongComposition.Mingce || f.ActivationId != "gift-and-plan" || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var recipient] || recipient == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || MouChenGongPhaseSpent(f.OwnerSeat, phase))
            throw new InvalidOperationException("Mingce requires one fresh exact actual own Play and one living other recipient.");
        var r = new MouChenGongGiftReceipt(MouChenGongSource(f), f.GameplayHash, phase, recipient);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongMingceStartedEvent(f.Id, r.Source, r.GameplayHash, phase, recipient));
        PublishMouChenGong(f, f.OwnerSeat, MouChenGongGiftChoices(f), "明策：将一张手牌或装备牌交给目标。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishMouChenGong(ProgramSkillFrame f, int chooser, IReadOnlyList<PromptChoice> choices, string text)
    {
        if (choices.Count == 0)
            throw new InvalidOperationException("Mingce lost every canonical private choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal IReadOnlyList<PromptChoice> MouChenGongGiftChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.SelectedTargetHand, destinationSeat: f.MouChenGongGift!.RecipientSeat).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-chen-gong-gift" } }).ToArray());
    [ProgramAnswer("mou-chen-gong-gift")]
    internal void AnswerMouChenGongGift(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouChenGongGift!;
        if (!MouChenGongGiftRoot(f) || r.Stage != MouChenGongGiftStage.GiftChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouChenGongGiftChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Mingce requires the owner's exact current HE payment slot.");
        var from = new CardLocation(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), f.OwnerSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r with { Stage = MouChenGongGiftStage.GiftChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(r.RecipientSeat), new(MouChenGongGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, r.RecipientSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, m =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            if (!MouChenGongGiftRoot(current) || current.MouChenGongGift!.Gift is not null || m.CardId != card.Id || m.From != from)
                throw new InvalidOperationException("Mingce can pay only its one original actual gift.");
            var paid = new MouChenGongGiftPayment(m.CardId, m.CardKind, m.From, m.To, m.Sequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouChenGongGift = current.MouChenGongGift with { Gift = paid, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongGiftPaidEvent(f.Id, r.RecipientSeat, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
        }), disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        SealMouChenGongMovements(f.Id, before);
        ContinueMouChenGongGift(f.Id);
    }

    internal IReadOnlyList<PromptChoice> MouChenGongBranchChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { ("lose-hp", "失去1点体力，令明策发动者摸两张牌并获得一个策"), ("draw", "摸一张牌") }.Select(p => new PromptChoice(new($"mou-chen-gong.{(f.Id)}.{(p.Item1)}"), p.Item2, [], [], new Dictionary<string, string> { ["program-action"] = "mou-chen-gong-mingce", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = p.Item1 })).ToArray());
    [ProgramAnswer("mou-chen-gong-mingce")]
    internal void AnswerMouChenGongBranch(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouChenGongGift!;
        if (!MouChenGongGiftRoot(f) || r.Stage != MouChenGongGiftStage.RecipientChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.RecipientSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouChenGongBranchChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Mingce accepts only its actual recipient's current private branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = selected.Parameters["branch"];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r with { Branch = branch, Stage = branch == "lose-hp" ? MouChenGongGiftStage.LosingHp : MouChenGongGiftStage.Drawing }, PendingMovementContinuation = branch == "lose-hp" ? new(f.OwnerSeat, 0, null) : null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongBranchAcceptedEvent(f.Id, r.RecipientSeat, branch));
        if (branch == "draw")
        {
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.RecipientSeat, 1, new(MouChenGongDrawReason));
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongLossIssuedEvent(f.Id, r.RecipientSeat, 1));
        var result = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, r.RecipientSeat, 1);
        if (result == SkillProgramStepOutcome.Continue && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id)
            ContinueMouChenGongGift(f.Id);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29600)]
    internal void CaptureMouChenGongLoss(IGameEvent fact)
    {
        if (fact is not ProgramSkillHpLostEvent lost || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouChenGongGift: { Stage: MouChenGongGiftStage.LosingHp } r } f || lost.FrameId != f.Id)
            return;
        var change = _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.SingleOrDefault(c => c.ParentFrameId == f.Id && c.Kind == HpChangeKind.Loss && c.TargetSeat == r.RecipientSeat);
        if (!MouChenGongGiftRoot(f) || lost.SkillId != f.SkillId || lost.TargetSeat != r.RecipientSeat || change is null || change.SourceSeat is not null || change.Amount != Math.Min(change.HpBefore, 1) || lost.Amount != change.Amount || lost.RemainingHp != change.HpAfter || r.Loss is not null)
            throw new InvalidOperationException("Mingce's recipient loss must be its one actual native HP deduction.");
        var paid = new MouChenGongLossPayment(change.Id, change.HpBefore, change.HpAfter, change.Amount);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouChenGongGift = r with { Stage = MouChenGongGiftStage.LossChildren, Loss = paid } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongLossReturnedEvent(f.Id, r.RecipientSeat, paid.ChangeId, paid.HpBefore, paid.HpAfter, paid.ActualAmount, false));
    }

    internal void ContinueMouChenGongGift(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouChenGongGift: { } r } f || f.Id != id)
            return;
        AssertMouChenGong(f);
        if (r.Stage is MouChenGongGiftStage.GiftChoice or MouChenGongGiftStage.RecipientChoice or MouChenGongGiftStage.Drawing)
            return;
        if (DrainMouChenGongChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouChenGongGift!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouChenGongGift(f);
            return;
        }

        if (r.Stage == MouChenGongGiftStage.GiftChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive)
            {
                FinishMouChenGongGift(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r with { Stage = MouChenGongGiftStage.RecipientChoice }, PendingMovementContinuation = null });
            PublishMouChenGong(f, r.RecipientSeat, MouChenGongBranchChoices(f), "明策：选择失去体力以助发动者，或自己摸一张牌。");
            return;
        }

        if (r.Stage == MouChenGongGiftStage.LosingHp)
        {
            var appeared = _coreProgramEventHistoryOperations.ProgramEventHistory<IdentityGeneralAppearedEvent>().SingleOrDefault(e => e.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && e.Receipt.ParentFrameId == id && e.Receipt.OwnerSeat == r.RecipientSeat && e.Receipt.RequestedDeduction == 1 && e.Receipt.OriginalInstructionIndex == f.InstructionIndex);
            if (appeared is null || !_coreProgramEventHistoryOperations.ProgramEventHistory<IdentityAppearanceCompletedEvent>().Any(e => e.AppearanceFrameId == appeared.AppearanceFrameId))
                throw new InvalidOperationException("Mingce cannot resume an unreturned native HP deduction.");
            var paid = new MouChenGongLossPayment(0, appeared.Receipt.RestoredHp, appeared.Receipt.RestoredHp, 0, true);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r with { Stage = MouChenGongGiftStage.LossChildren, Loss = paid } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongLossReturnedEvent(f.Id, r.RecipientSeat, 0, paid.HpBefore, paid.HpAfter, 0, true));
            r = f.MouChenGongGift!;
        }

        if (r.Stage == MouChenGongGiftStage.LossChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishMouChenGongGift(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r with { Stage = MouChenGongGiftStage.Drawing }, PendingMovementContinuation = null });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(MouChenGongDrawReason));
            return;
        }

        if (r.Stage != MouChenGongGiftStage.DrawChildren)
            throw new InvalidOperationException("Mingce cannot replay its accepted branch.");
        if (r.Branch == "lose-hp" && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && !r.CeGained)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r with { CeGained = true } });
            ChangeMouChenGongCe(f, 1);
        }

        FinishMouChenGongGift(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouChenGongMingceGift)]
    internal void ReturnMouChenGongDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        var r = f.MouChenGongGift!;
        var seat = r.Branch == "lose-hp" ? f.OwnerSeat : r.RecipientSeat;
        var count = r.Branch == "lose-hp" ? 2 : 1;
        if (!MouChenGongGiftRoot(f) || r.Stage != MouChenGongGiftStage.Drawing || r.Draw is not null || outcome.RecipientSeat != seat || outcome.RequestedCount != count)
            throw new InvalidOperationException("Mingce cannot replace or repeat its exact accepted native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = r with { Stage = MouChenGongGiftStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongDrawReturnedEvent(f.Id, seat, count, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        SealMouChenGongMovements(f.Id, outcome.SequenceBefore);
        ContinueMouChenGongGift(f.Id);
    }

    internal void FinishMouChenGongGift(ProgramSkillFrame f)
    {
        var r = f.MouChenGongGift!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongMingceCompletedEvent(f.Id, r.RecipientSeat, r.Branch, r.CeGained));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongGift = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouChenGongMingceGift)]
    internal PromptChoice AiMouChenGongGift(PendingDecision pending, ProgramSkillFrame f)
    {
        if (f.MouChenGongGift!.Stage == MouChenGongGiftStage.GiftChoice)
            return pending.Choices.OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(pending.PlayerSeat);
        var friendly = _coreAiBrainsAiBrainsAccess._aiBrains[pending.PlayerSeat].ScoreProgramTarget(view, f.OwnerSeat, new(OwnerDraw: 0, OwnerRecovery: 0, OwnerHpLoss: 0, TargetDraw: 1, TargetRecovery: 0, TargetHpLoss: 0, GivesSelected: false, DiscardsSelected: false)) > 0;
        var branch = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && view.Players[pending.PlayerSeat].Hp >= 4 && friendly ? "lose-hp" : "draw";
        return pending.Choices.Single(c => c.Parameters["branch"] == branch);
    }

    SkillProgramStepOutcome global::CardGame.Core.IMouChenGongHost.Gift(ProgramSkillFrame frame) => BeginMouChenGongGift(frame);
    SkillProgramStepOutcome global::CardGame.Core.IMouChenGongHost.Release(ProgramSkillFrame frame) => BeginMouChenGongRelease(frame);
}
