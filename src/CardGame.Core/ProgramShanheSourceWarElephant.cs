using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheWarElephantCounterSlashReceipt(int InstructionIndex, long DamageWindowId,
    int SourceSeat, long? SuspendedAttackId, long? SuspendedDuelId, long ChildUseId);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheWarElephantCounterSlashReceipt? ShanheWarElephantCounterSlash { get; init; }
}

internal interface IShanheWarElephantProgramHost
{
    SkillProgramStepOutcome CounterSlash(ProgramSkillFrame frame);
}

internal sealed class UseShanheWarElephantCounterSlashDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.UseShanheWarElephantCounterSlash;
    public override ISkillProgramEffectHandler Handler { get; } = new UseShanheWarElephantCounterSlashHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.Owner)
            throw new InvalidOperationException("Source Xiongshou counter Slash must be owned by the actual damaged participant.");
        var effect = new SkillProgramEffect(Op, SkillProgramEffectTarget.Owner, 0, r.Condition());
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}

internal sealed class UseShanheWarElephantCounterSlashHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.UseShanheWarElephantCounterSlash;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheWarElephantProgramHost>().CounterSlash(frame);
}
