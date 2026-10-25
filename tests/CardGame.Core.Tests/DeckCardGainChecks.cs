using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class DeckCardGainChecks
{
    private const string Owner = "fixture:deck-gain-owner", Enemy = "fixture:deck-gain-enemy";
    private const string Driver = "fixture:deck-gain-driver", Watch = "fixture:deck-gain-watch", Quiet = "fixture:deck-gain-quiet";
    private const string Deck = "fixture:deck-gain-deck", Reason = "skill-program.obtain-deck-cards";
    private static readonly CardKind[] Slashes = [CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash];
    private static readonly CardKind[] Tricks = [CardKind.Duel, CardKind.IronChain, CardKind.Indulgence];

    public static void NativePileSelectionShortfallAndGainChildReturnKeepOneReceipt()
    {
        var scene = Scene.Start();
        var first = scene.Pile(Slashes).Take(2).ToArray();
        scene.Begin("two-slashes");
        scene.Until(() => scene.P?.SkillPrompt?.SkillId == Watch && scene.P.Choices.Any(choice =>
            choice.Parameters.GetValueOrDefault("option-id") == "continue"));
        var parent = scene.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.DeckCardGain is not null);
        var receipt = parent.DeckCardGain!;
        Need(receipt.CardIds.SequenceEqual(first.Select(card => card.CardId)) &&
            receipt.SelectedKinds.SequenceEqual(first.Select(card => card.CardKind)) && receipt.FilterKinds.SequenceEqual(Slashes) &&
            receipt.CardIds is IList<int> { IsReadOnly: true } && receipt.SelectedKinds is IList<CardKind> { IsReadOnly: true } &&
            receipt.FilterKinds is IList<CardKind> { IsReadOnly: true } && receipt.BatchId > 0 && receipt.SequenceAfter > receipt.SequenceBefore &&
            scene.G.CardMovements.Count(move => move.Reason.Value == Reason) == 2 && scene.V.Armor == 0,
            "In an ordinary Shanhe battle the new operation takes real matching pile entities from the top, freezes IDs, kinds and filter, and waits for their actual gain child before its next instruction.");
        var prompt = scene.P!;
        scene.Accept(new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, scene.G.Revision));
        scene.Play();
        Need(scene.V.HandCount == 3 && scene.V.Armor == 1 &&
            scene.G.CardMovements.Count(move => move.Reason.Value == Reason) == 2 &&
            scene.G.CardMovements.Count(move => move.Reason.Value == "skill-program." + Watch + ".Draw") == 1 &&
            scene.Events().Count(fact => fact.ProgramFrameId == parent.Id) == 1 &&
            scene.G.ResolutionStack.OfType<ProgramSkillFrame>().All(frame => frame.DeckCardGain is null),
            "The real gain child draws one different card, then returns to grant armor once without selecting or obtaining the parent's paid cards again.");

        var remaining = scene.Pile(Slashes).ToArray();
        Need(remaining.Length is > 0 and < 4, "The bounded physical deck has fewer than four remaining Slash-family entities.");
        var before = scene.G.CardMovements.Count;
        scene.Begin("four-slashes"); scene.Play();
        var scarce = scene.G.CardMovements.Skip(before).Where(move => move.Reason.Value == Reason).ToArray();
        Need(scarce.Select(move => move.CardId).SequenceEqual(remaining.Select(card => card.CardId)) &&
            scarce.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) && scene.Pile(Slashes).Length == 0,
            "An undersized kind pool yields exactly the existing matching entities in current top order, with no created card or replacement draw.");

        var tricks = scene.Pile(Tricks);
        before = scene.G.CardMovements.Count;
        scene.Begin("tricks"); scene.Play();
        var gained = scene.G.CardMovements.Skip(before).Where(move => move.Reason.Value == Reason).ToArray();
        Need(gained.Select(move => move.CardId).SequenceEqual(tricks.Select(card => card.CardId)) &&
            gained.All(move => Tricks.Contains(move.CardKind)) && scene.Pile(Tricks).Length == 0 &&
            scene.G.CardMovements.All(move => move.Reason != CardMoveReasons.Draw),
            "The category filter obtains the actual available ordinary and delayed tricks without taking basic cards or masquerading as normal draw.");
        var eventCount = scene.Events().Length;
        before = scene.G.CardMovements.Count;
        scene.Begin("tricks"); scene.Play();
        Need(scene.Events().Length == eventCount && scene.G.CardMovements.Count == before && scene.V.Armor == 4,
            "An exhausted matching category completes without issuing another movement or replaying a completed gain, while the next program instruction still runs once.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public DeckCardsObtainedEvent[] Events() => G.Events.Select(item => item.Payload).OfType<DeckCardsObtainedEvent>().ToArray();
        public CardZoneDiagnostic[] Pile(CardKind[] kinds) => G.CreateCardZoneDiagnostics()
            .Where(card => card.Location == CardLocation.DrawPile && kinds.Contains(card.CardKind))
            .OrderByDescending(card => card.ZoneIndex).ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected shared deck gain fixture command."); }
        public void Begin(string activation) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], [], G.Revision, P!.PromptId));
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 150; step++)
            {
                if (done()) return;
                Need(P?.PlayerSeat != 0 && G.State.Status != EngineStatus.Completed,
                    "Unexpected shared deck gain boundary: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded shared deck gain boundary: " + JsonSerializer.Serialize(P));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:deck-gain", Owner, 12, 0, 0, 0, 0,
                [], [], [], [new(Enemy) { StartingHand = [] }], [], new string('c', 64));
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-deck-gain", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var cold = CardCatalog.Get(CardKind.ColdSlash);
            builder.AddCard(new("fixture:deck-gain-cold", cold.DisplayName, cold.CategoryName, cold.Description, CardKind.ColdSlash));
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"two-slashes","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"obtainDeckCards","target":"owner","amount":2,"cardKinds":["slash","fireSlash","thunderSlash","coldSlash"]},{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"four-slashes","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"obtainDeckCards","target":"owner","amount":4,"cardKinds":["slash","fireSlash","thunderSlash","coldSlash"]},{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"tricks","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"obtainDeckCards","target":"owner","amount":4,"category":"trick"},{"op":"grantArmor","target":"owner","amount":1}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"gain-child","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementReasons":["{{Reason}}"],"movementOccurrence":"perBatch",
                    "usageScope":"game","usageLimit":1,"optional":false,"effects":[
                      {"op":"chooseOption","target":"owner","resultBind":"continue","options":[{"id":"continue"},{"id":"other"}]},
                      {"op":"draw","target":"owner","amount":1}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "真实牌堆获取", description = "原生实体筛选" },
                    [Watch] = new { name = "真实获得牌子流程", description = "获得牌后的真实停顿与返回",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续", ["other"] = "另一选择" } }
                } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddSkill(new(Quiet, "普通守军", "没有额外能力"));
            builder.AddGeneral(new(Owner, "获取牌者", "supporter", Driver, "qun", 12, [Watch]));
            builder.AddGeneral(new(Enemy, "守军", "supporter", Quiet, "wei", 12));
            string[] cards = ["standard:slash", "standard:fire_slash", "standard:thunder_slash", "fixture:deck-gain-cold",
                "standard:duel", "standard:iron_chain", "standard:indulgence", "standard:dodge", "standard:dodge", "standard:peach", "standard:crossbow"];
            builder.AddDeck(new(Deck, "固定混合实体", 0, 0, [])
                { PhysicalCards = cards.Select(id => new ContentDeckPhysicalCard(id, Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "共享获取牌实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
