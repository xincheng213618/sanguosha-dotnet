using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheMuluChecks
{
    private const string Driver = "fixture:mulu-driver", Watch = "fixture:mulu-face-watch";
    private const string General = "fixture:mulu-player", Other = "fixture:mulu-other", Deck = "fixture:mulu-deck";

    public static void FourFollowersReviveOnlyDeadSummonerElephantsWithStateChildCold()
    {
        var scene = new Scene(); scene.Play();
        Need(scene.G.CreateSnapshot(0).Players.Count(player => player.TeamId == ShanheBattleSetup.EnemyTeamId &&
                player.GeneralId is StandardShanheMuluPackage.RageElephantId or StandardShanheMuluPackage.AngryElephantId) == 4,
            "The real opening roster contains the summoner and all four original followers without late generated seats.");
        foreach (var seat in new[] { 3, 4 }) { scene.Use("states", [seat]); scene.Play(); }
        scene.Use("weapon", [3]); scene.Play();
        var removed = scene.V(3).Hand.Concat(scene.V(3).Equipment).Select(card => card.Id).ToArray();
        foreach (var seat in new[] { 1, 3, 4, 7 }) { scene.Use("kill", [seat]); scene.Play(); }
        Need(new[] { 1, 3, 4, 7 }.All(seat => !scene.V(seat).IsAlive),
            "A friendly elephant, two enemy followers and an ordinary enemy really complete their separate deaths.");
        scene.Cold();
        scene.Accept(new EndPlayPhaseCommand(0, scene.G.Revision, scene.P!.PromptId));
        for (var index = 0; index < 2; index++)
        {
            scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Watch);
            var frame = scene.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == StandardShanheMuluPackage.SummonId);
            Need(frame.ShanheMuluRevival is { Stage: ShanheMuluRevivalStage.StateChildren } receipt &&
                receipt.TargetIndex == index && receipt.TargetSeats.SequenceEqual([3, 4]) &&
                scene.V(3 + index) is { IsAlive: true, Hp: 3, IsFaceDown: false, IsChained: false, HandCount: 0 } &&
                scene.V(3 + index).Equipment.Count == 0,
                "Each restored follower is full, face up, unchained and empty before its real state observer returns to the same summoner frame.");
            scene.Cold();
            scene.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        }
        scene.Reach(() => scene.Facts<ProgramBindingResolvedEvent>().Any(item =>
            item.SkillId == StandardShanheMuluPackage.SummonId && item.Completed));
        Need(!scene.V(1).IsAlive && !scene.V(7).IsAlive && scene.V(5).IsAlive && scene.V(6).IsAlive &&
            scene.Facts<ShanheMuluFollowerRevivedEvent>().Select(item => item.TargetSeat).SequenceEqual([3, 4]) &&
            scene.G.CreateCardZoneDiagnostics().Where(card => removed.Contains(card.CardId)).All(card => card.Location == CardLocation.DiscardPile) &&
            !scene.G.CardMovements.Any(move => move.To == CardLocation.Hand(3) && move.Sequence >
                scene.G.CardMovements.Where(move => move.From.OwnerSeat == 3 && move.To == CardLocation.DiscardPile).Max(move => move.Sequence)),
            "Only the two originally dead enemy followers revive once; ordinary allies, other-team elephants and removed physical cards retain their actual state.");
        scene.Cold();
    }

    public static void YangshengCountsOccurrencesCapsThreeAndReplacesEachOwnTurnCold()
    {
        var scene = new Scene(elephantPlayer: true); scene.Play();
        Need(scene.LastBonus().Amount == 0, "The first own turn has no earlier received-damage occurrences.");
        scene.Use("hurt-two"); scene.Play(); scene.Use("hurt-one"); scene.Play();
        scene.ExtraTurn();
        Need(scene.LastBonus() is { Amount: 2, RoundNumber: 1 },
            "Two received-damage occurrences totaling three HP freeze two, including before another own turn in the same round.");
        scene.Use("strike", [7]); scene.Play();
        Need(scene.Facts<DamageAppliedEvent>().Last(item => item.TargetSeat == 7).Amount == 3,
            "The frozen bonus applies to real standalone skill damage, not only card damage.");
        scene.Cold(); scene.Use("hurt-one"); scene.Play(); scene.ExtraTurn();
        Need(scene.LastBonus().Amount == 3, "A third occurrence increases the next frozen amount to its cap of three.");
        scene.Use("strike", [7]); scene.Play();
        Need(scene.Facts<DamageAppliedEvent>().Last(item => item.TargetSeat == 7).Amount == 4,
            "The capped bonus increases the next actual one-point damage to four.");
        scene.ExtraTurn();
        Need(scene.LastBonus().Amount == 3, "Another own turn replaces the old three-point layer instead of adding a second layer.");
        scene.Use("strike", [7]); scene.Play();
        Need(scene.Facts<DamageAppliedEvent>().Last(item => item.TargetSeat == 7).Amount == 4,
            "Repeated turn starts retain one frozen layer rather than accumulating six additional damage.");
        scene.Cold();
        scene.Accept(new EndPlayPhaseCommand(0, scene.G.Revision, scene.P!.PromptId)); scene.Play();
        Need(scene.LastBonus() is { RoundNumber: 2, Amount: 0 }, "The next round freezes a fresh zero when the owner has received no damage in that round.");
        scene.Use("strike", [7]); scene.Play();
        Need(scene.Facts<DamageAppliedEvent>().Last(item => item.TargetSeat == 7).Amount == 1,
            "The replaced zero removes the previous round's bonus from real damage.");
        scene.Cold();
    }

    private sealed class Scene
    {
        private readonly ContentRegistry _registry;
        public GameEngine G { get; private set; }
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public T[] Facts<T>() => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public ShanheElephantDamageFrozenEvent LastBonus() => Facts<ShanheElephantDamageFrozenEvent>().Last(item => item.Source.OwnerSeat == 0);
        public Scene(bool elephantPlayer = false)
        {
            _registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardShanheMuluPackage(), new Fixture());
            var setup = new ShanheBattleSetup(71, 1, "mulu", elephantPlayer ? StandardShanheMuluPackage.RageElephantId : General,
                elephantPlayer ? 23 : 12, elephantPlayer ? 20 : 0, 0, 0, 0,
                elephantPlayer ? [Driver] : [Watch], [], [],
                [new(StandardShanheMuluPackage.MuluId),
                 new(StandardShanheMuluPackage.RageElephantId), new(StandardShanheMuluPackage.AngryElephantId),
                 new(StandardShanheMuluPackage.RageElephantId), new(StandardShanheMuluPackage.AngryElephantId),
                 new(Other, MaxHpBonus: elephantPlayer ? 40 : 0)],
                [new(StandardShanheMuluPackage.RageElephantId)], new string('d', 64));
            G = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 8, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 24, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, _registry);
            Accept(new StartGameCommand());
        }
        public void Use(string activation, IReadOnlyList<int>? targets = null) =>
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId));
        public void ExtraTurn()
        { Use("extra"); Play(); Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId)); Play(); }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = P!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Mulu native command rejected."); }
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 500; step++)
            {
                if (condition()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.RescueDying && prompt.Choices.Any(choice => choice.Cards.Count == 0))
                    { Answer(choice => choice.Cards.Count == 0); continue; }
                    throw new InvalidOperationException("Unexpected Mulu human prompt: " + JsonSerializer.Serialize(prompt));
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Mulu check did not reach its native boundary.");
        }
        public void Cold()
        {
            var before = SnapshotJson.Serialize(G.CreateSnapshot(0));
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), _registry);
            Need(before == SnapshotJson.Serialize(G.CreateSnapshot(0)), "Cold replay preserves the summoner's paid target cursor and each native state child.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mulu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var definitions = new Dictionary<string, object>
            {
                [Driver] = new { name = "木鹿实战驱动", description = "真实失血、伤害、状态和额外回合" },
                [Watch] = new { name = "复活翻面观察", description = "复活后真实翻回正面的孩子选择",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"states","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"setFaceState","target":"selectedTarget","faceDown":true},{"op":"setChainedState","target":"selectedTarget","chained":true}]},
                    {"id":"weapon","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"placeNamedWeapon","target":"selectedTarget","outputKind":"qinggangSword"}]},
                    {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"loseHp","target":"selectedTarget","amount":20}]},
                    {"id":"hurt-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","amount":1}]},
                    {"id":"hurt-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","amount":2}]},
                    {"id":"strike","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"extra","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"pendExtraTurn","target":"owner"}]}
                  ]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"face-up-child","window":"characterTurnedFaceUp","subject":"any","optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"continued","options":[{"id":"continue"}]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = definitions }));
            foreach (var (id, program) in catalog.Programs)
            { var presentation = catalog.Presentations[id]; builder.AddSkill(new(id, presentation.Name, presentation.Description)
                { Program = program, ProgramPresentation = presentation }); }
            builder.AddGeneral(new(General, "木鹿检查玩家", "", Driver, "qun", 12));
            builder.AddGeneral(new(Other, "非随从敌军", "", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定闪", 4, 2, [new("standard:dodge", 100)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "木鹿实战", 8, 8, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [General, Other, .. StandardShanheMuluPackage.GeneralIds], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 6 }));
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
