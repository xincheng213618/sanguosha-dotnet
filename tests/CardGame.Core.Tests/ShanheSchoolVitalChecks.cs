using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSchoolVitalChecks
{
    private const string Owner = "fixture:school-vital-owner", Peer = "fixture:school-vital-peer";
    private const string Driver = "fixture:school-vital-driver", Quiet = "fixture:school-vital-quiet";
    private const string HealthWatch = "fixture:school-vital-health", RecoveryWatch = "fixture:school-vital-recovery";
    private const string Deck = "fixture:school-vital-deck";

    public static void OwnedDamageSchoolUsesNativeCardsSkillsAndChainLayer()
    {
        var s = Scene.Start(4);
        s.Card(CardKind.Slash, [1]); s.Card(CardKind.Duel, [1]); s.Use("hit", [1]);
        Need(s.Events<DamageAppliedEvent>().Select(e => (e.SourceSeat, e.TargetSeat, e.Amount))
                .SequenceEqual([(0, 1, 2), (0, 1, 2), (0, 1, 2)]),
            "A completed damage school grants the same native +1 layer to real Slash, real Duel and cardless skill damage.");
        s.Use("incoming", [2]); s.Use("source-less"); s.Use("lose-one");
        Need(s.Events<DamageAppliedEvent>().TakeLast(2).Select(e => (e.SourceSeat, e.TargetSeat, e.Amount, e.SourceLess))
                .SequenceEqual([(2, 0, 1, false), (0, 0, 1, true)]) && s.View().Hp == 1,
            "The owner's set does not increase another source's damage, source-less damage, or direct HP loss.");
        s.Card(CardKind.IronChain, [1, 2]); s.Card(CardKind.FireSlash, [1]);
        var modified = s.Events<ProgramCardDamageModifiedEvent>().Where(e => e.Source.SkillId == ShanheSchoolEffects.DamageSkillId).ToArray();
        Need(s.Events<DamageAppliedEvent>().TakeLast(2).Select(e => (e.TargetSeat, e.Amount, e.Nature))
                .SequenceEqual([(1, 2, DamageNature.Fire), (2, 3, DamageNature.Fire)]) &&
            s.Events<ChainedDamagePropagatedEvent>() is [{ FromSeat: 1, TargetSeat: 2, Amount: 2 }] &&
            modified.Length == 5 && modified.All(e => e.ModifiedAmount == e.BaseAmount + 1),
            "The damage school uses exactly one shared source modifier per genuine recipient, including the native propagated amount.");
    }

    public static void PreparationSchoolAdjustsHpWithoutRecoveryAndDrainsHealthChild()
    {
        var s = Scene.Start(5, 1);
        Need(s.View().Hp == s.View().MaxHp && s.Events<ShanheSchoolHpAdjustedEvent>() is [{ Before: 1, After: 4, ChangeId: not null }] &&
            s.Events<RecoveryAppliedEvent>().Length == 0 && s.Starts(HealthWatch) == 1 && s.Starts(RecoveryWatch) == 0,
            "Preparation adjusts HP to its maximum through a native setting change, resolves after-health children, and never emits a recovery. " + s.HealthFacts());
        s.Use("lose-one"); s.Use("heal-one");
        Need(s.Events<RecoveryAppliedEvent>() is [{ TargetSeat: 0, Amount: 1 }] && s.Starts(RecoveryWatch) == 1,
            "The same live after-recovery observer still triggers for genuine healing, proving adjustment deliberately bypassed it.");
        s.Use("lose-one"); s.NextOwnPlay();
        Need(s.View().Hp == 4 && s.Events<ShanheSchoolHpAdjustedEvent>().Select(e => (e.Before, e.After))
                .SequenceEqual([(1, 4), (3, 4)]) && s.Events<RecoveryAppliedEvent>().Length == 1,
            "The next genuine preparation repeats the adjustment once without replacing or duplicating an earlier recovery.");
    }

    public static void DyingSchoolRecoversToOneOnlyOncePerActualTurn()
    {
        var s = Scene.Start(7);
        s.Use("lose-six");
        Need(s.View().Hp == 1 && s.Starts(ShanheSchoolEffects.DyingSkillId) == 1 &&
            s.Events<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 0 && e.Amount == 3 && e.RemainingHp == 1),
            "The first overkill dying occurrence heals from negative two to exactly one, rather than adding only one HP. " + s.HealthFacts());
        s.Use("heal-four");
        var peaches = s.View().Hand.Count(c => c.Kind == CardKind.Peach);
        s.Use("lose-six", rescueWithPeach: true);
        Need(s.View().Hp == 1 && s.Starts(ShanheSchoolEffects.DyingSkillId) == 1 &&
            s.View().Hand.Count(c => c.Kind == CardKind.Peach) == peaches - 3,
            "A second dying occurrence in the same actual turn needs three genuine physical Peaches and cannot repeat the school recovery.");
        s.NextOwnPlay(); s.Use("heal-four"); s.Use("lose-six");
        Need(s.View().Hp == 1 && s.Starts(ShanheSchoolEffects.DyingSkillId) == 2,
            "The first dying occurrence in a later actual turn receives a fresh school recovery through the native turn usage ledger.");
    }

    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        private GameEngine G { get; } = game;
        private PendingDecision? Pending => G.PendingDecision;
        public PlayerSnapshot View() => G.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int Starts(string skillId) => Events<ProgramBindingStartedEvent>().Count(e => e.SkillId == skillId);
        public string HealthFacts() => JsonSerializer.Serialize(new
        {
            View().Hp, View().MaxHp, adjusted = Events<ShanheSchoolHpAdjustedEvent>(),
            recoveries = Events<RecoveryAppliedEvent>(),
            triggers = Events<ProgramBindingStartedEvent>().Select(e => new { e.SkillId, e.BindingId, e.Window }),
            losses = Events<ProgramSkillHpLostEvent>()
        });
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native school command."); }
        private void ReachPlay(bool rescueWithPeach = false)
        {
            for (var step = 0; step < 200; step++)
            {
                if (Pending is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(G.State.Winner == Winner.None, "The bounded school fixture ended before its native Play boundary.");
                if (Pending is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Kind == DecisionKind.RescueDying && rescueWithPeach
                        ? prompt.Choices.First(c => c.Parameters.GetValueOrDefault("response") == "peach")
                        : prompt.Kind is DecisionKind.RescueDying or DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification
                            ? prompt.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die")
                            : prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native school Play boundary.");
        }
        public void Card(CardKind kind, int[] targets)
        {
            var card = View().Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card.Id, targets, G.Revision, Pending!.PromptId, action.PlayedCardKind, action.TargetCardId));
            ReachPlay();
        }
        public void Use(string activation, int[]? targets = null, bool rescueWithPeach = false)
        {
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, Pending!.PromptId));
            ReachPlay(rescueWithPeach);
        }
        public void NextOwnPlay()
        {
            Accept(new EndPlayPhaseCommand(0, G.Revision, Pending!.PromptId)); ReachPlay();
        }

        public static Scene Start(int schoolId, int hp = 4)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheSchoolVitalPackage(), new Fixture());
            var school = StandardShanheCampaignCatalog.CurrentSchools.Single(s => s.Id == schoolId);
            var components = school.Requirements.SelectMany((count, index) => Enumerable.Range(0, count).Select(n =>
                new ShanheRewardDefinition($"component:{index}:{n}", "实际持有的套装组件")
                { Rarity = index + 1, CampaignEconomy = new(ReplacedEquipmentGold: 1), SchoolMembership = new(50000 + index * 10 + n, schoolId, index + 1) })).ToArray();
            var carried = components.Take(6).Select(r => r.Id).ToArray();
            var last = components.Length > 6 ? components.Last().Id : null;
            var battle = new ShanheNodeDefinition("battle", "套装实战", ShanheNodeKind.Battle, [],
                [new("guard", "守军", Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray())]);
            var nodes = last is null ? new[] { battle } :
                [new ShanheNodeDefinition("component", "获得最后组件", ShanheNodeKind.Event, ["battle"],
                    Choices: [new("take", "领取", RewardId: last)]), battle];
            var definition = new ShanheCampaignDefinition("fixture:school-vital", "套装真实持有", [Owner],
                [last is null ? "battle" : "component"], nodes, components) { Schools = [school] };
            var session = ShanheCampaignSession.Create(definition, new(83, Owner)
            { UseCodexRewardPool = true, UnlockedRewardIds = components.Select(r => r.Id).ToArray(), EquippedTacticIds = carried }, _ => registry);
            if (last is not null)
            {
                var enter = session.Submit(new MoveShanheNodeCommand("component", session.Revision)); Need(enter.Accepted, enter.Error ?? "Missing component event.");
                var take = session.Submit(new ChooseShanheNodeOptionCommand("take", null, session.Revision)); Need(take.Accepted, take.Error ?? "Missing component acquisition.");
            }
            var start = session.Submit(new MoveShanheNodeCommand("battle", session.Revision)); Need(start.Accepted, start.Error ?? "Missing battle.");
            var setup = session.PendingBattleSetup!;
            Need(session.GetSchoolProgress().Single().IsComplete && setup.TacticSkillIds.Contains(ShanheSchoolEffects.SkillId(school)!),
                "The battle must derive its set program from an actually acquired exact source recipe.");
            setup = setup with { Hp = hp, StartingCards = [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Duel, Suit.Club, 8),
                new(CardKind.IronChain, Suit.Club, 10), new(CardKind.FireSlash, Suit.Heart, 9),
                .. Enumerable.Range(0, 6).Select(_ => new ShanheStartingCard(CardKind.Peach, Suit.Heart, 3))] };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 9, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.ReachPlay(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:school-vital", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"slash-limit","query":"slashLimit","operation":"add","value":10,"priority":0}],"activations":[
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},
                    {"id":"lose-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
                    {"id":"lose-six","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":6}]},
                    {"id":"heal-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"owner","amount":1}]},
                    {"id":"heal-four","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"owner","amount":4}]},
                    {"id":"source-less","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"receiveOwnerDamage","target":"owner","amount":1}]}]},
                  {"id":"{{HealthWatch}}","revision":1,"triggers":[{"id":"after-setting","window":"afterHealthChanged","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{RecoveryWatch}}","revision":1,"triggers":[{"id":"after-recovery","window":"afterHpRecovered","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, HealthWatch, RecoveryWatch, Quiet }.ToDictionary(id => id, _ => new { name = "套装原生流程驱动", description = "实际伤害、体力变化和濒死" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "套装使用者", "supporter", Driver, "qun", 4) { AdditionalSkillIds = [HealthWatch, RecoveryWatch] });
            builder.AddGeneral(new(Peer, "固定敌方", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "实体闪", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河图套装实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
