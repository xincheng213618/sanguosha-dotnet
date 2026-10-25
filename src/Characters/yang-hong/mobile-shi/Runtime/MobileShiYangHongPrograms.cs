using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiYhJijianStartedEvent(long FrameId, int OwnerSeat, int CardId,
    int FirstSeat, int SecondSeat, bool FirstSwap, bool SecondSwap) : IGameEvent;
public sealed record ShiYhJijianResolvedEvent(long FrameId, int OwnerSeat, string Outcome,
    int? WinnerSeat, int? LoserSeat) : IGameEvent;
public sealed record ShiYhYuanmoMovedEvent(long FrameId, int OwnerSeat, int CardId,
    CardLocation From, CardLocation To, int Drawn) : IGameEvent;
public sealed record ShiYhYuanmoUsedEvent(long FrameId, int OwnerSeat, int Turn) : IGameEvent;

public enum ShiYhStage { SwapFirst = 0, SwapSecond = 1, Contest = 2, Slash = 3, Retaliate = 4, MoveChoice = 5, DrawChoice = 6 }

public sealed record ShiYhReceipt(string Operation, ShiYhStage Stage)
{
    public int CardId { get; init; }
    public int FirstSeat { get; init; }
    public int SecondSeat { get; init; }
    public bool FirstSwap { get; init; }
    public bool SecondSwap { get; init; }
    public int MoverSeat { get; init; }
    public int Drawn { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiYhReceipt? YangHong { get; init; }
}

internal static class ShiYangHongComposition
{
    internal const string JijianSkill = "mobile:shi-jijian";
    internal const string YuanmoSkill = "mobile:shi-yuanmo";
    internal const string JijianActivation = "jijian-contest";
    internal const string YuanmoPrepTrigger = "yuanmo-prep";
    internal const string YuanmoDamageTrigger = "yuanmo-damage";
    internal const string ProgramAction = "mobile-shi-yang-hong";
    internal const string JijianLendReason = "skill-program.mobile:shi-jijian.lend";
    internal const string YuanmoMoveReason = "skill-program.mobile:shi-yuanmo.move";
    internal const string PindianBind = "jijian-contest";

    [ProgramCatalogValidation(68640)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == YuanmoSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
                program.Triggers is not
                [
                    { Id: YuanmoPrepTrigger, Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
                        Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiYhYuanmo, Target: SkillProgramEffectTarget.Owner }] },
                    { Id: YuanmoDamageTrigger, Window: SkillProgramTriggerWindow.AfterDamageApplied,
                        Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                        DamageOccurrence: SkillProgramDamageOccurrence.PerDamage,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiYhYuanmo, Target: SkillProgramEffectTarget.Owner }] }
                ])
                throw new InvalidOperationException(path +
                    ": YuanMo requires exactly its prep and damage triggers resolving the owner operation.");
            return;
        }
        if (program.Id != JijianSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Triggers.Count != 0 ||
            program.Activations is not [{ Id: JijianActivation, MinCards: 1, MaxCards: 1, MinTargets: 2, MaxTargets: 2,
                TargetKind: SkillProgramTargetKind.AnyLiving, UsesPerPhase: 1,
                Effects: [{ Op: SkillProgramEffectOp.ResolveShiYhJijian, Target: SkillProgramEffectTarget.Owner }] }])
            throw new InvalidOperationException(path +
                ": JiJi requires exactly its once-per-phase secret-card two-target activation.");
    }
}

internal interface IShiYangHongHost
{
    SkillProgramStepOutcome ResolveJijian(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveYuanmo(ProgramSkillFrame frame);
}

internal sealed class ResolveShiYhJijianDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiYhJijian;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiYhHandler(SkillProgramEffectOp.ResolveShiYhJijian);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.StartPindian,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ConsumeSelectedCards(1)];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiYhYuanmoDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiYhYuanmo;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiYhHandler(SkillProgramEffectOp.ResolveShiYhYuanmo);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiYhHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiYhJijian => host.GetCapability<IShiYangHongHost>().ResolveJijian(frame),
        _ => host.GetCapability<IShiYangHongHost>().ResolveYuanmo(frame),
    };
}
