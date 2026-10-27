using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheFuyiTacticChecks
{
    private const string Skill = StandardShanheFuyiTacticsPackage.FuyiSlashSkillId;
    private const string Owner = "fixture:fuyi-owner", Enemy = "fixture:fuyi-enemy", Deck = "fixture:fuyi-deck";
    private static readonly CardKind[] SlashKinds = [CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash];
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheFuyiTacticsPackage(), new Fixture()));
    private static readonly Lazy<ContentRegistry> AoeRegistry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheFuyiTacticsPackage(), new Fixture(aoe: true)));

    public static void AoeSupplyObtainsAndUsesActualEntitiesUntilTheMatchingDeckIsExhausted()
    {
        var s = Scene.Create(aoe: true);
        s.Start();
        var first = s.Gains;
        Need(first.Length == 2 && s.View.HandCount == 2 &&
            first.Select(move => move.CardKind).ToHashSet().SetEquals([CardKind.ArrowBarrage, CardKind.BarbarianAssault]) &&
            first.All(move => move.CardId > 0 && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)),
            "The real first owner turn obtains one existing ArrowBarrage and one existing BarbarianAssault through their distinct held tactics.");
        foreach (var move in first) s.UseAoe(move.CardId);
        Need(s.View.HandCount == 0 && s.Events<DamageAppliedEvent>() is
            [{ SourceSeat: 0, TargetSeat: 1, Amount: 1 }, { SourceSeat: 0, TargetSeat: 1, Amount: 1 }] &&
            s.Events<CardUseFinishedEvent>().Length == 2,
            "Both obtained physical AOE cards are legal native uses and each actually damages the empty-handed enemy once.");

        s.NextOwnPlay();
        Need(s.Gains.Length == 3 && s.View.Hand is [{ Kind: CardKind.ArrowBarrage } second] &&
            s.Gains[^1].CardId == second.Id && first.All(move => move.CardId != second.Id),
            "On the next owner turn the second ArrowBarrage is still in the draw pile and is obtained; the spent BarbarianAssault in discard is not fabricated or substituted.");
        s.UseAoe(s.View.Hand.Single().Id);
        var paid = s.Gains;
        s.NextOwnPlay();
        var turns = s.Events<TurnStartedEvent>().ToDictionary(fact => fact.TurnNumber, fact => fact.ActorSeat);
        Need(s.Gains.SequenceEqual(paid) && s.View.HandCount == 0 && s.Game.CreateSnapshot(0).DrawPileCount == 20 &&
            paid.Select(move => move.CardId).Distinct().Count() == 3 && paid.All(move => turns[move.TurnNumber] == 0) &&
            s.Events<CardUseFinishedEvent>().Count(fact => fact.CardKind == CardKind.ArrowBarrage) == 2 &&
            s.Events<CardUseFinishedEvent>().Count(fact => fact.CardKind == CardKind.BarbarianAssault) == 1 &&
            s.Events<DamageAppliedEvent>().Count(fact => fact is { SourceSeat: 0, TargetSeat: 1, Amount: 1 }) == 3 &&
            paid.All(move => s.Game.CardMovements.Count(item => item.CardId == move.CardId &&
                    item.From == CardLocation.Hand(0) && item.To == CardLocation.Processing) == 1 &&
                s.Game.CardMovements.Count(item => item.CardId == move.CardId &&
                    item.From == CardLocation.Processing && item.To == CardLocation.DiscardPile) == 1) &&
            !s.Game.CardMovements.Any(move => move.Reason == CardMoveReasons.Draw || move.From == CardLocation.OutsideGame) &&
            new[] { StandardShanheFuyiTacticsPackage.FuyiArrowBarrageSkillId, StandardShanheFuyiTacticsPackage.FuyiBarbarianAssaultSkillId }
                .All(skill => s.Events<ProgramBindingStartedEvent>().Count(fact => fact.SkillId == skill && fact.OwnerSeat == 0 &&
                    fact.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) == 3),
            "After all three original AOE entities have been obtained, used and discarded once, both held sources still trigger on the third owner turn but obtain nothing; intervening enemy turns and ordinary draw never create replacement cards.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PlayerSnapshot View => Game.CreateSnapshot(0).Players[0];
        public CardMovementRecord[] Gains => Game.CardMovements.Where(move =>
            move.Reason.Value == "skill-program.obtain-deck-cards").ToArray();
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Fuyi command."); }
        public void Start() { Accept(new StartGameCommand()); Play(); }
        public void NextOwnPlay()
        { Accept(new EndPlayPhaseCommand(0, Game.Revision, Game.PendingDecision!.PromptId)); Play(); }
        public void UseAoe(int cardId)
        {
            var action = Game.GetHumanLegalActions().Single(action => action.CardId == cardId &&
                action.Kind is LegalActionKind.ArrowBarrage or LegalActionKind.BarbarianAssault);
            Accept(new PlayCardCommand(0, cardId, action.TargetSeats, Game.Revision, Game.PendingDecision!.PromptId));
            Play();
        }
        private void Play()
        {
            for (var step = 0; step < 140; step++)
            {
                if (Game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(Game.PendingDecision?.PlayerSeat != 0, "The bounded Fuyi integration reached an unexpected human prompt.");
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded Fuyi turn-start boundary.");
        }
        public static Scene Create(bool aoe = false)
        {
            var setup = new ShanheBattleSetup(113, 1, "fixture:fuyi", Owner, 12, 0, 0, 0, 0,
                [], [], [], [new(Enemy) { StartingHand = [] }], [], new string('a', 64))
            {
                TacticSkillIds = aoe
                    ? [StandardShanheFuyiTacticsPackage.FuyiArrowBarrageSkillId, StandardShanheFuyiTacticsPackage.FuyiBarbarianAssaultSkillId]
                    : [Skill]
            };
            return new(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, aoe ? AoeRegistry.Value : Registry.Value));
        }
    }

    private sealed class Fixture(bool aoe = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-fuyi-tactics", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var kind in SlashKinds)
                builder.AddCard(new("fixture:fuyi-" + kind, CardCatalog.Get(kind).DisplayName, "基本牌", CardCatalog.Get(kind).Description, kind));
            builder.AddGeneral(new(Owner, "拂衣去持有者", "supporter", "standard:none", "shu", 12));
            builder.AddGeneral(new(Enemy, "固定守军", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "四种真实杀和非匹配实体闪", 0, 0, [])
            {
                PhysicalCards = (aoe
                    ? new ContentDeckPhysicalCard[] { new("standard:arrow_barrage", Suit.Heart, 1),
                        new("standard:arrow_barrage", Suit.Spade, 10), new("standard:barbarian_assault", Suit.Spade, 13) }
                    : SlashKinds.Select((kind, index) => new ContentDeckPhysicalCard("fixture:fuyi-" + kind,
                        (Suit)index, index + 1)).ToArray()).Concat(Enumerable.Range(0, 20).Select(index =>
                        new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1))).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "拂衣去本人回合实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
