using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheArmorBreakTacticChecks
{
    private const string Geshan = StandardShanheArmorBreakTacticsPackage.GeshanDaniuSkillId;
    private const string Tijia = StandardShanheArmorBreakTacticsPackage.TijiaShuSkillId;
    private const string Driver = "fixture:armor-break-driver", Quiet = "fixture:armor-break-quiet";
    private const string Owner = "fixture:armor-break-owner", Deck = "fixture:armor-break-deck";

    public static void DoubleArmorPreservesNativeOverflowAndBypassPriorityAndSourcelessPayment()
    {
        var s = Scene.Start([Tijia]);
        s.Use("armor-three", 2);
        var slash = s.Action(CardKind.Slash, 2);
        Need(s.G.PredictPublicActionDamage(0, slash, 2) is
                { DamageAmount: 1, ArmorAbsorbed: 2, HpLossAmount: 0, HpAfter: 12, ArmorAfter: 1 },
            "Tijia's actual Slash prediction must expose two Armor pressure without doubling its one-point damage.");
        s.Card(slash);
        Need(s.V(2) is { Hp: 12, Armor: 1 } && s.E<DamageAppliedEvent>().Last() is
                { Amount: 1, ArmorPayment: { DamageAmount: 1, ArmorBefore: 3, ArmorAbsorbed: 2,
                    ArmorAfter: 1, ArmorDamageMultiplier: 2, HpLossAmount: 0 } },
            "A native one-point Slash pays exactly two Armor and keeps its original damage amount.");
        s.Use("hit-three", 2);
        Need(s.V(2) is { Hp: 10, Armor: 0 } && s.E<DamageAppliedEvent>().Last() is
                { Amount: 3, ArmorPayment: { DamageAmount: 3, ArmorBefore: 1, ArmorAbsorbed: 1,
                    ArmorAfter: 0, ArmorDamageMultiplier: 2, HpBefore: 12, HpLossAmount: 2, HpAfter: 10 } },
            "One remaining Armor cancels one whole damage point under the existing native ceiling convention, leaving two actual lost HP.");
        s.Use("armor-three", 2); s.Use("hit-three", 2);
        Need(s.V(2) is { Hp: 9, Armor: 0 } && s.E<DamageAppliedEvent>().Last() is
                { Amount: 3, ArmorPayment: { ArmorBefore: 3, ArmorAbsorbed: 3, ArmorAfter: 0,
                    ArmorDamageMultiplier: 2, HpBefore: 10, HpLossAmount: 1, HpAfter: 9 } },
            "Three Armor against three damage pays all three Armor and only one HP, rather than converting the missing three Armor pressure into HP damage.");
        s.Use("armor-three", 2); s.Use("armor-three", 2); s.Use("hit-three", 2);
        Need(s.V(2) is { Hp: 9, Armor: 0 } && s.E<DamageAppliedEvent>().Last() is
                { Amount: 3, ArmorPayment: { ArmorBefore: 6, ArmorAbsorbed: 6,
                    ArmorDamageMultiplier: 2, HpLossAmount: 0 } },
            "A fully funded multi-point native damage segment spends six Armor and no HP.");
        s.Use("hit-two", 3);
        Need(s.V(3).Hp == 10 && s.E<DamageAppliedEvent>().Last() is { Amount: 2, ArmorPayment: null },
            "The same source loses only two HP from a bare target: Tijia never doubles damage or HP loss.");

        var both = Scene.Start([Geshan, Tijia]);
        both.Use("armor-three", 2); both.Use("armor-three", 0);
        both.Use("hit-two", 2);
        Need(both.V(2) is { Hp: 10, Armor: 3 } && both.E<DamageAppliedEvent>().Last() is
                { Amount: 2, ArmorPayment: null },
            "When both locked tactics are actually acquired, bypass wins before the Armor multiplier and does not consume the other participant's Armor.");
        both.Use("hit-two", 0);
        Need(both.V(0) is { Hp: 12, Armor: 0 } && both.E<DamageAppliedEvent>().Last() is
                { SourceSeat: 0, TargetSeat: 0, Amount: 2, ArmorPayment:
                    { ArmorBefore: 3, ArmorAbsorbed: 3, ArmorDamageMultiplier: 2, HpLossAmount: 0 } },
            "The same stacked source still gets Tijia on genuine self damage because only Geshan excludes its owner.");
        both.Use("armor-three", 0); both.Use("armor-three", 0);
        both.Card(both.Action(CardKind.Lightning, 0)); both.NextOwnPlay();
        Need(both.V(0) is { Hp: 12, Armor: 3 } && both.E<DamageAppliedEvent>().Last() is
                { TargetSeat: 0, Amount: 3, Nature: DamageNature.Thunder, SourceLess: true,
                    ArmorPayment: { SourceLess: true, ArmorBefore: 6, ArmorAbsorbed: 3,
                        ArmorAfter: 3, ArmorDamageMultiplier: 1, HpLossAmount: 0 } } &&
            both.E<LightningResolvedEvent>().Any(e => e.Hit && e.DamageAmount == 3),
            "A real later Lightning judgment produces source-less Thunder damage and pays ordinary Armor once, even though its placeholder source seat owns both tactics.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static string Peer(int seat) => "fixture:armor-break-peer-" + seat;

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native armor-break command."); }
        private void Until(Func<bool> done)
        {
            for (var step = 0; step < 240; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The small Armor battle ended before the expected boundary.");
                if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Kind is DecisionKind.RescueDying or DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification
                        ? prompt.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die")
                        : prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native armor-break boundary: " + JsonSerializer.Serialize(P));
        }
        private void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string activation, int target)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], [target], G.Revision, P!.PromptId)); Play(); }
        public LegalAction Action(CardKind kind, int target)
        {
            var card = V(0).Hand.First(c => c.Kind == kind);
            return G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.TargetSeats.SequenceEqual([target]));
        }
        public int Card(LegalAction action)
        {
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            Play(); return action.CardId.Value;
        }
        public void NextOwnPlay()
        {
            var turn = G.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
            Until(() => G.State.TurnNumber > turn && P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public static Scene Start(string[] tactics)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheArmorBreakTacticsPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:armor-break", Owner, 12, 0, 0, 0, 1, [],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Lightning, Suit.Spade, 1)], [],
                [new(Peer(2)) { StartingHand = [] }, new(Peer(3)) { StartingHand = [] }],
                [new(Peer(1)) { StartingHand = [] }], new string('a', 64)) { TacticSkillIds = tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-armor-break", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,
                   "modifiers":[{"id":"fixture-range","query":"attackRange","operation":"add","value":10,"priority":0}],
                   "activations":[
                     {"id":"armor-three","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"grantArmor","target":"selectedTarget","amount":3}]},
                     {"id":"hit-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"selectedTarget","amount":2}]},
                     {"id":"hit-three","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"selectedTarget","amount":3}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "护甲伤害实战驱动", description = "原生护甲、技能伤害与真实牌" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "护甲伤害使用者", "supporter", Driver, "qun", 12));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new(Peer(seat), "固定队伍角色", "supporter", Quiet, "wei", 12));
            builder.AddDeck(new(Deck, "固定闪电命中实体", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "护甲伤害战法实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
