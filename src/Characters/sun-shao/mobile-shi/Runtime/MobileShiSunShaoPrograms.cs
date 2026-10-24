using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiSsGanjueIssuedEvent(long FrameId, int OwnerSeat, int ActualTurn, long UseId,
    long ActionId, int EquipmentCardId, Suit StrikeSuit, int TargetSeat) : IGameEvent;
public sealed record ShiSsGanjueSettledEvent(long FrameId, int OwnerSeat, int ActualTurn, long UseId,
    int TargetSeat, bool Damaged) : IGameEvent;
public sealed record ShiSsZhujiDiscardedEvent(long FrameId, int OwnerSeat, int ActualTurn, int ChosenCardId,
    Suit Suit, int DiscardedCount) : IGameEvent;
public sealed record ShiSsZhujiEquippedEvent(long FrameId, int OwnerSeat, int ActualTurn, int GainedCardId,
    int? ReplacedCardId) : IGameEvent;
public sealed record ShiSsZhujiBonusEvent(long FrameId, int OwnerSeat, int ActualTurn, string Choice,
    int Amount) : IGameEvent;

public enum ShiSsStage { GanjueMaterial = 0, GanjueIssued = 1, ZhujiAsk = 2, ZhujiHand = 3, ZhujiBonus = 4 }

public sealed record ShiSsReceipt(string Operation, int ActualTurn, ShiSsStage Stage)
{
    public Suit StrikeSuit { get; init; }
    public int EquipmentCardId { get; init; }
    public long UseId { get; init; }
    public int DamageCountBefore { get; init; }
    public int ChosenCardId { get; init; }
    public int DiscardedCount { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiSsReceipt? SunShao { get; init; }
}

internal static class ShiSunShaoComposition
{
    internal const string GanjueSkill = "mobile:shi-ganjue";
    internal const string ZhujiSkill = "mobile:shi-zhuji";
    internal const string GanjueActivation = "ganjue-use";
    internal const string ZhujiTrigger = "zhuji-play-ending";
    internal const string ProgramAction = "mobile-shi-sun-shao";
    internal const string ZhujiDiscardReason = "skill-program.mobile:shi-zhuji.discard";
    internal const string ZhujiGainReason = "skill-program.mobile:shi-zhuji.gain";

    [ProgramCatalogValidation(68400)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == GanjueSkill)
        {
            if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Triggers.Count != 0 ||
                program.Activations is not [{ Id: GanjueActivation, MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1,
                    TargetKind: SkillProgramTargetKind.OtherLiving, UsesPerTurn: null, UsesPerPhase: 1, UsesPerGame: null,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiSsGanjue, Target: SkillProgramEffectTarget.Owner }] }])
                throw new InvalidOperationException(path +
                    ": GanJue requires exactly its once-per-play-phase activation selecting one other living target at start.");
            return;
        }
        if (program.Id != ZhujiSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
            program.Triggers is not
            [
                { Id: ZhujiTrigger, Window: SkillProgramTriggerWindow.PlayEnding, Subject: SkillProgramTriggerSubject.Owner,
                    TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: false, IsSkillInvocation: false,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiSsZhuji, Target: SkillProgramEffectTarget.Owner }] }
            ])
            throw new InvalidOperationException(path +
                ": ZhuJi requires exactly its own play-ending trigger resolving the owner operation.");
    }
}

internal interface IShiSunShaoHost
{
    SkillProgramStepOutcome ResolveGanjue(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveZhuji(ProgramSkillFrame frame);
}

internal sealed class ResolveShiSsGanjueDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiSsGanjue;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiSsHandler(SkillProgramEffectOp.ResolveShiSsGanjue);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs,
        static (_, context) => context.PublicControlValue(2));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiSsZhujiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiSsZhuji;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiSsHandler(SkillProgramEffectOp.ResolveShiSsZhuji);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.DiscardSelected,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiSsHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiSsGanjue => host.GetCapability<IShiSunShaoHost>().ResolveGanjue(frame),
        _ => host.GetCapability<IShiSunShaoHost>().ResolveZhuji(frame),
    };
}
