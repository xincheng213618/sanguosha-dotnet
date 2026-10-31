#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanOlRuntime
{
    internal bool HasSanwenCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveSanwenActualGainDuplicates);

    internal IReadOnlyList<SanwenGainProof>? CaptureSanwenGainQualifications(CardMovementBatchContext batch)
    {
        if (!HasSanwenCapability || batch.TurnNumber < 1)
            return null;
        var result = new List<SanwenGainProof>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
        {
            if (!owner.IsAlive || !_coreNationalSkillsOperations.GetSkillBindingShard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.CardsGained).Any(b => ProgramInstructionResolver.Default.Features(b.Trigger).HasOperation(SkillProgramEffectOp.ResolveSanwenActualGainDuplicates)))
                continue;
            var incoming = (batch.NativeDrawInvocation is { } draw && draw.OwnerSeat == owner.Seat ? draw.Materials.Select(m => m.CardId) : batch.Movements.Where(m => m.To == CardLocation.Hand(owner.Seat) && m.From != m.To).Select(m => m.CardId)).Distinct().ToHashSet();
            if (incoming.Count == 0)
                continue;
            var old = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => !incoming.Contains(c.Id)).Select(c => new SanwenNamedEntity(c.Id, global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(owner, c)))).ToArray();
            var gained = incoming.Select(id => new SanwenNamedEntity(id, global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(owner, _coreAdvancedLifecycleOperations.GetAdvancedCard(id))))).Where(c => old.Any(w => w.Name == c.Name)).ToArray();
            if (gained.Length == 0)
                continue;
            result.Add(new(batch.Id, batch.TurnNumber, owner.Seat, gained, old.Where(w => gained.Any(g => g.Name == w.Name)).ToArray()));
        }

        return result.Count == 0 ? null : Array.AsReadOnly(result.ToArray());
    }

    internal void CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof)
    {
        if (!HasSanwenCapability)
            return;
        if (!_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof))
            throw new InvalidOperationException("Sanwen requires the exact completed native draw invocation.");
        for (var index = 0; index < _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Count; index++)
        {
            var batch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[index];
            if (batch.ParentFrameId != proof.ParentFrameId || !batch.Movements.Any(m => proof.Materials.Any(p => p.CardId == m.CardId && p.MovementSequence == m.Sequence)))
                continue;
            batch = batch with
            {
                SanwenGains = null
            };
            if (batch.Id == proof.LastBatchId)
                batch = batch with
                {
                    SanwenGains = CaptureSanwenGainQualifications(batch)
                };
            _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[index] = batch;
        }
    }

    internal bool SanwenIncomingBelongs(CardMovementBatchContext batch, int owner, int id) => batch.NativeDrawInvocation is { } draw ? draw.OwnerSeat == owner && draw.LastBatchId == batch.Id && _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw) && draw.Materials.Any(m => m.CardId == id) : batch.Movements.Any(m => m.CardId == id && m.From != m.To && m.To == CardLocation.Hand(owner));
    internal SanwenGainProof? SanwenContextFor(CardMovementBatchContext batch, int owner) => batch.SanwenGains?.SingleOrDefault(p => p.OwnerSeat == owner);
    internal bool ValidSanwenProof(SanwenGainProof q, CardMovementBatchContext batch) => q.BatchId == batch.Id && q.ActualTurn == batch.TurnNumber && q.ActualTurn > 0 && batch.SanwenGains?.Contains(q) == true && q.Incoming.Count > 0 && q.Witnesses.Count > 0 && q.Incoming.Select(e => e.CardId).Distinct().Count() == q.Incoming.Count && q.Witnesses.All(w => q.Incoming.All(i => i.CardId != w.CardId)) && q.Incoming.All(i => SanwenIncomingBelongs(batch, q.OwnerSeat, i.CardId) && q.Witnesses.Any(w => w.Name == i.Name));
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

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 1100)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureSanwenGainQualifications(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        if (CaptureSanwenGainQualifications(completed)is { } sanwen)
            return completed with
            {
                SanwenGains = sanwen
            };
        return completed;
    }
}
