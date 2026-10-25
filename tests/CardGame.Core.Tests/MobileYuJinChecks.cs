using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileYuJinChecks
{
    private const string Hero = "mobile:yu-jin", Skill = "mobile:jieyue", Owner = "fixture:mobile-yu-jin-owner",
        Driver = "fixture:mobile-yu-jin-driver", Quiet = "fixture:mobile-yu-jin-quiet", Watch = "fixture:mobile-yu-jin-watch",
        Opening = "fixture:mobile-yu-jin-opening", Mode = "identity:classic-mobile-jieyue";
    private const string StoreReason = "skill.mobile-yu-jin.jieyue.store", RetrieveReason = "skill.mobile-yu-jin.jieyue.retrieve";

    public static void OwnedPileEnablesRealDodgeNullificationAndPreparationRetrievalCold()
    {
        var s = Scene.Start(); s.Play(0);
        var stored = s.Card(0, CardKind.BaguaFormation);
        var trick = s.Card(0, CardKind.Dismantlement);
        s.Gift(stored, 1); s.Gift(trick, 1);
        var red = s.V(0).Hand.First(c => c.Kind == CardKind.Slash && c.Suit == Suit.Heart).Id;
        var black = s.V(0).Hand.First(c => c.Kind == CardKind.Slash && c.Suit == Suit.Spade).Id;
        var cost = s.V(0).Hand.First(c => c.Kind == CardKind.Dismantlement).Id;
        s.End(0); s.BeginJieyue(1, cost); s.Choice("store"); s.SelectStore(stored);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var original = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        s.Need(original.MobileYuJinTransfer is { Cards: [var paid] } && paid == stored &&
            s.Location(stored).Zone == CardZoneKind.PublicPersistentPile && s.Location(stored).OwnerSeat == 0 && s.P!.PlayerSeat == 1,
            "The provider's actual chosen hand entity is in Yu Jin's own source-specific public pile while the original paid frame awaits its movement child.");
        s.Cold(); s.Continue(); s.Play(1);
        var hp = s.V(0).Hp;
        s.Drive(1, "slash", [], [0]); s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 });
        s.Need(s.P!.Choices.Any(c => Conversion(c) && c.Cards.SequenceEqual([red])), "A real red hand card is offered as Dodge only after the actual pile deposit.");
        s.Answer(c => Conversion(c) && c.Cards.SequenceEqual([red])); s.Play(1);
        s.Need(s.V(0).Hp == hp && s.Location(red) == CardLocation.DiscardPile && s.Location(stored).Zone == CardZoneKind.PublicPersistentPile,
            "Dodge pays its red hand entity through the native response pipeline and never consumes the enabling Jieyue entity.");
        s.Native(1, trick, 0); s.Reach(() => s.P is { Kind: DecisionKind.Nullification, PlayerSeat: 0 }); s.Cold();
        s.Answer(c => Conversion(c) && c.Cards.SequenceEqual([black])); s.Play(1);
        s.Need(s.Location(black) == CardLocation.DiscardPile && s.G.Events.Any(e => e.Payload is CardActionAcceptedEvent accepted &&
            accepted.Action.ActorSeat == 0 && accepted.Action.EffectiveKind == CardKind.Nullification && accepted.Action.ConversionChain.Any(source => source.SkillId == Skill)),
            "The black hand material is genuinely used as Nullification, retaining native use provenance and the unchanged enabling pile.");
        s.End(1); s.Play(0);
        s.Need(s.Location(stored) == CardLocation.Hand(0) && s.G.CardMovements.Count(m => m.CardId == stored && m.Reason.Value == StoreReason) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == stored && m.Reason.Value == RetrieveReason) == 1 && s.G.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile) == 1,
            "The next real preparation retrieves the original entity exactly once without repeating the earlier cost or storage across cold replay.");
        s.End(0); s.Offer(); s.Skip(); s.Play(1);
        s.Drive(1, "slash", [], [0]); s.Play(1);
        s.Need(s.V(0).Hp == hp - 1 && s.G.Events.Count(e => e.Payload is CardActionAcceptedEvent a && a.Action.ActorSeat == 0 &&
            a.Action.EffectiveKind == CardKind.Dodge && a.Action.ConversionChain.Any(source => source.SkillId == Skill)) == 1,
            "Retrieving the last Jieyue card disables its conversion immediately; the following real Slash cannot obtain another converted Dodge from red hand cards.");
    }

    public static void ForeignEquipmentRecoveryAndOpaqueDiscardBranchKeepPaidStateCold()
    {
        var s = Scene.Start(); s.Play(0);
        var lion = s.Card(0, CardKind.SilverLion); s.Gift(lion, 1); s.End(0); s.Offer(); s.Skip(); s.Play(1);
        s.Native(1, lion); s.Play(1); s.Drive(1, "wound", [], []); s.Play(1);
        var hp = s.V(1).Hp; s.End(1); s.Play(0); var cost = s.Card(0, CardKind.Dismantlement);
        s.End(0); s.BeginJieyue(1, cost); s.Choice("store"); s.SelectStore(lion); s.Play(1);
        s.Need(s.V(1).Hp == hp + 1 && s.Location(lion).Zone == CardZoneKind.PublicPersistentPile && s.PaidColdCount > 0 &&
            s.G.CardMovements.Count(m => m.CardId == lion && m.From == CardLocation.Equipment(1) && m.Reason.Value == StoreReason) == 1,
            "A provider may deposit its actual equipped Silver Lion; its native removal recovery and paid movement children complete once on the original frame, including cold recovery.");

        var d = Scene.Start(); d.Play(0); var gift = d.Card(0, CardKind.Slash); d.Gift(gift, 1);
        var discardedCost = d.Card(0, CardKind.Dismantlement); d.End(0); d.BeginJieyue(1, discardedCost); d.Choice("discard");
        d.Reach(() => d.P?.PlayerSeat == 0 && d.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
        d.Need(d.P!.Choices.All(c => c.Cards.Count == 0) && d.P.Choices.All(c => !c.Description.Contains("杀", StringComparison.Ordinal)),
            "The target's hand remains opaque to Yu Jin; the owner chooses the ordinary hand slot without seeing its face or entity id.");
        d.Cold(); d.Answer(_ => true); d.Play(1);
        d.Need(d.V(1).HandCount == 0 && d.Location(gift) == CardLocation.DiscardPile &&
            d.G.CardMovements.Count(m => m.CardId == gift && m.To == CardLocation.DiscardPile) == 1 &&
            !d.G.CreateCardZoneDiagnostics().Any(c => c.Location.Zone == CardZoneKind.PublicPersistentPile && c.Location.OwnerSeat == 0),
            "The refusal branch makes Yu Jin perform one actual native foreign-card discard, creates no substitute pile and does not repeat either paid entity after replay.");
    }

    private static bool Conversion(PromptChoice choice) => choice.Parameters.GetValueOrDefault("conversion-skill-id") == Skill || choice.Parameters.GetValueOrDefault("conversion-skill") == Skill;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public int PaidColdCount;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public int Card(int seat, CardKind kind) => V(seat).Hand.First(c => c.Kind == kind).Id;
        public CardLocation Location(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Need(bool ok, string text) { if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { Pending = P, Players = G.CreateSnapshot(0).Players.Select(p => new { p.Seat, p.Hp, p.HandCount }), Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) })); }
        public void Accept(GameCommand c) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected native Jieyue command."); }
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Skip() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Choice(string option) { Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == option) == true); Answer(c => c.Parameters.GetValueOrDefault("option-id") == option); }
        public void Drive(int seat, string action, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(seat, Driver, action, cards, targets, G.Revision, P!.PromptId));
        public void Gift(int card, int target) { Drive(0, "gift", [card], [target]); Play(0); }
        public void Native(int seat, int card, int? target = null)
        {
            var a = G.GetLegalActions(seat).First(a => a.CardId == card && a.ConversionSource is null && (target is null ? a.Kind == LegalActionKind.Equip : a.TargetSeats.SequenceEqual([target.Value])));
            Accept(new PlayCardCommand(seat, card, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId));
        }
        public void BeginJieyue(int target, int cost)
        {
            Offer(); Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target") == true); Answer(c => c.Targets.SequenceEqual([target]));
            Reach(() => P?.Choices.Any(c => c.Cards.SequenceEqual([cost])) == true); Answer(c => c.Cards.SequenceEqual([cost]));
        }
        public void SelectStore(int card)
        {
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "mobile-yu-jin-jieyue" && c.Cards.SequenceEqual([card])) == true);
            Answer(c => c.Cards.SequenceEqual([card]));
        }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 400 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } setup) { Accept(new SelectGeneralCommand(setup.PlayerSeat, setup.PlayerSeat == 0 ? Owner : Peer(setup.PlayerSeat), G.Revision, setup.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId == Watch) { PaidColdCount++; Cold(); Continue(); continue; }
                if (P is { Kind: DecisionKind.PlayCard } play && play.PlayerSeat != 0) { End(play.PlayerSeat); continue; }
                if (P is { Kind: DecisionKind.Nullification }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "pass"); continue; }
                if (P is { Kind: DecisionKind.RespondDodge }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage"); continue; }
                if (P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards") == true) { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards"); continue; }
                Need(P is null, "Unexpected Jieyue prompt."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded Jieyue native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), G.CardMovements,
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "The original native receipt, views, events and physical movement history survive checkpoint JSON replay.");
        }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-yu-jin-peer-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-yu-jin", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            b.AddCard(new("fixture:mobile-yu-jin-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            object[] pause = [new { op = "chooseOption", target = "owner", resultBind = "continued", options = new[] { new { id = "continue" } } }];
            object[] skills = [
                new { id = Driver, revision = 1, activations = new object[] {
                    new { id = "gift", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "gift" }, new { op = "moveBoundCards", target = "owner", sourceBind = "gift", destination = "selectedTargetHand" } } },
                    new { id = "slash", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "useVirtualCard", target = "selectedTarget", outputKind = "slash", targetRestriction = "distanceUnlimitedAgainstTarget" } } },
                    new { id = "wound", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "loseHp", target = "owner", amount = 1 } } } } },
                new { id = Opening, revision = 1, modifiers = new[] { new { id = "whole-fixed-opening", query = "initialHandSize", operation = "add", value = 20, priority = 0 },
                    new { id = "preserve-test-materials", query = "handLimit", operation = "add", value = 30, priority = 0 } } },
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "zero-normal-draw", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Watch, revision = 1, triggers = new object[] {
                    new { id = "original-paid-hand-store", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" }, movementOccurrence = "perOwnerBatch", movementReasons = new[] { StoreReason }, optional = false, isSkillInvocation = false, effects = pause },
                    new { id = "original-paid-equipment-store", window = "cardsMoved", subject = "owner", sourceZones = new[] { "equipment" }, movementOccurrence = "perOwnerBatch", movementReasons = new[] { StoreReason }, optional = false, isSkillInvocation = false, effects = pause },
                    new { id = "original-paid-pile-retrieval", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { RetrieveReason }, optional = false, isSkillInvocation = false, effects = pause },
                    new { id = "actual-lion-recovery", window = "afterHealthChanged", subject = "owner", optional = false, isSkillInvocation = false,
                        condition = new { kind = "healthChangeKindIs", hpChangeKind = "recovery" }, effects = pause } } }
            ];
            var labels = new Dictionary<string, object>();
            foreach (var id in new[] { Driver, Quiet, Watch, Opening }) labels[id] = new { name = "真实节钺夹具", description = "原生牌区、阶段和已支付子结算。" };
            labels[Watch] = new { name = "节钺原付款子窗", description = "在原生存取牌或恢复子结算中暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动于禁", "", Skill, "wei", 8, [Driver, Quiet, Opening, Watch]));
            for (var i = 1; i < 4; i++) b.AddGeneral(new(Peer(i), "真实同桌", "", Driver, "wu", 8, [Quiet, Watch]));
            const string deck = "fixture:mobile-yu-jin-deck";
            b.AddDeck(new(deck, "固定二十张原始实体", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 4).SelectMany(i => new[] {
                new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7), new ContentDeckPhysicalCard("standard:slash", Suit.Heart, 6),
                new ContentDeckPhysicalCard("standard:dismantlement", Suit.Club, 8), new ContentDeckPhysicalCard("standard:bagua", Suit.Diamond, 5),
                new ContentDeckPhysicalCard("fixture:mobile-yu-jin-lion", Suit.Club, 1) }).ToArray() });
            b.AddMode(new(Mode, "移动节钺真实身份", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
