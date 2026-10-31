#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeQiMobileShiRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IShiHeQiHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShiHq(f, op);
    internal SkillProgramStepOutcome BeginShiHq(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShiHeQi is not null || f.InstructionIndex != 1 || f.SkillId != ShiHqQizhou || f.WindowContext is not { } w || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ } e] || e.Op != op || op == SkillProgramEffectOp.ResolveMobileShiHeQiQizhouMark && (f.TriggerId != "field-entry-mark" || w.MovementBatch is not { } batch || !batch.Movements.Any(m => ShiHqEntersField(m, f.OwnerSeat))) || op == SkillProgramEffectOp.ResolveMobileShiHeQiQizhouDraw && (f.TriggerId != "draw-extra-yingji" || w.Window != SkillProgramTriggerWindow.DrawPhaseStarting || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat) || op == SkillProgramEffectOp.ResolveMobileShiHeQiYingjiCleanup && !IsIssuedShiHqCleanupCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, w.OccurrenceIndex), w))
            throw new InvalidOperationException("Qizhou requires its original actual field entry, draw phase or issued whole-turn cleanup.");
        var stage = op == SkillProgramEffectOp.ResolveMobileShiHeQiQizhouMark ? ShiHqStage.Target : op == SkillProgramEffectOp.ResolveMobileShiHeQiQizhouDraw ? ShiHqStage.Drawing : ShiHqStage.CleanupChildren;
        var r = new ShiHqReceipt(ShiHqSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, w.ParentFrameId, op, stage)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHeQi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.ParentId, op));
        if (stage == ShiHqStage.Target)
        {
            PublishShiHq(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (stage == ShiHqStage.Drawing)
        {
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(ShiHqDrawReason));
            return SkillProgramStepOutcome.AwaitChild;
        }

        var materials = _coreMatchParticipantsPlayersAccess._players.SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Where(c => HasShiHqYingji(c.Id)).Select(c => new ShiHqMaterial(c.Id, c.Kind, CardLocation.Hand(p.Seat)))).ToArray();
        var marks = materials.ToDictionary(m => m.CardId, m => ShiHqMark(m.CardId)!);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHeQi = r with { Materials = materials }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        // This is the marked-card cleanup rule, not a logical discard or cost.
        if (materials.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(materials.Select(m => m.CardId).ToArray(), CardLocation.DiscardPile, new(ShiHqCleanupReason), disclosure: CardMovementDisclosure.FaceUp);
        var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && materials.Any(c => c.CardId == m.CardId && c.From == m.From)).ToArray();
        foreach (var movement in paid)
        {
            var material = materials.Single(m => m.CardId == movement.CardId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqCleanupMaterialEvent(f.Id, material, movement.Sequence));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqYingjiClearedEvent(marks[material.CardId].MarkId, f.Id, material.CardId, movement.Sequence));
        }

        var paidBatch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.SingleOrDefault(b => b.ParentFrameId == f.Id && b.ParentBatchId is null && paid.Any(m => b.Movements.Contains(m)))?.Id;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiHeQi = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).ShiHeQi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, BatchId = paidBatch } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqCleanupIssuedEvent(f.Id, r.Turn, r.TurnOwner, materials.Length, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueShiHq(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ShiHqChoices(ProgramSkillFrame f)
    {
        var r = f.ShiHeQi!;
        if (r.Stage == ShiHqStage.Target)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).Select(p => new PromptChoice(new($"shi-he-qi.{(f.Id)}.target.{(p.Seat)}"), $"将 {(p.Name)} 的一张手牌标记为应机牌", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "mobile-shi-he-qi", ["branch"] = "target" })).ToArray());
        if (r.Stage != ShiHqStage.Card || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            return[];
        return Array.AsReadOnly(r.Materials.Select((m, i) => (m, i)).Where(x => _cardZonesOperations._cardZones.GetLocation(x.m.CardId) == x.m.From).Select(x => new PromptChoice(new($"shi-he-qi.{(f.Id)}.slot.{(x.i)}"), r.Target == f.OwnerSeat ? $"标记 {(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Single(c => c.Id == x.m.CardId).DisplayName)}" : $"标记第 {(x.i + 1)} 张手牌", r.Target == f.OwnerSeat ? [x.m.CardId] : [], [r.Target], new Dictionary<string, string> { ["program-action"] = "mobile-shi-he-qi", ["branch"] = "card", ["slot"] = x.i.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal void PublishShiHq(ProgramSkillFrame f)
    {
        var choices = ShiHqChoices(f);
        if (choices.Count == 0)
        {
            FinishShiHq(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.ShiHeQi!.Stage == ShiHqStage.Target ? "绮胄：选择一名角色。" : "绮胄：选择其一张手牌标记为应机牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mobile-shi-he-qi")]
    internal void AnswerShiHq(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiHeQi: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiHqChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Qizhou changed its current private target or opaque hand-slot selection.");
        RequireShiHq(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == ShiHqStage.Target)
        {
            var target = choice.Targets.Single();
            var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => new ShiHqMaterial(c.Id, c.Kind, CardLocation.Hand(target))).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHeQi = r with { Stage = ShiHqStage.Card, Target = target, Materials = cards } });
            PublishShiHq(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var material = r.Materials[int.Parse(choice.Parameters["slot"], CultureInfo.InvariantCulture)];
        var reward = _coreProgramPersistentStateOperations.GetProgramBooleanState(f, "next-mark-draw");
        // Consume the previous obligation before this successful invocation can
        // sign its next one. A selected Slash therefore cannot reward itself.
        _coreProgramSkillChangesOperations.SetProgramBooleanState(f, "next-mark-draw", false);
        MarkShiHqEntity(f, material.CardId, r.Target, ShiHqMarkOrigin.SelectedHand);
        var next = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(material.Kind);
        if (next)
            _coreProgramSkillChangesOperations.SetProgramBooleanState(f, "next-mark-draw", true);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqSelectedEvent(f.Id, r.Target, material.CardId, reward, next));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { Step = ResolutionFrameStep.ResolvingEffect, ShiHeQi = r with { Stage = reward ? ShiHqStage.Drawing : ShiHqStage.Complete, Reward = reward, Materials = [material], Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (reward)
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(ShiHqDrawReason));
        else
            FinishShiHq(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void MarkShiHqEntity(ProgramSkillFrame f, int id, int holder, ShiHqMarkOrigin origin)
    {
        if (_cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(holder))
            throw new InvalidOperationException("Yingji marking requires the exact actual hand entity.");
        // Re-marking the same physical card does not create a second cleanup.
        if (!HasShiHqYingji(id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqYingjiMarkedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, f.Id, ShiHqSource(f), f.GameplayHash, id, holder, origin));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiHeQiQizhouMark, SkillProgramEffectOp.ResolveMobileShiHeQiQizhouDraw)]
    internal void ReturnShiHqDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.ShiHeQi is not { Stage: ShiHqStage.Drawing, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 2 || r.Operation == SkillProgramEffectOp.ResolveMobileShiHeQiQizhouMark && !r.Reward)
            throw new InvalidOperationException("Qizhou lost its one original two-card native draw return.");
        foreach (var id in outcome.ActualDrawIds)
        {
            if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ShiHqDrawReason))
                throw new InvalidOperationException("Yingji can mark only this exact native draw's actual obtained entities.");
            MarkShiHqEntity(f, id, f.OwnerSeat, r.Reward ? ShiHqMarkOrigin.NextMarkReward : ShiHqMarkOrigin.ExtraDraw);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiHeQi = r with { Stage = ShiHqStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqDrawPaidEvent(f.Id, outcome.ActualDrawIds.Count, outcome.Replacement is not null, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueShiHq(f.Id);
    }

    internal void ContinueShiHq(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiHeQi: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        RequireShiHq(f);
        if (r.Stage is ShiHqStage.Target or ShiHqStage.Card)
        {
            PublishShiHq(f);
            return;
        }

        if (r.Stage == ShiHqStage.Drawing)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        FinishShiHq(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishShiHq(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHeQi = f.ShiHeQi!with { Stage = ShiHqStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqCompletedEvent(f.Id, f.ShiHeQi!.Operation));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 49000)]
    internal bool ResumeShiHq(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiHeQi: not null } f || f.Id != id)
            return false;
        ContinueShiHq(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49000)]
    internal bool ReturnShiHqMovement(ProgramSkillFrame f)
    {
        if (f.ShiHeQi is null)
            return false;
        ContinueShiHq(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 49000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 49000)]
    internal bool KeepPaidShiHq(ProgramSkillFrame f) => f.ShiHeQi is { Stage: ShiHqStage.Drawing or ShiHqStage.DrawChildren or ShiHqStage.CleanupChildren or ShiHqStage.Complete } || f.SkillId == ShiHqQizhou && f.ShiHeQi is null && f.WindowContext is { } w && IsIssuedShiHqCleanupCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, w.OccurrenceIndex), w);
    internal bool ShiHqMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.ShiHeQi is { } r && effect?.Op == r.Operation && r.Stage is ShiHqStage.DrawChildren or ShiHqStage.CleanupChildren && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiHeQiQizhouMark)]
    internal PromptChoice ChooseShiHqAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Targets.Contains(f.OwnerSeat)) ?? p.Choices.First();
    internal void RequireShiHq(ProgramSkillFrame f)
    {
        var r = f.ShiHeQi ?? throw new InvalidOperationException("Qizhou lost its owning receipt.");
        if (r.Source != ShiHqSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || f.InstructionIndex != 1 || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.ParentId != f.WindowContext?.ParentFrameId || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHqStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Hash == r.Hash && e.Turn == r.Turn && e.TurnOwner == r.TurnOwner && e.ParentId == r.ParentId && e.Operation == r.Operation) != 1)
            throw new InvalidOperationException("Qizhou changed its original source, actual turn, native instruction or paid cursor.");
    }
}
