using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;
using static BoundaryLiDianChecks;

internal static class HappyDiaoChanPindianTargetChecks
{
    private const string Hero = "happy:shen-diao-chan", Huoxin = "happy:huoxin", LuKang = "classic:lu-kang";
    private const string Peer = "fixture:huoxin-peer", Deck = "fixture:huoxin-pindian-deck";
    private const string CostWatch = "fixture:huoxin-paid-cost", CostReason = "skill-program.happy:huoxin.cost";

    internal static void PaidHuoxinKeepsOrderedPrivatePairCold() => Run(watchPaidCost: false);

    internal static void PaidHuoxinCostMovementKeepsOriginalPairCold() => Run(watchPaidCost: true);

    private static void Run(bool watchPaidCost)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(watchPaidCost));
        var setup = new ShanheBattleSetup(37, 1, "happy-huoxin-pindian", Hero, 3, 0, 0, 0, 0, watchPaidCost ? [CostWatch] : [],
            [new(CardKind.Dodge, Suit.Club, 2)], [],
            [new(LuKang) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 5)] }, new(Peer) { StartingHand = [] }],
            [new(Peer) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 5)] }], new string('c', 64))
            { DefaultGodFactionId = "qun", ExtraSkillSlots = watchPaidCost ? 1 : 0 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, content);

        Accept(game, new StartGameCommand());
        Settle(game);
        var cost = game.CreateSnapshot(0).Players[0].Hand.Single().Id;
        var sourceCard = game.CreateSnapshot(2).Players[2].Hand.Single().Id;
        var opponentCard = game.CreateSnapshot(1).Players[1].Hand.Single().Id;
        Accept(game, new UseProgramSkillCommand(0, Huoxin, "charm-contest", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, _ => AtStage(HappyDiaoChanStage.Cost));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("mode") == "cost" && choice.Cards.SequenceEqual([cost]));
        var acceptsPaidCost = !watchPaidCost || CheckPaidCostEdge(cost);
        Reach(game, _ => AtStage(HappyDiaoChanStage.Pair));
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.HappyDiaoChan is not null);
        var paid = Facts<HappyDiaoChanCostPaidEvent>().Single();
        var discard = game.CardMovements.Single(move => move.Sequence == paid.Sequence);
        Require(paid.FrameId == root.Id && paid.CardId == cost && discard.CardId == cost &&
            discard.From == CardLocation.Hand(0) && discard.To == CardLocation.DiscardPile &&
            game.CreateSnapshot(0).Players[0].HandCount == 0 && P(game)!.Choices.Single().Targets.SequenceEqual([2, 1]),
            "Huoxin pays its only hand card and offers the single ordered pair with Qianjie only as the source.");

        Answer(game, choice => choice.Targets.SequenceEqual([2, 1]));
        var issued = Facts<HappyDiaoChanPindianIssuedEvent>().Single();
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var sourcePrompt = game.CreateSnapshot(2).PendingDecision;
        Require(issued == new HappyDiaoChanPindianIssuedEvent(root.Id, pair.Id, 2, 1) &&
            pair is { SourceSeat: 2, OpponentSeat: 1, PindianStep: PindianStep.ChooseSourceCard } && pair.ParentFrameId == root.Id &&
            sourcePrompt is { PlayerSeat: 2, IsPrivate: true } && sourcePrompt.Choices.Single().Cards.SequenceEqual([sourceCard]) &&
            game.CreateSnapshot(0).PendingDecision is null && game.CreateSnapshot(1).PendingDecision is null &&
            !game.CardMovements.Any(move => move.Reason == CardMoveReasons.PindianReveal),
            "The real ordered pair opens a private, unrevealed native choice for its first participant.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(game.ResolutionStack.OfType<PindianFrame>().Single().Id == pair.Id &&
            game.CreateSnapshot(2).PendingDecision?.PromptId == sourcePrompt!.PromptId && game.CreateSnapshot(0).PendingDecision is null,
            "Cold restore keeps the original pair and private source prompt.");
        Reach(game, _ => AtStage(HappyDiaoChanStage.HuoxinSuit));
        var result = Facts<PindianResultDeterminedEvent>().Single();
        Require(result.FrameId == pair.Id && result.Result is { SourceSeat: 2, OpponentSeat: 1, SourceRank: 5, OpponentRank: 5 } &&
            result.Result.SourceCardId == sourceCard && result.Result.OpponentCardId == opponentCard &&
            Facts<HappyDiaoChanPindianReturnedEvent>().Single() == new HappyDiaoChanPindianReturnedEvent(root.Id, pair.Id, 2, 1, 5, 5) &&
            Facts<HappyDiaoChanCostPaidEvent>().SequenceEqual([paid]),
            "Both original physical cards yield a real tie and return to the same Huoxin activation.");
        Require(acceptsPaidCost, "Huoxin must recognize its actual paid discard observer structurally before and after cold restore when the shared return has no explicit resume ID.");

        bool CheckPaidCostEdge(int paidCardId)
        {
            Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: CostWatch });
            var paidCost = Facts<HappyDiaoChanCostPaidEvent>().Single();
            var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paidCost.FrameId);
            var moved = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == paidCost.BatchId);
            var stop = P(game)!.PromptId;
            var stack = game.ResolutionStack.ToArray();
            var index = Array.IndexOf(stack, moved);
            Require(index > 0 && stack[index - 1].Id == parent.Id && parent.SkillId == Huoxin &&
                parent.HappyDiaoChan is { Stage: HappyDiaoChanStage.CostChildren } && parent.PendingMovementContinuation is not null &&
                moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId == parent.Id &&
                moved.Batch.Movements is [var movement] && movement.CardId == paidCardId && movement.Sequence == paidCost.Sequence &&
                movement.From == CardLocation.Hand(0) && movement.To == CardLocation.DiscardPile && movement.Reason.Value == CostReason &&
                CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(parent, moved),
                "The real paid discard creates the shared implicit return to the original Huoxin frame.");
            var liveMatch = game.GetRuntimeModule<CharacterDiaoChanHappyGodRuntime>().HappyDiaoChanStructuralEdge(moved, parent);

            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
            var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
            var restoredMoved = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Id == moved.Id);
            Require(P(game)?.PromptId == stop && restoredMoved.ResumeProgramFrameId is null &&
                CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(restoredParent, restoredMoved) &&
                Facts<HappyDiaoChanCostPaidEvent>().SequenceEqual([paidCost]),
                "Cold replay keeps the original paid implicit movement return and the same observer prompt.");
            var restoredMatch = game.GetRuntimeModule<CharacterDiaoChanHappyGodRuntime>().HappyDiaoChanStructuralEdge(restoredMoved, restoredParent);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            return liveMatch && restoredMatch;
        }

        bool AtStage(HappyDiaoChanStage stage) => game.ResolutionStack.LastOrDefault() is ProgramSkillFrame
            { HappyDiaoChan: { Stage: var current } } && current == stage;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
    }

    private sealed class Fixture(bool watchPaidCost) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-huoxin-pindian-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, LuKang);
            if (watchPaidCost)
            {
                var catalog = SkillProgramCatalog.Load($$"""
                    {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{CostWatch}}","revision":1,"triggers":[
                        {"id":"paid-huoxin","window":"cardsMoved","subject":"owner","sourceZones":["hand"],
                         "movementReasons":["{{CostReason}}"],"movementOccurrence":"perBatch","optional":false,"isSkillInvocation":false,
                         "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
                    ]}]}
                    """, $$"""
                    {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{CostWatch}}":{
                        "name":"惑心弃牌","description":"在实际弃置费用后暂停。","optionLabels":{"continue":"继续"} } } }
                    """);
                builder.AddSkill(new(CostWatch, catalog.Presentations[CostWatch].Name, catalog.Presentations[CostWatch].Description)
                    { Program = catalog.Programs[CostWatch], ProgramPresentation = catalog.Presentations[CostWatch] });
            }
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 10));
            builder.AddDeck(new(Deck, "惑心固定牌堆", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "惑心原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, LuKang, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
