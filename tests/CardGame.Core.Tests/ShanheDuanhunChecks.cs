using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDuanhunChecks
{
    private const string Skill = StandardShanheDuanhunPackage.SkillId;
    private const string Driver = "fixture:duanhun-damage", Deck = "fixture:duanhun-deck";

    public static void RealWeaponBypassesOtherAndSelfArmorWhileLeavingArmorEquipmentEffective()
    {
        var scene = Scene.Start(equipped: true);
        var weapon = scene.Player(0).Equipment.Single(card => card.Kind == CardKind.Duanhun);
        Need(EquipmentCatalog.Get(CardKind.Duanhun) is { Slot: EquipmentSlot.Weapon, WeaponAttackRange: 2, IgnoresArmor: false } &&
            scene.Player(0).Skills!.Any(skill => skill.ContentId == Skill),
            "A real range-two Duanhun installs its equipment-owned policy without receiving Qinggang's armor-equipment bypass flag.");
        var other = scene.Hit(1);
        var self = scene.Hit(0);
        Need(other is { SourceSeat: 0, TargetSeat: 1, Amount: 2, SourceLess: false, ArmorPayment: null } &&
            self is { SourceSeat: 0, TargetSeat: 0, Amount: 2, SourceLess: false, ArmorPayment: null } &&
            scene.Player(1).Hp == 6 && scene.Player(1).Armor == 3 && scene.Player(0).Hp == 6 && scene.Player(0).Armor == 3,
            "Native source-owned damage really removes HP while preserving three armor points on both another character and the weapon's own source.");
        var black = scene.Use(CardKind.Slash, 1, [2]);
        Need(scene.Player(2).Hp == 8 && scene.Player(2).Armor == 3 &&
            scene.Events<ArmorEffectAppliedEvent>().Any(fact => fact.ResolutionId == black && fact.ArmorCard == CardKind.RenwangShield) &&
            !scene.Events<DamageAppliedEvent>().Any(fact => fact.SourceCardUseResolutionId == black),
            "A real Renwang Shield still nullifies the black Slash; ignoring scalar armor does not bypass armor equipment.");
        var redAction = scene.Action(CardKind.Slash, 2, [2]);
        var predicted = scene.Game.PredictPublicActionDamage(0, redAction, 2);
        Need(predicted.HpLossAmount == 1 && predicted.ArmorAbsorbed == 0 && predicted.ArmorAfter == 3,
            "The visible real weapon's public prediction agrees with scalar armor bypass.");
        var red = scene.Use(CardKind.Slash, 2, [2]);
        Need(scene.Player(2).Hp == 7 && scene.Player(2).Armor == 3 &&
            scene.Events<DamageAppliedEvent>().Single(fact => fact.SourceCardUseResolutionId == red) is { Amount: 1, ArmorPayment: null },
            "The same real red Slash gets past Renwang normally, then bypasses only the recipient's scalar armor payment.");
        scene.Use(CardKind.Crossbow, 3, [0]);
        Need(!scene.Player(0).Equipment.Any(card => card.Kind == CardKind.Duanhun) &&
            scene.Game.CardMovements.Count(move => move.CardId == weapon.Id && move.From == CardLocation.Equipment(0) && move.To == CardLocation.DiscardPile) == 1,
            "Replacing the actual weapon moves that exact entity away and revokes its native equipment source.");
        var afterLoss = scene.Hit(1);
        Need(afterLoss.ArmorPayment is { ArmorBefore: 3, ArmorAbsorbed: 2, ArmorAfter: 1, HpLossAmount: 0 } &&
            scene.Player(1).Hp == 6 && scene.Player(1).Armor == 1,
            "Damage after the original weapon leaves the equipment zone resumes the original armor ledger instead of retaining a stale bypass.");
        scene.AssertPaid(black); scene.AssertPaid(red);
    }

    public static void AHandCardOrManualGrantCannotBypassAndRealLightningStillPaysArmor()
    {
        var scene = Scene.Start(equipped: false, manualGrant: true);
        Need(scene.Player(0).Hand.Any(card => card.Kind == CardKind.Duanhun) &&
            scene.Player(0).Skills!.Any(skill => skill.ContentId == Skill) && !scene.Player(0).Equipment.Any(card => card.Kind == CardKind.Duanhun),
            "The fresh negative case really holds the weapon in hand and manually owns the policy, without its physical equipment grant.");
        var unarmed = scene.Hit(1);
        Need(unarmed.ArmorPayment is { ArmorAbsorbed: 2, HpLossAmount: 0 } && scene.Player(1).Hp == 8 && scene.Player(1).Armor == 1,
            "Neither a held weapon nor a manually granted same-name policy bypasses native armor without the actual active weapon instance.");
        scene.Use(CardKind.Duanhun, 5, [0]);
        var armed = scene.Hit(1);
        Need(armed.ArmorPayment is null && scene.Player(1).Hp == 6 && scene.Player(1).Armor == 1 &&
            scene.Player(0).Equipment.Any(card => card.Kind == CardKind.Duanhun),
            "Using the real hand card creates its actual equipment source and enables this battle's next damage immediately, even with a separate manual grant.");
        scene.Use(CardKind.Lightning, 4, [0]);
        scene.EndPlay(); scene.ReachPlay(minTurn: 4);
        var lightning = scene.Events<DamageAppliedEvent>().Single(fact => fact.SourceLess && fact.TargetSeat == 0);
        Need(scene.Events<DamageRequestedEvent>().Any(fact => fact.SourceLess && fact.TargetSeat == 0 &&
                fact.SourceCard == CardKind.Lightning && fact.Amount == 3) &&
            lightning.ArmorPayment is { SourceLess: true, ArmorBefore: 3, ArmorAbsorbed: 3, ArmorAfter: 0, HpLossAmount: 0 } &&
            scene.Player(0).Hp == 8 && scene.Player(0).Armor == 0 && scene.Player(0).Equipment.Any(card => card.Kind == CardKind.Duanhun) &&
            scene.Game.State.Winner == Winner.None && scene.Game.ResolutionStack.Count == 0,
            "A real source-less Lightning judgment while Duanhun remains equipped pays all three armor points and loses no HP; no nominal source seat can borrow the weapon's policy.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        private PendingDecision? P => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot Player(int seat) => Game.CreateSnapshot(0).Players[seat];
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public static Scene Start(bool equipped, bool manualGrant = false)
        {
            var cards = new List<ShanheStartingCard>
            {
                new(CardKind.Slash, Suit.Spade, 1), new(CardKind.Slash, Suit.Heart, 2),
                new(CardKind.Crossbow, Suit.Club, 3), new(CardKind.Lightning, Suit.Spade, 4)
            };
            if (!equipped) cards.Add(new(CardKind.Duanhun, Suit.Spade, 5));
            var setup = new ShanheBattleSetup(238, 1, "fixture:duanhun", "classic:guan-yu", 8,
                4, 0, 0, 0, manualGrant ? [Driver, Skill] : [Driver], cards,
                equipped ? [new(CardKind.Duanhun, Suit.Spade, 5)] : [],
                Enumerable.Range(1, 2).Select(seat => new ShanheCombatantDefinition("classic:guan-yu", MaxHpBonus: 4, Hp: 8)
                {
                    StartingHand = [], InitialArmor = 3,
                    StartingEquipment = seat == 2 ? [new(CardKind.RenwangShield, Suit.Club, 6)] : []
                }).ToArray(), [], "fixture:duanhun");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 6
            }, registry));
            scene.Submit(new StartGameCommand()); scene.ReachPlay(); return scene;
        }
        public DamageAppliedEvent Hit(int target)
        {
            var before = Events<DamageAppliedEvent>().Length;
            Submit(new UseProgramSkillCommand(0, Driver, "hit", [], [target], Game.Revision, P!.PromptId));
            ReachPlay();
            return Events<DamageAppliedEvent>().Skip(before).Single();
        }
        public LegalAction Action(CardKind kind, int rank, int[] requested)
        {
            var card = Player(0).Hand.Single(item => item.Kind == kind && item.Rank == rank);
            var expected = kind == CardKind.Slash ? LegalActionKind.Slash : kind == CardKind.Lightning ? LegalActionKind.Lightning : LegalActionKind.Equip;
            var action = Game.GetHumanLegalActions().SingleOrDefault(item => item.Kind == expected && item.CardId == card.Id &&
                (item.PlayedCardKind ?? card.Kind) == kind && item.ConversionSource is null && item.AdditionalConversionSources is not { Count: > 0 } &&
                (item.TargetSeats.SequenceEqual(requested) || expected is LegalActionKind.Equip or LegalActionKind.Lightning &&
                    requested.SequenceEqual([0]) && item.TargetSeats.Count == 0));
            Need(action is not null, "Missing actual native Duanhun fixture action: " + JsonSerializer.Serialize(new
                { kind, rank, requested, Actions = Game.GetHumanLegalActions().Where(item => item.CardId == card.Id) }));
            return action!;
        }
        public long Use(CardKind kind, int rank, int[] targets)
        {
            var action = Action(kind, rank, targets);
            Submit(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, Game.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
            ReachPlay(); return Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == action.CardId).ResolutionId;
        }
        public void AssertPaid(long id)
        {
            var use = Events<CardUseDeclaredEvent>().Single(fact => fact.ResolutionId == id);
            Need(Game.CardMovements.Count(move => move.CardId == use.CardId && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
                Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == id) == 1,
                "Scalar armor bypass preserves each physical Slash's one native payment and finish.");
        }
        public void EndPlay() => Submit(new EndPlayPhaseCommand(0, Game.Revision, P!.PromptId));
        public void ReachPlay(int minTurn = 1)
        {
            for (var step = 0; step < 160; step++)
            {
                if (Game.State.TurnNumber >= minTurn && P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var pass = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") == "pass");
                    Need(pass is not null, "Unexpected native Duanhun boundary: " + JsonSerializer.Serialize(prompt));
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, pass!.Id, Game.Revision));
                }
                else Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Duanhun did not return to its bounded native Play: " + JsonSerializer.Serialize(new
                { Game.State, Pending = P, Stack = Game.ResolutionStack.Select(frame => new { Type = frame.GetType().Name, frame.Id }) }));
        }
        private void Submit(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Duanhun fixture native command rejected.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:duanhun", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Driver, revision = 1,
                    modifiers = new[] { new { id = "native-second-slash", query = "slashLimit", operation = "add", value = 1, priority = 0 } },
                    triggers = new[] { new { id = "opening-real-armor", window = "gameStarting", subject = "owner", optional = false,
                        effects = new[] { new { op = "grantArmor", target = "owner", amount = 3 } } } },
                    activations = new[] { new { id = "hit", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1,
                        targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "damage", target = "selectedTarget", amount = 2 } } } }
                } } });
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                { [Driver] = new { name = "原生伤害与护甲", description = "发起真实伤害，保留每一段原生护甲支付。" } } }));
            builder.AddSkill(new(Driver, "原生伤害与护甲", "发起真实伤害，保留每一段原生护甲支付。")
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddDeck(new(Deck, "固定闪电判定实体", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
