using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryXiaoQiaoTianxiangDiscardEntryChecks
{
    private const string Driver = "fixture:tianxiang-incoming", Watch = "fixture:tianxiang-rediscard";
    private const string Peer = "fixture:tianxiang-peer", Deck = "fixture:tianxiang-cost-deck";
    private const string RediscardReason = "skill-program.fixture:tianxiang-rediscard.SelectAndMoveOwnedCard";

    internal static void TianxiangGiftsOnlyTheOriginalPaidDiscard()
    {
        foreach (var (hero, skill) in new[] { ("boundary:xiao-qiao-mobile", "boundary:tianxiang-mobile"), ("boundary:xiao-qiao", "boundary:tianxiang") })
        foreach (var rediscard in new[] { false, true })
        {
            var (game, content) = Start(hero, skill, rediscard);
            var play = Next(game);
            var owner = game.CreateSnapshot(0).Players[0];
            var recipient = game.CreateSnapshot(1).Players[1];
            var card = owner.Hand.Single(c => c.Suit == Suit.Heart).Id;
            Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.CreateSnapshot(0).DrawPileCount == 0,
                "The Tianxiang payer begins actual Play with the sole heart hand card and no draw pile stock.");
            Submit(game, new UseProgramSkillCommand(0, Driver, "incoming", [], [2], game.Revision, play.PromptId));
            Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Answer(game, choice => choice.Cards.SequenceEqual([card]));
            Answer(game, choice => choice.Targets.SequenceEqual([1]) && choice.Parameters.GetValueOrDefault("branch") == "lose-hp-gift");
            var pending = Next(game);
            var paid = Facts<ProgramSuitPreventionPaymentEvent>(game).Single();
            var original = game.CardMovements.Single(move => move.CardId == card && move.Sequence == paid.MovementSequence);
            Need(paid.CostCardId == card && paid.CostFrom == CardLocation.Hand(0) && paid.EffectiveSuit == Suit.Heart &&
                 paid.RecipientSeat == 1 && paid.Branch == SuitPreventionBenefitBranch.LoseHpAndGift && original.From == CardLocation.Hand(0) &&
                 original.To == CardLocation.DiscardPile && original.Reason.Value == $"skill-program.{skill}.DiscardSuitPreventDamageAndBenefit",
                "The accepted Tianxiang pays its physical heart hand card, freezing the actual discard entry.");

            if (!rediscard)
            {
                Need(pending is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                     Facts<ProgramSuitPreventionGiftEvent>(game).Single() is { RecipientSeat: 1, Unavailable: false } gift && gift.CostCardId == card &&
                     game.CardMovements.Any(move => move.Sequence == gift.MovementSequence && move.CardId == card &&
                         move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(1)),
                    "A surviving original discard entry is genuinely delivered to the chosen recipient.");
                RequireHealthOutcome(game, owner.Hp, recipient.Hp, paid);
                continue;
            }

            var pickup = game.CardMovements.Where(move => move.CardId == card && move.Sequence > original.Sequence).ToArray();
            Need(pending.Choices.Any(choice => choice.Cards.SequenceEqual([card])) &&
                 game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == paid.ProgramFrameId &&
                     frame.SuitPreventionBenefit is { Stage: SuitPreventionBenefitStage.CostPaid } state && state.CostMovementSequence == original.Sequence) &&
                 pickup is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
                 shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
                 drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0) && !Facts<ProgramSuitPreventionBenefitIssuedEvent>(game).Any(),
                "An actual cost movement child reshuffles and obtains the same physical card before Tianxiang issues its HP loss.");
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
            Need(restored.CardMovements.SequenceEqual(game.CardMovements) &&
                 SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                "Cold replay keeps the paid Tianxiang, the drawn card, and the private choice to discard it again.");
            foreach (var run in new[] { game, restored })
            {
                Answer(run, choice => choice.Cards.SequenceEqual([card]));
                Need(Next(run) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The real child and paid HP-loss branch return to Play.");
                var later = run.CardMovements.Where(move => move.CardId == card && move.Reason.Value == RediscardReason).ToArray();
                Need(later.Length > 0 && later[0].From == CardLocation.Hand(0) && later[^1].To == CardLocation.DiscardPile &&
                     later[^1].Sequence > pickup[^1].Sequence && later[^1].BatchId != original.BatchId,
                    "The cost child really discards the same card from hand into a later physical entry.");
                RequireHealthOutcome(run, owner.Hp, recipient.Hp, paid);
                Need(Facts<ProgramSuitPreventionGiftEvent>(run).Single() is { RecipientSeat: 1, Unavailable: true, MovementSequence: 0 } gift &&
                     gift.CostCardId == card && run.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location == CardLocation.DiscardPile,
                    $"{skill} cannot give the later discard entry as the original paid cost.");
            }
        }
    }

    private static void RequireHealthOutcome(GameEngine game, int ownerHp, int recipientHp, ProgramSuitPreventionPaymentEvent paid)
    {
        Need(game.CreateSnapshot(0).Players[0].Hp == ownerHp && game.CreateSnapshot(1).Players[1].Hp == recipientHp - 1 &&
             Facts<ProgramSuitPreventionPaymentEvent>(game).SequenceEqual([paid]) &&
             Facts<ProgramSkillHpLostEvent>(game).Count(e => e.FrameId == paid.ProgramFrameId && e.TargetSeat == 1 && e.Amount == 1) == 1 &&
             !Facts<DamageAppliedEvent>(game).Any(e => e.TargetSeat == 0),
            "The original damage stays prevented and the chosen other player loses exactly one HP on the original paid frame.");
    }

    private static (GameEngine Game, ContentRegistry Content) Start(string hero, string skill, bool rediscard)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(hero, skill));
        var peers = Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray();
        var setup = new ShanheBattleSetup(107, 1, "tianxiang-paid-discard-entry", hero, 3, 0, 0, 0, 0,
            rediscard ? [Driver, Watch] : [Driver], [new(CardKind.Dodge, Suit.Heart, 2)], [], peers, [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        return (game, content);
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt) return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Tianxiang did not reach the next public decision.");
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> match)
    {
        var prompt = Next(game);
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(match).Id, game.Revision));
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Tianxiang command rejected.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture(string hero, string skill) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-tianxiang-paid-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, hero);
            var rules = $$$"""
                {"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                   "targetKind":"otherLiving","usesPerTurn":1,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":2}]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"draw-own-payment-and-discard-it","window":"cardsMoved","subject":"owner",
                   "sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.{{{skill}}}.DiscardSuitPreventDamageAndBenefit"],
                   "optional":false,"usageScope":"game","usageLimit":1,"effects":[
                     {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                     {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                      "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "实际有来源伤害", description = "令选中角色对自己造成伤害。" },
                    [Watch] = new { name = "取得后再弃置", description = "天香实际付款后摸一张牌，再弃置一张手牌。" }
                } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Peer, "同行角色", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "实际开局摸牌", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Club, 5), new("standard:dodge", Suit.Diamond, 6)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "天香原弃牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
