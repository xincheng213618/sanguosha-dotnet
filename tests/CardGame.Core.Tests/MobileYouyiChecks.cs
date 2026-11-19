using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileYouyiChecks
{
    private const string Youyi = "mobile-god:youyi", Jinzhi = "wushuang:skill-3252";
    private const string Driver = "fixture:mobile-youyi-driver", Mode = "identity:mobile-youyi-test", Deck = "fixture:mobile-youyi-deck";
    private const string JinzhiDiscard = "skill-program.wushuang.jinzhi.discard", Deposit = "skill.mobile-shen-hua-tuo.youyi.deposit";
    private static readonly string[] Generals =
        ["fixture:mobile-youyi-owner", "fixture:mobile-youyi-payer", "fixture:mobile-youyi-peer-2", "fixture:mobile-youyi-peer-3"];

    public static void CollectsActualPhaseDiscardWithoutInvocationPhase()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var play = Next(game);
        var earlierCard = game.CreateSnapshot(0).Players[0].Hand.First().Id;
        Submit(game, new UseProgramSkillCommand(0, Driver, "discard-before-phase", [earlierCard], [], game.Revision, play.PromptId));
        play = Next(game);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        Answer(game, choice => choice.Targets.SequenceEqual([1]));

        var response = Next(game);
        Need(response is { PlayerSeat: 1, Kind: DecisionKind.RespondDodge } && game.State.Phase == TurnPhase.Discard,
            "The foreign player's response must run inside the actual Discard phase.");
        var paid = Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "extended-view-as" &&
            choice.Parameters.GetValueOrDefault("conversion-skill-id") == Jinzhi && choice.Cards.Count == 1).Cards.Single();
        var offer = Next(game);
        var phase = game.Events.Select(e => e.Payload).OfType<ActualDiscardRecoveryPhaseStartedEvent>().Last(e =>
            e.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Discard, ActorSeat: 0 });
        var payment = game.CardMovements.Single(move => move.CardId == paid && move.Reason.Value == JinzhiDiscard);
        Need(offer is { PlayerSeat: 0, SkillPrompt.SkillId: Youyi } &&
            payment.Sequence > phase.MovementSequenceBefore && payment.To == CardLocation.DiscardPile &&
            game.Events.Select(e => e.Payload).OfType<NativeLogicalDiscardInvocationRecordedEvent>()
                .Any(e => e.Reason.Value == JinzhiDiscard && e.ExecutorSeat == 1 && e.Phase is null),
            "Youyi must cover the foreign player's completed genuine discard even though its legacy invocation has no phase token.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Command replay must restore the same Youyi offer in every seat view.");

        var expected = game.CardMovements.Where(move => move.Reason == CardMoveReasons.HandLimitDiscard && move.From == CardLocation.Hand(0))
            .Append(payment).OrderBy(move => move.Sequence).Select(move => move.CardId).ToArray();
        foreach (var run in new[] { game, restored })
        {
            Answer(run, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Next(run);
            var deposits = run.CardMovements.Where(move => move.Reason.Value == Deposit).ToArray();
            Need(deposits.Select(move => move.CardId).SequenceEqual(expected) &&
                deposits.All(move => move.From == CardLocation.DiscardPile && move.To == CardLocation.Benevolence) &&
                run.CreateCardZoneDiagnostics().Single(card => card.CardId == earlierCard).Location == CardLocation.DiscardPile,
                "Youyi must store the actual phase's surviving discard entries in order, including the foreign payment and excluding the earlier discard.");
        }
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 150; step++)
        {
            for (var seat = 0; seat < game.PlayerCount; seat++)
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Youyi did not reach the next public command boundary.");
    }

    private static PromptChoice Answer(GameEngine game, Func<PromptChoice, bool> choose)
    {
        var prompt = Next(game);
        var choice = prompt.Choices.FirstOrDefault(choose) ?? throw new InvalidOperationException(JsonSerializer.Serialize(prompt));
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
        return choice;
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Youyi fixture command rejected.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-youyi", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            AddSkill(builder, CharacterFixtureContent.EmbeddedProgramBundle("mobile-shen-hua-tuo"), Youyi);
            AddSkill(builder, CharacterFixtureContent.EmbeddedProgramBundle("wushuang-skills-a"), Jinzhi);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,
                    "activations":[{"id":"discard-before-phase","minCards":1,"maxCards":1,"minTargets":0,"maxTargets":0,
                       "sourceZones":["hand"],"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"discardSelected","target":"owner","amount":1}]}],
                    "triggers":[{"id":"slash-during-discard","window":"discardPhaseStarting","subject":"owner","turnOwnerScope":"own",
                       "optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                       {"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash",
                        "targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "弃牌阶段夹具", description = "在出牌阶段弃牌并于弃牌阶段使用【杀】。" } } });
            AddSkill(builder, SkillProgramCatalog.Load(rules, presentation), Driver);
            builder.AddGeneral(new(Generals[0], "游医角色", "supporter", Youyi, "wei", 2, [Driver]));
            builder.AddGeneral(new(Generals[1], "矜贽角色", "supporter", Jinzhi, "wu", 4));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定装备牌", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 60)
                .Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "游医阶段场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }

        private static void AddSkill(IContentRegistryBuilder builder, SkillProgramCatalog bundle, string skill)
        {
            var presentation = bundle.Presentations[skill];
            builder.AddSkill(new(skill, presentation.Name, presentation.Description)
                { Program = bundle.Programs[skill], ProgramPresentation = presentation });
        }
    }
}
