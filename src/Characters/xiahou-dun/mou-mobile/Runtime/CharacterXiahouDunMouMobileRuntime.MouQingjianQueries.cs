#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

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
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouDunMobileDiscardOriginEvent(batch.Id, move.Sequence, move.CardId, _coreNativeUsedDiscardOriginsOperations.IsNativeTrueUseDiscardMovement(batch, move)));
    }

    internal int[] MatchingMouXiahouDunDiscardIndexes(CardMovementBatchContext batch) => batch.Movements.Select((m, i) => (m, i)).Where(x => x.m.To == CardLocation.DiscardPile && x.m.From != x.m.To && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXiahouDunMobileDiscardOriginEvent>().Count(e => e == new MouXiahouDunMobileDiscardOriginEvent(batch.Id, x.m.Sequence, x.m.CardId, false)) == 1).Select(x => x.i).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 31200, SkillProgramEffectOp.CollectMouXiahouDunQingjian, SkillProgramEffectOp.DistributeMouXiahouDunQingjian)]
    internal bool CanOfferMouXiahouDunQingjian(ProgramTriggerLegalityContext gate)
    {
        if (gate.Context.Window == SkillProgramTriggerWindow.PlayEnding)
            return MouXiahouDunPile(gate.Owner.Seat, gate.Candidate.SkillInstanceId)is { } pile && _corePublicPileSourcesOperations.PublicPileCards(pile).Count > 0;
        return gate.Context is { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } && index >= 0 && index < batch.Movements.Count && MatchingMouXiahouDunDiscardIndexes(batch).Contains(index) && _cardZonesOperations._cardZones.GetLocation(batch.Movements[index].CardId) == CardLocation.DiscardPile && MouXiahouDunQingjianCount(gate.Owner.Seat) < MouXiahouDunQingjianCapacity(gate.Owner.Seat);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4700)]
    internal CardMovementBatchContext CompleteMovementBatchRecordMouXiahouDunDiscardOrigins(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        RecordMouXiahouDunDiscardOrigins(completed);
        return completed;
    }
}
