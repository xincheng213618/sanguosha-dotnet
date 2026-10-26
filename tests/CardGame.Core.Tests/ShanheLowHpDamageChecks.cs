using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.RuntimeSkills;

internal static class ShanheLowHpDamageChecks
{
    private const string Owner = "fixture:low-hp-owner", Peer = "fixture:low-hp-peer";
    private const string Driver = "fixture:low-hp-driver", Quiet = "fixture:low-hp-quiet", Deck = "fixture:low-hp-deck";

    public static void NativeHpThresholdTracksLossRecoveryCardsAndSkillDamage()
    {
        var s = Scene.Start();
        Need(s.V().Hp == 4, "The production low-HP tactic starts above its inclusive threshold.");
        s.Card(CardKind.Slash, 1); s.Use("hit", [1]);
        Need(s.Hits().Select(e => e.Amount).SequenceEqual([1, 1]) && s.Modifiers().Length == 0,
            "Neither the real Slash nor a genuine cardless skill hit receives the bonus at four HP.");

        s.Use("lose");
        Need(s.V().Hp == 3 && s.Hits().Length == 2,
            "A native one-HP loss crosses the threshold without becoming damage.");
        s.Card(CardKind.Slash, 2); s.Card(CardKind.Duel, 2); s.Use("hit", [1]);
        Need(s.Hits().Skip(2).Select(e => (e.Amount, e.SourceCardUseResolutionId is not null))
                .SequenceEqual([(2, true), (2, true), (2, false)]) &&
            s.Modifiers() is [
                { CardKind: CardKind.Slash, BaseAmount: 1, ModifiedAmount: 2 },
                { CardKind: CardKind.Duel, BaseAmount: 1, ModifiedAmount: 2 },
                { CardKind: null, BaseAmount: 1, ModifiedAmount: 2 }] &&
            s.Modifiers().All(e => e.Source.OwnerSeat == 0 && e.Source.BindingId == "low-hp-damage" &&
                e.Source.SkillInstanceId == "campaign:shanhe-tactic:" + StandardShanheLowHpDamagePackage.Kuangbao2SkillId),
            "At exactly three HP, real Slash, ordinary-trick and cardless-skill segments each add one through the acquired production tactic binding.");

        s.Use("heal");
        Need(s.V().Hp == 4 && s.Events<RecoveryAppliedEvent>() is [{ TargetSeat: 0, Amount: 1 }],
            "A native recovery restores four HP before the next damage segment.");
        s.Card(CardKind.Slash, 1); s.Use("hit", [1]);
        Need(s.Hits().TakeLast(2).All(e => e.Amount == 1) && s.Modifiers().Length == 3,
            "A later card and skill hit immediately stop receiving the bonus after real recovery.");

        s.Use("lose"); s.Use("source-less");
        Need(s.V().Hp == 2 && s.Hits().Last() is { TargetSeat: 0, Amount: 1, SourceLess: true } &&
            s.Modifiers().Length == 3,
            "Even below the threshold, the native source-less cost remains one damage and cannot borrow the owner's tactic.");
        var count = s.Hits().Length; s.Use("lose");
        Need(s.V().Hp == 1 && s.Hits().Length == count && s.Modifiers().Length == 3,
            "Direct HP loss below the threshold stays exactly one and never enters the damage modifier pipeline.");
    }

    public static void SourceHpThresholdLoaderAndUndefinedHp()
    {
        var rules = JsonNode.Parse($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:hp-modifier","revision":1,"damageModifiers":[
              {"id":"threshold","cardKinds":[],"amount":1,"condition":"sourceHpAtMost","sourceScope":"damageSource","maximumHp":3}]}]}
            """)!;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { ["fixture:hp-modifier"] = new { name = "体力阈值", description = "共享伤害阈值" } } });
        var modifier = SkillProgramCatalog.Load(rules.ToJsonString(), presentation).Programs["fixture:hp-modifier"].DamageModifiers.Single();
        Need(modifier is { Condition: SkillProgramDamageModifierCondition.SourceHpAtMost, MaximumHp: 3 } &&
            !RuntimeRules_Core_PassiveRulePolicies.MatchesPerOccurrenceSourceDamageModifier(modifier, false, 0, 0, 0, 1) &&
            RuntimeRules_Core_PassiveRulePolicies.MatchesPerOccurrenceSourceDamageModifier(modifier, true, 3, 0, 0, 1) &&
            !RuntimeRules_Core_PassiveRulePolicies.MatchesPerOccurrenceSourceDamageModifier(modifier, true, 4, 0, 0, 1),
            "The shared public/runtime predicate requires an actual HP value and compares the configured inclusive maximum.");

        Action<JsonObject>[] invalid = [m => m.Remove("maximumHp"), m => m["maximumHp"] = 0,
            m => m["maximumHp"] = -1, m => m["sourceScope"] = "ownerUsed", m => m["capsToAmount"] = true,
            m => m["damageLayer"] = "cardDamage", m => m["cardKinds"] = new JsonArray("slash"),
            m => m["condition"] = "always"];
        foreach (var change in invalid)
        {
            var altered = rules.DeepClone(); change(altered["skills"]![0]!["damageModifiers"]![0]!.AsObject());
            var rejected = false;
            try { SkillProgramCatalog.Load(altered.ToJsonString(), presentation); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "The loader must reject a missing/non-positive or misplaced HP threshold and every unsupported scope, cap, layer or card filter.");
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        private GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public DamageAppliedEvent[] Hits() => Events<DamageAppliedEvent>();
        public ProgramCardDamageModifiedEvent[] Modifiers() => Events<ProgramCardDamageModifiedEvent>()
            .Where(e => e.Source.SkillId == StandardShanheLowHpDamagePackage.Kuangbao2SkillId).ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native low-HP command."); }
        private void Play()
        {
            for (var step = 0; step < 180; step++)
            {
                if (P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(G.State.Winner == Winner.None, "The bounded low-HP battle ended before its expected boundary.");
                if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Kind is DecisionKind.RescueDying or DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification
                        ? prompt.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die")
                        : prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native low-HP Play boundary.");
        }
        public void Card(CardKind kind, int expected)
        {
            var card = V().Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual([1]));
            Need(G.PredictPublicActionDamage(0, action, 1).DamageAmount == expected,
                "Public real-card prediction must use the same current-HP threshold as native damage.");
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void Use(string activation, int[]? targets = null)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId)); Play(); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheLowHpDamagePackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:low-hp", Owner, 4, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Club, 8),
                 new(CardKind.Slash, Suit.Heart, 9), new(CardKind.Duel, Suit.Spade, 1)], [],
                Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray(), [], new string('a', 64))
                { TacticSkillIds = [StandardShanheLowHpDamagePackage.Kuangbao2SkillId] };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-low-hp", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"slash-limit","query":"slashLimit","operation":"add","value":10,"priority":0}],"activations":[
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
                    {"id":"heal","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"owner","amount":1}]},
                    {"id":"source-less","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"receiveOwnerDamage","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "真实体力阈值驱动", description = "原生失血回复与伤害" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "体力阈值使用者", "supporter", Driver, "qun", 4));
            builder.AddGeneral(new(Peer, "固定敌方", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "真实体力阈值伤害", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
