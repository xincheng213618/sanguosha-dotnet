using CardGame.Core;

internal static partial class AnniversaryMaTieChecks
{
    public static void LethalPreparationSlashReturnsWithoutStartingRetiredPhasesCold()
    {
        var s = Scene.Start(terminal: true); s.Offer(); s.Activate(); s.Menu(AnniversaryMaTieStage.Target);
        var parentId = s.Root!.Id;
        s.Answer(c => Branch(c) == "target" && c.Targets.SequenceEqual([1]));
        s.Menu(AnniversaryMaTieStage.SlashChoice);
        s.Answer(c => Branch(c) == "slash" && c.Cards.Count == 1 && c.Targets.SequenceEqual([1]));
        s.Watch();
        var origin = s.E<AnniversaryMaTieSlashIssuedEvent>().Single(e => e.Origin.FrameId == parentId).Origin;
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == origin.UseId);
        var cardId = use.PhysicalCardIds!.Single();
        var preparation = s.E<ActualDiscardRecoveryPhaseStartedEvent>().Single().Phase;
        s.Need(s.G.State.Winner == Winner.None && preparation.Kind == ActualDiscardRecoveryPhaseKind.Preparation &&
                s.G.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(f =>
                    f.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && f.OwnerSeat == 0) &&
                s.E<AnniversaryMaTieRewardIssuedEvent>().Count(e => e.ParentId == parentId && e.UseId == origin.UseId) == 1 &&
                !s.E<CardUseFinishedEvent>().Any(e => e.ResolutionId == origin.UseId),
            "The real Preparation parent retains its paid physical Slash and once-drawn reward before the lethal native child returns.");
        s.Cold(); s.Continue(); s.Reach(() => s.G.State.Status == EngineStatus.Completed);
        s.Need(s.G.State.Winner != Winner.None && !s.V(1).IsAlive && s.V(0).IsAlive &&
                s.G.ResolutionStack.Count == 0 && s.P is null && s.G.CreateSnapshot(0).ProcessingCardCount == 0 &&
                s.E<AnniversaryMaTieCompletedEvent>().Count(e => e.FrameId == parentId && e.Used == 1 && e.Damaged) == 1 &&
                s.E<AnniversaryMaTieSlashReturnedEvent>().Count(e => e.FrameId == parentId && e.UseId == origin.UseId) == 1 &&
                s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == origin.UseId && e.CardId == cardId) == 1 &&
                s.E<AnniversaryMaTieRewardIssuedEvent>().Count(e => e.ParentId == parentId && e.UseId == origin.UseId) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == cardId && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1,
            "The lethal native Slash, its paid reward and original Preparation instruction all finish once without a post-win phase or extra engine step.");
        s.Need(s.E<ActualDiscardRecoveryPhaseStartedEvent>().Select(e => e.Phase).SequenceEqual([preparation]) &&
                !s.E<DuFuRenPhaseConfirmedEvent>().Any() && !s.E<DuFuRenPhaseIssuedEvent>().Any(),
            "A decided game never starts Judgment or Draw, consumes Shunshi, or invents another actual-phase token after the original paid child.");
        s.Cold();
    }
}
