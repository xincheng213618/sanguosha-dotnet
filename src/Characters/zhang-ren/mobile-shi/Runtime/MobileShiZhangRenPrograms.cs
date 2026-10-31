using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiZrFuanArmedEvent(long FrameId, int OwnerSeat, int ActualTurn, int Round, int DrawCount) : IGameEvent;
public sealed record ShiZrFuanRecordedEvent(long FrameId, int OwnerSeat, int ActualTurn, int Round, Suit Suit, bool ViaRelaunch) : IGameEvent;
public sealed record ShiZrFuanClearedEvent(long FrameId, int OwnerSeat, int ActualTurn, int Round, int TargetSeat) : IGameEvent;
public sealed record ShiZrFuanSuitDiscardedEvent(int ActualTurn, int OwnerSeat, int CardId, Suit Suit) : IGameEvent;
public sealed record ShiZrFuanStrikeIssuedEvent(long FrameId, int OwnerSeat, int ActualTurn, long UseId, long ActionId,
    int TargetSeat, int DamageCountBefore) : IGameEvent;
public sealed record ShiZrFuanStrikeSettledEvent(long FrameId, long UseId, int TargetSeat, bool Damaged, string Choice) : IGameEvent;
public sealed record ShiZrYinXianBonusGainedEvent(int OwnerSeat, int ActualTurn, long ActionId, int Bonus) : IGameEvent;

public enum ShiZrStage { FuanAsk = 0, FuanRecord = 1, StrikeAsk = 10, StrikeTarget = 11, StrikeIssued = 12, StrikeChoose = 13, RelaunchRecord = 14 }

public sealed record ShiZrReceipt(string Operation, int ActualTurn, ShiZrStage Stage)
{
    public int Round { get; init; }
    public Suit Suit { get; init; }
    public int DrawCount { get; init; }
    public int TargetSeat { get; init; } = -1;
    public long UseId { get; init; }
    public long ActionId { get; init; }
    public int DamageCountBefore { get; init; }
    public bool ViaRelaunch { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiZrReceipt? ShiZhangRen { get; init; }
}

internal static class ShiZhangRenComposition
{
    internal const string FuanSkill = "mobile:shi-fuan";
    internal const string YinXianSkill = "mobile:shi-yinxian";
    internal const string FuanArmTrigger = "fuan-turn-ending";
    internal const string FuanStrikeTrigger = "fuan-strike";
    internal const string YinXianUseTrigger = "yinxian-use";
    internal const string ProgramAction = "mobile-shi-zhang-ren";
    internal const string FuanDrawReason = "skill-program.mobile:shi-fuan.arm";
    internal const string FuanRelaunchDrawReason = "skill-program.mobile:shi-fuan.relaunch";
    internal const int MaximumDraw = 5;

    [ProgramCatalogValidation(68200)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == FuanSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
                program.Triggers is not
                [
                    { Id: FuanArmTrigger, Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner,
                        TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: false, IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiZrFuan, Target: SkillProgramEffectTarget.Owner }] },
                    { Id: FuanStrikeTrigger, Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner,
                        TurnOwnerScope: SkillProgramTurnOwnerScope.AnyLiving, Optional: false, IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiZrFuanStrike, Target: SkillProgramEffectTarget.Owner }] }
                ])
                throw new InvalidOperationException(path +
                    ": FuAn requires exactly its own-turn-ending arm trigger and any-living-turn strike trigger.");
            return;
        }
        if (program.Id != YinXianSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
            program.Triggers is not
            [
                { Id: YinXianUseTrigger, Window: SkillProgramTriggerWindow.CardUseCompleted, OwnerRelation: SkillProgramCardActionOwnerRelation.Actor,
                    Optional: false, IsSkillInvocation: false,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiZrYinXian, Target: SkillProgramEffectTarget.Owner }] }
            ])
            throw new InvalidOperationException(path +
                ": YinXian requires exactly its completed-use actor trigger resolving the owner bonus.");
    }
}

internal interface IShiZhangRenHost
{
    SkillProgramStepOutcome ResolveFuan(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveFuanStrike(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveYinXian(ProgramSkillFrame frame);
}

internal sealed class ResolveShiZrFuanDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiZrFuan;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiZrHandler(SkillProgramEffectOp.ResolveShiZrFuan);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiZrFuanStrikeDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiZrFuanStrike;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiZrHandler(SkillProgramEffectOp.ResolveShiZrFuanStrike);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectTarget,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiZrYinXianDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiZrYinXian;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiZrHandler(SkillProgramEffectOp.ResolveShiZrYinXian);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier,
        static (_, context) => context.PublicControlValue(0));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
}

internal sealed class ShiZrHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiZrFuan => host.GetCapability<IShiZhangRenHost>().ResolveFuan(frame),
        SkillProgramEffectOp.ResolveShiZrFuanStrike => host.GetCapability<IShiZhangRenHost>().ResolveFuanStrike(frame),
        _ => host.GetCapability<IShiZhangRenHost>().ResolveYinXian(frame),
    };
}
