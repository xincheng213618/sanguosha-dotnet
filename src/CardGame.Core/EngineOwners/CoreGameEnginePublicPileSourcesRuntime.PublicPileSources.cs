#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePublicPileSourcesRuntime
{
    internal bool SupportsMultiplePublicPiles => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveSourceDingxiUsedCard) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileYuJinJieyue) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiHuangZuStore) || _characterZhenJiHappyGodOperations.Value.HasHappyZhenJi || _characterCaoXianOlOperations.Value.HasCaoXian || _characterSunShangxiangMouMobileOperations.HasMouSunShangxiangMobile || _characterGongsunZanMouMobileOperations.HasMouGszMobile || _characterDengAiMouOlOperations.Value.HasMouDengAiOl || _characterGuoHuaiMouMobileOperations.Value.HasMouGuoHuaiMobile || _characterGanNingMouMobileOperations.Value.HasMouGanNingMobile || _characterXiahouDunMouMobileOperations.Value.HasMouXiahouDunCapability || _characterLiFengOlOperations.HasLiFengCapability || _characterGuanHaiOlOperations.HasGuanHaiCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesOwnPublicPileConversions || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReplaceJibingDrawWithTwoBing) || _coreRicePileOperations.RiceCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.PlaceBoundHandAsCategoryTalisman) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreNonBasicOwnedPublicPile) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreArbitraryOwnedPublicPile) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.CollectFinalTargetCardInPublicPile) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreBoundHandInPublicPile) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreBoundCardsInPublicPile) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.CashOutPublicPile) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeXingGuMountPile) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveXingwuDepositAndBurst) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveBaoXinMutao) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreOlChuyuanAfterDamage);

    internal static string PublicPileIdentity(string skill, string instance) => RuntimeSkills.RuntimeRules_Core_PublicPileSources.PublicPileIdentity(skill, instance);
    internal IEnumerable<PublicPersistentPileSource> PublicPileSources(int seat) => _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Values.Where(s => s.OwnerSeat == seat);
    internal PublicPersistentPileSource? SinglePublicPileSource(int seat)
    {
        if (_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Count == 0)
            return null;
        PublicPersistentPileSource? single = null;
        foreach (var source in _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Values)
        {
            if (source.OwnerSeat != seat)
                continue;
            if (single is not null)
                return null;
            single = source;
        }

        return single;
    }

    internal static CardLocation PublicPileLocation(int seat) => RuntimeSkills.RuntimeRules_Core_PublicPileSources.PublicPileLocation(seat);
    internal IReadOnlyList<Card> PublicPileCards(PublicPersistentPileSource source) => _cardZonesOperations._cardZones.CardsAt(source.Location);
    internal IReadOnlyList<Card> PublicPileCards(int seat) => SinglePublicPileSource(seat)is { } source ? PublicPileCards(source) : [];
    internal PublicPersistentPileSource EnsurePublicPileSource(ProgramSkillFrame frame, int capacity)
    {
        var key = (frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId);
        if (_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.TryGetValue(key, out var found))
        {
            if (found.Capacity != capacity)
                throw new InvalidOperationException("A public pile source cannot change capacity.");
            return found;
        }

        var existing = PublicPileSources(frame.OwnerSeat).ToArray();
        if (!SupportsMultiplePublicPiles && existing.Length > 0)
            throw new InvalidOperationException("A public pile cannot mix source skill instances or capacities.");
        var pileId = existing.Any(s => s.PublicPileId is null) ? PublicPileIdentity(frame.SkillId, frame.SkillInstanceId) : null;
        var source = new PublicPersistentPileSource(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, capacity, pileId);
        _cardZonesOperations._cardZones.EnsurePublicPersistentPile(source.Location);
        _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Add(key, source);
        return source;
    }

    internal IReadOnlyList<PublicPersistentPileSource> ReferencedPublicPileSources(int seat, string skill, string? consumerInstance = null)
    {
        var sources = PublicPileSources(seat).Where(s => s.SkillId == skill).ToArray();
        if (consumerInstance is not null)
        {
            var own = sources.Where(s => s.SkillInstanceId == consumerInstance).ToArray();
            if (own.Length > 0)
                return own;
            var consumer = _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.FirstOrDefault(g => g.SkillInstanceId == consumerInstance);
            if (consumer is not null)
            {
                var same = sources.Where(s => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.Any(g => g.SkillId == skill && g.SkillInstanceId == s.SkillInstanceId && g.SourceId == consumer.SourceId)).ToArray();
                if (same.Length > 0)
                    return same;
            }
        }

        return sources;
    }

    internal PublicPersistentPileSource? PublicPileDraftSource(ProgramPublicPileDraft draft) => draft.SourceSkillInstanceId is { } instance ? _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((draft.OwnerSeat, draft.SourceSkillId, instance))is { } exact && exact.Location == draft.SourceLocation ? exact : null : ReferencedPublicPileSources(draft.OwnerSeat, draft.SourceSkillId).SingleOrDefault();
    internal IReadOnlyList<Card> PublicPileDraftCards(ProgramPublicPileDraft draft) => PublicPileDraftSource(draft)is { } source ? PublicPileCards(source) : [];
    internal CardLocation PublicPileDraftLocation(ProgramPublicPileDraft draft) => PublicPileDraftSource(draft)?.Location ?? throw new InvalidOperationException("Public pile draft lost its exact source location.");
    internal IReadOnlyList<PublicPersistentPileSnapshot>? CreatePublicPersistentPileSnapshots(int seat)
    {
        if (_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Count == 0)
            return null;
        PublicPersistentPileSource? first = null;
        List<PublicPersistentPileSnapshot>? snapshots = null;
        foreach (var source in _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Values)
        {
            if (source.OwnerSeat != seat)
                continue;
            if (first is null)
            {
                first = source;
                continue;
            }

            if (snapshots is null)
            {
                snapshots = [];
                snapshots.Add(CreatePublicPersistentPileSnapshot(seat, first));
            }

            snapshots.Add(CreatePublicPersistentPileSnapshot(seat, source));
        }

        return snapshots is null ? null : Array.AsReadOnly(snapshots.ToArray());
    }

    internal PublicPersistentPileSnapshot CreatePublicPersistentPileSnapshot(int seat, PublicPersistentPileSource source)
    {
        var name = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).ProgramPresentation?.AuthorityName;
        var cards = PublicPileCards(source);
        return new(seat, source.SkillId, source.SkillInstanceId, name, Array.AsReadOnly(cards.Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray()), cards.Count, source.Location);
    }

    internal int PublicPileProgramCount(int seat, string skill, string instance)
    {
        var program = _contentRegistryOperations._contentRegistry.GetSkill(skill).Program!;
        // A non-program input must still follow the original failing path.
        if (program is not null)
        {
            if (_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Count == 0)
                return 0;
            var hasOwnedSource = false;
            foreach (var source in _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Values)
                if (source.OwnerSeat == seat)
                {
                    hasOwnedSource = true;
                    break;
                }

            if (!hasOwnedSource)
                return 0;
        }

        if (program!.Triggers.SelectMany(trigger => trigger.Effects).Any(effect => PublicPileCashOutContract.IsOperation(effect.Op)))
            return _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((seat, skill, instance))is { } exact ? PublicPileCards(exact).Count : 0;
        var sourceSkills = program.Triggers.SelectMany(t => t.Effects).Concat(program.Activations.SelectMany(a => a.Effects)).Where(e => e.Op is SkillProgramEffectOp.ExchangePublicPile or SkillProgramEffectOp.SelectPublicPile or SkillProgramEffectOp.RemovePublicPileAfterAttackDamage or SkillProgramEffectOp.ResolvePreparationPublicPile or SkillProgramEffectOp.ExchangePublicPileHand or SkillProgramEffectOp.ObtainPublicPileCard or SkillProgramEffectOp.PublicPileColorDamage or SkillProgramEffectOp.RewardDiscardedActionColor or SkillProgramEffectOp.ResolveFirstGameDomainCrossing or SkillProgramEffectOp.OfferRiceAfterDamagePoint or SkillProgramEffectOp.ReplaceJudgmentFromPublicRice or SkillProgramEffectOp.AwakenOlDengji or SkillProgramEffectOp.AwakenOlTianxing).SelectMany(e => e.SkillIds).Distinct(StringComparer.Ordinal).ToArray();
        if (sourceSkills.Length == 0)
            sourceSkills = [skill];
        return sourceSkills.SelectMany(id => ReferencedPublicPileSources(seat, id, instance)).Select(s => PublicPileCards(s).Count).DefaultIfEmpty().Max();
    }

    internal IReadOnlyDictionary<string, int>? CapturePublicPileProgramCounts(CharacterState owner)
    {
        if (!SupportsMultiplePublicPiles)
            return null;
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances)
            counts[PublicPileIdentity(instance.SkillId, instance.SkillInstanceId)] = PublicPileProgramCount(owner.Seat, instance.SkillId, instance.SkillInstanceId);
        foreach (var source in PublicPileSources(owner.Seat))
            counts[PublicPileIdentity(source.SkillId, source.SkillInstanceId)] = PublicPileCards(source).Count;
        return new System.Collections.ObjectModel.ReadOnlyDictionary<string, int>(counts);
    }
}
