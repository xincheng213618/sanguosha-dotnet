using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheMobilizationChecks
{
    private const string Owner = "fixture:mobilization-owner", Ally = "fixture:mobilization-ally", Enemy = "fixture:mobilization-enemy";
    private const string Driver = "fixture:mobilization-driver", Watch = "fixture:mobilization-watch", Quiet = "fixture:mobilization-quiet";
    private const string Growth = "fixture:mobilization-opening-growth";
    private const string Deck = "fixture:mobilization-deck";

    public static void DeadAlliesReviveAtRealNewRoundAndRetainNaturalTurnsWithoutCards()
    {
        var s = Scene.Start(); s.Play();
        var originalCards = s.V(1).Hand.Concat(s.V(1).Equipment).Concat(s.V(2).Hand).Select(card => card.Id).ToArray();
        Need(originalCards.Length >= 3, "The real opening allies own original hand and equipment entities before their deaths.");
        s.Use("states", [1]); s.Use("states", [2]);
        Need(s.V(1).MaxHp == 7, "The first ally has genuinely increased its current maximum health before death.");
        foreach (var seat in new[] { 1, 2, 3 }) s.Use("kill", [seat]);
        Need(new[] { 1, 2, 3 }.All(seat => !s.V(seat).IsAlive) && s.Events<PlayerDiedEvent>().Length == 3,
            "Two public teammates and an enemy complete native deaths while the holder and the last enemy stay alive.");
        s.End(); s.Play();
        Need(s.Round == 2 && !s.V(1).IsAlive && !s.V(2).IsAlive && s.Events<ShanheMobilizationRevivedEvent>().Length == 0,
            "Without Mobilization the old allied corpses remain dead through a real new round.");
        s.Use("obtain", []);
        s.Use("extra", []);
        s.End(); s.Play();
        Need(s.Round == 2 && !s.V(1).IsAlive && !s.V(2).IsAlive && s.Events<ShanheMobilizationIssuedEvent>().Length == 0,
            "Acquiring the exact program during battle is immediate, but a genuine extra turn is not the next global round.");
        s.End();
        long ownerFrame = 0;
        for (var index = 0; index < 2; index++)
        {
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
            var frame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == StandardShanheMobilizationPackage.SkillId);
            if (index == 0) ownerFrame = frame.Id;
            Need(s.Round == 3 && frame.Id == ownerFrame && frame.ShanheMobilization is
                    { RoundNumber: 3, StatePublished: true } receipt && receipt.TargetIndex == index && receipt.TargetSeats.SequenceEqual([1, 2]) &&
                 s.V(index + 1) is { IsAlive: true, IsFaceDown: false, IsChained: false, HasAlcoholEffect: false, HandCount: 0 } revived &&
                 revived.Hp == revived.MaxHp && revived.Equipment.Count == 0 &&
                 (index != 0 || !s.V(2).IsAlive),
                "Each full-current-HP, face-up, unchained and sober teammate is restored empty before its actual state child returns to the original round owner frame.");
            s.AnswerContinue();
        }
        s.Play();
        Need(s.Events<ProgramBindingResolvedEvent>().Any(item => item.FrameId == ownerFrame && item.Activated && item.Completed) &&
             !s.V(3).IsAlive && s.V(0).IsAlive && s.V(4).IsAlive &&
             s.G.CreateCardZoneDiagnostics().Where(card => originalCards.Contains(card.CardId)).All(card => card.Location == CardLocation.DiscardPile) &&
             !s.G.CardMovements.Any(move => move.To.Zone == CardZoneKind.Hand && move.To.OwnerSeat is 1 or 2 &&
                 move.Sequence > s.G.CardMovements.Where(item => originalCards.Contains(item.CardId)).Max(item => item.Sequence)),
            "Only the dead allies revive; original death-cleanup entities stay discarded and Mobilization creates no initial or extra hand cards.");
        var turns = s.Events<TurnStartedEvent>().Length;
        s.End(); s.Play();
        var natural = s.Events<TurnStartedEvent>().Skip(turns).Select(turn => turn.ActorSeat).ToArray();
        Need(s.Round == 4 && Array.IndexOf(natural, 1) >= 0 && Array.IndexOf(natural, 2) > Array.IndexOf(natural, 1),
            "Both revived seats receive their actual ordered natural turns during the same third round before the fourth begins.");
        s.Use("states", [1]); s.Use("kill", [1]);
        Need(!s.V(1).IsAlive && s.Events<ShanheMobilizationRevivedEvent>().Length == 2,
            "A second completed allied death does not revive during the holder's current turn.");
        s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        Need(s.Round == 5 && s.V(1) is { IsAlive: true, Hp: 7, MaxHp: 7, HandCount: 0,
                IsFaceDown: false, IsChained: false, HasAlcoholEffect: false },
            "The same ally revives again at the next real round without a once-only quota or refill.");
        s.AnswerContinue(); s.Play();
        Need(s.Events<ShanheMobilizationRevivedEvent>().Select(item => (item.TargetSeat, item.RoundNumber)).SequenceEqual([(1, 3), (2, 3), (1, 5)]),
            "Repeated deaths produce exactly their three native revival facts and never include the holder or the dead enemy.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.PlayerCount).Select(seat => G.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public int Round => Events<RoundStartedEvent>().Last().RoundNumber;
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "A native Mobilization command failed."); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 320 && !goal(); step++)
            {
                Need(G.State.Winner == Winner.None, "The native Mobilization fixture ended before its intended round boundary.");
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is
                        "let-die" or "pass" or "take-damage" or "no-nullification");
                    Need(choice is not null, "Unexpected native Mobilization prompt: " + JsonSerializer.Serialize(prompt));
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(goal(), "The bounded native Mobilization flow did not reach its original boundary.");
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string id, int[] targets)
        { Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId)); Play(); }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void AnswerContinue()
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId,
            p.Choices.First(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, G.Revision)); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheMobilizationPackage(), new Fixture());
            var setup = new ShanheBattleSetup(71, 1, "mobilization-native", Owner, 10, 0, 0, 0, 0, [], [], [],
                [new(Enemy) { StartingHand = [] }, new(Enemy) { StartingHand = [] }],
                [new(Ally, MaxHpBonus: 2) { StartingHand = [new(CardKind.Dodge, Suit.Club, 2)],
                    StartingEquipment = [new(CardKind.QinggangSword, Suit.Spade, 6)] },
                 new(Ally, MaxHpBonus: 1) { StartingHand = [new(CardKind.Dodge, Suit.Club, 3)] }], new string('a', 64));
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 48
            }, registry);
            var s = new Scene(game); s.Accept(new StartGameCommand()); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobilization-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var sword = CardCatalog.Get(CardKind.QinggangSword);
            builder.AddCard(new("fixture:mobilization-sword", sword.DisplayName, sword.CategoryName, sword.Description, CardKind.QinggangSword));
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"states","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"setFaceState","target":"selectedTarget","faceDown":true},{"op":"setChainedState","target":"selectedTarget","chained":true}]},
                    {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"loseHp","target":"selectedTarget","amount":20}]},
                    {"id":"obtain","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"grantSkills","target":"owner","skillIds":["{{StandardShanheMobilizationPackage.SkillId}}"]}]},
                    {"id":"extra","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"pendExtraTurn","target":"owner"}]}
                  ]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"face-up-child","window":"characterTurnedFaceUp","subject":"any","optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{Growth}}","revision":1,"triggers":[{"id":"opening-growth","window":"gameStarting","subject":"owner","optional":false,
                    "effects":[{"op":"changeMaximumHp","target":"owner","amount":1}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    { [Driver] = new { name = "动员令原生驱动", description = "真实死亡、授技及自然轮次" },
                      [Quiet] = new { name = "静守", description = "跳过出牌" },
                      [Growth] = new { name = "开局活力", description = "真实开局增加自身1点体力上限" },
                      [Watch] = new { name = "复活状态观察", description = "实际正面状态孩子",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
                { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "动员令持有者", "supporter", Driver, "shu", 10) { AdditionalSkillIds = [Watch] });
            builder.AddGeneral(new(Ally, "原位队友", "supporter", Quiet, "shu", 4) { AdditionalSkillIds = [Growth] });
            builder.AddGeneral(new(Enemy, "真实敌军", "supporter", Quiet, "wei", 6));
            builder.AddDeck(new(Deck, "零正常摸牌的真实牌堆", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "动员令真实五席", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 3, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
}
