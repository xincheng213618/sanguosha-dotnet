using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceGarrisonChecks
{

    public static void NativePreviewMoveAndStartKeepTheChosenRosterAndPhysicalOpening()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceDefinition(25, includeCodex: false);
        foreach (var chapter in new[] { 1, 2, 3 })
        foreach (var kind in new[] { ShanheNodeKind.Battle, ShanheNodeKind.EliteBattle })
        {
            // Admit a real later entry without manufacturing wins. Keep the
            // original starts and graph, including sibling elite references.
            var node = source.Nodes.First(candidate => candidate.ChapterNumber == chapter && candidate.Kind == kind &&
                candidate.Encounters!.Any(encounter => encounter.SourceBattleId is null));
            var entry = source with
            {
                StartingNodeIds = source.StartingNodeIds.Append(node.Id).Distinct(StringComparer.Ordinal).ToArray(), RandomStartingNode = false,
                Nodes = source.Nodes.Select(candidate => candidate.Id == node.Id
                    ? candidate with { Encounters = candidate.Encounters!.Where(encounter => encounter.SourceBattleId is null).ToArray() }
                    : candidate).ToArray()
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
