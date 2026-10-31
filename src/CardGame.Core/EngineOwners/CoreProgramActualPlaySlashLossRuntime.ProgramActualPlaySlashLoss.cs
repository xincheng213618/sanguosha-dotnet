#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramActualPlaySlashLossRuntime
{
    internal bool TracksActualPlaySlashLoss => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerValue(SkillProgramTriggerValueKind.CurrentActualPlayPhysicalSlashLossCount);

    internal ActualPlayPhysicalSlashLossStamp? CaptureActualPlaySlashLoss(Card card, CardLocation from, CardLocation to)
    {
        if (!TracksActualPlaySlashLoss || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber < 1 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId < 1 || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) || from.OwnerSeat is not { } owner || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || to.OwnerSeat == owner)
            return null;
        var actor = _programPhaseScheduleOperations._programPhaseSchedule is { Phase: TurnPhase.Play } schedule ? schedule.Frame.ZhangShiPingHongji is { Stage: HongjiStage.ExtraPhase } hongji ? hongji.RecipientSeat : schedule.Frame.OwnerSeat : _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat;
        return actor == owner ? new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, actor, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id) : null;
    }

    internal int CurrentActualPlaySlashLossCount(int owner) => !TracksActualPlaySlashLoss || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || owner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat ? 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.ActualPlaySlashLoss is { } stamp && stamp.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && stamp.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && stamp.PhaseActorSeat == owner && stamp.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && m.TurnNumber == stamp.ActualTurnNumber && m.From.OwnerSeat == owner && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && m.To.OwnerSeat != owner && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(m.CardKind));
}
