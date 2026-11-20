using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MobileGuoJiaYijiGainReturnChecks
{
    private const string GuoJia = "mobile:guo-jia", Yiji = "mobile:yiji";
    private const string Driver = "fixture:yiji-gain-return-driver", Watch = "fixture:yiji-first-receiver";
    private const string Recipient = "fixture:yiji-recipient", Peer = "fixture:yiji-observer", Deck = "fixture:yiji-gain-return-deck";
    private const string View = "skill-program.wushuang.yiji.view", Give = "skill-program.wushuang.yiji.give";

    internal static void FirstYijiRecipientDeathReturnsToLiveChoicesOrFinishesTheMatchCold()
    {
        Check(terminal: false);
        Check(terminal: true);
    }

    private static void Check(bool terminal)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(terminal));
        var game = Start(content, terminal);
        var recipient = terminal ? 2 : 1;
        var survivor = terminal ? 1 : 2;
        var play = Next(game);
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The ordinary mobile Guo Jia is in his actual Play phase.");
        Accept(game, new UseProgramSkillCommand(0, Driver, "hurt", [], [], game.Revision, play!.PromptId));
        var activate = Next(game);
        Require(activate?.SkillPrompt?.SkillId == Yiji &&
            Facts<DamageAppliedEvent>(game).Single() is { TargetSeat: 0, Amount: 1 },
            "One point of genuine damage opens ordinary mobile Yiji.");
        Answer(game, activate!, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        var first = Next(game);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Yiji);
        var damageWindow = game.ResolutionStack.OfType<DamageTriggerWindowFrame>().Single();
        var materials = game.CardMovements.Where(move => move.Reason.Value == View).ToArray();
        Require(first?.SkillPrompt?.SkillId == Yiji && root.WindowContext?.ParentFrameId == damageWindow.Id &&
            materials.Length == 2 && materials.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Processing) &&
            game.CreateSnapshot(0).PrivateRevealedCards?.Select(card => card.Id).ToHashSet().SetEquals(materials.Select(move => move.CardId)) == true &&
            game.CreateSnapshot(recipient).PrivateRevealedCards is null or { Count: 0 },
            "Both original top cards are privately reserved on the real damage frame before any one is handed out.");
        var given = materials[0].CardId;
        var remaining = materials[1].CardId;
        Answer(game, first!, choice => choice.Cards.SequenceEqual([given]) && choice.Targets.SequenceEqual([recipient]));
        var receive = Next(game);
        var gain = game.CardMovements.Single(move => move.Reason.Value == Give);
        Require(receive is { SkillPrompt.SkillId: Watch } && receive.PlayerSeat == recipient &&
            gain.CardId == given && gain.From == CardLocation.Processing && gain.To == CardLocation.Hand(recipient) &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single().ResumeProgramFrameId == root.Id &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id).WushuangOriginalSkill?.Cards.SequenceEqual([remaining]) == true &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == remaining).Location == CardLocation.Processing,
            "The first real delivery pauses at its own recipient's gain child with the second private material still reserved.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold public-command replay preserves the private pending material and already-paid gift child.");
        game = restored;
        Answer(game, Next(game)!, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        var next = Next(game);
        Require(Facts<PlayerDiedEvent>(game).Single() is { VictimSeat: var dead } && dead == recipient &&
            game.CardMovements.Any(move => move.CardId == given && move.From == CardLocation.Hand(recipient) &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.DeathDiscard),
            "The first recipient really dies in the gain child and the committed gift participates in native death cleanup.");

        if (terminal)
        {
            Require(Facts<WinnerDeterminedEvent>(game).Any() && game.State.Status == EngineStatus.Completed &&
                next is null && game.ResolutionStack.Count == 0 &&
                game.CardMovements.Count(move => move.Reason.Value == Give) == 1 &&
                game.CreateCardZoneDiagnostics().Single(card => card.CardId == remaining).Location == CardLocation.DiscardPile,
                "The death of the last opponent ends the match and releases the reserved second card without publishing an unanswerable Yiji choice.");
            return;
        }

        Require(next is { PlayerSeat: 0, SkillPrompt.SkillId: Yiji } &&
            next.Choices.All(choice => choice.Cards.SequenceEqual([remaining]) && !choice.Targets.Contains(recipient)) &&
            game.ResolutionStack.OfType<DamageTriggerWindowFrame>().Single().Id == damageWindow.Id,
            "The original damage window offers the second material to current living players only.");
        Answer(game, next!, choice => choice.Targets.SequenceEqual([survivor]));
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            game.CardMovements.Count(move => move.Reason.Value == Give) == 2 &&
            game.CardMovements.Single(move => move.CardId == remaining && move.Reason.Value == Give).To == CardLocation.Hand(survivor) &&
            Facts<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == root.Id && fact.SkillId == Yiji && fact.Activated && fact.Completed) &&
            game.ResolutionStack.All(frame => frame is not DamageFrame and not DamageTriggerWindowFrame),
            "The original nonterminal Yiji gives the second card once and returns the original damage to Play.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision? Next(GameEngine game)
    {
        for (var step = 0; step < 130; step++)
        {
            if (game.State.Status == EngineStatus.Completed)
                return null;
            var prompt = Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).PendingDecision)
                .FirstOrDefault(item => item is not null);
            if (prompt is not null)
            {
                var pass = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") == "let-die");
                if (pass is null)
                    return prompt;
                Answer(game, prompt, choice => choice.Id == pass.Id);
            }
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The mobile Yiji fixture did not reach its next public boundary.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> select) =>
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));

    private static GameEngine Start(ContentRegistry content, bool terminal)
    {
        var recipient = new ShanheCombatantDefinition(Recipient, Hp: 1) { StartingHand = [] };
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(83, 1, "mobile-yiji-first-gain-return", GuoJia, 3, 0, 0, 0, 0, [Driver], [],
            [], terminal ? [recipient] : [recipient, peer], terminal ? [peer] : [], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        return game;
    }

    private sealed class Fixture(bool terminal) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-yiji-gain-return", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, GuoJia);
            var rules = $$"""
                {"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"hurt","minCards":0,"maxCards":0,"minTargets":0,
                    "maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"first-gain-loses-hp","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{Give}}"],"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]},
                      {"op":"loseHp","target":"owner","amount":1}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "真实承伤", description = "对自己造成1点伤害。" },
                    [Watch] = new
                    {
                        name = "首张来牌", description = "实际获得遗计牌后，失去1点体力。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" }
                    }
                }
            });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
            {
                var shown = catalog.Presentations[id];
                builder.AddSkill(new(id, shown.Name, shown.Description) { Program = program, ProgramPresentation = shown });
            }
            builder.AddGeneral(new(Recipient, "第一张牌的接收者", "supporter", Watch, "qun", 3));
            builder.AddGeneral(new(Peer, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定无救援的牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 12).Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, rank + 1)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动遗计首张交付", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [GuoJia, Recipient, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int>
                {
                    [ShanheBattleSetup.PlayerTeamId] = terminal ? 2 : 1,
                    [ShanheBattleSetup.EnemyTeamId] = terminal ? 1 : 2
                }));
        }
    }
}
