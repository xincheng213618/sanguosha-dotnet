using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record MobileSunJianPoluReceipt(CardConversionSource Source, string Hash, int Count,
    int RecipientIndex = 0, bool AwaitingMovement = false, NativeDrawOutcome? Draw = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MobileSunJianPoluReceipt? MobileSunJianPolu { get; init; }
}
internal interface IMobileSunJianPoluHost
{ SkillProgramStepOutcome Draw(ProgramSkillFrame frame); }
internal static class MobileSunJianComposition
{
    [ProgramCatalogValidation(65000)]
    private static void Validate(string path, SkillProgram program)
    {
        var uses = program.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMobileSunJianPolu)).ToArray();
        if (program.Id != "boundary:polu-mobile" && uses.Length == 0 &&
            !program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMobileSunJianPolu))) return;
        if (program.Id != "boundary:polu-mobile" || program.Activations.Count != 0 || program.Triggers.Count != 2 || uses.Length != 2 ||
            uses.Any(t => !t.Optional || t.Subject != SkillProgramTriggerSubject.Owner || t.UsageScope is not null || t.UsageLimit is not null ||
                t.Effects is not [{ Op: SkillProgramEffectOp.SelectTargets, Target: SkillProgramEffectTarget.Owner,
                    TargetKind: SkillProgramTargetKind.AnyLiving, MinimumTargets: 0, MaximumTargets: 8 },
                    { Op: SkillProgramEffectOp.ResolveMobileSunJianPolu, Target: SkillProgramEffectTarget.Owner }]) ||
            uses.Count(t => t.Window == SkillProgramTriggerWindow.OwnerDied && t.Condition.Kind == SkillProgramTriggerConditionKind.Always) != 1 ||
            uses.Count(t => t.Window == SkillProgramTriggerWindow.CharacterDied && t.Condition.Kind == SkillProgramTriggerConditionKind.DeathKillerIsOwner) != 1)
            throw new InvalidOperationException(path + ": mobile Polu requires its two original death triggers, arbitrary living targets and shared actual invocation count.");
    }
}
internal sealed class MobileSunJianPoluDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileSunJianPolu;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) =>
        context.Frame.WindowContext?.Window == SkillProgramTriggerWindow.OwnerDied || context.State.CanContinueAfterOwnerDeath(context.Frame);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadTargetSet(0)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(3));
    public override ISkillProgramEffectHandler Handler { get; } = new MobileSunJianPoluHandler();
}
internal sealed class MobileSunJianPoluHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileSunJianPolu;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileSunJianPoluHost>().Draw(frame);
}
