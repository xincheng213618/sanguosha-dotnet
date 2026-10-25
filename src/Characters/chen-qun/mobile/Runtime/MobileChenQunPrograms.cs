namespace CardGame.Core;

public sealed record MobileDingpinPreparedEvent(long FrameId, CardConversionSource Source, string Hash,
    ActualDiscardRecoveryPhaseKey Phase, int Target, int CardId, long Before) : IGameEvent;
public sealed record MobileDingpinCompletedEvent(long FrameId, int Owner, int Target, int Turn,
    int TurnOwner, long JudgmentId, Suit? Suit) : IGameEvent;

internal interface IMobileChenQunSelection : IProgramSelectionQuery
{
    bool CardAllowed(int owner, Card card);
    bool TargetAllowed(int owner, int target);
}
internal interface IMobileChenQunHost
{ void Run(ProgramSkillFrame frame, SkillProgramEffectOp operation); }

[ProgramSelectionModule(ProgramSelectionStage.CardAfterDiscard, 77200, SkillProgramEffectOp.PrepareMobileChenQunDingpin)]
internal sealed class MobileChenQunCardFilter : IProgramSelectionCardFilter
{
    public bool Allows(ProgramSelectionContext context, Card card, IProgramSelectionQueries queries) =>
        queries.Get<IMobileChenQunSelection>().CardAllowed(context.OwnerSeat, card);
}
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 77200, SkillProgramEffectOp.PrepareMobileChenQunDingpin)]
internal sealed class MobileChenQunTargetFilter : IProgramSelectionTargetFilter
{
    public bool Allows(ProgramSelectionContext context, int target, IProgramSelectionQueries queries) =>
        queries.Get<IMobileChenQunSelection>().TargetAllowed(context.OwnerSeat, target);
}
internal abstract class MobileChenQunDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler => new MobileChenQunHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(2));
    public override ProgramFactRequirements FactRequirements => new(Activated:
        ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase | ProgramTrackedFact.WholeActualUseDeclaration);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        Op == SkillProgramEffectOp.PrepareMobileChenQunDingpin ? [new ReadSelectedTarget()] :
            [new ReadSelectedTarget(), new ReadCardSet("dingpin-cost")];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) =>
        Op == SkillProgramEffectOp.CompleteMobileChenQunDingpin && context.State.CanContinueAfterOwnerDeath(context.Frame);
}
internal sealed class PrepareMobileChenQunDescriptor : MobileChenQunDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileChenQunDingpin; }
internal sealed class CompleteMobileChenQunDescriptor : MobileChenQunDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.CompleteMobileChenQunDingpin; }
internal sealed class MobileChenQunHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IMobileChenQunHost>().Run(frame, operation); return SkillProgramStepOutcome.Continue; }
}
internal static class MobileChenQunComposition
{
    [ProgramCatalogValidation(77200)]
    private static void Validate(string path, SkillProgram program)
    {
        if (!program.Activations.SelectMany(a => a.Effects).Concat(program.Triggers.SelectMany(t => t.Effects))
            .Any(e => e.Op is SkillProgramEffectOp.PrepareMobileChenQunDingpin or SkillProgramEffectOp.CompleteMobileChenQunDingpin)) return;
        if (program.Id != "mobile:dingpin" || program.Triggers.Count != 0 || program.Activations is not
            [{ Id: "judge-unused-category", MinCards: 1, MaxCards: 1, MinTargets: 1, MaxTargets: 1,
               TargetKind: SkillProgramTargetKind.AnyWounded, UsesPerTurn: null, UsesPerPhase: null,
               Effects: [{ Op: SkillProgramEffectOp.PrepareMobileChenQunDingpin },
                   { Op: SkillProgramEffectOp.CaptureSelectedCards, ResultBind: "dingpin-cost" },
                   { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "dingpin-cost", Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true },
                   { Op: SkillProgramEffectOp.StartJudgment, Target: SkillProgramEffectTarget.SelectedTarget, JudgmentReason: "skill.mobile.dingpin", ResultBind: "dingpin-card", OutcomeBind: "dingpin-outcome" },
                   { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTarget, NumberExpression: SkillProgramNumberExpression.TargetLostHp } draw,
                   { Op: SkillProgramEffectOp.CompleteMobileChenQunDingpin },
                   { Op: SkillProgramEffectOp.TurnOver, Target: SkillProgramEffectTarget.Owner } turnover,
                   { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "dingpin-card", Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true }] } activation] ||
            !activation.SourceZones.SequenceEqual([CardZoneKind.Hand]) || activation.MarkerCost is not null || activation.ContinueAfterOwnerDeath ||
            draw.Condition is not { Kind: SkillProgramConditionKind.JudgmentOutcomeMatchesSuits, SourceBind: "dingpin-outcome" } ||
            !draw.Condition.Suits.SequenceEqual([Suit.Spade, Suit.Club]) ||
            turnover.Condition is not { Kind: SkillProgramConditionKind.JudgmentOutcomeMatchesSuits, SourceBind: "dingpin-outcome" } ||
            !turnover.Condition.Suits.SequenceEqual([Suit.Heart, Suit.Diamond]))
            throw new InvalidOperationException(path + ": mobile Dingpin requires its real hand discard, wounded target, finalized black lost-HP draw, black target ban and red turnover.");
    }
}
