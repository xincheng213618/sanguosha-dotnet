using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ChenJiaoQingYanShownEvent(long FrameId, int Round, int OwnerSeat, int CardId,
    int Ordinal, int ShowIndex) : IGameEvent;
public sealed record ChenJiaoQingYanUsedEvent(long FrameId, int Round, int OwnerSeat, int Ordinal,
    CardKind Kind, long UseId) : IGameEvent;
public sealed record ChenJiaoCeDuanStartedEvent(long FrameId, int Round, int OwnerSeat, int TargetSeat,
    int HolderCount) : IGameEvent;
public sealed record ChenJiaoCeDuanShownEvent(long FrameId, int Round, int HolderSeat, int CardId,
    int ShowIndex) : IGameEvent;
public sealed record ChenJiaoCeDuanUseIssuedEvent(long FrameId, long UseId, int OwnerSeat, int TargetSeat,
    int MaterialCount, int DamageCountBefore) : IGameEvent;
public sealed record ChenJiaoCeDuanCompletedEvent(long FrameId, long UseId, bool Damaged, bool Drew) : IGameEvent;

public enum ChenJiaoStage { Show = 0, ChooseKind = 3, ChooseColor = 5, Issue = 6, Settle = 9 }

public sealed record ChenJiaoReceipt(string Operation, int ActualTurn, ChenJiaoStage Stage = ChenJiaoStage.Show)
{
    public int Ordinal { get; init; }
    public int Cursor { get; init; }
    public int TargetSeat { get; init; } = -1;
    public int Round { get; init; }
    public long UseId { get; init; }
    public int MaterialCount { get; init; }
    public int DamageCountBefore { get; init; }
    public CardKind Kind { get; init; }
    public Suit Color { get; init; }
    public bool Skip { get; init; }
    public bool Damaged { get; init; }
    public IReadOnlyList<int> Holders { get; init; } = [];
    public IReadOnlyList<int> ShownIds { get; init; } = [];
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChenJiaoReceipt? ChenJiao { get; init; }
}

internal static class ChenJiaoComposition
{
    internal const string QingYanSkill = "mobile:shi-qingyan";
    internal const string CeDuanSkill = "mobile:shi-ceduan";
    internal const string QingYanActivation = "qingyan-show";
    internal const string CeDuanActivation = "ceduan-use";
    internal const string ProgramAction = "mobile-shi-chen-jiao";
    internal const string QingYanShowReason = "skill-program.mobile:shi-qingyan.show";
    internal const string CeDuanShowReason = "skill-program.mobile:shi-ceduan.show";
    internal const string CeDuanTakeReason = "skill-program.mobile:shi-ceduan.take";
    internal const int MaximumOrdinal = 5;

    [ProgramCatalogValidation(68100)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == QingYanSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Triggers.Count != 0 ||
                program.Activations is not [{ Id: QingYanActivation, MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0,
                    TargetKind: SkillProgramTargetKind.AnyLiving, UsesPerTurn: null, UsesPerPhase: null, UsesPerGame: null }] ||
                program.Activations[0].Effects is not [{ Op: SkillProgramEffectOp.ResolveChenJiaoQingYan,
                    Target: SkillProgramEffectTarget.Owner }])
                throw new InvalidOperationException(path +
                    ": QingYan requires exactly one unlimited, targetless play activation resolving the owner operation.");
            return;
        }
        if (program.Id != CeDuanSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Triggers.Count != 0 ||
            program.Activations is not [{ Id: CeDuanActivation, MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1,
                TargetKind: SkillProgramTargetKind.OtherLivingWhoseAttackRangeIncludesOwner, UsesPerTurn: null,
                UsesPerPhase: 1, UsesPerGame: null }] ||
            program.Activations[0].Effects is not [{ Op: SkillProgramEffectOp.ResolveChenJiaoCeDuan,
                Target: SkillProgramEffectTarget.Owner }, { Op: SkillProgramEffectOp.ResolveChenJiaoCeDuanSettle,
                Target: SkillProgramEffectTarget.Owner }])
            throw new InvalidOperationException(path +
                ": CeDuan requires exactly its once-per-play, one in-range-target activation resolving the owner operation.");
    }
}

internal interface IChenJiaoHost
{
    SkillProgramStepOutcome ResolveQingYan(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveCeDuan(ProgramSkillFrame frame);
    SkillProgramStepOutcome SettleCeDuan(ProgramSkillFrame frame);
}

internal sealed class ResolveChenJiaoQingYanDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveChenJiaoQingYan;
    public override ISkillProgramEffectHandler Handler { get; } = new ChenJiaoHandler(SkillProgramEffectOp.ResolveChenJiaoQingYan);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveChenJiaoCeDuanDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveChenJiaoCeDuan;
    public override ISkillProgramEffectHandler Handler { get; } = new ChenJiaoHandler(SkillProgramEffectOp.ResolveChenJiaoCeDuan);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectTarget,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveChenJiaoCeDuanSettleDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveChenJiaoCeDuanSettle;
    public override ISkillProgramEffectHandler Handler { get; } = new ChenJiaoHandler(SkillProgramEffectOp.ResolveChenJiaoCeDuanSettle);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (e, c) => c.Draw(new(SkillProgramEffectOp.Draw, SkillProgramEffectTarget.Owner, 1, e.Condition)));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
}

internal sealed class ChenJiaoHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveChenJiaoQingYan => host.GetCapability<IChenJiaoHost>().ResolveQingYan(frame),
        SkillProgramEffectOp.ResolveChenJiaoCeDuan => host.GetCapability<IChenJiaoHost>().ResolveCeDuan(frame),
        _ => host.GetCapability<IChenJiaoHost>().SettleCeDuan(frame),
    };
}
