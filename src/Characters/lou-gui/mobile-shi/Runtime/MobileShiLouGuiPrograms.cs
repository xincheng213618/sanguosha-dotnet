using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiLgGuanshaAppliedEvent(long FrameId, int OwnerSeat, int ActualTurn,
    int ReplacedCount, int GainedCount, int DistinctNames) : IGameEvent;
public sealed record ShiLgJiYuCostPaidEvent(long FrameId, int OwnerSeat, int ActualTurn, long PhaseId, int CardId) : IGameEvent;
public sealed record ShiLgJiYuGainedEvent(long FrameId, int OwnerSeat, int ActualTurn, long PhaseId, int CardId,
    int GainOrdinal) : IGameEvent;

public enum ShiLgStage { GuanShaAsk = 0, JiYuCost = 1 }

public sealed record ShiLgReceipt(string Operation, int ActualTurn, ShiLgStage Stage)
{
    public long PhaseId { get; init; }
    public IReadOnlyList<int> GainedIds { get; init; } = [];
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiLgReceipt? ShiLouGui { get; init; }
}

internal static class ShiLouGuiComposition
{
    internal const string GuanShaSkill = "mobile:shi-guansha";
    internal const string JiYuSkill = "mobile:shi-jiyu";
    internal const string GuanShaTrigger = "guansha-play-ending";
    internal const string JiYuActivation = "jiyu-use";
    internal const string ProgramAction = "mobile-shi-lou-gui";
    internal const string GuanShaReplaceReason = "skill-program.mobile:shi-guansha.replace";
    internal const string GuanShaGainReason = "skill-program.mobile:shi-guansha.gain";
    internal const string JiYuCostReason = "skill-program.mobile:shi-jiyu.cost";
    internal const string JiYuGainReason = "skill-program.mobile:shi-jiyu.gain";

    [ProgramCatalogValidation(68300)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == GuanShaSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
                program.Triggers is not
                [
                    { Id: GuanShaTrigger, Window: SkillProgramTriggerWindow.PlayEnding, Subject: SkillProgramTriggerSubject.Owner,
                        TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: false, IsSkillInvocation: false,
                        UsageScope: SkillUsageScope.Game, UsageLimit: 1,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiLgGuanSha, Target: SkillProgramEffectTarget.Owner }] }
                ])
                throw new InvalidOperationException(path +
                    ": GuanSha requires exactly its once-per-game own play-ending trigger resolving the owner operation.");
            return;
        }
        if (program.Id != JiYuSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Triggers.Count != 0 ||
            program.Activations is not [{ Id: JiYuActivation, MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0,
                TargetKind: SkillProgramTargetKind.AnyLiving, UsesPerTurn: null, UsesPerPhase: null, UsesPerGame: null,
                Effects: [{ Op: SkillProgramEffectOp.ResolveShiLgJiYu, Target: SkillProgramEffectTarget.Owner }] }])
            throw new InvalidOperationException(path +
                ": JiYu requires exactly its unlimited targetless play activation whose legality gate enforces the twice-per-phase cap.");
    }
}

internal interface IShiLouGuiHost
{
    SkillProgramStepOutcome ResolveGuanSha(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveJiYu(ProgramSkillFrame frame);
}

internal sealed class ResolveShiLgGuanShaDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiLgGuanSha;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiLgHandler(SkillProgramEffectOp.ResolveShiLgGuanSha);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiLgJiYuDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiLgJiYu;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiLgHandler(SkillProgramEffectOp.ResolveShiLgJiYu);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.DiscardSelected,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiLgHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiLgGuanSha => host.GetCapability<IShiLouGuiHost>().ResolveGuanSha(frame),
        _ => host.GetCapability<IShiLouGuiHost>().ResolveJiYu(frame),
    };
}
