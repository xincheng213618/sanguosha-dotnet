namespace CardGame.Core;

// Builds frozen trigger operands at their original read points. Runtime modules
// retain only shared services; these scoped cursors preserve live owner identity.
[ProgramRuntimeModule]
internal sealed class ProgramTriggerFactProjector(ICardZoneQueries zones, IProgramContentQueries content,
    IProgramEventBoundaryQueries history, IProgramTurnQueries turn, IProgramPhaseQueries phase)
{
    internal static SkillProgramUnselectedDualHpFacts? CaptureUnselectedHp<TQuery>(TQuery query, CharacterState player)
        where TQuery : struct, IProgramTriggerFactQueries =>
        query.UnambiguousHp(player) is null && player.HappyQiaoHealth is { } health
            ? new(player.Seat, health.DaQiaoHp, health.XiaoQiaoHp, health.MaximumHp) : null;

    internal (int Value, bool Undefined, int? FixedMinimum, IReadOnlyList<SkillProgramUnselectedDualHpFacts>? Unselected)
        CaptureLivingHp<TQuery>(TQuery query) where TQuery : struct, IProgramTriggerFactQueries
    {
        int? minimum = null;
        List<SkillProgramUnselectedDualHpFacts>? unselected = null;
        foreach (var player in query.Players.Where(player => player.IsAlive && player.HasHpValue))
        {
            if (query.UnambiguousHp(player) is { } hp) minimum = minimum is { } prior ? Math.Min(prior, hp) : hp;
            else if (CaptureUnselectedHp(query, player) is { } dual) (unselected ??= []).Add(dual);
        }
        return unselected is not null
            ? (0, true, minimum, Array.AsReadOnly(unselected.ToArray()))
            : (minimum ?? 0, minimum is null && query.Players.Any(player => player.IsAlive && !player.HasHpValue), null, null);
    }

    internal SkillProgramTriggerFacts Capture<TQuery>(TQuery query, CharacterState owner)
        where TQuery : struct, IProgramTriggerFactQueries
    {
        var ownerHp = query.UnambiguousHp(owner);
        var ownerDual = CaptureUnselectedHp(query, owner);
        var turnOwner = query.Players[turn.CurrentSeat];
        var turnOwnerHp = query.UnambiguousHp(turnOwner);
        var turnOwnerDual = CaptureUnselectedHp(query, turnOwner);
        if (turnOwnerHp is null && turnOwnerDual is null) turnOwnerHp = turnOwner.Hp;
        var livingHp = CaptureLivingHp(query);
        var turnDeaths = content.UsesActualTurnDeathBenefits
            ? query.ActualTurnDeathCounts(owner.Seat) : ((int Deaths, int Kills)?)null;
        var states = new Dictionary<string, bool>(StringComparer.Ordinal);
        foreach (var instance in query.GetSkillBindingShard(owner).ProgramInstances)
            foreach (var definition in instance.Program.BooleanStates)
            {
                states[SkillProgramTriggerFacts.BooleanStateKey(
                    instance.SkillId, instance.SkillInstanceId, definition.Id)] =
                    query.GetProgramBooleanState(owner.Seat, instance.SkillId, instance.SkillInstanceId, definition.Id);
            }
        return new(
            query.CountCardsUsedByCurrentPlayerThisTurn(owner.Seat),
            ownerHp ?? (ownerDual is null ? owner.Hp : 0),
            query.UsesClassicBattleRules,
            CardsUsedOrRespondedThisTurn: query.CountCardsUsedOrRespondedByPlayerThisTurn(owner.Seat),
            OwnerIsTurnPlayer: owner.Seat == turn.CurrentSeat,
            TurnOwnerFactionId: content.UsesTriggerCondition(SkillProgramTriggerConditionKind.TurnOwnerFactionIs)
                ? query.GetEffectiveFactionId(query.Players[turn.CurrentSeat]) : null,
            TurnOwnerDamageDealtThisTurn: content.UsesTriggerValue(SkillProgramTriggerValueKind.TurnOwnerDamageDealtThisTurn)
                ? history.SinceLastBoundary(item => item is TurnStartedEvent).OfType<DamageAppliedEvent>()
                    .Where(item => !item.SourceLess && item.SourceSeat == turn.CurrentSeat && item.Amount > 0).Sum(item => item.Amount) : null,
            TurnOwnerSlashUseCount: content.UsesTriggerValue(SkillProgramTriggerValueKind.TurnOwnerSlashUseCount)
                ? history.SinceLastBoundary(item => item is TurnStartedEvent).OfType<CardUsedEvent>()
                    .Count(item => item.SourceSeat == turn.CurrentSeat &&
                        item.CardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash) : null,
            CurrentAttackRange: query.GetAttackRange(owner.Seat),
            CurrentMaxHp: owner.MaxHp,
            CurrentAvailableEquipmentSlotCount: query.UsesStrategicTriggerValue(SkillProgramTriggerValueKind.CurrentAvailableEquipmentSlotCount) ? Enum.GetValues<EquipmentSlot>().Count(slot => owner.EquipmentSlotCapacity(slot) > 0) : null,
            CurrentHandCount: zones.Count(CardLocation.Hand(owner.Seat)),
            TurnDiscardSuitMask: query.TracksTurnDiscardSuits ? query.CurrentTurnDiscardSuitMask : null,
            LivingPlayersMinHp: livingHp.Value,
            TurnOwnerDiscardPhaseHandDiscardCount: query.TurnOwnerDiscardPhaseHandDiscardCount,
            DiscardPhaseSuitsAllDistinct: owner.Seat == turn.CurrentSeat && query.FullDiscardPhaseSuitsAllDistinct,
            LordGeneralId: query.Players.SingleOrDefault(player => player.Role == Role.Lord)?.General.Id,
            OwnedZoneCounts: new SkillProgramOwnedZoneCounts(
                zones.Count(CardLocation.WoodenOxGrain(owner.Seat)),
                zones.Count(CardLocation.BuquWound(owner.Seat)),
                zones.Count(CardLocation.Authority(owner.Seat)),
                zones.Count(CardLocation.Chunlao(owner.Seat)),
                zones.Count(new CardLocation(CardZoneKind.PublicPersistentPile, owner.Seat))),
            BooleanStates: states,
            PublicPersistentPileCounts: query.CapturePublicPileProgramCounts(owner),
            PlayPhaseKillCountByTurnOwner: query.PlayPhaseKillCount,
            PlayPhaseDamageDealtByTurnOwner: query.PlayPhaseDamageDealt,
            MarkerCounts: owner.Markers.Count > 0
                ? new Dictionary<PlayerMarkerKind, int>(owner.Markers)
                : null,
            OwnerIsFaceDown: owner.IsFaceDown,
            UnfulfilledPhaseColorRestrictionInstances: query.UnfulfilledPlayPhaseColorRestrictions(owner.Seat),
            OwnerKilledThisTurn: content.UsesTriggerCondition(SkillProgramTriggerConditionKind.OwnerKilledThisTurn)
                ? history.SinceLastBoundary(item => item is TurnStartedEvent).OfType<PlayerDiedEvent>()
                    .Any(item => item.KillerSeat == owner.Seat) : null,
            LivingWoundedCount: query.UsesStrategicTriggerValue(SkillProgramTriggerValueKind.LivingWoundedCount)
                ? query.Players.Count(player => player.IsAlive && query.IsRuleWounded(player)) : 0,
            PlayPhaseDamageTakenByAny: query.UsesStrategicTriggerValue(SkillProgramTriggerValueKind.PlayPhaseDamageTakenByAny)
                ? query.CountPlayPhaseDamageTakenByAny() : 0,
            LowHandPopulationSeats: content.HasTriggerOperation(SkillProgramEffectOp.DrawAndDraftLowHandPopulation)
                ? query.Players.Where(player => player.IsAlive && player.HasHpValue &&
                    (query.UnambiguousHp(player) is { } hp ? zones.Count(CardLocation.Hand(player.Seat)) < hp :
                        player.HappyQiaoHealth is { } health &&
                        (zones.Count(CardLocation.Hand(player.Seat)) < health.DaQiaoHp || zones.Count(CardLocation.Hand(player.Seat)) < health.XiaoQiaoHp)))
                    .Select(player => player.Seat).ToArray() : null,
            GlobalMarkerCounts: query.UsesStrategicTriggerValue(SkillProgramTriggerValueKind.GlobalMarkerCount)
                ? Enum.GetValues<PlayerMarkerKind>().Where(marker => marker is PlayerMarkerKind.Camp or PlayerMarkerKind.Junlue).ToDictionary(marker => marker,
                    marker => query.Players.Where(player => player.IsAlive).Sum(player => player.Markers.GetValueOrDefault(marker))) : null)
        { OwnerHpUndefined = !owner.HasHpValue || ownerDual is not null, OwnerRuleWounded = query.UsesTreatedWounded ? query.IsRuleWounded(owner) : null,
          TurnOwnerHpUndefined = !turnOwner.HasHpValue || turnOwnerDual is not null, LivingMinHpUndefined = livingHp.Undefined,
          UnselectedOwnerHp = ownerDual, UnselectedTurnOwnerHp = turnOwnerDual,
          UnselectedLivingHp = livingHp.Unselected, LivingFixedHpMinimum = livingHp.FixedMinimum,
          TurnOwnerHp = content.UsesTriggerValue(SkillProgramTriggerValueKind.TurnOwnerHp)
              ? turnOwnerHp : null,
          RoundTurnSeatCount = content.UsesTriggerValue(SkillProgramTriggerValueKind.RoundTurnSeatCount)
              ? query.RoundTurnSeatCount : null,
          ActualTurnDeathCount = turnDeaths?.Deaths,
          OwnerActualTurnKillCount = turnDeaths?.Kills,
          LivingPlayerCount = content.UsesTriggerValue(SkillProgramTriggerValueKind.LivingPlayerCount)
              ? query.Players.Count(player => player.IsAlive) : null,
          OwnerTrickUsesThisActualTurn = query.TracksActualTurnTrickUses ? query.ActualTurnTrickUseCount(owner.Seat) : null,
          PhaseIsPlay = content.UsesTriggerCondition(SkillProgramTriggerConditionKind.PhaseIsPlay)
              ? phase.Phase == TurnPhase.Play : null,
          FengjiRoundChoicePending = content.UsesTriggerCondition(SkillProgramTriggerConditionKind.FengjiRoundChoicePending)
              ? query.IsFengjiRoundChoicePending(owner.Seat) : null,
          FengjiSwapAvailable = content.UsesTriggerCondition(SkillProgramTriggerConditionKind.FengjiSwapAvailable)
              ? query.IsFengjiSwapAvailable(owner.Seat) : null,
          CurrentTurnUsedCardCategoryCount = query.TracksCurrentTurnUseKinds ? query.CurrentTurnUsedCategoryCount(owner.Seat) : null,
          CurrentActualPlayPhysicalSlashLossCount = query.TracksActualPlaySlashLoss ? query.CurrentActualPlaySlashLossCount(owner.Seat) : null,
          TurnOwnerUsedSameSuitCards = content.UsesTriggerCondition(SkillProgramTriggerConditionKind.TurnOwnerUsedSameSuitCards)
              ? query.CurrentTurnOwnerUsedSameSuitCards() : null,
          CurrentTurnNonConvertedUseCount = content.UsesTriggerValue(SkillProgramTriggerValueKind.CurrentTurnNonConvertedUseCount)
              ? query.CountNonConvertedActualUsesThisTurn(owner.Seat) : null };
    }

    internal SkillProgramTriggerFacts Capture<TQuery, TAction>(TQuery query, TAction actionFacts,
        CharacterState owner, CardActionContext action)
        where TQuery : struct, IProgramTriggerFactQueries
        where TAction : struct, IProgramTriggerActionFactQueries =>
        Capture(query, owner) with
        {
            ActualTurnActorUseOrdinal = actionFacts.TracksActualTurnUseOrdinal ? actionFacts.CapturedActualTurnActorUseOrdinal(action) : null,
            CardActionActorIsCurrentTurn = action.ActorSeat == turn.CurrentSeat,
            CardActionActorIsOwner = action.ActorSeat == owner.Seat,
            CardActionOpponentIsOwner = content.UsesTriggerCondition(SkillProgramTriggerConditionKind.CardActionOpponentIsOwner)
                ? action.OpponentSeat == owner.Seat : null,
            CardActionPhaseIsPlay = phase.Phase == TurnPhase.Play,
            CardActionSuit = content.UsesTriggerCondition(SkillProgramTriggerConditionKind.CardActionSuitIs)
                ? action.EffectiveSuit : null,
            CardActionHandCardCount = content.UsesTriggerValue(SkillProgramTriggerValueKind.CardActionHandCardCount)
                ? action.PhysicalCards.Count(cost => cost.From == CardLocation.Hand(owner.Seat)) : null,
            CardActionPhysicalCardCount = content.UsesTriggerValue(SkillProgramTriggerValueKind.CardActionPhysicalCardCount)
                ? action.PhysicalCards.Count : null,
            CardActionCategory = RuntimeSkills.RuntimeRules_Core_ProgramCardPayments.GetProgramCardCategory(action.EffectiveKind),
            CardActionMatchesPreviousPlayCard = actionFacts.TracksPlayCardHistory ? actionFacts.MatchesPreviousPlayCard(action) : null,
            PreviousPlayCardIsBasic = actionFacts.TracksPlayCardHistory ? actionFacts.PreviousPlayCardIsBasic(action) : null,
            CardActionFromOwnerHand = action.PhysicalCards.Count > 0 &&
                action.PhysicalCards.All(cost =>
                    cost.From is { Zone: CardZoneKind.Hand, OwnerSeat: { } holder } &&
                    holder == action.ActorSeat),
            CardActionCardIsBlack = content.UsesTriggerCondition(SkillProgramTriggerConditionKind.CardActionCardIsBlack)
                ? action.EffectiveSuit is { } blackSuit
                    ? blackSuit is Suit.Spade or Suit.Club
                    : action.PhysicalCards.Count > 0 && action.PhysicalCards.All(cost =>
                        zones.CardsAt(zones.GetLocation(cost.CardId)).Single(card => card.Id == cost.CardId).Suit is Suit.Spade or Suit.Club)
                : null,
            CardActionCardIsRed = action.EffectiveSuit is { } frozenSuit
                ? RuntimeSkills.RuntimeRules_Core_CommonRules.IsRedSuit(frozenSuit)
                : action.PhysicalCards.Count > 0 &&
                action.PhysicalCards.All(cost =>
                    zones.CardsAt(zones.GetLocation(cost.CardId))
                        .Single(card => card.Id == cost.CardId).Suit is Suit.Heart or Suit.Diamond),
            CardUseDesignatedTargetCount = action.Type == CardActionType.Use
                ? action.EffectiveDesignatedTargetSeats.Distinct().Count()
                : 0,
            CardUseConversionSkillIds = Array.AsReadOnly(action.ConversionChain
                .Select(source => source.SkillId).Distinct(StringComparer.Ordinal).ToArray())
        };

}
