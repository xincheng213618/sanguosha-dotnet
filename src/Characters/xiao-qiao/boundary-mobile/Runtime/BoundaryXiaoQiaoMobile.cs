using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BoundaryXiaoQiaoJudgmentOverride(long ProgramFrameId, long JudgmentFrameId,
    CardConversionSource Source, string GameplayHash, int CardId, Suit Suit);
public sealed record BoundaryXiaoQiaoJudgmentRewrittenEvent(BoundaryXiaoQiaoJudgmentOverride Receipt) : IGameEvent;
public sealed partial record JudgmentFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundaryXiaoQiaoJudgmentOverride? BoundaryXiaoQiaoOverride { get; init; }
}

internal interface IBoundaryXiaoQiaoMobileHost { SkillProgramStepOutcome Rewrite(ProgramSkillFrame frame); }
internal sealed class RewriteBoundaryXiaoQiaoJudgmentDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewriteBoundaryXiaoQiaoJudgment;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryXiaoQiaoJudgmentHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ReplaceJudgment, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.JudgmentBeforeResult)];
}
internal sealed class BoundaryXiaoQiaoJudgmentHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RewriteBoundaryXiaoQiaoJudgment;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryXiaoQiaoMobileHost>().Rewrite(f);
}

public sealed partial class GameEngine
{
}
