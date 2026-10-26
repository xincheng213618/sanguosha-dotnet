using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheYingxiangChecks
{
    private const string Owner = "fixture:yingxiang-owner", Ally = "fixture:yingxiang-ally", Enemy = "fixture:yingxiang-enemy";
    private const string Driver = "fixture:yingxiang-driver", Skill = "shanhe:companion-yingxiang";

    public static void PhysicalGiftUseAndRoundLimitedQingjueResumeCold()
    {
        var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages:
            [new StandardShanheExpandedCompanionPackage(), new Fixture()]);
        var setup = new ShanheBattleSetup(71, 1, "yingxiang", Owner, 6, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Slash, Suit.Diamond, 8),
                new(CardKind.Dodge, Suit.Spade, 2), new(CardKind.Dodge, Suit.Spade, 3)], [], [new(Enemy) { StartingHand = [] }],
            [new(Ally) { StartingHand = [] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        Submit(new StartGameCommand()); Play();
        var slashes = game.CreateSnapshot(0).Players[0].Hand.Where(card => card.Kind == CardKind.Slash).Select(card => card.Id).ToArray();
        Submit(new UseProgramSkillCommand(0, Driver, "give", slashes, [2], game.Revision, game.PendingDecision!.PromptId)); Play();
        Need(Facts<ShanheYingxiangTaggedEvent>().Length == 2, "The real hand transfer marks both physical gifts by their native movement sequences.");
        Use("request", 2); Play();
        Need(Facts<ShanheYingxiangUsePaidEvent>() is [{ OwnerSeat: 0, UserSeat: 2 }] &&
            game.CardMovements.Count(move => move.Reason.Value == "skill-program.shanhe-yingxiang.draw") == 2 &&
            Facts<ShanheYingxiangQingjueStartedEvent>().Length == 0,
            "Actually using one marked Slash draws once for issuer and holder, without mistaking that payment for a non-use loss.");

        Use("strip", 2);
        Reach(() => game.PendingDecision?.SkillPrompt?.SkillId == Skill);
        Need(Facts<ShanheYingxiangQingjueStartedEvent>().Length == 1, "A real non-use batch losing the remaining marked card invokes Qingjue once.");
        Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "card" &&
            game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Id == choice.Cards.Single()).Suit == Suit.Spade);
        Cold();
        Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "card" &&
            game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Id == choice.Cards.Single()).Suit == Suit.Spade);
        Reach(() => game.PendingDecision?.SkillPrompt?.SkillId == Skill &&
            game.PendingDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "option"));
        Need(game.CardMovements.Count(move => move.Reason.Value == "skill-program.shanhe-yingxiang.qingjue-discard") == 2,
            "Two selected repeated-suit hand cards are discarded as one actual payment before receiving two choices.");
        Cold();
        Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "option" && choice.Parameters["value"] == "2");
        Reach(() => game.PendingDecision?.SkillPrompt?.SkillId == Skill &&
            game.PendingDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "option"));
        Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "option" && choice.Parameters["value"] == "1");
        Reach(() => game.PendingDecision?.SkillPrompt?.SkillId == Skill &&
            game.PendingDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "recipient"));
        Answer(choice => choice.Targets.SequenceEqual([2])); Play();
        Need(game.CardMovements.Count(move => move.Reason.Value == "skill-program.shanhe-yingxiang.qingjue-discard") == 2 &&
            game.CardMovements.Count(move => move.Reason.Value == "skill-program.shanhe-yingxiang.qingjue-give") == 2 &&
            game.CardMovements.Count(move => move.Reason.Value == "skill-program.shanhe-yingxiang.qingjue-gain") == 3 &&
            game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Suit).Distinct().Count() == 4,
            "Cold continuation never pays twice, gives the surviving actual discarded cards and obtains one real card of every missing suit.");
        Use("strip", 2); Play(); Cold();
        Need(Facts<ShanheYingxiangQingjueStartedEvent>().Length == 1,
            "A second genuine marked-card loss in the same round does not invoke Qingjue again.");
        return;

        T[] Facts<T>() => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        void Submit(GameCommand command)
        { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Yingxiang fixture command rejected."); }
        void Use(string activation, int target) =>
            Submit(new UseProgramSkillCommand(0, Driver, activation, [], [target], game.Revision, game.PendingDecision!.PromptId));
        void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = game.PendingDecision!; Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
        void Play() => Reach(() => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 180; step++)
            {
                if (condition()) return;
                if (game.PendingDecision is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard)
                        throw new InvalidOperationException("Yingxiang returned to play before its expected child: " + JsonSerializer.Serialize(game.ResolutionStack));
                    var choice = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") is "take-damage" or "pass") ?? prompt.Choices[0];
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
                }
                else Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The Yingxiang native child did not complete.");
        }
        void Cold()
        {
            var checkpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == checkpoint &&
                SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                "The private Qingjue selection and paid original frame restore exactly from the command journal.");
            game = restored;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-yingxiang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,"activations":[
                 {"id":"give","minCards":1,"maxCards":2,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                  "effects":[{"op":"giveSelected","target":"selectedTarget"}]},
                 {"id":"request","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                  "effects":[{"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"request"}]},
                 {"id":"strip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLivingWithHand","usesPerTurn":null,
                  "effects":[{"op":"selectOwnedCards","target":"selectedTarget","zones":["hand"],"amount":2,"resultBind":"stripped"},
                   {"op":"moveBoundCards","target":"owner","sourceBind":"stripped","destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "原生牌移动", description = "赠牌、请求出杀及真实弃牌" } } }));
            builder.AddSkill(new(Driver, "原生牌移动", "赠牌、请求出杀及真实弃牌") { Program = catalog.Programs[Driver] });
            builder.AddGeneral(new(Owner, "萦香拥有者", "", Skill, "qun", 6, [Driver]));
            builder.AddGeneral(new(Ally, "持牌队友", "", null, "shu", 6));
            builder.AddGeneral(new(Enemy, "敌军", "", null, "wei", 6));
            builder.AddDeck(new("fixture:yingxiang-deck", "四花色实体牌", 0, 0, [])
            { PhysicalCards = Enum.GetValues<Suit>().Where(suit => suit != Suit.None).SelectMany(suit =>
                Enumerable.Range(1, 10).Select(rank => new ContentDeckPhysicalCard("standard:dodge", suit, rank))).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "萦香机制", 3, 3, new Dictionary<string, int>(), "fixture:yingxiang-deck",
                GeneralPoolIds: [Owner, Ally, Enemy], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
