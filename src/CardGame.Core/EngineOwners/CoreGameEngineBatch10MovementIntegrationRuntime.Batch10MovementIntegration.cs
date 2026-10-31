#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBatch10MovementIntegrationRuntime
{
    internal IReadOnlyDictionary<int, IReadOnlyList<XiaHuiMovementSource>>? CaptureXiaHuiOriginalSources(IEnumerable<CardLocation> sources)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.XiaHuiBlackHandAndGainedEntityRestrictions))
            return null;
        Dictionary<int, IReadOnlyList<XiaHuiMovementSource>>? frozen = null;
        foreach (var source in sources)
        {
            if (source.OwnerSeat is not { } owner || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
                continue;
            var policies = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[owner], SkillProgramCardPolicyKind.XiaHuiBlackHandAndGainedEntityRestrictions).ToArray();
            if (policies.Length == 0)
                continue;
            foreach (var card in _cardZonesOperations._cardZones.CardsAt(source))
            {
                var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card);
                if (suit is not (Suit.Spade or Suit.Club))
                    continue;
                (frozen ??= []).Add(card.Id, Array.AsReadOnly(policies.Select(item => new XiaHuiMovementSource(new(item.Source.SkillId, item.Policy.Id, owner, item.Source.SkillInstanceId), _contentRegistryOperations._contentRegistry.GetSkill(item.Source.SkillId).Program!.GameplayHash, suit)).ToArray()));
            }
        }

        return frozen is null ? null : new System.Collections.ObjectModel.ReadOnlyDictionary<int, IReadOnlyList<XiaHuiMovementSource>>(frozen);
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectBuZhiMovementCandidates(CardMovementBatchContext batch)
    {
        if (batch.Movements.Count < 2 && (batch.NativeDrawInvocation?.ActualCount ?? 0) < 2 && (batch.NativeBuZhiDiscardInvocation?.ActualCount ?? 0) < 2 || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferNativeBatchHandGainOrCardLossBenefit))
            yield break;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
        {
            if (!owner.IsAlive || !_characterBuZhiOlOperations.BuZhiMovementQualifies(batch, owner.Seat))
                continue;
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved))
                if (_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects.Any(e => e.Op == SkillProgramEffectOp.OfferNativeBatchHandGainOrCardLossBenefit))
                    yield return candidate;
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2100)]
    internal void CollectWindow_CollectBuZhiMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectBuZhiMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2200)]
    internal bool ExcludeOrdinaryMovement_OfferNativeBatchHandGainOrCardLossBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferNativeBatchHandGainOrCardLossBenefit);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 4100)]
    internal ProgramSkillWindowContext? TryNativeBatchBenefit(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.OfferNativeBatchHandGainOrCardLossBenefit))
            return new(trigger.Window, frame.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 900)]
    internal void CaptureMovementBatchStartXiaHuiOriginalSources(CardMovementBatchBuilder batch) => batch.XiaHuiOriginalSources = CaptureXiaHuiOriginalSources(batch.SourceCountsBefore.Keys);
}
