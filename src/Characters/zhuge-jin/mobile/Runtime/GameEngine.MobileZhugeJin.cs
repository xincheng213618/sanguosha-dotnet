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
