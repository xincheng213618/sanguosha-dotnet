#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouDunMouMobileRuntime
{
    internal int MouXiahouDunQingjianCount(int seat) => _corePublicPileSourcesOperations.PublicPileSources(seat).Where(p => p.SkillId == MouXiahouDunQingjian).Sum(p => _corePublicPileSourcesOperations.PublicPileCards(p).Count);
    internal int MouXiahouDunQingjianCapacity(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue ? Math.Max(1, _coreMatchParticipantsPlayersAccess._players[seat].Hp - 1) : 0;
    internal PublicPersistentPileSource? MouXiahouDunPile(int seat, string instance) => _corePublicPileSourcesOperations.PublicPileSources(seat).SingleOrDefault(p => p.SkillId == MouXiahouDunQingjian && p.SkillInstanceId == instance);
    internal void RecordMouXiahouDunDiscardOrigins(CardMovementBatchContext batch)
    {
        if (!HasMouXiahouDunCapability)
            return;
        // Capture while the native Use/response producer is still alive.
        // Discard-trigger candidates may be consumed after that producer retires.
        foreach (var move in batch.Movements.Where(m => m.To == CardLocation.DiscardPile && m.From != m.To))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouDunMobileDiscardOriginEvent(batch.Id, move.Sequence, move.CardId, _coreNativeUsedDiscardOriginsOperations.Value.IsNativeTrueUseDiscardMovement(batch, move)));
    }

    private bool IsMouQingjianDiscardCurrent(CardMovementBatchContext batch, CardMovementRecord move) =>
        move.To == CardLocation.DiscardPile && move.From != move.To &&
        _zoneEntries.CurrentEntry(CardLocation.DiscardPile, move.CardId) == move.Sequence &&
        _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileDiscardOriginEvent>()
            .Count(e => e == new MouXiahouDunMobileDiscardOriginEvent(batch.Id, move.Sequence, move.CardId, false)) == 1;

    internal int[] MatchingMouXiahouDunDiscardIndexes(CardMovementBatchContext batch) => batch.Movements
        .Select((move, index) => (move, index))
        .Where(item => IsMouQingjianDiscardCurrent(batch, item.move)).Select(item => item.index).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 31200, SkillProgramEffectOp.CollectMouXiahouDunQingjian, SkillProgramEffectOp.DistributeMouXiahouDunQingjian)]
    internal bool CanOfferMouXiahouDunQingjian(ProgramTriggerLegalityContext gate)
    {
        if (gate.Context.Window == SkillProgramTriggerWindow.PlayEnding)
            return MouXiahouDunPile(gate.Owner.Seat, gate.Candidate.SkillInstanceId)is { } pile && _corePublicPileSourcesOperations.PublicPileCards(pile).Count > 0;
        return gate.Context is { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } &&
            (uint)index < (uint)batch.Movements.Count && IsMouQingjianDiscardCurrent(batch, batch.Movements[index]) &&
            MouXiahouDunQingjianCount(gate.Owner.Seat) < MouXiahouDunQingjianCapacity(gate.Owner.Seat);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4700)]
    internal CardMovementBatchContext CompleteMovementBatchRecordMouXiahouDunDiscardOrigins(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        RecordMouXiahouDunDiscardOrigins(completed);
        return completed;
    }
}
