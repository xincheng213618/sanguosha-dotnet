using System.Text.Json.Serialization;
namespace CardGame.Core;

internal interface IBoundaryYuanShuHost { SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal abstract class BoundaryYuanShuDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new BoundaryYuanShuHandler(Op);
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.TurnEffects;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition());
        if (Op != SkillProgramEffectOp.GrantBoundaryYuanShuLordSkills) RequireAlways(effect, reader.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindows(Op == SkillProgramEffectOp.GrantBoundaryYuanShuWangzunPenalty ?
            [SkillProgramTriggerWindow.TurnStartBeforeNormalFlow] : [SkillProgramTriggerWindow.TurnEnding])];
}
internal sealed class PrepareBoundaryYuanShuJixiDescriptor : BoundaryYuanShuDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryYuanShuJixi; }
internal sealed class GrantBoundaryYuanShuLordSkillsDescriptor : BoundaryYuanShuDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantBoundaryYuanShuLordSkills; }
internal sealed class GrantBoundaryYuanShuWangzunPenaltyDescriptor : BoundaryYuanShuDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantBoundaryYuanShuWangzunPenalty; }
internal sealed class BindBoundaryYuanShuYongsiDiscardDescriptor : BoundaryYuanShuDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindBoundaryYuanShuYongsiDiscard;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition(), resultBind: "yongsi-discardable");
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindows([SkillProgramTriggerWindow.DiscardPhaseStarting]), new CreateCardSet("yongsi-discardable", 1, false, CardOwner: SkillProgramEffectTarget.Owner)];
}
internal sealed class BoundaryYuanShuHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryYuanShuHost>().Execute(frame, op);
}
internal static class BoundaryYuanShuComposition
{
    internal const string Jixi = "boundary:jixi-mobile", Yongsi = "boundary:yongsi-mobile", Wangzun = "boundary:wangzun-mobile";
    [ProgramCatalogValidation(55200)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.SelectMany(t => t.Effects).Concat(program.Activations.SelectMany(a => a.Effects)).Any(e => e.Op is
            SkillProgramEffectOp.PrepareBoundaryYuanShuJixi or SkillProgramEffectOp.GrantBoundaryYuanShuLordSkills or SkillProgramEffectOp.GrantBoundaryYuanShuWangzunPenalty or SkillProgramEffectOp.BindBoundaryYuanShuYongsiDiscard)) return;
        var valid = program.Id == Jixi && program.Activations.Count == 0 && program.Triggers is
            [{ Window: SkillProgramTriggerWindow.TurnEnding, Optional: false, UsageScope: SkillUsageScope.Game, UsageLimit: 1, Effects:
                [{ Op: SkillProgramEffectOp.PrepareBoundaryYuanShuJixi }, { Op: SkillProgramEffectOp.ChangeMaximumHp, Amount: 1 },
                 { Op: SkillProgramEffectOp.Recover, Amount: 1 }, { Op: SkillProgramEffectOp.ChooseOption },
                 { Op: SkillProgramEffectOp.GrantSkills }, { Op: SkillProgramEffectOp.Draw, Amount: 2 }, { Op: SkillProgramEffectOp.GrantBoundaryYuanShuLordSkills }] }]
            || program.Id == Wangzun && program.Activations.Count == 0 && program.Triggers is
            [{ Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Optional: true, TurnOwnerScope: SkillProgramTurnOwnerScope.AnyLiving,
                Effects: [{ Op: SkillProgramEffectOp.Draw, Amount: 1 }, { Op: SkillProgramEffectOp.GrantBoundaryYuanShuWangzunPenalty }] }]
            || program.Id == Yongsi && program.Triggers.Any(t => t.Window == SkillProgramTriggerWindow.DiscardPhaseStarting && t.Effects is
                [{ Op: SkillProgramEffectOp.BindBoundaryYuanShuYongsiDiscard }, { Op: SkillProgramEffectOp.ChooseOption }, { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, AwaitMovementTriggers: true }, { Op: SkillProgramEffectOp.LoseHp, Amount: 1 }]);
        if (!valid) throw new InvalidOperationException(path + ": mobile Jixi/Wangzun requires the original awakening or current Lord preparation flow.");
    }
}
public sealed record BoundaryYuanShuOwnHpDecreasedEvent(int OwnerSeat, int TurnNumber, long HpChangeId, int Before, int After) : IGameEvent;
public sealed record BoundaryYuanShuJixiReceipt(long WindowId, int FirstTurn, int SecondTurn, int ThirdTurn);
public sealed record BoundaryYuanShuJixiPreparedEvent(long FrameId, int OwnerSeat, string SkillInstanceId, string GameplayHash, BoundaryYuanShuJixiReceipt Receipt) : IGameEvent;
public sealed record BoundaryYuanShuLordSkillGrantedEvent(long FrameId, int OwnerSeat, int LordSeat, string GrantId, string SkillId, string SkillInstanceId, string SourceId, string LordGrantId, string LordInstanceId) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryYuanShuJixiReceipt? BoundaryYuanShuJixi { get; init; }
}
