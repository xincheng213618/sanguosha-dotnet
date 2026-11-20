using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class ClassicZhenJiLuoshenJudgmentEntryChecks
{
    private const string ZhenJi = "classic:zhen-ji", Luoshen = "classic:luoshen", Tiandu = "mobile:tiandu";
    private const string Reason = "skill.luoshen", Gain = "skill-program.mobile:tiandu.claimJudgmentCard";
    private const string Repeat = "skill-program.classic:luoshen.repeatJudgment";
    private const string Watch = "fixture:classic-luoshen-top", Peer = "fixture:classic-luoshen-peer";
    private const string Deck = "fixture:classic-luoshen-deck";

    internal static void LuoshenContinuesAfterTianduAndUsesTheNextJudgmentEntryCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var invoke = Reach(game, prompt => prompt.SkillPrompt?.SkillId == Luoshen);
        Answer(game, invoke, choice => Action(choice) == "activate");
        var tiandu = Reach(game, prompt => IsTiandu(game, prompt));
        var finalized = game.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Single();
        var first = Facts<JudgmentResolvedEvent>(game).Single(fact => fact.Reason == Reason);
        var cardId = first.CardId ?? throw new InvalidOperationException("The real first judgment must contain a card.");
        var firstEntry = game.CardMovements.Single(move => move.CardId == cardId && move.To == CardLocation.Judgment(0));
        Require(first is { TargetSeat: 0, Suit: Suit.Spade } && finalized.Judgment.JudgmentFrameId == first.ResolutionId &&
            finalized.Judgment.CardId == cardId && firstEntry.From == CardLocation.DrawPile,
            "The original classic Luoshen judgment really revealed a black card and offers formal Mobile Tiandu.");

        Answer(game, tiandu, choice => Action(choice) == "program-judgment-trigger-activate");
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Watch);
        var claim = Facts<ProgramJudgmentCardClaimedEvent>(game).Single();
        var actualGain = game.CardMovements.Single(move => move.CardId == cardId && move.Reason.Value == Gain);
        Require(claim.JudgmentFrameId == first.ResolutionId && claim.SkillId == Tiandu && claim.CardId == cardId &&
            actualGain.From == CardLocation.Judgment(0) && actualGain.To == CardLocation.Hand(0) &&
            actualGain.Sequence > firstEntry.Sequence,
            "The finalized claimant has already obtained the exact original card before the real gain child starts.");

        game = Cold(game, content);
        var moveToTop = Reach(game, prompt => prompt.SkillPrompt?.SkillId == Watch);
        Answer(game, moveToTop, choice => choice.Cards.SequenceEqual([cardId]));
        var repeat = Reach(game, prompt => prompt.Kind == DecisionKind.ProgramRepeatJudgment);
        var top = game.CardMovements.Last(move => move.CardId == cardId);
        Require(top.From == CardLocation.Hand(0) && top.To == CardLocation.DrawPile &&
            top.Sequence > actualGain.Sequence && !Facts<ProgramJudgmentCardClaimedEvent>(game).Any(fact => fact.SkillId == Luoshen),
            "The first native return keeps the finalized black result and offers another judgment without taking back the card now on top.");

        Answer(game, repeat, choice => Action(choice) == "continue");
        var secondTiandu = Reach(game, prompt => IsTiandu(game, prompt));
        var second = Facts<JudgmentResolvedEvent>(game).Single(fact => fact.Reason == Reason && fact.ResolutionId != first.ResolutionId);
        var secondEntry = game.CardMovements.Last(move => move.CardId == cardId);
        Require(second is { Suit: Suit.Spade } && second.CardId == cardId &&
            secondEntry.From == CardLocation.DrawPile && secondEntry.To == CardLocation.Judgment(0) && secondEntry.Sequence > top.Sequence,
            "Continuing reveals the same physical card under a new native judgment entry.");
        Answer(game, secondTiandu, choice => Action(choice) == "program-judgment-trigger-skip");
        var stop = Reach(game, prompt => prompt.Kind == DecisionKind.ProgramRepeatJudgment);
        var luoshenGain = game.CardMovements.Single(move => move.CardId == cardId && move.Reason.Value == Repeat);
        Require(Facts<ProgramJudgmentCardClaimedEvent>(game).Where(fact => fact.SkillId == Luoshen).ToArray() is [var secondClaim] &&
            secondClaim.JudgmentFrameId == second.ResolutionId && secondClaim.CardId == cardId &&
            luoshenGain.From == CardLocation.Judgment(0) && luoshenGain.To == CardLocation.Hand(0) &&
            luoshenGain.Sequence > secondEntry.Sequence && !Facts<TurnHandLimitExemptCardGrantedEvent>(game).Any(),
            "Classic Luoshen claims only the fresh judgment after Tiandu declines and issues no boundary-edition hand limit exemption.");
        Answer(game, stop, choice => Action(choice) == "stop");
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    }

    private static bool IsTiandu(GameEngine game, PendingDecision prompt) =>
        prompt.Kind == DecisionKind.ProgramJudgmentTrigger &&
        game.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().LastOrDefault() is { } window &&
        window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex].SkillId == Tiandu;

    private static string? Action(PromptChoice choice) =>
        choice.Parameters.GetValueOrDefault("program-action") ?? choice.Parameters.GetValueOrDefault("action");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision Reach(GameEngine game, Func<PendingDecision, bool> ready)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
            {
                Require(ready(prompt), $"Unexpected classic Luoshen fixture prompt: {prompt.Prompt}.");
                return prompt;
            }
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The classic Luoshen fixture did not reach its next public decision.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> select) =>
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));

    private static GameEngine Cold(GameEngine original, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(original.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(original.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(original.ResolutionStack) &&
            SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(original.CreateSnapshot(0)),
            "Cold public-command replay keeps the paid Tiandu child attached to the original black judgment.");
        return restored;
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var setup = new ShanheBattleSetup(31, 1, "classic-luoshen-original-judgment", ZhenJi, 3, 0, 0, 0, 0,
            [Tiandu, Watch], [], [], [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }], [], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        return game;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-luoshen-judgment-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, ZhenJi, "mobile:guo-jia");
            var rules = $$"""
                {"skills":[{"id":"{{Watch}}","revision":1,"triggers":[{"id":"put-tiandu-gain-on-top",
                  "window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                  "movementReasons":["{{Gain}}"],"optional":false,
                  "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},
                    "cardOwnerRef":{"kind":"owner"},"zones":["hand"],"count":1,"destination":"drawPileTop",
                    "resultBind":"top-card","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Watch] = new { name = "置于牌堆顶", description = "取得判定牌后，将一张手牌置于牌堆顶。" }
                }
            });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            var shown = catalog.Presentations[Watch];
            builder.AddSkill(new(Watch, shown.Name, shown.Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = shown });
            builder.AddGeneral(new(Peer, "仍在场的角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定黑桃判定", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 12).Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, rank + 1)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "经典洛神原判定", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [ZhenJi, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
