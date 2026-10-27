using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ResponseConversionSelectionChecks
{
    private const string Longhun = "wushuang:skill-3023";
    private const string Longdan = "classic:longdan";
    private static readonly Lazy<ContentSkillDefinition> LongdanDefinition = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardActiveSkillExpansionPackage(), new StandardRescueSkillExpansionPackage(),
        new StandardClassicGeneralPackage()).GetSkill(Longdan));
    private static readonly Lazy<ContentSkillDefinition> Definition = new(() =>
    {
        var assembly = typeof(StandardContentPackage).Assembly;
        string Read(string name) { using var stream = assembly.GetManifestResourceStream(
            $"CardGame.Content.Standard.SkillPrograms.wushuang-skills-b-audit.{name}.json")!;
            using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
        var catalog = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
        return new(Longhun, catalog.Presentations[Longhun].Name, catalog.Presentations[Longhun].Description)
            { Program = catalog.Programs[Longhun], ProgramPresentation = catalog.Presentations[Longhun] };
    });

    public static void ResponsesRequireSkillAndExactMaterials(string output)
    {
        foreach (var (suit, kind) in new[] { (Suit.Spade, DecisionKind.Nullification), (Suit.Club, DecisionKind.RespondDodge),
                     (Suit.Diamond, DecisionKind.RespondSlash), (Suit.Heart, DecisionKind.RescueDying) })
        {
            var (setup, registry) = Create(suit);
            Equip(setup);
            if (kind == DecisionKind.RescueDying)
            {
                while (setup.CreateSnapshot(0).Players[0].Hp > 0)
                {
                    Reach(setup, DecisionKind.PlayCard);
                    Accept(setup, new UseProgramSkillCommand(0, Fixture.Driver, "hurt", [], [], setup.Revision, setup.PendingDecision!.PromptId));
                }
            }
            else Accept(setup, new EndPlayPhaseCommand(0, setup.Revision, setup.PendingDecision!.PromptId));
            Reach(setup, kind);
            var checkpoint = setup.CreateCheckpoint();
            foreach (var count in new[] { 1, 2 })
            {
                using var model = Restore(checkpoint, registry);
                var game = Program.Engine(model);
                var prompt = game.CreateSnapshot(0).PendingDecision!;
                var revision = game.Revision;
                var commands = game.AcceptedCommands.Count;
                var physical = model.Hand.ToDictionary(card => card.Id, card => (card.Kind, card.Name, card.SuitGlyph, card.Rank));
                var window = new MainWindow(model);
                try
                {
                    var root = (FrameworkElement)window.Content;
                    var skill = model.HumanSkillCards.Single(item => item.ContentId == Longhun);
                    Program.Assert(skill.IsAvailable && skill.TypeText.Contains("主动") && !model.HasSelection &&
                        model.HandResponseExtraChoices.Count == 0 && model.Hand.All(card => !card.IsPlayable) &&
                        !model.HumanEquipmentSlots.Any(slot => slot.IsSelectable) && !model.CanConfirmSelected,
                        $"{kind}: an available conversion must wait on its skill rail without showing or preselecting costs.");
                    Click(root, model.ActivateHumanSkillCommand, skill);
                    Program.Assert(model.HasSelection && model.IsCardPaymentSelectionPending &&
                        model.HandResponseExtraChoices.Count == 0 && model.Hand.Any(card => card.IsPlayable) && !model.CanConfirmSelected,
                        "Longhun must enable its published materials immediately, without choosing a card name or count.");
                    var equipment = model.HumanEquipmentSlots.Single(slot => slot.Card is not null);
                    Program.Assert(equipment.IsSelectable, "Published equipment must be selectable after choosing the skill.");
                    Click(root, model.SelectEquipmentMaterialCommand, equipment);
                    var ids = new List<int> { equipment.Card!.Id };
                    if (count == 2)
                    {
                        Program.Assert(model.CanConfirmSelected, "One material must already offer the single-card conversion while a second remains optional.");
                        var card = model.Hand.First(item => item.IsPlayable);
                        ids.Add(card.Id);
                        Click(root, model.SelectCardCommand, card);
                    }
                    var choice = prompt.Choices.Single(item => item.Cards.Count == count && item.Cards.ToHashSet().SetEquals(ids) &&
                        item.Parameters.GetValueOrDefault("conversion-skill-id") == Longhun);
                    Program.Assert(model.CanConfirmSelected && game.Revision == revision && game.AcceptedCommands.Count == commands &&
                        model.Hand.All(card => physical[card.Id] == (card.Kind, card.Name, card.SuitGlyph, card.Rank)) &&
                        model.HumanEquipmentSlots.Single(slot => slot.Card is not null).IsSelected,
                        "Drafts must retain physical card faces and pay nothing before confirmation.");
                    model.SelectHandResponseChoiceCommand.Execute(choice with { });
                    Program.Assert(game.Revision == revision && model.CanConfirmSelected,
                        "Copied raw responses cannot submit or replace a draft.");
                    model.ClearSelectionCommand.Execute(null);
                    Program.Assert(!model.HasSelection && !model.CanConfirmSelected && model.HandResponseExtraChoices.Count == 0 &&
                        !model.HumanEquipmentSlots.Any(slot => slot.IsSelectable || slot.IsSelected) && game.Revision == revision,
                        "Esc/cancel must exit the conversion while leaving the original response unanswered.");
                    LegacySaveFixtures.Load(model);
                    game = Program.Engine(model);
                    Program.Assert(!model.HasSaveError && !model.HasSelection && model.HandResponseExtraChoices.Count == 0 &&
                        !model.CanConfirmSelected && game.Revision == revision,
                        "Reloading the pending response must require explicit skill selection again.");
                    Click(root, model.ActivateHumanSkillCommand, model.HumanSkillCards.Single(item => item.ContentId == Longhun));
                    Click(root, model.SelectEquipmentMaterialCommand, model.HumanEquipmentSlots.Single(slot => slot.Card is not null));
                    if (count == 2) Click(root, model.SelectCardCommand, model.Hand.Single(card => card.Id == ids[1]));
                    var expected = GameReplay.Restore(checkpoint, registry);
                    Accept(expected, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, expected.Revision));
                    model.ConfirmSelectedCommand.Execute(null);
                    Program.Assert(game.AcceptedCommands.Count == commands + 1 && game.AcceptedCommands.Last() is AnswerPromptCommand answer &&
                        answer.Choice == choice.Id && game.CardMovements.SequenceEqual(expected.CardMovements) &&
                        SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(expected.CreateSnapshot(0)),
                        "Confirm must submit exactly the published conversion and pay its native hand/equipment cost once.");
                    var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
                    Program.Assert(SnapshotJson.Serialize(cold.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                        "The accepted conversion and any native follow-up must survive cold replay.");
                }
                finally { window.Content = null; window.Close(); }
            }
        }
        var (rescue, rescueRegistry) = StableHandResponseFixture.Create(DecisionKind.RescueDying, output, jijiu: true);
        using (rescue)
        {
            var game = Program.Engine(rescue);
            var revision = game.Revision;
            var card = rescue.Hand.First(item => item.Kind == CardKind.Dodge &&
                game.PendingDecision!.Choices.Any(choice => choice.Cards.SequenceEqual([item.Id]) &&
                    choice.Parameters.GetValueOrDefault("conversion-skill-id") == "standard:jijiu"));
            Program.Assert(!card.IsPlayable && rescue.Hand.Any(item => item.Kind == CardKind.Peach && item.IsPlayable),
                "Jijiu must preserve native Peach while its red Dodge waits for skill selection.");
            rescue.ActivateHumanSkillCommand.Execute(rescue.HumanSkillCards.Single(item => item.ContentId == "standard:jijiu"));
            Program.Assert(rescue.HandResponseExtraChoices.Count == 0 && rescue.IsCardPaymentSelectionPending &&
                rescue.Hand.Single(item => item.Id == card.Id).IsPlayable && game.Revision == revision,
                "A single-output response conversion must skip an unnecessary mode chooser and remain unpaid.");
            rescue.SelectCardCommand.Execute(rescue.Hand.Single(item => item.Id == card.Id));
            Program.Assert(rescue.CanConfirmSelected && game.Revision == revision, "Other conversions must share the confirmation boundary.");
            rescue.ConfirmSelectedCommand.Execute(null);
            Program.Assert(game.Revision == revision + 1 && game.AcceptedCommands.Last() is AnswerPromptCommand,
                "Jijiu must commit only after explicit confirmation.");
            StableHandResponseFixture.RecordAndReplay(game, rescueRegistry, output, "skill-first-jijiu.after");
        }
    }

    public static void PlayGroupsSingleAndMultipleCardConversions(string output)
    {
        var (setup, registry) = Create(null);
        Equip(setup);
        foreach (var kind in new[] { CardKind.Peach, CardKind.FireSlash })
        foreach (var count in new[] { 1, 2 })
        {
            using var model = Restore(setup.CreateCheckpoint(), registry);
            var game = Program.Engine(model);
            var revision = game.Revision;
            var window = new MainWindow(model);
            try
            {
                var root = (FrameworkElement)window.Content;
                Click(root, model.ActivateHumanSkillCommand, model.HumanSkillCards.Single(skill => skill.ContentId == Longhun));
                Program.Assert(model.EquipmentPlayChoices.Count == 0 && !model.HasCardNameSelection && model.Hand.Any(card => card.IsPlayable) &&
                    !model.SupplementalActiveSkillActions.Any(action => action.ProgramSkillId == Longhun),
                    "The rail must enable conversion materials without a name/count chooser: " +
                    string.Join(" | ", model.EquipmentPlayChoices.Select(choice => choice.Description)) + "; hand=" +
                    string.Join(",", model.Hand.Select(card => card.SuitGlyph + card.Name)) + "; " + model.ActionHint);
                var suit = kind == CardKind.Peach ? "♥" : "♦";
                var equipment = model.HumanEquipmentSlots.FirstOrDefault(slot => slot.Card is { } card &&
                    card.Suit == (kind == CardKind.Peach ? Suit.Heart : Suit.Diamond));
                if (equipment is not null) Click(root, model.SelectActiveEquipmentCommand, equipment);
                var cards = model.Hand.Where(card => card.IsPlayable && card.SuitGlyph == suit).Take(count - (equipment is null ? 0 : 1)).ToArray();
                Program.Assert(cards.Length == count - (equipment is null ? 0 : 1), "The scene must have enough matching hand/equipment materials.");
                foreach (var card in cards) Click(root, model.SelectCardCommand, card);
                Program.Assert(model.Hand.Where(card => card.SuitGlyph != suit).All(card => !card.IsPlayable) &&
                    model.PlayButtonText.Contains(CardCatalog.Get(kind).DisplayName) &&
                    game.Revision == revision && !model.HasCardNameSelection,
                    "The selected material suit must determine the output immediately and exclude mixed suits without paying early.");
                var selectedIds = cards.Select(card => card.Id).Concat(equipment is null ? [] : new[] { equipment.Card!.Id }).ToHashSet();
                var incompatible = model.Hand.First(card => card.SuitGlyph != suit);
                model.SelectCardCommand.Execute(incompatible);
                Program.Assert(model.Hand.Where(card => card.IsSelected).Select(card => card.Id)
                    .Concat(model.HumanEquipmentSlots.Where(slot => slot.IsSelected).Select(slot => slot.Card!.Id)).ToHashSet().SetEquals(selectedIds),
                    "Clicking an incompatible suit must leave the existing one/two-card draft intact.");
                if (count == 2)
                {
                    Program.Assert(model.Hand.All(card => !card.IsPlayable || card.IsSelected), "Two materials must disable a third card.");
                    var reverse = model.Hand.Single(card => card.Id == cards[^1].Id);
                    Click(root, model.SelectCardCommand, reverse);
                    Program.Assert(model.Hand.Count(card => card.IsSelected) + model.HumanEquipmentSlots.Count(slot => slot.IsSelected) == 1,
                        "Removing the second material must return to the single-card conversion without restarting the skill.");
                    Click(root, model.SelectCardCommand, model.Hand.Single(card => card.Id == reverse.Id));
                }
                if (kind == CardKind.FireSlash && !model.Seats.Any(seat => seat.IsSelectedTarget))
                    model.SelectTargetCommand.Execute(model.Seats.Single(seat => seat.Seat == 1));
                Program.Assert(model.CanConfirmSelected && game.Revision == revision, "Selecting conversion materials/targets cannot pay early.");
                Program.Render(root, 1120, 740, Path.Combine(output, $"longhun-material-{kind}-{count}.png"));
                var moves = game.CardMovements.Count;
                model.ConfirmSelectedCommand.Execute(null);
                Program.Assert(game.Revision == revision + 1 && (count == 1
                    ? game.AcceptedCommands.Last() is PlayCardCommand played && played.ConversionSource?.SkillId == Longhun && played.PlayedCardKind == kind
                    : game.AcceptedCommands.Last() is UseProgramSkillCommand used && used.SkillId == Longhun && used.CardIds.Count == 2),
                    "Material selection must retain the native single-card or multi-card command identity.");
                Program.Assert(selectedIds.All(id => game.CardMovements.Skip(moves).Count(move => move.CardId == id && move.To == CardLocation.DiscardPile) == 1),
                    "Confirmation must pay each selected hand/equipment entity exactly once.");
            }
            finally { window.Content = null; window.Close(); }
        }
    }

    public static void ZhangbaUsesWeaponSlotAndExactPair(string output)
    {
        foreach (var incoming in new[] { CardKind.Duel, CardKind.BarbarianAssault })
        {
            var (setup, registry) = Create(Suit.Diamond, incoming);
            var spear = setup.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Equip &&
                setup.CreateSnapshot(0).Players[0].Hand.Single(card => card.Id == action.CardId).Kind == CardKind.ZhangbaSerpentSpear);
            Accept(setup, new PlayCardCommand(0, spear.CardId!.Value, [], setup.Revision, setup.PendingDecision!.PromptId));
            Reach(setup, DecisionKind.PlayCard);
            Accept(setup, new EndPlayPhaseCommand(0, setup.Revision, setup.PendingDecision!.PromptId));
            Reach(setup, DecisionKind.RespondSlash);
            Program.Assert(setup.PendingDecision!.IncomingCard == incoming, "The regression must reach its real native incoming card.");
            var checkpoint = setup.CreateCheckpoint();
            foreach (var response in new[] { "native", "longdan", "zhangba", "decline" })
            {
                using var model = Restore(checkpoint, registry);
                var game = Program.Engine(model);
                var prompt = model.ChoiceSnapshot.PendingDecision!;
                var revision = game.Revision;
                var commands = game.AcceptedCommands.Count;
                var movements = game.CardMovements.Count;
                var window = new MainWindow(model);
                try
                {
                    var root = (FrameworkElement)window.Content;
                    EquipmentSlotViewModel Weapon() => model.HumanEquipmentSlots.Single(slot => slot.Card?.Kind == CardKind.ZhangbaSerpentSpear);
                    HumanSkillViewModel Skill() => model.HumanSkillCards.Single(skill => skill.ContentId == Longdan);
                    void WeaponClick() => Click(root, model.SelectEquipmentMaterialCommand, Weapon());
                    Program.Assert(model.HandResponseExtraChoices.Count == 0 && !model.HasCenterChoices &&
                        Weapon().IsSelectable && !Weapon().IsSelected && Skill().IsAvailable && !model.CanConfirmSelected &&
                        model.Hand.Any(card => card.Kind == CardKind.Slash && card.IsPlayable) &&
                        model.Hand.Where(card => card.Kind == CardKind.Dodge).All(card => !card.IsPlayable),
                        $"{incoming}/{response}: native Slash, Longdan and weapon must be separate entries; extras={model.HandResponseExtraChoices.Count}, " +
                        $"center={model.HasCenterChoices}, weapon={Weapon().IsSelectable}/{Weapon().IsSelected}, longdan={Skill().IsAvailable}, confirm={model.CanConfirmSelected}, " +
                        "hand=" + string.Join(",", model.Hand.Select(card => $"{card.Id}:{card.Kind}:{card.IsPlayable}")) +
                        "; choices=" + string.Join("|", prompt.Choices.Select(item => item.Description)));
                    PromptChoice choice;
                    if (response == "native")
                    {
                        var card = model.Hand.First(card => card.Kind == CardKind.Slash && card.IsPlayable);
                        Click(root, model.SelectCardCommand, card);
                        choice = prompt.Choices.Single(item => item.Cards.SequenceEqual([card.Id]) &&
                            item.Parameters.GetValueOrDefault("conversion-skill-id") is null);
                    }
                    else
                    {
                        WeaponClick();
                        var material = model.Hand.First(card => card.Kind == CardKind.Dodge && card.IsPlayable);
                        Click(root, model.SelectCardCommand, material);
                        Program.Assert(Weapon().IsSelected && !model.CanConfirmSelected && model.HandResponseExtraChoices.Count == 0,
                            "One Zhangba material must remain an unpaid, incomplete draft.");
                        Click(root, model.ActivateHumanSkillCommand, Skill());
                        Program.Assert(!Weapon().IsSelected && !model.Hand.Any(card => card.IsSelected) &&
                            model.Hand.Where(card => card.Kind == CardKind.Slash).All(card => !card.IsPlayable) &&
                            model.Hand.Any(card => card.Kind == CardKind.Dodge && card.IsPlayable),
                            "Switching to Longdan must discard the Zhangba draft and offer only its legal Dodge material.");
                        if (response == "longdan")
                        {
                            var card = model.Hand.First(card => card.Kind == CardKind.Dodge && card.IsPlayable);
                            Click(root, model.SelectCardCommand, card);
                            choice = prompt.Choices.Single(item => item.Cards.SequenceEqual([card.Id]) &&
                                item.Parameters.GetValueOrDefault("conversion-skill-id") == Longdan);
                        }
                        else
                        {
                            WeaponClick();
                            WeaponClick();
                            Program.Assert(!model.HasSelection && !Weapon().IsSelected && !model.IsCardPaymentSelectionPending,
                                "Re-clicking the selected weapon must return to ordinary Slash selection.");
                            WeaponClick();
                            LegacySaveFixtures.Load(model);
                            game = Program.Engine(model);
                            Program.Assert(!model.HasSelection && !Weapon().IsSelected && model.HandResponseExtraChoices.Count == 0,
                                "Reloading a pending response must not restore an uncommitted equipment draft.");
                            WeaponClick();
                            var pair = model.ChoiceSnapshot.PendingDecision!.Choices.First(item => item.Parameters.GetValueOrDefault("response") == "zhangba-slash");
                            foreach (var id in pair.Cards) Click(root, model.SelectCardCommand, model.Hand.Single(card => card.Id == id));
                            choice = response == "decline" ? model.ChoiceSnapshot.PendingDecision!.Choices.Single(item =>
                                item.Parameters.GetValueOrDefault("response") == "take-damage") : pair;
                            Program.Assert(model.Hand.Count(card => card.IsSelected) == 2 && model.CanConfirmSelected &&
                                model.PlayButtonText == "打出杀" && model.Hand.Where(card => !pair.Cards.Contains(card.Id)).All(card => !card.IsPlayable) &&
                                Weapon().IsSelected && model.HandResponseExtraChoices.Count == 0,
                                "Exactly two published materials must enable the bottom Slash button without exposing pair combinations.");
                            if (incoming == CardKind.Duel && response == "zhangba")
                                Program.Render(root, 1120, 740, Path.Combine(output, "zhangba-response-selected.png"));
                        }
                    }
                    Program.Assert(game.Revision == revision && game.AcceptedCommands.Count == commands && game.CardMovements.Count == movements,
                        "Choosing or switching response entries must not consume cards or answer the prompt.");
                    var expected = GameReplay.Restore(checkpoint, registry);
                    Accept(expected, new AnswerPromptCommand(0, expected.PendingDecision!.PromptId, choice.Id, expected.Revision));
                    if (response == "decline") model.CancelActionCommand.Execute(null);
                    else model.ConfirmSelectedCommand.Execute(null);
                    Program.Assert(game.AcceptedCommands.Count == commands + 1 && game.AcceptedCommands.Last() is AnswerPromptCommand answer &&
                        answer.Choice == choice.Id && game.CardMovements.SequenceEqual(expected.CardMovements) &&
                        SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(expected.CreateSnapshot(0)) && !model.HasSelection,
                        "Each response must submit the exact native choice once and clear its draft for the next response.");
                    var after = game.Revision;
                    model.ConfirmSelectedCommand.Execute(null);
                    Program.Assert(game.Revision == after, "Repeated confirmation must not consume another pair.");
                    if (response == "zhangba")
                    {
                        var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
                        Program.Assert(SnapshotJson.Serialize(cold.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                            "The selected Zhangba pair must retain its native replay contract.");
                    }
                }
                finally { window.Content = null; window.Close(); }
            }
        }
    }

    private static (GameEngine, ContentRegistry) Create(Suit? suit, CardKind? incoming = null)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(suit, incoming));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0,
            HumanRole = Role.Lord, ModeId = Fixture.Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4 }, registry);
        Accept(game, new StartGameCommand());
        Accept(game, new SelectGeneralCommand(0, Fixture.Owner, game.Revision, game.PendingDecision!.PromptId));
        Reach(game, DecisionKind.PlayCard);
        return (game, registry);
    }

    private static void Equip(GameEngine game)
    {
        var action = game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.Equip);
        Accept(game, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, game.Revision, game.PendingDecision!.PromptId));
        Reach(game, DecisionKind.PlayCard);
    }

    private static MainViewModel Restore(GameCheckpoint checkpoint, ContentRegistry registry)
    {
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, checkpoint));
        var model = new MainViewModel(false, 31, true, store, contentRegistry: registry)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        LegacySaveFixtures.Load(model);
        Program.Assert(!model.HasSaveError, model.SaveStatus);
        return model;
    }

    private sealed class Fixture(Suit? suit, CardKind? incoming = null) : IGameContentPackage
    {
        internal const string Mode = "identity:classic-response-conversion-ui", Owner = "fixture:response-conversion-owner", Driver = "fixture:response-conversion-driver";
        public PackageManifest Manifest { get; } = new("fixture-response-conversion-ui", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddSkill(incoming is null ? Definition.Value : LongdanDefinition.Value);
            if (incoming is not null) builder.AddCard(new("fixture:response-spear", "丈八蛇矛", "装备牌", "两张手牌当杀", CardKind.ZhangbaSerpentSpear,
                AiTags: new Dictionary<string, string> { ["slot"] = "weapon", ["conversion"] = "two-hand-cards-as-slash" }));
            var rules = """
                {"schemaVersion":$SCHEMA$,"skills":[
                {"id":"fixture:response-conversion-driver","revision":1,"activations":[
                {"id":"hurt","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]},
                {"id":"fixture:response-conversion-attack","revision":1,"triggers":[{"id":"attack","window":"$WINDOW$","subject":"owner","optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"anyWounded"},$USE$]}]}]}
                """.Replace("$SCHEMA$", SkillProgramCatalog.RulesSchemaVersion.ToString())
                .Replace("$WINDOW$", suit is Suit.Diamond or Suit.Spade ? "turnStartBeforeNormalFlow" : "afterNormalDraw")
                .Replace("$USE$", incoming == CardKind.BarbarianAssault
                    ? """{"op":"useVirtualOrdinaryTrick","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"owner"},"outputKind":"barbarianAssault"}"""
                    : suit is Suit.Diamond or Suit.Spade
                    ? """{"op":"useVirtualOrdinaryTrick","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"outputKind":"duel"}"""
                    : """{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}""");
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "驱动", description = "原生发动" },
                    ["fixture:response-conversion-attack"] = new { name = "来袭", description = "原生来袭" } } }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, "规则夹具")
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "转化选牌", "supporter", incoming is null ? Longhun : Longdan, "qun", incoming is null ? 4 : 12,
                AdditionalSkillIds: [Driver]) { InitialHp = incoming is null ? 2 : 10 });
            var peers = Enumerable.Range(1, 3).Select(seat => $"fixture:response-conversion-peer-{seat}").ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "来袭者", "supporter", "fixture:response-conversion-attack", "wei", 6));
            builder.AddDeck(new("fixture:response-conversion-deck", "材料", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 80)
                .Select(index => new ContentDeckPhysicalCard(incoming is null ? "standard:crossbow" : (index % 3) switch
                    { 0 => "fixture:response-spear", 1 => "standard:slash", _ => "standard:dodge" },
                    suit ?? (index % 2 == 0 ? Suit.Heart : Suit.Diamond), index % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "龙魂选牌", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                "fixture:response-conversion-deck", 4, [Owner, .. peers]));
        }
    }

    private static void Reach(GameEngine game, DecisionKind kind)
    {
        for (var step = 0; step < 50; step++)
        {
            if (game.State.Status == EngineStatus.Completed) throw new InvalidOperationException($"Completed before {kind}: " +
                string.Join(" | ", game.Events.TakeLast(8).Select(item => item.Payload.GetType().Name + ":" + JsonSerializer.Serialize(item.Payload, item.Payload.GetType()))));
            if (game.PendingDecision is { PlayerSeat: 0 } prompt && prompt.Kind == kind) return;
            if (game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.Nullification } skip)
                Accept(game, new AnswerPromptCommand(0, skip.PromptId, skip.Choices.Single(choice => choice.Parameters.GetValueOrDefault("response") == "pass").Id, game.Revision));
            else if (game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards } discard)
                Accept(game, new DiscardCardsCommand(0, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, game.Revision));
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException($"Did not reach {kind}: {game.PendingDecision?.Kind}.");
    }
    private static void Accept(GameEngine game, GameCommand command) { var result = game.Submit(command); Program.Assert(result.Accepted, result.Error?.Message ?? "Native command rejected."); }
    private static void Click(FrameworkElement root, ICommand command, object parameter)
    {
        root.Measure(new Size(1120, 740)); root.Arrange(new Rect(0, 0, 1120, 740)); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
        var button = Program.Find<Button>(root).Single(item => ReferenceEquals(item.Command, command) && ReferenceEquals(item.CommandParameter, parameter));
        Program.Assert(button.IsEnabled && button.Visibility == Visibility.Visible, "The actual conversion control must be enabled and visible.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    }
}
