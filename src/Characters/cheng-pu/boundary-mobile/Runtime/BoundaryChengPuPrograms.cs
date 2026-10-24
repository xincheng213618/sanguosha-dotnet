using System.Text.Json.Serialization;
namespace CardGame.Core;

internal static class BoundaryChengPuComposition
{
    internal const string Lihuo = "boundary:lihuo-mobile";
    [ProgramCatalogValidation(56200)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Any(e => e.Op == SkillProgramEffectOp.ResolveBoundaryChengPuLihuo)) return;
        if (p.Id != Lihuo || p.Activations.Count != 0 || p.Modifiers.Count != 0 || p.DamageModifiers.Count != 0 ||
            p.ViewAs is not [{ Id: "ordinary-slash-to-fire-slash", InputKinds: [CardKind.Slash], OutputKind: CardKind.FireSlash, ForPlay: true, ForResponse: false }] ||
            p.Triggers is not [{ Id: "fire-slash-completion-loss", Window: SkillProgramTriggerWindow.CardUseCompleted, OwnerRelation: SkillProgramCardActionOwnerRelation.Actor,
                CardKinds: [CardKind.FireSlash], Optional: false, Condition.Kind: SkillProgramTriggerConditionKind.CardUseCausedDamage,
                Effects: [{ Op: SkillProgramEffectOp.ResolveBoundaryChengPuLihuo, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": mobile Lihuo requires only ordinary Slash conversion and every native FireSlash completion payment.");
    }
}
internal interface IBoundaryChengPuHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame); }
internal sealed class ResolveBoundaryChengPuLihuoDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryChengPuLihuo;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryChengPuHandler();
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.LoseHp, static (_, c) => c.PublicControlValue(-1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCompleted)];
}
internal sealed class BoundaryChengPuHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryChengPuLihuo;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryChengPuHost>().Resolve(f);
}
public sealed record BoundaryChengPuLihuoReceipt(long UseId, long ActionId, int TotalDamage, int RequestedLoss, int HpBefore)
{ public bool Paid { get; init; } public int HpAfter { get; init; } }
public sealed record BoundaryChengPuLihuoIssuedEvent(long FrameId, CardConversionSource Source, string Hash, long UseId, long ActionId, int TotalDamage, int RequestedLoss, int HpBefore) : IGameEvent;
public sealed record BoundaryChengPuLihuoPaidEvent(long FrameId, long UseId, int Loss, int HpBefore, int HpAfter) : IGameEvent;
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryChengPuLihuoReceipt? BoundaryChengPuLihuo { get; init; } }
