using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum MobileDuanfaStage { Cards, CostChildren, Drawing }
public sealed record MobileDuanfaReceipt(CardConversionSource Source, string Hash, SkillGrantPhaseExpiry Phase, MobileDuanfaStage Stage,
    int Limit = 0, int SpentBefore = 0, int ActualCount = 0, long Before = 0, long After = 0)
{
    private IReadOnlyList<int> _cards = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
}
public sealed record MobileDuanfaIssuedEvent(long FrameId, CardConversionSource Source, string Hash, SkillGrantPhaseExpiry Phase) : IGameEvent;
public sealed record MobileDuanfaPaidEvent(long FrameId, CardConversionSource Source, string Hash, SkillGrantPhaseExpiry Phase,
    int Limit, int SpentBefore, int ActualCount, long Before, long After) : IGameEvent;
public sealed record MobileDuanfaDrawReturnedEvent(long FrameId, int Requested, int ActualCount, long Before, long After) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileDuanfaReceipt? MobileDuanfa { get; init; }
}
internal interface IMobileZhouFangHost
{
    SkillProgramStepOutcome Begin(ProgramSkillFrame frame);
    void PublishBait(ProgramSkillFrame frame, string sourceBind);
}
internal sealed class ResolveMobileDuanfaDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileDuanfa;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveMobileDuanfaHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectOwnedCards, static (_, c) => c.PublicControlValue(2));
    public override ProgramFactRequirements FactRequirements => new(Activated: ProgramTrackedFact.NativeLogicalDiscard);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireActivationEntry()];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
}
internal sealed class ResolveMobileDuanfaHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileDuanfa;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) => h.GetCapability<IMobileZhouFangHost>().Begin(f);
}
public sealed record MobileYoudiBaitPublishedEvent(long FrameId, CardConversionSource Source, string Hash, long InvocationId, int CardId, Suit Suit) : IGameEvent;
internal sealed class PublishMobileYoudiBaitDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PublishMobileYoudiBait;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "sourceBind", "condition");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), sourceBind: r.RequiredIdentifier("sourceBind"));
        RequireAlways(e, r.Path); return e;
    }
    public override ISkillProgramEffectHandler Handler { get; } = new PublishMobileYoudiBaitHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeLogicalDiscard);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding), new ReadSingleCardSet(e.SourceBind!)];
}
internal sealed class PublishMobileYoudiBaitHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PublishMobileYoudiBait;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h)
    { h.GetCapability<IMobileZhouFangHost>().PublishBait(f, e.SourceBind!); return SkillProgramStepOutcome.Continue; }
}
internal static class MobileZhouFangComposition
{
    [ProgramCatalogValidation(82300)]
    private static void Validate(string path, SkillProgram p)
    {
        if (p.Triggers.SelectMany(t => t.Effects).Any(e => e.Op == SkillProgramEffectOp.PublishMobileYoudiBait))
        {
            if (p.Id != "mobile:youdi" || p.Activations.Count != 0 || p.Triggers is not [{ Id: "ending-discard-and-rewards", Window: SkillProgramTriggerWindow.TurnEnding,
                Subject: SkillProgramTriggerSubject.Owner, Optional: true, Effects: [
                    { Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherLiving },
                    { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ResultBind: "mobile-youdi-bait", ChooserRef.Kind: ProgramParticipantRef.SelectedTarget, CardOwnerRef.Kind: ProgramParticipantRef.Owner,
                        Zones: [CardZoneKind.Hand], Destination: SkillProgramCardDestination.DiscardPile, FreezeMovedCardSuit: true, AwaitMovementTriggers: true },
                    { Op: SkillProgramEffectOp.PublishMobileYoudiBait, SourceBind: "mobile-youdi-bait", Condition.Kind: SkillProgramConditionKind.Always },
                    { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1 },
                    { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ChooserRef.Kind: ProgramParticipantRef.Owner, CardOwnerRef.Kind: ProgramParticipantRef.SelectedTarget,
                        Zones: [CardZoneKind.Hand, CardZoneKind.Equipment], Destination: SkillProgramCardDestination.SelectedTargetHand, TargetReference.Kind: ProgramParticipantRef.Owner,
                        SkipIfNoCards: true, AwaitMovementTriggers: true } ] }])
                throw new InvalidOperationException(path + ": mobile Youdi requires a real hand discard, its public frozen result, then native draw before chosen HE acquisition.");
        }
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op == SkillProgramEffectOp.ResolveMobileDuanfa)) return;
        if (p.Id != "mobile:duanfa" || p.Triggers.Count != 0 || p.Activations is not [{ Id: "phase-black-recycle", MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0,
            TargetKind: SkillProgramTargetKind.AnyLiving, UsesPerTurn: null, UsesPerPhase: null, UsesPerGame: null, Condition.Kind: SkillProgramConditionKind.Always,
            Effects: [{ Op: SkillProgramEffectOp.ResolveMobileDuanfa, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": mobile Duanfa requires its single native HE payment and actual-Play-phase budget activation.");
    }
}
