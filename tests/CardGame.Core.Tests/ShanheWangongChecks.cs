using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheWangongChecks
{
    public static void PreviousActualCardControlsPaidSlashDistanceQuotaAndDamage()
    {
        var s = Scene.Start([
            new(CardKind.Slash, Suit.Spade, 7), new(CardKind.ThunderSlash, Suit.Club, 8),
            new(CardKind.FireSlash, Suit.Heart, 9), new(CardKind.DrawTwo, Suit.Diamond, 3),
            new(CardKind.Slash, Suit.Club, 4), new(CardKind.Slash, Suit.Spade, 5)]);
        Need(!s.Actions(s.Origin(0)).Any(action => action.TargetSeats.Contains(2)),
            "Without a previous actual basic card, the opening Slash retains its ordinary distance limit.");
        s.Predict(s.Origin(0), 1, 1);
        var first = s.Card(s.Origin(0), 1); s.Play();
        s.Damage(first, 1, 1);
        Need(!s.Captures().Any(fact => fact.Receipt.UseFrameId == first),
            "The opening Slash cannot manufacture a previous basic card from its own accepted use.");

        s.Predict(s.Origin(1), 2, 2);
        var second = s.Card(s.Origin(1), 2); s.Play();
        var third = s.Card(s.Origin(2), 2); s.Play();
        s.Damage(second, 2, 2); s.Damage(third, 2, 2);
        Need(s.Captures().Count(fact => fact.Receipt.UseFrameId == second || fact.Receipt.UseFrameId == third) == 2,
            "Actual consecutive basic Slashes exceed the ordinary quota and reach the seat outside attack range, each with one captured bonus.");
        foreach (var index in new[] { 0, 1, 2 }) s.PaidOnce(s.Origin(index));

        s.Card(s.Origin(3), 0); s.Play();
        Need(!s.Actions(s.Origin(4)).Any(),
            "An intervening actual trick replaces the preceding basic card immediately and removes the extra Slash quota.");
        s.NextTurn();
        Need(!s.Actions(s.Origin(4)).Any(action => action.TargetSeats.Contains(2)),
            "The previous non-basic card still denies unrestricted distance after the phase and turn boundaries.");
        var plain = s.Card(s.Origin(4), 1); s.Play();
        s.Damage(plain, 1, 1);
        Need(!s.Captures().Any(fact => fact.Receipt.UseFrameId == plain),
            "A Slash immediately following the earlier trick deals its ordinary damage, even in a new turn.");
        var renewed = s.Card(s.Origin(5), 2); s.Play();
        s.Damage(renewed, 2, 2); s.PaidOnce(s.Origin(4)); s.PaidOnce(s.Origin(5));
    }

    public static void LastActualBasicSurvivesTurnBoundaryWithoutKeepingAlcoholBonus()
    {
        var s = Scene.Start([new(CardKind.Alcohol, Suit.Spade, 9),
            new(CardKind.Slash, Suit.Club, 7), new(CardKind.Slash, Suit.Spade, 8)]);
        var alcohol = s.Card(s.Origin(0), 0); s.Play(); s.NextTurn();
        Need(!s.Player().HasAlcoholEffect,
            "The earlier Alcohol's native damage boost expires at the next actual turn.");
        var first = s.Card(s.Origin(1), 2); s.Play();
        var receipt = s.Captures().Single(fact => fact.Receipt.UseFrameId == first).Receipt;
        Need(receipt.Source.SkillId == StandardShanheWangongPackage.SkillId &&
            receipt.PreviousOriginFrameId == alcohol,
            "The new turn's first out-of-range Slash captures the owner's exact previous-turn basic use instead of a phase-local history or another actor's use.");
        s.Damage(first, 2, 2); s.PaidOnce(s.Origin(0)); s.PaidOnce(s.Origin(1));
        var second = s.Card(s.Origin(2), 2); s.Play();
        s.Damage(second, 2, 2); s.PaidOnce(s.Origin(2));
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        private GameEngine Game { get; } = game;
        private PendingDecision? P => Game.PendingDecision;
        public PlayerSnapshot Player(int seat = 0) => Game.CreateSnapshot(0).Players[seat];
        private T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public WushuangWangongUseCapturedEvent[] Captures() => Events<WushuangWangongUseCapturedEvent>();
        public int Origin(int index) => Game.CardMovements.Where(move => move.From == CardLocation.OutsideGame &&
            move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal)
            .OrderBy(move => move.Sequence).ElementAt(index).CardId;
        public LegalAction[] Actions(int id) => Game.GetHumanLegalActions().Where(action => action.CardId == id).ToArray();

        public void Predict(int id, int target, int expected)
        {
            var action = Actions(id).First(candidate => candidate.TargetSeats.SequenceEqual([target]));
            var revision = Game.Revision;
            var uses = Events<CardActionAcceptedEvent>().Length;
            var captures = Captures().Length;
            Need(Game.PredictPublicActionDamage(0, action, target).DamageAmount == expected &&
                Game.Revision == revision && Events<CardActionAcceptedEvent>().Length == uses && Captures().Length == captures,
                "The visible proposed damage includes exactly the available Wangong bonus without issuing or consuming a use.");
        }

        public long Card(int id, int target)
        {
            var action = Actions(id).FirstOrDefault(action => action.TargetSeats.SequenceEqual([target])) ??
                Actions(id).FirstOrDefault(action => action.TargetSeats.Count == 0 && target == 0);
            Need(action is not null, "The exact paid card must be legal at its native distance and quota gates.");
            Accept(new PlayCardCommand(0, id, action!.TargetSeats, Game.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            return Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == id && fact.SourceSeat == 0).ResolutionId;
        }

        public void Damage(long use, int target, int amount) => Need(
            Events<DamageAppliedEvent>().Count(fact => fact.SourceCardUseResolutionId == use &&
                fact.SourceSeat == 0 && fact.TargetSeat == target && fact.Amount == amount) == 1,
            "The accepted Slash must cause exactly its expected native damage once.");

        public void PaidOnce(int id) => Need(
            Events<CardActionAcceptedEvent>().Count(fact => fact.Action.ActorSeat == 0 &&
                fact.Action.Type == CardActionType.Use && fact.Action.PhysicalCards.Any(card => card.CardId == id)) == 1 &&
            Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                move.To.Zone == CardZoneKind.Processing) == 1 &&
            Game.CardMovements.Count(move => move.CardId == id && move.To == CardLocation.DiscardPile) == 1 &&
            Player().Hand.All(card => card.Id != id),
            "The privilege keeps the real physical-card cost: one accepted hand entity, one processing payment and one ordinary discard.");

        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void NextTurn()
        {
            var turn = Game.CreateSnapshot(0).TurnNumber;
            Accept(new EndPlayPhaseCommand(0, Game.Revision, P!.PromptId));
            Reach(() => Game.CreateSnapshot(0).TurnNumber > turn && P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        }

        private void Accept(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Wangong fixture command was rejected.");
        }
        private void Reach(Func<bool> done)
        {
            for (var step = 0; step < 700; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(choice =>
                        choice.Parameters.GetValueOrDefault("program-action") == "skip" ||
                        choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(choice is not null, "The fixed Wangong flow must not enter an unrelated player decision: " + prompt.Kind);
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The native Wangong flow did not reach its expected boundary.");
        }

        public static Scene Start(ShanheStartingCard[] cards)
        {
            var setup = new ShanheBattleSetup(83, 1, "fixture:shanhe-wangong", StandardShanheLocalTroopPackage.FootmanId,
                30, 30, 0, 0, 0, [StandardShanheWangongPackage.SkillId], cards, [],
                Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(StandardShanheLocalTroopPackage.FootmanId,
                    MaxHpBonus: 30, Hp: 30) { StartingHand = [] }).ToArray(), [], new string('a', 64));
            var registry = ContentRegistry.Build(new FixedDeck(StandardShanheCampaignCatalog.CreateBattleRegistry(setup)));
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = true,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    // Actual Shanhe seats and skills, with a small all-Slash draw pile so later
    // AI turns cannot randomly steal the paid fixture cards or create a Dodge.
    private sealed class FixedDeck(ContentRegistry source) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shanhe-wangong-deck", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.CopyModeFeatureFactoriesFrom(source);
            foreach (var card in source.Cards.Values) builder.AddCard(card);
            foreach (var skill in source.Skills.Values) builder.AddSkill(skill);
            foreach (var general in source.Generals.Values) builder.AddGeneral(general);
            foreach (var deck in source.Decks.Values)
                builder.AddDeck(deck.Id == source.Modes[ShanheBattleSetup.BattleModeId].DeckId
                    ? deck with { Cards = [], PhysicalCards = Enumerable.Range(0, 96)
                        .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() }
                    : deck);
            foreach (var mode in source.Modes.Values) builder.AddMode(mode);
        }
    }
}
