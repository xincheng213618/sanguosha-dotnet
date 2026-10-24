using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryHuangZhongMobileChecks
{
    private const string Hero = "boundary:huang-zhong-mobile", Skill = "boundary:liegong-mobile", Owner = "fixture:mobile-huang-zhong-owner",
        Driver = "fixture:mobile-huang-zhong-driver", Mode = "identity:classic-mobile-huang-zhong";

    public static void RankAddsPermissionAndOriginalTargetBenefitsDoNotLeakCold()
    {
        var low = Scene.Start(weapon: true); low.Play();
        var lowCard = low.V(0).Hand[0].Id;
        low.Need(low.G.GetLegalActions(0).All(a => a.CardId != lowCard || a.Kind != LegalActionKind.Slash || !a.TargetSeats.Contains(2)),
            "A rank-one Slash without a weapon cannot reach a role at native distance two.");
        low.Use("equip", 0); low.Play();
        low.Need(low.V(0).Equipment.Any(c => c.Kind == CardKind.QinggangSword), "The native deck equipment producer supplies a real range-two weapon.");
        var lowAction = low.Slash(lowCard, [2]); low.PlayAction(lowAction); low.Reach(low.Offered);
        var lowUse = low.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Action?.ActorSeat == 0 && f.CardKind == CardKind.Slash);
        low.Need(lowUse.Action!.EffectiveRank == 1 && lowUse.TargetSeats.SequenceEqual(new[] { 2 }),
            "The actual low-rank material is legally paid against a distance-two role because mobile rank permission preserves ordinary weapon range.");
        low.Activate(); low.Play();
        low.Need(low.V(2).Hp == 3, "The accepted native low-rank Slash resolves its two independently qualified benefits after normal range admission.");

        var s = Scene.Start(); s.Play(); s.Use("draw-five", 2); s.Play(); s.Use("hurt", 2); s.Play();
        var material = s.V(0).Hand.First().Id; var originalHand = s.V(0).HandCount;
        var action = s.Slash(material, [1, 2]);
        s.Need(s.V(0).Equipment.Count == 0 && s.V(1).Hand.Any(c => c.Kind == CardKind.Dodge) && s.V(2).Hand.Any(c => c.Kind == CardKind.Dodge),
            "The high-rank native Slash reaches a distant target without a weapon, and both targets have real physical Dodges before target-local qualification.");
        s.PlayAction(action); s.Reach(s.Offered);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Action?.ActorSeat == 0 && f.CardKind == CardKind.Slash); var useId = use.Id; var actionId = use.Action!.ActionId;
        s.Need(use.Action.EffectiveRank == 13 && use.Action.EffectiveDesignatedTargetSeats.SequenceEqual(new[] { 1, 2 }) && s.V(0).HandCount == originalHand - 1,
            "One original actual converted Slash pays one physical material and retains both finalized targets with its effective rank.");
        s.Cold(); s.Activate(); s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 2 });
        var current = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == useId);
        var receipts = current.FinalTargetSlashReceipts!.Where(r => r.SkillId == Skill).ToArray();
        s.Need(receipts.Length == 2 && receipts.All(r => r.TargetSeat == 1 && r.ActionId == actionId && r.CardUseFrameId == useId) &&
            receipts.Count(r => r.PreventCancellation) == 1 && receipts.Sum(r => r.DamageBonus) == 1 &&
            s.V(1).Hp == 3 && s.V(2).Hp == 4 && s.E<CardRespondedEvent>().All(e => e.ResponderSeat != 1) &&
            s.G.CardMovements.All(m => m.From != CardLocation.Hand(1)),
            "Both mobile effects issue for the first target on the original Use; its real Dodge is never offered or paid and it takes two damage, while the ineligible second target retains a genuine Dodge window.");
        s.Cold(); var dodge = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single(); s.Answer(c => c.Cards.SequenceEqual([dodge])); s.Play();
        s.Need(s.V(2).Hp == 4 && s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 2 && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            s.E<CardRespondedEvent>().All(e => e.ResponderSeat != 1) && s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "The second target's actual paid Dodge cancels only its own Slash effect; both cold restores preserve original target rights, one physical use payment and one final native completion.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public bool Offered() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public LegalAction Slash(int card, int[] targets) => G.GetLegalActions(0).First(a => a.CardId == card && a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual(targets));
        public void PlayAction(LegalAction action) => Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
            { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Huang Zhong command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 180 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-huang-zhong-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger or DecisionKind.RespondDodge }) throw new InvalidOperationException("Unexpected mobile Huang Zhong boundary: " + Short());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Bounded mobile Huang Zhong scene missed its expected boundary.");
        }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold retains native rank admission, original target-local receipts and one physical Slash payment."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        private string Short() => JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) });
        public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + Short()); }
        public static Scene Start(bool weapon = false)
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(weapon)); var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r)); s.Accept(new StartGameCommand()); return s; }
    }
    private sealed class Fixture(bool weapon) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-huang-zhong-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Activation(string id, object effect) => new { id, minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { effect } };
            var skills = new object[] { new { id = Driver, revision = 1,
                activations = new[] {
                    Activation("equip", new { op = "useRandomDeckEquipment", target = "owner", resultBind = "equipped" }),
                    Activation("draw-five", new { op = "draw", target = "selectedTarget", amount = 5 }),
                    Activation("hurt", new { op = "loseHp", target = "selectedTarget", amount = 1 }) },
                viewAs = new[] { new { id = "red-slash", inputKinds = Array.Empty<string>(), inputSuits = new[] { "heart" }, outputKind = "slash", forPlay = true, forResponse = false } },
                modifiers = new[] { new { id = "one-native-extra-target", query = "cardTargetCount", operation = "add", value = 1, cardKinds = new[] { "slash", "fireSlash", "thunderSlash" }, priority = 0 } }
            } };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实固定额外目标", description = "合法原生红牌杀、双目标与装备摸牌" } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动界黄忠真实场景", "supporter", Skill, "shu", 4, AdditionalSkillIds: [Driver]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-huang-zhong-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "固定真实角色", "supporter", "standard:none", "wu", 5));
            const string deck = "fixture:mobile-huang-zhong-deck";
            b.AddDeck(new(deck, "固定真实基本/装备实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard(weapon ? "standard:qinggang_sword" : "standard:dodge", Suit.Heart, weapon ? 1 : 13)).ToArray() });
            b.AddMode(new(Mode, "移动界黄忠原生流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
