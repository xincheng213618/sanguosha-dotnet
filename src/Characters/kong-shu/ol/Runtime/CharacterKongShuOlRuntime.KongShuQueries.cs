#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKongShuOlRuntime
{
    internal bool HasLeiluanCapability => _contentRegistryOperations._contentRegistry?.ProgramDependencies.UsesLeiluanBasicUse == true;
    internal bool HasKongShuCapability => HasLeiluanCapability || _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveFuchaoCurrentTurnChain) == true || _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveFuchaoRoundEndingDrawAndDiscard) == true;
    internal int LeiluanCurrentX => Math.Max(1, _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && p.IsChained));

    internal bool LeiluanDisabled(int owner, string state)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var at = Array.FindLastIndex(history, e => e is LeiluanDisabledEvent d && d.OwnerSeat == owner && d.SkillId == "ol:leiluan" && d.StateId == state);
        return at >= 0 && !history.Skip(at + 1).OfType<DamageAppliedEvent>().Any(e => e.TargetSeat == owner && e.Amount > 0);
    }

    internal bool LeiluanNameUsed(int owner, CardKind kind)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        foreach (var e in history.OfType<TrueRoundCardNameUsedEvent>().Where(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(e.EffectiveKind) == CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(kind)))
        {
            var actor = e.ActorSeat;
            var native = history.OfType<CurrentTurnCardUseKindsRecordedEvent>().SingleOrDefault(n => e.CardActionId is { } id ? n.CardActionId == id : n.CardActionId is null && n.OriginFrameId == e.OriginFrameId);
            var accepted = e.CardActionId is { } actionId ? history.OfType<CardActionAcceptedEvent>().FirstOrDefault(a => a.Action.ActionId == actionId)?.Action : null;
            var response = e.NullificationUse || native?.RoundActionType == CardActionType.Response || accepted?.Type == CardActionType.Response;
            if (!response)
            {
                var declared = history.OfType<CardUseDeclaredEvent>().SingleOrDefault(d => d.ResolutionId == e.OriginFrameId);
                if (declared is null || declared.SourceSeat != e.ActorSeat)
                    throw new InvalidOperationException("Leiluan round history lost its native declaration identity.");
                foreach (var change in history.OfType<ProgramCardUseActorReplacedEvent>().Where(c => c.CardUseFrameId == e.OriginFrameId))
                {
                    if (change.PreviousActorSeat != actor || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(change.ActorSeat))
                        throw new InvalidOperationException("Leiluan round history lost its real actor replacement chain.");
                    actor = change.ActorSeat;
                }
            }

            if (actor == owner)
                return true;
        }

        return false;
    }

    internal bool LeiluanNativeDirection(CharacterState owner, CardKind kind, bool response)
    {
        if (!response)
            return kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.Peach or CardKind.Alcohol;
        if (kind is CardKind.Peach or CardKind.Alcohol)
            return _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.ResponderSeat == owner.Seat && _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp <= 0 && (kind == CardKind.Peach ? _coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(owner.Seat, dying.VictimSeat) : dying.VictimSeat == owner.Seat && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat));
        return kind == CardKind.Dodge && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(owner, kind);
    }

    internal IReadOnlyList<ProgramMultiCardViewAsSelection> LeiluanSelections(CharacterState owner, CardKind kind, bool forResponse, bool ignoreSuitUseProhibition = false)
    {
        if (!HasLeiluanCapability || !owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 || !LeiluanNativeDirection(owner, kind, forResponse))
            return[];
        var x = LeiluanCurrentX;
        var candidates = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Concat(_coreNativeDamageHandlesOperations.UsesFormalWoodenOx && _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(c => c.Kind == CardKind.WoodenOx) ? _cardZonesOperations._cardZones.CardsAt(CardLocation.WoodenOxGrain(owner.Seat)) : []).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, c) && !_coreCardConversionsOperations.HasProgramCardIdentity(owner, c)).OrderBy(c => c.Id).ToArray();
        if (candidates.Length < x)
            return[];
        var result = new List<ProgramMultiCardViewAsSelection>();
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances)
            foreach (var rule in instance.Program.ViewAs.Where(r => r.LeiluanBasicUse is not null && (r.OutputKind == kind || !forResponse && kind == CardKind.Slash && CoreNativeCardClassificationsRuntime.IsSlashCard(r.OutputKind))))
            {
                if (LeiluanDisabled(owner.Seat, rule.LeiluanBasicUse!.StateId) || LeiluanNameUsed(owner.Seat, rule.OutputKind) || _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, rule.OutputKind, CardActionType.Use))
                    continue;
                var source = new CardConversionSource(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId);
                foreach (var cards in CoreGameEngineCardConversionsRuntime.EnumerateCardCombinations(candidates, x))
                {
                    var ids = cards.Select(c => c.Id).ToArray();
                    if (forResponse ? !_coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(owner, cards, kind) || _characterCuiYanOlOperations.IsCuiYanNativeResponseForbidden(owner, kind, cards) : !ignoreSuitUseProhibition && (_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner.Seat, ids) || _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(owner.Seat, ids, cards.Count == 1 ? _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, cards) : Suit.None)))
                        continue;
                    result.Add(new(Array.AsReadOnly(cards.ToArray()), source, rule.OutputKind));
                }
            }

        return Array.AsReadOnly(result.DistinctBy(s => (s.Source, s.OutputKind, Cards: string.Join('-', s.Cards.Select(c => c.Id)))).ToArray());
    }

    internal bool IsLeiluanSelection(ProgramMultiCardViewAsSelection selection) => _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.LeiluanBasicUse is not null;
    internal bool IsLeiluanSource(CardConversionSource? source) => source is not null && _corePlayCardHistoryOperations.ViewAsRule(source)?.LeiluanBasicUse is not null;
}
