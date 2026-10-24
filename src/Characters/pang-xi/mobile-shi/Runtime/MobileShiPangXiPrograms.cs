using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiPxXuyeDrawEvent(long FrameId, int OwnerSeat, int ActualTurn, int VictimSeat, int Drawn) : IGameEvent;
public sealed record ShiPxXuyeTopPlacedEvent(long FrameId, int OwnerSeat, int ActualTurn, int VictimSeat, int CardId) : IGameEvent;
public sealed record ShiPxKuangxiangSwappedEvent(long FrameId, int OwnerSeat, int ActualTurn, int TargetSeat,
    IReadOnlyList<int> OwnerTookIds, IReadOnlyList<int> TargetTookIds) : IGameEvent
{
    private readonly IReadOnlyList<int> _ownerTookIds = Array.AsReadOnly(OwnerTookIds.ToArray());
    private readonly IReadOnlyList<int> _targetTookIds = Array.AsReadOnly(TargetTookIds.ToArray());
    public IReadOnlyList<int> OwnerTookIds { get => _ownerTookIds; init => _ownerTookIds = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> TargetTookIds { get => _targetTookIds; init => _targetTookIds = Array.AsReadOnly(value.ToArray()); }
}
public sealed record ShiPxKuangxiangRiderEvent(long FrameId, int OwnerSeat, int ActualTurn, string Side,
    int SelfDrawn) : IGameEvent;
public sealed record ShiPxKuangxiangClearedEvent(long FrameId, int OwnerSeat, int ActualTurn) : IGameEvent;

public enum ShiPxStage { XuyeAsk = 0, XuyePick = 1, RiderAsk = 2, RiderPick = 3 }

public sealed record ShiPxReceipt(string Operation, int ActualTurn, ShiPxStage Stage)
{
    public int VictimSeat { get; init; }
    public string Side { get; init; } = "";
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiPxReceipt? PangXi { get; init; }
}

internal static class ShiPangXiComposition
{
    internal const string XuyeSkill = "mobile:shi-xuye";
    internal const string KuangxiangSkill = "mobile:shi-kuangxiang";
    internal const string XuyeTrigger = "xuye-damage";
    internal const string KuangxiangActivation = "kuangxiang-swap";
    internal const string KuangxiangRiderTrigger = "kuangxiang-loss";
    internal const string KuangxiangSweepTrigger = "kuangxiang-sweep";
    internal const string ProgramAction = "mobile-shi-pang-xi";
    internal const string SwapGiveReason = "skill-program.mobile:shi-kuangxiang.swap-give";
    internal const string SwapTakeReason = "skill-program.mobile:shi-kuangxiang.swap-take";
    internal const string XuyeDrawReason = "skill-program.mobile:shi-xuye.draw";
    internal const string RiderDrawReason = "skill-program.mobile:shi-xuye.rider-draw";

    [ProgramCatalogValidation(68500)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == XuyeSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
                program.Triggers is not
                [
                    { Id: XuyeTrigger, Window: SkillProgramTriggerWindow.AfterDamageApplied, Subject: SkillProgramTriggerSubject.Any,
                        Optional: false, IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiPxXuye, Target: SkillProgramEffectTarget.Owner }] }
                ])
                throw new InvalidOperationException(path +
                    ": XuYe requires exactly its once-per-turn any-subject damage trigger resolving the owner operation.");
            return;
        }
        if (program.Id != KuangxiangSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 ||
            program.Activations is not [{ Id: KuangxiangActivation, MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1,
                TargetKind: SkillProgramTargetKind.OtherLiving, UsesPerTurn: null, UsesPerPhase: 1, UsesPerGame: null,
                Effects: [{ Op: SkillProgramEffectOp.ResolveShiPxKuangxiang, Target: SkillProgramEffectTarget.Owner }] }] ||
            program.Triggers is not
            [
                { Id: KuangxiangRiderTrigger, Window: SkillProgramTriggerWindow.CardsMoved, Subject: SkillProgramTriggerSubject.Owner,
                    Optional: false, IsSkillInvocation: false,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiPxRider, Target: SkillProgramEffectTarget.Owner }] },
                { Id: KuangxiangSweepTrigger, Window: SkillProgramTriggerWindow.PlayPhaseStarting, Subject: SkillProgramTriggerSubject.Owner,
                    Optional: false, IsSkillInvocation: false,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiPxSweep, Target: SkillProgramEffectTarget.Owner }] }
            ])
            throw new InvalidOperationException(path +
                ": KuangXiang requires exactly its once-per-phase swap activation, its hand-loss rider and its own play-start sweep.");
    }
}

internal interface IShiPangXiHost
{
    SkillProgramStepOutcome ResolveXuye(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveKuangxiang(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveRider(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveSweep(ProgramSkillFrame frame);
}

internal sealed class ResolveShiPxXuyeDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiPxXuye;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiPxHandler(SkillProgramEffectOp.ResolveShiPxXuye);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiPxKuangxiangDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiPxKuangxiang;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiPxHandler(SkillProgramEffectOp.ResolveShiPxKuangxiang);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Gift,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiPxRiderDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiPxRider;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiPxHandler(SkillProgramEffectOp.ResolveShiPxRider);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiPxSweepDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiPxSweep;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiPxHandler(SkillProgramEffectOp.ResolveShiPxSweep);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier,
        static (_, _) => { });
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiPxHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiPxXuye => host.GetCapability<IShiPangXiHost>().ResolveXuye(frame),
        SkillProgramEffectOp.ResolveShiPxKuangxiang => host.GetCapability<IShiPangXiHost>().ResolveKuangxiang(frame),
        SkillProgramEffectOp.ResolveShiPxRider => host.GetCapability<IShiPangXiHost>().ResolveRider(frame),
        _ => host.GetCapability<IShiPangXiHost>().ResolveSweep(frame),
    };
}
