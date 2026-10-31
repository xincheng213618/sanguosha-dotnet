#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSelectionAiRuntime
{
    int IProgramSelectionAiState.MarkerCount(int ownerSeat, PlayerMarkerKind marker) => _coreMatchParticipantsPlayersAccess._players[ownerSeat].Markers.GetValueOrDefault(marker);
    IReadOnlyList<Card> IProgramSelectionAiState.OwnerHand(int ownerSeat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Select(ProgramSelectionProjection.Freeze).ToArray());
    int IProgramSelectionAiState.EffectivePindianRank(int ownerSeat, Card card) => _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    SkillProgramCardCategory IProgramSelectionAiState.CardCategory(CardKind kind) => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind);
    bool IProgramSelectionAiState.HasNextActualUseAdjustment(int ownerSeat) => _coreNextActualUseTargetAdjustmentsOperations.CurrentNextActualUseAdjustment(ownerSeat)is not null;
    bool IProgramSelectionAiState.HasPindianClaimPolicy(int ownerSeat) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramCardPolicyKind.PindianClaim);
    IEnumerable<ProgramTargetCommitContext> IProgramSelectionRewardState.Declarations => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramTargetCommittedEvent>().Select(receipt => receipt.Declaration);

    IEnumerable<SkillProgramEffect> IProgramSelectionRewardState.EligibleRewardEffects(int ownerSeat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner);
        return _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(owner, SkillProgramTriggerWindow.ProgramTargetCommitted, facts).Where(candidate => _coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(program => program.Id == candidate.SkillId)).SelectMany(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects);
    }
}
