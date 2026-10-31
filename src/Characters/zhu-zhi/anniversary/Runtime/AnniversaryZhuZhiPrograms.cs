namespace CardGame.Core;

internal interface IAnniversaryZhuZhiHost { SkillProgramStepOutcome Qualify(ProgramSkillFrame frame, string resultBind); }
internal sealed class ResolveAnniversaryZhuZhiAnguoDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveAnniversaryZhuZhiAnguo;
    public override ISkillProgramEffectHandler Handler { get; } = new AnniversaryZhuZhiHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.Recover, static (_, c) => c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "resultBind", "condition");
        var e = new SkillProgramEffect(Op, r.RequiredEnum<SkillProgramEffectTarget>("target"), 0, r.Condition(), resultBind: r.RequiredIdentifier("resultBind"));
        if (e.Target != SkillProgramEffectTarget.Owner) throw new InvalidOperationException(r.Path + ": Anguo requires its owner placeholder.");
        RequireAlways(e, r.Path);
        if (!AnniversaryZhuZhiComposition.Branches.Contains(e.ResultBind!)) throw new InvalidOperationException(r.Path + ": Anguo requires one of its six ordered branch results.");
        return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new ReadSelectedTarget(), new CreateChoiceResult(e.ResultBind!, ["perform", "skip"])];
}
internal sealed class AnniversaryZhuZhiHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveAnniversaryZhuZhiAnguo;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) => h.GetCapability<IAnniversaryZhuZhiHost>().Qualify(f, e.ResultBind!);
}
public sealed record AnniversaryZhuZhiBranchEvent(long FrameId, CardConversionSource Source, string ResultBind, int RecipientSeat, bool Performed) : IGameEvent;

internal static class AnniversaryZhuZhiComposition
{
    internal const string Skill = "anniversary:anguo",Activation = "three-minimum-benefits";
    internal static readonly string[] Branches = ["target-hand", "target-hp", "target-equipment", "owner-hand", "owner-hp", "owner-equipment"];
    [ProgramCatalogValidation(45800)] internal static void Validate(string path, SkillProgram p)
    {
        if (p.Id != Skill && !p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op == SkillProgramEffectOp.ResolveAnniversaryZhuZhiAnguo)) return;
        if (p.Id != Skill || p.Triggers.Count != 0 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.CardPolicies.Count != 0 ||
            p.Activations is not [{ Id: Activation, MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.OtherLiving, UsesPerPhase: 1, UsesPerTurn: null, Effects.Count: 12 } a])
            throw new InvalidOperationException(path + ": Anguo requires its one actual other-living Play activation and six native branches.");
        for (var i = 0; i < Branches.Length; i++)
        {
            var check = a.Effects[i * 2]; var action = a.Effects[i * 2 + 1]; var branch = Branches[i];
            var op = (i % 3) switch { 0 => SkillProgramEffectOp.Draw, 1 => SkillProgramEffectOp.Recover, _ => SkillProgramEffectOp.UseRandomDeckEquipment };
            if (check.Op != SkillProgramEffectOp.ResolveAnniversaryZhuZhiAnguo || check.Target != SkillProgramEffectTarget.Owner || check.ResultBind != branch || check.Condition.Kind != SkillProgramConditionKind.Always ||
                action.Op != op || action.Condition.Kind != SkillProgramConditionKind.ChoiceIs || action.Condition.SourceBind != branch || action.Condition.OptionId != "perform" ||
                action.Target != (i is 0 or 1 ? SkillProgramEffectTarget.SelectedTarget : SkillProgramEffectTarget.Owner) || (i % 3 != 2 && action.Amount != 1) ||
                (i == 2 && action.TargetReference is not null) || (i == 5 && action.TargetReference?.Kind != ProgramParticipantRef.Owner))
                throw new InvalidOperationException(path + ": Anguo must qualify and execute target then remaining owner Draw/Recovery/native equipment branches in order.");
        }
    }
}

public sealed partial class GameEngine
{
}
