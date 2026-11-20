using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class LeCaiYongFeibaiDrawChecks
{
    private const string Owner = "le:cai-yong", Feibai = "le:feibai", Watch = "fixture:feibai-draw-watch";
    private const string Peer = "fixture:feibai-peer", Deck = "fixture:feibai-deck";
    private const string ObtainReason = "skill-program.le-cai-yong.feibai.obtain", DrawReason = "skill-program.le-cai-yong.feibai.draw";

    internal static void PrivateSampleAndFallbackDrawKeepOriginalEntitiesCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(47, 1, "feibai-private-sample-and-fallback", Owner, 3, 0, 0, 0, 0, [Watch],
            [new(CardKind.Crossbow, Suit.Club, 3), new(CardKind.Crossbow, Suit.Diamond, 4)], [],
            [new(Peer) { StartingHand = [] }], [], new string('f', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Require(NextPrompt().Kind == DecisionKind.PlayCard, "The fixed Feibai owner reaches the first actual Play.");
        var first = OwnerView().Hand.Single(card => card.Rank == 3).Id;
        var second = OwnerView().Hand.Single(card => card.Rank == 4).Id;

        Equip(first);
        AcceptSkill();
        var samplePrompt = NextPrompt();
        var sampler = Root();
        var sampledIds = samplePrompt.Choices.Select(choice => choice.Cards.Single()).ToArray();
        Require(sampler.LeCaiYongFeibai is { Stage: FeibaiStage.Selecting, Qualification.Prefix.X: 4 } sampled &&
            sampledIds is [var left, var right] && left != right && sampled.Sample.SequenceEqual(sampledIds) &&
            game.CreateCardZoneDiagnostics().Count(card => card.Location == CardLocation.DrawPile &&
                card.CardKind == CardKind.Crossbow && sampledIds.Contains(card.CardId)) == 2 &&
            OwnsPrivateSample(samplePrompt, sampledIds),
            "The first real Crossbow Use offers the owner two distinct original four-rune pile entities in private.");
        Restore();
        Require(OwnsPrivateSample(samplePrompt, sampledIds) && Root().Id == sampler.Id,
            "Cold replay keeps the two private sample faces on the same decision and owning frame.");
        var obtained = sampledIds[0];
        Answer(Choice(item => item.Cards.SequenceEqual([obtained])));
        Require(NextPrompt().Kind == DecisionKind.PlayCard && Facts<FeibaiCompletedEvent>().Single() is
            { Fallback: false, ObtainedCardId: var selected } && selected == obtained &&
            game.CardMovements.Single(move => move.Reason.Value == ObtainReason) is
                { CardId: var moved, From: var from, To: var to } && moved == obtained && from == CardLocation.DrawPile && to == CardLocation.Hand(0) &&
            OwnerView().Hand.Any(card => card.Id == obtained && !HasXian(card)) && Facts<FeibaiFallbackDrawIssuedEvent>().Length == 0,
            "Only the chosen sample moves into hand as an unmarked obtain and the first Use finishes without a fallback.");

        Equip(second);
        AcceptSkill();
        Require(NextPrompt().SkillPrompt?.SkillId == Watch, "The empty eight-rune pool reaches the native fallback request.");
        var root = Root();
        var actualTurn = game.State.TurnNumber;
        var attempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        Require(root.LeCaiYongFeibai is { Stage: FeibaiStage.Drawing, Sample.Count: 0, Qualification.Prefix: { X: 8, PreviousKind: CardKind.Crossbow } } &&
            attempt.Context is { RecipientSeat: 0, RequestedCount: 2 } && attempt.Context.ParentFrameId == root.Id &&
            attempt.Context.Reason.Value == DrawReason && root.NativeDraw?.ActiveAttemptId == attempt.Id &&
            Facts<FeibaiDisabledEvent>().Single() == new FeibaiDisabledEvent(0, actualTurn, root.Id) &&
            Facts<FeibaiFallbackDrawReturnedEvent>().Length == 0 && game.CardMovements.All(move => move.Reason.Value != DrawReason),
            "The second real Use locks out Feibai for its actual turn before its original, still unpaid Draw2 attempt.");
        Answer(Choice(item => item.Parameters.GetValueOrDefault("option-id") == "request"));

        var gainPrompt = NextPrompt();
        var moves = game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();
        var actualIds = moves.Select(move => move.CardId).ToArray();
        var completed = Root().NativeOperationDrawReturns.Single();
        var markFact = Facts<CardEntityMarksChangedEvent>().Single(fact => fact.FrameId == root.Id);
        Require(gainPrompt.SkillPrompt?.SkillId == Watch && Root().Id == root.Id &&
            Root().LeCaiYongFeibai?.Stage == FeibaiStage.DrawChildren && completed.Operation == SkillProgramEffectOp.ResolveLeCaiYongFeibai &&
            completed.Outcome.AttemptFrameId == attempt.Id && completed.Outcome.ActualDrawIds.SequenceEqual(actualIds) &&
            actualIds.Length == 2 && moves.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) &&
            markFact.Changes.SequenceEqual(actualIds.Select(id => new CardEntityMarkChange(id, "xian", true))) &&
            OwnerView().Hand.Where(HasXian).Select(card => card.Id).Order().SequenceEqual(actualIds.Order()) &&
            Facts<FeibaiCompletedEvent>().All(fact => fact.FrameId != root.Id),
            "The gain child sees precisely the two fallback Draw entities already marked with Xian on the original frame.");
        Restore();
        Require(NextPrompt().PromptId == gainPrompt.PromptId && Root().NativeOperationDrawReturns.Single().Outcome.ActualDrawIds.SequenceEqual(actualIds) &&
            OwnerView().Hand.Where(HasXian).Select(card => card.Id).Order().SequenceEqual(actualIds.Order()),
            "Cold replay restores the paying gain child, the original draw result and its exact two marks.");
        Answer(Choice(item => item.Parameters.GetValueOrDefault("option-id") == "gain"));
        var resumed = NextPrompt();
        for (var remaining = 1; remaining <= actualIds.Length && resumed.SkillPrompt?.SkillId == Watch; remaining++)
        {
            Require(Root().Id == root.Id, "Every fallback gain window returns to the same Feibai activation.");
            Answer(Choice(item => item.Parameters.GetValueOrDefault("option-id") == "gain"));
            resumed = NextPrompt();
        }
        Require(resumed.Kind == DecisionKind.PlayCard && Facts<FeibaiFallbackDrawReturnedEvent>().Single() is
            { FrameId: var drawFrame, ActualCount: 2, ReplacementCount: 0 } && drawFrame == root.Id &&
            Facts<FeibaiCompletedEvent>().Count(fact => fact.FrameId == root.Id && fact.Fallback) == 1 &&
            Facts<CardUseFinishedEvent>().Count(fact => fact.CardId == second) == 1,
            "The shared return completes one fallback and finishes the same original equipment use.");

        Equip(actualIds[0]);
        Require(NextPrompt().Kind == DecisionKind.PlayCard && game.State.TurnNumber == actualTurn &&
            Facts<FeibaiStartedEvent>().Length == 2 && Facts<FeibaiDisabledEvent>().Length == 1 &&
            Facts<CardUseFinishedEvent>().Count(fact => fact.CardId == actualIds[0]) == 1,
            "A third physical equipment use still completes in that turn without another Feibai offer or payment.");

        PlayerSnapshot OwnerView() => game.CreateSnapshot(0).Players[0];
        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        bool OwnsPrivateSample(PendingDecision expected, int[] ids) => Prompt() is { IsPrivate: true, PlayerSeat: 0 } current &&
            current.PromptId == expected.PromptId && current.Choices.Select(choice => choice.Cards.Single()).SequenceEqual(ids) &&
            game.CreateSnapshot(0).PrivateRevealedCards?.Select(card => card.Id).SequenceEqual(ids) == true &&
            game.CreateSnapshot(1) is { PendingDecision: null, PrivateRevealedCards: null };
        T[] Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        ProgramSkillFrame Root() => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Feibai);
        PromptChoice Choice(Func<PromptChoice, bool> matches) => Prompt()!.Choices.Single(matches);
        void Restore() => game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        void Answer(PromptChoice choice) => Accept(game, new AnswerPromptCommand(0, Prompt()!.PromptId, choice.Id, game.Revision));
        void AcceptSkill()
        {
            Require(NextPrompt().SkillPrompt?.SkillId == Feibai, "The completed equipment use offers the production Feibai trigger.");
            Answer(Choice(item => item.Parameters.GetValueOrDefault("program-action") == "activate"));
        }
        void Equip(int id)
        {
            var action = game.GetLegalActions(0).Single(item => item.CardId == id && item.Kind == LegalActionKind.Equip);
            Accept(game, new PlayCardCommand(0, id, action.TargetSeats, game.Revision, Prompt()!.PromptId, action.PlayedCardKind));
        }
        PendingDecision NextPrompt()
        {
            for (var step = 0; step < 100; step++)
            {
                if (Prompt() is { } prompt) return prompt;
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("Expected the next private Feibai fixture decision.");
        }
    }

    private static bool HasXian(CardSnapshot card) => card.SkillMarkLabel?.Contains("弦", StringComparison.Ordinal) == true;

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-feibai-native-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Owner);
            var watcher = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"request","window":"nativeDrawStarting","subject":"owner","condition":{"kind":"phaseIsPlay"},
                    "optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"request"}]}]},
                  {"id":"gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                    "movementReasons":["{{DrawReason}}"],"movementOccurrence":"perBatch","optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"gain"}]}]}
                ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"飞白原摸牌","description":"确认飞白的实体摸牌。","optionLabels":{"request":"确认请求","gain":"确认获得"} } } }
                """);
            builder.AddSkill(new(Watch, "飞白原摸牌", "确认飞白的实体摸牌。")
                { Program = watcher.Programs[Watch], ProgramPresentation = watcher.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定对手", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "固定四字实体牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:crossbow", Suit.Heart, 7), 20).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "飞白实体抽样与原摸牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
