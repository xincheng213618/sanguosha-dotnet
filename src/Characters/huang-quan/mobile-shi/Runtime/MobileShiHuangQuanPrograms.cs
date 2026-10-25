using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiQuanCuixianPlacedEvent(long FrameId, int OwnerSeat, int HolderSeat, long UseId,
    IReadOnlyList<int> CardIds) : IGameEvent;
public sealed record ShiQuanCuixianClaimedEvent(long FrameId, int OwnerSeat, int HolderSeat, long UseId,
    IReadOnlyList<int> CardIds) : IGameEvent;
public sealed record ShiQuanCuixianChosenEvent(long FrameId, int OwnerSeat, int HolderSeat, string Branch,
    IReadOnlyList<int> CardIds, long? DuelUseId) : IGameEvent;
public sealed record ShiQuanGushiZoneUsedEvent(long FrameId, int OwnerSeat, int Turn, CardZoneKind Zone) : IGameEvent;
public sealed record ShiQuanGushiCardClaimedEvent(long FrameId, int OwnerSeat, int CardId, CardZoneKind FromZone) : IGameEvent;
public sealed record ShiQuanGushiReplacedEvent(long FrameId, int OwnerSeat, int HolderSeat,
    IReadOnlyList<int> GivenIds, IReadOnlyList<int> TakenIds) : IGameEvent;

public enum ShiQuanStage
{
    PickOwn = 0, PickHolder = 1, PlaceChildren = 2,
    CuixianChoice = 3, DuelIssued = 4,
    GushiChoice = 5, GushiPickHolder = 6, GushiPickCards = 7, GushiChildren = 8,
}

public sealed record ShiQuanReceipt(string Operation, ShiQuanStage Stage)
{
    public int HolderSeat { get; init; }
    public long UseId { get; init; }
    public CardZoneKind Zone { get; init; }
    public int[] Placed { get; init; } = [];
    public int[] Chosen { get; init; } = [];
    public int Taken { get; init; }
    public ShiQuanDuelOrigin? Duel { get; init; }
}

/// <summary>Typed origin of the zero-entity Duel issued by 摧先② at a holder's turn start.</summary>
public sealed record ShiQuanDuelOrigin(long ParentProgramFrameId, int InstructionIndex, long CardUseFrameId,
    int OwnerSeat, int HolderSeat, string SkillId, string BindingId, string SkillInstanceId, string GameplayHash,
    int TurnNumber, bool AttackStarted = false);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiQuanReceipt? ShiQuan { get; init; }
}

internal static class ShiHuangQuanComposition
{
    internal const string CuixianSkill = "mobile:shi-cuixian";
    internal const string GushiSkill = "mobile:shi-gushi";
    internal const string PlaceTrigger = "cuixian-place";
    internal const string ClaimTrigger = "cuixian-claim";
    internal const string TurnTrigger = "cuixian-turn";
    internal const string ZoneHandTrigger = "gushi-hand";
    internal const string ZoneEquipmentTrigger = "gushi-equipment";
    internal const string ZoneJudgmentTrigger = "gushi-judgment";
    internal const string ProgramAction = "mobile-shi-huang-quan";
    internal const string PileId = "mobile-shi-huang-quan.cuixian";
    internal const string PlaceReason = "skill-program.mobile:shi-cuixian.place";
    internal const string ClaimReason = "skill-program.mobile:shi-cuixian.claim";
    internal const string GainReason = "skill-program.mobile:shi-cuixian.gain";
    internal const string GiveReason = "skill-program.mobile:shi-cuixian.give";
    internal const string GushiGainReason = "skill-program.mobile:shi-gushi.gain";
    internal const string GushiReplaceInReason = "skill-program.mobile:shi-gushi.replace-in";
    internal const string GushiReplaceOutReason = "skill-program.mobile:shi-gushi.replace-out";
    internal const string DeathReason = "skill-program.mobile:shi-cuixian.holder-death";

    internal static readonly CardKind[] DamageKinds =
        [CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash, CardKind.Duel,
         CardKind.FireAttack, CardKind.BarbarianAssault, CardKind.ArrowBarrage, CardKind.UnexpectedAssault];

    internal static bool IsDamageKind(CardKind kind) => DamageKinds.Contains(kind);

    internal static CardLocation Pile(int holderSeat) =>
        new(CardZoneKind.PrivatePersistentPile, holderSeat, PileId);

    [ProgramCatalogValidation(68770)]
    internal static void ValidateCuixian(string path, SkillProgram program)
    {
        if (program.Id != CuixianSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
            program.Triggers is not
            [
                { Id: PlaceTrigger, Window: SkillProgramTriggerWindow.CardUseTargetsFinalized,
                    OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, Optional: true, IsSkillInvocation: false,
                    IncludeResponseUses: false, Condition: { Kind: SkillProgramTriggerConditionKind.All } },
                { Id: ClaimTrigger, Window: SkillProgramTriggerWindow.CardUseCompleted,
                    OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, Optional: true, IsSkillInvocation: false,
                    IncludeResponseUses: true, Condition: { Kind: SkillProgramTriggerConditionKind.Always } },
                { Id: TurnTrigger, Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
                    Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.AnyLiving,
                    Optional: true, IsSkillInvocation: false,
                    Condition: { Kind: SkillProgramTriggerConditionKind.Always } }
            ] ||
            program.Triggers.Any(trigger => trigger.Effects is not
                [{ Op: SkillProgramEffectOp.ResolveShiHqCuixian, Target: SkillProgramEffectTarget.Owner }]))
            throw new InvalidOperationException(path +
                ": CuiXian requires exactly its placement, claim and foreign turn-start triggers resolving the owner operation.");
        var place = program.Triggers[0];
        if (!place.CardKinds.OrderBy(kind => kind).SequenceEqual(DamageKinds.OrderBy(kind => kind)) ||
            place.Condition is not { Kind: SkillProgramTriggerConditionKind.All, Children: var children } ||
            children is not
            [
                { Kind: SkillProgramTriggerConditionKind.Compare, Comparison: SkillProgramComparisonOperator.GreaterThanOrEqual,
                    Left: { Kind: SkillProgramTriggerValueKind.CardActionPhysicalCardCount },
                    Right: { Kind: SkillProgramTriggerValueKind.IntegerConstant, Value: 1 } },
                { Kind: SkillProgramTriggerConditionKind.Compare, Comparison: SkillProgramComparisonOperator.Equal,
                    Left: { Kind: SkillProgramTriggerValueKind.CardUseDesignatedTargetCount },
                    Right: { Kind: SkillProgramTriggerValueKind.IntegerConstant, Value: 1 } }
            ])
            throw new InvalidOperationException(path +
                ": CuiXian placement requires the damage kinds and the physical-single-target condition.");
    }

    [ProgramCatalogValidation(68780)]
    internal static void ValidateGushi(string path, SkillProgram program)
    {
        if (program.Id != GushiSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
            program.Triggers is not
            [
                { Id: ZoneHandTrigger, Window: SkillProgramTriggerWindow.CardsMoved,
                    Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                    SourceZones: [CardZoneKind.Hand], MovementOccurrence: SkillProgramMovementOccurrence.PerCard,
                    Condition: { Kind: SkillProgramTriggerConditionKind.Always } },
                { Id: ZoneEquipmentTrigger, Window: SkillProgramTriggerWindow.CardsMoved,
                    Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                    SourceZones: [CardZoneKind.Equipment], MovementOccurrence: SkillProgramMovementOccurrence.PerCard,
                    Condition: { Kind: SkillProgramTriggerConditionKind.Always } },
                { Id: ZoneJudgmentTrigger, Window: SkillProgramTriggerWindow.CardsMoved,
                    Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                    SourceZones: [CardZoneKind.Judgment], MovementOccurrence: SkillProgramMovementOccurrence.PerCard,
                    Condition: { Kind: SkillProgramTriggerConditionKind.Always } }
            ] ||
            program.Triggers.Any(trigger => trigger.Effects is not
                [{ Op: SkillProgramEffectOp.ResolveShiHqGushi, Target: SkillProgramEffectTarget.Owner }]))
            throw new InvalidOperationException(path +
                ": GuShi requires exactly its locked per-card lost-zone movement triggers resolving the owner operation.");
    }
}

internal interface IShiHuangQuanHost
{
    SkillProgramStepOutcome ResolveCuixian(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveGushi(ProgramSkillFrame frame);
}

internal sealed class ResolveShiHqCuixianDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiHqCuixian;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiQuanHandler(SkillProgramEffectOp.ResolveShiHqCuixian);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiHqGushiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiHqGushi;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiQuanHandler(SkillProgramEffectOp.ResolveShiHqGushi);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiQuanHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiHqCuixian => host.GetCapability<IShiHuangQuanHost>().ResolveCuixian(frame),
        _ => host.GetCapability<IShiHuangQuanHost>().ResolveGushi(frame),
    };
}
