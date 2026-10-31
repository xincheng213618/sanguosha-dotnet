#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHpConditionalSkillGrantsRuntime
{
    internal IReadOnlySet<string> HpConditionalCapabilitySkillIds => _contentRegistryOperations._contentRegistry.ProgramDependencies.HpConditionalCapabilitySkillIds;

    internal ProgramSkillFrame CaptureHpConditionalProgramSource(ProgramSkillFrame frame)
    {
        var grant = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).ActiveGrants.FirstOrDefault(grant => grant.SkillId == frame.SkillId && grant.SkillInstanceId == frame.SkillInstanceId && grant.HpConditionalSource is not null);
        return grant is null ? frame : frame with
        {
            HpConditionalSourceReceipt = new(frame.Id, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, grant.GrantId, grant.HpConditionalSource!)
        };
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 500)]
    internal bool CanContinueHpConditionalProgram(ProgramSkillFrame frame) => frame.HpConditionalSourceReceipt is { } receipt && receipt.FrameId == frame.Id && receipt.OwnerSeat == frame.OwnerSeat && receipt.SkillId == frame.SkillId && receipt.SkillInstanceId == frame.SkillInstanceId && receipt.GameplayHash == frame.GameplayHash && receipt.GrantId == HpConditionalGrantId(frame.OwnerSeat, receipt.Source) && _contentRegistryOperations._contentRegistry.GetSkill(receipt.Source.CapabilitySkillId).Program?.HpSkillGrants.Any(rule => rule.Id == receipt.Source.RuleId && rule.SkillId == frame.SkillId) == true;
    internal static string HpConditionalGrantId(int ownerSeat, HpConditionalSkillGrantSource source) => RuntimeSkills.RuntimeRules_Core_HpConditionalSkillGrants.HpConditionalGrantId(ownerSeat, source);
    internal bool IsHpConditionalGrantQualified(CharacterState owner, SkillGrant grant)
    {
        if (grant.HpConditionalSource is not { } relation)
            return true;
        var source = owner.SkillGrants.Grants.FirstOrDefault(candidate => candidate.GrantId == relation.CapabilityGrantId && candidate.SkillId == relation.CapabilitySkillId && candidate.SkillInstanceId == relation.CapabilitySkillInstanceId && candidate.HpConditionalSource is null);
        if (source is null || !HpConditionalSourceQualified(owner, source))
            return false;
        return _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.HpSkillGrants.Any(rule => rule.Id == relation.RuleId && rule.SkillId == grant.SkillId && rule.Matches(owner.Hp) && _characterTangZiOlOperations.MatchesTangZiWoundedGrant(owner, rule)) == true;
    }

    internal bool HpConditionalSourceQualified(CharacterState owner, SkillGrant source)
    {
        if (!_corePrivateGeneralLibrariesOperations.LibraryBaseGrantLive(owner, source) || !_coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, source) || !_coreLordSkillProjectionsOperations.IsProjectedGrantQualified(owner, source) || !_corePrivateGeneralLibrariesOperations.IsGeneralLibraryGrantQualified(owner, source))
            return false;
        var suppressors = _corePrivateGeneralLibrariesOperations.HasPrivateGeneralLibraryCapability ? _corePrivateGeneralLibrariesOperations.PrivateGeneralLibrarySuppressionInputs(owner) : owner.SkillGrants.Grants.Where(grant => grant.HpConditionalSource is null && _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).SuppressionRule is { } rule && owner.Hp == rule.OwnerHpEquals && _corePrivateGeneralLibrariesOperations.LibraryBaseGrantLive(owner, grant) && _coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, grant) && _coreLordSkillProjectionsOperations.IsProjectedGrantQualified(owner, grant)).Select(grant => grant.SkillId).ToHashSet(StringComparer.Ordinal);
        return suppressors.Count == 0 || suppressors.Contains(source.SkillId) || source.SourceId.StartsWith("equipment:", StringComparison.Ordinal);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 50)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 30)]
    internal void SynchronizeHpConditionalSkillGrants()
    {
        if (HpConditionalCapabilitySkillIds.Count == 0 || _coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._synchronizingHpConditionalSkillGrants)
            return;
        // The catalogue may contain Baobian while this match never owns it.
        // Use raw grant membership (including later grants), not a match-start cache.
        if (_coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._hpConditionalSkillInstances.Count == 0 && !_coreMatchParticipantsPlayersAccess._players.Any(owner => owner.SkillGrants.HasEnabledSkill(HpConditionalCapabilitySkillIds)))
            return;
        _coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._synchronizingHpConditionalSkillGrants = true;
        try
        {
            foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
            {
                foreach (var key in _coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._hpConditionalSkillInstances.Keys.Where(key => key.OwnerSeat == owner.Seat && !owner.SkillGrants.Grants.Any(grant => grant.GrantId == key.Source.CapabilityGrantId && grant.SkillId == key.Source.CapabilitySkillId && grant.SkillInstanceId == key.Source.CapabilitySkillInstanceId)).ToArray())
                    _coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._hpConditionalSkillInstances.Remove(key);
                var desired = new Dictionary<string, SkillGrant>(StringComparer.Ordinal);
                foreach (var source in owner.SkillGrants.Grants.Where(grant => grant.HpConditionalSource is null && _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Program?.HpSkillGrants.Count > 0 && HpConditionalSourceQualified(owner, grant)))
                    foreach (var rule in _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.HpSkillGrants.Where(rule => rule.Matches(owner.Hp) && _characterTangZiOlOperations.MatchesTangZiWoundedGrant(owner, rule)))
                    {
                        var relation = new HpConditionalSkillGrantSource(source.GrantId, source.SkillId, source.SkillInstanceId, rule.Id);
                        var id = HpConditionalGrantId(owner.Seat, relation);
                        // Separate grants preserve independent ownership; an already owned
                        // effective skill shares its instance, just like ordinary grants.
                        if (!_coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._hpConditionalSkillInstances.TryGetValue((owner.Seat, relation), out var instance))
                        {
                            var existing = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.Where(grant => grant.SkillId == rule.SkillId).OrderBy(grant => grant.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault() ?? desired.Values.FirstOrDefault(grant => grant.SkillId == rule.SkillId);
                            instance = existing?.SkillInstanceId ?? id;
                            _coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._hpConditionalSkillInstances.Add((owner.Seat, relation), instance);
                        }

                        desired.Add(id, new SkillGrant(id, rule.SkillId, instance, "hp-conditional:" + source.SourceId, HpConditionalSource: relation));
                    }

                foreach (var stale in owner.SkillGrants.Grants.Where(grant => grant.HpConditionalSource is not null && !desired.ContainsKey(grant.GrantId)))
                {
                    owner.SkillGrants.RemoveGrant(stale.GrantId);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HpConditionalSkillGrantChangedEvent(owner.Seat, stale.GrantId, stale.SkillId, stale.SkillInstanceId, stale.HpConditionalSource!, false, owner.Hp));
                }

                foreach (var grant in desired.Values.Where(grant => !owner.SkillGrants.Grants.Any(old => old.GrantId == grant.GrantId)))
                {
                    owner.SkillGrants.Grant(grant);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HpConditionalSkillGrantChangedEvent(owner.Seat, grant.GrantId, grant.SkillId, grant.SkillInstanceId, grant.HpConditionalSource!, true, owner.Hp));
                }
            }
        }
        finally
        {
            _coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._synchronizingHpConditionalSkillGrants = false;
        }
    }
}
