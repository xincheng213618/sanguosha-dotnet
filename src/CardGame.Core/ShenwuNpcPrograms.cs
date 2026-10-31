namespace CardGame.Core;

internal interface IShenwuNpcProgramHost
{
    SkillProgramStepOutcome ExecuteShenwuNpc(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal sealed class ShenwuMingwanCommittedUseContract : IProgramCommittedUseContract
{
    public string SkillId => "shenwu:mingwan";
    public void ValidateTrigger(SkillProgramTrigger t)
    {
        if (t.Id != "draw-for-later-real-use" || t.Window != SkillProgramTriggerWindow.CardUseCommitted ||
            t.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || t.Optional || !t.IncludeResponseUses ||
            t.Subject is not null || t.SourceSkillId is not null || t.SourceViewAsId is not null ||
            t.Condition.Kind != SkillProgramTriggerConditionKind.Always || t.Priority != 0 ||
            t.UsageScope is not null || t.UsageLimit is not null || t.DynamicUsageLimit is not null ||
            t.NamedUsageGroup is not null || t.MarkerCost is not null || t.ChoiceGroup is not null || t.OrderGroup is not null ||
            t.CardKinds.Count != 0 || t.CardCategories.Count != 0 || t.SourceZones.Count != 0 || t.DestinationZones.Count != 0 ||
            t.Suits.Count != 0 || t.DamageCardKinds.Count != 0 || t.MovementReasons.Count != 0 || t.ExcludedMovementReasons.Count != 0 ||
            t.ExcludedReasons.Count != 0 || t.JudgmentReasons.Count != 0 || t.JudgmentSource is not null ||
            t.DamageOccurrence is not null || t.MovementOccurrence is not null || t.RequireDamageSource is not null ||
            t.RequireNoCardConversion is not null || t.GainPhaseQualification is not null || t.AllowNoEventTarget ||
            t.NoDyingAtActivation || t.DeferredTurnEndOnly || t.OnlyDesignatedCardTargets || t.AllowOwnDiscardPhaseEnded ||
            t.EvaluateConditionAtResolution || t.IgnoreOwnSkillMovements || t.MovementDiscardOnly || t.SingleActionInstance ||
            t.Effects is not [{ Op: SkillProgramEffectOp.DrawShenwuMingwanUse, Target: SkillProgramEffectTarget.Owner,
                Amount: 0, Condition.Kind: SkillProgramConditionKind.Always, NumberExpression: null, SourceBind: null,
                ResultBind: null, ActorReference: null, TargetReference: null }])
            throw new InvalidOperationException("Mingwan requires one exact forced actor committed-Use binding, including actual response Uses, with native own-turn qualification.");
    }
}

internal abstract class ShenwuNpcOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new ShenwuNpcEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
}
internal abstract class ShenwuNpcBindingDescriptor : ShenwuNpcOperationDescriptor
{
    protected abstract IReadOnlyList<string> Choices { get; }
    protected abstract SkillProgramTriggerWindow Window { get; }
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "resultBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), resultBind: r.RequiredIdentifier("resultBind"));
        RequireAlways(effect, r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(Window), new SelectSingleTarget(), new CreateChoiceResult(effect.ResultBind!, Choices)];
}
internal sealed class DrawShenwuWuzangReplacementDescriptor : ShenwuNpcOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawShenwuWuzangReplacement;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseStarting)];
}
internal sealed class DrawShenwuCangmuReplacementDescriptor : ShenwuNpcOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawShenwuCangmuReplacement;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseStarting)];
}
internal sealed class ResolveShenwuYinzeiDescriptor : ShenwuNpcOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShenwuYinzei;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}
internal sealed class BindShenwuOtherDamageSourceDescriptor : ShenwuNpcBindingDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindShenwuOtherDamageSource;
    protected override IReadOnlyList<string> Choices => ["eligible", "other"];
    protected override SkillProgramTriggerWindow Window => SkillProgramTriggerWindow.AfterDamageApplied;
}
internal sealed class BindShenwuUndamagingTurnOwnerDescriptor : ShenwuNpcBindingDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindShenwuUndamagingTurnOwner;
    protected override IReadOnlyList<string> Choices => ["eligible", "other"];
    protected override SkillProgramTriggerWindow Window => SkillProgramTriggerWindow.AfterTurnEnded;
}
internal sealed class RecordShenwuMingwanVictimDescriptor : ShenwuNpcOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecordShenwuMingwanVictim;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}
internal sealed class DrawShenwuMingwanUseDescriptor : ShenwuNpcOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawShenwuMingwanUse;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted)];
}
internal sealed class PreventShenwuNitaiOwnTurnDamageDescriptor : ShenwuNpcOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PreventShenwuNitaiOwnTurnDamage;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
}
internal sealed class LoseShenwuTanyuHpDescriptor : ShenwuNpcOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.LoseShenwuTanyuHp;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding)];
}
internal sealed class BindShenwuRecoveryBeneficiaryDescriptor : ShenwuNpcBindingDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindShenwuRecoveryBeneficiary;
    protected override IReadOnlyList<string> Choices => ["eligible", "other"];
    protected override SkillProgramTriggerWindow Window => SkillProgramTriggerWindow.AfterHpRecovered;
}
internal sealed class BindShenwuDeathBenefitDescriptor : ShenwuNpcBindingDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindShenwuDeathBenefit;
    protected override IReadOnlyList<string> Choices => ["kill-minion", "fallen-god", "fallen-player", "none"];
    protected override SkillProgramTriggerWindow Window => SkillProgramTriggerWindow.CharacterDied;
}
internal sealed class ShenwuNpcEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShenwuNpcProgramHost>().ExecuteShenwuNpc(frame, effect);
}

/// <summary>Public actual-turn evidence; the target restriction is reconstructed from rule history.</summary>
public sealed record ShenwuMingwanVictimRecordedEvent(int ActualTurn, int OwnerSeat, int VictimSeat, long DamageFrameId, long CardUseFrameId) : IGameEvent;
public sealed record ShenwuYinzeiDiscardedEvent(long ProgramFrameId, int OwnerSeat, int SourceSeat, int CardId) : IGameEvent;

public sealed partial class GameEngine
{
}
