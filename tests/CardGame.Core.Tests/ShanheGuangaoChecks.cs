using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGuangaoChecks
{
    public static void LegalExtraTargetsAndEvenHandNullificationRecoverCold()
    {
        foreach (var even in new[] { true, false })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(71, 1, "guangao", "fixture:guangao-owner", 4, 0, 0, 0, 0, [],
                even ? [new(CardKind.Slash, Suit.Spade, 7)] : [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Club, 8)],
                [new(CardKind.QinggangSword, Suit.Spade, 6)],
                [new("fixture:guangao-peer") { StartingHand = [] }, new("fixture:guangao-peer") { StartingHand = [] }],
                [new("fixture:guangao-peer") { StartingHand = [] }], new string('g', 64));
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry);
            Submit(game, new StartGameCommand()); Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
            var slash = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == CardKind.Slash);
            Submit(game, new PlayCardCommand(0, slash.Id, [2], game.Revision, game.PendingDecision!.PromptId));
            Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("stage") == "target"));
            Need(game.PendingDecision!.Choices.Where(choice => choice.Targets.Count > 0).All(choice => !choice.Targets.Contains(0) && !choice.Targets.Contains(2)) &&
                game.PendingDecision.Choices.Any(choice => choice.Targets.SequenceEqual([3])),
                "Guangao offers only living legal extra targets, never the Slash user or its accepted original target.");
            game = Cold(game, registry); Answer(game, choice => choice.Targets.SequenceEqual([3]));
            if (even)
            {
                Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("stage") == "nullify"));
                Need(game.CardMovements.Count(movement => movement.To == CardLocation.Hand(0) && movement.From == CardLocation.DrawPile &&
                    movement.Reason.Value == "shanhe.companion.guangao.draw") == 1,
                    "Even hand count is captured after actual Slash payment and draws exactly one real native card.");
                game = Cold(game, registry); Answer(game, choice => choice.Targets.SequenceEqual([2]));
            }
            Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
            var players = game.CreateSnapshot(0).Players;
            Need(players[2].Hp == (even ? 10 : 9) && players[3].Hp == 9 &&
                game.CardMovements.Count(movement => movement.Reason.Value == "shanhe.companion.guangao.draw") == (even ? 1 : 0),
                "The added target resolves a real Slash; only the even-hand activation draws and may nullify an original target, while odd hand count resolves both targets without that benefit.");
            _ = Cold(game, registry);
        }
        IncomingSlashLetsActorChooseThenOwnerNullify();
    }

    private static void IncomingSlashLetsActorChooseThenOwnerNullify()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "guangao-incoming", "fixture:guangao-peer", 10, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Spade, 7)], [new(CardKind.QinggangSword, Suit.Spade, 6)],
            [new("fixture:guangao-peer") { StartingHand = [] }, new("fixture:guangao-peer") { StartingHand = [] }],
            [new("fixture:guangao-owner") { StartingHand = [] }], new string('g', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);
        Submit(game, new StartGameCommand()); Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
        var slash = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == CardKind.Slash);
        Submit(game, new PlayCardCommand(0, slash.Id, [2], game.Revision, game.PendingDecision!.PromptId));
        Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("stage") == "target"));
        Need(game.PendingDecision!.PlayerSeat == 0 && game.PendingDecision.Choices.Where(choice => choice.Targets.Count > 0)
            .All(choice => choice.Targets.SequenceEqual([1])), "The actual Slash user chooses whether to add the other Guangao owner.");
        game = Cold(game, registry); Answer(game, choice => choice.Targets.SequenceEqual([1]));
        Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == 0);
        var added = game.Events.Select(envelope => envelope.Payload).OfType<ProgramCardUseTargetAddedEvent>()
            .Single(fact => fact.SkillId == StandardShanheExpandedCompanionPackage.GuangaoSkillId);
        var skipped = game.Events.Select(envelope => envelope.Payload).OfType<CardEffectSkippedEvent>()
            .Where(fact => fact.ResolutionId == added.CardUseFrameId && fact.Reason == CardEffectSkipReason.SkillNullified).ToArray();
        var completed = game.Events.Select(envelope => envelope.Payload).OfType<ProgramBindingResolvedEvent>()
            .Where(fact => fact.SkillId == StandardShanheExpandedCompanionPackage.GuangaoSkillId).ToArray();
        var draws = game.CardMovements.Count(movement => movement.To == CardLocation.Hand(1) &&
            movement.Reason.Value == "shanhe.companion.guangao.draw");
        var players = game.CreateSnapshot(0).Players;
        Need(added.OwnerSeat == 1 && added.TargetSeat == 1 && skipped is [{ SourceSeat: 0, TargetSeat: 1 }] &&
            completed.Any(fact => fact.FrameId == added.FrameId && fact.OwnerSeat == 1 && fact.Activated && fact.Completed) &&
            draws == 1 && players[1].Hp == 4 && players[2].Hp == 9,
            "The other skill owner's AI resolves its own even-hand draw and nullifies only itself, preserving the original target's real Slash damage: " +
            System.Text.Json.JsonSerializer.Serialize(new { added, skipped, completed, draws, OwnerHp = players[1].Hp, OriginalHp = players[2].Hp }));
        _ = Cold(game, registry);
    }

    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Guangao native command rejected."); }
    private static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = game.PendingDecision!; Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    private static void Reach(GameEngine game, Func<PendingDecision, bool> stop)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.PendingDecision is { } prompt && stop(prompt)) return;
            if (game.PendingDecision is { Choices.Count: > 0 } choice && choice.PlayerSeat == 0 && choice.Kind != DecisionKind.PlayCard)
                Answer(game, candidate => candidate.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die" || candidate.Id == choice.Choices[0].Id);
            else Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Guangao did not reach its native choice or Play boundary: " +
            System.Text.Json.JsonSerializer.Serialize(new
            {
                game.State, game.PendingDecision,
                Stack = game.ResolutionStack.Select(frame => new { frame.Id, Type = frame.GetType().Name,
                    Program = frame is ProgramSkillFrame program ? new { program.OwnerSeat, program.SkillId, program.InstructionIndex,
                        program.ShanheGuangao, program.NativeDraw, program.PendingMovementContinuation } : null }),
                Recent = game.Events.TakeLast(8).Select(envelope => envelope.Payload)
            }));
    }
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
            "Cold replay preserves Guangao's accepted extra target, once-only draw and pending nullification choice.");
        return restored;
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shanhe-guangao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var assembly = typeof(StandardShanheExpandedCompanionPackage).Assembly;
            using var rules = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shanhe-companion-guangao.rules.json")!);
            using var presentation = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shanhe-companion-guangao.presentation.json")!);
            var catalog = SkillProgramCatalog.Load(rules.ReadToEnd(), presentation.ReadToEnd());
            var shown = catalog.Presentations[StandardShanheExpandedCompanionPackage.GuangaoSkillId];
            builder.AddSkill(new(StandardShanheExpandedCompanionPackage.GuangaoSkillId, shown.Name, shown.Description)
            { Program = catalog.Programs[StandardShanheExpandedCompanionPackage.GuangaoSkillId], ProgramPresentation = shown });
            builder.AddGeneral(new("fixture:guangao-owner", "犷骜", "", StandardShanheExpandedCompanionPackage.GuangaoSkillId, "qun", 4));
            builder.AddGeneral(new("fixture:guangao-peer", "固定对手", "", null, "qun", 10));
            builder.AddDeck(new("fixture:guangao-deck", "固定实体桃牌堆", 0, 2, [new("standard:peach", 80)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "犷骜原生短局", 4, 4, new Dictionary<string, int>(), "fixture:guangao-deck",
                GeneralPoolIds: ["fixture:guangao-owner", "fixture:guangao-peer"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
