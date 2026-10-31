#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    internal bool HasDuFuRenShunshi => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GiveForNextActualPhases) == true;

    internal bool IsDuFuRenGiftContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.OwnerSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive)
            return false;
        int? excluded;
        if (context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)
        {
            if (c.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c)
                return false;
            excluded = null;
        }
        else
        {
            if (context.Window != SkillProgramTriggerWindow.AfterDamageApplied || c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || context.Amount <= 0 || context.DamageFrameId is not { } damageId || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.ParentFrameId == damageId && w.Id == context.ParentFrameId)is not { } parent || parent.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex].ToProgramCandidate() != c)
                return false;
            excluded = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(parent).IsSourceLess ? null : context.SourceSeat;
        }

        return _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat && p.Seat != excluded) && (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count > 0 || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Any(card => !card.IsGeneralWeapon && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(c.OwnerSeat, c.SkillId, c.SkillInstanceId, card) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(c.OwnerSeat, card, CardLocation.Equipment(c.OwnerSeat), OwnedCardMoveIntent.Transfer)));
    }
}
