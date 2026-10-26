using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDingcuoChecks
{
    private const string Skill = StandardShanheDingcuoPackage.SkillId;
    private const string Owner = "fixture:shanhe-dingcuo-owner", Peer = "fixture:shanhe-dingcuo-peer";
    private const string Driver = "fixture:shanhe-dingcuo-driver", Quiet = "fixture:shanhe-dingcuo-quiet";
    private const string Deck = "fixture:shanhe-dingcuo-deck";

    public static void NativeSelfDamageUsesActualDrawColorsAndOneSharedTurnAllowance()
    {
        // Resolve one fixed native deal, then recolor those exact physical deck
        // entities before constructing the actual scene. No runtime zone edits
        // or repeated seed search are needed to guarantee a mixed-color pair.
        var stock = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray();
        var probe = Scene.Start(stock);
        probe.Begin("self-damage"); probe.Activate(); probe.Play();
        var pair = probe.Draws().Select(move => move.CardId).ToArray();
        Need(pair.Length == 2 && probe.Discards().Length == 0,
            "Two actual same-color deck entities complete Dingcuo without a discard.");
        stock[pair[0] - 1] = stock[pair[0] - 1] with { Suit = Suit.Heart };

        var s = Scene.Start(stock);
        var before = s.V.Hand.Count;
        var original = s.V.Hand.Single().Id;
        s.Begin("self-damage");
        s.Reach(prompt => prompt.SkillPrompt?.SkillId == Skill && prompt.Choices.Any(choice =>
            choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Need(s.V.Hp == 19 && s.Bindings().Length == 0 && s.Draws().Length == 0 && s.Discards().Length == 0 &&
            s.G.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().Single() is
                { SourceSeat: 0, TargetSeat: 0, Amount: 1 },
            "Self-inflicted real damage offers the independent Shanhe source before its optional activation starts or pays the draw/discard steps.");
        s.Activate();
        s.Reach(prompt => prompt.SkillPrompt?.SkillId == Skill && prompt.Choices.Any(choice => choice.Cards.Count == 1));
        var drawn = s.Draws();
        Need(s.Bindings() is [{ BindingId: "dingcuo-damage-taken" }] &&
            drawn.Select(move => move.CardId).SequenceEqual(pair) &&
            drawn.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) &&
            s.V.Hand.Where(card => pair.Contains(card.Id)).Select(card => card.Suit is Suit.Heart or Suit.Diamond).Distinct().Count() == 2 &&
            s.V.Hand.Count == before + 2,
            "The paid draw obtains the two original mixed-color entities before the mandatory hand-discard child.");
        s.Answer(choice => choice.Cards.SequenceEqual([original])); s.Play();
        Need(s.Discards() is [{ CardId: var paid }] && paid == original && s.V.Hand.Count == before + 1 &&
            !s.V.Hand.Any(card => card.Id == original) && pair.All(id => s.V.Hand.Any(card => card.Id == id)),
            "The discard really pays one chosen current hand card, including a card owned before the two-card draw.");

        s.Use("hit", [1]); s.Use("incoming", [1]); s.Use("self-damage");
        Need(s.Bindings().Length == 1 && s.Draws().Length == 2 && s.Discards().Length == 1 && s.V.Hp == 17,
            "Caused damage, another received hit and a second self-hit share the already spent actual-turn allowance.");

        var turn = s.G.State.TurnNumber; s.NextOwnPlay();
        Need(s.G.State.TurnNumber > turn, "Normal native turns, rather than a fixture ledger edit, reset the allowance.");
        s.Use("lose-hp");
        Need(s.Bindings().Length == 1 && s.Draws().Length == 2,
            "Direct HP loss in a fresh turn neither offers Dingcuo nor spends its damage allowance.");
        before = s.V.Hand.Count;
        s.Begin("hit", [1]); s.Activate(); s.Play();
        Need(s.Bindings().Select(binding => binding.BindingId).SequenceEqual(["dingcuo-damage-taken", "dingcuo-damage-caused"]) &&
            s.Draws().Length == 4 && s.Discards().Length == 1 && s.V.Hand.Count == before + 2,
            "The next actual turn offers the caused-damage branch, draws two same-color cards and needs no additional discard.");
        s.Use("self-damage");
        Need(s.Bindings().Length == 2 && s.Draws().Length == 4,
            "After the caused-damage branch is used, the received branch cannot claim a separate allowance in that same turn.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        private PendingDecision? P => G.PendingDecision;
        public ProgramBindingStartedEvent[] Bindings() => G.Events.Select(item => item.Payload)
            .OfType<ProgramBindingStartedEvent>().Where(binding => binding.SkillId == Skill).ToArray();
        public CardMovementRecord[] Draws() => G.CardMovements.Where(move =>
            move.Reason.Value == $"skill-program.{Skill}.{SkillProgramEffectOp.Draw}").ToArray();
        public CardMovementRecord[] Discards() => G.CardMovements.Where(move =>
            move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile &&
            move.Reason.Value.StartsWith("skill-program." + Skill + ".", StringComparison.Ordinal)).ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The native Dingcuo command was rejected."); }
        public void Answer(Func<PromptChoice, bool> select)
        {
            var prompt = P ?? throw new InvalidOperationException("The Dingcuo scene has no decision.");
            var choice = prompt.Choices.FirstOrDefault(select) ?? throw new InvalidOperationException(JsonSerializer.Serialize(prompt));
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
        }
        public void Reach(Func<PendingDecision, bool> done)
        {
            for (var step = 0; step < 100; step++)
            {
                if (P is { } prompt && done(prompt)) return;
                Need(P?.PlayerSeat != 0, "Unexpected native Dingcuo prompt: " + JsonSerializer.Serialize(P));
                Need(G.State.Winner == Winner.None, "The bounded Dingcuo fixture must retain both living participants.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native Dingcuo continuation.");
        }
        public void Play() => Reach(prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Begin(string activation, int[]? targets = null) => Accept(new UseProgramSkillCommand(
            0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId));
        public void Activate()
        {
            Reach(prompt => prompt.SkillPrompt?.SkillId == Skill && prompt.Choices.Any(choice =>
                choice.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        }
        public void Use(string activation, int[]? targets = null) { Begin(activation, targets); Play(); }
        public void NextOwnPlay() { Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId)); Play(); }
        public static Scene Start(IReadOnlyList<ContentDeckPhysicalCard> stock)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheDingcuoPackage(), new Fixture(stock));
            var setup = new ShanheBattleSetup(83, 1, "fixture:shanhe-dingcuo", Owner, 20, 0, 0, 0, 0,
                [Skill], [new(CardKind.Slash, Suit.Spade, 7)], [], [new(Peer) { StartingHand = [] }], [], new string('c', 64));
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(IReadOnlyList<ContentDeckPhysicalCard> stock) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-dingcuo", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"self-damage","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1}]},
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},
                    {"id":"lose-hp","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id,
                        _ => new { name = "定措原生行为驱动", description = "固定小局的真实伤害和正常回合" })
                }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "定措使用者", "supporter", Driver, "shu", 20));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 20));
            builder.AddDeck(new(Deck, "固定原生实体牌", 0, 0, []) { PhysicalCards = stock });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "定措真实小型战斗", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
