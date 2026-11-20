using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class BoundaryMobileShuangxiongDiscardEntryChecks
{
    private const string Hero = "boundary:yan-liang-wen-chou-mobile", Skill = "boundary:shuangxiong-mobile";
    private const string Watch = "fixture:mobile-shuangxiong-reentry", Opening = "fixture:mobile-shuangxiong-opening";
    private const string Mode = "identity:mobile-shuangxiong-reentry", Deck = "fixture:mobile-shuangxiong-deck";
    private const string Rediscard = "skill-program.fixture:mobile-shuangxiong-reentry.SelectAndMoveOwnedCard";
    private const string Claim = "skill.boundary-yan-liang-wen-chou.shuangxiong.claim";
    private static readonly string[] Generals = [Hero, "fixture:mobile-shuangxiong-peer-1", "fixture:mobile-shuangxiong-peer-2", "fixture:mobile-shuangxiong-peer-3"];

    internal static void ShuangxiongCannotClaimANewerEntryOfAnOpponentDuelResponseCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        ReachDecision(game, p => p.Kind == DecisionKind.SelectGeneral || p.SkillPrompt?.SkillId == Skill);
        while (Pending(game) is { Kind: DecisionKind.SelectGeneral } setup)
        {
            Accept(game, new SelectGeneralCommand(setup.PlayerSeat, Generals[setup.PlayerSeat], game.Revision, setup.PromptId));
            ReachDecision(game, p => p.Kind == DecisionKind.SelectGeneral || p.SkillPrompt?.SkillId == Skill);
        }

        Require(game.CreateSnapshot(0).Players[0].HandCount == 3, "The fixed initial deal gives three physical cards to the Shuangxiong user.");
        var retained = ChooseOppositeReveal(game);
        var owner = game.CreateSnapshot(0).Players[0];
        Require(game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 1 } && owner.Hand.Any(c => c.Id == retained),
            "The real draw replacement exhausts the stock and leaves only its unselected card in discard.");
        PlayConvertedDuel(game);
        ReachDecision(game, p => p is { PlayerSeat: 1, Kind: DecisionKind.RespondSlash });
        var response = Pending(game)!.Choices.First(c => c.Parameters.GetValueOrDefault("response") == "slash").Cards.Single();
        AnswerChoice(game, c => c.Cards.SequenceEqual([response]));
        ReachDecision(game, p => p is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } && p.Choices.Any(c => Action(c, "activate")));

        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.Duel);
        var action = Facts<CardActionAcceptedEvent>(game).Single(e => e.Action.Type == CardActionType.Response && e.Action.ParentActionId == use.Action!.ActionId).Action;
        var original = game.CardMovements.Single(m => m.CardId == response && m.Reason == CardMoveReasons.ResponseFinished);
        Require(action.ActorSeat == 1 && action.PhysicalCards.Single().CardId == response && original.From == CardLocation.Processing &&
            original.To == CardLocation.DiscardPile && Facts<CardResponseCompletedEvent>(game).Single() is
                { OriginalContinuation: ProgramCardContinuation.DuelSlash, NativeActorSeat: 1 } complete && complete.ActionId == action.ActionId &&
            game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(f => f.ResponseCompletion is { CostsDrained: true } returnState &&
                returnState.ActionId == action.ActionId && returnState.NativeCosts.Contains(original)),
            "The other player actually responds to the converted Duel, and the original physical finish stays on its response-completion frame.");

        var control = Cold(game, registry);
        AnswerChoice(control, c => Action(c, "skip"));
        LoseDuel(control, use.Id, owner.Hp);
        Require(Pending(control)?.SkillPrompt?.SkillId == Skill, "Actual Duel damage enables the original Slash claim.");
        AnswerChoice(control, c => Action(c, "activate"));
        ReachDecision(control, IsPlay);
        var gain = Facts<BoundaryYanLiangWenChouResponseClaimedEvent>(control).Single();
        Require(gain.UseId == use.Id && gain.CardId == response &&
            control.CardMovements.Single(m => m.Reason.Value == Claim).To == CardLocation.Hand(0),
            "The same untouched Slash is normally obtained from the original Duel response.");

        AnswerChoice(game, c => Action(c, "activate"));
        ReachDecision(game, p => p is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } && p.Choices.Any(c => c.Cards.SequenceEqual([response])));
        var returned = game.CardMovements.Where(m => m.CardId == response && m.Sequence > original.Sequence).ToArray();
        Require(returned is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
            shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(1),
            "The completed-response child genuinely reshuffles and redraws the response Slash before the Duel continues.");
        game = Cold(game, registry);
        AnswerChoice(game, c => c.Cards.SequenceEqual([response]));
        var fresh = game.CardMovements.Single(m => m.CardId == response && m.Reason.Value == Rediscard);
        Require(fresh.From == CardLocation.Hand(1) && fresh.To == CardLocation.DiscardPile && fresh.Sequence > original.Sequence &&
            fresh.BatchId != original.BatchId, "The hand re-discard creates a different physical discard entry for the same Slash.");
        LoseDuel(game, use.Id, owner.Hp);
        if (Pending(game)?.SkillPrompt?.SkillId == Skill)
        {
            AnswerChoice(game, c => Action(c, "activate"));
            ReachDecision(game, IsPlay);
        }
        Require(!Facts<BoundaryYanLiangWenChouResponseClaimedEvent>(game).Any() && !game.CardMovements.Any(m => m.Reason.Value == Claim) &&
            game.CreateCardZoneDiagnostics().Single(c => c.CardId == response).Location == CardLocation.DiscardPile,
            $"Shuangxiong may not claim entry {fresh.Sequence} using the original Duel response at entry {original.Sequence}.");
        _ = Cold(game, registry);
    }

    internal static void ShuangxiongCollectsBothNativeZhangbaDiscardEntriesWithoutCompletionObserverCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(zhangba: true));
        var enemy = new ShanheCombatantDefinition(Generals[1], Hp: 1)
        {
            StartingHand = [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3)],
            StartingEquipment = [new(CardKind.ZhangbaSerpentSpear, Suit.Spade, 12)]
        };
        var setup = new ShanheBattleSetup(31, 1, "mobile-shuangxiong-zhangba", Hero, 4, 0, 0, 0, 0,
            [], [new(CardKind.Slash, Suit.Spade, 7)], [], [enemy], [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        ReachDecision(game, p => p.SkillPrompt?.SkillId == Skill && p.Choices.Any(c => Action(c, "activate")));
        ChooseOppositeReveal(game);
        var hp = game.CreateSnapshot(0).Players[0].Hp;
        PlayConvertedDuel(game);
        ReachDecision(game, p => p is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });

        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.Duel);
        var zhangba = Facts<ZhangbaSerpentSpearConvertedEvent>(game).Single(e => e.ResolutionId == use.Id && !e.IsUse);
        var originals = game.CardMovements.Where(m => m.Reason == CardMoveReasons.ResponseFinished).ToArray();
        Require(zhangba.UserSeat == 1 && zhangba.PhysicalCardIds.Count == 2 && !Facts<CardResponseCompletionStartedEvent>(game).Any() &&
            originals.Select(m => m.CardId).SequenceEqual(zhangba.PhysicalCardIds) && originals.All(m =>
                m.CardKind == CardKind.Dodge && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile),
            "The real ordinary weapon response finishes both physical Dodges in its native material order.");
        game = Cold(game, registry);
        LoseDuel(game, use.Id, hp);
        Require(Pending(game)?.SkillPrompt?.SkillId == Skill, "The original native Zhangba response enables the mobile Shuangxiong claim.");
        AnswerChoice(game, c => Action(c, "activate"));
        ReachDecision(game, IsPlay);
        var claims = Facts<BoundaryYanLiangWenChouResponseClaimedEvent>(game).ToArray();
        Require(claims.All(e => e.UseId == use.Id && e.OwnerSeat == 0) && claims.Select(e => e.CardId).SequenceEqual(zhangba.PhysicalCardIds) &&
            game.CardMovements.Where(m => m.Reason.Value == Claim).Select(m => m.CardId).SequenceEqual(zhangba.PhysicalCardIds) &&
            game.CreateSnapshot(0).Players[0].Hand.Count(c => zhangba.PhysicalCardIds.Contains(c.Id)) == 2,
            "Real Duel damage lets Shuangxiong receive the two exact native Zhangba entities once in their original order.");
        _ = Cold(game, registry);
    }

    private static int ChooseOppositeReveal(GameEngine game)
    {
        var originalHand = game.CreateSnapshot(0).Players[0].Hand;
        AnswerChoice(game, c => Action(c, "activate"));
        ReachDecision(game, p => p.SkillPrompt?.SkillId == Skill && p.Choices.Any(c => Action(c, "select-subset")));
        var revealed = Facts<ProgramCardsRevealedEvent>(game).Single(e => e.SkillId == Skill).Cards;
        Require(revealed.Count == 2, "The actual mobile Shuangxiong draw replacement reveals two physical cards.");
        var retained = revealed.First(c => originalHand.Any(h => Red(c.Suit) != Red(h.Suit)));
        AnswerChoice(game, c => c.Cards.SequenceEqual([retained.Id]));
        ReachDecision(game, IsPlay);
        return retained.Id;
    }

    private static void PlayConvertedDuel(GameEngine game)
    {
        var duel = game.GetHumanLegalActions().First(a => a.PlayedCardKind == CardKind.Duel && a.TargetSeats.SequenceEqual([1]) &&
            a.ConversionSource?.SkillId == Skill);
        Accept(game, new PlayCardCommand(0, duel.CardId!.Value, duel.TargetSeats, game.Revision, Pending(game)!.PromptId, duel.PlayedCardKind)
            { ConversionSource = duel.ConversionSource, AdditionalConversionSources = duel.AdditionalConversionSources });
    }

    private static void LoseDuel(GameEngine game, long useId, int originalHp)
    {
        ReachDecision(game, p => p is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        AnswerChoice(game, c => c.Parameters.GetValueOrDefault("response") is "take-damage" or "pass");
        ReachDecision(game, p => p.SkillPrompt?.SkillId == Skill || IsPlay(p));
        Require(Facts<DuelResponseEvent>(game).Any(e => e.ResolutionId == useId && e.ResponderSeat == 0 && !e.UsedSlash) &&
            Facts<DamageAppliedEvent>(game).Single(e => e.SourceCardUseResolutionId == useId) is { SourceSeat: 1, TargetSeat: 0, Amount: 1 } &&
            game.CreateSnapshot(0).Players[0].Hp == originalHp - 1, "The failed native Duel response causes real damage to the Shuangxiong user.");
    }

    private static bool IsPlay(PendingDecision p) => p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
    private static bool Red(Suit suit) => suit is Suit.Heart or Suit.Diamond;
    private static bool Action(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision? Pending(GameEngine game) => Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);

    private static void ReachDecision(GameEngine game, Func<PendingDecision, bool> goal)
    {
        for (var step = 0; step < 100; step++)
        {
            if (Pending(game) is { } p && goal(p))
                return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The mobile Shuangxiong fixture missed the next native prompt.");
    }

    private static void AnswerChoice(GameEngine game, Func<PromptChoice, bool> accepts)
    {
        var pending = Pending(game)!;
        Accept(game, new AnswerPromptCommand(pending.PlayerSeat, pending.PromptId, pending.Choices.First(accepts).Id, game.Revision));
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay must preserve the actual response settlement, private choice and every seat's view.");
        return restored;
    }

    private sealed class Fixture(bool zhangba = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-shuangxiong-reentry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            if (zhangba)
            {
                CharacterFixtureContent.Register(builder, Hero);
                builder.AddCard(new("fixture:mobile-shuangxiong-spear", "丈八蛇矛", "装备牌",
                    "将两张手牌当作一张杀使用或打出。", CardKind.ZhangbaSerpentSpear));
                builder.AddGeneral(new(Generals[1], "丈八响应方", "supporter", "standard:none", "qun", 4));
                builder.AddDeck(new(Deck, "红色实体杀", 0, 0, []) { PhysicalCards = Enumerable.Range(1, 8)
                    .Select(rank => new ContentDeckPhysicalCard("standard:slash", Suit.Heart, rank)).ToArray() });
                builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界双雄原生丈八", 2, 2, new Dictionary<string, int>(), Deck,
                    GeneralPoolIds: [Hero, Generals[1]], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                    { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
                return;
            }

            CharacterFixtureContent.Register(new FixtureHeroBuilder(builder), Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Opening}}}","revision":1,
                    "modifiers":[{"id":"opening-bonus","query":"initialHandSize","operation":"add","value":2,"priority":0}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"draw-then-rediscard","window":"cardResponseCompleted",
                    "ownerRelation":"actor","cardKinds":["slash"],"optional":true,"effects":[
                      {"op":"draw","target":"owner","amount":2},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Opening] = new { name = "测试起手", description = "开局多发两张牌。" },
                    [Watch] = new { name = "响应后摸回重弃", description = "摸两张牌，再弃置一张手牌。" }
                } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[1], "响应方", "supporter", Watch, "qun", 4));
            foreach (var peer in Generals.Skip(2))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "红黑各四的实体杀", 1, 2, []) { PhysicalCards = Enumerable.Range(1, 8)
                .Select(rank => new ContentDeckPhysicalCard("standard:slash", rank <= 4 ? Suit.Heart : Suit.Spade, rank)).ToArray() });
            builder.AddMode(new(Mode, "移动界双雄原响应入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class FixtureHeroBuilder(IContentRegistryBuilder builder) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => builder.AddGeneral(general.Id == Hero
            ? general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Opening] } : general);
        public void AddSkill(ContentSkillDefinition skill) => builder.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => builder.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => builder.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => builder.AddMode(mode);
    }
}
