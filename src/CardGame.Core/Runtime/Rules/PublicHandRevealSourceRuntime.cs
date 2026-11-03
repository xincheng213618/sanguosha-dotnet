namespace CardGame.Core;

// Qualification occurs at the real show. The issued window later consumes only
// its frozen candidates and never pays their already reserved quota again.
[ProgramRuntimeModule]
internal sealed class PublicHandRevealSourceRuntime(IPublicCardRevealQueries reveals,
    IPublicHandRevealWindows windows, IProgramWindowCandidateQueries candidates,
    IProgramBindingFacts qualification, IProgramTriggerUsageReservations usage,
    IProgramBoundCardFacts cards, IProgramContentQueries content,
    IMatchParticipantStateAccess participants, IProgramRuntimeFrames frames,
    IProgramRuntimeEvents events, IProgramTurnQueries turn,
    EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess setup,
    EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess round)
{
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 34000)]
    internal void Capture(IGameEvent payload)
    {
        if (!content.HasTriggerWindow(SkillProgramTriggerWindow.PublicHandRevealed) ||
            !setup._setupComplete || turn.TurnNumber <= 0) return;
        var (parentId, ids) = reveals.Describe(payload);
        if (ids.Count == 0 || frames.Find(parentId) is null) return;
        foreach (var player in participants.Players.Where(player => player.IsAlive))
        {
            var sources = candidates.Shard(player).GetTriggerCandidates(SkillProgramTriggerWindow.PublicHandRevealed)
                .Select(candidate => (Candidate: candidate, Trigger: qualification.Trigger(candidate)))
                .Where(source => source.Trigger.Invocation?.Source == SkillProgramInvocationSource.PublicHandReveal).ToArray();
            if (sources.Length == 0) continue;
            var capturedFacts = candidates.Capture(player);
            // Keep the native Hand order: trigger/binding order must not reorder
            // different suits shown by the same original producer.
            foreach (var card in cards.Hand(player.Seat).Where(card => ids.Contains(card.Id)))
            {
                var suit = cards.EffectiveSuit(player.Seat, card);
                foreach (var (candidate, trigger) in sources)
                {
                    var claim = new PublicHandRevealClaim(parentId,
                        new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId),
                        candidate.GameplayHash, round._roundNumber, suit, card.Id,
                        RetainOwnerDeath: trigger.Invocation!.OwnerDeath == SkillProgramInvocationOwnerDeath.Retain,
                        Priority: candidate.Priority);
                    var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.PublicHandRevealed,
                        parentId, player.Seat, SourceSeat: player.Seat, TargetSeat: player.Seat,
                        Facts: capturedFacts) { PublicHandReveal = claim };
                    if (!qualification.CanIssue(candidate, context) || !usage.TryReserve(candidate, context, out var fact)) continue;
                    windows.Issue(parentId, claim with { UsageReserved = fact is not null }, capturedFacts);
                    if (fact is not null) events.AdvanceFact(fact);
                }
            }
        }
    }
}
