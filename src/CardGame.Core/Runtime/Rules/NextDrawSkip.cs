namespace CardGame.Core;

internal interface INextDrawSkip : IProgramRuntimeOperations
{
    void Schedule(ProgramSkillFrame frame, int targetSeat);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NextDrawSkip(IMatchParticipantStateAccess participants, IProgramRuntimeFrames frames) : INextDrawSkip
{
    public void Schedule(ProgramSkillFrame supplied, int targetSeat)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        if (frame.ActivationId is null || frame.SelectedTargetSeats is not [var selected] ||
            targetSeat != selected || targetSeat == frame.OwnerSeat || !participants.At(targetSeat).IsAlive)
            throw new InvalidOperationException("A draw skip requires a living opponent on the original activation.");
        participants.At(targetSeat).SkipNextDrawPhase = true;
    }

    internal static bool Consume(CharacterState owner)
    {
        var pending = owner.SkipNextDrawPhase;
        owner.SkipNextDrawPhase = false;
        return pending;
    }
}

internal sealed class ScheduleNextDrawSkipDescriptor : ProgramOperationDescriptorBase, ISkillProgramEffectHandler
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ScheduleNextDrawSkip;
    public override ISkillProgramEffectHandler Handler => this;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(
        ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, context) => context.PublicControlValue(2));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        if (reader.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.SelectedTarget)
            throw new InvalidOperationException("A future draw skip needs a selected target.");
        return new(Op, SkillProgramEffectTarget.SelectedTarget, 0, reader.Condition());
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadSelectedTarget()];

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host)
    {
        host.GetCapability<INextDrawSkip>().Schedule(frame, targetSeat);
        return SkillProgramStepOutcome.Continue;
    }
}
