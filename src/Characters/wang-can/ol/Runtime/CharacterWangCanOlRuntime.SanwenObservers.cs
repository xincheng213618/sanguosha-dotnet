#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanOlRuntime
{
    internal SanwenGainProof? SanwenContextFor(CardMovementBatchContext batch, int owner)
    {
        var gain = batch.HandGains?.SingleOrDefault(g => g.RecipientSeat == owner);
        if (gain is not { RecipientAlive: true, ActualTurn: > 0 } || !gain.Sources.Any(source => ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(source)).HasOperation(SkillProgramEffectOp.ResolveSanwenActualGainDuplicates)))
            return null;
        var previous = gain.PreviousHand.Select(c => new SanwenNamedEntity(c.CardId, CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(c.EffectiveKind))).ToArray();
        var incoming = gain.Incoming.Select(c => new SanwenNamedEntity(c.CardId, CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(c.EffectiveKind))).Where(c => previous.Any(w => w.Name == c.Name)).ToArray();
        return incoming.Length == 0 ? null : new(gain.BatchId, gain.ActualTurn, owner, incoming, previous.Where(w => incoming.Any(c => c.Name == w.Name)).ToArray());
    }

    internal IReadOnlyList<int> SanwenPayable(SanwenGainProof q) => Array.AsReadOnly(q.Incoming.Where(i => _cardZonesOperations._cardZones.GetLocation(i.CardId) == CardLocation.Hand(q.OwnerSeat) && q.Witnesses.Any(w => w.Name == i.Name && _cardZonesOperations._cardZones.GetLocation(w.CardId) == CardLocation.Hand(q.OwnerSeat)) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(q.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(i.CardId), CardLocation.Hand(q.OwnerSeat), OwnedCardMoveIntent.Discard)).Select(i => i.CardId).ToArray());
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 3700)]
    internal ProgramSkillWindowContext? TrySanwenGain(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.ResolveSanwenActualGainDuplicates))
            return new(trigger.Window, frame.Id, candidate.OwnerSeat, SourceSeat: frame.Batch.OriginOwnerSeat, TargetSeat: candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch)
            {
                Sanwen = SanwenContextFor(frame.Batch, candidate.OwnerSeat)
            };
        return null;
    }

}
