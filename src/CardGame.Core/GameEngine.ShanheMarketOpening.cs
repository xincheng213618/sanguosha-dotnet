namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool TryBeginShanheMarketOpening(ProgramSkillFrame execution, ShanheMarketPurchaseFrame market,
        IReadOnlyList<string> instanceIds)
    {
        if (instanceIds.Count == 0) return false;
        var ids = instanceIds.ToHashSet(StringComparer.Ordinal);
        var facts = CaptureProgramTriggerFacts(_players[0]);
        var candidates = CollectEligibleProgramTriggerCandidates(_players[0], SkillProgramTriggerWindow.GameStarting, facts)
            .Where(c => ids.Contains(c.SkillInstanceId)).ToArray();
        if (candidates.Length == 0) return false;
        var window = new ProgramLifecycleTriggerWindowFrame(++_resolutionSequence, 0,
            SkillProgramTriggerWindow.GameStarting, Array.AsReadOnly(candidates),
            ProgramLifecycleContinuation.ResumeParentProgram, facts)
        { ResumeProgramFrameId = execution.Id,
            ShanheMarketOpening = new(market.Id, execution.Id, market.RewardId) };
        PushRuntimeFrame(window);
        AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    private bool IsExactShanheMarketOpeningWindow(ProgramLifecycleTriggerWindowFrame window)
    {
        if (window.ShanheMarketOpening is not { } origin || window.Window != SkillProgramTriggerWindow.GameStarting ||
            window.OwnerSeat != 0 || window.Continuation != ProgramLifecycleContinuation.ResumeParentProgram ||
            window.ResumeProgramFrameId != origin.ExecutionFrameId || window.Candidates.Count == 0 ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            _resolutionStack.SingleOrDefault(f => f.Id == origin.ExecutionFrameId) is not
                ProgramSkillFrame { ShanheMarketApplication: { Stage: ShanheMarketApplicationStage.Materials } receipt } execution ||
            receipt.PurchaseFrameId != origin.PurchaseFrameId || receipt.RewardId != origin.RewardId ||
            !window.Candidates.All(c => c.OwnerSeat == 0 && receipt.NewSkillInstanceIds.Contains(c.SkillInstanceId, StringComparer.Ordinal))) return false;
        var market = RequireShanheMarketProgram(execution);
        var index = _resolutionStack.FindIndex(f => f.Id == execution.Id);
        return market.Id == origin.PurchaseFrameId && market.RewardId == origin.RewardId &&
            index >= 0 && index + 1 < _resolutionStack.Count && _resolutionStack[index + 1].Id == window.Id;
    }

    // A paid purchase may temporarily suspend a real damage/response subtree.
    // Only descendants of that exact new purchase own its additional children.
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 62030)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 62030)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 62030)]
    private bool HasShanheMarketPurchaseObserver(long id)
    {
        var market = _resolutionStack.OfType<ShanheMarketPurchaseFrame>().SingleOrDefault();
        if (market is null || !ValidShanheMarketPurchase(market)) return false;
        return _resolutionStack.FindIndex(f => f.Id == id) > _resolutionStack.FindIndex(f => f.Id == market.Id);
    }

}
