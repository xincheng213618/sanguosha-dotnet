#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoXiaoQiaoOlRuntime
{
    internal bool HasDaQiaoXiaoQiaoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveXingwuDepositAndBurst) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.MaintainLuoyanPileSkills);

    internal bool DaQiaoXiaoQiaoHasPile(int seat) => _corePublicPileSourcesOperations.PublicPileSources(seat).Any(p => p.SkillId == "ol:xingwu" && _corePublicPileSourcesOperations.PublicPileCards(p).Count > 0);
    internal bool DaQiaoXiaoQiaoLuoyanSourceLive(CharacterState owner, SkillGrant source) => owner.IsAlive && source.IsEnabled && source.SkillId == "ol:luoyan" && source.DaQiaoXiaoQiaoLuoyanSource is null && _coreHpConditionalSkillGrantsOperations.Value.HpConditionalSourceQualified(owner, source) && _coreHpConditionalSkillGrantsOperations.Value.IsHpConditionalGrantQualified(owner, source) && _characterHeQiOlOperations.IsEquipmentSuitGrantQualified(owner, source);
    internal bool IsDaQiaoXiaoQiaoLuoyanGrantQualified(CharacterState owner, SkillGrant grant)
    {
        if (grant.DaQiaoXiaoQiaoLuoyanSource is not { } relation)
            return true;
        return HasDaQiaoXiaoQiaoCapability && relation.LeafSkillId == grant.SkillId && grant.SkillId is ("boundary:tianxiang" or "classic:liuli") && DaQiaoXiaoQiaoHasPile(owner.Seat) && owner.SkillGrants.Grants.Any(s => s.GrantId == relation.CapabilityGrantId && s.SkillInstanceId == relation.CapabilitySkillInstanceId && DaQiaoXiaoQiaoLuoyanSourceLive(owner, s));
    }

    internal long CaptureDaQiaoXiaoQiaoLuoyanQualificationStamp(CharacterState owner, long existingStamp)
    {
        if (!HasDaQiaoXiaoQiaoCapability || !owner.SkillGrants.Grants.Any(g => g.SkillId == "ol:luoyan" || g.DaQiaoXiaoQiaoLuoyanSource is not null))
            return existingStamp;
        var key = (existingStamp, owner.SkillGrants.Revision, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, owner.Hp, owner.IsAlive, owner.IdentityConcealed);
        if (!_characterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._daQiaoXiaoQiaoQualificationCache.TryGetValue(owner.Seat, out var cached) || cached.Key != key)
        {
            var stamp = long.MinValue / 4 + ++_characterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._daQiaoXiaoQiaoQualificationRevision;
            if (stamp == existingStamp)
                stamp = long.MinValue / 4 + ++_characterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._daQiaoXiaoQiaoQualificationRevision;
            cached = (key, stamp);
            _characterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._daQiaoXiaoQiaoQualificationCache[owner.Seat] = cached;
        }

        return cached.Stamp;
    }

    internal static string DaQiaoXiaoQiaoDerivedId(int seat, DaQiaoXiaoQiaoLuoyanSource r) => RuntimeSkills.RuntimeRules_da_qiao_xiao_qiao_ol_DaQiaoXiaoQiaoLuoyan.DaQiaoXiaoQiaoDerivedId(seat, r);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27340)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 27340)]
    internal void SynchronizeDaQiaoXiaoQiaoLuoyan()
    {
        if (!HasDaQiaoXiaoQiaoCapability || _characterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._synchronizingDaQiaoXiaoQiaoLuoyan)
            return;
        _characterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._synchronizingDaQiaoXiaoQiaoLuoyan = true;
        try
        {
            foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
            {
                var desired = new Dictionary<string, SkillGrant>(StringComparer.Ordinal);
                if (DaQiaoXiaoQiaoHasPile(owner.Seat))
                    foreach (var source in owner.SkillGrants.Grants.Where(s => DaQiaoXiaoQiaoLuoyanSourceLive(owner, s)).ToArray())
                        foreach (var skill in new[]
                        {
                            "boundary:tianxiang",
                            "classic:liuli"
                        }

                        )
                        {
                            _ = _contentRegistryOperations._contentRegistry.GetSkill(skill);
                            var relation = new DaQiaoXiaoQiaoLuoyanSource(source.GrantId, source.SkillInstanceId, skill);
                            var id = DaQiaoXiaoQiaoDerivedId(owner.Seat, relation);
                            var previous = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DaQiaoXiaoQiaoLuoyanChangedEvent>().LastOrDefault(e => e.OwnerSeat == owner.Seat && e.GrantId == id && e.Added);
                            var independent = owner.SkillGrants.Grants.FirstOrDefault(g => g.SkillId == skill && g.DaQiaoXiaoQiaoLuoyanSource is null && g.IsEnabled && _coreHpConditionalSkillGrantsOperations.Value.HpConditionalSourceQualified(owner, g) && _coreHpConditionalSkillGrantsOperations.Value.IsHpConditionalGrantQualified(owner, g));
                            desired[id] = new(id, skill, previous?.InstanceId ?? independent?.SkillInstanceId ?? id, "luoyan:" + source.SourceId)
                            {
                                DaQiaoXiaoQiaoLuoyanSource = relation
                            };
                        }

                foreach (var stale in owner.SkillGrants.Grants.Where(g => g.DaQiaoXiaoQiaoLuoyanSource is not null && !desired.ContainsKey(g.GrantId)).ToArray())
                {
                    foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                        _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.GrantRevision);
                    if (owner.SkillGrants.RemoveGrant(stale.GrantId))
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DaQiaoXiaoQiaoLuoyanChangedEvent(owner.Seat, stale.GrantId, stale.SkillId, stale.SkillInstanceId, stale.DaQiaoXiaoQiaoLuoyanSource!, false));
                }

                foreach (var grant in desired.Values)
                {
                    foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                        _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.GrantRevision);
                    if (owner.SkillGrants.Grant(grant))
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DaQiaoXiaoQiaoLuoyanChangedEvent(owner.Seat, grant.GrantId, grant.SkillId, grant.SkillInstanceId, grant.DaQiaoXiaoQiaoLuoyanSource!, true));
                }
            }
        }
        finally
        {
            _characterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._synchronizingDaQiaoXiaoQiaoLuoyan = false;
        }
    }
}
