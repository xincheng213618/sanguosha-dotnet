namespace CardGame.Core;

internal interface IBoundaryLingTongHost { SkillProgramStepOutcome SelectEquipmentPair(ProgramSkillFrame frame); }
internal interface IBoundaryLingTongQueries : IProgramSelectionQuery { IReadOnlyList<int> MovableSources(int ownerSeat); }

[ProgramTargetResolver(SkillProgramTargetKind.BoundaryLingTongMovableEquipmentSource)]
internal sealed class BoundaryLingTongMovableSourceResolver : IProgramTargetResolver
{
    public IReadOnlyList<int> Resolve(ProgramTargetResolverContext context, IProgramSelectionQueries queries) =>
        queries.Get<IBoundaryLingTongQueries>().MovableSources(context.OwnerSeat);
}

internal sealed class SelectBoundaryLingTongEquipmentPairDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryLingTongEquipmentPairHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.SelectTargets, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), minimumTargets: 2, maximumTargets: 2);
        if (e.Condition is not { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "xuanfeng-branch", OptionId: "move" })
            throw new InvalidOperationException(r.Path + ": mobile Xuanfeng equipment selection requires its move branch.");
        return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new SelectTargetSet(2, 2)];
}
internal sealed class BoundaryLingTongEquipmentPairHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IBoundaryLingTongHost>().SelectEquipmentPair(frame);
}

internal static class BoundaryLingTongComposition
{
    internal const string Skill = "boundary:xuanfeng-mobile";
    [ProgramCatalogValidation(56800)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Any(e => e.Op == SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair)) return;
        if (p.Id != Skill || p.Activations.Count != 0 || p.Triggers.Count != 2 ||
            p.Triggers[0] is not { Id: "equipment-left", Window: SkillProgramTriggerWindow.CardsMoved, Subject: SkillProgramTriggerSubject.Owner, Optional: true,
                SourceZones: [CardZoneKind.Equipment], MovementOccurrence: SkillProgramMovementOccurrence.PerOwnerBatch } ||
            p.Triggers[1] is not { Id: "discard-phase", Window: SkillProgramTriggerWindow.DiscardPhaseEnded, Subject: SkillProgramTriggerSubject.Owner, Optional: true,
                TurnOwnerScope: SkillProgramTurnOwnerScope.Own, AllowOwnDiscardPhaseEnded: true } ||
            p.Triggers.Any(t => t.Effects is not [
                { Op: SkillProgramEffectOp.ChooseOption, Target: SkillProgramEffectTarget.Owner, ResultBind: "xuanfeng-branch", Options:
                    [{ Id: "discard", RequiredTargetKind: null }, { Id: "move", RequiredTargetKind: SkillProgramTargetKind.BoundaryLingTongMovableEquipmentSource }] },
                { Op: SkillProgramEffectOp.ChooseOtherOwnedCardDiscard, ChooserRef.Kind: ProgramParticipantRef.Owner, Zones: [CardZoneKind.Hand, CardZoneKind.Equipment], Condition: { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "xuanfeng-branch", OptionId: "discard" } },
                { Op: SkillProgramEffectOp.ChooseOtherOwnedCardDiscard, ChooserRef.Kind: ProgramParticipantRef.Owner, Zones: [CardZoneKind.Hand, CardZoneKind.Equipment], Condition: { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "xuanfeng-branch", OptionId: "discard" } },
                { Op: SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair },
                { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ChooserRef.Kind: ProgramParticipantRef.Owner, CardOwnerRef.Kind: ProgramParticipantRef.SelectedFirst,
                    TargetReference.Kind: ProgramParticipantRef.SelectedSecond, Zones: [CardZoneKind.Equipment], Destination: SkillProgramCardDestination.SelectedTargetCorrespondingZone,
                    AwaitMovementTriggers: true, ProhibitReplacingEquipment: true, Condition: { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "xuanfeng-branch", OptionId: "move" } }]))
            throw new InvalidOperationException(path + ": mobile Xuanfeng requires its two native loss/discard windows, sequential discards and awaited ordered equipment transfer.");
    }
}

public sealed record BoundaryLingTongEquipmentPairSelectedEvent(long FrameId, int OwnerSeat, string SkillInstanceId, string GameplayHash, int SourceSeat, int DestinationSeat) : IGameEvent;
