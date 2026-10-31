#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMiHengLeRuntime
{
    internal bool HasSiruiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesEqualNameLengthUse;

    internal static int SiruiNameLength(CardKind kind) => RuntimeSkills.RuntimeRules_mi_heng_le_SiruiQueries.SiruiNameLength(kind);
    internal bool IsSiruiSource(CardConversionSource? source) => source is { SkillId: "le:sirui" } && _corePlayCardHistoryOperations.ViewAsRule(source)is { EqualNameLengthUse: true };
    internal ActualDiscardRecoveryPhaseKey? SiruiCurrentPhase(int actor)
    {
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        return _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } && phase.ActorSeat == actor && phase.ActualTurnOwnerSeat == actor && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == actor && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == actor && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && (_coreProgramLifecycleOperations.IsEndOwnedExtraPlayPhase() || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == phase.TurnNumber && e.ActorSeat == actor)) ? phase : null;
    }

    internal bool CanUseSiruiPhase(CharacterState owner, CardConversionSource source) => owner.IsAlive && source.OwnerSeat == owner.Seat && IsSiruiSource(source) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, source.SkillId, source.SkillInstanceId) && SiruiCurrentPhase(owner.Seat)is { } phase && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<SiruiPhaseUseIssuedEvent>().Any(e => e.Source.OwnerSeat == owner.Seat && e.Phase.Token == phase.Token);
    internal bool CanUseSiruiConversion(CharacterState owner, Card card, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool forResponse, bool dyingUse = false, int? requestedUseTarget = null)
    {
        if (!rule.EqualNameLengthUse)
            return true;
        var source = new CardConversionSource(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId);
        var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
        if (!CanUseSiruiPhase(owner, source) || SiruiPureProviderFrontier(owner.Seat) || location.OwnerSeat != owner.Seat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || SiruiNameLength(card.Kind) != SiruiNameLength(rule.OutputKind) || _coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card) || _coreCardConversionsOperations.HasProgramCardIdentity(owner, card) || !_coreActivityOrdinaryTricksOperations.IsActivityOrdinaryTrickNameAvailable(rule.OutputKind))
            return false;
        return !forResponse && !dyingUse && requestedUseTarget is null || IsSiruiTrueResponseUse(owner, rule.OutputKind, dyingUse, requestedUseTarget);
    }

    internal bool SiruiPureProviderFrontier(int actor)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        bool At(long owner) => top?.Id == owner || top is ResponseWindowFrame response && response.ParentFrameId == owner;
        return _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } f && f.CurrentCandidateSeat == actor && At(f.OwnerFrameId) || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } d && d.CurrentCandidateSeat == actor && At(d.Attack.ResolutionId);
    }

    internal bool IsSiruiTrueResponseUse(CharacterState owner, CardKind kind, bool dyingUse = false, int? requestedUseTarget = null)
    {
        if (SiruiCurrentPhase(owner.Seat)is null)
            return false;
        if (requestedUseTarget is { } target)
            return kind == CardKind.Peach && _coreWenyiPeachSuppliesOperations.IsWenyiPeachSupplyQuery(owner, target);
        if (dyingUse || kind is CardKind.Peach or CardKind.Alcohol)
            return _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && dying.ResponderSeat == owner.Seat && (kind == CardKind.Peach || kind == CardKind.Alcohol && dying.VictimSeat == owner.Seat);
        if (kind == CardKind.Nullification)
            return _resolutionStackOperations._resolutionStack.LastOrDefault()is NullificationWindowFrame n && n.CandidateIndex < n.CandidateSeats.Count && n.CandidateSeats[n.CandidateIndex] == owner.Seat;
        if (kind == CardKind.Dodge)
            return _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(owner, kind);
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true })
            return false;
        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: null } borrowed && borrowed.WeaponOwnerSeat == owner.Seat)
            return true;
        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } qinglong && qinglong.Attack.SourceSeat == owner.Seat && CoreNativeDamageHandlesRuntime.SameAttackOwner(qinglong.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack))
            return true;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { } effect)
            return false;
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(f))
            return f.LvKuangLvXiang!.Actor == owner.Seat;
        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(f) || _characterWuJingOlOperations.IsWuJingHejiRequest(f))
            return f.OwnerSeat == owner.Seat;
        return _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(f, effect.Op, owner.Seat);
    }

    internal bool SiruiIgnoresSlashDistance(CharacterState actor, CardConversionSource? source) => source is not null && IsSiruiSource(source) && CoreNativeCardClassificationsRuntime.IsSlashCard(_corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind) && CanUseSiruiPhase(actor, source);
    internal bool SiruiIgnoresSlashLimit(CharacterState actor, CardConversionSource? source) => SiruiIgnoresSlashDistance(actor, source);
    internal IReadOnlyList<SlashUseVariant> SiruiPhysicalSlashVariants(CharacterState actor, Card card) => CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Where(kind => _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, forResponse: false).Any(source => IsSiruiSource(source) && _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == kind)).Select(kind => new SlashUseVariant(kind, AdditionalConversionSource: null, UsesZhuqueFan: false)).ToArray();
    internal bool SiruiExplicitForcedSlashFrontier(CharacterState actor)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        bool At(long parent) => top?.Id == parent || top is ResponseWindowFrame response && response.ParentFrameId == parent;
        return _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } borrowed && borrowed.WeaponOwnerSeat == actor.Seat && At(borrowed.ResolutionId) || _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } qinglong && qinglong.Attack.SourceSeat == actor.Seat && CoreNativeDamageHandlesRuntime.SameAttackOwner(qinglong.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && At(qinglong.Attack.ResolutionId);
    }

    internal bool SiruiNativeConversionMatchesOutput(CharacterState actor, CardConversionSource source, CardKind kind) => !IsSiruiSource(source) || !SiruiExplicitForcedSlashFrontier(actor) || _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == kind;
    internal bool SiruiAllowsUnspecifiedForcedSlash(CharacterState actor, Card card, CardKind kind) => !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || !SiruiExplicitForcedSlashFrontier(actor) || CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, kind) || SiruiPhysicalSlashVariants(actor, card).Count == 0;
    internal bool SiruiSlashAliasAllowed(CharacterState actor, bool forResponse) => IsSiruiTrueResponseUse(actor, CardKind.Slash) && (!forResponse || _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not null || _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not null || _resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramSkillFrame);
    internal bool HasSiruiForcedSlash(CharacterState actor, CharacterState target, NativeOwners.QinglongCrescentBladeHandle pending) => _coreNativeDamageHandlesOperations.Value.UsesFormalQinglongCrescentBlade && pending.Attack.SourceSeat == actor.Seat && pending.Attack.TargetSeat == target.Seat && CoreNativeDamageHandlesRuntime.SameAttackOwner(pending.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target) && CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => HasSiruiSlashDistance(actor, kind));
    internal bool SiruiBorrowedChoiceAllowed(CharacterState actor, CharacterState target, Card card, PromptChoice choice)
    {
        if (SiruiPhysicalSlashVariants(actor, card).Count == 0)
            return true;
        if (!Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("response-card-kind"), out var kind) || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return false;
        _ = CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var source);
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var excluded = from == CardLocation.Equipment(actor.Seat) && _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot == EquipmentSlot.Weapon ? card.Id : (int? )null;
        // The coarse native request may exist because Sirui can reach this
        // target. Its unconverted sibling choice must still prove its own range.
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, card, source, kind, physicalCardIds: [card.Id], excludedEquipmentId: excluded);
    }

    internal bool IsSiruiPhysicalSlashKind(CharacterState actor, Card card, CardKind kind) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, forResponse: false).Any(source => IsSiruiSource(source) && _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == kind);
    internal CardKind SiruiSelectedSlashKind(CharacterState actor, Card card, CardKind fallback, CardConversionSource? explicitSource = null)
    {
        var selected = explicitSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        return selected is not null && IsSiruiSource(selected) && _corePlayCardHistoryOperations.ViewAsRule(selected)is { } rule && CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind) && _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, rule.OutputKind, false).Contains(selected) ? rule.OutputKind : fallback;
    }

    internal bool HasSiruiSlashDistance(CharacterState actor, CardKind kind, IReadOnlyList<int>? materialIds = null, CardConversionSource? conversionSource = null, long? existingUseFrameId = null)
    {
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return false;
        if (existingUseFrameId is { } existing && materialIds is [var issuedId] && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(use => use.Id == existing && use.Sirui is { NativeKind: SiruiNativeUseKind.CardUse } r && r.CardId == issuedId && r.Source.OwnerSeat == actor.Seat && CoreNativeCardClassificationsRuntime.IsSlashCard(r.OutputKind) && (use.CardKind == kind || _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use) && kind == CardKind.FireSlash) && ValidSiruiInvoice(use, r, requirePayment: false)))
            return true;
        if (existingUseFrameId is not null || materialIds is { Count: not 1 })
            return false;
        if (materialIds is not null && conversionSource is null)
            return false;
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor));
        if (materialIds is [var id])
            cards = cards.Where(c => c.Id == id);
        return cards.Any(card => _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.Any(instance => instance.Program.ViewAs.Any(rule => rule.EqualNameLengthUse && rule.OutputKind == kind && (conversionSource is null || conversionSource == new CardConversionSource(instance.SkillId, rule.Id, actor.Seat, instance.SkillInstanceId)) && CanUseSiruiConversion(actor, card, instance, rule, forResponse: false))));
    }

    internal bool SiruiIgnoresAlcoholLimit(CharacterState actor, CardConversionSource? source) => source is not null && IsSiruiSource(source) && _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind is CardKind.Alcohol or CardKind.RealgarWine && CanUseSiruiPhase(actor, source);
    internal bool SiruiIgnoresTrickDistance(CharacterState actor, CardConversionSource? source, IReadOnlyList<int>? materialIds = null) => source is not null && IsSiruiSource(source) && _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == CardKind.Snatch && CanUseSiruiPhase(actor, source) && materialIds is [var id] && _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat == actor.Seat && _cardZonesOperations._cardZones.GetLocation(id).Zone is CardZoneKind.Hand or CardZoneKind.Equipment && SiruiNameLength(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id).Kind) == SiruiNameLength(CardKind.Snatch);
    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 20)]
    internal bool SiruiConversionUnavailable(ProgramConfiguredViewAsQuery query) => !CanUseSiruiConversion(query.Owner, query.Card, query.Instance, query.Rule, query.ForResponse, query.DyingUse, query.RequestedUseTarget);
    [ProgramDistanceRule(ProgramDistanceStage.ConvertedSlashExemption, 10)]
    internal bool SiruiIgnoresSlashDistanceQuery(ProgramSlashConversionDistanceQuery q) => SiruiIgnoresSlashDistance(q.Owner, q.Source);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 40)]
    internal bool DistanceBorrowedSlash40HasSiruiSlashDistance(ProgramSlashDistanceQuery q) => HasSiruiSlashDistance(q.Actor, q.Kind, q.PhysicalCardIds, q.Conversion);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 30)]
    internal bool DistanceBorrowedPhysicalSlash30IsSiruiPhysicalSlashKind(ProgramSlashDistanceQuery q) => IsSiruiPhysicalSlashKind(q.Actor, q.PhysicalCard!, q.Kind);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 20)]
    internal bool DistanceBorrowedPaidWeaponSlash20SiruiPhysicalSlashVariants(ProgramSlashDistanceQuery q) => SiruiPhysicalSlashVariants(q.Actor, q.PhysicalCard!).Concat(_characterGuanYuWumiaoOperations.JuewuPhysicalSlashVariants(q.Actor, q.PhysicalCard!)).Concat(_characterCaoZhiLeOperations.FulePhysicalSlashVariants(q.Actor, q.PhysicalCard!)).Any();
}
