namespace CardGame.Core;

internal interface IPublicHandRevealWindows : IProgramRuntimeOperations
{
    void Issue(long parentFrameId, PublicHandRevealClaim claim, SkillProgramTriggerFacts? facts = null);
    bool Begin(long parentFrameId, PublicHandRevealReturnKind returnKind);
    bool Begin(long parentFrameId, string returnKey);
    bool Suspends(long frameId);
    bool ProducerSuspended(long frameId, string returnKey);
}

// The producer owns the reveal and its rule qualification. This capability owns
// issued candidates, suspension, the child cursor and its exact native return.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class PublicHandRevealWindowRuntime(IProgramRuntimeFrames frames,
    IProgramRuntimeEvents events, IMatchParticipantStateAccess participants,
    IProgramContentQueries content, IProgramBindingActions bindings,
    IPublicHandRevealNativeReturns returns,
    EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess outcome) : IPublicHandRevealWindows
{
    public void Issue(long parentFrameId, PublicHandRevealClaim claim, SkillProgramTriggerFacts? facts = null)
    {
        if (frames.Find(parentFrameId) is not { } parent || claim.RevealFrameId != parentFrameId ||
            claim.Source.OwnerSeat < 0 || claim.Source.OwnerSeat >= participants.Players.Count ||
            string.IsNullOrWhiteSpace(claim.Source.SkillInstanceId) ||
            content.Skill(claim.Source.SkillId).Program is not { } program || program.GameplayHash != claim.GameplayHash ||
            !program.Triggers.Any(t => t.Id == claim.Source.BindingId && t.Window == SkillProgramTriggerWindow.PublicHandRevealed))
            throw new InvalidOperationException("A public Hand reveal requires its actual producer and declared source binding.");
        frames.Replace(parent.Id, parent with
        {
            PublicHandRevealClaims = (parent.PublicHandRevealClaims ?? []).Append(claim).ToArray(),
            PublicHandRevealCapturedFacts = (parent.PublicHandRevealCapturedFacts ?? []).Append(facts).ToArray()
        });
    }

    public bool Begin(long parentFrameId, PublicHandRevealReturnKind returnKind) => Begin(parentFrameId, returnKind, null);
    public bool Begin(long parentFrameId, string returnKey) => Begin(parentFrameId, PublicHandRevealReturnKind.Registered, returnKey);

    private bool Begin(long parentFrameId, PublicHandRevealReturnKind returnKind, string? returnKey)
    {
        if (frames.Top is not { PublicHandRevealClaims: { Count: > 0 } all } parent || parent.Id != parentFrameId)
            return false;
        var capturedFacts = parent.PublicHandRevealCapturedFacts;
        if (parent.PublicHandRevealConsumedCount < 0 || parent.PublicHandRevealConsumedCount > all.Count ||
            capturedFacts is null || capturedFacts.Count != all.Count ||
            returnKind == PublicHandRevealReturnKind.Registered && string.IsNullOrWhiteSpace(returnKey))
            throw new InvalidOperationException("A public Hand reveal lost its original producer cursor or native return.");
        var claims = all.Skip(parent.PublicHandRevealConsumedCount).ToArray();
        if (claims.Length == 0) return false;
        var window = new PublicHandRevealWindowFrame(frames.NextId(), parentFrameId, returnKind)
        {
            Claims = claims,
            CapturedFacts = capturedFacts.Skip(parent.PublicHandRevealConsumedCount).ToArray(),
            ReturnKey = returnKey
        };
        window = window with
        {
            InvocationAuthorizations = Array.AsReadOnly(claims.Select((q, index) =>
                new ProgramInvocationAuthorization(window.Id, parentFrameId,
                    SkillProgramTriggerWindow.PublicHandRevealed, Candidate(q, index),
                    RetainOwnerDeath: q.RetainOwnerDeath, UsageReserved: q.UsageReserved)).ToArray())
        };
        frames.ReplaceTop(parent with { PublicHandRevealConsumedCount = all.Count });
        frames.Push(window);
        events.AdvanceFact(new PublicHandRevealWindowStartedEvent(window.Id, parentFrameId, returnKind, claims.Length, returnKey));
        frames.Advance(window.Id);
        return true;
    }

    internal static ProgramTriggerCandidate Candidate(PublicHandRevealClaim claim, int index) =>
        new(claim.Source.OwnerSeat, claim.Source.SkillId, claim.Source.BindingId,
            claim.Source.SkillInstanceId!, claim.GameplayHash, claim.Priority, index);

    public bool Suspends(long frameId)
    {
        foreach (var window in frames.OfType<PublicHandRevealWindowFrame>())
            for (var parent = frames.ParentOf(window.Id); parent is not null; parent = frames.ParentOf(parent.Id))
                if (parent.Id == frameId) return true;
        return false;
    }

    public bool ProducerSuspended(long frameId, string returnKey) =>
        frames.ChildOf(frameId) is PublicHandRevealWindowFrame window &&
        window.ParentFrameId == frameId && window.ReturnKind == PublicHandRevealReturnKind.Registered &&
        window.ReturnKey == returnKey;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5)]
    internal bool PreserveProducer(ProgramSkillFrame frame) =>
        frames.ChildOf(frame.Id) is PublicHandRevealWindowFrame window && window.ParentFrameId == frame.Id;

    [NativeFrameRoute(typeof(PublicHandRevealWindowFrame), 34000)]
    internal void Advance(PublicHandRevealWindowFrame window)
    {
        if (frames.Top?.Id != window.Id || frames.ParentOf(window.Id)?.Id != window.ParentFrameId ||
            window.ClaimIndex < 0 || window.ClaimIndex > window.Claims.Count ||
            window.CapturedFacts.Count != window.Claims.Count)
            throw new InvalidOperationException("A public Hand reveal cannot advance before its owning child returns.");
        while (window.ClaimIndex < window.Claims.Count)
        {
            var claim = window.Claims[window.ClaimIndex];
            if (participants.At(claim.Source.OwnerSeat).IsAlive && outcome._winner == Winner.None)
            {
                var candidate = Candidate(claim, window.ClaimIndex);
                bindings.Begin(candidate, new(SkillProgramTriggerWindow.PublicHandRevealed,
                    window.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: candidate.OwnerSeat,
                    OccurrenceIndex: candidate.OccurrenceIndex, Facts: window.CapturedFacts[window.ClaimIndex])
                    { PublicHandReveal = claim });
                return;
            }
            CompleteCandidate(window);
            window = (PublicHandRevealWindowFrame)frames.Top!;
        }
        frames.Pop(window.Id, ResolutionFrameKind.PublicHandRevealWindow);
        events.AdvanceFact(new PublicHandRevealWindowReturnedEvent(window.Id, window.ParentFrameId));
        returns.Return(window);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 34000)]
    internal bool IssuedCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        context.Window == SkillProgramTriggerWindow.PublicHandRevealed &&
        ProgramInvocations.Find(frames.Find(context.ParentFrameId), candidate, context) is not null;

    [ProgramWindowReturn(SkillProgramTriggerWindow.PublicHandRevealed)]
    internal void ReturnCandidate(ProgramSkillFrame frame, bool completed)
    {
        if (frame.WindowContext is not { PublicHandReveal: { } claim } context ||
            frames.Top is not PublicHandRevealWindowFrame window || window.Id != context.ParentFrameId ||
            window.ClaimIndex >= window.Claims.Count || window.Claims[window.ClaimIndex] != claim)
            throw new InvalidOperationException("A public Hand reveal must return to its original candidate cursor.");
        CompleteCandidate(window);
        frames.Advance(window.Id);
    }

    private void CompleteCandidate(PublicHandRevealWindowFrame window)
    {
        events.AdvanceFact(new PublicHandRevealClaimReturnedEvent(window.Id, window.ClaimIndex, window.Claims[window.ClaimIndex]));
        frames.ReplaceTop(window with { ClaimIndex = window.ClaimIndex + 1 });
    }

    [ProgramViewContributor(ProgramViewContribution.InvariantProcessing, 34000)]
    internal IReadOnlyList<Card> InvariantProcessing(IReadOnlyList<Card> cards)
    {
        var suspended = frames.OfType<ResolutionFrame>().Where(f => Suspends(f.Id)).SelectMany(f => f switch
        {
            CardUseFrame use => use.Action?.PhysicalCards.Select(c => c.CardId) ?? use.PhysicalCardIds ?? [],
            ProgramSkillFrame program => program.CardAttack?.PhysicalCardIds ?? [],
            JudgmentFrame judgment => judgment.CardAttack?.PhysicalCardIds ?? [],
            _ => Enumerable.Empty<int>()
        }).ToHashSet();
        return suspended.Count == 0 ? cards : Array.AsReadOnly(cards.Where(c => !suspended.Contains(c.Id)).ToArray());
    }
}
