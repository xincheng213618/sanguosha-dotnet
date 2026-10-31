namespace CardGame.Core;

public sealed record ProgramConversionPolarityCommittedEvent(long FrameId, long? ParentFrameId,
    int OwnerSeat, string SkillId, string SkillInstanceId, string BindingId,
    SkillPolarity PreviousState, SkillPolarity CurrentState, int TurnNumber, int PhaseInstanceId) : IGameEvent;
internal interface IConversionPolarityProgramHost { void CommitConversionPolarity(ProgramSkillFrame frame); }
internal sealed class CommitConversionPolarityDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.CommitConversionPolarity;
    public override bool UsesConversionPolarity => true;
    public override ISkillProgramEffectHandler Handler { get; } = new CommitConversionPolarityHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ToggleBooleanState, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}
public sealed class CommitConversionPolarityHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.CommitConversionPolarity;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host)
    { host.GetCapability<IConversionPolarityProgramHost>().CommitConversionPolarity(frame); return SkillProgramStepOutcome.Continue; }
}
