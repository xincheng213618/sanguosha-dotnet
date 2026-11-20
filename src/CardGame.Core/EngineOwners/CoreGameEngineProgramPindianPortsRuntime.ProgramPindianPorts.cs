#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPindianPortsRuntime
{
    bool IProgramPindianPolicyQueries.ProhibitsPindianTarget(int seat) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.ProhibitPindianTarget);
    bool IProgramPindianPolicyQueries.CanDiscardForPindian(int seat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[seat];
        return owner.IsAlive && _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.PindianDiscardForMaximumDeck)
            .Any(binding => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 ||
                _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(card =>
                    !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, card,
                        CardLocation.Equipment(seat), OwnedCardMoveIntent.Discard) &&
                    !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(seat,
                        binding.Source.SkillId, binding.Source.SkillInstanceId, card)));
    }
    void IProgramPindianProducer.Begin(long programFrameId, SkillPromptPresentation presentation, int sourceSeat, int opponentSeat, string resultBind, SkillProgramCardSetVisibility visibility) => _corePindianOperations.BeginSharedPindian(programFrameId, presentation, sourceSeat, opponentSeat, programResultBind: resultBind, programResultVisibility: visibility);
}
