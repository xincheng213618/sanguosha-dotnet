#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuOlMouRuntime
{
    internal bool MouGuquAcceptedUse(int actor, CardKind kind, IReadOnlyList<int> ids, long? useId)
    {
        if (useId is not { } id || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { Type: CardActionType.Use } a } use || use.SourceSeat != actor || a.ActorSeat != actor || use.CardKind != kind || !a.PhysicalCards.Select(c => c.CardId).SequenceEqual(ids))
            return false;
        if (use.GuquAdmission is not { } admitted || admitted.UseId != use.Id || admitted.ActionId != a.ActionId || admitted.ProviderSeat != a.ProviderSeat || admitted.MaterialCount != ids.Count || admitted.EffectiveKind != kind || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuquUseAcceptedEvent>().Count(e => e.Admission == admitted) != 1 || !MouGuquActorLineageMatches(use.Id, a.ActionId, admitted.ActorSeat, actor, a.ProviderSeat, a.Type))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == id && e.SourceSeat == admitted.ActorSeat && e.CardId == use.CardId && (e.CardKind == kind || CoreNativeCardClassificationsRuntime.IsSlashCard(e.CardKind) && CoreNativeCardClassificationsRuntime.IsSlashCard(kind))) == 1;
    }

    internal bool MouGuquActorLineageMatches(long useId, long actionId, int originalActor, int currentActor, int provider, CardActionType type)
    {
        if (originalActor == currentActor)
            return true;
        if (type != CardActionType.Use || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { } action } use || action.ActionId != actionId || action.ActorSeat != currentActor || action.ProviderSeat != provider || use.SourceSeat != currentActor)
            return false;
        var cursor = originalActor;
        foreach (var changed in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == useId))
        {
            if (changed.PreviousActorSeat != cursor || changed.ProviderSeat != provider)
                return false;
            cursor = changed.ActorSeat;
        }

        return cursor == currentActor;
    }

    internal bool MouGuquPrepaidUse(int actor, CardKind kind, IReadOnlyList<int> ids, CardConversionSource? source)
    {
        if (ids is not [var id] || _coreCardDeclarationsOperations.LiveDeclarationPayment(id)is not { GuquPermit: { } permit } paid || !ValidMouGuquDeclaration(paid) || permit.ActorSeat != actor || permit.Conversion != source || permit.OutputKind != kind && !(CoreNativeCardClassificationsRuntime.IsSlashCard(permit.OutputKind) && CoreNativeCardClassificationsRuntime.IsSlashCard(kind)))
            return false;
        return _resolutionStackOperations._resolutionStack.Any(f => f.AcceptedDeclarationPayment == paid) || _resolutionStackOperations._resolutionStack.LastOrDefault()is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } d && d.Payment == paid;
    }

    internal bool MouGuquPhysicalUseForbidden(int actor, IReadOnlyList<int> ids)
    {
        if (!HasMouGuquCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor))
            return false;
        // A declaration has already signed this permission before its true
        // material left Hand. Its cost children cannot retroactively add a ban.
        if (ids is [var id] && _coreCardDeclarationsOperations.LiveDeclarationPayment(id)is { GuquPermit: { } p } paid && p.ActorSeat == actor && ValidMouGuquDeclaration(paid))
            return false;
        var sources = MouGuquRestrictingSources(actor);
        if (sources.Count == 0)
            return false;
        // Hand ownership activates the restriction; the allowed original
        // entities can also reside in another native usable owned zone.
        return ids.Count == 0 || ids.Distinct().Count() != ids.Count || sources.Any(s => ids.Any(id => !IsMouGuquQueForSource(s.Source, id)));
    }

    internal bool IsMouGuquUseAllowed(int actorSeat, CardKind kind, IReadOnlyList<int> physicalIds, CardConversionSource? source = null, long? existingUseFrameId = null)
    {
        if (_characterZhugeLiangWumiaoOperations.IsWuZglUseMaterialForbidden(actorSeat, physicalIds, existingUseFrameId))
            return false;
        if (!HasMouGuquCapability || MouGuquAcceptedUse(actorSeat, kind, physicalIds, existingUseFrameId) || MouGuquPrepaidUse(actorSeat, kind, physicalIds, source))
            return true;
        var sources = MouGuquRestrictingSources(actorSeat);
        if (sources.Count == 0)
            return true;
        if (MouGuquPhysicalUseForbidden(actorSeat, physicalIds))
            return false;
        // The selected current client adds the exact conversion restriction
        // omitted from the public hero text. It affects only these other
        // holders; the Guqu issuer's own conversions remain normal.
        if (physicalIds is [var id] && source is null && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == kind)
            return true;
        return physicalIds is [var shown] && kind == CardKind.Alcohol && IsExactMouZuifengAlcoholConversion(actorSeat, shown, source);
    }

    internal bool IsMouGuquPaidResponseAllowed(CardActionContext action)
    {
        var ids = action.PhysicalCards.Select(c => c.CardId).ToArray();
        var conversion = action.ConversionChain.FirstOrDefault();
        if (MouGuquPrepaidUse(action.ActorSeat, action.EffectiveKind, ids, conversion))
            return true;
        // The native action is constructed after its real payment. Reconstruct
        // only its original Hand residence from the frozen native cost; do not
        // demand that its now-paid entity is still physically in Hand.
        var originalHand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).Select(c => c.Id).Concat(action.PhysicalCards.Where(c => c.From == CardLocation.Hand(action.ActorSeat)).Select(c => c.CardId)).ToHashSet();
        var sources = CurrentMouGuquSources().Where(s => s.Source.OwnerSeat != action.ActorSeat && originalHand.Any(id => IsMouGuquQueForSource(s.Source, id))).ToArray();
        if (sources.Length == 0)
            return true;
        if (ids.Length == 0 || ids.Distinct().Count() != ids.Length || sources.Any(s => ids.Any(id => !IsMouGuquQueForSource(s.Source, id))))
            return false;
        return action.PhysicalCards is [var cost] && conversion is null && cost.CardKind == action.EffectiveKind && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cost.CardId && m.From == cost.From && (m.To == CardLocation.Processing || m.To == CardLocation.DiscardPile) && (m.Reason == CardMoveReasons.Respond || m.Reason == CardMoveReasons.Nullification));
    }

    internal int? MouGuquResponseUseActor(CharacterState provider, CardKind kind, bool dyingUse = false)
    {
        if (dyingUse || kind == CardKind.Nullification)
            return provider.Seat;
        if (kind != CardKind.Dodge)
            return null; // Duel/AOE Slash is pure Played.
        return _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(provider, kind) ? provider.Seat : null;
    }

    internal bool IsMouGuquResponseAllowed(CharacterState provider, CardKind kind, IReadOnlyList<int> ids, CardConversionSource? source = null, bool dyingUse = false) => (source is not null || _characterHanDangMouMobileOperations.IsMouHanDangResponseAvailable(provider, kind, ids, dyingUse)) && !_characterZhugeLiangWumiaoOperations.IsWuZglUseMaterialForbidden(provider.Seat, ids) && (MouGuquResponseUseActor(provider, kind, dyingUse)is not { } actor || IsMouGuquUseAllowed(actor, kind, ids, source));
    internal bool IsMouGuquCardlessAllowed(CharacterState actor, CardKind kind, bool forResponse, bool dyingUse = false) => forResponse && !dyingUse ? IsMouGuquResponseAllowed(actor, kind, []) : IsMouGuquUseAllowed(actor.Seat, kind, []);
    internal bool IsMouGuquPlayActionAllowed(CharacterState actor, LegalAction action, CardKind kind) => action.Kind == LegalActionKind.Recast || IsMouGuquUseAllowed(actor.Seat, kind, action.CardId is> 0 ? [action.CardId.Value] : [], action.ConversionSource);
    internal bool IsMouGuquViewAsAllowed(CharacterState owner, CardKind kind, IReadOnlyList<int> ids, CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = null) => requestedUseTarget is not null || dyingUse || !forResponse ? IsMouGuquUseAllowed(owner.Seat, kind, ids, source) : IsMouGuquResponseAllowed(owner, kind, ids, source);
    internal DeclaredCardPayment CaptureMouGuquDeclaration(DeclaredCardPayment paid, CardDeclarationParentReturn parent)
    {
        if (!HasMouGuquCapability || !CharacterXiaoQiaoLeRuntime.WeiwanDeclarationIsTrueUse(parent))
            return paid;
        if (paid.GuquPermit is not null || !IsMouGuquUseAllowed(parent.ActorSeat, paid.DeclaredKind, [paid.Cost.CardId], paid.Source))
            throw new InvalidOperationException("Guqu cannot accept a forbidden or previously paid declaration conversion.");
        var permit = new GuquDeclarationPermit(paid.DeclarationId, parent.ActorSeat, paid.ProviderSeat, paid.Cost.CardId, paid.Cost.CardKind, paid.Cost.From, paid.Source, paid.DeclaredKind, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuquDeclarationCapturedEvent(permit));
        return paid with
        {
            GuquPermit = permit,
            GuquDeclarationClaims = CaptureMouGuquClaims(paid.DeclarationId, 0, parent.ActorSeat, paid.ProviderSeat, paid.DeclaredKind, [paid.Cost])
        };
    }

    internal bool ValidMouGuquDeclaration(DeclaredCardPayment paid) => paid.GuquPermit is { } p && p.DeclarationId == paid.DeclarationId && p.ActorSeat == paid.ActorSeat && p.ProviderSeat == paid.ProviderSeat && p.CardId == paid.Cost.CardId && p.PrintedKind == paid.Cost.CardKind && p.From == paid.Cost.From && p.Conversion == paid.Source && p.OutputKind == paid.DeclaredKind && p.From == CardLocation.Hand(p.ProviderSeat) && p.MovementSequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuquDeclarationCapturedEvent>().Count(e => e.Permit == p) == 1;
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeLegalActionKind, 10)]
    internal bool MouGuquEquipmentActionForbidden(ProgramLegalActionQuery query) => query.Action.Kind == LegalActionKind.UseEquipmentEffect && query.Action.EquipmentKind == CardKind.ZhangbaSerpentSpear && MouGuquRestrictingSources(query.Actor.Seat).Count != 0;
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 10)]
    internal bool MouGuquLegalActionForbidden(ProgramLegalActionQuery query) => !IsMouGuquPlayActionAllowed(query.Actor, query.Action, query.EffectiveKind!.Value);
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 10)]
    internal bool MouGuquViewAsForbidden(ProgramViewAsLegalityQuery query) => !IsMouGuquViewAsAllowed(query.Owner, query.Kind, query.CardIds, query.Source, query.ForResponse, query.DyingUse, query.RequestedUseTarget);
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 10)]
    internal bool MouGuquNativeResponseForbidden(ProgramNativeResponseQuery query) => !IsMouGuquResponseAllowed(query.Provider, query.Kind, query.CardIds, query.Source, query.DyingUse);
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 20)]
    internal void ValidateMouGuquSubmittedUse(ProgramSubmittedUseQuery query)
    {
        if (!IsMouGuquUseAllowed(query.ActorSeat, query.Kind, query.CardIds, query.ConversionChain.FirstOrDefault()))
            throw new InvalidOperationException("Guqu forbids this true Use material or conversion.");
    }
}
