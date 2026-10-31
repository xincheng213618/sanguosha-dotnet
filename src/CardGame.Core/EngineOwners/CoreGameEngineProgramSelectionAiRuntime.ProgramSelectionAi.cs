#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSelectionAiRuntime
{
    int global::CardGame.Core.IProgramSelectionAiState.MarkerCount(int ownerSeat, PlayerMarkerKind marker) => _coreMatchParticipantsPlayersAccess._players[ownerSeat].Markers.GetValueOrDefault(marker);
    IReadOnlyList<Card> global::CardGame.Core.IProgramSelectionAiState.OwnerHand(int ownerSeat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Select(ProgramSelectionProjection.Freeze).ToArray());
    int global::CardGame.Core.IProgramSelectionAiState.EffectivePindianRank(int ownerSeat, Card card) => _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    SkillProgramCardCategory global::CardGame.Core.IProgramSelectionAiState.CardCategory(CardKind kind) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind);
    bool global::CardGame.Core.IProgramSelectionAiState.HasNextActualUseAdjustment(int ownerSeat) => _coreNextActualUseTargetAdjustmentsOperations.CurrentNextActualUseAdjustment(ownerSeat)is not null;
    bool global::CardGame.Core.IProgramSelectionAiState.HasPindianClaimPolicy(int ownerSeat) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramCardPolicyKind.PindianClaim);
    IEnumerable<ProgramTargetCommitContext> global::CardGame.Core.IProgramSelectionRewardState.Declarations => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramTargetCommittedEvent>().Select(receipt => receipt.Declaration);

    IEnumerable<SkillProgramEffect> global::CardGame.Core.IProgramSelectionRewardState.EligibleRewardEffects(int ownerSeat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner);
        return _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(owner, SkillProgramTriggerWindow.ProgramTargetCommitted, facts).Where(candidate => _coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(program => program.Id == candidate.SkillId)).SelectMany(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects);
    }
}
