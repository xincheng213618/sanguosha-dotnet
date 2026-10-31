#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiAnniversaryGodRuntime
{
    internal bool CanBeginShenZfJudgment(ProgramSkillFrame f, int targetSeat, string reason, string bind, SkillProgramCardSetVisibility visibility) => f.ShenZhangFei is { Operation: SkillProgramEffectOp.ResolveShenZhangFeiShencai, Stage: ShenZhangFeiStage.Judging, JudgmentId: > 0 } r && f.ActivationId == "sentence" && f.TriggerId is null && ShenZhangFeiComposition.IsShencai(f.SkillId) && targetSeat == r.TargetSeat && reason == "anniversary-shencai" && bind == "shencai-judgment" && visibility == SkillProgramCardSetVisibility.Public && r.Source == ShenZfSource(f) && r.GameplayHash == f.GameplayHash && f.SelectedTargetSeats.SequenceEqual([targetSeat]);
    internal bool IsShenZfJudgment(JudgmentFrame j, ProgramSkillFrame f) => CanBeginShenZfJudgment(f, j.TargetSeat, j.Reason, j.ProgramResultBind ?? "", j.ProgramResultVisibility ?? SkillProgramCardSetVisibility.Private) && f.ShenZhangFei!.JudgmentId == j.Id && j.ParentFrameId == f.Id && j.SourceSeat == f.OwnerSeat && j.Continuation == JudgmentContinuationKind.ProgramSkill;
    internal ProgramJudgmentTriggerCandidate[] PrependShenZfJudgmentGain(JudgmentFrame j, ProgramJudgmentTriggerCandidate[] candidates)
    {
        // The innate disposition is issued by the paid parent. It runs before
        // final-judgment skills such as Tiandu, regardless of table order.
        var retained = candidates.Where(c => !ShenZhangFeiComposition.IsShencai(c.SkillId) || c.TriggerId != "judgment-first-gain").ToArray();
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == j.ParentFrameId)is not { } root || !IsShenZfJudgment(j, root) || !_coreMatchParticipantsPlayersAccess._players[root.OwnerSeat].IsAlive || _coreNativeDamageHandlesOperations.GetJudgmentCard(j)is not { } card || _cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Judgment(j.TargetSeat))
            return retained;
        return[new(root.OwnerSeat, root.SkillId, "judgment-first-gain", root.SkillInstanceId, root.GameplayHash), ..retained];
    }

    internal bool IsShenZfGainCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (!ShenZhangFeiComposition.IsShencai(c.SkillId) || c.BindingId != "judgment-first-gain" || w.Window != SkillProgramTriggerWindow.JudgmentFinalized || w.Judgment is not { } result || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _coreJudgmentCursorRuntimeOperations.ActiveJudgment is not { } j || j.Id != result.JudgmentFrameId || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == j.ParentFrameId)is not { } root || !IsShenZfJudgment(j, root) || root.OwnerSeat != c.OwnerSeat || root.SkillInstanceId != c.SkillInstanceId || root.GameplayHash != c.GameplayHash || result.SubjectSeat != j.TargetSeat || result.CardId != _coreNativeDamageHandlesOperations.GetJudgmentCard(j)?.Id || _cardZonesOperations._cardZones.GetLocation(result.CardId) != CardLocation.Judgment(j.TargetSeat))
            return false;
        return _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { CandidateIndex: 0 } p && p.ParentFrameId == j.Id && p.Candidates.Count > 0 && p.Candidates[0] == new ProgramJudgmentTriggerCandidate(c.OwnerSeat, c.SkillId, c.BindingId, c.SkillInstanceId, c.GameplayHash) && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiJudgmentGainEvent>().Any(e => e.JudgmentId == j.Id);
    }

    internal void ClaimShenZfJudgment(ProgramSkillFrame f)
    {
        if (!IsShenZfGainCandidate(ShenZfFrameCandidate(f), f.WindowContext!))
            throw new InvalidOperationException("Shencai can claim only its own original final judgment once.");
        var j = _coreJudgmentCursorRuntimeOperations.ActiveJudgment!;
        var root = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == j.ParentFrameId);
        var card = _coreNativeDamageHandlesOperations.GetJudgmentCard(j)!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = f.ShenZhangFei!with { RootId = root.Id, JudgmentId = j.Id } });
        MoveShenZfCard(f, card, CardLocation.Judgment(j.TargetSeat), CardLocation.Hand(f.OwnerSeat), ShenZhangFeiStage.GainChildren, "judgment-gain");
        var paid = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).ShenZhangFei!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiJudgmentGainEvent(root.Id, f.Id, j.Id, f.OwnerSeat, j.TargetSeat, card.Id, paid.After));
        ContinueShenZf(f.Id);
    }

    internal bool HasShenZfJudgmentGainDisposition(JudgmentFrame j)
    {
        var claims = _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiJudgmentGainEvent>().Where(e => e.JudgmentId == j.Id).ToArray();
        if (claims is not [var e] || e.RootId != j.ParentFrameId || e.TargetSeat != j.TargetSeat || e.CardId != _coreNativeDamageHandlesOperations.GetJudgmentCard(j)?.Id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == e.RootId)is not { } root || !IsShenZfJudgment(j, root) || root.OwnerSeat != e.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().SingleOrDefault(w => w.ParentFrameId == j.Id)is not { } window || window.Candidates.Count == 0 || window.Candidates[0] != new ProgramJudgmentTriggerCandidate(root.OwnerSeat, root.SkillId, "judgment-first-gain", root.SkillInstanceId, root.GameplayHash))
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.CardId == e.CardId && m.Sequence >= e.Sequence).ToArray();
        if (moves is not [var first, ..] || first.Sequence != e.Sequence || first.From != CardLocation.Judgment(j.TargetSeat) || first.To != CardLocation.Hand(e.OwnerSeat) || first.Reason.Value != ShenZfReason + "judgment-gain" || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiMovementPaidEvent>().Count(p => p.FrameId == e.ClaimId && p.CardId == e.CardId && p.After == e.Sequence && p.From == first.From && p.To == first.To) != 1)
            return false;
        var location = first.To;
        foreach (var movement in moves.Skip(1))
        {
            if (movement.From != location)
                return false;
            location = movement.To;
        }

        var child = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == e.ClaimId);
        return _cardZonesOperations._cardZones.GetLocation(e.CardId) == location && (child is { ShenZhangFei: { Stage: ShenZhangFeiStage.GainChildren, Paid: true } r } && r.RootId == root.Id && r.JudgmentId == j.Id && r.CardId == e.CardId && r.After == e.Sequence && child.WindowContext?.ParentFrameId == window.Id || child is null && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingResolvedEvent>().Any(p => p.FrameId == e.ClaimId && p.Completed && p.BindingId == "judgment-first-gain" && p.OwnerSeat == e.OwnerSeat));
    }

    internal bool IsExactShenZfJudgmentGainSubtree(ProgramJudgmentTriggerWindowFrame window, JudgmentFrame? judgment, int index)
    {
        if (judgment is null || !window.Activated || window.ZhenyiBeforeResult == true || index < 2 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index] != window || _resolutionStackOperations._resolutionStack[index - 1] != judgment || window.ParentFrameId != judgment.Id || window.CandidateIndex != 0 || _resolutionStackOperations._resolutionStack[index - 2] is not ProgramSkillFrame root || !IsShenZfJudgment(judgment, root) || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramSkillFrame { ShenZhangFei: { Stage: ShenZhangFeiStage.GainChildren, Paid: true } r } child || r.RootId != root.Id || r.JudgmentId != judgment.Id || child.OwnerSeat != root.OwnerSeat || child.SkillId != root.SkillId || child.SkillInstanceId != root.SkillInstanceId || child.GameplayHash != root.GameplayHash || child.TriggerId != "judgment-first-gain" || child.WindowContext is not { Window: SkillProgramTriggerWindow.JudgmentFinalized } context || context.ParentFrameId != window.Id || context.Judgment != window.Judgment || window.Candidates.Count == 0 || window.Candidates[0] != new ProgramJudgmentTriggerCandidate(child.OwnerSeat, child.SkillId, child.TriggerId, child.SkillInstanceId, child.GameplayHash) || !HasShenZfJudgmentGainDisposition(judgment) || child.PendingMovementContinuation is not { } pending || !ShenZfMovementProof(child, ProgramInstructionResolver.Default.Resolve(child, _contentRegistryOperations._contentRegistry.GetSkill(child.SkillId).Program!).Instructions[0], pending))
            return false;
        if (index + 2 == _resolutionStackOperations._resolutionStack.Count)
            return true;
        if (_resolutionStackOperations._resolutionStack[index + 2] is not CardsMovedTriggerWindowFrame moved || moved.ResumeProgramFrameId is not null || moved.Batch.ParentFrameId != child.Id || moved.Batch.AwaitingProgramFrameId != child.Id || !moved.Batch.Movements.Any(m => m.Sequence == r.After && m.CardId == r.CardId && m.From == r.From && m.To == r.To && m.Reason.Value == ShenZfReason + "judgment-gain"))
            return false;
        // The first real gain batch is proved above; keep the existing exact
        // movement, HP, rescue and nested observer edges for its whole suffix.
        return _coreObserverSubtreesOperations.HasMovementObserverSuffix(index + 1);
    }

    internal bool HasUnclaimedShenZfJudgmentResult(ProgramSkillFrame f, int cardId, long before)
    {
        if (f.ShenZhangFei is not { Operation: SkillProgramEffectOp.ResolveShenZhangFeiShencai, ResultCardId: { } resultId, ResultKind: { } kind } r || f.TriggerId is not null || f.ActivationId != "sentence" || resultId != cardId || _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Count(e => e.ResolutionId == r.JudgmentId && e.ParentResolutionId == f.Id && e.TargetSeat == r.TargetSeat && e.Reason == "anniversary-shencai" && e.CardId == cardId && e.CardKind == kind) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiJudgmentReturnedEvent>().Count(e => e == new ShenZhangFeiJudgmentReturnedEvent(f.Id, r.JudgmentId, cardId, kind)) != 1)
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cardId && m.Sequence <= before)is { } move && move.From == CardLocation.Judgment(r.TargetSeat) && move.To == CardLocation.Processing && move.Reason.Value == "skill-program.judgment.result";
    }

    internal bool TryReturnShenZfJudgment(JudgmentFrame j)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenZhangFei: { Stage: ShenZhangFeiStage.Judging } r } f || f.Id != j.ParentFrameId)
            return false;
        if (!IsShenZfJudgment(j, f) || _resolutionStackOperations._resolutionStack.Any(p => p.Id == j.Id))
            throw new InvalidOperationException("Shencai lost its complete native judgment return.");
        var results = _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Where(e => e.ResolutionId == j.Id && e.ParentResolutionId == f.Id && e.TargetSeat == r.TargetSeat && e.Reason == "anniversary-shencai").ToArray();
        if (results is not [var result])
            throw new InvalidOperationException("Shencai requires exactly one real final judgment outcome.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r with { Stage = ShenZhangFeiStage.JudgmentReturned, ResultCardId = result.CardId, ResultKind = result.CardKind } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiJudgmentReturnedEvent(f.Id, j.Id, result.CardId, result.CardKind));
        ContinueShenZf(f.Id);
        return true;
    }
}
