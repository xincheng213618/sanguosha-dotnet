#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuJinDingOlRuntime
{
    internal bool HasZhongshenCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesRoundGainedRedHandOnly;

    internal bool MatchesZhongshenRoundGain(CharacterState owner, Card card, SkillProgramViewAs rule)
    {
        if (rule.RoundGainedRedHandOnly != true)
            return true;
        if (!owner.IsAlive || card.IsGeneralWeapon || _cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Hand(owner.Seat) || CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)) != true || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhongshenRoundBoundaryEvent>().SingleOrDefault(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber)is not { } round)
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card.Id && m.Sequence > round.MovementSequence && m.To == CardLocation.Hand(owner.Seat) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m) != CardLocation.Hand(owner.Seat));
    }

    // Registration only; the query still belongs to legacy GameEngine.Zhongshen.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 20)]
    internal void ProjectZhongshenCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasZhongshenCapability && _coreMatchParticipantsPlayersAccess._players.Any(player => _cardZonesOperations._cardZones.Contains(CardLocation.Hand(player.Seat), cardId)) && HasZhongshenCardMark(cardId) ? "重身" : null);
    }

    internal bool HasZhongshenCardMark(int cardId)
    {
        if (_cardZonesOperations._cardZones.GetLocation(cardId)is not { Zone: CardZoneKind.Hand, OwnerSeat: { } owner } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner))
            return false;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId);
        return _coreSkillProgramsOperations.EnabledViewAsPrograms(_coreMatchParticipantsPlayersAccess._players[owner]).SelectMany(p => p.ViewAs).Any(r => r.RoundGainedRedHandOnly == true && MatchesZhongshenRoundGain(_coreMatchParticipantsPlayersAccess._players[owner], card, r));
    }
}
