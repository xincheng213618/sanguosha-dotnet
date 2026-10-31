namespace CardGame.Core;

internal interface IProgramExpressionCaptureHost
{
    void Capture(ProgramSkillFrame frame, string name, ProgramExpression expression);
}

internal sealed class CaptureExpressionNumberDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.CaptureNumber;
    public override ISkillProgramEffectHandler Handler { get; } = new CaptureHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.CaptureNumber,
        static (effect, context) => context.CaptureExpressionNumber(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "resultBind", "valueExpr", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0,
            reader.Condition(), resultBind: reader.RequiredIdentifier("resultBind"))
            { ValueExpression = reader.RequiredIntExpression("valueExpr") };
        RequireAlways(effect, reader.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new CreateNumberBinding(effect.ResultBind!)];
    private sealed class CaptureHandler : ISkillProgramEffectHandler
    {
        public SkillProgramEffectOp Op => SkillProgramEffectOp.CaptureNumber;
        public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
        {
            host.GetCapability<IProgramExpressionCaptureHost>().Capture(frame, effect.ResultBind!, effect.ValueExpression!);
            return SkillProgramStepOutcome.Continue;
        }
    }
}

public sealed partial class GameEngine
{
}
