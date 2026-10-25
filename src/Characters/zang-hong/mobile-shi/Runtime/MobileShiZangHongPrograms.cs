using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiZhLiezhiPaidEvent(long FrameId, int OwnerSeat, int MaxHpAfter) : IGameEvent;
public sealed record ShiZhJueguUsedEvent(long FrameId, int OwnerSeat, int Turn) : IGameEvent;

public sealed record ShiZhReceipt(string Operation, ShiZhStage Stage)
{
    public int DiscardCount { get; init; }
    public int Discarded { get; init; }
}

public enum ShiZhStage { Discarding = 0 }

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiZhReceipt? ZangHong { get; init; }
}

internal static class ShiZangHongComposition
{
    internal const string LiezhiSkill = "mobile:shi-liezhi";
    internal const string JueguSkill = "mobile:shi-juegu";
    internal const string LiezhiActivation = "liezhi-virtual";
    internal const string JueguTrigger = "juegu-targeted";
    internal const string ProgramAction = "mobile-shi-zang-hong";
    internal const string JueguDiscardReason = "skill-program.mobile:shi-juegu.discard";

    [ProgramCatalogValidation(68700)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == JueguSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
                program.Triggers is not
                [
                    { Id: JueguTrigger, Window: SkillProgramTriggerWindow.CardUseTargetsFinalized,
                        OwnerRelation: SkillProgramCardActionOwnerRelation.Target, Optional: true,
                        Condition: { Kind: SkillProgramTriggerConditionKind.Compare,
                            Left: SkillProgramTriggerValue { Kind: SkillProgramTriggerValueKind.CurrentHp },
                            Comparison: SkillProgramComparisonOperator.Equal,
                            Right: SkillProgramTriggerValue { Kind: SkillProgramTriggerValueKind.CurrentMaxHp } },
                        IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiZhJuegu, Target: SkillProgramEffectTarget.Owner }] }
                ])
                throw new InvalidOperationException(path +
                    ": JueGu requires exactly its optional unwounded become-target trigger resolving the owner operation.");
            return;
        }
        if (program.Id != LiezhiSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Triggers.Count != 0 ||
            program.Activations is not [{ Id: LiezhiActivation, MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0,
                UsesPerTurn: 1, UsesPerPhase: null,
                Effects: [{ Op: SkillProgramEffectOp.ResolveShiZhLiezhi, Target: SkillProgramEffectTarget.Owner }] }])
            throw new InvalidOperationException(path +
                ": LieZhi requires exactly its once-per-turn zero-card virtual-basic activation.");
    }
}

internal interface IShiZangHongHost
{
    SkillProgramStepOutcome ResolveLiezhi(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveJuegu(ProgramSkillFrame frame);
}

internal sealed class ResolveShiZhLiezhiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiZhLiezhi;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiZhHandler(SkillProgramEffectOp.ResolveShiZhLiezhi);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiZhJueguDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiZhJuegu;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiZhHandler(SkillProgramEffectOp.ResolveShiZhJuegu);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiZhHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiZhLiezhi => host.GetCapability<IShiZangHongHost>().ResolveLiezhi(frame),
        _ => host.GetCapability<IShiZangHongHost>().ResolveJuegu(frame),
    };
}
