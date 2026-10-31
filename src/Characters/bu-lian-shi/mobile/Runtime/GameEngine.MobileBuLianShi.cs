namespace CardGame.Core;

internal interface IMobileBuLianShiAnxuHost { void Prepare(ProgramSkillFrame frame); }
internal sealed class PrepareMobileBuLianShiAnxuDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileBuLianShiAnxu;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileBuLianShiAnxuHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new ReadTargetSet(2, 2), new ReadCardSet("mobile-anxu-gift"), new CreateChoiceResult("mobile-anxu-equality", ["equal", "unequal"])];
}
internal sealed class MobileBuLianShiAnxuHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileBuLianShiAnxu;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host)
    { host.GetCapability<IMobileBuLianShiAnxuHost>().Prepare(frame); return SkillProgramStepOutcome.Continue; }
}
public sealed record MobileBuLianShiAnxuComparedEvent(long FrameId, int RecipientSeat, int DonorSeat, int RecipientHand, int DonorHand, bool Equal) : IGameEvent;
public sealed record MobileBuLianShiAnxuDeliveredEvent(long FrameId, string SkillInstanceId, string GameplayHash, int RecipientSeat, int DonorSeat, int CardId, long BatchId, long MovementSequence) : IGameEvent;
