using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum SourceExecutionerHuanfuStage { Cards, CostChildren, Drawing }
public sealed record SourceExecutionerHuanfuBet(long UseId, long ActionId, CardConversionSource Source,
    string GameplayHash, long ProgramFrameId, int ActorSeat, bool ActorWager, int MaxHpAtPayment,
    int Amount, long SequenceBefore, long SequenceAfter, int DamageTotal = 0, bool Settled = false)
{
    private IReadOnlyList<int> _cards = Array.AsReadOnly(Array.Empty<int>());
    private IReadOnlyList<int> _targets = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> Targets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
}
public sealed record SourceExecutionerHuanfuReceipt(long UseId, long ActionId, SourceExecutionerHuanfuStage Stage,
    bool ActorWager, int Limit, long SequenceBefore = 0, long SequenceAfter = 0, int ActualCount = 0,
    long? PaidProgramFrameId = null)
{
    private IReadOnlyList<int> _cards = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record CardUseFrame
{
    private readonly IReadOnlyList<SourceExecutionerHuanfuBet>? _sourceExecutionerHuanfuBets;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<SourceExecutionerHuanfuBet>? SourceExecutionerHuanfuBets
    {
        get => _sourceExecutionerHuanfuBets;
        init => _sourceExecutionerHuanfuBets = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SourceExecutionerHuanfuReceipt? SourceExecutionerHuanfu { get; init; }
}
public sealed record SourceExecutionerYishiPreventedEvent(long ProgramFrameId, long DamageFrameId,
    long UseId, int OwnerSeat, int TargetSeat, int Amount) : IGameEvent;
public sealed record SourceExecutionerHuanfuPaidEvent(long ProgramFrameId, long UseId, long ActionId,
    CardConversionSource Source, string GameplayHash, int Amount, int MaxHpAtPayment,
    long SequenceBefore, long SequenceAfter) : IGameEvent;
public sealed record SourceExecutionerHuanfuSettledEvent(long ProgramFrameId, long PaidProgramFrameId,
    long UseId, int OwnerSeat, int Wager, int ActualDamage, int RequestedDraw) : IGameEvent;
public sealed record SourceExecutionerHuanfuDrawReturnedEvent(long ProgramFrameId, long UseId,
    int Requested, int ActualCount) : IGameEvent;

internal interface ISourceExecutionerHost
{
    SkillProgramStepOutcome Yishi(ProgramSkillFrame frame);
    SkillProgramStepOutcome Huanfu(ProgramSkillFrame frame);
}
internal sealed class ResolveSourceExecutionerYishiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveSourceExecutionerYishi;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(2));
    public override ISkillProgramEffectHandler Handler { get; } = new SourceExecutionerYishiHandler();
}
internal sealed class SourceExecutionerYishiHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveSourceExecutionerYishi;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int t, ISkillProgramEffectHost h) => h.GetCapability<ISourceExecutionerHost>().Yishi(f);
}
internal sealed class ResolveSourceExecutionerHuanfuDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveSourceExecutionerHuanfu;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeLogicalDiscard);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindows([SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramTriggerWindow.CardUseCompleted])];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectOwnedCards,
        static (_, context) => context.PublicControlValue(1));
    public override ISkillProgramEffectHandler Handler { get; } = new SourceExecutionerHuanfuHandler();
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
}
internal sealed class SourceExecutionerHuanfuHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveSourceExecutionerHuanfu;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int t, ISkillProgramEffectHost h) => h.GetCapability<ISourceExecutionerHost>().Huanfu(f);
}
internal static class SourceExecutionerContract
{
    internal const string Yishi = "shanhe:source-yishi-611", Huanfu = "shanhe:source-huanfu-3156";
    [ProgramCatalogValidation(62059)]
    internal static void ValidateYishi(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.ResolveSourceExecutionerYishi)) return;
        if (p.Id != Yishi || p.Activations.Count != 0 || p.ViewAs.Count != 0 || p.CardPolicies.Count != 0 || p.DamageModifiers.Count != 0 || p.Modifiers.Count != 0 ||
            p.Triggers is not [{ Id: "prevent-slash-and-take", Window: SkillProgramTriggerWindow.BeforeDamageApplied,
                Subject: SkillProgramTriggerSubject.DamageSource, Optional: true, Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [{ Op: SkillProgramEffectOp.ResolveSourceExecutionerYishi, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always },
                    { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ChooserRef.Kind: ProgramParticipantRef.Owner,
                      CardOwnerRef.Kind: ProgramParticipantRef.EventTarget, Zones: [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment],
                      Amount: 1, Destination: SkillProgramCardDestination.OwnerHand, SkipIfNoCards: true, AwaitMovementTriggers: true,
                      Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": source611 Yishi requires an optional real Slash prevention followed by native HEJ acquisition.");
    }
    [ProgramCatalogValidation(62060)]
    internal static void ValidateHuanfu(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.ResolveSourceExecutionerHuanfu)) return;
        if (p.Id != Huanfu || p.Activations.Count != 0 || p.ViewAs.Count != 0 || p.CardPolicies.Count != 0 || p.DamageModifiers.Count != 0 || p.Modifiers.Count != 0 || p.Triggers.Count != 3)
            throw new InvalidOperationException(path + ": source3156 Huanfu requires exactly its original actor/target wagers and completion.");
        foreach (var t in p.Triggers)
        {
            var valid = t.Id switch
            {
                "actor-wager" => t.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && t.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor && t.Optional,
                "target-wager" => t.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && t.OwnerRelation == SkillProgramCardActionOwnerRelation.Target && t.Optional,
                "settle-wager" => t.Window == SkillProgramTriggerWindow.CardUseCompleted && t.OwnerRelation == SkillProgramCardActionOwnerRelation.Observer && !t.Optional,
                _ => false
            };
            if (!valid || t.Condition.Kind != SkillProgramTriggerConditionKind.Always || !t.CardKinds.SequenceEqual(new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash }) ||
                t.Effects is not [{ Op: SkillProgramEffectOp.ResolveSourceExecutionerHuanfu, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }])
                throw new InvalidOperationException(path + ": source3156 Huanfu lost its exact native Slash wager binding.");
        }
    }
}
