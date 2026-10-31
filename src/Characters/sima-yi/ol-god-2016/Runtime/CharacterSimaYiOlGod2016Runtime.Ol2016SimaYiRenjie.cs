#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiOlGod2016Runtime
{
    internal const string Ol2016SimaRenjie = "ol:renjie-2016";
    internal bool HasOl2016SimaRenjie => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(Ol2016SimaRenjie) || _contentRegistryOperations._contentRegistry.Skills.ContainsKey("happy:renjie");

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40725, SkillProgramEffectOp.ChangeAttributedMarker)]
    internal bool AllowsOl2016SimaDiscardRen(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != Ol2016SimaRenjie || gate.Trigger.Id != "discard-ren")
            return true;
        return gate.WindowContext is { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { MovementTiming: { Phase: TurnPhase.Discard } timing } batch, MovementIndex: { } index } && timing.PhaseActorSeat == gate.OwnerSeat && batch.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && index >= 0 && index < batch.Movements.Count && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(batch.Movements[index]) && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(batch.Movements[index]) == CardLocation.Hand(gate.OwnerSeat);
    }
}
