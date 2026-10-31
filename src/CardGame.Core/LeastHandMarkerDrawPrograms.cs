namespace CardGame.Core;
public sealed record ProgramMarkerExtraDrawPolicy(int TargetSeat, PlayerMarkerKind Marker, int SourceSeat, string SkillId, string SkillInstanceId, string BindingId);
public sealed record ProgramLeastHandMarkerDrawReceipt(int InstructionIndex, int TargetSeat);
public sealed record ProgramLeastHandMarkerOrDrawEvent(long FrameId, int OwnerSeat, int TargetSeat, string SkillId, string SkillInstanceId, PlayerMarkerKind Marker, bool GrantedMarker) : IGameEvent;
public sealed record ProgramMarkerExtraDrawConsumedEvent(long FrameId, int TargetSeat, PlayerMarkerKind Marker, int SourceSeat, string SkillId, string SkillInstanceId) : IGameEvent;
public interface ILeastHandMarkerDrawProgramHost
{
    SkillProgramStepOutcome GrantLeastHandMarkerOrDraw(ProgramSkillFrame frame, int targetSeat, PlayerMarkerKind marker);
}

public sealed class GrantLeastHandMarkerOrDrawHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantLeastHandMarkerOrDraw;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => host.GetCapability<ILeastHandMarkerDrawProgramHost>().GrantLeastHandMarkerOrDraw(frame, targetSeat, effect.Marker!.Value);
}

internal sealed class GrantLeastHandMarkerOrDrawDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantLeastHandMarkerOrDraw;
    public override ISkillProgramEffectHandler Handler { get; } = new GrantLeastHandMarkerOrDrawHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (effect, context) => context.Draw(new(SkillProgramEffectOp.Draw, SkillProgramEffectTarget.SelectedTarget, 1, effect.Condition)));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "marker", "amount", "condition");
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.SelectedTarget || r.RequiredInt("amount") != 1)
            throw new InvalidOperationException("Least-hand marker benefits require one selected living target and exactly one marker/card.");
        var effect = new SkillProgramEffect(Op, SkillProgramEffectTarget.SelectedTarget, 1, r.Condition(), marker: r.RequiredEnum<PlayerMarkerKind>("marker"));
        RequireAlways(effect, r.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseSkipped), new ReadTargetSet(1, 1)];
}
