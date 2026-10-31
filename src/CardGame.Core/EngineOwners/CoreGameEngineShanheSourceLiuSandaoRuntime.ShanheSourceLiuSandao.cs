#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceLiuSandaoRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IShanheSourceLiuSandaoHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => ResolveShanheSourceLiuSandao(frame, operation);
    internal int SourceHuchenEnemyKills(int ownerSeat) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PlayerDiedEvent>().Count(fact => fact.KillerSeat == ownerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(fact.VictimSeat) && _coreTeamActivityProgramsOperations.PublicTeamEnemy(ownerSeat, _coreMatchParticipantsPlayersAccess._players[fact.VictimSeat]));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24790, SkillProgramEffectOp.AdjustShanheSourceHuchenDraw)]
    internal bool CanOfferSourceHuchen(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && context.OwnerSeat == candidate.OwnerSeat && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat && SourceHuchenEnemyKills(candidate.OwnerSeat) > 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24791, SkillProgramEffectOp.DiscardShanheSourceZhanshenWeapons)]
    internal bool CanOfferSourceZhanshen(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        return context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == candidate.OwnerSeat && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat && owner.IsAlive && owner.HasHpValue && owner.Hp < owner.MaxHp && _coreMatchParticipantsPlayersAccess._players.Any(player => !player.IsAlive && _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(owner, player));
    }

    internal SkillProgramStepOutcome ResolveShanheSourceLiuSandao(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (operation == SkillProgramEffectOp.AdjustShanheSourceHuchenDraw)
        {
            var count = SourceHuchenEnemyKills(frame.OwnerSeat);
            if (count > 0)
                _coreProgramLifecycleOperations.AdjustProgramNormalDraw(frame, count);
            return SkillProgramStepOutcome.Continue;
        }

        if (operation != SkillProgramEffectOp.DiscardShanheSourceZhanshenWeapons || frame.InstructionIndex != 2 || !_coreGameActivationAwakeningOperations.ExactGameActivationAwakeningParent(frame))
            throw new InvalidOperationException("Source Zhanshen lost its actual paid Preparation awakening.");
        var weapons = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon).ToArray();
        if (weapons.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _corePhysicalMovementFacadesOperations.MoveCards(weapons, CardLocation.Equipment(frame.OwnerSeat), CardLocation.DiscardPile, new("skill-program." + frame.SkillId + ".awakening-weapon-discard"));
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }
}
