#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnHandLimitKindExemptionsRuntime
{
    void ITurnHandLimitKindExemptionProgramHost.GrantTurnHandLimitCardKindExemption(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds) => GrantProgramTurnHandLimitCardKindExemption(frame, cardKinds);
    internal void GrantProgramTurnHandLimitCardKindExemption(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, SkillProgramDamageModifierExpiration expiration = SkillProgramDamageModifierExpiration.CurrentTurnEnd)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return;
        var policy = _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandLimitKindExemption(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), cardKinds, expiration);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnHandLimitCardKindExemptionGrantedEvent(policy));
    }

    internal IReadOnlyList<TurnHandLimitCardKindExemption>? GetTurnHandLimitCardKindExemptionsSnapshot(int ownerSeat)
    {
        var policies = _turnCardUseEffectsOperations._turnCardUseEffects.HandLimitKindExemptions.Where(policy => (policy.Expiration == SkillProgramDamageModifierExpiration.NextOwnerTurnStart || policy.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && policy.TurnSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && policy.Source.OwnerSeat == ownerSeat).ToArray();
        return policies.Length == 0 ? null : Array.AsReadOnly(policies.Select(policy => policy with { CardKinds = Array.AsReadOnly(policy.CardKinds.ToArray()) }).ToArray());
    }
}
