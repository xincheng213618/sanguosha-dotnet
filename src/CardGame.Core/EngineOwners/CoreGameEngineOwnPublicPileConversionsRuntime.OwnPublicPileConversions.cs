#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOwnPublicPileConversionsRuntime
{
    internal bool IsOwnPublicPileSlashMaterial(CharacterState owner, Card card) => IsOwnPublicPileCard(owner, card) && _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, CardKind.Slash, forResponse: false).Any(source => _corePlayCardHistoryOperations.ViewAsRule(source)?.SourcePileSkillId is not null);
    internal bool IsOwnPublicPileActionCost(CardActionContext action, CardActionCost cost) => _coreSourceDingxiNativeOperations.IsSourceDingxiOriginalActionCost(action, cost) || _characterHuangZuMobileShiOperations.IsShiHzOriginalActionCost(action, cost) || _characterGongsunZanMouMobileOperations.IsMouGszHuActionCost(action, cost) || _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalActionCost(action, cost) || action.PhysicalCards is [var only] && only == cost && cost.From.Zone == CardZoneKind.PublicPersistentPile && cost.From.OwnerSeat == action.ProviderSeat && action.ConversionChain.Any(source => source.OwnerSeat == action.ProviderSeat && _corePlayCardHistoryOperations.ViewAsRule(source)is { SourcePileSkillId: { } skill, InputCount: 1, OutputKind: CardKind.Slash } && CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && (skill == source.SkillId && (cost.From.PublicPileId is null || cost.From.PublicPileId == CoreGameEnginePublicPileSourcesRuntime.PublicPileIdentity(skill, source.SkillInstanceId)) || _corePublicPileSourcesOperations.ReferencedPublicPileSources(action.ProviderSeat, skill, source.SkillInstanceId).Any(pile => pile.Location == cost.From)));
    internal CardKind PhysicalSlashUseBaseKind(CharacterState owner, Card card) => _characterLuBuOlMagicOperations.MoGangquanPhysicalFire(owner, card) ? CardKind.FireSlash : _characterGuanYuWumiaoOperations.WuyouIdentity(owner, card)?.Identity.OutputKind is { } wuyou && CoreNativeCardClassificationsRuntime.IsSlashCard(wuyou) ? wuyou : IsOwnPublicPileCard(owner, card) && !_coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(owner, card, card.Kind, false) ? CardKind.Slash : CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) ? card.Kind : CardKind.Slash;
    internal void RefreshOwnPublicPileAddedTargetCandidates(CardUseFrame use, CardUseFrame previous)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.UsesOwnPublicPileConversions || use.Action is not { Type: CardActionType.Use, PhysicalCards: [var cost] } action || previous.Action is not { } previousAction || previousAction.ActionId != action.ActionId || !IsOwnPublicPileActionCost(action, cost))
            return;
        var added = action.EffectiveDesignatedTargetSeats.Except(previousAction.EffectiveDesignatedTargetSeats).Distinct().Order().ToArray();
        if (added.Length == 0)
            return;
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.ParentFrameId == use.Id && CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) == SkillProgramTriggerWindow.CardUseTargetsFinalized);
        if (window is null)
            return;
        // Additions keep the paid action and original candidates. Each new target
        // receives only its matching pile-conversion binding, once per source.
        var candidates = _coreCardActionsOperations.CollectSharedCardActionCandidates(action, SkillProgramTriggerWindow.CardUseTargetsFinalized, added, null, window.Continuation).Where(c =>
        {
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program!.Triggers.Single(t => t.Id == c.TriggerId);
            return trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.ConversionSource && !trigger.SingleActionInstance && added.Contains(c.OpponentSeat) && action.ConversionChain.Any(source => source.OwnerSeat == c.OwnerSeat && source.SkillId == trigger.SourceSkillId && (trigger.SourceViewAsId is null || source.BindingId == trigger.SourceViewAsId) && _corePlayCardHistoryOperations.ViewAsRule(source)?.SourcePileSkillId is not null) && !window.Candidates.Any(old => old.OwnerSeat == c.OwnerSeat && old.SkillId == c.SkillId && old.TriggerId == c.TriggerId && old.SkillInstanceId == c.SkillInstanceId && old.OpponentSeat == c.OpponentSeat);
        }).OrderByDescending(c => c.Priority).ThenBy(c => _coreTableOrderOperations.RelativeTableOrder(0, c.OwnerSeat)).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).ThenBy(c => c.OpponentSeat).Select(c => c with { FrozenContext = c.FrozenContext!with { ParentFrameId = window.Id, CardUse = c.FrozenContext.CardUse!with { ParentCardUseFrameId = use.Id } } }).ToArray();
        if (candidates.Length > 0)
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Candidates = Array.AsReadOnly(window.Candidates.Concat(candidates).ToArray()) });
    }

    internal Card? OwnPublicPileResponseMaterial(IReadOnlyList<CardActionCost> costs, CardConversionSource? source) => costs is [var cost] && cost.From.Zone == CardZoneKind.PublicPersistentPile && (source is not null && _corePlayCardHistoryOperations.ViewAsRule(source)?.SourcePileSkillId is not null || source is null && _resolutionStackOperations._resolutionStack.Any(f => f.NativePublicPilePayment is { SequenceAfter: > 0 } p && p.Card.Id == cost.CardId && p.Card.Kind == cost.CardKind && p.Pile.Location == cost.From)) ? _coreCardAttackStateOperations.GetAttackCard(cost.CardId) : null;
    internal bool IsOwnPublicPileCard(CharacterState owner, Card card) => _cardZonesOperations._cardZones.GetLocation(card.Id)is { Zone: CardZoneKind.PublicPersistentPile } location && location.OwnerSeat == owner.Seat && !_characterGongsunZanMouMobileOperations.IsMouGszHuCard(owner, card.Id);
    internal bool MatchesOwnPublicPileConversion(CharacterState owner, Card card, IndexedSkillProgramInstance instance, SkillProgramViewAs rule)
    {
        if (rule.SourcePileSkillId is not { } sourceSkill)
            return !IsOwnPublicPileCard(owner, card);
        if (!owner.IsAlive || !IsOwnPublicPileCard(owner, card))
            return false;
        return _corePublicPileSourcesOperations.ReferencedPublicPileSources(owner.Seat, sourceSkill, instance.SkillInstanceId).Any(pile => pile.Location == _cardZonesOperations._cardZones.GetLocation(card.Id) && (sourceSkill != instance.SkillId || pile.SkillInstanceId == instance.SkillInstanceId) && _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(source => source.SkillId == pile.SkillId && source.SkillInstanceId == pile.SkillInstanceId));
    }

    internal IReadOnlyList<Card> GetOwnPublicPileConversionCards(CharacterState owner)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.UsesOwnPublicPileConversions || !owner.IsAlive || _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Count == 0)
            return[];
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.SelectMany(instance => instance.Program.ViewAs.Where(rule => rule.SourcePileSkillId is not null).SelectMany(rule => _corePublicPileSourcesOperations.ReferencedPublicPileSources(owner.Seat, rule.SourcePileSkillId!, instance.SkillInstanceId).SelectMany(_corePublicPileSourcesOperations.PublicPileCards).Where(card => MatchesOwnPublicPileConversion(owner, card, instance, rule)))).DistinctBy(card => card.Id).ToArray();
    }

    internal Card? FindOwnPublicPileConversionCard(CharacterState owner, int cardId) => GetOwnPublicPileConversionCards(owner).SingleOrDefault(card => card.Id == cardId);
    internal void ValidateOwnPublicPileConversionCommit(CardConversionSource? source, CharacterState provider, IReadOnlyList<Card> materials, bool forResponse, CardKind? effectiveKind = null)
    {
        if (_characterGongsunZanMouMobileOperations.ValidateMouGszHuConversion(provider, materials, source, forResponse, effectiveKind))
            return;
        if (source is null && materials is [var original] && (_coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(provider, original, original.Kind, forResponse) || !forResponse && _characterPangTongFriendOperations.IsFriendYangmingOriginalUseChoice(provider, original, original.Kind)))
            return;
        if (!materials.Any(card => IsOwnPublicPileCard(provider, card)) && (source is null || _corePlayCardHistoryOperations.ViewAsRule(source)?.SourcePileSkillId is null))
            return;
        if (source is null || source.OwnerSeat != provider.Seat || materials.Count != 1 || _corePlayCardHistoryOperations.ViewAsRule(source)is not { SourcePileSkillId: not null } rule || !_coreActionQueriesOperations.GetProgramViewAsConversions(provider, materials[0], rule.OutputKind, forResponse).Contains(source))
            throw new InvalidOperationException("The public pile entity or its exact live conversion source is no longer legal at payment.");
    }
}
