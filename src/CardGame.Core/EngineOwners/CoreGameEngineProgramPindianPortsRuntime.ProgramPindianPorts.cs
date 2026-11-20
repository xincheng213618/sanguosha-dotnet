#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPindianPortsRuntime
{
    IReadOnlyList<int> IProgramPindianPolicyQueries.PindianOpponents(int sourceSeat)
    {
        var policies = (IProgramPindianPolicyQueries)this;
        if (!policies.HasPindianInput(sourceSeat))
            return [];

        return _coreMatchParticipantsPlayersAccess._players
            .Where(player => player.IsAlive && player.Seat != sourceSeat && policies.CanBePindianTarget(sourceSeat, player.Seat))
            .Select(player => player.Seat)
            .Order()
            .ToArray();
    }

    ProgramLegalityParticipant IProgramPindianPolicyQueries.CreateLegalityParticipant(int seat)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        return new(seat, _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count,
            _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.ProhibitPindianTarget),
            CanDiscardForPindian(player));
    }

    bool IProgramPindianPolicyQueries.HasPindianInput(int seat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[seat];
        return _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 || CanDiscardForPindian(owner);
    }

    bool IProgramPindianPolicyQueries.CanDiscardForPindian(int seat) =>
        CanDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[seat]);

    private bool CanDiscardForPindian(CharacterState owner)
    {
        var seat = owner.Seat;
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
