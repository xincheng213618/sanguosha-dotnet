#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOldHandDispositionRuntime
{
    internal const string OldHandDispositionReason = "program.old-hand-disposition";
    internal int[] OldHandDispositionIds(int owner, int turn) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(card => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card.Id && m.TurnNumber == turn && m.To == CardLocation.Hand(owner) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m) != CardLocation.Hand(owner))).Select(c => c.Id).Order().ToArray();
    internal bool OldHandDispositionCurrent(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(p => p.Id == f.SkillId && p.GameplayHash == f.GameplayHash);
    internal SkillProgramStepOutcome ReplaceDiscardWithOldHandDisposition(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!_coreHalfHandPhaseDebtProgramsOperations.HalfHandDebtLifecycleMatches(f, SkillProgramTriggerWindow.DiscardPhaseStarting) || _coreActualTurnProgressPhaseTurnNumberAccess._phase != TurnPhase.Discard || f.OldHandDisposition is not null)
            throw new InvalidOperationException("Old-hand replacement lost its exact initial discard parent.");
        var r = new OldHandDispositionReceipt
        {
            ActualTurn = _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber,
            ParentFrameId = f.WindowContext!.ParentFrameId,
            InstructionIndex = f.InstructionIndex,
            Source = new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            OldIds = OldHandDispositionIds(f.OwnerSeat, _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OldHandDisposition = r });
        _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDiscardPhase;
        if (r.OldIds.Count == 0 || !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat))
        {
            FinishOldHandDisposition(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishOldHandDisposition(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static Dictionary<string, string> OldHandParameters(ProgramSkillFrame f, string branch) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OldHandDisposition.OldHandParameters(f, branch);
    internal int[] RemainingOldHandIds(ProgramSkillFrame f) => f.OldHandDisposition!.OldIds.Except(f.OldHandDisposition.DisposedIds).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat)).ToArray();
    internal IReadOnlyList<PromptChoice> OldHandChoices(ProgramSkillFrame f)
    {
        var r = f.OldHandDisposition!;
        var others = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).OrderBy(p => p.Seat).ToArray();
        var choices = new List<PromptChoice>();
        if (r.Stage == "mode")
        {
            choices.Add(new(new($"old-hand.{(f.Id)}.give"), "将所有旧手牌分配给其他角色", [], [], OldHandParameters(f, "give")));
            choices.Add(new(new($"old-hand.{(f.Id)}.discard"), "弃置旧手牌，再弃置一名其他角色至多等量张牌", [], [], OldHandParameters(f, "discard")));
        }
        else if (r.Stage == "give")
            foreach (var id in RemainingOldHandIds(f))
                foreach (var p in others)
                    choices.Add(new(new($"old-hand.{(f.Id)}.{(id)}.{(p.Seat)}"), $"将【{(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == id).DisplayName)}】交给 {(p.Name)}", [id], [p.Seat], OldHandParameters(f, "card-give")));
        else if (r.Stage == "target")
            foreach (var p in others)
                choices.Add(new(new($"old-hand.{(f.Id)}.target.{(p.Seat)}"), $"弃置 {(p.Name)} 的牌（至多{(r.ActualDiscardCount)}张）", [], [p.Seat], OldHandParameters(f, "target")));
        else if (r.Stage == "foreign" && r.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && r.ForeignDiscardCount < r.ActualDiscardCount)
            choices.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard).Select(c =>
            {
                var ps = c.Parameters.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                ps["program-action"] = "old-hand-disposition";
                ps["branch"] = "foreign-discard";
                return new PromptChoice(c.Id, c.Description, c.Cards, c.Targets, ps);
            }));
        if (r.Stage is "target" or "foreign")
            choices.Add(new(new($"old-hand.{(f.Id)}.finish"), "结束弃牌（至多等量）", [], [], OldHandParameters(f, "finish")));
        return r.Stage == "give" ? _coreCardDistributionPresentationOperations.PresentCardDistribution(f, choices, "old-hand") : Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishOldHandDisposition(ProgramSkillFrame f)
    {
        if (!OldHandDispositionCurrent(f))
        {
            FinishOldHandDisposition(f);
            return;
        }

        var choices = OldHandChoices(f);
        if (choices.Count == 0)
        {
            FinishOldHandDisposition(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "素俭：处理非本回合获得的手牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("old-hand-disposition")]
    internal void ResolveOldHandDisposition(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OldHandDisposition: not null } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !OldHandDispositionCurrent(f) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], OldHandChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Old-hand disposition lost its current exact private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var r = f.OldHandDisposition!;
        switch (selected.Parameters["branch"])
        {
            case "give":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OldHandDisposition = r with { Stage = "give" } });
                PublishOldHandDisposition(f);
                return;
            case "discard":
                PayOldHandDisposition(f, RemainingOldHandIds(f), CardLocation.DiscardPile, "target", null);
                return;
            case "card-give":
                PayOldHandDisposition(f, selected.Cards, CardLocation.Hand(selected.Targets.Single()), "give", selected.Targets.Single());
                return;
            case "target":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OldHandDisposition = r with { Stage = "foreign", TargetSeat = selected.Targets.Single() } });
                PublishOldHandDisposition(f);
                return;
            case "foreign-discard":
                if (r.TargetSeat is not { } victim || !Enum.TryParse<CardZoneKind>(selected.Parameters.GetValueOrDefault("source-zone"), out var zone) || !int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), out var slot))
                    throw new InvalidOperationException("Old-hand foreign discard lost its current source slot.");
                var cards = _cardZonesOperations._cardZones.CardsAt(new(zone, victim));
                if (slot < 0 || slot >= cards.Count)
                    throw new InvalidOperationException("Foreign discard slot no longer exists.");
                PayOldHandDisposition(f, [cards[slot].Id], CardLocation.DiscardPile, "foreign", victim);
                return;
            case "finish":
                FinishOldHandDisposition(f);
                return;
            default:
                throw new InvalidOperationException("Unknown old-hand disposition branch.");
        }
    }

    internal void PayOldHandDisposition(ProgramSkillFrame f, IReadOnlyList<int> ids, CardLocation destination, string next, int? target)
    {
        var r = f.OldHandDisposition!;
        if (ids.Count == 0)
        {
            FinishOldHandDisposition(f);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OldHandDisposition = r with { Stage = "movement", ReturnStage = next, TargetSeat = target, PaidIds = ids, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var logicalDiscard = destination == CardLocation.DiscardPile ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, ids, new(OldHandDispositionReason), f.Id) : null;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, new(OldHandDispositionReason), (next == "give" ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, target!.Value, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = active.OldHandDisposition!;
            var actual = records.Count(m => m.To == destination || next == "foreign" && m.To == CardLocation.OutsideGame);
            paid = paid with
            {
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                DisposedIds = next is "give" or "target" ? paid.DisposedIds.Concat(ids).Distinct().ToArray() : paid.DisposedIds,
                ActualDiscardCount = next == "target" ? actual : paid.ActualDiscardCount,
                ForeignDiscardCount = next == "foreign" ? paid.ForeignDiscardCount + actual : paid.ForeignDiscardCount
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { OldHandDisposition = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OldHandDispositionPaymentEvent(f.Id, paid.Source, paid.GameplayHash, paid.ActualTurn, next, target, ids, before, paid.After, actual));
        }) : (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = active.OldHandDisposition!;
            var actual = records.Count(m => m.To == destination || next == "foreign" && m.To == CardLocation.OutsideGame);
            paid = paid with
            {
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                DisposedIds = next is "give" or "target" ? paid.DisposedIds.Concat(ids).Distinct().ToArray() : paid.DisposedIds,
                ActualDiscardCount = next == "target" ? actual : paid.ActualDiscardCount,
                ForeignDiscardCount = next == "foreign" ? paid.ForeignDiscardCount + actual : paid.ForeignDiscardCount
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { OldHandDisposition = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OldHandDispositionPaymentEvent(f.Id, paid.Source, paid.GameplayHash, paid.ActualTurn, next, target, ids, before, paid.After, actual));
        }));
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
    }

    internal SkillProgramEffect? OldHandPausedEffect(ProgramSkillFrame f) => f.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 260)]
    internal bool IsOldHandDispositionMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ReplaceDiscardWithOldHandDisposition && f.OldHandDisposition is { Stage: "movement" } r && r.InstructionIndex == f.InstructionIndex && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Movement, 21701)]
    internal bool ReturnOldHandDispositionMovement(ProgramSkillFrame f)
    {
        if (f.OldHandDisposition is not { } r)
            return false;
        if (f.PendingMovementContinuation is not { } p || !IsOldHandDispositionMovement(f, OldHandPausedEffect(f), p))
            throw new InvalidOperationException("Old-hand disposition consumed an unrelated movement return.");
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd() || _coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, OldHandDisposition = r with { Stage = r.ReturnStage } });
        if (!OldHandDispositionCurrent(f) || r.ReturnStage == "give" && RemainingOldHandIds(f).Length == 0 || r.ReturnStage == "target" && r.ActualDiscardCount == 0 || r.ReturnStage == "foreign" && (r.ForeignDiscardCount >= r.ActualDiscardCount || r.TargetSeat is not { } target || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive))
            FinishOldHandDisposition(f);
        else
            PublishOldHandDisposition(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 21701)]
    internal bool ResumeOldHandDisposition(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OldHandDisposition: not null } f || f.Id != id)
            return false;
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd())
            return true;
        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnOldHandDispositionMovement(f);
            return true;
        }

        if (!OldHandDispositionCurrent(f))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishOldHandDisposition(f);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishOldHandDisposition(f);
        return true;
    }

    internal void FinishOldHandDisposition(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OldHandDisposition = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ReplaceDiscardWithOldHandDisposition)]
    internal PromptChoice SelectAiOldHandDisposition(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.OldHandDisposition!;
        if (r.Stage == "mode")
            return decision.Choices.First(c => c.Parameters["branch"] == (_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], p)) ? "give" : "discard"));
        if (r.Stage == "give")
            return decision.Choices.OrderByDescending(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])).ThenBy(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        if (r.Stage == "target")
            return decision.Choices.Where(c => c.Targets.Count == 1).OrderBy(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])).ThenBy(c => c.Targets.Single()).First();
        return decision.Choices.FirstOrDefault(c => c.Parameters["branch"] == "foreign-discard") ?? decision.Choices.First();
    }

    SkillProgramStepOutcome global::CardGame.Core.IOldHandDispositionHost.ReplaceDiscardWithOldHandDisposition(ProgramSkillFrame f) => ReplaceDiscardWithOldHandDisposition(f);
}
