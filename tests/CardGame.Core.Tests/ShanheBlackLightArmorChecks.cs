using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBlackLightArmorChecks
{
    private const string Skill = StandardShanheBlackLightArmorPackage.SkillId;
    private const string Multi = "fixture:black-light-multi", Deck = "fixture:black-light-deck";

    public static void ActualArmorNullifiesOnlyItsOwnMultiTargetSlashDamageAndBlackTrickEffects()
    {
        var scene = Scene.Start(ignoreArmor: false);
        var armor = scene.Player(1).Equipment.Single(card => card.Kind == CardKind.BlackLightArmor);
        Need(scene.Player(1).Skills!.Any(skill => skill.ContentId == Skill),
            "Installing the real opening armor entity supplies its equipment-owned native skill.");
        var single = scene.Use(CardKind.Slash, 1, [1]);
        Need(scene.Player(1).Hp == 19 && !scene.Nullified(single, 1),
            "A sole target takes an ordinary black Slash normally despite wearing Black Light Armor.");
        var multi = scene.Use(CardKind.FireSlash, 2, [1, 3]);
        Need(scene.Player(1).Hp == 19 && scene.Player(3).Hp == 19 && scene.Nullified(multi, 1) && !scene.Nullified(multi, 3),
            "One real multi-target red Fire Slash is ineffective only for its armored target; its other target still takes real native fire damage.");
        var area = scene.Use(CardKind.ArrowBarrage, 3, []);
        Need(scene.Player(1).Hp == 19 && scene.Player(2).Hp == 19 && scene.Player(3).Hp == 18 && scene.Nullified(area, 1),
            "An actual red damage trick with implicit native AOE targets skips the wearer and independently resolves its other targets.");
        var black = scene.Use(CardKind.IronChain, 4, [1, 3]);
        Need(!scene.Player(1).IsChained && scene.Player(3).IsChained && scene.Nullified(black, 1),
            "A genuine black, non-damage Iron Chain freezes its paid appearance and is ineffective only for the armored target.");
        var red = scene.Use(CardKind.IronChain, 5, [1, 3]);
        Need(scene.Player(1).IsChained && !scene.Player(3).IsChained && !scene.Nullified(red, 1) &&
            scene.Player(1).Equipment.Single(card => card.Kind == CardKind.BlackLightArmor).Id == armor.Id,
            "A red non-damage trick still resolves normally, and protection leaves the same equipped entity in place.");
        foreach (var use in new[] { single, multi, area, black, red }) scene.AssertSinglePayment(use);
        Need(scene.Game.State.Winner == Winner.None && scene.Game.ResolutionStack.Count == 0,
            "Every target suppression returns through the original native action to the live human Play boundary.");
    }

    public static void QinggangMultiTargetSlashBypassesArmorWithoutBypassingBlackTrickProtection()
    {
        var scene = Scene.Start(ignoreArmor: true);
        Need(scene.Player(0).Equipment.Any(card => card.Kind == CardKind.QinggangSword),
            "The attacker's armor bypass comes from a real installed Qinggang Sword entity.");
        var slash = scene.Use(CardKind.FireSlash, 2, [1, 3]);
        Need(scene.Player(1).Hp == 19 && scene.Player(3).Hp == 19 && !scene.Nullified(slash, 1),
            "Qinggang's actual multi-target Slash ignores Black Light Armor before individual response attacks are issued.");
        var chain = scene.Use(CardKind.IronChain, 4, [1, 3]);
        Need(!scene.Player(1).IsChained && scene.Player(3).IsChained && scene.Nullified(chain, 1),
            "The same physical weapon does not grant ordinary tricks armor bypass: the black Iron Chain still has its wearer's effect nullified.");
        scene.AssertSinglePayment(slash); scene.AssertSinglePayment(chain);
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        private PendingDecision? P => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot Player(int seat) => Game.CreateSnapshot(0).Players[seat];
        private T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public bool Nullified(long id, int target) => Events<ProgramCardEffectNullifiedEvent>().Any(fact =>
            fact.SkillId == Skill && fact.CardUseFrameId == id && fact.OwnerSeat == target);
        public static Scene Start(bool ignoreArmor)
        {
            var cards = new ShanheStartingCard[]
            {
                new(CardKind.Slash, Suit.Spade, 1), new(CardKind.FireSlash, Suit.Heart, 2),
                new(CardKind.ArrowBarrage, Suit.Heart, 3), new(CardKind.IronChain, Suit.Club, 4),
                new(CardKind.IronChain, Suit.Diamond, 5)
            };
            var setup = new ShanheBattleSetup(191, 1, "fixture:black-light", "classic:guan-yu", 4,
                0, 0, 0, 0, [Multi], cards,
                ignoreArmor ? [new(CardKind.QinggangSword, Suit.Spade, 6)] : [],
                Enumerable.Range(1, 3).Select(seat => new ShanheCombatantDefinition("classic:ma-chao", MaxHpBonus: 16, Hp: 20)
                {
                    StartingHand = [], StartingEquipment = seat == 1 ? [new(CardKind.BlackLightArmor, Suit.Diamond, 2)] : []
                }).ToArray(), [], "fixture:black-light");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry));
            scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
        public long Use(CardKind kind, int rank, int[] targets)
        {
            var card = Player(0).Hand.Single(item => item.Kind == kind && item.Rank == rank);
            var expectedAction = kind switch
            {
                CardKind.Slash or CardKind.FireSlash => LegalActionKind.Slash,
                CardKind.ArrowBarrage => LegalActionKind.ArrowBarrage,
                CardKind.IronChain => LegalActionKind.IronChain,
                _ => throw new InvalidOperationException("This armor fixture uses only its stated physical native card kinds.")
            };
            var action = Game.GetHumanLegalActions().SingleOrDefault(item => item.Kind == expectedAction && item.CardId == card.Id &&
                (item.PlayedCardKind ?? card.Kind) == kind && item.ConversionSource is null &&
                item.AdditionalConversionSources is not { Count: > 0 } && item.TargetSeats.SequenceEqual(targets));
            Need(action is not null, "Missing native Black Light Armor use: " + JsonSerializer.Serialize(new
                { kind, rank, targets, Actions = Game.GetHumanLegalActions().Where(item => item.CardId == card.Id) }));
            Submit(new PlayCardCommand(0, card.Id, action!.TargetSeats, Game.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
            Play(); return Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == card.Id).ResolutionId;
        }
        public void AssertSinglePayment(long id)
        {
            var use = Events<CardUseDeclaredEvent>().Single(fact => fact.ResolutionId == id);
            Need(Game.CardMovements.Count(move => move.CardId == use.CardId && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
                Game.CardMovements.Count(move => move.CardId == use.CardId && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1 &&
                Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == id) == 1,
                "Equipment target immunity preserves the same real card's single payment and single native finish.");
        }
        private void Play()
        {
            for (var step = 0; step < 160; step++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var pass = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") == "pass");
                    Need(pass is not null, "Unexpected native armor prompt: " + JsonSerializer.Serialize(prompt));
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, pass!.Id, Game.Revision));
                }
                else Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Black Light Armor did not return from its native targets: " + JsonSerializer.Serialize(new
                { Game.State, Pending = P, Stack = Game.ResolutionStack.Select(frame => new { Type = frame.GetType().Name, frame.Id }) }));
        }
        private void Submit(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Native Black Light Armor command rejected.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:black-light", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Multi, revision = 1, modifiers = new[]
                {
                    new { id = "two-real-targets", query = "cardTargetCount", operation = "add", value = 1, priority = 0,
                        cardKinds = new[] { "slash", "fireSlash", "thunderSlash", "coldSlash" } },
                    new { id = "two-native-slashes", query = "slashLimit", operation = "add", value = 2, priority = 0,
                        cardKinds = Array.Empty<string>() }
                } } } });
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                { [Multi] = new { name = "真实多目标杀", description = "增加原生杀目标和次数。" } } }));
            builder.AddSkill(new(Multi, "真实多目标杀", "增加原生杀目标和次数。")
            { Program = catalog.Programs[Multi], ProgramPresentation = catalog.Presentations[Multi] });
            builder.AddDeck(new(Deck, "空起手安全牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 64).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, index % 13 + 1)).ToArray() });
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
