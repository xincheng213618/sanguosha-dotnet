using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSteadyHandChecks
{
    private const string Owner = "fixture:steady-hand-owner", Peer = "fixture:steady-hand-peer";
    private const string Driver = "fixture:steady-hand-driver", Deck = "fixture:steady-hand-deck";
    private const string Identity = "fixture:steady-hand-identity", IdentityOwner = "fixture:steady-hand-identity-owner";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheSteadyHandPackage(), new Fixture()));

    public static void MaximumHpBasePreservesBonusesThroughRealHealthChangesAndNativeDiscard()
    {
        foreach (var held in new[] { false, true })
        {
            var s = Scene.Start(held);
            Need(s.V is { Hp: 5, MaxHp: 5, HandCount: 10, HandLimit: 7 },
                "The full-health baseline includes the independent two-card hand-limit bonus in either catalog.");
            s.Use("damage-two");
            Need(s.V.Hp == 3 && s.V.HandLimit == (held ? 7 : 5) &&
                s.Events<DamageAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 2 }],
                "Real damage changes current HP; only the owned tactic keeps the maximum-HP base plus its independent bonus.");
            s.Use("recover-one");
            Need(s.V.Hp == 4 && s.V.HandLimit == (held ? 7 : 6) &&
                s.Events<RecoveryAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 1, RemainingHp: 4 }],
                "Real recovery changes HP without freezing or replacing the independent hand-limit bonus.");
            s.Use("grow");
            Need(s.V is { Hp: 4, MaxHp: 6 } && s.V.HandLimit == (held ? 8 : 6),
                "Increasing actual maximum HP updates the tactic's live base without inventing extra recovery.");
            s.Use("shrink");
            Need(s.V is { Hp: 4, MaxHp: 4, HandLimit: 6 } &&
                s.Events<MaximumHpChangedEvent>().Where(fact => fact.SkillId == Driver)
                    .Select(fact => (fact.Delta, fact.MaximumHp)).SequenceEqual([(1, 6), (-2, 4)]),
                "Lowering actual maximum HP re-evaluates the same native Set modifier and retains the additive bonus.");
            s.Use("damage-one");
            var capacity = held ? 6 : 5;
            Need(s.V is { Hp: 3, MaxHp: 4, HandCount: 10 } && s.V.HandLimit == capacity &&
                s.Events<DamageAppliedEvent>().Select(fact => fact.Amount).SequenceEqual([2, 1]),
                "A later real hit leaves the owned limit based on the reduced maximum HP rather than current HP.");
            s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
            s.Until(() => s.P is { Kind: DecisionKind.DiscardCards });
            var discard = s.P!;
            Need(discard.RequiredCardCount == 10 - capacity && discard.ValidCardIds.Count == 10,
                "The original discard phase requests the precise overflow from the same live hand-limit query.");
            var paid = discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray();
            s.Accept(new DiscardCardsCommand(0, paid, discard.PromptId, s.G.Revision));
            s.Until(() => s.V.HandCount == capacity && paid.All(id => s.G.CardMovements.Any(move =>
                move.CardId == id && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile)));
            // Discard completes the actual turn in the same command. HandLimit
            // is published for the current actor, so inspect its next real Play.
            s.Play();
            Need(s.V.Hp == 3 && s.V.HandCount == capacity && s.V.HandLimit == capacity && paid.All(id =>
                    s.G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                        move.To == CardLocation.DiscardPile) == 1) && s.V.Hand.All(card => !paid.Contains(card.Id)),
                "The next actual own Play retains the precise hand after each real discard payment, with unchanged HP and capacity. " +
                JsonSerializer.Serialize(new
                {
                    held, capacity, s.V.Hp, s.V.MaxHp, s.V.HandCount, s.V.HandLimit,
                    s.G.State.CurrentSeat, s.G.State.Phase,
                    Paid = paid.Select(id => new
                    {
                        CardId = id,
                        DirectMoves = s.G.CardMovements.Count(move => move.CardId == id &&
                            move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile),
                        StillInHand = s.V.Hand.Any(card => card.Id == id)
                    }).ToArray()
                }));
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => G.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native steady-hand command rejected."); }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 100; step++)
            {
                if (done()) return;
                Need(P?.PlayerSeat != 0 && G.State.Winner == Winner.None, "Unexpected earlier native steady-hand prompt.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native steady-hand flow did not reach its boundary.");
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard } && G.ResolutionStack.Count == 0);
        public void Use(string activation)
        {
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], [], G.Revision, P!.PromptId));
            Play();
        }
        public static Scene Start(bool held)
        {
            var cards = Enumerable.Range(0, 10).Select(_ => new ShanheStartingCard(CardKind.Dodge, Suit.Club, 7)).ToArray();
            var setup = new ShanheBattleSetup(83, 1, "fixture:steady-hand", Owner, 5, 0, 0, 0, 0,
                [], cards, [], [new(Peer) { StartingHand = [] }], [], new string('a', 64))
            { TacticSkillIds = held ? [StandardShanheSteadyHandPackage.SkillId] : [] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
        public static Scene StartIdentity()
        {
            var game = GameEngine.CreateStandard(new()
            {
                Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Identity,
                UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false,
                MaxTurns = 3, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value);
            var scene = new Scene(game); scene.Accept(new StartGameCommand());
            scene.Until(() => scene.P is { Kind: DecisionKind.SelectGeneral });
            scene.Accept(new SelectGeneralCommand(0, IdentityOwner, game.Revision, scene.P!.PromptId));
            scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:steady-hand", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,
                   "modifiers":[{"id":"independent-bonus","query":"handLimit","operation":"add","priority":0,"value":2}],
                   "activations":[
                     {"id":"damage-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"owner","amount":2}]},
                     {"id":"damage-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"owner","amount":1}]},
                     {"id":"recover-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"recover","target":"owner","amount":1}]},
                     {"id":"grow","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"changeMaximumHp","target":"owner","amount":1}]},
                     {"id":"shrink","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"changeMaximumHp","target":"owner","amount":-2}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "真实体力与容量", description = "伤害、回复及体力上限变化。" } } }));
            builder.AddSkill(new(Driver, "真实体力与容量", "伤害、回复及体力上限变化。")
                { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "山河图容量使用者", "supporter", Driver, "shu", 5));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", "standard:none", "wei", 20));
            builder.AddGeneral(new(IdentityOwner, "普通身份场持有者", "supporter", Driver, "shu", 5,
                [StandardShanheSteadyHandPackage.SkillId]));
            foreach (var seat in Enumerable.Range(1, 3))
                builder.AddGeneral(new(Peer + seat, "普通身份场角色", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "零常规摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddDeck(new(Deck + "-identity", "普通身份场实体牌", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "信手拈来实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
            builder.AddMode(new(Identity, "普通身份场", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck + "-identity",
                GeneralCandidateCount: 4, GeneralPoolIds: [IdentityOwner, Peer + "1", Peer + "2", Peer + "3"]));
        }
    }
}
