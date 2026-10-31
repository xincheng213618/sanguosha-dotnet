using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheChilianCompanionChecks
{
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
}
