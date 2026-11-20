using System.Globalization;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

using static BoundaryLiDianChecks;

internal static class MouGuanPingChecks
{
    private const string Hero = "mou:guan-ping", Wuwei = "mou:wuwei";
    private const string Peer = "fixture:wuwei-peer", Watch = "fixture:wuwei-gain", Deck = "fixture:wuwei-deck";
    private const string DrawReason = "skill-program.mou:wuwei.draw";

    internal static void WholeColorSlashOptionsAndRepeatedNativeDrawCold()
    {
        const int owner = 0, target = 2;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peer = new ShanheCombatantDefinition(Peer, Hp: 20) { StartingHand = [] };
        var setup = new ShanheBattleSetup(7, 1, "mou-guan-ping-native", Hero, 4, 0, 0, 0, 0, [Watch],
            [new(CardKind.Slash, Suit.Spade, 3), new(CardKind.DrawTwo, Suit.Spade, 5), new(CardKind.Crossbow, Suit.Club, 6),
             new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.IronChain, Suit.Diamond, 8), new(CardKind.ZhangbaSerpentSpear, Suit.Heart, 9)],
            [], [peer, peer], [peer], new string('e', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Accept(game, new StartGameCommand());
        Settle(game);
        var targetHp = Player(target).Hp;
        var first = UseWholeColor(red: true);
        Require(first.MouGuanPing is { Use: { TypeCount: 3, Extra: false }, Cursor: 0 },
            "The paid red Dodge, Iron Chain and Zhangba freeze three selectable options on the first Slash.");
        Choose(1); WatchGain(); FinishGain();
        Choose(2); Choose(3); Settle(game);
        CheckSlash(first, [1, 2, 3], damage: 2);
        Require(Facts<SkillStateChangedEvent>().Count(fact => fact.Seat == target && fact.SourceSeat == owner &&
                fact.SkillId == Watch && fact.Action == SkillStateAction.Suppress && fact.Origin?.Source.SkillId == Wuwei) == 1 &&
            Player(target).Hp == targetHp - 2,
            "The original target receives the turn suppression and loses two HP after all three distinct choices.");

        var second = UseWholeColor(red: false);
        Require(second.MouGuanPing is { Use: { TypeCount: 3, Extra: true }, Cursor: 0 },
            "The earned additional Wuwei activation pays the complete current black hand across three categories.");
        Choose(1); WatchGain();
        var earlier = Root().NativeOperationDrawReturns.Single();
        FinishGain();
        Choose(1); WatchGain();

        var parent = Root();
        var useId = second.MouGuanPing!.Use.UseId;
        var gainPrompt = P(game)!.PromptId;
        var batch = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.ParentFrameId == second.Id);
        var receipts = parent.NativeOperationDrawReturns;
        var latest = receipts.Last();
        var spent = Draws();
        Require(parent.Id == second.Id && parent.MouGuanPing is { Stage: MouGuanPingStage.Children, Cursor: 2, Mask: 1 } &&
            receipts.Count == 2 && receipts[0] == earlier && receipts.All(receipt => receipt.InstructionIndex == 1 &&
                receipt.Operation == SkillProgramEffectOp.ResolveMouGuanPingWuwei && receipt.Reason.Value == DrawReason &&
                receipt.Outcome is { RecipientSeat: owner, RequestedCount: 1, Replacement: null, ActualDrawIds.Count: 1 }) &&
            batch.Batch.Movements.Select(move => move.CardId).SequenceEqual(latest.Outcome.ActualDrawIds) &&
            spent.Length == 3 && spent.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner)) &&
            spent.Select(move => move.CardId).Distinct().Count() == 3 &&
            spent.Skip(1).Select(move => move.CardId).SequenceEqual(receipts.SelectMany(receipt => receipt.Outcome.ActualDrawIds)) &&
            !Facts<DamageAppliedEvent>().Any(fact => fact.SourceCardUseResolutionId == useId) &&
            !Facts<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == useId),
            "The second actual gain is paid by the same original instruction while its Slash still waits for its remaining option.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restored = Root();
        Require(P(game)?.PromptId == gainPrompt && restored.Id == parent.Id && restored.MouGuanPing?.Use.UseId == useId &&
            restored.NativeOperationDrawReturns.SelectMany(receipt => receipt.Outcome.ActualDrawIds)
                .SequenceEqual(receipts.SelectMany(receipt => receipt.Outcome.ActualDrawIds)) &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.Id == batch.Id) && Draws().SequenceEqual(spent),
            "Cold replay preserves the paid observer, both actual native return receipts and the same unresolved Slash.");
        FinishGain();
        Choose(3); Settle(game);
        CheckSlash(second, [1, 1, 3], damage: 1);
        Require(Draws().SequenceEqual(spent) && Player(target).Hp == targetHp - 3,
            "Repeated Draw choices pay exactly twice on the extra Slash and never award the distinct-option damage bonus.");

        ProgramSkillFrame Root() => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.MouGuanPing is not null);
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        T[] Facts<T>() => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        CardMovementRecord[] Draws() => game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();

        ProgramSkillFrame UseWholeColor(bool red)
        {
            var hand = Player(owner).Hand;
            var cards = hand.Where(card => red ? card.Suit is Suit.Heart or Suit.Diamond : card.Suit is Suit.Spade or Suit.Club)
                .Select(card => card.Id).Order().ToArray();
            Accept(game, new UseProgramSkillCommand(owner, Wuwei, "color-hand-slash", cards, [target], game.Revision, P(game)!.PromptId));
            Reach(game, IsOption);
            var root = Root();
            var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == root.MouGuanPing!.Use.UseId);
            Require(use.CardKind == CardKind.Slash && use.TargetSeats.SequenceEqual([target]) && use.Action is { } action &&
                action.PhysicalCards.Select(card => card.CardId).Order().SequenceEqual(cards) &&
                action.PhysicalCards.All(card => card.From == CardLocation.Hand(owner)),
                "The original Slash paid exactly every entity of the selected color from the owner's hand and kept the declared target.");
            return root;
        }

        bool IsOption(PendingDecision prompt) => prompt.PlayerSeat == owner &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "mou-guan-ping");
        void Choose(int option)
        {
            Reach(game, IsOption);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == option.ToString(CultureInfo.InvariantCulture));
        }
        void WatchGain() => Reach(game, prompt => prompt.PlayerSeat == owner && prompt.SkillPrompt?.SkillId == Watch);
        void FinishGain() => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");

        void CheckSlash(ProgramSkillFrame root, IReadOnlyList<int> options, int damage)
        {
            var use = root.MouGuanPing!.Use;
            var selected = Facts<MouGuanPingOptionEvent>().Where(fact => fact.FrameId == root.Id).ToArray();
            var drawReturns = Facts<MouGuanPingDrawReturnedEvent>().Where(fact => fact.FrameId == root.Id).ToArray();
            var expectedDrawIndexes = options.Select((option, index) => (option, index)).Where(item => item.option == 1).Select(item => item.index);
            Require(selected.All(fact => fact.UseId == use.UseId && fact.ActionId == use.ActionId) && selected.Select(fact => fact.Option).SequenceEqual(options) &&
                drawReturns.All(fact => fact.Actual == 1 && fact.Replacement == 0) && drawReturns.Select(fact => fact.Index).SequenceEqual(expectedDrawIndexes) &&
                Facts<DamageAppliedEvent>().Single(fact => fact.SourceCardUseResolutionId == use.UseId) is { SourceSeat: owner, TargetSeat: target } applied &&
                applied.Amount == damage && Facts<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use.UseId) == 1,
                "The exact original Slash resolves once with its own option returns and resulting damage.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mou-guan-ping-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"paid-wuwei","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementReasons":["{{DrawReason}}"],"movementOccurrence":"perBatch","optional":false,"isSkillInvocation":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
                ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                    "name":"武威得牌","description":"在武威的实体牌到账后暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddCard(new("fixture:wuwei-spear", "丈八蛇矛", "装备牌",
                "将两张手牌当作一张杀使用或打出。", CardKind.ZhangbaSerpentSpear));
            builder.AddGeneral(new(Peer, "真实武威目标", "", Watch, "wei", 20));
            builder.AddDeck(new(Deck, "固定真实黑闪牌堆", 0, 2, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7), 40).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "谋关平真实原生杀", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
