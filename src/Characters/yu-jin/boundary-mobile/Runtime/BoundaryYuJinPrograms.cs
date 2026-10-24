namespace CardGame.Core;

internal static class BoundaryYuJinComposition
{
    internal const string Skill = "boundary:jieyue-mobile", Gift = "jieyue-gift", Answer = "jieyue-answer",
        Hand = "jieyue-kept-hand", Equipment = "jieyue-kept-equipment", Remainder = "jieyue-remainder";
    [ProgramCatalogValidation(57600)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.BindBoundaryYuJinRemainder)) return;
        if (p.Id != Skill || p.Activations.Count != 0 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.CardIdentities.Count != 0 || p.CardPolicies.Count != 0 || p.DamageModifiers.Count != 0 ||
            p.Triggers is not [{ Id: "ending-gift-and-recipient-choice", Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner,
                TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: true, Effects: [
                    { Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherLiving },
                    { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ResultBind: Gift, Amount: 1, Destination: SkillProgramCardDestination.SelectedTargetHand, AwaitMovementTriggers: true },
                    { Op: SkillProgramEffectOp.ChooseOption, ResultBind: Answer },
                    { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 3 },
                    { Op: SkillProgramEffectOp.SelectOwnedCards, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1, ResultBind: Hand },
                    { Op: SkillProgramEffectOp.SelectOwnedCards, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1, ResultBind: Equipment },
                    { Op: SkillProgramEffectOp.BindBoundaryYuJinRemainder, ResultBind: Remainder },
                    { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: Remainder, ExceptBind: null, Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true } ] }])
            throw new InvalidOperationException(path + ": mobile Jieyue needs its original Ending gift, recipient option and two native retained-card bindings before one atomic remainder discard.");
        var e = p.Triggers[0].Effects;
        if (e[1].ChooserRef?.Kind != ProgramParticipantRef.Owner || e[1].CardOwnerRef?.Kind != ProgramParticipantRef.Owner || e[1].TargetReference?.Kind != ProgramParticipantRef.SelectedTarget ||
            !e[1].Zones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment]) || e[2].ChooserRef?.Kind != ProgramParticipantRef.SelectedTarget ||
            !e[2].Options.Select(o => o.Id).SequenceEqual(["retain", "draw"]) || !e[4].Zones.SequenceEqual([CardZoneKind.Hand]) || !e[5].Zones.SequenceEqual([CardZoneKind.Equipment]) ||
            e.Take(3).Any(x => x.Condition.Kind != SkillProgramConditionKind.Always) ||
            e.Skip(3).Where((_, index) => index != 0).Any(x => x.Condition.Kind != SkillProgramConditionKind.ChoiceIs || x.Condition.SourceBind != Answer || x.Condition.OptionId != "retain") ||
            e[3].Condition.Kind != SkillProgramConditionKind.ChoiceIs || e[3].Condition.SourceBind != Answer || e[3].Condition.OptionId != "draw")
            throw new InvalidOperationException(path + ": Jieyue's own HE gift, recipient chooser, distinct retained zones and mutually exclusive result branches cannot change.");
    }
}
internal interface IBoundaryYuJinHost { void BindRemainder(ProgramSkillFrame frame, string resultBind); }
internal sealed class BindBoundaryYuJinRemainderDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindBoundaryYuJinRemainder;
    public override ISkillProgramEffectHandler Handler { get; } = new BindBoundaryYuJinRemainderHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.DiscardOwnedZoneCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "resultBind", "condition");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), resultBind: r.RequiredIdentifier("resultBind"));
        if (e.Condition.Kind != SkillProgramConditionKind.ChoiceIs) throw new InvalidOperationException(r.Path + ": Jieyue remainder requires its exact recipient retain branch.");
        return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding), new ReadSelectedTarget(),
            new ReadSingleCardSet(BoundaryYuJinComposition.Hand), new ReadSingleCardSet(BoundaryYuJinComposition.Equipment),
            new CaptureSourceCard(e.ResultBind!, int.MaxValue, false, SkillProgramEffectTarget.SelectedTarget)];
}
internal sealed class BindBoundaryYuJinRemainderHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.BindBoundaryYuJinRemainder;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host)
    { host.GetCapability<IBoundaryYuJinHost>().BindRemainder(f, e.ResultBind!); return SkillProgramStepOutcome.Continue; }
}
