#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanPuOlRuntime
{
    internal const string YanPuGiftReason = "skill-program.yan-pu.huantu.gift";
    SkillProgramStepOutcome global::CardGame.Core.IYanPuHost.ExecuteYanPu(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginYanPu(f, op);
    internal SkillProgramStepOutcome BeginYanPu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.YanPu is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context)
            throw new InvalidOperationException("YanPu requires a new exact actual-boundary instruction.");
        var c = op == SkillProgramEffectOp.ResolveHuantuEnding ? YanPuCurrentEndingFrameCandidate(f) ?? throw new InvalidOperationException("Huantu Ending lost its exact current candidate.") : new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        YanPuHuantuPromise? promise = null;
        YanPuDyingExitReturn? exited = null;
        var stage = op == SkillProgramEffectOp.GiveCardSkipDrawForEnding ? YanPuStage.GiftChoice : op == SkillProgramEffectOp.ResolveHuantuEnding ? YanPuStage.EndingChoice : YanPuStage.ExitDrawChildren;
        if (op == SkillProgramEffectOp.GiveCardSkipDrawForEnding && !IsYanPuDrawContext(c, context) || op == SkillProgramEffectOp.ResolveHuantuEnding && (promise = CurrentYanPuEnding(c, context))is null || op == SkillProgramEffectOp.DrawDyingExitRoundDistance && !IsYanPuDyingExitContext(c, context))
            throw new InvalidOperationException("YanPu lost its actual Draw, issued Ending or survived Dying parent.");
        if (op == SkillProgramEffectOp.DrawDyingExitRoundDistance)
            exited = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(w => w.Id == context.ParentFrameId).YanPuDyingExit!;
        var r = new YanPuReceipt
        {
            Source = YanPuSource(f),
            GameplayHash = f.GameplayHash,
            InstructionIndex = f.InstructionIndex,
            WindowId = context.ParentFrameId,
            Round = _coreActualRoundProgressRoundNumberAccess._roundNumber,
            Turn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            TurnOwner = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat,
            Target = promise?.RecipientSeat ?? context.TargetSeat!.Value,
            Stage = stage,
            Promise = promise,
            Exited = exited
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanPu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanPuStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.Round, r.Turn, r.TurnOwner, r.Target, stage));
        if (promise is not null)
        {
            if (YanPuPromiseConsumed(promise.Id))
                throw new InvalidOperationException("An issued Huantu Ending cannot be consumed twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanPuHuantuConsumedEvent(promise.Id, context.ParentFrameId, f.Id));
        }

        if (exited is not null)
        {
            IssueYanPuDraw(f, r.Target, 3, YanPuStage.ExitDrawChildren);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishYanPuChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> YanPuChoices(ProgramSkillFrame f)
    {
        var r = f.YanPu!;
        PromptChoice Choice(string option, string label, IReadOnlyList<int>? ids = null) => new(new($"yanpu.{(f.Id)}.{(r.Stage)}.{(option)}.{(string.Join('.', ids ?? []))}"), label, ids ?? [], [], new Dictionary<string, string> { ["program-action"] = "yan-pu", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
        if (r.Stage == YanPuStage.GiftChoice)
        {
            var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, f.WindowContext!.OccurrenceIndex);
            if (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !IsYanPuDrawContext(c, f.WindowContext))
                return[];
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).Select(c => Choice("gift", $"交给{(_coreMatchParticipantsPlayersAccess._players[r.Target].Name)}【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).DisplayName)}】并跳过此次摸牌阶段", c.Cards)).ToArray());
        }

        if (r.Stage == YanPuStage.EndingChoice)
            return[Choice("recover-draw", "令其回复1点体力并摸两张牌"), Choice("draw-give", "你摸三张牌并交给其两张手牌")];
        if (r.Stage == YanPuStage.EndingGiftChoice)
            return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !r.SelectedCards.Contains(c.Id)).Select(c => Choice("ending-card", $"交给{(_coreMatchParticipantsPlayersAccess._players[r.Target].Name)}【{(c.DisplayName)}】（{(r.SelectedCards.Count + 1)}/{(r.RequiredGift)}）", [c.Id])).ToArray());
        return[];
    }

    internal void PublishYanPuChoice(ProgramSkillFrame f)
    {
        var choices = YanPuChoices(f);
        if (choices.Count == 0)
        {
            CompleteYanPu(f, f.YanPu!.Promise is not null);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.YanPu!.Stage == YanPuStage.GiftChoice ? "缓图：交给其一张牌。" : "缓图：结算本次实际回合的收益。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("yan-pu")]
    internal void AnswerYanPu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YanPu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], YanPuChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("YanPu lost its exact private chooser or current owned physical payment.");
        AssertYanPu(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Stage != YanPuStage.EndingChoice && !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
        {
            CompleteYanPu(f, r.Promise is not null);
            return;
        }

        var option = choice.Parameters["option"];
        if (option == "gift")
        {
            var id = choice.Cards.Single();
            var from = _cardZonesOperations._cardZones.GetLocation(id);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanPu = r with { Stage = YanPuStage.GiftChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(r.Target), new(YanPuGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, r.Target, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, movements) =>
            {
                var root = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var move = movements.Single();
                var paid = root.YanPu!;
                var promise = new YanPuHuantuPromise(root.Id, paid.Source, paid.GameplayHash, paid.WindowId, paid.Round, paid.Turn, paid.TurnOwner, paid.Target, id, from, move.Sequence);
                var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(p => p.Id == paid.WindowId);
                if (parent.ActualPhaseSubstitution is not null || parent.OwnerSeat != paid.Target || parent.Window != SkillProgramTriggerWindow.DrawPhaseStarting)
                    throw new InvalidOperationException("A paid Huantu gift cannot replace another actual phase substitution.");
                _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ActualPhaseSubstitution = new(parent.Id, root.Id, paid.Target, root.SkillId, root.SkillInstanceId, paid.Turn, SkillProgramTurnPhase.Draw) });
                _coreRuntimeOperations.ReplaceRuntimeTop(root with { YanPu = paid with { Promise = promise } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanPuHuantuIssuedEvent(promise));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(root.Id, root.SkillId, root.TriggerId!, root.SkillInstanceId, paid.Target, paid.Turn, SkillProgramTriggerWindow.DrawPhaseStarting, SkillProgramTurnPhase.Draw));
                CommitYanPuMovement(root.Id, new(0, root.OwnerSeat, YanPuGiftReason, before, move.Sequence, 1, id, from, CardLocation.Hand(paid.Target)));
            }));
            // Equipment-removal recovery can have queued native children after the payment callback.
            ContinueYanPu(f.Id);
            return;
        }

        if (option == "recover-draw")
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanPu = r with { Stage = YanPuStage.RecoveryChildren }, PendingMovementContinuation = new(r.Target, 0, null) });
            if (_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, r.Target, 1, null, null);
            CommitYanPuMovement(f.Id, new(r.Movements.Count, r.Target, "skill-program.yan-pu.huantu.recover", before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, 0));
            ContinueYanPu(f.Id);
            return;
        }

        if (option == "draw-give")
        {
            IssueYanPuDraw(f, f.OwnerSeat, 3, YanPuStage.OwnerDrawChildren);
            return;
        }

        if (option != "ending-card")
            throw new InvalidOperationException("Unknown YanPu instruction choice.");
        var selected = r.SelectedCards.Append(choice.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanPu = r with { SelectedCards = selected } });
        if (selected.Length < r.RequiredGift)
        {
            PublishYanPuChoice(f);
            return;
        }

        var sequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanPu = f.YanPu!with { Stage = YanPuStage.EndingGiftChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(selected, CardLocation.Hand(r.Target), new("skill-program.yan-pu.huantu.ending-gift"), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, r.Target, selected, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, movements) => CommitYanPuMovement(f.Id, new(r.Movements.Count, f.OwnerSeat, "skill-program.yan-pu.huantu.ending-gift", sequence, movements.Max(m => m.Sequence), movements.Count))));
        ContinueYanPu(f.Id);
    }

    internal void CommitYanPuMovement(long id, YanPuMovement movement)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.YanPu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { YanPu = r with { Movements = r.Movements.Append(movement).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanPuMovementIssuedEvent(id, movement));
    }

    internal void IssueYanPuDraw(ProgramSkillFrame f, int subject, int count, YanPuStage stage)
    {
        var r = f.YanPu!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var reason = stage == YanPuStage.ExitDrawChildren ? "skill-program.yan-pu.bihuo.draw" : "skill-program.yan-pu.huantu.draw";
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanPu = r with { Stage = stage }, PendingMovementContinuation = new(subject, 0, null) });
        var actual = _coreMatchParticipantsPlayersAccess._players[subject].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[subject], count, true, new(reason)).Count : 0;
        CommitYanPuMovement(f.Id, new(r.Movements.Count, subject, reason, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, actual));
        ContinueYanPu(f.Id);
    }

    internal bool DrainYanPuChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueYanPu(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { YanPu: { } r } f && f.Id == id)
        {
            AssertYanPu(f);
            if (r.Stage is YanPuStage.GiftChoice or YanPuStage.EndingChoice or YanPuStage.EndingGiftChoice)
            {
                if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Stage != YanPuStage.EndingChoice && !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
                {
                    CompleteYanPu(f, r.Promise is not null);
                    return;
                }

                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishYanPuChoice(f);
                return;
            }

            if (DrainYanPuChildren(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            r = f.YanPu!;
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CompleteYanPu(f, true);
                return;
            }

            switch (r.Stage)
            {
                case YanPuStage.GiftChildren:
                    CompleteYanPu(f, true);
                    return;
                case YanPuStage.RecoveryChildren:
                    IssueYanPuDraw(f, r.Target, 2, YanPuStage.RecipientDrawChildren);
                    return;
                case YanPuStage.RecipientDrawChildren:
                case YanPuStage.EndingGiftChildren:
                    CompleteYanPu(f, true);
                    return;
                case YanPuStage.OwnerDrawChildren:
                    var n = Math.Min(2, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count);
                    if (n == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
                    {
                        CompleteYanPu(f, true);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanPu = r with { Stage = YanPuStage.EndingGiftChoice, RequiredGift = n } });
                    PublishYanPuChoice(f);
                    return;
                case YanPuStage.ExitDrawChildren:
                    if (_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanPuRoundDistanceIssuedEvent(f.Id, r.Source, r.GameplayHash, r.Round, r.Turn, r.Exited!.DyingFrameId, r.Target));
                    CompleteYanPu(f, true);
                    return;
                default:
                    throw new InvalidOperationException("Unknown YanPu paid continuation.");
            }
        }
    }

    internal void CompleteYanPu(ProgramSkillFrame f, bool applied)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YanPuCompletedEvent(f.Id, applied));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YanPu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, applied);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26410)]
    internal bool ResumeYanPu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YanPu: not null } f || f.Id != id)
            return false;
        ContinueYanPu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26410)]
    internal bool ReturnYanPuMovement(ProgramSkillFrame f)
    {
        if (f.YanPu is null)
            return false;
        ContinueYanPu(f.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 660)]
    internal bool IsYanPuAwaitedMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.YanPu is { Stage: not (YanPuStage.GiftChoice or YanPuStage.EndingChoice or YanPuStage.EndingGiftChoice), Movements.Count: > 0 } r && (e is null || YanPuComposition.IsOperation(e.Op)) && p.BeforeCount == 0 && p.CoverageResultBind is null && p.SubjectSeat == r.Movements[^1].SubjectSeat;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26410)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26410)]
    internal bool ContinuePaidYanPu(ProgramSkillFrame f)
    {
        if (f.YanPu is { } r)
            return (r.Promise is not null || r.Exited is not null) && _coreProgramEventHistoryOperations.ProgramEventHistory<YanPuStartedEvent>().Any(e => e.ProgramFrameId == f.Id && e.Source == r.Source);
        // An issued Ending must also be allowed to enter its first instruction
        // after the original skill instance disappeared. Its authority is the
        // exact unconsumed gift item, not the absent source or a parallel debt.
        if (f.TriggerId is null || f.InstructionIndex is < 0 or > 1 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.WindowContext is not { } context)
            return false;
        return YanPuCurrentEndingFrameCandidate(f)is { } c && CurrentYanPuEnding(c, context)is { } p && !YanPuPromiseConsumed(p.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.GiveCardSkipDrawForEnding, SkillProgramEffectOp.ResolveHuantuEnding, SkillProgramEffectOp.DrawDyingExitRoundDistance)]
    internal PromptChoice ChooseAiYanPu(PendingDecision d, ProgramSkillFrame f) => d.Choices.FirstOrDefault(c => c.Parameters["option"] == (_coreMatchParticipantsPlayersAccess._players[f.YanPu!.Target].IsAlive ? "recover-draw" : "draw-give")) ?? d.Choices.OrderBy(c => c.Cards.Count == 0 ? 0 : global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
