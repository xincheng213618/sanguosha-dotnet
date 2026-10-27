using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheCounterspellClaimChecks
{
    private const string Owner = "fixture:counterspell-claim-owner", Peer = "fixture:counterspell-claim-peer";
    private const string IdentityOwner = "fixture:counterspell-claim-identity-owner", Identity = "fixture:counterspell-claim-identity";
    private const string Watch = "fixture:counterspell-claim-gain", Conversion = "fixture:counterspell-claim-pair", Deck = "fixture:counterspell-claim-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheCounterspellClaimPackage(), new StandardShanheMultiTargetTricksPackage(), new Fixture()));

    public static void PaidClaimArrivesBeforeNextSequentialTargetAndNeverMovesAgain()
    {
        var s = Scene.Start();
        var baseline = s.BeginDuel(1, [1]); s.Run([0], 1);
        Need(s.Events<ShanheNullifiedTrickMaterialClaimedEvent>().Length == 0 &&
            s.G.CardMovements.Count(move => move.CardId == baseline && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1,
            "Before actual ownership, a genuine counterspell leaves its original Duel on the normal discard path.");
        s.Buy();
        var card = s.BeginDuel(2, [1, 2]); s.Run([0], 1);
        var use = s.Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == card);
        var facts = s.Events<ShanheNullifiedTrickMaterialClaimedEvent>().Where(fact => fact.CardUseFrameId == use.ResolutionId).ToArray();
        Need(facts is [{ RecipientSeat: 0, CardId: var claimed }] && claimed == card && s.ClaimChildren == 1 &&
            s.G.CreateSnapshot(0).Players.Select(player => player.Hp).SequenceEqual([30, 30, 29, 30]) &&
            s.Events<NullificationResolvedEvent>().Where(fact => fact.ResolutionId == use.ResolutionId)
                .Select(fact => (fact.TargetSeat, fact.EffectNullified)).SequenceEqual([(1, true), (2, false)]),
            "The first canceled Duel target gives the exact card immediately, drains its real gain child, then the second target still takes its independent native Duel damage.");
        s.AssertClaim(use.ResolutionId, [card], 0);
        Need(s.G.CreateSnapshot(0).Players[0].Hand.Any(value => value.Id == card) &&
            s.Events<DuelResponseEvent>().Where(fact => fact.ResolutionId == use.ResolutionId).All(fact => fact.ResponderSeat != 1),
            "A canceled target never supplies a Slash, while the claimed original remains in its real recipient's hand after all targets finish.");
    }

    public static void FinalCounterspellChainSelectsLastHolderAndEvenChainDoesNotClaim()
    {
        var s = Scene.Start(peerClaimant: true); s.Buy();
        var restored = s.BeginDuel(1, [2]); s.Run([0, 1], 2);
        var firstUse = s.Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == restored);
        Need(s.Events<NullificationRespondedEvent>().Where(fact => fact.ResolutionId == firstUse.ResolutionId)
                .Select(fact => (fact.ResponderSeat, fact.EffectNullified)).SequenceEqual([(0, true), (1, false)]) &&
            s.Events<ShanheNullifiedTrickMaterialClaimedEvent>().Length == 0 && s.ClaimChildren == 0 &&
            s.G.CardMovements.Count(move => move.CardId == restored && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1,
            "An actual counter-counterspell restores the trick: neither holder claims its entity at an earlier provisional odd depth.");

        var canceled = s.BeginDuel(2, [2]); s.Run([0, 2, 1], 2);
        var use = s.Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == canceled);
        Need(s.Events<NullificationRespondedEvent>().Where(fact => fact.ResolutionId == use.ResolutionId)
                .Select(fact => (fact.ResponderSeat, fact.ChainDepth, fact.EffectNullified))
                .SequenceEqual([(0, 1, true), (2, 2, false), (1, 3, true)]) &&
            s.Events<ShanheNullifiedTrickMaterialClaimedEvent>().Single(fact => fact.CardUseFrameId == use.ResolutionId).RecipientSeat == 1 &&
            !s.G.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == canceled) &&
            s.G.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == canceled),
            "The genuine final third counterspell belongs to the other holder, who receives the original entity instead of the first counterspell user.");
        s.AssertClaim(use.ResolutionId, [canceled], 1);
    }

    public static void MultiMaterialConversionClaimsEveryOriginalAndOrdinaryModeStaysIsolated()
    {
        var s = Scene.Start(); s.Buy();
        var materials = s.Hand(0).Where(card => card.Kind == CardKind.Dodge && card.Suit == Suit.Club).Select(card => card.Id).Take(2).ToArray();
        Need(materials.Length == 2, "The native pair conversion must pay two actual same-suit materials.");
        var use = s.Convert(materials); s.Run([0], 1);
        Need(s.Events<ProgramViewAsConvertedEvent>().Single(fact => fact.FrameId == use).PhysicalCardIds.SequenceEqual(materials) &&
            s.Events<ShanheNullifiedTrickMaterialClaimedEvent>().Where(fact => fact.CardUseFrameId == use)
                .Select(fact => fact.CardId).Order().SequenceEqual(materials.Order()) && s.ClaimChildren == 1 &&
            s.Events<DamageAppliedEvent>().Where(fact => fact.SourceCardUseResolutionId == use).Select(fact => fact.TargetSeat).SequenceEqual([2, 3]),
            "One actual pair-as-Arrow-Barrage use transfers both original entities in its first canceled target's gain boundary, while the remaining two targets resolve natively.");
        s.AssertClaim(use, materials, 0);

        var ordinary = Scene.StartIdentity();
        Need(ordinary.G.CreateSnapshot(0).Players[0].Skills.Any(skill => skill.Id == StandardShanheCounterspellClaimPackage.SkillId),
            "The ordinary identity user really holds this same program, so the isolation check cannot pass because it was absent.");
        var ordinaryMaterials = ordinary.Hand(0).Take(2).Select(card => card.Id).ToArray();
        var ordinaryUse = ordinary.Convert(ordinaryMaterials); ordinary.Run([0], 1);
        Need(ordinary.Events<ShanheNullifiedTrickMaterialClaimedEvent>().Length == 0 && ordinary.ClaimChildren == 0 &&
            ordinaryMaterials.All(id => ordinary.G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished) == 1) &&
            ordinary.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == ordinaryUse) == 1,
            "Even the actually held policy does not acquire canceled materials in an ordinary identity game; both original costs keep their native finish movements.");
    }

    public static void DelayedJudgmentClaimsBothPhaseTricksAndLightningBeforeNaturalContinuation()
    {
        var s = Scene.Start(delayedCards: true); s.Buy();
        var indulgence = s.PlaceDelayed(CardKind.Indulgence, 1);
        var shortage = s.PlaceDelayed(CardKind.SupplyShortage, 1);
        s.EndPlay();
        s.DelayedUntilPlay(1, new Dictionary<int, int[]> { [indulgence] = [0], [shortage] = [0] });
        Need(s.DelayedClaimChildren == 2 && s.Events<ShanheDelayedNullifiedTrickMaterialClaimedEvent>()
                .Select(fact => (fact.CardId, fact.EffectiveKind, fact.JudgmentOwnerSeat, fact.RecipientSeat))
                .SequenceEqual([(indulgence, CardKind.Indulgence, 1, 0), (shortage, CardKind.SupplyShortage, 1, 0)]) &&
            !s.Events<DelayedCardResolvedEvent>().Any(fact => fact.CardId == indulgence || fact.CardId == shortage) &&
            s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 },
            "The two actual judgment-zone tricks each arrive in hand before their real gain child, then the second delayed effect and the holder's normal play phase still proceed without either canceled judgment.");
        s.AssertDelayedClaim(indulgence, 1); s.AssertDelayedClaim(shortage, 1);

        var lightningScene = Scene.Start(delayedCards: true); lightningScene.Buy();
        var lightning = lightningScene.PlaceDelayed(CardKind.Lightning, 0);
        lightningScene.EndPlay();
        lightningScene.DelayedUntilPlay(0, new Dictionary<int, int[]> { [lightning] = [0] });
        Need(lightningScene.DelayedClaimChildren == 1 && lightningScene.Events<LightningResolvedEvent>()
                .Single(fact => fact.CardId == lightning) is { Hit: false, NextTargetSeat: null, JudgmentCardId: null, DamageAmount: 0 } &&
            !lightningScene.G.CardMovements.Any(move => move.CardId == lightning && move.Reason == CardMoveReasons.DelayedCardTransfer) &&
            lightningScene.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 },
            "The actual physical Lightning is acquired directly instead of transferred to the next judgment zone, and its owner's next natural play phase remains reachable.");
        lightningScene.AssertDelayedClaim(lightning, 0);

        var restored = Scene.Start(delayedCards: true); restored.Buy();
        var original = restored.PlaceDelayed(CardKind.Indulgence, 1);
        restored.EndPlay(); restored.DelayedUntilPlay(1, new Dictionary<int, int[]> { [original] = [0, 1] });
        Need(restored.DelayedClaimChildren == 0 && restored.Events<ShanheDelayedNullifiedTrickMaterialClaimedEvent>().Length == 0 &&
            restored.Events<DelayedCardResolvedEvent>().Single(fact => fact.CardId == original) is
                { CardKind: CardKind.Indulgence, JudgmentSucceeded: true, SkippedPlayPhase: false } &&
            restored.G.CardMovements.Count(move => move.CardId == original && move.From == CardLocation.Judgment(1) &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.DelayedCardFinish) == 1,
            "A real counter-counterspell restores the held Indulgence, whose genuine heart judgment and normal delayed finish remain unchanged; no provisional claim steals it.");
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public int ClaimChildren { get; private set; }
        public int DelayedClaimChildren { get; private set; }
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(value => value is not null);
        public IReadOnlyList<CardSnapshot> Hand(int seat) => G.CreateSnapshot(seat).Players[seat].Hand;
        public T[] Events<T>() => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native counterspell-claim command."); }
        public void Run(int[]? counterspellSeats = null, int? targetSeat = null)
        {
            counterspellSeats ??= []; var counterIndex = 0;
            var responseCountBefore = Events<NullificationRespondedEvent>().Length;
            for (var step = 0; step < 300; step++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0)
                {
                    Need(counterIndex == counterspellSeats.Length, "Every planned genuine counterspell must actually have paid its own card.");
                    foreach (var response in Events<NullificationRespondedEvent>().Skip(responseCountBefore))
                        Need(G.CardMovements.Count(move => move.CardId == response.NullificationCardId &&
                                move.From == CardLocation.Hand(response.ResponderSeat) && move.To == CardLocation.Processing) == 1 &&
                            G.CardMovements.Count(move => move.CardId == response.NullificationCardId && move.From == CardLocation.Processing &&
                                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.NullificationFinished) == 1,
                            "Every actual counterspell and counter-counterspell pays and finishes its responder's own exact physical entity once.");
                    return;
                }
                if (P is not { } prompt) { Accept(new AdvanceOneStepCommand(G.Revision)); continue; }
                PromptChoice? choice = null;
                if (prompt.Kind == DecisionKind.Nullification)
                {
                    var window = G.ResolutionStack.OfType<NullificationWindowFrame>().Last();
                    if (counterIndex < counterspellSeats.Length && prompt.PlayerSeat == counterspellSeats[counterIndex] &&
                        window.TargetSeats.Contains(targetSeat!.Value))
                    {
                        var id = Hand(prompt.PlayerSeat).First(card => card.Kind == CardKind.Nullification).Id;
                        choice = prompt.Choices.FirstOrDefault(value => value.Cards.SequenceEqual([id]));
                        Need(choice is not null, "The planned real Nullification entity must be in the native response choices.");
                        counterIndex++;
                    }
                    else choice = prompt.Choices.FirstOrDefault(value => value.Parameters.GetValueOrDefault("response") == "pass");
                }
                else if (prompt.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge)
                    choice = prompt.Choices.FirstOrDefault(value => value.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                else if (prompt.Kind == DecisionKind.ProgramTrigger)
                {
                    ObserveClaimChild();
                    choice = prompt.Choices.FirstOrDefault(value => value.Parameters.GetValueOrDefault("option") == "continue") ?? prompt.Choices.FirstOrDefault();
                }
                Need(choice is not null, $"Unexpected counterspell-claim decision {prompt.Kind} for seat {prompt.PlayerSeat}; frames={string.Join(',', G.ResolutionStack.Select(frame => frame.GetType().Name))}.");
                Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice!.Id, G.Revision));
            }
            throw new InvalidOperationException($"Counterspell-claim flow did not return: {P?.Kind}/{P?.PlayerSeat}; frames={string.Join(',', G.ResolutionStack.Select(frame => frame.GetType().Name))}.");
        }
        public void Buy()
        {
            var market = G.CreateShanheMarketSnapshot(0)!;
            var item = market.ShopItems.Single(value => value.Reward?.Id == StandardShanheCampaignCatalog.CurrentCaochuanjiejianRewardId);
            Need(item.GoldCost == 300, "The published real rare tactic costs its source's three hundred copper.");
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision)); Run();
            Need(G.CreateShanheMarketSnapshot(0)!.Gold == market.Gold - 300 &&
                Events<ShanheMarketPurchaseCommittedEvent>() is [{ Price: 300 }] &&
                Events<ShanheMarketPurchaseAppliedEvent>().Single().RewardId == StandardShanheCampaignCatalog.CurrentCaochuanjiejianRewardId,
                "The actual purchase must charge once and grant this policy immediately in the live battle.");
        }
        public int BeginDuel(int rank, int[] targets)
        {
            var card = Hand(0).Single(value => value.Kind == CardKind.Duel && value.Rank == rank);
            var action = G.GetHumanLegalActions().Single(value => value.CardId == card.Id && value.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind)); return card.Id;
        }
        public long Convert(int[] materials)
        {
            Accept(new UseProgramSkillCommand(0, Conversion, "pair", materials, [], G.Revision, P!.PromptId));
            return Events<CardUseDeclaredEvent>().Last().ResolutionId;
        }
        public int PlaceDelayed(CardKind kind, int target)
        {
            var card = Hand(0).Single(value => value.Kind == kind);
            var action = G.GetHumanLegalActions().First(value => value.CardId == card.Id &&
                (value.TargetSeats.SequenceEqual([target]) || kind == CardKind.Lightning && target == 0 && value.TargetSeats.Count == 0));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind)); Run();
            Need(G.CardMovements.Count(move => move.CardId == card.Id && move.From == CardLocation.Processing && move.To == CardLocation.Judgment(target)) == 1 &&
                Events<CardUseFinishedEvent>().Count(fact => fact.CardId == card.Id) == 1,
                "The delayed card must first finish a real native use and placement, rather than being inserted into a fake judgment state.");
            return card.Id;
        }
        public void EndPlay()
        { var prompt = P!; Accept(new EndPlayPhaseCommand(prompt.PlayerSeat, G.Revision, prompt.PromptId)); }
        public void DelayedUntilPlay(int targetPlaySeat, IReadOnlyDictionary<int, int[]> plans)
        {
            var paid = plans.Keys.ToDictionary(id => id, _ => 0);
            var responsesBefore = Events<NullificationRespondedEvent>().Length;
            for (var step = 0; step < 360; step++)
            {
                if (P is { Kind: DecisionKind.PlayCard } play && play.PlayerSeat == targetPlaySeat && G.ResolutionStack.Count == 0)
                {
                    Need(plans.All(plan => paid[plan.Key] == plan.Value.Length), "Every intended delayed-window response must really pay its own card.");
                    foreach (var response in Events<NullificationRespondedEvent>().Skip(responsesBefore))
                        Need(G.CardMovements.Count(move => move.CardId == response.NullificationCardId &&
                                move.From == CardLocation.Hand(response.ResponderSeat) && move.To == CardLocation.Processing) == 1 &&
                            G.CardMovements.Count(move => move.CardId == response.NullificationCardId && move.From == CardLocation.Processing &&
                                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.NullificationFinished) == 1,
                            "Every delayed-window counterspell and counter-counterspell pays and finishes the responder's own exact entity once.");
                    return;
                }
                if (P is not { } prompt) { Accept(new AdvanceOneStepCommand(G.Revision)); continue; }
                if (prompt.Kind == DecisionKind.PlayCard) { EndPlay(); continue; }
                PromptChoice? choice = null;
                if (prompt.Kind == DecisionKind.Nullification)
                {
                    var window = G.ResolutionStack.OfType<NullificationWindowFrame>().Last();
                    if (window.DelayedReturn is { } delayed && plans.TryGetValue(delayed.CardId, out var responseSeats) &&
                        paid[delayed.CardId] < responseSeats.Length && prompt.PlayerSeat == responseSeats[paid[delayed.CardId]])
                    {
                        var id = Hand(prompt.PlayerSeat).First(card => card.Kind == CardKind.Nullification).Id;
                        choice = prompt.Choices.Single(value => value.Cards.SequenceEqual([id])); paid[delayed.CardId]++;
                    }
                    else choice = prompt.Choices.First(value => value.Parameters.GetValueOrDefault("response") == "pass");
                }
                else if (prompt.Kind == DecisionKind.ProgramTrigger)
                { ObserveClaimChild(); choice = prompt.Choices.First(); }
                Need(choice is not null, $"Unexpected delayed continuation {prompt.Kind}/{prompt.PlayerSeat}.");
                Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice!.Id, G.Revision));
            }
            throw new InvalidOperationException($"Delayed claim did not reach natural Play {targetPlaySeat}: {P?.Kind}/{P?.PlayerSeat}.");
        }
        private void ObserveClaimChild()
        {
            var window = G.ResolutionStack.OfType<NullificationWindowFrame>().Last();
            Need(G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.ResumeShanheNullifiedClaimWindowId == window.Id),
                "The genuine cards-gained program must be a child of the original awaited counterspell window.");
            if (window.ShanheDelayedNullifiedTrickClaim is { } delayed)
            {
                Need(window.DelayedReturn == delayed.Origin && Hand(delayed.RecipientSeat).Any(card => card.Id == delayed.Origin.CardId) &&
                    !Events<NullificationResolvedEvent>().Any(fact => fact.WindowFrameId == window.Id) &&
                    !Events<DelayedCardResolvedEvent>().Any(fact => fact.CardId == delayed.Origin.CardId) &&
                    !Events<LightningResolvedEvent>().Any(fact => fact.CardId == delayed.Origin.CardId),
                    "The real delayed gain child sees the exact entity already in hand and its unchanged delayed token before the effect or natural phase can finish.");
                DelayedClaimChildren++; return;
            }
            var use = G.ResolutionStack.OfType<CardUseFrame>().Last(frame => frame.ShanheNullifiedTrickClaim is not null);
            var claim = use.ShanheNullifiedTrickClaim!;
            Need(claim.WindowFrameId == window.Id && claim.CardIds.All(id => Hand(claim.RecipientSeat).Any(card => card.Id == id)) &&
                !Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == use.Id) &&
                !Events<DamageAppliedEvent>().Any(fact => fact.SourceCardUseResolutionId == use.Id),
                "The real cards-gained child sees every original material in hand on its still-live original use before later targets or completion.");
            ClaimChildren++;
        }
        public void AssertDelayedClaim(int cardId, int judgmentOwner)
        {
            Need(G.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Judgment(judgmentOwner) &&
                    move.To == CardLocation.Hand(0) && move.Reason.Value == "shanhe.counterspell-claim") == 1 &&
                !G.CardMovements.Any(move => move.CardId == cardId && move.From == CardLocation.Judgment(judgmentOwner) &&
                    move.To != CardLocation.Hand(0)) && Hand(0).Any(card => card.Id == cardId),
                "The actual held delayed entity moves directly from its judgment zone to hand once and is never subsequently discarded or transferred by native delayed completion.");
        }
        public void AssertClaim(long use, int[] materials, int recipient)
        {
            Need(materials.All(id => G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Processing && move.To == CardLocation.Hand(recipient)) == 1 &&
                !G.CardMovements.Any(move => move.CardId == id && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile)) &&
                Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use) == 1 &&
                materials.All(id => Hand(recipient).Any(card => card.Id == id)),
                "Every original physical material pays once, transfers directly from Processing once, and never moves again on original-use completion.");
        }
        public static Scene Start(bool peerClaimant = false, bool delayedCards = false)
        {
            ShanheRewardDefinition[] rewards =
            [new(StandardShanheCampaignCatalog.CurrentCaochuanjiejianRewardId, "草船借箭")
                { TacticSkillId = StandardShanheCounterspellClaimPackage.SkillId, Rarity = 2 },
             new("duel-targets", "原生多目标决斗") { TacticSkillId = StandardShanheMultiTargetTricksPackage.DantiaowangSkillId },
             new("cards", "实体牌", StartingCards:
                [new(CardKind.Duel, Suit.Heart, 1), new(CardKind.Duel, Suit.Diamond, 2),
                 new(CardKind.Dodge, Suit.Club, 4), new(CardKind.Dodge, Suit.Club, 5),
                 new(CardKind.Nullification, Suit.Spade, 6), new(CardKind.Nullification, Suit.Spade, 7),
                 new(CardKind.Nullification, Suit.Spade, 8), new(CardKind.Nullification, Suit.Spade, 9),
                 .. (delayedCards ? new[] { new ShanheStartingCard(CardKind.Indulgence, Suit.Heart, 10),
                     new ShanheStartingCard(CardKind.SupplyShortage, Suit.Club, 11), new ShanheStartingCard(CardKind.Lightning, Suit.Spade, 12) } : [])])];
            var definition = new ShanheCampaignDefinition("fixture:counterspell-claim", "原生无懈取牌", [Owner], ["cards"],
                [new("cards", "实体补给", ShanheNodeKind.Event, ["shop", "battle"], Choices: [new("take", "领取", RewardId: "cards")]),
                 new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("claim", "草船借箭", 300, RewardId: rewards[0].Id, LeavesNode: false)]),
                 new("battle", "敌友", ShanheNodeKind.Battle, [],
                    [new("peers", "实际原生响应", [new(Peer) { StartingHand = [new(CardKind.Nullification, Suit.Club, 10)] },
                        new(Peer) { StartingHand = [] }], Allies: [new(Peer)
                        { StartingHand = [new(CardKind.Nullification, Suit.Club, 11), new(CardKind.Nullification, Suit.Spade, 12)],
                          ExtraSkillIds = peerClaimant ? [StandardShanheCounterspellClaimPackage.SkillId, Watch] : [] }])])], rewards, StartingGold: 1000);
            var session = ShanheCampaignSession.Create(definition, new(31, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true, EquippedTacticIds = ["duel-targets"] }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { revision => new MoveShanheNodeCommand("cards", revision), revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                  revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = session.Submit(command(session.Revision)); Need(result.Accepted, result.Error ?? "Cannot enter native claim fixture."); }
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, Registry.Value);
            var s = new Scene(game); s.Accept(new StartGameCommand()); s.Run(); return s;
        }
        public static Scene StartIdentity()
        {
            var game = GameEngine.CreateStandard(new()
            {
                Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Identity, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, Registry.Value);
            var s = new Scene(game); s.Accept(new StartGameCommand());
            for (var step = 0; step < 80; step++)
            {
                if (s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return s;
                if (s.P is { Kind: DecisionKind.SelectGeneral } prompt)
                    s.Accept(new SelectGeneralCommand(prompt.PlayerSeat, prompt.PlayerSeat == 0 ? IdentityOwner : prompt.ValidContentIds.First(), game.Revision, prompt.PromptId));
                else s.Accept(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("Ordinary identity setup did not reach its native play boundary.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:counterspell-claim", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var mode in new[] { ShanheBattleSetup.BattleModeId, Identity })
            { builder.AddModeFeatureFactory(new HumanSetupFactory(mode)); builder.AddModeFeatureFactory(new HumanProfileFactory(mode)); }
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Conversion}}","revision":1,
                   "viewAs":[{"id":"pair","inputKinds":[],"inputSuits":[],"inputCount":2,"sameSuit":true,"outputKind":"arrowBarrage","forPlay":true,"forResponse":false}],
                   "activations":[{"id":"pair","usesPerTurn":null,"minCards":2,"maxCards":2,"sourceZones":["hand"],"selectedCardsSameSuit":true,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving",
                     "effects":[{"op":"useSelectedCardsAs","target":"owner","sourceBind":"pair","outputKind":"arrowBarrage"}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"actual-claim-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                   "movementOccurrence":"perBatch","movementReasons":["shanhe.counterspell-claim"],"optional":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var bundle = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Conversion] = new { name = "双实体万箭", description = "同花色实体牌转万箭。" },
                    [Watch] = new { name = "实际获得牌子结算", description = "原生获得牌后返回原无懈。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                }
            }));
            foreach (var id in new[] { Conversion, Watch })
                builder.AddSkill(new(id, bundle.Presentations[id].Name, bundle.Presentations[id].Description)
                { Program = bundle.Programs[id], ProgramPresentation = bundle.Presentations[id] });
            builder.AddGeneral(new(Owner, "原生锦囊使用者", "supporter", Watch, "shu", 30, [Conversion]));
            builder.AddGeneral(new(Peer, "独立响应者", "supporter", "standard:none", "wei", 30));
            builder.AddGeneral(new(IdentityOwner, "普通身份实际持有者", "supporter", StandardShanheCounterspellClaimPackage.SkillId, "shu", 30, [Conversion, Watch]));
            foreach (var index in new[] { 1, 2, 3 }) builder.AddGeneral(new(Peer + index, "普通响应者", "supporter", "standard:none", "wei", 30));
            builder.AddDeck(new(Deck, "零自然摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "原生草船借箭", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
            builder.AddDeck(new(Deck + "-identity", "普通身份真实转换", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:nullification", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Identity, "普通身份场", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                Deck + "-identity", GeneralCandidateCount: 4, GeneralPoolIds: [IdentityOwner, Peer + "1", Peer + "2", Peer + "3"]));
        }
    }

    // Production setup supplies every roster, grant and deck; only controllers become human.
    private sealed class HumanSetupFactory(string mode) : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:claim-human-setup:" + mode;
        public string ModeId => mode;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase) => new StandardModeSetupFactory().ValidateInitialOptions(options, phase);
        public IGameModeFeature Create(ModeFeatureCreationContext context) => new HumanSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(context));
    }
    private sealed class HumanProfileFactory(string mode) : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:claim-human-profile:" + mode;
        public string ModeId => mode;
        public IGameModeFeature Create(ModeFeatureCreationContext context) => new StandardModeProfileFactory().Create(
            new ModeFeatureCreationContext(context.Options! with { HumanSeats = null }, context.ModeDefinition!, context.ContentRegistry,
                new ModeHostRegistry().Register<IModeSetupRandom>(() => context.GetHost<IModeSetupRandom>())
                    .Register<IModeNationalRevealHost>(() => context.GetHost<IModeNationalRevealHost>())
                    .Register<IModeSetupEquipmentHost>(() => context.GetHost<IModeSetupEquipmentHost>())));
    }
    private sealed class HumanSetup(IMatchModeSetupFeature inner) : IMatchModeSetupFeature
    {
        public ModeSetupPlan BuildPlayers()
        {
            var plan = inner.BuildPlayers();
            return new(plan.Seats.Select(seat => seat with { IsHuman = true }), plan.AvailableGeneralIds,
                plan.SelectionOrder, plan.ShenwuSharedGodIds, plan.ShenwuLeaderGeneralId, plan.GuanduConfiguration, plan.InitialActorSeat);
        }
        public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest request) => inner.GetGeneralOffers(request);
        public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string id) => inner.RemoveGeneralIdsAfterSelection(id);
        public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> original) => inner.PrepareDeck(original);
        public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> original) => inner.ProjectDeckCardNames(original);
        public IReadOnlyList<Type> InitialEquipmentCompletionFacts => inner.InitialEquipmentCompletionFacts;
        public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> drawPile, IReadOnlyList<ModeSeatSetupFacts> seats) => inner.PrepareSelectedEquipment(drawPile, seats);
        public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts seat, string generalId) => inner.PrepareSelectedGeneral(seat, generalId);
    }
}
