using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record SourceKeshouMaterial(int CardId, CardLocation From, bool IsRed);
public sealed record SourceKeshouReceipt(long WindowId, long AttackId, CardConversionSource Source, string GameplayHash,
    int OriginalAmount, int SequenceBefore)
{
    private IReadOnlyList<SourceKeshouMaterial> _materials = Array.AsReadOnly(Array.Empty<SourceKeshouMaterial>());
    public IReadOnlyList<SourceKeshouMaterial> Materials
    { get => _materials; init => _materials = Array.AsReadOnly(value.ToArray()); }
    public bool? SoleFactionAfterCost { get; init; }
}
public sealed record SourceKeshouPaidBranchEvent(long ProgramFrameId, SourceKeshouReceipt Receipt) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SourceKeshouReceipt? SourceKeshou { get; init; }
}
internal interface ISourceKeshouHost
{
    SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, string step);
}
internal sealed class ResolveShanheSourceKeshouDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceKeshou;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "stateId", "resultBind", "condition");
        var step = r.RequiredIdentifier("stateId");
        var bind = r.RequiredIdentifier("resultBind");
        if (!(step == "select-cost" && bind == "keshou-cost" || step == "after-cost" && bind == "sole-faction"))
            throw new InvalidOperationException("Source993 requires its exact selection or paid faction binding.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0,
            r.Condition(), stateId: step, resultBind: bind);
        RequireAlways(effect, r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => e.StateId == "select-cost"
        ? [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied), new CaptureSourceCard(e.ResultBind!, 2, false, SkillProgramEffectTarget.Owner)]
        : [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied), new CreateChoiceResult(e.ResultBind!, ["sole", "shared"])];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.PreventCurrentDamage,
        static (e, c) => c.PreventCurrentDamage(e));
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveShanheSourceKeshouHandler();
}
internal sealed class ResolveShanheSourceKeshouHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceKeshou;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<ISourceKeshouHost>().Resolve(f, e.StateId!);
}
internal static class SourceKeshouContract
{
    internal const string SkillId = "shanhe:source-keshou-993";
    [ProgramCatalogValidation(62061)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceKeshou)) &&
            !p.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceKeshou))) return;
        if (p.Id != SkillId || p.Activations.Count != 0 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 ||
            p.CardPolicies.Count != 0 || p.DamageModifiers.Count != 0 || p.BooleanStates.Count != 0 || p.Triggers is not
            [{ Id: "pay-colour-pair-reduce-and-judge", Window: SkillProgramTriggerWindow.BeforeDamageApplied,
                Subject: SkillProgramTriggerSubject.DamageTarget, Optional: true, Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [
                    { Op: SkillProgramEffectOp.ResolveShanheSourceKeshou, StateId: "select-cost", ResultBind: "keshou-cost" },
                    { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "keshou-cost", Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true },
                    { Op: SkillProgramEffectOp.ReduceCurrentDamage, Amount: 1 },
                    { Op: SkillProgramEffectOp.ResolveShanheSourceKeshou, StateId: "after-cost", ResultBind: "sole-faction" },
                    { Op: SkillProgramEffectOp.StartJudgment, JudgmentReason: "shanhe.source-keshou", ResultBind: "keshou-judgment", Visibility: SkillProgramCardSetVisibility.Public },
                    { Op: SkillProgramEffectOp.Draw, Amount: 1 },
                    { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "keshou-judgment", Destination: SkillProgramCardDestination.DiscardPile }
                ] }]) throw new InvalidOperationException(path + ": Source993 requires its complete paid reduction, current-faction judgment and ordinary cleanup sequence.");
        var e = p.Triggers[0].Effects;
        if (e.Any(x => x.Target != SkillProgramEffectTarget.Owner) || e.Take(4).Any(x => x.Condition.Kind != SkillProgramConditionKind.Always) ||
            !IsSole(e[4].Condition) || !IsSole(e[6].Condition) || e[5].Condition is not { Kind: SkillProgramConditionKind.All, Children.Count: 2 } draw ||
            !IsSole(draw.Children[0]) || draw.Children[1] is not { Kind: SkillProgramConditionKind.BoundCardsMatchSuits, SourceBind: "keshou-judgment" } red ||
            !red.Suits.SequenceEqual(new[] { Suit.Heart, Suit.Diamond }))
            throw new InvalidOperationException(path + ": Source993's paid branch and red judgment must retain their exact frame-local predicates.");
    }
    private static bool IsSole(SkillProgramCondition c) => c is
        { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "sole-faction", OptionId: "sole" };
}
