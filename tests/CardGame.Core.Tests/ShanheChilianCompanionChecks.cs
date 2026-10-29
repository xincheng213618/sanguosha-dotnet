using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheChilianCompanionChecks
{
    private const string Driver = "fixture:chilian-driver", Ally = "fixture:chilian-ally", Low = "fixture:chilian-low", Equal = "fixture:chilian-equal";
    private const string Mode = ShanheBattleSetup.BattleModeId, Skill = StandardShanheChilianCompanionPackage.LiangyinId;

    public static void ChainedDamageJiemingStorageAndLiangyinRetainFrozenCursorCold()
    {
        // The reported first-turn chain: Jieming discards adjacent Slashes,
        // Chunlao stores them, and Liangyin observes each outside-game move.
        var setup = new ShanheBattleSetup(595894275, 1, "source-city-5021", "boundary:xun-yu", 5, 2, 2, 1, 1,
            ["classic:anjian", "shanhe:rank-five-rebirth"], [], [],
            [new("shanhe:source-coalition-scout-2455-hell", 3, 7, 6, 4, 2, [])
                { InitialArmor = 7, ArmorRecoveryPerRound = 1, ArmorRecoveryCap = 5, ExtraDamage = 1,
                    SourceDifficultyAttributes = [] }],
            [new(StandardShanheChilianCompanionPackage.ChilianId)
                { StartingHand = [new(CardKind.Slash, Suit.Heart, 5), new(CardKind.Alcohol, Suit.Spade, 3)] }],
            new string('a', 64))
        {
            SourceDifficultyId = 25, ExtraSkillSlots = 1, DefaultGodFactionId = "qun", PlayerRank = 5,
            RebirthAvailable = true, StartingResourcesExpireOnUse = true, AttemptNumber = 1,
            TacticSkillIds = ["shanhe:ruoxi", "shanhe:current-kuangbao-2162",
                "shanhe:current-shenlong-baiwei-2143", "shanhe:current-pianzhuanjia-2117"]
        };
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null, ModeId = Mode,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ShanheBattle = setup,
            ConfirmOpeningHand = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false,
            AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 150
        }, registry);
        while (game.Revision < 25)
        {
            switch (game.Revision)
            {
                case 0: Accept(game, new StartGameCommand()); break;
                case 1: Answer(game, choice => choice.Id.Value == "setup.opening.confirm"); break;
                case 6: Use(69, [2]); break;
                case 9: Use(146, [0, 2]); break;
                case 12: Use(115, [2]); break;
                case 13: Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"); break;
                case 14: Answer(game, choice => choice.Targets.SequenceEqual([2])); break;
                default: Accept(game, new AdvanceOneStepCommand(game.Revision)); break;
            }
        }
        Need(game.ResolutionStack.OfType<DamageTriggerWindowFrame>().Any() &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Count() == 2 &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Last().SkillId == Skill,
            "The actual Liangyin child remains nested beneath the original damage and storage windows.");
        var observer = game.ResolutionStack.OfType<ProgramSkillFrame>().Last();
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last();
        Need(observer.WindowContext == window.Contexts![window.CandidateIndex],
            "Liangyin retains the exact issued movement context and frozen facts.");
        game = Cold(game, registry);
        Play(game);
        var stored = game.Events.Select(item => item.Payload).OfType<ProgramAdjacentDiscardStoredEvent>().ToArray();
        Need(stored.Length == 2 && stored.Select(item => item.CardId).Distinct().Count() == 2 &&
            !game.ResolutionStack.Any(frame => frame is DamageFrame or DamageTriggerWindowFrame or CardsMovedTriggerWindowFrame) &&
            !game.IsFaulted,
            "Both discarded Slashes store once and all nested children return to Play without faulting the damage cursor.");
        Cold(game, registry);

        void Use(int cardId, int[] targets) => Accept(game,
            new PlayCardCommand(0, cardId, targets, game.Revision, game.PendingDecision!.PromptId));
    }

    public static void OpeningBodyAndGlobalLiangyinUseActualMovementCold()
    {
        var (game, registry) = Start();
        var own = game.CreateSnapshot(0).Players[0];
        var printed = own.Skills?.Select(skill => skill.Id).ToArray() ?? [];
        Need(own.MaxHp == 4 && own.Hp == 4 && printed.Contains(StandardShanheChilianCompanionPackage.ChunlaoId) &&
            printed.Contains(Skill), "The actual four-HP female Qun opening body has the two screenshot skill editions.");
        Play(game);
        var ownerHand = game.CreateSnapshot(0).Players[0].HandCount;
        var highHand = game.CreateSnapshot(0).Players[1].HandCount;
        var lowHand = game.CreateSnapshot(0).Players[2].HandCount;
        Need(ownerHand == 2 && highHand == 4 && lowHand == 1 && game.CreateSnapshot(0).Players[3].HandCount == ownerHand,
            "The fixed physical opening has strict greater, strict fewer and equal-hand targets.");

        for (var occurrence = 0; occurrence < 2; occurrence++)
        {
            var prompt = game.PendingDecision!;
            Accept(game, new UseProgramSkillCommand(0, Driver, "foreign-deposit", [], [1], game.Revision, prompt.PromptId));
            Reach(game, p => p.SkillPrompt?.SkillId == Skill && IsActivation(p));
            game = Cold(game, registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(game, p => p.SkillPrompt?.SkillId == Skill && p.Choices.Any(choice => choice.Targets.Count == 1));
            Need(game.PendingDecision!.Choices.Where(choice => choice.Targets.Count == 1).All(choice => choice.Targets.SequenceEqual([1])),
                "Outbound movement offers only a strictly higher-hand role; equal hands and the owner are excluded.");
            game = Cold(game, registry); Answer(game, choice => choice.Targets.SequenceEqual([1]));
            Play(game);
            Accept(game, new UseProgramSkillCommand(0, Driver, "foreign-return", [], [1], game.Revision, game.PendingDecision!.PromptId));
            if (occurrence == 0)
            {
                Reach(game, p => p.SkillPrompt?.SkillId == Skill && IsActivation(p));
                game = Cold(game, registry);
                Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
                Reach(game, p => p.SkillPrompt?.SkillId == Skill && p.Choices.Any(choice => choice.Targets.Count == 1));
                Need(game.PendingDecision!.Choices.Where(choice => choice.Targets.Count == 1).All(choice => choice.Targets.SequenceEqual([2])),
                    "Returning a foreign outside-game card to hand offers only the strict lower-hand role.");
                game = Cold(game, registry); Answer(game, choice => choice.Targets.SequenceEqual([2]));
            }
            Play(game);
        }
        var after = game.CreateSnapshot(0).Players;
        Need(after[0].HandCount == ownerHand && after[1].HandCount == highHand + 2 && after[2].HandCount == lowHand - 1 &&
            after[3].HandCount == ownerHand && game.CardMovements.Count(move => move.From.Zone == CardZoneKind.Hand && move.From.OwnerSeat == 1 &&
                move.To.Zone == CardZoneKind.Authority && move.To.OwnerSeat == 0) == 2 &&
            game.CardMovements.Count(move => move.From.Zone == CardZoneKind.Authority && move.From.OwnerSeat == 0 &&
                move.To == CardLocation.Hand(1)) == 2,
            "Two actual foreign domain crossings in one turn each grant one draw, and an independent inbound choice pays the selected role's real discard; the owner never gains the newer version's joint benefit.");
        Cold(game, registry);
    }

    private static bool IsActivation(PendingDecision prompt) => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected Chilian command."); }
    private static void Answer(GameEngine game, Func<PromptChoice, bool> pick)
    { var prompt = game.PendingDecision!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(pick).Id, game.Revision)); }
    private static void Play(GameEngine game) => Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.Kind == DecisionKind.PlayCard);
    private static void Reach(GameEngine game, Func<PendingDecision, bool> stop)
    {
        for (var step = 0; step < 200; step++)
        {
            if (game.PendingDecision is { } prompt && stop(prompt)) return;
            if (game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard })
            {
                if (game.ResolutionStack.Count > 0) { Accept(game, new AdvanceOneStepCommand(game.Revision)); continue; }
                throw new InvalidOperationException("Chilian child returned to Play before the expected native boundary: " + JsonSerializer.Serialize(new
                { Moves = game.CardMovements.TakeLast(5), Hand = game.CreateSnapshot(0).Players.Select(player => player.HandCount), Events = game.Events.TakeLast(8).Select(e => e.Payload) }));
            }
            if (game.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } p)
            {
                var choice = p.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") ?? p.Choices[0];
                Accept(game, new AnswerPromptCommand(0, p.PromptId, choice.Id, game.Revision));
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Missing Chilian native boundary: " + JsonSerializer.Serialize(game.PendingDecision));
    }
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(JsonSerializer.Serialize(restored.CreateSnapshot(0)) == JsonSerializer.Serialize(game.CreateSnapshot(0)) &&
            GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == GameCheckpointJson.Serialize(game.CreateCheckpoint()),
            "The actual selected target, movement material and child return restore from the command journal.");
        return restored;
    }

    private static (GameEngine, ContentRegistry) Start()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(),
            new StandardShanheChilianCompanionPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "chilian-fixture", StandardShanheChilianCompanionPackage.ChilianId, 4, 0, 0, 0, 0,
            [Driver], [new(CardKind.Dodge), new(CardKind.Dodge)], [],
            [new(Low) { StartingHand = [new(CardKind.Dodge)] }, new(Equal) { StartingHand = [new(CardKind.Dodge), new(CardKind.Dodge)] }],
            [new(Ally) { StartingHand = [new(CardKind.Dodge), new(CardKind.Dodge), new(CardKind.Dodge), new(CardKind.Dodge)] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = Mode, ShanheBattle = setup, AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false,
            AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 6
        }, registry);
        Accept(game, new StartGameCommand());
        return (game, registry);
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-chilian", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"foreign-deposit","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLivingWithHand","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"selectedTarget","zones":["hand"],"amount":1,"resultBind":"foreign-card"},
                      {"op":"moveBoundCards","target":"owner","sourceBind":"foreign-card","destination":"ownerPersistentZone","destinationZone":"authority","awaitMovementTriggers":true}]},
                    {"id":"foreign-return","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"selectSourceCard","target":"owner","cardSource":"owner","zones":["authority"],"resultBind":"held-card"},
                      {"op":"moveBoundCards","target":"owner","sourceBind":"held-card","destination":"selectedTargetHand"}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "牌域驱动", description = "真实其他角色手牌移出再移入" } } }));
            builder.AddSkill(new(Driver, "牌域驱动", "真实移动") { Program = catalog.Programs[Driver] });
            foreach (var id in new[] { Ally, Low, Equal }) builder.AddGeneral(new(id, "固定角色", "", null, "wei", 4));
            builder.AddDeck(new("fixture:chilian-deck", "固定闪", 0, 0, [new("standard:dodge", 50)]));
            builder.AddMode(new(Mode, "赤炼机制", 4, 4, new Dictionary<string, int>(), "fixture:chilian-deck",
                GeneralPoolIds: [StandardShanheChilianCompanionPackage.ChilianId, Ally, Low, Equal], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
