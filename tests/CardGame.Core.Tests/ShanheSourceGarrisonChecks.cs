using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceGarrisonChecks
{
    public static void ThreeChaptersOfferDifferentLocalRostersAndARealEliteBranch()
    {
        foreach (var difficulty in new[] { 1, 25 })
        {
            var definition = StandardShanheCampaignCatalog.CreateSourceDefinition(difficulty);
            var graph = StandardShanheSourceRouteCatalog.Season5;
            foreach (var chapter in new[] { 1, 2, 3 })
            {
                var ordinary = definition.Nodes.Where(node => node.ChapterNumber == chapter && node.Kind == ShanheNodeKind.Battle).ToArray();
                var local = ordinary.SelectMany(node => node.Encounters!).Where(encounter => encounter.SourceBattleId is null).ToArray();
                var signatures = local.Select(encounter => string.Join("|", encounter.Enemies.Select(enemy => enemy.GeneralId))).Distinct().ToArray();
                Need(signatures.Length >= 4 && local.All(encounter =>
                        encounter.Enemies.Count >= chapter && encounter.Enemies.Count <= chapter + 1 &&
                        encounter.SourceCityId is > 0 && encounter.BossTransition is null && encounter.VictoryGold == 200) &&
                    ordinary.All(node => node.Encounters!.Count(encounter => encounter.SourceBattleId is null) == 4 &&
                        node.Encounters.All(encounter => encounter.SourceCityId is { } city &&
                            node.Id == StandardShanheCampaignCatalog.Source2026CityNodeId(city))),
                    "Every actual ordinary city offers four different local lineups with chapter-appropriate enemy counts and keeps only its genuine city identity.");
            }
            foreach (var region in graph.Regions.Where(region => region.MinimumSourceDifficultyId <= difficulty))
            {
                var prefix = "source-region-" + region.SourceUniqueId;
                var market = definition.Nodes.Single(node => node.Id == prefix + ":market");
                var elite = definition.Nodes.Single(node => node.Id == prefix + ":local-elite");
                var expectedNext = prefix + (region.ChapterNumber == 3 ? ":story" : ":camp");
                Need(elite.Kind == ShanheNodeKind.EliteBattle && elite.ChapterNumber == region.ChapterNumber &&
                    market.NextNodeIds.Contains(elite.Id) && elite.NextNodeIds.SequenceEqual([expectedNext]) &&
                    elite.CategorizedRewards && elite.PrimaryTacticRarity == 2 && elite.PrimarySkillRarity == 2 &&
                    elite.Encounters is { Count: > 0 } && elite.Encounters.All(encounter =>
                        encounter.SourceCityId is null && encounter.SourceBattleId is null && encounter.BossTransition is null &&
                        encounter.Enemies.Count == region.ChapterNumber + 1 && encounter.VictoryGold == 200 &&
                        encounter.Name.Contains("本地", StringComparison.Ordinal)) &&
                    definition.Rewards.Any(reward => elite.RewardIds!.Contains(reward.Id) && reward.Rarity == 2 && reward.TacticSkillId is not null) &&
                    definition.Rewards.Any(reward => elite.RewardIds!.Contains(reward.Id) && reward.Rarity == 2 && reward.SkillId is not null),
                    "Every source region has a selectable local elite with real enemies, two rare reward categories and its original rejoin path; it claims no official city or battle.");
            }
            Need(definition.Nodes.Where(node => node.Kind == ShanheNodeKind.EliteBattle).Count() ==
                    graph.Regions.Count(region => region.MinimumSourceDifficultyId <= difficulty) &&
                definition.Nodes.SelectMany(node => node.Encounters ?? []).All(encounter =>
                    encounter.Enemies.Count + (encounter.Allies?.Count ?? 0) + 1 <= 8) &&
                definition.Nodes.Where(node => node.Kind == ShanheNodeKind.Battle)
                    .SelectMany(node => node.Encounters!).Count(encounter => encounter.SourceBattleId == 2397) == 7,
                "The optional branches keep the total-seat cap and preserve exactly the existing seven genuinely eligible Luoyang Mulu placements.");
        }
    }

    public static void NativePreviewMoveAndStartKeepTheChosenRosterAndPhysicalOpening()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceDefinition(25, includeCodex: false);
        foreach (var chapter in new[] { 1, 2, 3 })
        foreach (var kind in new[] { ShanheNodeKind.Battle, ShanheNodeKind.EliteBattle })
        {
            // A real node is made the entrypoint to inspect later chapters
            // without manufacturing wins or changing any of its candidates.
            var node = source.Nodes.First(candidate => candidate.ChapterNumber == chapter && candidate.Kind == kind &&
                candidate.Encounters!.All(encounter => encounter.SourceBattleId is null));
            var byId = source.Nodes.ToDictionary(candidate => candidate.Id, StringComparer.Ordinal);
            var reachable = new HashSet<string>(StringComparer.Ordinal);
            var remaining = new Stack<string>([node.Id]);
            while (remaining.TryPop(out var id))
            {
                if (!reachable.Add(id)) continue;
                var current = byId[id];
                foreach (var next in current.NextNodeIds.Concat((current.Choices ?? [])
                    .Concat((current.StoryEvents ?? []).SelectMany(story => story.Choices))
                    .Select(choice => choice.BattleNodeId).OfType<string>())) remaining.Push(next);
            }
            var entry = source with
            {
                StartingNodeIds = [node.Id], RandomStartingNode = false,
                Nodes = source.Nodes.Where(candidate => reachable.Contains(candidate.Id)).ToArray()
            };
            var session = ShanheCampaignSession.Create(entry, new(83, "classic:guan-yu")
                { SourceDifficultyId = source.SourceDifficultyId }, StandardShanheCampaignCatalog.CreateBattleRegistry);
            var revision = session.Revision;
            var preview = session.PreviewEncounter(node.Id);
            Need(session.Revision == revision && session.PreviewEncounter(node.Id).Id == preview.Id,
                "Previewing the actual ordinary or elite candidate consumes neither a map move nor its seeded selection.");
            var moved = session.Submit(new MoveShanheNodeCommand(node.Id, session.Revision));
            Need(moved.Accepted, moved.Error ?? "Cannot enter the actual garrison node.");
            var setup = session.PendingBattleSetup!;
            var selected = node.Encounters!.Single(encounter => encounter.Id == preview.Id);
            Need(setup.EnemyRoster.Select(enemy => enemy.GeneralId).SequenceEqual(preview.Enemies.Select(enemy => enemy.GeneralId)) &&
                session.Snapshot.CurrentNodeId == node.Id && selected.SourceCityId == preview.SourceCityId &&
                selected.SourceBattleId is null && setup.BossTransition is null &&
                (kind == ShanheNodeKind.EliteBattle ? selected.SourceCityId is null : selected.SourceCityId is > 0),
                "The move uses the exact previewed group and retains a real ordinary city or the elite's explicit absence of a source city.");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            var game = StandardShanheCampaignCatalog.CreateBattle(setup);
            Need(game.Submit(new StartGameCommand()).Accepted, "The real garrison starts through its native command.");
            // Fixed-general setup deals in construction; Start emits GameStarted
            // directly. SetupCompleted belongs to interactive general selection.
            for (var step = 0; step < 120 &&
                !(game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                  game.ResolutionStack.Count == 0); step++)
            {
                Need(game.PendingDecision is null && game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted,
                    $"The first native Play must arrive without answering another prompt; chapter={chapter}, kind={kind}, pending={game.PendingDecision?.Kind}, actor={game.PendingDecision?.PlayerSeat}.");
            }
            var view = game.CreateSnapshot(0);
            Need(view.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                game.ResolutionStack.Count == 0 && view.CurrentSeat == 0 && view.TurnNumber == 1 &&
                game.Events.Any(envelope => envelope.Payload is GameStartedEvent) &&
                game.Events.Where(envelope => envelope.Payload is TurnStartedEvent)
                    .All(envelope => ((TurnStartedEvent)envelope.Payload).ActorSeat == 0),
                "Inspect the fully initialized first human Play before any enemy's later turn or draw phase.");
            var deck = registry.Decks[registry.Modes[ShanheBattleSetup.BattleModeId].DeckId!];
            Need(view.Players.Count == 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count && view.Players.Count <= 8,
                "The native battle seats every admitted companion and every selected enemy within the eight-player limit.");
            for (var index = 0; index < setup.EnemyRoster.Count; index++)
            {
                var member = setup.EnemyRoster[index];
                var general = registry.Generals[member.GeneralId];
                var player = view.Players[1 + setup.AllyRoster.Count + index];
                var initialCards = (member.StartingHand?.Count ?? deck.InitialHandSize) + member.InitialHandBonus +
                    (member.StartingCards?.Count ?? 0);
                Need(player.GeneralId == member.GeneralId && player.MaxHp == general.BaseHp + member.MaxHpBonus &&
                    player.Hp == (member.Hp ?? player.MaxHp) && player.HandCount == initialCards,
                    "Each actual roster member loads its registered body, prepared health and complete physical opening hand.");
            }
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
}
