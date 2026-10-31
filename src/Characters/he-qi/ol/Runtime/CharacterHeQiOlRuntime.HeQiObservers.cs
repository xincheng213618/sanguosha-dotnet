#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeQiOlRuntime
{
    internal IReadOnlySet<string> EquipmentSuitCapabilitySkillIds => _contentRegistryOperations._contentRegistry.ProgramDependencies.EquipmentSuitCapabilitySkillIds;

    internal ProgramSkillFrame CaptureEquipmentSuitProgramSource(ProgramSkillFrame frame)
    {
        var grant = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).ActiveGrants.FirstOrDefault(grant => grant.SkillId == frame.SkillId && grant.SkillInstanceId == frame.SkillInstanceId && grant.EquipmentSuitSource is not null);
        return grant is null ? frame : frame with
        {
            EquipmentSuitSourceReceipt = new(frame.Id, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, grant.GrantId, grant.EquipmentSuitSource!)
        };
    }

    internal bool ValidateEquipmentSuitProgramSource(ProgramSkillFrame frame) => frame.EquipmentSuitSourceReceipt is { } receipt && receipt.FrameId == frame.Id && receipt.OwnerSeat == frame.OwnerSeat && receipt.SkillId == frame.SkillId && receipt.SkillInstanceId == frame.SkillInstanceId && receipt.GameplayHash == frame.GameplayHash && receipt.GrantId == EquipmentSuitGrantId(frame.OwnerSeat, receipt.Source) && _contentRegistryOperations._contentRegistry.GetSkill(receipt.Source.CapabilitySkillId).Program?.EquipmentSuitSkillGrants.Any(rule => rule.Id == receipt.Source.RuleId && rule.SkillId == frame.SkillId) == true;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 510)]
    internal bool CanContinueEquipmentSuitProgram(ProgramSkillFrame frame) => ValidateEquipmentSuitProgramSource(frame) && frame.FrozenHandRoster is { Stage: "movement" } or { Stage: "damage-issued", DamageIssued: true } && _coreFrozenHandRosterOperations.ValidFrozenHandRoster(frame);
    internal static string EquipmentSuitGrantId(int ownerSeat, EquipmentSuitSkillGrantSource source) => $"equipment-suit:{(ownerSeat)}:{(source.CapabilityGrantId.Length)}:{(source.CapabilityGrantId)}:{(source.CapabilitySkillInstanceId.Length)}:{(source.CapabilitySkillInstanceId)}:{(source.RuleId)}";
    internal bool IsEquipmentSuitGrantQualified(CharacterState owner, SkillGrant grant)
    {
        if (grant.EquipmentSuitSource is not { } relation)
            return true;
        var source = owner.SkillGrants.Grants.FirstOrDefault(candidate => candidate.GrantId == relation.CapabilityGrantId && candidate.SkillId == relation.CapabilitySkillId && candidate.SkillInstanceId == relation.CapabilitySkillInstanceId && candidate.EquipmentSuitSource is null);
        if (source is null || !EquipmentSuitSourceQualified(owner, source))
            return false;
        return _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.EquipmentSuitSkillGrants.Any(rule => rule.Id == relation.RuleId && rule.SkillId == grant.SkillId && EquipmentSuitCount(owner) >= rule.MinimumDistinctSuits) == true;
    }

    internal int EquipmentSuitCount(CharacterState owner)
    {
        if (owner.IdentityConcealed)
            return 0;
        // Build only source-independent rewrite bindings. Calling the final binding
        // shard here would recurse through this very qualification predicate.
        var policies = owner.SkillGrants.Grants.Where(g => g.EquipmentSuitSource is null && _coreHpConditionalSkillGrantsOperations.HpConditionalSourceQualified(owner, g) && _coreHpConditionalSkillGrantsOperations.IsHpConditionalGrantQualified(owner, g)).GroupBy(g => (g.SkillId, g.SkillInstanceId)).OrderBy(g => g.Key.SkillId, StringComparer.Ordinal).ThenBy(g => g.Key.SkillInstanceId, StringComparer.Ordinal).SelectMany(g => _contentRegistryOperations._contentRegistry.GetSkill(g.Key.SkillId).Program?.CardPolicies.Where(p => p.Kind == SkillProgramCardPolicyKind.RewriteSuit) ?? []).ToArray();
        foreach (var policy in policies)
            if (policy.Condition.Kind != SkillProgramConditionKind.Always || policy.OwnerRole is not null)
                throw new InvalidOperationException("Equipment suit qualification does not support context-dependent suit rewrites.");
        Suit SuitOf(Card card)
        {
            var suit = card.Suit;
            foreach (var policy in policies)
                if (policy.CardKinds.Count == 0 && policy.RequiredCardKinds.Count == 0 && policy.InputSuit == suit)
                    suit = policy.OutputSuit!.Value;
            return suit;
        }

        return _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Select(SuitOf).Where(s => s != Suit.None).Distinct().Count();
    }

    internal bool EquipmentSuitSourceQualified(CharacterState owner, SkillGrant source) => source.EquipmentSuitSource is null && _coreHpConditionalSkillGrantsOperations.HpConditionalSourceQualified(owner, source) && _coreHpConditionalSkillGrantsOperations.IsHpConditionalGrantQualified(owner, source);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 60)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 40)]
    internal void SynchronizeEquipmentSuitSkillGrants()
    {
        if (EquipmentSuitCapabilitySkillIds.Count == 0 || _characterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._synchronizingEquipmentSuitSkillGrants)
            return;
        // The catalogue may contain Qizhou while this match never owns it.
        // Use raw grant membership (including later grants), not a match-start cache.
        if (_characterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._equipmentSuitSkillInstances.Count == 0 && !_coreMatchParticipantsPlayersAccess._players.Any(owner => owner.SkillGrants.HasEnabledSkill(EquipmentSuitCapabilitySkillIds)))
            return;
        _characterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._synchronizingEquipmentSuitSkillGrants = true;
        try
        {
            foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
            {
                foreach (var key in _characterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._equipmentSuitSkillInstances.Keys.Where(key => key.OwnerSeat == owner.Seat && !owner.SkillGrants.Grants.Any(grant => grant.GrantId == key.Source.CapabilityGrantId && grant.SkillId == key.Source.CapabilitySkillId && grant.SkillInstanceId == key.Source.CapabilitySkillInstanceId)).ToArray())
                    _characterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._equipmentSuitSkillInstances.Remove(key);
                var desired = new Dictionary<string, SkillGrant>(StringComparer.Ordinal);
                foreach (var source in owner.SkillGrants.Grants.Where(grant => grant.EquipmentSuitSource is null && _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Program?.EquipmentSuitSkillGrants.Count > 0 && EquipmentSuitSourceQualified(owner, grant)))
                    foreach (var rule in _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.EquipmentSuitSkillGrants.Where(rule => EquipmentSuitCount(owner) >= rule.MinimumDistinctSuits))
                    {
                        var relation = new EquipmentSuitSkillGrantSource(source.GrantId, source.SkillId, source.SkillInstanceId, rule.Id);
                        var id = EquipmentSuitGrantId(owner.Seat, relation);
                        // Separate grants preserve independent ownership; an already owned
                        // effective skill shares its instance, just like ordinary grants.
                        if (!_characterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._equipmentSuitSkillInstances.TryGetValue((owner.Seat, relation), out var instance))
                        {
                            var existing = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.Where(grant => grant.SkillId == rule.SkillId).OrderBy(grant => grant.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault() ?? desired.Values.FirstOrDefault(grant => grant.SkillId == rule.SkillId);
                            instance = existing?.SkillInstanceId ?? id;
                            _characterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._equipmentSuitSkillInstances.Add((owner.Seat, relation), instance);
                        }

                        desired.Add(id, new SkillGrant(id, rule.SkillId, instance, "equipment-suit:" + source.SourceId, EquipmentSuitSource: relation));
                    }

                foreach (var stale in owner.SkillGrants.Grants.Where(grant => grant.EquipmentSuitSource is not null && !desired.ContainsKey(grant.GrantId)))
                {
                    owner.SkillGrants.RemoveGrant(stale.GrantId);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSuitSkillGrantChangedEvent(owner.Seat, stale.GrantId, stale.SkillId, stale.SkillInstanceId, stale.EquipmentSuitSource!, false, EquipmentSuitCount(owner)));
                }

                foreach (var grant in desired.Values.Where(grant => !owner.SkillGrants.Grants.Any(old => old.GrantId == grant.GrantId)))
                {
                    owner.SkillGrants.Grant(grant);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSuitSkillGrantChangedEvent(owner.Seat, grant.GrantId, grant.SkillId, grant.SkillInstanceId, grant.EquipmentSuitSource!, true, EquipmentSuitCount(owner)));
                }
            }
        }
        finally
        {
            _characterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._synchronizingEquipmentSuitSkillGrants = false;
        }
    }
}
