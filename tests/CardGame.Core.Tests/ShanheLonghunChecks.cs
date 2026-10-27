using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ShanheLonghunChecks
{
    private const string Skill = StandardShanheLonghunPackage.SkillId;
    private const string Mode = "fixture:shanhe-longhun", Owner = "fixture:shanhe-longhun-owner";
    private const string Quiet = "fixture:shanhe-longhun-quiet", Attack = "fixture:shanhe-longhun-attack";

    public static void TwoBlackResponsesCompulsorilyDiscardCurrentTurnCard()
    {
        foreach (var suit in new[] { Suit.Club, Suit.Spade })
        {
            var scene = Scene.Start(suit);
            scene.Response();
            var turnOwner = scene.Game.State.CurrentSeat;
            var choice = scene.ConvertedChoice(2);
            var materials = choice.Cards.ToArray();
            scene.Answer(candidate => candidate.Id == choice.Id);
            scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Skill);
            Need(!scene.P!.Choices.Any(candidate => candidate.Parameters.GetValueOrDefault("program-action") is "activate" or "skip"),
                "The S5 two-black bonus goes directly to its compulsory target/card selection without an invocation or skip choice.");
            scene.Play();
            var discarded = scene.Game.CardMovements.Where(move => move.From == CardLocation.Hand(turnOwner) &&
                move.To == CardLocation.DiscardPile && move.Reason.Value.Contains(Skill, StringComparison.Ordinal)).ToArray();
            Need(turnOwner != 0 && discarded.Length == 1 &&
                scene.Events<ProgramCardTriggerResolvedEvent>().Count(fact => fact.SkillId == Skill && fact.Activated) == 1,
                "A genuine two-club Dodge or two-spade Nullification discards one actual card from the current turn player exactly once.");
            Need(scene.Events<CardActionAcceptedEvent>().Any(fact => fact.Action.ActorSeat == 0 &&
                    fact.Action.EffectiveKind == (suit == Suit.Club ? CardKind.Dodge : CardKind.Nullification) &&
                    fact.Action.PhysicalCards.Select(card => card.CardId).ToHashSet().SetEquals(materials) &&
                    fact.Action.ConversionChain.Any(conversion => conversion.SkillId == Skill && conversion.BindingId == scene.Binding(2))) &&
                materials.All(id => scene.Game.CardMovements.Count(move => move.CardId == id &&
                    move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
                    scene.Game.CardMovements.Count(move => move.CardId == id && move.To == CardLocation.DiscardPile) == 1),
                "The native response pays both published physical materials once and completes its ordinary processing/discard return.");
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, Suit suit, bool ordinary)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Game.PendingDecision;
        private string ActiveSkill => ordinary ? "ol:longhun" : Skill;
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public string Binding(int count) => count + (suit == Suit.Club ? "-club" : "-spade");
        public PromptChoice ConvertedChoice(int count) => P!.Choices.First(candidate =>
            candidate.Parameters.GetValueOrDefault("conversion-skill-id") == ActiveSkill &&
            candidate.Parameters.GetValueOrDefault("conversion-binding-id") == Binding(count) && candidate.Cards.Count == count);
        public void Response() => Reach(() => P is { PlayerSeat: 0 } prompt &&
            prompt.Kind == (suit == Suit.Club ? DecisionKind.RespondDodge : DecisionKind.Nullification) &&
            prompt.Choices.Any(candidate => candidate.Parameters.GetValueOrDefault("conversion-skill-id") == ActiveSkill && candidate.Cards.Count == 2));
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = P!;
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, Game.Revision));
        }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Accept(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Longhun fixture command was rejected.");
        }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 300; step++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.DiscardCards } discard)
                    Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, Game.Revision));
                else if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } play)
                    Accept(new EndPlayPhaseCommand(0, Game.Revision, play.PromptId));
                else if (P is { PlayerSeat: 0 } prompt && prompt.Choices.Count > 0)
                {
                    var choice = prompt.Choices.FirstOrDefault(candidate => candidate.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        prompt.Choices.FirstOrDefault(candidate => candidate.Parameters.GetValueOrDefault("response") is "pass" or "take-damage") ?? prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The native Longhun boundary did not arrive: " + JsonSerializer.Serialize(P));
        }

        public static Scene Start(Suit suit, bool ordinary = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheLonghunPackage(), new Fixture(suit, ordinary));
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = 17, ModeId = Mode, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = "player",
                UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 12
            }, registry), suit, ordinary);
            scene.Accept(new StartGameCommand());
            scene.Reach(() => scene.P is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
            scene.Accept(new SelectGeneralCommand(0, Owner, scene.Game.Revision, scene.P!.PromptId));
            return scene;
        }
    }

    private sealed class Fixture(Suit suit, bool ordinary) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-longhun", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            if (ordinary)
            {
                var assembly = typeof(StandardContentPackage).Assembly;
                string Read(string suffix)
                {
                    using var stream = assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.ol-shen-zhao-yun." + suffix + ".json")!;
                    using var reader = new StreamReader(stream); return reader.ReadToEnd();
                }
                var original = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
                builder.AddSkill(new("ol:longhun", "龙魂", original.Presentations["ol:longhun"].Description)
                    { Program = original.Programs["ol:longhun"], ProgramPresentation = original.Presentations["ol:longhun"] });
            }
            var window = suit == Suit.Club ? "afterNormalDraw" : "turnStartBeforeNormalFlow";
            var use = suit == Suit.Club
                ? """{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}"""
                : """{"op":"useVirtualOrdinaryTrick","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"outputKind":"duel"}""";
            var rules = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"skip","window":"afterNormalDraw","subject":"owner","priority":-100,"optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{{Attack}}}","revision":1,"triggers":[{"id":"attack","window":"{{{window}}}","subject":"owner","optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{{{use}}}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Quiet] = new { name = "跳过出牌", description = "固定对手" },
                        [Attack] = new { name = "原生来牌", description = "原生响应流程" } } }));
            foreach (var (id, program) in rules.Programs)
                builder.AddSkill(new(id, rules.Presentations[id].Name, rules.Presentations[id].Description)
                    { Program = program, ProgramPresentation = rules.Presentations[id] });
            builder.AddGeneral(new(Owner, "龙魂持有者", "", ordinary ? "ol:longhun" : Skill, "qun", 12));
            for (var seat = 1; seat < 4; seat++)
                builder.AddGeneral(new("fixture:shanhe-longhun-peer-" + seat, "固定对手", "", Quiet, "qun", 12, [Attack]));
            const string deck = "fixture:shanhe-longhun-deck";
            builder.AddDeck(new(deck, "黑色实体牌", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 120)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", suit, 7)).ToArray() });
            builder.AddMode(new(Mode, "龙魂真实响应夹具", 4, 4, new Dictionary<string, int>(), deck, 2,
                [Owner, "fixture:shanhe-longhun-peer-1", "fixture:shanhe-longhun-peer-2", "fixture:shanhe-longhun-peer-3"],
                ContentModeKind.Team, new Dictionary<string, int> { ["player"] = 1, ["other"] = 3 }));
        }
    }
}
