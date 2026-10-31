#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCharacterTurnedOverProgramsRuntime
{
    internal void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown)
    {
        var isFaceDown = _coreMatchParticipantsPlayersAccess._players[targetSeat].IsFaceDown;
        if (wasFaceDown == isFaceDown)
            return;
        _coreDiscardedProvenanceOperations.RecordProvenanceFaceUpReset(targetSeat, wasFaceDown);
        _coreCharacterStateProgramsOperations.RecordCharacterStateChange(targetSeat, SkillProgramTriggerWindow.CharacterTurnedOver, turnedOver: new(wasFaceDown, isFaceDown));
    }

    internal static bool CharacterTurnedOverFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CharacterTurnedOverPrograms.CharacterTurnedOverFrameRidesOn(ride, beneath);
    internal bool CharacterTurnedOverDamageObserverRidesOn(int index)
    {
        if (!global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]))
            return false;
        for (var parentIndex = index - 1; parentIndex >= 1; parentIndex--)
        {
            var frame = _resolutionStackOperations._resolutionStack[parentIndex];
            var parent = _resolutionStackOperations._resolutionStack[parentIndex - 1];
            if (CharacterTurnedOverFrameRidesOn(frame, parent))
                return true;
            if (!_coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(frame, parent) && !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(frame, parent) && !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(frame, parent) && !_corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(frame, parent) && !_coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(frame, parent))
                return false;
        }

        return false;
    }
}
