namespace CardGame.Core;

internal interface IBoundaryGuYongHost
{ SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect); }

internal sealed class BoundaryGuYongHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IBoundaryGuYongHost>().Execute(frame, effect);
}

internal abstract class BoundaryGuYongDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new BoundaryGuYongHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, context) => context.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        var shenxing = Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing;
        reader.AllowOnly("op", "target", shenxing ? "resultBind" : "sourceBind", "condition");
        var effect = new SkillProgramEffect(Op, reader.RequiredEnum<SkillProgramEffectTarget>("target"), 0, reader.Condition(),
            resultBind: shenxing ? reader.RequiredIdentifier("resultBind") : null,
            sourceBind: shenxing ? null : reader.RequiredIdentifier("sourceBind"));
        if (effect.Target != SkillProgramEffectTarget.Owner) throw new InvalidOperationException(reader.Path + ": mobile Gu Yong requires the skill owner.");
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing
            ? [new CreateChoiceResult(effect.ResultBind!, ["same", "different"])]
            : [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding), new ReadCardSet(effect.SourceBind!)];
}
internal sealed class PrepareBoundaryGuYongShenxingDescriptor : BoundaryGuYongDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryGuYongShenxing; }
internal sealed class PrepareBoundaryGuYongBingyiDescriptor : BoundaryGuYongDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryGuYongBingyi; }

[ProgramSelectionModule(ProgramSelectionStage.PhaseLimit, 53700, SkillProgramEffectOp.PrepareBoundaryGuYongShenxing)]
internal sealed class BoundaryGuYongHpPhaseLimit : IProgramSelectionPhaseLimitProvider
{ public int? GetLimit(ProgramSelectionContext context, IProgramSelectionQueries queries) => Math.Max(0, queries.Get<IProgramSelectionTargetState>().Hp(context.OwnerSeat)); }

public sealed record BoundaryGuYongColorsFrozenEvent(long FrameId, int OwnerSeat, string SkillInstanceId, string GameplayHash, string ResultBind, string Outcome) : IGameEvent;

internal static class BoundaryGuYongComposition
{
    internal static bool AllowsEmptyRecipientDraw(IReadOnlyList<SkillProgramEffect> effects, int index, SkillProgramTriggerWindow? window) =>
        index == 3 && window == SkillProgramTriggerWindow.TurnEnding && effects is [
            { Op: SkillProgramEffectOp.SelectOwnedCards, Target: SkillProgramEffectTarget.Owner,
                NumberExpression: SkillProgramNumberExpression.AllOwnedZoneCards, ResultBind: "shown-hand", Zones: [CardZoneKind.Hand] },
            { Op: SkillProgramEffectOp.PrepareBoundaryGuYongBingyi, Target: SkillProgramEffectTarget.Owner, SourceBind: "shown-hand" },
            { Op: SkillProgramEffectOp.SelectTargets, Target: SkillProgramEffectTarget.Owner, SourceBind: "shown-hand",
                NumberExpression: SkillProgramNumberExpression.BoundCardCount, MinimumTargets: 0, MaximumTargets: 8, TargetKind: SkillProgramTargetKind.AnyLiving,
                TargetAiOrder: SkillProgramTargetAiOrder.Stable, ReplaceSelection: null },
            { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTargets, Amount: 1, NumberExpression: null, SourceBind: null, ResultBind: null }]
        && effects.All(effect => effect.Condition.Kind == SkillProgramConditionKind.Always);

    internal static bool IsCommittedEmptyRecipientDraw(ProgramSkillFrame frame, ProgramExecutionPlan plan, int amount) =>
        frame.SkillId == "boundary:bingyi-mobile" && frame.TriggerId == "ending-reveal-share" && frame.ActivationId == frame.TriggerId &&
        frame.InstructionIndex == 4 && amount == 1 && frame.SelectedTargetSeats.Count == 0 && frame.NativeDraw is null && frame.NamedBoundTargetSelection is null &&
        plan.BindingId == frame.TriggerId && AllowsEmptyRecipientDraw(plan.Instructions, frame.InstructionIndex - 1, frame.WindowContext?.Window) &&
        frame.CardSetBindings.Count(binding => binding.Name == "shown-hand" && binding.Visibility == SkillProgramCardSetVisibility.Public && binding.CardIds.Count > 0) == 1;

    [ProgramCatalogValidation(53700)]
    private static void Validate(string path, SkillProgram program)
    {
        var effects = program.Activations.SelectMany(activation => activation.Effects).Concat(program.Triggers.SelectMany(trigger => trigger.Effects)).ToArray();
        if (effects.Any(effect => effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing) && program.Id != "boundary:shenxing-mobile" ||
            effects.Any(effect => effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongBingyi) && program.Id != "boundary:bingyi-mobile")
            throw new InvalidOperationException(path + ": mobile Gu Yong preparation belongs only to its matching printed skill.");
        if (program.Id == "boundary:shenxing-mobile")
        {
            if (program.Triggers.Count != 0 || program.Activations is not [var activation] || activation.Id != "discard-pair" ||
                activation.MinCards != 2 || activation.MaxCards != 2 || activation.MinTargets != 0 || activation.MaxTargets != 0 ||
                !activation.SourceZones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment]) || activation.UsesPerTurn is not null || activation.UsesPerPhase is not null || activation.UsesPerGame is not null ||
                activation.Effects is not [{ Op: SkillProgramEffectOp.PrepareBoundaryGuYongShenxing, ResultBind: "paid-colors" },
                    { Op: SkillProgramEffectOp.DiscardSelected, Amount: 2 },
                    { Op: SkillProgramEffectOp.Draw, Amount: 2, Condition.Kind: SkillProgramConditionKind.ChoiceIs, Condition.SourceBind: "paid-colors", Condition.OptionId: "different" },
                    { Op: SkillProgramEffectOp.Draw, Amount: 1, Condition.Kind: SkillProgramConditionKind.ChoiceIs, Condition.SourceBind: "paid-colors", Condition.OptionId: "same" }])
                throw new InvalidOperationException(path + ": mobile Shenxing requires its HP-limited original two-card discard and frozen color reward.");
        }
        if (program.Id == "boundary:bingyi-mobile")
        {
            if (program.Activations.Count != 0 || program.Triggers is not [{ Id: "ending-reveal-share", Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: true } trigger] ||
                !AllowsEmptyRecipientDraw(trigger.Effects, 3, trigger.Window))
                throw new InvalidOperationException(path + ": mobile Bingyi reveals its complete hand before an empty-safe, frozen-card-count recipient selection.");
        }
    }
}
