#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineUniqueHpTargetAnnouncementsRuntime
{
    internal bool HasUniqueHpPeerPrograms => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DiscardDrawAndOfferUniqueHpPeer);

    internal bool IsUniqueHpPeerCandidate(ProgramTriggerCandidate c) => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.DiscardDrawAndOfferUniqueHpPeer }];
    internal ActualUseTargetIdentity? FreezeUniqueHpPeerTarget(CardUseFrame use, int target)
    {
        if (!HasUniqueHpPeerPrograms || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || target == use.SourceSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !use.TargetSeats.Contains(target))
            return null;
        if (use.Action is { Type: CardActionType.Use } action && action.ActorSeat == use.SourceSeat && action.EffectiveKind == use.CardKind)
            return new(use.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, use.CardKind, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, null);
        if (UniqueHpLegacyTargetProducer(use, target)is not { } producer)
            return null;
        return new(use.Id, null, use.SourceSeat, use.SourceSeat, use.CardKind, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, producer);
    }

    internal long? UniqueHpLegacyTargetProducer(CardUseFrame use, int target)
    {
        if (use.Action is not null || !use.TargetSeats.Contains(target) || use.CardAttack?.ProgramSkillCardUseFrameId is not { } producer || !_coreSameTypeActualUseAidOperations.MatchesSameTypeAidLegacyProducer(use, producer) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == producer)?.SelectedTargetSeats is not [var original] || target != original && target != use.CardAttack.TargetSeat && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SameTypeAidTargetAddedEvent>().Any(e => e.RecipientSeat == target && _coreSameTypeActualUseAidOperations.IsSameTypeAidTargetFact(use, e)))
            return null;
        return producer;
    }

    internal void AppendUnannouncedUniqueHpTargets(CardUseFrame supplied, long windowId, List<(ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context)> entries)
    {
        if (!HasUniqueHpPeerPrograms || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(supplied.CardKind))
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(supplied.Id) ?? throw new InvalidOperationException("The target announcement lost its real Slash.");
        var seen = (use.UniqueHpAnnouncedTargets ?? []).ToList();
        foreach (var target in use.TargetSeats.Distinct())
        {
            if (seen.Contains(target) || FreezeUniqueHpPeerTarget(use, target)is not { } identity)
                continue;
            seen.Add(target);
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use = use with { UniqueHpAnnouncedTargets = seen.ToArray() });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UniqueHpTargetAnnouncedEvent(windowId, identity));
            if (_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(use.Id, target))
                continue;
            var facts = use.Action is { } action ? _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[target], action) : _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[target]);
            foreach (var candidate in _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[target], SkillProgramTriggerWindow.OtherActualUseTargeted, facts))
                if (IsUniqueHpPeerCandidate(candidate))
                    entries.Add((candidate, new(SkillProgramTriggerWindow.OtherActualUseTargeted, windowId, target, SourceSeat: identity.ActorSeat, TargetSeat: target, OccurrenceIndex: candidate.OccurrenceIndex, Facts: facts) { ActualUseTarget = identity }));
        }
    }

    internal ActualUseTargetWindowFrame AppendUniqueHpTargetsToCurrentWindow(ActualUseTargetWindowFrame parent)
    {
        if (parent.ReturnKind == ActualUseTargetReturnKind.OrdinaryTrick || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { } use)
            return parent;
        var entries = new List<(ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context)>();
        AppendUnannouncedUniqueHpTargets(use, parent.Id, entries);
        if (entries.Count == 0)
            return parent;
        entries = entries.OrderByDescending(e => e.Candidate.Priority).ThenBy(e => _coreTableOrderOperations.RelativeTableOrder(0, e.Candidate.OwnerSeat)).ThenBy(e => e.Candidate.SkillId, StringComparer.Ordinal).ToList();
        // Preserve the completed prefix, current cursor and all old candidates.
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { Candidates = parent.Candidates.Concat(entries.Select(e => e.Candidate)).ToArray(), Contexts = parent.Contexts.Concat(entries.Select(e => e.Context)).ToArray() });
        return parent;
    }

    internal bool TryBeginUnannouncedUniqueHpTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy)
    {
        if (!HasUniqueHpPeerPrograms || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use)
            return false;
        return _coreActualUseTargetProgramsOperations.StartActualUseTargetWindow(use, [], legacy ? ActualUseTargetReturnKind.LegacyVirtualSlash : ActualUseTargetReturnKind.Slash, null);
    }

    internal bool MatchesAnnouncedUniqueHpLegacyTarget(ActualUseTargetIdentity identity)
    {
        if (!HasUniqueHpPeerPrograms || identity.ActionId is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)is not { } use || use.UniqueHpAnnouncedTargets?.Contains(identity.TargetSeat) != true || identity.LegacyProducerProgramId != UniqueHpLegacyTargetProducer(use, identity.TargetSeat) || _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().LastOrDefault()is not { } parent || parent.ParentFrameId != use.Id || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Contexts.Count != parent.Candidates.Count || !IsUniqueHpPeerCandidate(parent.Candidates[parent.CandidateIndex]) || parent.Contexts[parent.CandidateIndex].ActualUseTarget != identity)
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueHpTargetAnnouncedEvent>().Count(e => e.WindowFrameId == parent.Id && e.Target == identity) == 1;
    }

    internal void AssertUniqueHpTargetAnnouncements()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.UniqueHpAnnouncedTargets is not null))
        {
            var seen = use.UniqueHpAnnouncedTargets!;
            var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueHpTargetAnnouncedEvent>().Where(e => e.Target.CardUseFrameId == use.Id).ToArray();
            if (!HasUniqueHpPeerPrograms || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || seen.Distinct().Count() != seen.Count || seen.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || issued.Length != seen.Count || issued.Select(e => e.Target.TargetSeat).Distinct().Count() != issued.Length || !issued.Select(e => e.Target.TargetSeat).SequenceEqual(seen) || issued.Any(e => e.Target.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || e.Target.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || e.WindowFrameId <= use.Id || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(e.Target.EffectiveKind)))
                throw new InvalidOperationException("The once-announced target set lost its exact real Slash/window facts.");
        }
    }
}
