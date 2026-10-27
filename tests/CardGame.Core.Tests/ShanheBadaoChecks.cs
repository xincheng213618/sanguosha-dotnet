using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBadaoChecks
{
    private const string Owner = "fixture:badao-owner", Peer = "fixture:badao-peer";
    private const string Driver = "fixture:badao-driver", Reflect = "fixture:badao-reflect", Quiet = "fixture:badao-quiet";
    private const string Deck = "fixture:badao-deck";

    public static void PreviousRoundActualDamageControlsCardsSkillsReflectionAndChainPrediction()
    {
        var s = Scene.Start();
        Need(s.Round == 1, "The first real global round exists before any previous-round qualification.");
        s.Predict(CardKind.FireSlash, [1], 1);
        var before = s.Hits().Length;
        // Public damage prediction describes payment if damage is reached;
        // Renwang Shield cancels this use before there is a damage attempt.
        s.Card(CardKind.Slash, [2], null);
        Need(s.Hits().Length == before && s.Game.CreateSnapshot(0).Players[2].Hp == 30,
            "Renwang Shield really prevents the black Slash; an attempted and paid use without positive applied damage cannot fill the previous-round counter.");

        s.NextRound();
        Need(s.Round == 2, "Both tactics first qualify after an actual full round with no applied damage.");
        s.Predict(CardKind.FireSlash, [1], 3);
        s.Card(CardKind.IronChain, [1, 3], null);
        before = s.Hits().Length;
        s.Card(CardKind.FireSlash, [1], 3);
        Need(s.Hits().Skip(before).Select(hit => (hit.TargetSeat, hit.Amount)).SequenceEqual([(1, 3), (3, 3)]) &&
             s.Events<ProgramCardDamageModifiedEvent>().Count(fact => fact.Source.SkillId is
                 StandardShanheBadaoPackage.SkillId or StandardShanheBadaoPackage.SecondSkillId) == 2,
            "Two qualifying sources add once to the root Fire Slash; the actual chained recipient inherits three rather than adding the two bonuses again.");

        s.NextRound();
        Need(s.Round == 3, "The six actual damage points belong to the completed second round.");
        s.Predict(CardKind.Slash, [1], 1);
        s.Use("armor-two", [1]);
        var hp = s.Game.CreateSnapshot(0).Players[1].Hp;
        before = s.Hits().Length;
        s.Use("skill-two", [1]);
        Need(s.Hits().Skip(before).ToArray() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 2,
                ArmorPayment: { ArmorAbsorbed: 2, HpLossAmount: 0 } }] &&
             s.Game.CreateSnapshot(0).Players[1].Hp == hp,
            "An actual two-point skill damage is recorded as Amount=2 even when native armor pays both points and HP stays unchanged, matching Core's applied-damage counter.");
        s.Predict(CardKind.Slash, [1], 1);

        s.NextRound();
        Need(s.Round == 4, "The preceding armor-paid skill damage is still the previous round's actual damage.");
        s.Predict(CardKind.Slash, [1], 2);
        before = s.Hits().Length;
        s.Use("provoke", [3]);
        Need(s.Hits().Skip(before).ToArray() is [{ SourceSeat: 0, TargetSeat: 3, Amount: 2 }] &&
             s.Events<DamageAppliedEvent>().Any(hit => hit.SourceSeat == 3 && hit.TargetSeat == 0 && hit.Amount == 1) &&
             s.Events<ProgramBindingResolvedEvent>().Count(binding => binding.SkillId == Reflect && binding.Activated && binding.Completed) == 1,
            "The native after-damage reflection belongs to its real holder and receives only the below-three bonus; the triggering enemy's damage keeps its own source and amount.");
        s.Predict(CardKind.Slash, [1], 2);

        s.NextRound();
        Need(s.Round == 5, "The reflected two damage points count toward their actual source's completed round.");
        s.Predict(CardKind.Slash, [1], 2);
        before = s.Hits().Length;
        s.Use("skill-two", [1]);
        Need(s.Hits().Skip(before).ToArray() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 3 }],
            "Previous reflected damage disables the no-damage source but leaves the below-three source active for a genuine skill hit.");
        s.Predict(CardKind.Slash, [1], 2);

        s.NextRound();
        Need(s.Round == 6, "A final native round advances past an exact three-point completed total.");
        s.Card(CardKind.Slash, [1], 1);
        Need(s.Events<ProgramCardDamageModifiedEvent>().Where(fact => fact.Source.SkillId is
                    StandardShanheBadaoPackage.SkillId or StandardShanheBadaoPackage.SecondSkillId)
                .GroupBy(fact => fact.Source.SkillId).ToDictionary(group => group.Key, group => group.Count()) is var counts &&
             counts.GetValueOrDefault(StandardShanheBadaoPackage.SkillId) == 1 &&
             counts.GetValueOrDefault(StandardShanheBadaoPackage.SecondSkillId) == 3,
            "At the exact previous-round boundary of three neither source applies; first-round, source ownership and each independent bonus are preserved throughout the real six-round flow.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => Game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public DamageAppliedEvent[] Hits() => Events<DamageAppliedEvent>().Where(hit => hit.SourceSeat == 0).ToArray();
        public int Round => Events<RoundStartedEvent>().Last().RoundNumber;
        public PlayerSnapshot View => Game.CreateSnapshot(0).Players[0];
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "A native Badao command failed."); }
        public void Play()
        {
            for (var step = 0; step < 220; step++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(Game.State.Winner == Winner.None, "The small round fixture ended before returning to the original Play boundary.");
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is
                        "pass" or "take-damage" or "no-nullification" or "let-die");
                    Need(choice is not null, "Unexpected Badao native choice: " + JsonSerializer.Serialize(prompt));
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The bounded Badao native flow did not return to Play.");
        }
        public void Use(string activation, int[] targets)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, Game.Revision, P!.PromptId)); Play(); }
        private (CardSnapshot Card, LegalAction Action) Action(CardKind kind, int[] targets)
        {
            var card = View.Hand.First(item => item.Kind == kind);
            var action = Game.GetHumanLegalActions().First(item => item.CardId == card.Id &&
                item.ConversionSource is null && item.TargetSeats.SequenceEqual(targets));
            return (card, action);
        }
        public void Predict(CardKind kind, int[] targets, int expected)
        {
            var (_, action) = Action(kind, targets);
            var revision = Game.Revision; var events = Game.Events.Count; var moves = Game.CardMovements.Count;
            for (var query = 0; query < 3; query++)
                Need(Game.PredictPublicActionDamage(0, action, targets[0]).DamageAmount == expected,
                    $"Public prediction must share the actual previous-round total; round={Round}, expected={expected}.");
            Need(Game.Revision == revision && Game.Events.Count == events && Game.CardMovements.Count == moves,
                "Repeated public prediction does not mutate the round history, damage facts or paid materials.");
        }
        public void Card(CardKind kind, int[] targets, int? expected)
        {
            var (card, action) = Action(kind, targets);
            if (expected is { } amount) Predict(kind, targets, amount);
            var before = Hits().Length;
            Accept(new PlayCardCommand(0, card.Id, targets, Game.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)); Play();
            if (expected is { } dealt && dealt > 0)
                Need(Hits().Skip(before).First() is { SourceSeat: 0 } hit && hit.TargetSeat == targets[0] && hit.Amount == dealt,
                    "The accepted native root damage matches its exact public prediction.");
            Need(Game.CardMovements.Count(move => move.CardId == card.Id && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
                 Game.CardMovements.Count(move => move.CardId == card.Id && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1,
                "Every physical Slash and chain uses its original material once and finishes it once in the real discard pile.");
        }
        public void NextRound()
        {
            var previous = Round;
            Accept(new EndPlayPhaseCommand(0, Game.Revision, P!.PromptId)); Play();
            Need(Round == previous + 1, "The threshold must advance through three real enemy turns to the next global round.");
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheBadaoPackage(), new Fixture());
            var setup = new ShanheBattleSetup(71, 1, "fixture:badao", Owner, 10, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Club, 3), new(CardKind.FireSlash, Suit.Heart, 4),
                 new(CardKind.Slash, Suit.Club, 5), new(CardKind.IronChain, Suit.Club, 6)], [],
                [new(Peer) { StartingHand = [] },
                 new(Peer) { StartingHand = [], StartingEquipment = [new(CardKind.RenwangShield, Suit.Club, 2)] },
                 new(Peer) { StartingHand = [] }], [], new string('e', 64))
                { TacticSkillIds = [StandardShanheBadaoPackage.SkillId, StandardShanheBadaoPackage.SecondSkillId] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 32, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:badao-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var kind in new[] { CardKind.FireSlash, CardKind.IronChain, CardKind.RenwangShield })
            {
                var card = CardCatalog.Get(kind);
                builder.AddCard(new("fixture:badao-card-" + kind.ToString().ToLowerInvariant(),
                    card.DisplayName, card.CategoryName, card.Description, kind));
            }
            var programs = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"hand","query":"handLimit","operation":"add","value":20,"priority":0}],"activations":[
                    {"id":"skill-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"selectedTarget","amount":2}]},
                    {"id":"armor-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"grantArmor","target":"selectedTarget","amount":2}]},
                    {"id":"provoke","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}]},
                  {"id":"{{{Reflect}}}","revision":1,"triggers":[{"id":"real-reflection","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,
                    "effects":[{"op":"damage","target":"owner","targetRef":{"kind":"eventSource"},"sourceRef":{"kind":"owner"},"amount":1,"nature":"normal"}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Reflect, Quiet }.ToDictionary(id => id, _ => new { name = "拔刀术原生轮次", description = "真实伤害、反伤、护甲与轮次" }) }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "拔刀术持有者", "supporter", Driver, "shu", 10) { AdditionalSkillIds = [Reflect] });
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "实际备用实体", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "拔刀术四人场", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
