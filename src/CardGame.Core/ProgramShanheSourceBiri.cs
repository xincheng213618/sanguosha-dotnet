using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ShanheSourceBiriStage { RecoveryChildren, DrawChildren }
public sealed record ShanheSourceBiriReceipt(int InstructionIndex, long BeforeDamageFrameId,
    long DamageParentFrameId, long AttackResolutionId, int PreventedAmount, int RequestedRecoveryAmount,
    ShanheSourceBiriStage Stage = ShanheSourceBiriStage.RecoveryChildren,
    long? RecoveryAttemptId = null, int RecoveryAttemptAmount = 0,
    long? AppliedRecoveryFrameId = null, int ActualRecovered = 0,
    long MovementSequenceBefore = 0, long MovementSequenceAfter = 0, int DrawCount = 0);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceBiriReceipt? ShanheSourceBiri { get; init; }
}

public sealed record ShanheSourceBiriIssuedEvent(long ProgramFrameId, CardConversionSource Source,
    int InstructionIndex, long BeforeDamageFrameId, long DamageParentFrameId, long AttackResolutionId,
    int PreventedAmount, int RequestedRecoveryAmount) : IGameEvent;
public sealed record ShanheSourceBiriRecoveredEvent(long ProgramFrameId, long RecoveryFrameId,
    long RecoveryParentFrameId, int OwnerSeat, int Amount) : IGameEvent;
public sealed record ShanheSourceBiriDrawIssuedEvent(long ProgramFrameId, int OwnerSeat,
    int ActualRecovered, int DrawCount) : IGameEvent;

internal interface IShanheSourceBiriHost
{
    SkillProgramStepOutcome PreventFireRecoverDraw(ProgramSkillFrame frame);
}
internal sealed class PreventShanheSourceFireRecoverDrawDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PreventShanheSourceFireRecoverDraw;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.PreventCurrentDamage,
        static (effect, context) => context.PreventCurrentDamage(effect));
    public override ISkillProgramEffectHandler Handler { get; } = new PreventShanheSourceFireRecoverDrawHandler();
}
internal sealed class PreventShanheSourceFireRecoverDrawHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PreventShanheSourceFireRecoverDraw;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<IShanheSourceBiriHost>().PreventFireRecoverDraw(frame);
}
internal static class ShanheSourceBiriContract
{
    [ProgramCatalogValidation(62047)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.PreventShanheSourceFireRecoverDraw)) &&
            !program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.PreventShanheSourceFireRecoverDraw))) return;
        if (program.Id != "shanhe:source-biri-14039" || program.Activations.Count != 0 || program.Triggers is not
            [{ Id: "prevent-fire", Window: SkillProgramTriggerWindow.BeforeDamageApplied,
               Subject: SkillProgramTriggerSubject.DamageTarget, Optional: false,
               Condition: { Kind: SkillProgramTriggerConditionKind.DamageNatureIs, DamageNature: DamageNature.Fire },
               Effects: [{ Op: SkillProgramEffectOp.PreventShanheSourceFireRecoverDraw,
                   Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": source Biri requires its single locked owner fire-damage instruction.");
    }
}
