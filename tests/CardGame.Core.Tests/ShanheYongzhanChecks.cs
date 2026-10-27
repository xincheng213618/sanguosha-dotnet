using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheYongzhanChecks
{
    private const string Owner = "fixture:yongzhan-owner", Ally = "fixture:yongzhan-ally", Enemy = "fixture:yongzhan-enemy";
    private const string Driver = "fixture:yongzhan-driver", Quiet = "fixture:yongzhan-quiet", Watch = "fixture:yongzhan-watch";
    private const string Deck = "fixture:yongzhan-deck";

    public static void RealRescueHitsOnlyEnemiesFromTheSurvivorAndRepeatsOnTheNextRescue()
    {
        foreach (var (skill, amount) in new[]
                 { (StandardShanheYongzhanPackage.SkillId, 1), (StandardShanheYongzhanPackage.SecondSkillId, 2) })
        {
            var s = Scene.Start(skill);
            var allyHp = s.View(1).Hp;
            var enemyHp = new[] { s.View(2).Hp, s.View(3).Hp };
            for (var occurrence = 1; occurrence <= 2; occurrence++)
            {
                s.BeginFall(occurrence == 1 ? "fall-four" : "fall-one");
                s.Until(() => s.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
                Need(s.View(0).Hp == 0 && s.Events<DyingResolvedEvent>().Count(fact => fact.VictimSeat == 0 && fact.Survived) == occurrence - 1 &&
                     s.Events<ProgramBindingStartedEvent>().Count(fact => fact.SkillId == skill) == occurrence - 1 &&
                     Enumerable.Range(2, 2).All(seat => s.View(seat).Hp == enemyHp[seat - 2] - (occurrence - 1) * amount),
                    "Entering a real unresolved Dying window does not prematurely fire survived-Dying damage.");
                var peach = s.View(1).Hand.First(card => card.Kind == CardKind.Peach).Id;
                var prompt = s.P!;
                var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault("response") == "let-die" &&
                    item.Parameters.GetValueOrDefault("target-seat") == "0" && item.Cards.Count == 0);
                s.Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, s.Game.Revision));
                s.Until(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
                var bindings = s.Events<ProgramBindingStartedEvent>().Where(fact => fact.SkillId == skill).ToArray();
                var frames = bindings.Select(fact => fact.FrameId).ToHashSet();
                var requested = s.Events<DamageRequestedEvent>().Where(fact =>
                    s.DamageParent(fact.ResolutionId) is { } parent && frames.Contains(parent)).ToArray();
                Need(s.View(0) is { Hp: 1, IsAlive: true } && s.View(0).Hand.Count(card => card.Kind == CardKind.Peach) == 1 && s.View(1).Hp == allyHp &&
                     Enumerable.Range(2, 2).All(seat => s.View(seat).Hp == enemyHp[seat - 2] - occurrence * amount) &&
                     bindings.Length == occurrence && bindings.All(fact => fact.OwnerSeat == 0 && fact.Window == SkillProgramTriggerWindow.DyingExited) &&
                     requested.Length == occurrence * 2 && requested.All(fact => fact.SourceSeat == 0 && fact.TargetSeat is 2 or 3 &&
                         fact.Amount == amount && fact.Nature == DamageNature.Normal && fact.SourceCard is null && !fact.SourceLess) &&
                     requested.Select(fact => fact.ResolutionId).Distinct().Count() == occurrence * 2 &&
                     frames.All(parent => requested.Where(fact => s.DamageParent(fact.ResolutionId) == parent)
                         .Select(fact => fact.TargetSeat).SequenceEqual([2, 3])),
                    "Each actual rescued exit owns one original skill frame and two native enemy damage requests; source is the surviving holder, never the rescuer or a source-less substitute. " + s.Details());
                Need(s.Events<DyingResolvedEvent>().Count(fact => fact.VictimSeat == 0 && fact.Survived) == occurrence &&
                     s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.SkillId == skill && fact.Activated && fact.Completed) == occurrence &&
                     s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.SkillId == Watch && fact.Activated && fact.Completed) == occurrence * 2 &&
                     s.Events<PublicTeamTargetsBoundEvent>().Count(fact => frames.Contains(fact.ProgramFrameId) &&
                         fact.OwnerSeat == 0 && fact.Kind == SkillProgramTargetKind.PublicTeamEnemies && fact.Count == 2 && fact.TargetSeatsKey == "2,3") == occurrence &&
                     s.Events<DyingResponseEvent>().Count(fact => fact.ResponderSeat == 1 && fact.UsedPeach && fact.PeachCardId == peach) == 1 &&
                     s.Events<DamageRequestedEvent>().Count(fact => fact.SourceSeat == 2 && fact.TargetSeat == 0) == occurrence &&
                     s.Game.CardMovements.Count(move => move.CardId == peach && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing) == 1 &&
                     s.Game.CardMovements.Count(move => move.CardId == peach && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1 &&
                     s.Game.ResolutionStack.All(frame => frame is not ProgramSkillFrame and not DyingFrame),
                    "The exact physical Peach rescues once, both enemies' actual draw observers finish, and the original attack and skill return before Play; a later genuine rescue repeats without a per-turn limit. " + s.Details());
            }
            s.BeginFall("fall-one");
            s.Until(() => !s.View(0).IsAlive);
            Need(s.Events<DyingResolvedEvent>().Count(fact => fact.VictimSeat == 0 && !fact.Survived) == 1 &&
                 s.Events<ProgramBindingStartedEvent>().Count(fact => fact.SkillId == skill) == 2 &&
                 Enumerable.Range(2, 2).All(seat => s.View(seat).Hp == enemyHp[seat - 2] - 2 * amount),
                "A real fatal third Dying without a rescue does not create another survived-Dying candidate or enemy hit.");
        }
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        private readonly Dictionary<long, long> _damageParents = [];
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => Game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public long? DamageParent(long id) => _damageParents.TryGetValue(id, out var parent) ? parent : null;
        public string Details() => JsonSerializer.Serialize(new { players = Game.CreateSnapshot(0).Players,
            dying = Events<DyingResolvedEvent>(), damage = Events<DamageRequestedEvent>(), bindings = Events<ProgramBindingResolvedEvent>() });
        public void Accept(GameCommand command)
        {
            var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "A native Yongzhan command failed.");
            // The real after-damage draw children keep each DamageFrame on its
            // original program parent. Read that typed edge before it returns;
            // DamageRequested names this child, not its program owner.
            foreach (var damage in Game.ResolutionStack.OfType<DamageFrame>())
                _damageParents[damage.Id] = damage.ParentFrameId;
        }
        public void BeginFall(string activation) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], [2],
            Game.Revision, P!.PromptId));
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 220; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is
                        "pass" or "take-damage" or "let-die" or "no-nullification");
                    Need(choice is not null, "Unexpected real Yongzhan choice: " + JsonSerializer.Serialize(prompt));
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The bounded real Yongzhan flow did not reach its original boundary. " + Details());
        }
        public static Scene Start(string skill)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheYongzhanPackage(), new Fixture());
            var setup = new ShanheBattleSetup(71, 1, "fixture:yongzhan", Owner, 4, 0, 0, 0, 0, [],
                [new(CardKind.Peach, Suit.Heart, 6)], [],
                [new(Enemy) { StartingHand = [] }, new(Enemy) { StartingHand = [] }],
                [new(Ally) { StartingHand = [new(CardKind.Peach, Suit.Heart, 2), new(CardKind.Peach, Suit.Diamond, 3)] }],
                new string('d', 64)) { TacticSkillIds = [skill] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand());
            scene.Until(() => scene.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:yongzhan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"fall-four","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":4}]},
                    {"id":"fall-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"real-hit-child","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,
                    "effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Watch, Quiet }.ToDictionary(id => id, _ => new { name = "勇战原生救援", description = "真实濒死、桃与伤害孩子" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "勇战持有者", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Ally, "实际同行", "supporter", Quiet, "shu", 6));
            builder.AddGeneral(new(Enemy, "实际敌军", "supporter", Watch, "wei", 12) { AdditionalSkillIds = [Quiet] });
            builder.AddDeck(new(Deck, "实际摸牌实体", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "勇战四人阵", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
