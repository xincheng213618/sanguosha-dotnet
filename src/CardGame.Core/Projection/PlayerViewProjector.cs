namespace CardGame.Core;

// Assembly and viewer permissions are independent of GameEngine. The scoped
// ports retain live legacy storage reads until their owning rules move out;
// this service retains no match state or engine-wide host.
[ProgramRuntimeModule]
internal sealed class PlayerViewProjector(ICardZoneQueries zones, IProgramCardSnapshotQueries snapshots,
    IHandCardViewQueries handCards, IProgramContentQueries content, IActivePlayerEffectProjection activeEffects)
{
    /// <summary>Build a viewer-safe snapshot; revealAll is an explicit diagnostic/post-game choice.</summary>
    internal GameSnapshot Project<TVisibility, TCards, TRules, TEffects, TMatch>(
        TVisibility visibility, TCards cards, TRules rules, TEffects effectFacts, TMatch match,
        int viewerSeat, bool revealAll)
        where TVisibility : struct, IPlayerViewVisibilityQueries
        where TCards : struct, IPlayerViewCardsQueries
        where TRules : struct, IPlayerViewRulesQueries
        where TEffects : struct, IActivePlayerEffectQueries
        where TMatch : struct, IPlayerViewMatchQueries
    {
        var playerSnapshots = new PlayerSnapshot[match.PlayerCount];
        for (var playerIndex = 0; playerIndex < match.PlayerCount; playerIndex++)
        {
            var player = match.PlayerAt(playerIndex);
            var playerHand = zones.CardsAt(CardLocation.Hand(player.Seat));
            var canSeeRole = !visibility.Rules.ConcealsRoleCompletely &&
                             (revealAll ||
                              visibility.Rules.ShowsPublicTeams ||
                              player.Role == Role.Lord ||
                              player.RoleRevealed ||
                              player.Seat == viewerSeat);
            var canSeeHand = revealAll || player.Seat == viewerSeat;
            var sharedHandAccess = visibility.HasShiLsHandView(viewerSeat, player.Seat);
            var hand = canSeeHand || sharedHandAccess || visibility.HasShanheSourceZhaoranPublicHand(player.Seat)
                ? playerHand.Select(card => handCards.Project(player, card)).ToArray()
                : [];
            var playerEquipment = zones.CardsAt(CardLocation.Equipment(player.Seat));
            var equipment = playerEquipment
                .Select(card => snapshots.EquipmentSnapshot(card, viewerSeat, revealAll))
                .ToArray();
            var judgment = zones.CardsAt(CardLocation.Judgment(player.Seat))
                .Select(snapshots.JudgmentSnapshot)
                .ToArray();
            var woodenOxGrain = zones.CardsAt(CardLocation.WoodenOxGrain(player.Seat));
            var buquWounds = zones.CardsAt(CardLocation.BuquWound(player.Seat));
            var authority = zones.CardsAt(CardLocation.Authority(player.Seat));
            var chunlao = zones.CardsAt(CardLocation.Chunlao(player.Seat));
            var pojunHold = zones.CardsAt(CardLocation.PojunHold(player.Seat));
            var canSeeGeneral = player.GeneralSelected &&
                                (revealAll || player.GeneralRevealed || player.Seat == viewerSeat);
            var general = canSeeGeneral ? player.General : RuntimeSkills.RuntimeRules_Core_CommonRules.CreateHiddenGeneral();
            if (player.IdentityConcealed && !revealAll && player.Seat != viewerSeat)
                general = RuntimeSkills.RuntimeRules_Core_IdentityConcealment.IdentityConcealedGeneral();
            var canSeeSecondaryGeneral = player.SecondaryGeneralSelected &&
                                         (revealAll ||
                                          player.SecondaryGeneralRevealed ||
                                          player.Seat == viewerSeat);
            var secondaryGeneral = canSeeSecondaryGeneral ? player.SecondaryGeneral : null;
            var effectiveFactionId = visibility.GetEffectiveFactionId(player);
            var hasIdentityGodFaction = visibility.SupportsGodFactionSelection &&
                                        string.Equals(player.General.FactionId, "god", StringComparison.Ordinal) &&
                                        player.ChosenFactionId is not null;
            var hasScenarioGodFaction = visibility.GetScenarioGodFaction(player) is not null;
            var hasActivityFaction = visibility.Rules.PublishesActivityFaction;
            var canSeeFaction = effectiveFactionId is not null &&
                                (visibility.Rules.UsesHiddenFaction
                                    ? revealAll || player.FactionRevealed || player.Seat == viewerSeat
                                    : (hasIdentityGodFaction || hasScenarioGodFaction || hasActivityFaction || visibility.HasNativeEffectiveFactionDeclaration(player) || visibility.GetPrivateGeneralLibraryFaction(player) is not null) &&
                                      (revealAll || player.GeneralRevealed || player.Seat == viewerSeat));
            var primarySkills = canSeeGeneral
                ? player.IdentityConcealed ? general.Skills.ToArray()
                    : rules.OwnedRuntimeSkills(player, general, includeAcquired: true).ToArray() : null;
            primarySkills = primarySkills?.Where(skill => visibility.IsSkillVisible(player.Seat, skill.ContentId, viewerSeat, revealAll)).ToArray();
            var secondarySkills = secondaryGeneral is not null
                ? rules.OwnedRuntimeSkills(player, secondaryGeneral, includeAcquired: false).ToArray() : null;
            var activeWushuangSkillIds = visibility.IsWushuangMode ? rules.GetSkillBindingShard(player).ProgramInstances
                .Select(instance => instance.SkillId).ToHashSet(StringComparer.Ordinal) : null;
            var publicPile = cards.SinglePublicPileSource(player.Seat);
            var publicPileCards = publicPile is not null ? cards.PublicPileCards(publicPile) : Array.Empty<Card>();
            var deferredPileCards = zones.CardsAt(new CardLocation(CardZoneKind.PublicDeferredPile, player.Seat));
            var reserveCards = zones.CardsAt(new CardLocation(CardZoneKind.PrivateReserve, player.Seat));
            playerSnapshots[playerIndex] = new PlayerSnapshot(
                player.Seat,
                player.Name,
                player.IsHuman,
                canSeeRole ? player.Role : null,
                !visibility.Rules.ConcealsRoleCompletely && (player.RoleRevealed || player.Role == Role.Lord),
                general.Id,
                general.Name,
                general.PortraitKey,
                player.HasHpValue && player.HappyQiaoHealth is null ? player.Hp : 0,
                player.HasHpValue && player.HappyQiaoHealth is null ? player.MaxHp : 0,
                player.IsAlive,
                playerHand.Count,
                hand,
                player.GeneralSelected && player.GeneralRevealed,
                player.HasAlcoholEffect)
            {
                ViewerCanUseHand = sharedHandAccess,
                IdentityConcealed = player.IdentityConcealed,
                HappyQiaoHealth = player.HasHpValue ? player.HappyQiaoHealth : null,
                WushuangBuild = visibility.IsWushuangMode && (primarySkills is not null || secondarySkills is not null)
                    ? WushuangBuildProfile.Read((primarySkills ?? []).Concat(secondarySkills ?? [])
                        .Select(skill => skill.ContentId).OfType<string>().Distinct(StringComparer.Ordinal)
                        .Where(id => activeWushuangSkillIds!.Contains(id))
                        .Select(id => content.FindSkill(id)?.Program).OfType<SkillProgram>())
                    : null,
                Armor = player.Armor,
                IncomingDamageCap = rules.GetIncomingFixedDamageCap(player,
                    (primarySkills ?? []).Concat(secondarySkills ?? []).Select(skill => skill.ContentId)
                        .OfType<string>().ToHashSet(StringComparer.Ordinal)),
                HpUndefined = !player.HasHpValue || player.HappyQiaoHealth is not null,
                HandLimit = match.SetupComplete && player.Seat == match.CurrentSeat && player.HasHpValue
                    ? rules.GetHandLimit(player) : null,
                AlcoholDamageBonus = player.AlcoholDamageBonus >= 2 ? player.AlcoholDamageBonus : null,
                IsFaceDown = player.IsFaceDown,
                ActiveEffects = activeEffects.Project(effectFacts, match, player, primarySkills, secondarySkills),
                OriginalHandEntities = rules.ProjectOriginalHandEntities(player.Seat, viewerSeat, revealAll),
                TeamId = visibility.Rules.ShowsPublicTeams && player.TeamRevealed ? player.TeamId : null,
                IsTeamRevealed = visibility.Rules.ShowsPublicTeams && player.TeamRevealed,
                Equipment = equipment,
                IsEquipmentAreaAbolished = player.EquipmentAreaAbolished,
                EquipmentSlotCapacities = player.EquipmentSlotCapacities.Count > 0
                    ? new Dictionary<EquipmentSlot, int>(player.EquipmentSlotCapacities) : null,
                ConfiguredConversionTiers = rules.GetConfiguredConversionTierSnapshot(player),
                AlternatingChoiceStates = rules.GetAlternatingChoiceStateSnapshot(player),
                BeneficiarySuitShields = rules.BeneficiarySuitShields.Count > 0 &&
                    rules.BeneficiarySuitShields.Where(s => s.BeneficiarySeat == player.Seat).ToArray() is { Length: > 0 } shields ? shields : null,
                DeferredHandAlignments = rules.DeferredHandAlignments.Count > 0 &&
                    rules.DeferredHandAlignments.Where(d => d.TargetSeat == player.Seat).ToArray() is { Length: > 0 } alignments ? alignments : null,
                TurnHandCategoryRestrictions = rules.GetTurnHandCategoryRestrictionSnapshot(player.Seat),
                MouCaoPiControls = rules.MouCaoPiControlsSnapshot(player.Seat),
                TurnHandLimitCardKindExemptions = rules.GetTurnHandLimitCardKindExemptionsSnapshot(player.Seat),
                TurnSlashSuitAllowances = rules.GetTurnSlashSuitAllowancesSnapshot(player.Seat),
                FirstRoundGameUsageRefunds = rules.GetFirstRoundGameUsageRefundsSnapshot(player.Seat),
                JudgedRankSlashThreshold = rules.JudgedRankSlashSnapshot(player.Seat),
                ActualPlayPhaseCardUseState = rules.TracksActualPlayPhaseCardUses && match.Phase == TurnPhase.Play && player.Seat == match.CurrentSeat ? new(player.Seat, match.TurnNumber, match.CardUseDebitPhaseInstanceId, rules.GetActualPlayPhaseUseCount(player.Seat)) : null,
                IssuedPlayPhaseUseProhibitions = rules.IssuedPlayPhaseUseProhibitions.Count > 0 && rules.HasIssuedPlayPhaseUseBan(player.Seat) &&
                    rules.IssuedPlayPhaseUseProhibitions.Where(p => p.ActorSeat == player.Seat).ToArray() is { Length: > 0 } issuedBans ? issuedBans : null,
                IssuedPlayPhaseSuitUseAllowances = rules.GetIssuedPlayPhaseSuitUseAllowances(player.Seat),
                WoodenOxGrainCount = woodenOxGrain.Count,
                WoodenOxGrain = playerEquipment.Any(card => card.Kind == CardKind.WoodenOx) || woodenOxGrain.Count > 0
                    ? canSeeHand
                        ? woodenOxGrain.Select(card => cards.ProjectFuleHandCard(player, card, snapshots.Snapshot(card))).ToArray()
                        : Array.Empty<CardSnapshot>()
                    : null,
                IsChained = player.IsChained,
                Judgment = judgment,
                IsJudgmentAreaAbolished = player.JudgmentAreaAbolished,
                BuquWounds = rules.HasProgramSkill(player, "classic:buqu") || buquWounds.Count > 0
                    ? buquWounds.Select(snapshots.Snapshot).ToArray()
                    : null,
                AuthorityCount = authority.Count,
                PublicPersistentPileCards = publicPileCards.Count > 0 ? publicPileCards.Select(snapshots.Snapshot).ToArray() : null,
                PublicPersistentPileCount = publicPileCards.Count,
                PublicPersistentPileName = publicPile is not null ? content.Skill(publicPile.SkillId).ProgramPresentation?.AuthorityName : null,
                PublicPersistentPileSkillId = publicPile?.SkillId,
                SourceCurses = rules.CreateSourceCurseSnapshots(player.Seat),
                PublicPersistentPiles = rules.CreatePublicPersistentPileSnapshots(player.Seat),
                PrivateGeneralLibraries = rules.ProjectPrivateGeneralLibraries(player,viewerSeat,revealAll),
                PrivateTurnHolds = rules.ProjectPrivateTurnHolds(player.Seat, viewerSeat, revealAll),
                NamedAmbushPiles = rules.ViewContributors.NamedAmbushPiles(rules.ProjectNamedAmbushPiles(player.Seat, viewerSeat, revealAll), player.Seat, viewerSeat, revealAll),
                PrivateWhirlPiles = rules.ViewContributors.PrivateWhirlPiles(rules.ProjectPrivateWhirlPiles(player.Seat, viewerSeat, revealAll), player.Seat, viewerSeat, revealAll),
                PublicDeferredPileName = rules.DeferredPublicPileDeposits.Count > 0 &&
                    rules.DeferredPublicPileDeposits.FirstOrDefault(item => item.OwnerSeat == player.Seat) is { } deposit ? content.Skill(deposit.SkillId).ProgramPresentation?.AuthorityName : null,
                PublicDeferredPileCount = deferredPileCards.Count,
                PublicDeferredPileCards = deferredPileCards.Count > 0 ? deferredPileCards.Select(snapshots.Snapshot).ToArray() : null,
                AuthorityName = handCards.AuthorityName(primarySkills),
                PrivateReserveCount = reserveCards.Count,
                PrivateReserveCards = canSeeHand && (rules.HasProgramPersistentZone(player, CardZoneKind.PrivateReserve) ||
                    reserveCards.Count > 0)
                    ? reserveCards.Select(snapshots.Snapshot).ToArray() : null,
                AuthorityCards = authority.Select(snapshots.Snapshot).ToArray(),
                ChunlaoCount = chunlao.Count,
                ChunlaoCards = rules.HasProgramPersistentZone(player, CardZoneKind.Chunlao) || chunlao.Count > 0
                    ? chunlao.Select(snapshots.Snapshot).ToArray()
                    : null,
                RoundHoldCount = zones.CardsAt(CardLocation.RoundHold(player.Seat)).Count,
                PojunHoldCount = pojunHold.Count,
                PojunHoldCards = pojunHold.Count > 0
                    ? pojunHold.Select(snapshots.Snapshot).ToArray()
                    : null,
                Markers = rules.AppendOriginalUseLightningMarker(player.Seat, rules.AppendMouGuoHuaiJingce(player.Seat, rules.AppendMouGanNingWei(player.Seat, rules.AppendMouHuaXiongWei(player.Seat, rules.AppendMouLixianProgressMarkers(player, canSeeGeneral, rules.AppendMoQuanyuPublicMarkers(player.Seat, rules.AppendOlMoRumoMarker(player.Seat, player.Markers.Count > 0 || player.Shanjia is { LossMarks: > 0 }
                    ? player.Markers.Where(marker => marker.Key != PlayerMarkerKind.ShanjiaLoss)
                        .Concat(player.Shanjia is { LossMarks: > 0 } shanjia
                            ? [new KeyValuePair<PlayerMarkerKind, int>(PlayerMarkerKind.ShanjiaLoss, shanjia.LossMarks)]
                            : Array.Empty<KeyValuePair<PlayerMarkerKind, int>>())
                        .OrderBy(marker => marker.Key)
                        .Select(marker => new PlayerMarkerSnapshot(
                            marker.Key,
                            PlayerMarkerCatalog.GetDisplayName(marker.Key),
                            marker.Value))
                        .ToArray()
                    : null))))))),
                FactionId = canSeeFaction ? effectiveFactionId : null,
                IsFactionRevealed = visibility.Rules.UsesHiddenFaction
                    ? player.FactionRevealed
                    : (hasIdentityGodFaction || hasScenarioGodFaction || hasActivityFaction || visibility.HasNativeEffectiveFactionDeclaration(player) || visibility.GetPrivateGeneralLibraryFaction(player) is not null) && player.GeneralRevealed,
                SecondaryGeneralId = secondaryGeneral?.Id,
                SecondaryGeneralName = secondaryGeneral?.Name,
                SecondaryPortraitKey = secondaryGeneral?.PortraitKey,
                IsSecondaryGeneralPublic = visibility.Rules.UsesSecondaryGeneral && player.SecondaryGeneralRevealed,
                Skills = primarySkills,
                SecondarySkills = secondarySkills,
                SkillRuntimeStates = primarySkills is not null
                    ? primarySkills.Concat(secondarySkills ?? [])
                        .Select(skill => skill.ContentId)
                        .OfType<string>()
                        .Distinct(StringComparer.Ordinal)
                        .Select(skillId => rules.CreateProgramAwareSkillStateSnapshot(player, skillId))
                        .ToArray()
                    : null
            };
            playerSnapshots[playerIndex] = rules.ProjectPlayer(playerSnapshots[playerIndex], viewerSeat, revealAll);
        }

        var visibleDecision = rules.GetPendingDecisionForActor(viewerSeat);
        if (visibleDecision is not null) visibleDecision = rules.ProjectVisibleTargetHandDecision(visibleDecision, viewerSeat);
        // Publish only the responder's remaining selection bounds, including restored
        // older prompts. The private runtime draft stays on its owning frame.
        if (visibleDecision is { Kind: DecisionKind.ProgramTrigger } &&
            match.LastFrame is ProgramSkillFrame { OwnedCardSelection: { } owned } &&
            visibleDecision.Choices.All(choice => choice.Parameters.GetValueOrDefault("program-action") is "select-owned-cards" or "finish-owned-cards"))
            visibleDecision = visibleDecision with
            {
                RequiredCardCount = owned.RequiredCount - owned.SelectedCardIds.Count,
                MinimumCardCount = owned.AllowEarlyFinish ? Math.Max(0, owned.MinimumCount - owned.SelectedCardIds.Count) : null
            };
        if (visibleDecision is not null) visibleDecision = rules.ProjectCardPaymentCount(visibleDecision);
        if (visibleDecision is not null) visibleDecision = rules.ProjectNativeCardSelections(visibleDecision);
        ProgramSkillFrame? lastProgram = null;
        for (var index = match.FrameCount - 1; index >= 0; index--)
            if (match.FrameAt(index) is ProgramSkillFrame program) { lastProgram = program; break; }
        IReadOnlyList<CardSnapshot> programRevealedCards = lastProgram is null
            ? Array.Empty<CardSnapshot>()
            : rules.ViewContributors.Cards(ProgramViewContribution.ProgramPublicCards).DistinctBy(card => card.Id).ToArray();
        programRevealedCards = programRevealedCards.Concat(match.Frames.OfType<ProgramSkillFrame>()
            .Where(f => f.ExtraEffectsAndRankGuess?.Operation == SkillProgramEffectOp.GuessRanksForDrawReplacement)
            .SelectMany(f => f.ExtraEffectsAndRankGuess!.RevealedCardIds)
            .Where(id => zones.GetLocation(id) == CardLocation.Processing).Select(id => snapshots.Snapshot(zones.CardsAt(zones.GetLocation(id)).Single(card => card.Id == id))))
            .DistinctBy(card => card.Id).ToArray();
        if (lastProgram?.PublicPileDraft is { Stage: "pile" or "distribute" } pileDraft)
            programRevealedCards = programRevealedCards.Concat(cards.PublicPileDraftCards(pileDraft).Select(snapshots.Snapshot)).DistinctBy(card => card.Id).ToArray();
        if (match.LastFrame is ProgramSkillFrame { InstructionIndex: > 0 } pileFrame)
        {
            var effect = ProgramInstructionResolver.Default.Resolve(pileFrame,
                content.Skill(pileFrame.SkillId).Program!).GetPausedInstruction(pileFrame.InstructionIndex).Effect;
            if (effect.Op == SkillProgramEffectOp.ObtainPublicPileCard)
                programRevealedCards = programRevealedCards.Concat(cards.ReferencedPublicPileSources(pileFrame.OwnerSeat,
                    effect.SkillIds.Single(), pileFrame.SkillInstanceId).SelectMany(cards.PublicPileCards).Select(snapshots.Snapshot))
                    .DistinctBy(card => card.Id).ToArray();
        }
        IReadOnlyList<CardSnapshot> publicRevealedCards = match.LastFrame is PindianFrame { Result: { } contest }
            ? zones.CardsAt(CardLocation.Processing)
                .Where(card => card.Id == contest.SourceCardId || card.Id == contest.OpponentCardId)
                .Select(snapshots.Snapshot).ToArray()
            : programRevealedCards.Count > 0
            ? programRevealedCards
            : match.ActiveGroupCard is { Effect: GroupCardEffect.PublicDraft } publicDraft
            ? publicDraft.RevealedCardIds
                .Select(cardId => zones.CardsAt(CardLocation.Processing)
                    .Single(card => card.Id == cardId))
                .Select(snapshots.Snapshot)
                .ToArray()
            : match.ActiveFireAttack is { FireAttackSelection.RevealedCardId: not null } fireAttack
                ? [snapshots.Snapshot(cards.GetFireAttackRevealedCard(fireAttack))]
                : Array.Empty<CardSnapshot>();

        if (lastProgram?.AlternatingSuitTop is { Mode: "completed", Stage: "order" } orderedCosts)
            publicRevealedCards = publicRevealedCards.Concat(orderedCosts.CardIds.Except(orderedCosts.SelectedIds).Select(id => snapshots.Snapshot(zones.CardsAt(zones.GetLocation(id)).Single(c=>c.Id==id)))).DistinctBy(c=>c.Id).ToArray();

        return PlayerViewFreezer.Freeze(rules.ProjectGame(new GameSnapshot(
            revealAll ? match.Options.Seed : null,
            match.Options.HumanSeats is null ? match.Options.HumanSeat : viewerSeat,
            match.Status,
            match.Winner,
            match.TurnNumber,
            match.CurrentSeat,
            match.Phase,
            zones.Count(CardLocation.DrawPile),
            zones.Count(CardLocation.DiscardPile),
            playerSnapshots,
            visibleDecision,
            rules.ViewContributors.ProcessingCount(zones.CardsAt(CardLocation.Processing), revealAll),
            match.Revision)
        {
            CardDeclarations = rules.GetCardDeclarationSnapshots(viewerSeat, revealAll),
            HarvestPickerSeat = match.ActiveGroupCard is { Effect: GroupCardEffect.PublicDraft } harvest &&
                harvest.TargetIndex < harvest.TargetSeats.Count && harvest.RevealedCardIds.Count > 0
                ? harvest.TargetSeats[harvest.TargetIndex] : null,
            TableOrder = match.CurrentTableOrder(),
            BenevolenceCards = zones.CardsAt(CardLocation.Benevolence) is { Count: > 0 } benevolence
                ? benevolence.Select(snapshots.Snapshot).ToArray() : null,
            WinnerSeat = match.WinnerSeat,
            WinnerTeamId = visibility.Rules.ShowsPublicTeams ? match.WinnerTeamId : null,
            WinnerFactionId = visibility.Rules.UsesHiddenFaction ? match.WinnerFactionId : null,
            ModeKind = match.ModeDefinition.ModeKind,
            IdentityComposition = visibility.Rules.PublishesIdentityComposition
                ? new PublicIdentityComposition(
                    match.ModeDefinition.RoleCounts.GetValueOrDefault(nameof(Role.Lord)),
                    match.ModeDefinition.RoleCounts.GetValueOrDefault(nameof(Role.Loyalist)),
                    match.ModeDefinition.RoleCounts.GetValueOrDefault(nameof(Role.Rebel)),
                    match.ModeDefinition.RoleCounts.GetValueOrDefault(nameof(Role.Renegade))) : null,
            PublicRevealedCards = rules.ViewContributors.Cards(ProgramViewContribution.PublicCardsBeforeResolution).Concat(publicRevealedCards)
                .Concat(rules.ViewContributors.Cards(ProgramViewContribution.PublicCardsAfterResolution))
                .Where(card => rules.ViewContributors.IsPublicCardVisible(card.Id, revealAll))
                .DistinctBy(card => card.Id).ToArray(),
            PrivateRevealedCards = rules.ViewContributors.Cards(ProgramViewContribution.PrivateCards, viewerSeat)
                .DistinctBy(card => card.Id).ToArray() is { Length: > 0 } privateCards ? privateCards : null,
            FriendPredictions = rules.GetFriendPredictionViews(viewerSeat),
            BoundarySidiPredictions = rules.GetBoundarySidiPredictionViews(viewerSeat),
            ProgramResponseExchangeStates = rules.GetResponseExchangeStateSnapshots(),
            TurnProhibitedPhysicalCards = rules.GetResponseEntityRestrictionSnapshots()
        }, viewerSeat, revealAll));
    }
}
