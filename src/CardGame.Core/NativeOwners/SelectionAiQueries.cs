#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SelectionAiQueries(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianPoliciesEntries> getCoreGameEnginePindianPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries> getCoreGameEngineNextActualUseTargetAdjustmentsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IProgramSelectionAiState, IProgramSelectionRewardState
{
    public int MarkerCount(int ownerSeat, PlayerMarkerKind marker) => matchParticipantsState.ReferenceToPlayers[ownerSeat].Markers.GetValueOrDefault(marker);
    public IReadOnlyList<Card> OwnerHand(int ownerSeat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[ownerSeat]).Select(ProgramSelectionProjection.Freeze).ToArray());
    public int EffectivePindianRank(int ownerSeat, Card card) => getCoreGameEnginePindianPoliciesRuntime().EffectivePindianRank(matchParticipantsState.ReferenceToPlayers[ownerSeat], card);
    public SkillProgramCardCategory CardCategory(CardKind kind) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind);
    public bool HasNextActualUseAdjustment(int ownerSeat) => getCoreGameEngineNextActualUseTargetAdjustmentsRuntime().CurrentNextActualUseAdjustment(ownerSeat)is not null;
    public bool HasPindianClaimPolicy(int ownerSeat) => getCoreGameEngineProgramCardPoliciesRuntime().HasCardPolicy(matchParticipantsState.ReferenceToPlayers[ownerSeat], SkillProgramCardPolicyKind.PindianClaim);
    public IEnumerable<ProgramTargetCommitContext> Declarations => getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().OfType<ProgramTargetCommittedEvent>().Select(receipt => receipt.Declaration);

    public IEnumerable<SkillProgramEffect> EligibleRewardEffects(int ownerSeat)
    {
        var owner = matchParticipantsState.ReferenceToPlayers[ownerSeat];
        var facts = getCoreGameEngineProgramLifecycleRuntime().CaptureProgramTriggerFacts(owner);
        return getCoreGameEngineProgramLifecycleRuntime().CollectEligibleProgramTriggerCandidates(owner, SkillProgramTriggerWindow.ProgramTargetCommitted, facts).Where(candidate => getCoreGameEngineSkillProgramsRuntime().EnabledSkillPrograms(owner).Any(program => program.Id == candidate.SkillId)).SelectMany(candidate => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger2(candidate).Effects);
    }
}
