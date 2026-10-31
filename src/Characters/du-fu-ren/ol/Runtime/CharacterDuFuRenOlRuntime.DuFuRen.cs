#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    internal const string DuFuRenGiftReason = "skill-program.du-fu-ren.shunshi.gift";
    internal static CardConversionSource DuFuRenSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_du_fu_ren_ol_DuFuRen.DuFuRenSource(f);
    SkillProgramStepOutcome global::CardGame.Core.IDuFuRenHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginDuFuRen(f, op);
    internal SkillProgramStepOutcome BeginDuFuRen(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.DuFuRen is not null || f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { } context)
            throw new InvalidOperationException("Du Fu Ren requires its new exact native triggered instruction.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        IReadOnlyList<DuFuRenGain>? gains = null;
        if (op == SkillProgramEffectOp.ResolveYiseOwnedGain ? (gains = CurrentDuFuRenGains(candidate, context))is null : !IsDuFuRenGiftContext(candidate, context))
            throw new InvalidOperationException("Du Fu Ren lost its current gain/preparation/actual off-turn damage candidate.");
        int? excluded = null;
        if (context.Window == SkillProgramTriggerWindow.AfterDamageApplied)
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Single(w => w.Id == context.ParentFrameId);
            excluded = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(parent).IsSourceLess ? null : context.SourceSeat;
        }

        var r = new DuFuRenReceipt(DuFuRenSource(f), f.GameplayHash, f.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, op == SkillProgramEffectOp.ResolveYiseOwnedGain ? DuFuRenStage.RedChoice : DuFuRenStage.GiftChoice, excluded, context.DamageFrameId, context.MovementBatch?.Id, op == SkillProgramEffectOp.ResolveYiseOwnedGain ? context.TargetSeat : null);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuFuRen = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.Turn, r.TurnOwner, r.Stage, r.DamageFrameId, r.BatchId));
        if (gains is not null)
        {
            if (gains.Any(g => g.Suit is Suit.Spade or Suit.Club))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenBlackIssuedEvent(f.Id, r.Source, r.BatchId!.Value, r.RecipientSeat!.Value));
            if (!gains.Any(g => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(g.Suit)) || !_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat!.Value].IsAlive || _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat.Value].Hp >= _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat.Value].MaxHp)
            {
                CompleteDuFuRen(f, true);
                return SkillProgramStepOutcome.AwaitChild;
            }
        }

        PublishDuFuRenChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> DuFuRenChoices(ProgramSkillFrame f)
    {
        var r = f.DuFuRen!;
        PromptChoice Choice(string option, string label, IReadOnlyList<int>? ids = null, int? target = null) => new(new($"du-fu-ren.{(f.Id)}.{(r.Stage)}.{(option)}.{(target)}.{(string.Join('.', ids ?? []))}"), label, ids ?? [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "du-fu-ren", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
        if (r.Stage == DuFuRenStage.RedChoice)
            return _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat!.Value].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat.Value].Hp < _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat.Value].MaxHp ? [Choice("recover", $"令{(_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat.Value].Name)}回复1点体力"), Choice("decline", "不回复体力")] : [];
        if (r.Stage != DuFuRenStage.GiftChoice || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return[];
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, f.WindowContext!.OccurrenceIndex);
        if (!IsDuFuRenGiftContext(candidate, f.WindowContext))
            return[];
        var targets = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && p.Seat != r.DamageSourceSeat).ToArray();
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (_, card) => !card.IsGeneralWeapon).SelectMany(card => targets.Select(target => Choice("gift", $"交给{(target.Name)}【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(card.Cards.Single()).DisplayName)}】", card.Cards, target.Seat))).ToArray());
    }

    internal void PublishDuFuRenChoice(ProgramSkillFrame f)
    {
        var choices = DuFuRenChoices(f);
        if (choices.Count == 0)
        {
            CompleteDuFuRen(f, f.DuFuRen!.Stage == DuFuRenStage.RedChoice);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.DuFuRen!.Stage == DuFuRenStage.GiftChoice ? "顺世：交给伤害来源之外的一名其他角色一张牌。" : "异色：是否令获牌者回复体力？", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("du-fu-ren")]
    internal void AnswerDuFuRen(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuFuRen: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], DuFuRenChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Du Fu Ren lost its exact private chooser, physical card or legal recipient.");
        AssertDuFuRen(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            CompleteDuFuRen(f, r.Stage == DuFuRenStage.RedChoice);
            return;
        }

        if (choice.Parameters["option"] == "decline")
        {
            CompleteDuFuRen(f, true);
            return;
        }

        if (choice.Parameters["option"] == "recover")
        {
            var target = r.RecipientSeat!.Value;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuFuRen = r with { Stage = DuFuRenStage.RecoveryChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(target, 0, null) });
            if (_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, 1, null, null);
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { DuFuRen = current.DuFuRen!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            ContinueDuFuRen(f.Id);
            return;
        }

        if (choice.Parameters["option"] != "gift")
            throw new InvalidOperationException("Unknown Du Fu Ren private choice.");
        var cardId = choice.Cards.Single();
        var recipient = choice.Targets.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuFuRen = r with { Stage = DuFuRenStage.GiftChildren, RecipientSeat = recipient, CardId = cardId, From = from, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([cardId], CardLocation.Hand(recipient), new(DuFuRenGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, [cardId], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = current.DuFuRen!;
            var movement = records.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { DuFuRen = paid with { GiftSequence = movement.Sequence, After = movement.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenGiftIssuedEvent(f.Id, paid.Source, paid.GameplayHash, paid.Turn, paid.TurnOwner, recipient, cardId, from, movement.Sequence));
        }));
        ContinueDuFuRen(f.Id);
    }

    internal bool DrainDuFuRenChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueDuFuRen(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuFuRen: { } r } f || f.Id != id)
            return;
        AssertDuFuRen(f);
        if (r.Stage is DuFuRenStage.GiftChoice or DuFuRenStage.RedChoice)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                CompleteDuFuRen(f, r.Stage == DuFuRenStage.RedChoice);
                return;
            }

            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishDuFuRenChoice(f);
            return;
        }

        if (DrainDuFuRenChildren(f))
            return;
        CompleteDuFuRen(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
    }

    internal void CompleteDuFuRen(ProgramSkillFrame f, bool applied)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenCompletedEvent(f.Id, applied));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuFuRen = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, applied);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26520)]
    internal bool ResumeDuFuRen(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuFuRen: not null } f || f.Id != id)
            return false;
        ContinueDuFuRen(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26520)]
    internal bool ReturnDuFuRenMovement(ProgramSkillFrame f)
    {
        if (f.DuFuRen is null)
            return false;
        ContinueDuFuRen(f.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 130)]
    internal bool IsDuFuRenAwaitedMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.DuFuRen is { } r && r.Stage is DuFuRenStage.GiftChildren or DuFuRenStage.RecoveryChildren && (e is null || DuFuRenComposition.IsOperation(e.Op)) && p.BeforeCount == 0 && p.CoverageResultBind is null && p.SubjectSeat == (r.Stage == DuFuRenStage.GiftChildren ? f.OwnerSeat : r.RecipientSeat);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26520)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26520)]
    internal bool ContinuePaidDuFuRen(ProgramSkillFrame f) => f.DuFuRen is { } r && (r.Stage == DuFuRenStage.RecoveryChildren || r.Stage == DuFuRenStage.GiftChildren && r.GiftSequence > 0) && _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash) == 1;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveYiseOwnedGain, SkillProgramEffectOp.GiveForNextActualPhases)]
    internal PromptChoice AiDuFuRen(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters["option"] == "recover") ?? p.Choices.OrderBy(c => c.Cards.Count == 0 ? 0 : global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
