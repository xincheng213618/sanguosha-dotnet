using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceWuhuanRiderChecks
{
    private const string Deck = "fixture:source-wuhuan-rider-deck";
    private const string Tuxi = StandardShanheSourceWuhuanRiderPackage.TuxiSkillId;
    private const string Jixi = StandardShanheSourceWuhuanRiderPackage.JixiSkillId;

    public static void JixiPaysADelayedTrickEntityForOneNativeSnatchAndKeepsItsDistanceRules()
    {
        var s = Scene.Start(21);
        var material = s.V(0).Hand.Single(c => c.Kind == CardKind.Indulgence);
        var originalTargetCard = s.V(1).Hand.Single().Id;
        var handBefore = s.V(0).HandCount;
        var actions = s.G.GetHumanLegalActions().Where(a => a.CardId == material.Id &&
            a.PlayedCardKind == CardKind.Snatch && a.ConversionSource?.SkillId == Jixi).ToArray();
        Need(s.V(0).GeneralId == StandardShanheSourceWuhuanRiderPackage.HellId &&
            actions.Any(a => a.TargetSeats.SequenceEqual([1])) &&
            actions.All(a => !a.TargetSeats.Contains(0) && !a.TargetSeats.Contains(2)),
            "The source1600 hand-trick conversion includes the actual delayed trick, targets an adjacent other player, and cannot target self or the distance-two seat.");
        var action = actions.First(a => a.TargetSeats.SequenceEqual([1]) && a.TargetCardId is null);
        s.Accept(new PlayCardCommand(0, material.Id, action.TargetSeats, s.G.Revision, s.P!.PromptId,
            action.PlayedCardKind, action.TargetCardId)
        { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        s.Play();
        var declared = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == material.Id);
        Need(declared.CardKind == CardKind.Snatch && declared.SourceSeat == 0 &&
            s.Events<TargetCardTakenEvent>().Count(e => e.ResolutionId == declared.ResolutionId && e.TargetSeat == 1) == 1 &&
            s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == declared.ResolutionId && e.CardKind == CardKind.Snatch) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material.Id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material.Id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
            s.Zone(material.Id) == CardLocation.DiscardPile && s.Zone(originalTargetCard) == CardLocation.Hand(0) &&
            s.V(1).HandCount == 0 && s.V(0).HandCount == handBefore &&
            !s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == declared.ResolutionId),
            "The real delayed-trick material pays one Snatch declaration and finish, enters discard once, and takes the target's original hand entity without creating a delayed judgment or a second use.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.CreateSnapshot(0).Players.Count)
            .Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        private bool IsPlay => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0;
        public void Accept(GameCommand command)
        {
            var result = G.Submit(command);
            Need(result.Accepted, (result.Error?.Message ?? "Native Wuhuan command rejected.") + " " + Diagnostic());
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(predicate);
            Need(choice is not null, "Missing native Wuhuan choice. " + Diagnostic());
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Play() => Until(() => IsPlay);
        public void Until(Func<bool> stop)
        {
            for (var step = 0; step < 160; step++)
            {
                if (stop()) return;
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.Kind == DecisionKind.SelectTargetCard) { Answer(_ => true); continue; }
                    var pass = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip" ||
                        c.Parameters.GetValueOrDefault("response") == "pass");
                    Need(pass is not null, "Unexpected native Wuhuan boundary. " + Diagnostic());
                    Accept(new AnswerPromptCommand(0, p.PromptId, pass!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Bounded Wuhuan flow did not reach its original native boundary. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new
        {
            G.State.TurnNumber, G.State.Phase, G.State.CurrentSeat, G.State.Winner, Pending = P,
            Hands = Enumerable.Range(0, G.CreateSnapshot(0).Players.Count).Select(seat => new { Seat = seat, V(seat).HandCount }),
            Stack = G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }),
            Facts = G.Events.TakeLast(8).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType()))
        });
        public static Scene Start(int difficulty)
        {
            ShanheStartingCard[] playerCards = difficulty == 11
                ? Enumerable.Range(1, 4).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Spade, rank)).ToArray()
                : [new ShanheStartingCard(CardKind.Indulgence, Suit.Club, 7), new(CardKind.Dodge, Suit.Spade, 8)];
            var enemies = (difficulty == 11 ? new[] { "classic:ma-chao" } :
                new[] { "classic:ma-chao", "classic:guan-yu", "classic:zhang-fei" })
                .Select(id => new ShanheCombatantDefinition(id)
                    { StartingHand = [new(CardKind.Dodge, Suit.Spade, 2)] }).ToArray();
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-wuhuan-rider",
                StandardShanheSourceWuhuanRiderPackage.GetGeneralId(difficulty), 4, 0, 0, 0, 0,
                [], playerCards, [], enemies, [], "fixture:source-wuhuan-rider") { SourceDifficultyId = difficulty };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 1 + enemies.Length, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, registry));
            scene.Accept(new StartGameCommand());
            scene.Until(() => difficulty == 11
                ? scene.P?.SkillPrompt?.SkillId == Tuxi && scene.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate")
                : scene.IsPlay);
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-wuhuan-rider", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => builder.AddDeck(new(Deck, "真实固定闪实体", 0, 2, [])
        {
            PhysicalCards = Enumerable.Range(0, 64).Select(index =>
                new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, index % 13 + 1)).ToArray()
        });
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
