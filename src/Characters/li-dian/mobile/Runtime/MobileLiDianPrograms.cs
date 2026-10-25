using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record MobileLiDianWangxiReceipt(CardConversionSource Source, string Hash, long WindowId, long DamageId,
    int SourceSeat, int TargetSeat, int Amount, int OccurrenceIndex, int TurnNumber, int TurnSeat, IReadOnlyList<int> Recipients,
    int RecipientIndex = 0, bool AwaitingMovement = false, NativeDrawOutcome? Draw = null)
{
    private readonly IReadOnlyList<int> _recipients = Array.AsReadOnly(Recipients.ToArray());
    public IReadOnlyList<int> Recipients { get => _recipients; init => _recipients = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MobileLiDianWangxiReceipt? MobileLiDianWangxi { get; init; }
}
internal interface IMobileLiDianWangxiHost
{ SkillProgramStepOutcome Draw(ProgramSkillFrame f); }
internal sealed class MobileLiDianWangxiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawMobileLiDianWangxi;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
    public override ISkillProgramEffectHandler Handler { get; } = new MobileLiDianWangxiHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(4d));
}
internal sealed class MobileLiDianWangxiHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DrawMobileLiDianWangxi;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost h) => h.GetCapability<IMobileLiDianWangxiHost>().Draw(f);
}
internal static class MobileLiDianComposition
{
    [ProgramCatalogValidation(71010)]
    private static void Validate(string path, SkillProgram p)
    {
        var uses = p.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawMobileLiDianWangxi)).ToArray();
        if (p.Id != "mobile:wangxi" && uses.Length == 0 && !p.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawMobileLiDianWangxi))) return;
        if (p.Id != "mobile:wangxi" || p.Activations.Count != 0 || p.Triggers.Count != 2 || uses.Length != 2 ||
            uses.Any(t => t.Window != SkillProgramTriggerWindow.AfterDamageApplied || !t.Optional || t.DamageOccurrence != SkillProgramDamageOccurrence.PerDamagePoint ||
                t.Condition.Kind != SkillProgramTriggerConditionKind.OtherDamageParticipantAlive || t.UsageScope is not null ||
                t.Effects is not [{ Op: SkillProgramEffectOp.DrawMobileLiDianWangxi, Target: SkillProgramEffectTarget.Owner }]) ||
            uses.Count(t => t.Subject == SkillProgramTriggerSubject.Owner && t.Id == "taking-each-point") != 1 ||
            uses.Count(t => t.Subject == SkillProgramTriggerSubject.DamageSource && t.Id == "dealing-each-point") != 1)
            throw new InvalidOperationException(path + ": mobile Wangxi requires its two optional per-point native damage participant bindings.");
    }
}
