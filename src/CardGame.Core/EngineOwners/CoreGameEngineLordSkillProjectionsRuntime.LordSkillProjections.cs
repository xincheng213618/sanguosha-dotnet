#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLordSkillProjectionsRuntime
{
    internal bool HasLordProjectionCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesLordSkillProjection;

    internal IEnumerable<SkillGrant> EffectiveUnprojectedGrants(CharacterState owner)
    {
        var active = owner.SkillGrants.Grants.Where(g => g.LordProjection is null && (g.GeneralLibraryProjection is null ? _coreNationalSkillsOperations.IsSkillGrantStateEffective(owner, g) : _corePrivateGeneralLibrariesOperations.IsGeneralLibraryGrantQualified(owner, g)) && _coreNationalSkillsOperations.IsRawSkillSourceAdmitted(owner, g) && (g.SourceId is not (CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource) || !_contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Tags.HasFlag(SkillTag.Lord) || owner.Role == Role.Lord || _corePrintedLordSkillQualificationsOperations.IsPrintedLordGrantQualified(owner, g))).ToArray();
        return active;
    }

    internal bool IsProjectedGrantQualified(CharacterState owner, SkillGrant grant)
    {
        if (!_coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, grant))
            return false;
        if (grant.LordProjection is not { } source)
            return true;
        if (!owner.IsAlive || !_coreModeAllegianceOperations.ModeAllegiancePolicy.AllowsLordSkillProjection || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.LordSeat) || source.LordSeat == owner.Seat)
            return false;
        var lord = _coreMatchParticipantsPlayersAccess._players[source.LordSeat];
        return lord.IsAlive && lord.Role == Role.Lord && EffectiveUnprojectedGrants(owner).Any(g => g.GrantId == source.CapabilityGrantId && g.SkillInstanceId == source.CapabilitySkillInstanceId && _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Program?.LordSkillProjection == true) && EffectiveUnprojectedGrants(lord).Any(g => g.GrantId == source.LordGrantId && g.SkillInstanceId == source.LordSkillInstanceId && g.SkillId == grant.SkillId && _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Tags.HasFlag(SkillTag.Lord));
    }

    internal long CaptureLordProjectionDependencyStamp()
    {
        if (!HasLordProjectionCapability)
            return 0;
        var changed = _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencies.Length != _coreMatchParticipantsPlayersAccess._players.Count;
        if (changed)
            _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencies = new GameEngine.ProjectionDependency[_coreMatchParticipantsPlayersAccess._players.Count];
        for (var i = 0; i < _coreMatchParticipantsPlayersAccess._players.Count; i++)
        {
            var p = _coreMatchParticipantsPlayersAccess._players[i];
            var current = new GameEngine.ProjectionDependency(p.SkillGrants.Revision, p.IsAlive, p.Hp, p.Role, p.GeneralSelected, p.GeneralRevealed, p.SecondaryGeneralSelected, p.SecondaryGeneralRevealed);
            if (_coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencies[i] != current)
            {
                changed = true;
                _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencies[i] = current;
            }
        }

        if (changed)
            _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencyRevision++;
        return _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencyRevision;
    }

    internal bool HasSkillRoleQualification(CharacterState owner, string skillId, string? instance, Role role)
    {
        if (owner.Role == role)
            return true;
        if (role != Role.Lord || !_contentRegistryOperations._contentRegistry.GetSkill(skillId).Tags.HasFlag(SkillTag.Lord))
            return false;
        // The menu uses the same stable instance that execution resolves. Do not let another grant authorize it.
        instance ??= _corePrintedLordSkillQualificationsOperations.PreferredQualifiedPrintedLordInstance(owner, skillId) ?? _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.Where(g => g.SkillId == skillId).OrderBy(g => g.SkillInstanceId, StringComparer.Ordinal).Select(g => g.SkillInstanceId).FirstOrDefault();
        return owner.SkillGrants.Grants.Any(g => g.SkillId == skillId && g.SkillInstanceId == instance && g.IsEnabled && (_corePrintedLordSkillQualificationsOperations.IsPrintedLordGrantQualified(owner, g) || g.LordProjection is not null && IsProjectedGrantQualified(owner, g))) || _boundaryYuanShuOperations.Get().IsLordGrantQualified(owner.Seat, skillId, instance);
    }

    internal IReadOnlyList<string> AdvancedOwnedSkillIds(CharacterState owner) => owner.SkillGrants.Grants.Where(g => g.IsEnabled && !g.IsModeRule && (g.LordProjection is null || IsProjectedGrantQualified(owner, g)) && _corePrivateGeneralLibrariesOperations.IsGeneralLibraryGrantQualified(owner, g)).Select(g => g.SkillId).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 40)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 20)]
    internal void SynchronizeLordSkillProjections()
    {
        if (!HasLordProjectionCapability || _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._synchronizingLordProjections)
            return;
        _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._synchronizingLordProjections = true;
        try
        {
            CharacterState? lord = null;
            if (_coreModeAllegianceOperations.ModeAllegiancePolicy.AllowsLordSkillProjection)
                for (var i = 0; i < _coreMatchParticipantsPlayersAccess._players.Count; i++)
                {
                    var player = _coreMatchParticipantsPlayersAccess._players[i];
                    if (!player.IsAlive || player.Role != Role.Lord)
                        continue;
                    if (lord is not null)
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    lord = player;
                }

            foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
            {
                var grants = owner.SkillGrants.Grants;
                var needsProjectionSync = false;
                for (var i = 0; i < grants.Count; i++)
                    if (grants[i].LordProjection is not null || lord is not null && owner.IsAlive && owner.Seat != lord.Seat && _contentRegistryOperations._contentRegistry.GetSkill(grants[i].SkillId).Program?.LordSkillProjection == true)
                    {
                        needsProjectionSync = true;
                        break;
                    }

                // Disabled capabilities still retain their derived relations;
                // existing projections still need stale-source cleanup.
                if (!needsProjectionSync)
                    continue;
                SynchronizeOwnerLordSkillProjections(owner, lord);
            }
        }
        finally
        {
            _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._synchronizingLordProjections = false;
        }
    }

    internal void SynchronizeOwnerLordSkillProjections(CharacterState owner, CharacterState? lord)
    {
        var desired = new Dictionary<string, SkillGrant>(StringComparer.Ordinal);
        if (lord is not null && owner.IsAlive && owner.Seat != lord.Seat)
            foreach (var capability in owner.SkillGrants.Grants.Where(g => g.LordProjection is null && _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Program?.LordSkillProjection == true))
                foreach (var source in lord.SkillGrants.Grants.Where(g => g.LordProjection is null && _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Tags.HasFlag(SkillTag.Lord)))
                {
                    var relation = new LordSkillProjectionSource(lord.Seat, source.GrantId, source.SkillInstanceId, capability.GrantId, capability.SkillInstanceId);
                    var id = $"lord-projection:{(owner.Seat)}:{(capability.GrantId.Length)}:{(capability.GrantId)}:{(capability.SkillInstanceId.Length)}:{(capability.SkillInstanceId)}:{(lord.Seat)}:{(source.GrantId.Length)}:{(source.GrantId)}:{(source.SkillInstanceId.Length)}:{(source.SkillInstanceId)}:{(source.SkillId)}";
                    desired.Add(id, new SkillGrant(id, source.SkillId, id, $"lord-projection:{(capability.SourceId)}", LordProjection: relation));
                }

        foreach (var stale in owner.SkillGrants.Grants.Where(g => g.LordProjection is not null && !desired.ContainsKey(g.GrantId)))
        {
            owner.SkillGrants.RemoveGrant(stale.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LordSkillProjectionChangedEvent(owner.Seat, stale.GrantId, stale.SkillId, stale.LordProjection!, false));
        }

        foreach (var grant in desired.Values.Where(g => !owner.SkillGrants.Grants.Any(old => old.GrantId == g.GrantId)))
        {
            owner.SkillGrants.Grant(grant);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LordSkillProjectionChangedEvent(owner.Seat, grant.GrantId, grant.SkillId, grant.LordProjection!, true));
        }
    }
}
