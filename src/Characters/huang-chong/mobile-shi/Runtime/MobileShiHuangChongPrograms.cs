using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiHcCujunStartedEvent(long FrameId, int OwnerSeat, int TargetSeat) : IGameEvent;
public sealed record ShiHcCujunGiftMovedEvent(long FrameId, int ChooserSeat, int TargetSeat, int CardId, int Sequence) : IGameEvent;
public sealed record ShiHcCujunSlashGrantedEvent(long FrameId, int ChooserSeat, int TargetSeat,
    int GrantedTurn, int GrantedPhaseInstanceId) : IGameEvent;
public sealed record ShiHcCujunClaimIssuedEvent(long FrameId, int OwnerSeat, int TargetSeat, int TurnNumber) : IGameEvent;
public sealed record ShiHcCujunClaimSettledEvent(long ClaimFrameId, long SettleFrameId, int OwnerSeat, int TargetSeat,
    int TurnNumber, IReadOnlyList<int> CardIds) : IGameEvent;
public sealed record ShiHcSiliInvokedEvent(long FrameId, int OwnerSeat, int TurnNumber, string Branch) : IGameEvent;
public sealed record ShiHcSiliSlashArmedEvent(long FrameId, int OwnerSeat, int CardId, int TurnNumber) : IGameEvent;
public sealed record ShiHcSiliDrawGrantedEvent(long FrameId, int OwnerSeat, int TurnNumber, bool GrantedInDrawPhase) : IGameEvent;
public sealed record ShiHcSiliDrawConsumedEvent(long FrameId, long GrantedFrameId, int OwnerSeat, int TurnNumber) : IGameEvent;
public sealed record ShiHcSiliBeishuiArmedEvent(long FrameId, int OwnerSeat, int TurnNumber) : IGameEvent;
public sealed record ShiHcSiliBeishuiConsumedEvent(long FrameId, long ArmedFrameId, int OwnerSeat, int TurnNumber) : IGameEvent;

public enum ShiHcStage
{
    CujunTarget = 0, CujunChoosers = 1, CujunSettling = 2, CujunClaimSettling = 3,
    SiliBranch = 4, SiliDrawSettling = 5, SiliBeishuiSettling = 6,
}

public sealed record ShiHcReceipt(string Operation, ShiHcStage Stage)
{
    public int TargetSeat { get; init; }
    public int[] Choosers { get; init; } = [];
    public int Answered { get; init; }
    public int Cursor { get; init; }
    public int[] GiftCards { get; init; } = [];
    public int SlashCardId { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiHcReceipt? HuangChong { get; init; }
}

internal static class ShiHuangChongComposition
{
    internal const string CujunSkill = "mobile:shi-cujun";
    internal const string SiliSkill = "mobile:shi-sili";
    internal const string CujunDiscardTrigger = "cujun-discard";
    internal const string CujunClaimTrigger = "cujun-claim";
    internal const string SiliHpTrigger = "sili-hp";
    internal const string SiliDrawTrigger = "sili-draw";
    internal const string SiliBeishuiTrigger = "sili-beishui";
    internal const string ProgramAction = "mobile-shi-huang-chong";
    internal const string CujunGiftReason = "skill-program.mobile:shi-cujun.gift";
    internal const string CujunClaimReason = "skill-program.mobile:shi-cujun.claim";
    internal const string SiliGainReason = "skill-program.mobile:shi-sili.gain";

    internal static bool IsDamageKind(CardKind kind) => kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash
        or CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage;

    [ProgramCatalogValidation(68750)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == CujunSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
                program.Triggers is not
                [
                    { Id: CujunDiscardTrigger, Window: SkillProgramTriggerWindow.DiscardPhaseStarting,
                        Subject: SkillProgramTriggerSubject.Owner, Optional: true, IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiHcCujun, Target: SkillProgramEffectTarget.Owner }] },
                    { Id: CujunClaimTrigger, Window: SkillProgramTriggerWindow.DiscardPhaseEnded,
                        Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiHcCujun, Target: SkillProgramEffectTarget.Owner }] }
                ])
                throw new InvalidOperationException(path +
                    ": CuJun requires exactly its optional discard-start trigger and mandatory claim-settlement trigger resolving the owner operation.");
            return;
        }
        if (program.Id != SiliSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
            program.Triggers is not
            [
                { Id: SiliHpTrigger, Window: SkillProgramTriggerWindow.AfterHealthChanged,
                    Subject: SkillProgramTriggerSubject.Owner, Optional: true, IsSkillInvocation: false,
                    HpChangeOccurrence: SkillProgramHpChangeOccurrence.PerEvent,
                    UsageScope: SkillUsageScope.Turn, UsageLimit: 1,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiHcSili, Target: SkillProgramEffectTarget.Owner }] },
                { Id: SiliDrawTrigger, Window: SkillProgramTriggerWindow.DrawPhaseStarting,
                    Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiHcSili, Target: SkillProgramEffectTarget.Owner }] },
                { Id: SiliBeishuiTrigger, Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
                    Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiHcSili, Target: SkillProgramEffectTarget.Owner }] }
            ])
            throw new InvalidOperationException(path +
                ": SiLi requires exactly its once-per-turn optional health-change trigger, mandatory draw-grant trigger and mandatory beishui trigger resolving the owner operation.");
    }
}

internal interface IShiHuangChongHost
{
    SkillProgramStepOutcome ResolveCujun(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveSili(ProgramSkillFrame frame);
}

internal sealed class ResolveShiHcCujunDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiHcCujun;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiHcHandler(SkillProgramEffectOp.ResolveShiHcCujun);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiHcSiliDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiHcSili;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiHcHandler(SkillProgramEffectOp.ResolveShiHcSili);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiHcHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiHcCujun => host.GetCapability<IShiHuangChongHost>().ResolveCujun(frame),
        _ => host.GetCapability<IShiHuangChongHost>().ResolveSili(frame),
    };
}
