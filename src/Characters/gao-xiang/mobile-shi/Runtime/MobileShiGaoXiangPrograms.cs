using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiZxChiyuanGivenEvent(long FrameId, int OwnerSeat, int ReceiverSeat, int CardId) : IGameEvent;
public sealed record ShiZxChiyuanSlashIssuedEvent(long FrameId, int OwnerSeat, long UseId, int ReceiverSeat, int TargetSeat) : IGameEvent;
public sealed record ShiZxErdiStartedEvent(long FrameId, int OwnerSeat, int TargetSeat, int Turn) : IGameEvent;
public sealed record ShiZxErdiDiscardedEvent(long FrameId, int Seat, int Count) : IGameEvent;

public enum ShiZxStage
{
    PickReceiver = 0, PickSlashTarget = 1, SlashSettling = 2, RevengeSettling = 3,
    ErdiTarget = 4, OwnerDiscarding = 5, TheirDiscarding = 6, ErdiSettling = 7,
}

public sealed record ShiZxReceipt(string Operation, ShiZxStage Stage)
{
    public int CardId { get; init; }
    public int ReceiverSeat { get; init; }
    public int ErdiTargetSeat { get; init; }
    public int[] Selected { get; init; } = [];
    public int OwnerDiscardCount { get; init; }
    public int DamageCount { get; init; }
    public int DamageEventsBefore { get; init; }
    public bool Revenge { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiZxReceipt? GaoXiang { get; init; }
}

internal static class ShiGaoXiangComposition
{
    internal const string ChiyuanSkill = "mobile:shi-chiyuan";
    internal const string ErdiSkill = "mobile:shi-erdi";
    internal const string ChiyuanActivation = "chiyuan-gift";
    internal const string ErdiTrigger = "erdi-ending";
    internal const string ProgramAction = "mobile-shi-gao-xiang";
    internal const string ChiyuanGiveReason = "skill-program.mobile:shi-chiyuan.give";
    internal const string ErdiDiscardReason = "skill-program.mobile:shi-erdi.discard";

    [ProgramCatalogValidation(68740)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == ErdiSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
                program.Triggers is not
                [
                    { Id: ErdiTrigger, Window: SkillProgramTriggerWindow.PlayEnding,
                        Subject: SkillProgramTriggerSubject.Owner, Optional: true, IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiZxErdi, Target: SkillProgramEffectTarget.Owner }] }
                ])
                throw new InvalidOperationException(path +
                    ": ErDi requires exactly its optional owner ending-phase trigger resolving the owner operation.");
            return;
        }
        if (program.Id != ChiyuanSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Triggers.Count != 0 ||
            program.Activations is not [{ Id: ChiyuanActivation, MinCards: 1, MaxCards: 1, MinTargets: 0, MaxTargets: 0,
                SourceZones: [CardZoneKind.Hand], UsesPerPhase: 1, UsesPerTurn: null,
                Effects: [{ Op: SkillProgramEffectOp.ResolveShiZxChiyuan, Target: SkillProgramEffectTarget.Owner }] }])
            throw new InvalidOperationException(path +
                ": ChiYuan requires exactly its once-per-phase one-card zero-target activation.");
    }
}

internal interface IShiGaoXiangHost
{
    SkillProgramStepOutcome ResolveChiyuan(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveErdi(ProgramSkillFrame frame);
}

internal sealed class ResolveShiZxChiyuanDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiZxChiyuan;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiZxHandler(SkillProgramEffectOp.ResolveShiZxChiyuan);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ConsumeSelectedCards(1)];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiZxErdiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiZxErdi;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiZxHandler(SkillProgramEffectOp.ResolveShiZxErdi);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiZxHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiZxChiyuan => host.GetCapability<IShiGaoXiangHost>().ResolveChiyuan(frame),
        _ => host.GetCapability<IShiGaoXiangHost>().ResolveErdi(frame),
    };
}
