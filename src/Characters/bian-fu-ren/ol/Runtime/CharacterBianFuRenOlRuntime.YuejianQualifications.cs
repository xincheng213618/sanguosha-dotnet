#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBianFuRenOlRuntime
{
    internal bool HasBianFuRenYuejian(int owner) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).GetInstanceTriggers(SkillProgramTriggerWindow.DiscardPileReceived).Any(binding => binding.Trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.YuejianRevealAndClaimUsedCard));
    internal int[] MatchingBianFuRenUsedDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => (batch.BianFuRenUsedDiscards ?? []).Where(receipt => receipt.Action.ActorSeat != candidate.OwnerSeat && receipt.Action.EffectiveDesignatedTargetSeats.Contains(candidate.OwnerSeat) && receipt.Materials.Any(BianFuRenMaterialAvailable)).Select(receipt => batch.Movements.Select((move, index) => (move, index)).First(pair => receipt.Materials.Any(material => material.CompletionMovementSequence == pair.move.Sequence)).index).Distinct().ToArray();
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 100)]
    internal int[]? SelectWindowIndexes_MatchingBianFuRenUsedDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.YuejianRevealAndClaimUsedCard))
            return MatchingBianFuRenUsedDiscardIndexes(batch, candidate);
        return null;
    }
}
