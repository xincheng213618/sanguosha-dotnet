using System.Globalization;

namespace CardGame.Core;

internal interface IHellKingProgramHost
{
    SkillProgramStepOutcome ExecuteHellKing(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal abstract class HellKingOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new HellKingEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
}

internal abstract class HellKingBindingDescriptor : HellKingOperationDescriptor
{
    protected virtual bool ReplacesTarget => false;
    protected virtual bool SelectsTarget => true;
    protected virtual IReadOnlyList<string> Choices => ["eligible", "none"];
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "resultBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), resultBind: r.RequiredIdentifier("resultBind"));
        RequireAlways(effect, r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        Array.AsReadOnly((SelectsTarget ? new ProgramResourceOperation[] { ReplacesTarget ? new ReplaceSingleTarget() : new SelectSingleTarget() } : [])
            .Append(new CreateChoiceResult(effect.ResultBind!, Choices)).ToArray());
}
internal sealed class BindHellRandomOtherDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindHellRandomOther;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class BindHellOtherDamageSourceDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindHellOtherDamageSource;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class BindHellPeachPoisonTargetDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindHellPeachPoisonTarget; }
internal sealed class ChooseHellLowestHpOtherDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseHellLowestHpOther; public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class ChooseHellHighestHpOtherDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseHellHighestHpOther; public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class ChooseHellNextHighestHpOtherDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseHellNextHighestHpOther; protected override bool ReplacesTarget => true; public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class ChooseHellReincarnationTargetDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseHellReincarnationTarget; public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice; }
internal sealed class RollHellElementDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.RollHellElement; protected override bool SelectsTarget => false; protected override IReadOnlyList<string> Choices => ["fire", "thunder", "none"];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class RollHellAssaultDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.RollHellAssault; protected override bool SelectsTarget => false; protected override IReadOnlyList<string> Choices => ["nanman", "wanjian"]; }
internal sealed class BindHellRepeatedDamageTargetDescriptor : HellKingBindingDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindHellRepeatedDamageTarget; }
internal sealed class DiscardHellRandomOtherMountDescriptor : HellKingOperationDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.DiscardHellRandomOtherMount; public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterTurnEnded)]; }
internal sealed class RewardHellRecoverySourceDescriptor : HellKingOperationDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewardHellRecoverySource; public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterHpRecovered)]; }
internal sealed class DiscardHellOutsideDrawGainDescriptor : HellKingOperationDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.DiscardHellOutsideDrawGain; public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsGained)]; }
internal sealed class ExchangeHellReincarnationHpDescriptor : HellKingOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ExchangeHellReincarnationHp;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition());
        if (e.Condition.Kind != SkillProgramConditionKind.ChoiceIs) throw new InvalidOperationException("Reincarnation requires its eligible named-target branch.");
        return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new ReadSelectedTarget(), new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class UseHellPreparationAlcoholDescriptor : HellKingOperationDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.UseHellPreparationAlcohol; public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new ReadSelectedTarget(), new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)]; }
internal sealed class SetHellOthersChainedDescriptor : HellKingOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SetHellOthersChained;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class SetHellAllLivingChainedDescriptor : HellKingOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SetHellAllLivingChained;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.OwnerDied)];

    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class TurnHellFaceUpKillerDescriptor : HellKingOperationDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.TurnHellFaceUpKiller; public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.OwnerDied)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class DrawHellPreparationDescriptor : HellKingOperationDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawHellPreparation; public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)]; }
internal sealed class HellKingEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => host.GetCapability<IHellKingProgramHost>().ExecuteHellKing(frame, effect);
}

internal sealed class HellFuduCommittedUseContract : IProgramCommittedUseContract
{
    public string SkillId => "hell:fudu";
    public void ValidateTrigger(SkillProgramTrigger t)
    {
        if (t.Id != "poison-on-other-peach-use" || t.Window != SkillProgramTriggerWindow.CardUseCommitted || t.OwnerRelation != SkillProgramCardActionOwnerRelation.Observer ||
            t.Optional || !t.IncludeResponseUses || t.Subject is not null || t.SourceSkillId is not null || t.SourceViewAsId is not null ||
            t.Condition.Kind != SkillProgramTriggerConditionKind.Always || t.Priority != 0 || t.UsageScope is not null || t.UsageLimit is not null || t.DynamicUsageLimit is not null ||
            t.NamedUsageGroup is not null || t.MarkerCost is not null || t.ChoiceGroup is not null || t.OrderGroup is not null || t.CardKinds is not [CardKind.Peach] ||
            t.CardCategories.Count != 0 || t.SourceZones.Count != 0 || t.DestinationZones.Count != 0 || t.Suits.Count != 0 || t.DamageCardKinds.Count != 0 ||
            t.MovementReasons.Count != 0 || t.ExcludedMovementReasons.Count != 0 || t.ExcludedReasons.Count != 0 || t.JudgmentReasons.Count != 0 || t.JudgmentSource is not null ||
            t.DamageOccurrence is not null || t.MovementOccurrence is not null || t.RequireDamageSource is not null || t.RequireNoCardConversion is not null || t.GainPhaseQualification is not null ||
            t.AllowNoEventTarget || t.NoDyingAtActivation || t.DeferredTurnEndOnly || t.OnlyDesignatedCardTargets || t.AllowOwnDiscardPhaseEnded || t.EvaluateConditionAtResolution ||
            t.IgnoreOwnSkillMovements || t.MovementDiscardOnly || t.SingleActionInstance ||
            t.Effects is not [{ Op: SkillProgramEffectOp.BindHellPeachPoisonTarget, Target: SkillProgramEffectTarget.Owner, ResultBind: "victim", Condition.Kind: SkillProgramConditionKind.Always },
                { Op: SkillProgramEffectOp.LoseHp, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1, Condition: { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "victim", OptionId: "eligible" } }])
            throw new InvalidOperationException("Fudu requires its exact forced observer Peach committed-Use, including rescue Uses, followed only by the selected participant's native HP loss.");
    }
}

// Scalars only: the random draw is committed once before any resulting child.
public sealed record HellKingChoiceRecordedEvent(long ProgramFrameId, int InstructionIndex, int OwnerSeat,
    string ResultBind, string OptionId, int? TargetSeat, int ActualTurn) : IGameEvent;
public sealed record HellFanshiDamageCountedEvent(int ActualTurn, int OwnerSeat, long DamageFrameId) : IGameEvent;
public sealed record HellCurrentHpExchangedEvent(long ProgramFrameId, int OwnerSeat, int OtherSeat,
    int OwnerBefore, int OtherBefore, int OwnerAfter, int OtherAfter) : IGameEvent;
public sealed record HellRandomCardDiscardedEvent(long ProgramFrameId, int OwnerSeat, int CardOwnerSeat, int CardId) : IGameEvent;

public sealed partial class GameEngine
{

}
