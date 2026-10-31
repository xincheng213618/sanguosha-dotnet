#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSlashSuitDiscardRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ISlashSuitDiscardProgramHost.SuppressCurrentSlashTargetAndJudgeSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => BeginProgramSlashSuitDiscard(frame, effect);
    internal global::CardGame.Core.NativeOwners.CardAttackHandle SlashSuitDiscardAttack(ProgramSkillFrame frame, bool requireActive = true)
    {
        var context = frame.WindowContext;
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(item => item.Id == context?.ParentFrameId);
        var attack = window?.AttackOwnerFrameId is { } ownerId ? _kernelCoreMainOperations.CreateCardAttackHandle(ownerId) : throw new InvalidOperationException("Suit discard lost its Slash owner.");
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == attack.ResolutionId);
        if (context is not { Window: SkillProgramTriggerWindow.SlashBeforeResponse, CardUse: { } action } || context.TargetSeat != attack.TargetSeat || frame.OwnerSeat != attack.CardUserSeat || use.Action?.ActionId != action.CardActionId || action.ParentCardUseFrameId != use.Id || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || requireActive && !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || frame.SlashSuitDiscard is { } draft && (draft.CardUseFrameId != use.Id || draft.CardActionId != action.CardActionId || draft.TargetSeat != attack.TargetSeat))
            throw new InvalidOperationException("Suit discard lost its exact final target or actual Slash.");
        return attack;
    }

    internal SkillProgramStepOutcome BeginProgramSlashSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var attack = SlashSuitDiscardAttack(frame);
        if (frame.SlashSuitDiscard is not null || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive)
            throw new InvalidOperationException("Suit judgment requires live actual participants and a clean cursor.");
        frame = frame with
        {
            SlashSuitDiscard = new(attack.ResolutionId, frame.WindowContext!.CardUse!.CardActionId, attack.TargetSeat, effect.JudgmentReason!, effect.ResultBind!, ProgramSlashSuitDiscardStage.Judging)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { JudgmentSuitDodgeRestriction = new(frame.Id, attack.TargetSeat) });
        _coreCurrentTurnSkillSuppressionsOperations.IssueCurrentTurnNonLockedSkillSuppression(frame, attack.TargetSeat);
        return _coreProgramLifecycleOperations.StartProgramJudgment(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), frame.OwnerSeat, effect.JudgmentReason!, effect.ResultBind!, SkillProgramCardSetVisibility.Public, frame.OwnerSeat);
    }

    internal void CaptureProgramSlashSuitJudgment(JudgmentFrame judgment, Suit suit)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == judgment.ParentFrameId)is not { SlashSuitDiscard: { Stage: ProgramSlashSuitDiscardStage.Judging } draft } frame)
            return;
        if (judgment.Continuation != JudgmentContinuationKind.ProgramSkill || judgment.TargetSeat != frame.OwnerSeat || judgment.SourceSeat != frame.OwnerSeat || judgment.Reason != draft.JudgmentReason || judgment.ProgramResultBind != draft.ResultBind || judgment.ParentAttackId != draft.CardUseFrameId || draft.JudgmentFrameId is not null)
            throw new InvalidOperationException("Suit judgment result lost its typed parent.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { SlashSuitDiscard = draft with { JudgmentFrameId = judgment.Id, FinalSuit = suit } });
    }

    internal bool TryReturnProgramSlashSuitJudgment(JudgmentFrame judgment)
    {
        if (_coreProgramLifecycleOperations.GetActiveProgramFrame(judgment.ParentFrameId)is not { SlashSuitDiscard: { } draft } frame)
            return false;
        if (draft.Stage != ProgramSlashSuitDiscardStage.Judging || judgment.TargetSeat != frame.OwnerSeat || judgment.SourceSeat != frame.OwnerSeat || judgment.Reason != draft.JudgmentReason || judgment.ProgramResultBind != draft.ResultBind || judgment.ParentAttackId != draft.CardUseFrameId || draft.JudgmentFrameId is { } actualId && actualId != judgment.Id)
            throw new InvalidOperationException("Suit judgment receipt lost its exact instruction.");
        _ = SlashSuitDiscardAttack(frame);
        frame = frame with
        {
            SlashSuitDiscard = draft with
            {
                Stage = ProgramSlashSuitDiscardStage.JudgmentMovement,
                JudgmentFrameId = judgment.Id
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        // A finalized-judgment claimant keeps its real card. Only our unclaimed Processing result is disposed.
        if (_coreNativeDamageHandlesOperations.GetJudgmentCard(judgment)is { } card && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.JudgmentFinish);
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 850)]
    internal bool ResumeProgramSlashSuitDiscard(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.SlashSuitDiscard is not { } draft)
            return false;
        if (_coreShanheS5MaSiblingsOperations.IsShanheS5Tieqi(frame))
            return _coreShanheS5MaSiblingsOperations.ResumeShanheS5Tieqi(frame);
        if (frame.PendingMovementContinuation is not null)
            return true;
        _ = SlashSuitDiscardAttack(frame);
        if (draft.Stage == ProgramSlashSuitDiscardStage.Judging)
            return true;
        if (draft.Stage == ProgramSlashSuitDiscardStage.Choosing)
            return true;
        if (draft.Stage == ProgramSlashSuitDiscardStage.PaymentMovement)
        {
            var paid = draft.PaymentMovementSequence is { } sequence && draft.PaidCardId is { } cardId && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(move => move.Sequence == sequence && move.CardId == cardId && move.From == draft.PaidFrom && move.From.OwnerSeat == draft.TargetSeat && move.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && move.To == CardLocation.DiscardPile && move.Reason.Value == "skill-program.slash-suit-discard.payment");
            FinishProgramSlashSuitDiscard(frame, paid);
            return false;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive || draft.FinalSuit is null or Suit.None || !SlashSuitDiscardCards(draft).Any())
        {
            FinishProgramSlashSuitDiscard(frame, false);
            return false;
        }

        frame = frame with
        {
            SlashSuitDiscard = draft with
            {
                Stage = ProgramSlashSuitDiscardStage.Choosing
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        PublishProgramSlashSuitDiscard(frame);
        return true;
    }

    internal IEnumerable<Card> SlashSuitDiscardCards(ProgramSlashSuitDiscardDraft draft) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat])).Where(card => draft.FinalSuit is { } suit && suit != Suit.None && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat], card) == suit);
    internal IReadOnlyList<PromptChoice> SlashSuitDiscardChoices(ProgramSkillFrame frame)
    {
        var draft = frame.SlashSuitDiscard!;
        Dictionary<string, string> Parameters() => new()
        {
            ["program-action"] = "slash-suit-discard",
            ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)
        };
        return SlashSuitDiscardCards(draft).Select(card => new PromptChoice(new($"slash-suit-discard.{(frame.Id)}.pay.{(card.Id)}"), $"弃置【{(card.DisplayName)}】{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(draft.FinalSuit))}{(card.RankText)}", [card.Id], [], Parameters())).Append(new(new($"slash-suit-discard.{(frame.Id)}.decline"), frame.SkillId == "boundary:tieqi-mobile" ? "不弃置，不能使用【闪】响应此杀" : "不弃置，不能抵消此杀", [], [], Parameters())).ToArray();
    }

    internal void PublishProgramSlashSuitDiscard(ProgramSkillFrame frame)
    {
        var draft = frame.SlashSuitDiscard!;
        var choices = SlashSuitDiscardChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.TargetSeat, $"【{(skill.Name)}】：是否弃置一张{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(draft.FinalSuit))}牌，使你{((frame.SkillId == "boundary:tieqi-mobile" ? "可以使用【闪】响应此杀" : "能抵消此杀"))}？", choices.SelectMany(choice => choice.Cards).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            TargetSeat = draft.TargetSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool IsProgramSlashSuitDiscardChoiceLegal(PromptChoice choice) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { SlashSuitDiscard: { Stage: ProgramSlashSuitDiscardStage.Choosing } } frame && choice.Parameters.GetValueOrDefault("frame-id") == frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) && SlashSuitDiscardChoices(frame).Any(candidate => candidate.Id == choice.Id);
    [ProgramAnswer("slash-suit-discard")]
    internal void ResolveProgramSlashSuitDiscardChoice(PromptChoice choice)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var draft = frame.SlashSuitDiscard!;
        if (!IsProgramSlashSuitDiscardChoiceLegal(choice))
            throw new InvalidOperationException("Suit discard choice is stale.");
        _ = SlashSuitDiscardAttack(frame);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Cards is not [var cardId] || !_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishProgramSlashSuitDiscard(frame, false);
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        var card = SlashSuitDiscardCards(draft).Single(candidate => candidate.Id == cardId);
        frame = frame with
        {
            SlashSuitDiscard = draft with
            {
                Stage = ProgramSlashSuitDiscardStage.PaymentMovement,
                PaidCardId = cardId,
                PaidFrom = from
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, draft.TargetSeat, card, from, new("skill-program.slash-suit-discard.payment"));
        var sequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(move => move.CardId == cardId && move.From == from && move.To == CardLocation.DiscardPile && move.Reason.Value == "skill-program.slash-suit-discard.payment")?.Sequence;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SlashSuitDiscard = frame.SlashSuitDiscard!with { PaymentMovementSequence = sequence } });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void FinishProgramSlashSuitDiscard(ProgramSkillFrame frame, bool paid)
    {
        var draft = frame.SlashSuitDiscard!;
        var state = _coreCardAttackStateOperations.GetCardAttackState(draft.CardUseFrameId);
        if (paid && state.JudgmentSuitDodgeRestriction is { } restriction && restriction.ProgramFrameId == frame.Id && restriction.TargetSeat == draft.TargetSeat)
            _coreCardAttackStateOperations.UpdateCardAttackState(draft.CardUseFrameId, current => current!with { JudgmentSuitDodgeRestriction = null });
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SlashSuitDiscard = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSlashSuitDiscardResolvedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, draft.CardUseFrameId, draft.TargetSeat, draft.FinalSuit, draft.PaidCardId, paid));
    }

    internal bool IsSlashDodgeCancellationPrevented(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _coreWushuangCombatTacticsOperations.WushuangDrunkenSlashUnrespondable(attack) || _coreFinalTargetSlashOperations.FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat).Any(item => item.PreventCancellation) || _coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(attack.ResolutionId, CurrentCardEnhancement.Uncancelable) || _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).JudgmentSuitDodgeRestriction?.TargetSeat == attack.TargetSeat;
    [ProgramAiChoice(SkillProgramEffectOp.SuppressCurrentSlashTargetAndJudgeSuitDiscard)]
    internal PromptChoice SelectAiSlashSuitDiscard(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices.OrderBy(choice => choice.Cards.Count == 0 ? double.MaxValue : global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(SlashSuitDiscardCards(frame.SlashSuitDiscard!).Single(card => card.Id == choice.Cards[0]), _coreMatchParticipantsPlayersAccess._players[frame.SlashSuitDiscard!.TargetSeat])).First();
    internal void AssertProgramSlashSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.SlashSuitDiscard is not { } draft)
            return;
        if (_coreShanheS5MaSiblingsOperations.IsShanheS5Tieqi(frame))
        {
            _coreShanheS5MaSiblingsOperations.AssertShanheS5Tieqi(frame);
            return;
        }

        if (paused.Op != SkillProgramEffectOp.SuppressCurrentSlashTargetAndJudgeSuitDiscard || paused.JudgmentReason != draft.JudgmentReason || paused.ResultBind != draft.ResultBind || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(draft.TargetSeat) || draft.TargetSeat == frame.OwnerSeat || draft.Stage == ProgramSlashSuitDiscardStage.PaymentMovement && (draft.PaidCardId is null || draft.PaidFrom is null))
            throw new InvalidOperationException("Suit discard lost its typed cursor.");
        _ = SlashSuitDiscardAttack(frame, requireActive: false);
        if (ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && draft.Stage == ProgramSlashSuitDiscardStage.Choosing && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != draft.TargetSeat || !decision.IsPrivate || !decision.Choices.Select(choice => choice.Id).SequenceEqual(SlashSuitDiscardChoices(frame).Select(choice => choice.Id))))
            throw new InvalidOperationException("Suit discard lost its exact target-private HE choices.");
    }
}
