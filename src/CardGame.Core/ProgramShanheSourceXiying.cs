using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum ShanheSourceXiyingStage { Cost, CostChildren, Targets, TargetChildren, RewardChildren }
public sealed record ShanheSourceXiyingReceipt(long ParentId, int ActualTurn, int PlayPhaseInstance, ShanheSourceXiyingStage Stage,
    int CostCardId = 0, long CostBefore = 0, long CostAfter = 0, int Index = 0,
    int TargetSeat = -1, int TargetCardId = 0, long TargetBefore = 0, long TargetAfter = 0,
    long? PaidFrameId = null, int? RewardCardId = null)
{
    private IReadOnlyList<int> _seats = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Seats { get => _seats; init => _seats = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ShanheSourceXiyingReceipt? ShanheSourceXiying { get; init; }
}
public sealed record ShanheSourceXiyingPaidEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    int ActualTurn, int PhaseInstance, int CardId, long Before, long After) : IGameEvent;
public sealed record ShanheSourceXiyingTargetPaidEvent(long FrameId, int TargetSeat, int CardId, long Before, long After) : IGameEvent;
public sealed record ShanheSourceXiyingRewardEvent(long FrameId, long PaidFrameId, int OwnerSeat, int? CardId, long Before, long After) : IGameEvent;

internal interface IShanheSourceXiyingHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame); }
internal sealed class ResolveShanheSourceXiyingDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceXiying;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindows([SkillProgramTriggerWindow.PlayPhaseStarting, SkillProgramTriggerWindow.TurnEnding])];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectOwnedCards, static (_, c) => c.PublicControlValue(2));
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveShanheSourceXiyingHandler();
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
}
internal sealed class ResolveShanheSourceXiyingHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceXiying;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) => h.GetCapability<IShanheSourceXiyingHost>().Resolve(f);
}
internal static class ShanheSourceXiyingContract
{
    internal const string Skill = "shanhe:source-xiying-1180",Start = "play-start-pay-and-demand",Ending = "paid-play-ending-reward";
    [ProgramCatalogValidation(62063)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceXiying)) return;
        if (p.Id != Skill || p.Triggers.Count != 2 || p.Activations.Count != 0 || p.Modifiers.Count != 0 || p.ViewAs.Count != 0 || p.CardPolicies.Count != 0 || p.DamageModifiers.Count != 0)
            throw new InvalidOperationException(path + ": source1180 requires only its exact paid PlayStart and issued actual Ending bindings.");
        foreach (var t in p.Triggers)
            if (t.Subject != SkillProgramTriggerSubject.Owner || t.TurnOwnerScope != SkillProgramTurnOwnerScope.Own || t.Condition.Kind != SkillProgramTriggerConditionKind.Always ||
                !(t.Id == Start && t.Window == SkillProgramTriggerWindow.PlayPhaseStarting && t.Optional || t.Id == Ending && t.Window == SkillProgramTriggerWindow.TurnEnding && !t.Optional) ||
                t.Effects is not [{ Op: SkillProgramEffectOp.ResolveShanheSourceXiying, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }])
                throw new InvalidOperationException(path + ": source1180 lost its original native phases, payment or owner binding.");
    }
}
