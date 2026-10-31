#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuWumiaoRuntime
{
    internal WuyouHandLease? ActiveWuyouLease(int holder, int cardId)
    {
        if (!HasWuyouCapability || _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(holder))
            return null;
        var lease = _coreProgramEventHistoryOperations.ProgramEventHistory<WuyouLeaseIssuedEvent>().Where(e => e.Lease.HolderSeat == holder && e.Lease.CardId == cardId).Select(e => e.Lease).OrderByDescending(e => e.MovementCutoff).ThenByDescending(e => e.FrameId).FirstOrDefault();
        return lease is not null && ValidWuyouLease(lease) && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cardId && m.Sequence > lease.MovementCutoff && m.From == CardLocation.Hand(holder) && m.To != m.From) ? lease : null;
    }

    internal GameEngine.ProgramCardIdentityMatch? WuyouIdentity(CharacterState holder, Card card) => ActiveWuyouLease(holder.Seat, card.Id)is { } lease ? new(_contentRegistryOperations._contentRegistry.GetSkill(lease.Source.SkillId).Program!, new SkillProgramCardIdentity(lease.Source.BindingId, [CardZoneKind.Hand], [], [], lease.EffectiveKind, new(SkillProgramConditionKind.Always, 0, [])), lease.Source) : null;
    internal bool IsWuyouIdentityMaterial(CharacterState holder, Card card, CardKind kind, CardConversionSource? source) => ActiveWuyouLease(holder.Seat, card.Id)is { } lease && source == lease.Source && (lease.EffectiveKind == kind || lease.EffectiveKind == CardKind.Slash && kind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(holder));
    internal bool IsWuyouIdentityKind(CharacterState holder, Card card, CardKind kind) => ActiveWuyouLease(holder.Seat, card.Id)?.EffectiveKind == kind;
    internal IReadOnlyList<CardConversionSource> WuyouIdentitySources(CharacterState holder, Card card, CardKind kind) => ActiveWuyouLease(holder.Seat, card.Id)is { } lease && lease.EffectiveKind == kind ? [lease.Source] : [];
    internal bool WuyouNativeProvider(int actor, int provider, CardKind kind) => actor == provider || CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } request && CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(request) && request.OwnerSeat == actor && request.CurrentCandidateSeat == provider;
    internal WuyouHandLease? WuyouPreUseLease(CharacterState actor, CardKind kind, IReadOnlyList<int>? materialIds)
    {
        if (!HasWuyouCapability || materialIds is not [var id])
            return null;
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        if (from is { Zone: CardZoneKind.Hand, OwnerSeat: { } provider } && WuyouNativeProvider(actor.Seat, provider, kind))
            return ActiveWuyouLease(provider, id);
        foreach (var frame in _resolutionStackOperations._resolutionStack.AsEnumerable().Reverse())
        {
            var paid = frame is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration ? declaration.Payment : frame.AcceptedDeclarationPayment;
            if (paid is { Claimed: false, Wuyou: { } lease } && paid.ActorSeat == actor.Seat && paid.Cost.CardId == id && WuyouNativeProvider(actor.Seat, paid.ProviderSeat, kind) && ValidWuyouDeclaration(paid, lease))
                return lease;
        }

        return null;
    }

    internal bool HasWuyouMaterialPrivilege(CharacterState actor, CardKind kind, IReadOnlyList<int>? materialIds, long? existingUseFrameId = null) => existingUseFrameId is { } use ? HasIssuedWuyouUse(use, actor.Seat) : WuyouPreUseLease(actor, kind, materialIds)is not null;
    internal bool HasWuyouPotentialSlash(CharacterState actor) => HasWuyouCapability && _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Any(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) && ActiveWuyouLease(actor.Seat, card.Id)is { } lease && (CoreNativeCardClassificationsRuntime.IsSlashCard(lease.EffectiveKind) || CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, false).Count != 0)));
    internal bool HasPublicWuyouContributionPotential(CharacterState actor) => HasWuyouCapability && _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuyouStartedEvent>().Any(started => started.DonorSeat == actor.Seat);
    internal bool HasPotentialWuyouFactionSlash(CharacterState actor, CardKind kind) => HasWuyouCapability && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(actor, CardKind.Slash)is { } policy && _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(actor.Seat, policy.FactionId).Any(seat => HasPublicWuyouContributionPotential(_coreMatchParticipantsPlayersAccess._players[seat]));
    internal bool IsWuyouFactionPaymentLegal(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false)
    {
        if (!HasWuyouCapability || !CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(pending) || pending.TargetSeat is not { } targetSeat || !CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !HasPotentialWuyouFactionSlash(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat], kind))
            return true;
        var providerSeat = pending.CurrentCandidateSeat;
        var actor = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        var provider = _coreMatchParticipantsPlayersAccess._players[providerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var ids = cards.Select(card => card.Id).ToArray();
        var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards);
        var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards);
        var rank = _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(actor, provider, cards, isTrueZhangba, kind);
        if (pending.IsBorrowedSwordUse)
            return _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, kind, physicalSuit: suit, allowAnyPhysicalSuit: false, effectiveColor: color, effectiveRank: rank, physicalCardIds: ids);
        if (pending.IsQinglongCrescentBladeUse)
            return _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, kind, color, allowAnyColor: false, physicalSuit: suit, effectiveRank: rank, physicalCardIds: ids);
        if (pending.IsAssistedProgramUse)
            return _coreAssistedFactionSlashOperations.IsAssistedProvidedSlashTarget(actor.Seat, targetSeat, kind, rank, allowPotentialRank: false, physicalCardIds: ids);
        return _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(actor, target, kind, physicalSuit: suit, allowAnyPhysicalSuit: false, effectiveColor: color, effectiveRank: rank, physicalCardIds: ids);
    }

    internal void IssueWuyouUse(CardUseFrame use)
    {
        if (!HasWuyouCapability || use.Action is not { Type: CardActionType.Use, PhysicalCards: [var material] } action || material.From.Zone != CardZoneKind.Hand || material.From.OwnerSeat != action.ProviderSeat)
            return;
        if (use.CardKind == CardKind.Alcohol && action.EffectiveKind == CardKind.Alcohol && _characterHouChengAnniversaryOperations.HouChengWineMark(material.CardId)is { } wine && action.ConversionChain.Contains(wine.Source))
            return;
        var payment = use.AcceptedDeclarationPayment ?? _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(action.ProviderSeat, material.CardId);
        var lease = payment is { Wuyou: { } original } && payment.Cost.CardId == material.CardId && payment.ActorSeat == action.ActorSeat && ValidWuyouDeclaration(payment, original) ? original : ActiveWuyouLease(action.ProviderSeat, material.CardId);
        if (lease is null)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || use.WuyouUse is not null || use.PhysicalCardIds is not [var id] || id != material.CardId || use.CardId != id || use.SourceSeat != action.ActorSeat || !WuyouNativeProvider(action.ActorSeat, action.ProviderSeat, use.CardKind) || lease.CardId != material.CardId || lease.HolderSeat != action.ProviderSeat || lease.PrintedKind != material.CardKind || !(use.CardKind == lease.EffectiveKind || lease.EffectiveKind == CardKind.Slash && use.CardKind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]) || action.ConversionChain.Any(source => IsJuewuSource(source) && _corePlayCardHistoryOperations.ViewAsRule(source)is { RankTwoDamageUse: true } rule && (rule.OutputKind == use.CardKind || rule.OutputKind == CardKind.Slash && use.CardKind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat])))))
            throw new InvalidOperationException("Wuyou must bind the original provider Hand entity to its one accepted native use.");
        var receipt = new WuyouNativeUseReceipt(lease, use.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, use.CardKind, material, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { WuyouUse = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouUseAcceptedEvent(receipt));
    }

    internal bool HasIssuedWuyouUse(long useId, int actor)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { WuyouUse: { } r, Action: { Type: CardActionType.Use } action } use || r.UseId != use.Id || r.ActionId != action.ActionId || action.ActorSeat != actor || use.SourceSeat != actor || !WuyouIssuedActorMatches(use, r) || r.ProviderSeat != action.ProviderSeat || action.PhysicalCards is not [var material] || material != r.Material || r.Lease.HolderSeat != r.ProviderSeat || r.Lease.CardId != material.CardId || material.From != CardLocation.Hand(r.ProviderSeat) || !ValidWuyouLease(r.Lease) || !(use.CardKind == r.EffectiveKind || CoreNativeCardClassificationsRuntime.IsSlashCard(r.EffectiveKind) && use.CardKind == CardKind.FireSlash && _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use)))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WuyouUseAcceptedEvent>().Where(e => e.Receipt.UseId == useId).ToArray()is [var accepted] && accepted.Receipt == r;
    }

    internal bool WuyouIssuedActorMatches(CardUseFrame use, WuyouNativeUseReceipt receipt)
    {
        var actor = receipt.ActorSeat;
        foreach (var change in _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id))
        {
            if (change.PreviousActorSeat != actor || change.ProviderSeat != receipt.ProviderSeat)
                return false;
            actor = change.ActorSeat;
        }

        return use.SourceSeat == actor && use.Action?.ActorSeat == actor;
    }

    internal DeclaredCardPayment CaptureWuyouDeclarationPayment(DeclaredCardPayment payment, CardDeclarationParentReturn parent)
    {
        if (parent.Purpose is not (CardDeclarationPurpose.Use or CardDeclarationPurpose.Dodge or CardDeclarationPurpose.Counterspell or CardDeclarationPurpose.Recovery or CardDeclarationPurpose.BorrowedSword or CardDeclarationPurpose.ProvidedSlash or CardDeclarationPurpose.BorrowedSwordProvidedSlash or CardDeclarationPurpose.QinglongProvidedSlash) || payment.Claimed || payment.Wuyou is not null || ActiveWuyouLease(payment.ProviderSeat, payment.Cost.CardId)is not { } lease)
            return payment;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouDeclarationBoundEvent(payment.DeclarationId, payment.OwnerFrameId, payment.ProviderSeat, parent.ActorSeat, payment.Cost.CardId, lease));
        return payment with
        {
            Wuyou = lease
        };
    }

    internal bool ValidWuyouDeclaration(DeclaredCardPayment payment, WuyouHandLease lease) => payment.Wuyou == lease && ValidWuyouLease(lease) && lease.HolderSeat == payment.ProviderSeat && lease.CardId == payment.Cost.CardId && payment.Cost.From == CardLocation.Hand(payment.ProviderSeat) && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == payment.OwnerFrameId)is { } owner && (owner.AcceptedDeclarationPayment == payment || owner is CardDeclarationFrame d && d.Payment == payment) && _coreProgramEventHistoryOperations.ProgramEventHistory<WuyouDeclarationBoundEvent>().Where(e => e.DeclarationId == payment.DeclarationId).ToArray()is [var fact] && fact.OwnerFrameId == payment.DeclarationId && fact.ProviderSeat == payment.ProviderSeat && fact.ActorSeat == payment.ActorSeat && fact.CardId == payment.Cost.CardId && fact.Lease == lease;
    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 28520)]
    internal void AddWuyouIdentityActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        if (!HasWuyouCapability)
            return;
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(c => selectedPhysicalCardId is null || selectedPhysicalCardId == c.Id))
        {
            var lease = ActiveWuyouLease(actor.Seat, card.Id);
            if (lease is null)
                continue;
            var kind = lease.EffectiveKind;
            var source = lease.Source;
            var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card);
            var color = CoreGameEnginePublicPileColorsRuntime.SuitColor(suit);
            if (kind == CardKind.IronChain)
                actions.Add(new(LegalActionKind.Recast, card.Id, null, "武佑：重铸【铁索连环】", kind) { ConversionSource = source });
            if (_characterHouChengAnniversaryOperations.IsAnniversaryHouChengWineCard(card.Id))
                continue;
            if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
                continue;
            if (CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && kind != CardKind.Nullification)
            {
                foreach (var option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: color, hasActualColor: true, physicalCardIds: [card.Id], conversionSource: source))
                    actions.Add(new(option.ActionKind, card.Id, option.TargetSeats.Count == 1 ? option.TargetSeats[0] : null, "武佑：" + option.Description, kind, option.TargetCardId, option.TargetSeats) { ConversionSource = source });
            }
            else if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, suit))
                actions.Add(new(LegalActionKind.Peach, card.Id, actor.Seat, "武佑：使用【桃】", kind) { ConversionSource = source });
            else if (kind == CardKind.Alcohol && actor.HasHpValue && !actor.HasAlcoholEffect && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, suit))
                actions.Add(new(LegalActionKind.Alcohol, card.Id, null, "武佑：使用【酒】", kind) { ConversionSource = source });
            else if (kind is CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning)
            {
                var actionKind = kind == CardKind.Indulgence ? LegalActionKind.Indulgence : kind == CardKind.SupplyShortage ? LegalActionKind.SupplyShortage : LegalActionKind.Lightning;
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !p.JudgmentAreaAbolished && (kind == CardKind.Lightning ? p.Seat == actor.Seat : p.Seat != actor.Seat) && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(p, kind) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, suit)))
                    actions.Add(new(actionKind, card.Id, target.Seat, $"武佑：对 {(target.Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】", kind) { ConversionSource = source });
            }
        }
    }

    internal bool TryExecuteWuyouIdentityUse(CharacterState actor, Card card, LegalAction action)
    {
        if (action.Kind == LegalActionKind.Recast || action.PlayedCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) || kind == CardKind.Nullification || !IsWuyouIdentityMaterial(actor, card, kind, action.ConversionSource))
            return false;
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card);
        var adjusted = _coreRedAdditionalTargetsOperations.IsTargetAdjustmentAction(actor, action) && _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats?.SequenceEqual(action.TargetSeats) == true;
        var option = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(suit), hasActualColor: true, physicalCardIds: [card.Id], conversionSource: action.ConversionSource).Where(o => o.ActionKind == action.Kind && o.TargetCardId == action.TargetCardId && (o.TargetSeats.SequenceEqual(action.TargetSeats) || adjusted)).OrderByDescending(o => o.TargetSeats.Count).FirstOrDefault() ?? throw new InvalidOperationException("Wuyou's original named entity lost its native trick targets.");
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, option.TargetSeats, kind, conversionSource: action.ConversionSource, designatedTargetSeats: kind == CardKind.BorrowedSword && adjusted && action.TargetSeats.Count >= 4 ? action.TargetSeats.Where((_, index) => index % 2 == 0).ToArray() : null);
        var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(actor.Seat), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, targets, option.ActionKind, option.TargetCardId, option.RequiredCardKind, kind);
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 28520)]
    internal void AssertWuyouNativeUses()
    {
        if (!HasWuyouCapability)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.WuyouUse is not null))
            if (!HasIssuedWuyouUse(use.Id, use.SourceSeat))
                throw new InvalidOperationException("Wuyou's accepted native entity lost its original provider, identity or exact action invoice.");
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 30)]
    internal bool DistanceCardUseExemption30HasIssuedWuyouUse(ProgramCardDistanceQuery q) => q.CardUseFrameId is { } wuyouUse && HasIssuedWuyouUse(wuyouUse, q.Actor.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 20)]
    internal bool DistanceProvidedSlash20HasPotentialWuyouFactionSlash(ProgramSlashDistanceQuery q) => q.AllowAnyPhysicalSuit && q.PhysicalCardIds is null && HasPotentialWuyouFactionSlash(q.Actor, q.Kind);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 30)]
    internal bool DistanceBorrowedSlash30HasPublicWuyouContributionPotential(ProgramSlashDistanceQuery q) => q.PhysicalCardIds is null && q.AllowAnyPhysicalSuit && HasPublicWuyouContributionPotential(q.Actor);
}
