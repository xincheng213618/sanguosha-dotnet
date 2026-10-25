namespace CardGame.Core;

internal static class MobileZhugeJinComposition
{
    internal static bool IsSupportTargetShape(SkillProgramTargetKind kind, int minimum, int maximum, SkillProgramNumberExpression? expression,
        string? bind, SkillProgramTargetAiOrder order) => kind == SkillProgramTargetKind.OtherLiving && minimum == 0 && maximum == 2 && expression is null && bind is null && order == SkillProgramTargetAiOrder.SupportDraw;
    internal static bool AllowsEmptyHongyuanDraw(IReadOnlyList<SkillProgramEffect> effects, int index, SkillProgramTriggerWindow? window) =>
        index == 2 && window == SkillProgramTriggerWindow.DrawPhaseStarting &&
        effects is [{ Op: SkillProgramEffectOp.AdjustNormalDraw, Target: SkillProgramEffectTarget.Owner, Amount: -1, NumberExpression: null },
            { Op: SkillProgramEffectOp.SelectTargets, Target: SkillProgramEffectTarget.Owner, TargetKind: SkillProgramTargetKind.OtherLiving, MinimumTargets: 0, MaximumTargets: 2,
                TargetAiOrder: SkillProgramTargetAiOrder.SupportDraw, NumberExpression: null, SourceBind: null, ReplaceSelection: null },
            { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTargets, Amount: 1, NumberExpression: null, SourceBind: null, ResultBind: null }] &&
        effects.All(e => e.Condition.Kind == SkillProgramConditionKind.Always);
    internal static bool IsHongyuanDrawCursor(ProgramSkillFrame f, ProgramExecutionPlan plan) =>
        f.SkillId == "mobile:hongyuan" && f.TriggerId == "reduce-and-support" && f.ActivationId == f.TriggerId && f.InstructionIndex == 3 &&
        f.WindowContext is { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context && context.OwnerSeat == f.OwnerSeat &&
        f.SelectedTargetSeats.Count <= 2 && f.SelectedTargetSeats.Distinct().Count() == f.SelectedTargetSeats.Count && !f.SelectedTargetSeats.Contains(f.OwnerSeat) &&
        f.NamedBoundTargetSelection is null && f.OwnedCardSelection is null && plan.BindingId == f.TriggerId &&
        AllowsEmptyHongyuanDraw(plan.Instructions, f.InstructionIndex - 1, context.Window);
    internal static bool IsCommittedEmptyHongyuanDraw(ProgramSkillFrame f, ProgramExecutionPlan plan, int amount) =>
        IsHongyuanDrawCursor(f, plan) && amount == 1 && f.SelectedTargetSeats.Count == 0 && f.NativeDraw is null;
    [ProgramCatalogValidation(72300)]
    private static void Validate(string path, SkillProgram program)
    {
        var reserved = program.Triggers.Where(t => t.Effects.Any(e => e.TargetKind is { } kind && e.TargetAiOrder is { } order &&
            IsSupportTargetShape(kind, e.MinimumTargets, e.MaximumTargets, e.NumberExpression, e.SourceBind, order))).ToArray();
        if (reserved.Any(t => program.Id != "mobile:hongyuan" || t.Id != "reduce-and-support" || !t.Optional || t.Subject != SkillProgramTriggerSubject.Owner ||
            !AllowsEmptyHongyuanDraw(t.Effects, 2, t.Window)))
            throw new InvalidOperationException(path + ": zero-to-two support draw belongs to the exact native Hongyuan draw-phase binding.");
    }
}
public sealed partial class GameEngine
{
    // The actual draw-phase reduction is already committed before support begins.
    // Its original binding may finish after a beneficiary child removes the source.
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 72300)]
    private bool KeepPaidMobileZhugeJinHongyuan(ProgramSkillFrame f) =>
        f.SkillId == "mobile:hongyuan" && f.TriggerId == "reduce-and-support" && f.ActivationId == f.TriggerId &&
        f.InstructionIndex is >= 1 and <= 3 && f.WindowContext is { Window: SkillProgramTriggerWindow.DrawPhaseStarting } c && c.OwnerSeat == f.OwnerSeat &&
        _resolutionStack.SingleOrDefault(frame => frame.Id == c.ParentFrameId) is ProgramLifecycleTriggerWindowFrame
            { Window: SkillProgramTriggerWindow.DrawPhaseStarting, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase } parent && parent.OwnerSeat == f.OwnerSeat &&
        ProgramEventHistory<ProgramNormalDrawAdjustedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId &&
            e.BindingId == f.TriggerId && e.OwnerSeat == f.OwnerSeat && e.Adjustment == -1) == 1;
    private bool HasMobileMingzheRegistration => _contentRegistry.Skills.ContainsKey("mobile:mingzhe");
    private int[] OrderMobileZhugeJinHongyuanTargets(ProgramSkillFrame frame, int[] targets) =>
        frame.SkillId == "mobile:hongyuan" && frame.TriggerId == "reduce-and-support" && frame.InstructionIndex == 2
            ? targets.OrderBy(seat => RelativeTableOrder(_turnProgression.OwnerSeat, seat)).ToArray() : targets;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 72300)]
    private bool CanOfferMobileZhugeJinMingzhe(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != "mobile:mingzhe") return true;
        var c = gate.Context; var candidate = gate.Candidate;
        var parent = _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault(w => w.Id == c.ParentFrameId);
        if (parent is null || c.Window != SkillProgramTriggerWindow.CardsMoved || c.MovementBatch is not { } batch || c.MovementIndex is not { } index ||
            index < 0 || index >= batch.Movements.Count || index != candidate.OccurrenceIndex || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
            parent.Candidates[parent.CandidateIndex] != candidate || parent.Batch.Id != batch.Id || !parent.Batch.Movements.SequenceEqual(batch.Movements)) return false;
        var movement = batch.Movements[index];
        if (!_cardMovements.Contains(movement) || movement.Reason == CardMoveReasons.RecastDiscard) return false;
        var original = movement;
        if (gate.Trigger.MovementDiscardOnly)
        {
            if (GetProgramDiscardSource(movement) is not { OwnerSeat: var source, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } from || source != gate.OwnerSeat) return false;
            if (movement.From == CardLocation.Processing)
            {
                // GetProgramDiscardSource has already proved this exact native
                // Processing completion. Read only its immediately prior entry,
                // where the original owner's effective red suit was frozen.
                original = _cardMovements.LastOrDefault(m => m.CardId == movement.CardId && m.Sequence < movement.Sequence)!;
                if (original is null || original.From != from || original.To != CardLocation.Processing) return false;
            }
        }
        else
        {
            if (movement.From.OwnerSeat != gate.OwnerSeat || movement.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) return false;
            var direct = movement.Reason == CardMoveReasons.Use || movement.Reason == CardMoveReasons.Respond || movement.Reason == CardMoveReasons.Nullification || movement.Reason == CardMoveReasons.ProgramJudgmentReplace;
            var use = _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == batch.ParentFrameId && u.Action is { Type: CardActionType.Use });
            if (!direct && (use is null || !IsCardUsePaymentBatch(use, batch))) return false;
        }
        return original.RedOwnedLoss is { } loss && loss.OwnerSeat == gate.OwnerSeat && loss.MovementSequence == original.Sequence &&
            loss.EffectiveSuit is Suit.Heart or Suit.Diamond && loss.Timing.ActualTurnOwnerSeat != gate.OwnerSeat &&
            gate.Trigger.SourceZones.Contains(original.From.Zone);
    }
}
