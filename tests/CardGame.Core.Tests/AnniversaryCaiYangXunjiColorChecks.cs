using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class AnniversaryCaiYangXunjiColorChecks
{
    private const string Hero = "anniversary:cai-yang", Xunji = "anniversary:xunji";
    private const string Chosen = "classic:guan-xing-zhang-bao", Fuhun = "classic:fuhun";
    private const string Mode = "identity:classic-xunji-effective-color", Deck = "fixture:xunji-effective-color-deck";
    private static readonly string[] Generals = [Hero, Chosen, "fixture:xunji-dodge-target", "fixture:xunji-peer"];

    internal static void XunjiUsesTheOriginalBlackColorOfACompositeSlashCold()
    {
        var stock = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2), 40).ToArray();
        var probe = Start(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock)));
        var slots = probe.CreateSnapshot(1).Players[1].Hand.Select(card => card.Id).Take(3).ToArray();
        Require(slots.Length == 3, "The fixed native opening must deal the designated player three physical materials.");
        stock[slots[0] - 1] = new("standard:dodge", Suit.Spade, 3);
        stock[slots[1] - 1] = new("standard:dodge", Suit.Club, 4);
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock));

        var game = Start(content);
        Accept(game, new UseProgramSkillCommand(0, Xunji, "designate-next-turn", [], [1], game.Revision, P(game)!.PromptId));
        PlayPhase(game, 0);
        var grant = Facts<CaiYangXunjiArmedEvent>(game).Single().Grant;
        Require(grant.Target == 1 && grant.Source is { OwnerSeat: 0, SkillId: Xunji } && Facts<CaiYangXunjiDueEvent>(game).Length == 0,
            "The official Xunji activation binds one original other player and remains pending until their next actual turn.");
        EndPlay(game, 0);
        PlayPhase(game, 1);
        var actualTurn = game.State.TurnNumber;
        Require(Facts<CaiYangXunjiDueEvent>(game).SequenceEqual([new(grant.FrameId, actualTurn, 1)]) && actualTurn > grant.IssuedTurn,
            "The selected player's own next actual turn owns the original Xunji grant.");
        var hand = game.CreateSnapshot(1).Players[1].Hand;
        var spade = hand.Single(card => card.Suit == Suit.Spade).Id;
        var club = hand.Single(card => card.Suit == Suit.Club).Id;
        var heart = hand.First(card => card.Suit == Suit.Heart).Id;

        var control = Cold(game, content);
        var mixed = UseFuhun(control, [spade, heart], null);
        EndPlay(control, 1);
        Reach(control, prompt => IsDuelResponse(prompt) || TurnEnded(control, actualTurn));
        Require(TurnEnded(control, actualTurn) && Facts<CaiYangBlackUseEvent>(control).All(fact => fact.ActionId != mixed.ActionId) &&
            Facts<CaiYangXunjiIssuedEvent>(control).Length == 0 &&
            Facts<CaiYangXunjiConsumedEvent>(control).SequenceEqual([new(grant.FrameId, actualTurn, false)]),
            "The genuinely paid red-and-black Fuhun Slash has no color and does not meet Xunji at that person's Ending.");

        var black = UseFuhun(game, [spade, club], false);
        EndPlay(game, 1);
        Reach(game, prompt => IsDuelResponse(prompt) || TurnEnded(game, actualTurn));
        Require(IsDuelResponse(P(game)) && Facts<CaiYangBlackUseEvent>(game).SequenceEqual([new(actualTurn, 1, black.ActionId)]),
            $"The accepted two-material Slash {black.ActionId} is black with no suit and must issue Xunji at turn {actualTurn}'s Ending.");
        var duel = Facts<CaiYangXunjiIssuedEvent>(game).Single().Origin;
        Require(duel.ActualTurn == actualTurn && duel.Target == 1 && duel.GrantId == grant.FrameId &&
            duel.Source is { OwnerSeat: 0, SkillId: Xunji, BindingId: "next-black-turn-ending-duel" } &&
            duel.Source.SkillInstanceId == grant.Source.SkillInstanceId &&
            game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == duel.UseId).Action is
                { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0 },
            "The color qualification issues the original owner-to-selected-seat native Duel once, with no invented material.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.Count == 0);
        Reach(game, _ => TurnEnded(game, actualTurn));
        Require(Facts<CaiYangXunjiReturnedEvent>(game).SequenceEqual([new(duel.FrameId, duel.UseId, duel.ActionId)]) &&
            Facts<CaiYangXunjiConsumedEvent>(game).SequenceEqual([new(grant.FrameId, actualTurn, true)]) &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == duel.UseId && fact.CardKind == CardKind.Duel) == 1 &&
            Facts<CaiYangXunjiDamageBoundEvent>(game).Single() is { UseId: var damageUse, SourceSeat: 0, TargetSeat: 1, Amount: 1 } &&
            damageUse == duel.UseId && Facts<DamageAppliedEvent>(game).Count(fact => fact is { SourceSeat: 1, TargetSeat: 0, Amount: 1 }) == 1,
            "Cold-resumed Xunji completes that actual Duel and its equal reverse damage once.");
        _ = Cold(game, content);
    }

    private static CardActionContext UseFuhun(GameEngine game, int[] materials, bool? expectedRed)
    {
        Accept(game, new UseProgramSkillCommand(1, Fuhun, "two-hand-cards-as-slash", materials, [2], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { Kind: DecisionKind.RespondDodge, PlayerSeat: 2 });
        var appearance = Facts<CardUseAppearanceCapturedEvent>(game).Single(fact => fact.Action.ActorSeat == 1 &&
            fact.Action.ConversionChain.Any(source => source.SkillId == Fuhun)).Action;
        Require(appearance is { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, EffectiveSuit: null or Suit.None } &&
            appearance.EffectiveIsRed == expectedRed && appearance.PhysicalCards.Select(card => card.CardId).Order().SequenceEqual(materials.Order()) &&
            game.CardMovements.Where(move => move.Reason == CardMoveReasons.Use && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing)
                .Select(move => move.CardId).Order().SequenceEqual(materials.Order()),
            $"Native Fuhun must freeze the real combined color {expectedRed} independently of suit and pay both exact cards once.");
        Answer(game, choice => choice.Cards.Count == 1);
        PlayPhase(game, 1);
        return appearance;
    }

    private static bool IsDuelResponse(PendingDecision? prompt) => prompt is { Kind: DecisionKind.RespondSlash, PlayerSeat: 1 };
    private static bool TurnEnded(GameEngine game, int turn) => Facts<TurnEndedEvent>(game).Any(fact => fact is { ActorSeat: 1 } && fact.TurnNumber == turn);
    private static void PlayPhase(GameEngine game, int seat) =>
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard } && prompt.PlayerSeat == seat);
    private static void EndPlay(GameEngine game, int seat) => Accept(game, new EndPlayPhaseCommand(seat, game.Revision, P(game)!.PromptId));
    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void Reach(GameEngine game, Func<PendingDecision?, bool> reached)
    {
        for (var step = 0; step < 100; step++)
        {
            var prompt = P(game);
            if (reached(prompt))
                return;
            Require(prompt is null, "Unexpected Xunji decision: " + prompt?.Prompt);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Xunji did not reach the expected actual turn or native return.");
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt is { Kind: DecisionKind.SelectGeneral or DecisionKind.PlayCard });
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt is { Kind: DecisionKind.SelectGeneral or DecisionKind.PlayCard });
        }
        Require(P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The fixed identity opening must give Cai Yang the first actual Play.");
        return game;
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold commands preserve the selected actual turn, Fuhun's paid color and the original native Duel prompt.");
        return restored;
    }

    private sealed class Fixture(IReadOnlyList<ContentDeckPhysicalCard> stock) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-xunji-effective-color", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, Chosen, "classic:guan-yu", "classic:zhang-fei");
            builder.AddGeneral(new(Generals[2], "真实出闪的目标", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Generals[3], "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定手牌实体", 3, 2, []) { PhysicalCards = stock });
            builder.AddMode(new(Mode, "寻嫉组合牌颜色", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
