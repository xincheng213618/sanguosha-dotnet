using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhugeJinChecks
{
    private const string Hero = "mobile:zhuge-jin", Owner = "fixture:mobile-zhuge-jin-owner", Peer = "fixture:mobile-zhuge-jin-peer", Driver = "fixture:mobile-zhuge-jin-driver",
        Watch = "fixture:mobile-zhuge-jin-gain", Hongyuan = "mobile:hongyuan", Mingzhe = "mobile:mingzhe", Mode = "identity:mobile-zhuge-jin", Deck = "fixture:mobile-zhuge-jin-deck";
    public static void HongyuanOrderAndOffTurnRedDiscardPerCardCold()
    {
        var s = Scene.Start(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hongyuan);
        var before = Enumerable.Range(0, 4).Select(i => s.V(i).HandCount).ToArray();
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-targets") == true);
        s.Answer(c => c.Targets.SequenceEqual([3, 0])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Hongyuan); var id = root.Id;
        s.Need(root.SelectedTargetSeats.SequenceEqual([3, 0]) && root.NativeDraw is { RecipientIndex: 0, RecipientApplied: true } draw && draw.Recipients.SequenceEqual([3, 0]) &&
            s.V(3).HandCount == before[3] + 1 && s.V(0).HandCount == before[0] && s.V(2).HandCount == before[2],
            "The owner at seat2 commits recipients in actual action order 3 then0; only the first native draw is paid before its real gain child and normal draw.");
        s.Need(s.G.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(f => f.Id == root.WindowContext!.ParentFrameId).NormalDrawAdjustment == -1,
            "The original draw-phase parent has paid its reduction before any beneficiary children run.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
        s.Answer(c => c.Targets.SequenceEqual([2])); s.Play(2);
        s.Need(s.V(2).HandCount == before[2] + 1 && s.V(3).HandCount == before[3] + 1 && s.V(0).HandCount == before[0] + 1 &&
            s.G.CardMovements.Count(m => m.To == CardLocation.Hand(2) && m.Reason == CardMoveReasons.Draw) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Hongyuan && e.BindingId == "reduce-and-support" && e.Activated && e.Completed) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != id) && s.V(2).Skills!.All(skill => skill.Id != Hongyuan) &&
            s.E<ProgramOwnerSkillsReplacedEvent>().Count(e => e.OwnerSeat == 2 && e.LostSkillIds.SequenceEqual([Hongyuan])) == 1,
            "Cold completes both beneficiaries once; a real dying-entry child removes Hongyuan, yet its already-paid normal draw remains reduced from two to one.");
        var ownCard = s.V(2).Hand.First().Id;
        s.Accept(new UseProgramSkillCommand(2, Driver, "own-discard", [ownCard], [], s.G.Revision, s.P!.PromptId)); s.Play(2);
        s.Need(!s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Mingzhe), "A real red discard during the owner's actual turn gives no mobile Mingzhe offer.");
        s.Accept(new EndPlayPhaseCommand(2, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Driver); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        var paid = s.V(2).Hand.Take(2).Select(c => c.Id).ToArray(); var hand = s.V(2).HandCount;
        foreach (var card in paid) s.Answer(c => c.Cards.SequenceEqual([card]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Mingzhe);
        var discardWindow = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last(); var batch = discardWindow.Batch;
        s.Need(batch.Movements.Count == 2 && batch.Movements.Select(m => m.CardId).SequenceEqual(paid) && batch.Movements.All(m =>
            m.From == CardLocation.Hand(2) && m.To == CardLocation.DiscardPile && m.RedOwnedLoss is { EffectiveSuit: Suit.Heart, Timing.ActualTurnOwnerSeat: 3 }) && s.V(2).HandCount == hand - 2,
            "The other actual turn's original native discard atomically pays two real red hand entities and freezes their effective suit and actual turn owner.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        var mingzheId = s.E<ProgramBindingStartedEvent>().Single(e => e.SkillId == Mingzhe && e.BindingId == "red-hand-discard").FrameId;
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Mingzhe);
        s.Need(s.V(2).HandCount == hand - 1 && s.G.CardMovements.Count(m => m.To == CardLocation.Hand(2) && m.Reason.Value == "skill-program.mobile:mingzhe.Draw") == 1,
            "Each discarded entity has its own optional Mingzhe choice; the accepted first one has paid only one native draw before the second choice.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play(3);
        s.Need(s.V(2).HandCount == hand - 1 && paid.All(card => s.G.CardMovements.Count(m => m.CardId == card && m.Reason.Value == "skill-program." + Driver + ".MoveBoundCards" && m.To == CardLocation.DiscardPile) == 1) &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == mingzheId && e.SkillId == Mingzhe && e.BindingId == "red-hand-discard" && e.Activated && e.Completed) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == discardWindow.Id && e.SkillId == Mingzhe && e.BindingId == "red-hand-discard" && !e.Activated) == 1,
            "One accepted and one declined per-card choice draw exactly once; neither the original discard nor a paid draw is repeated.");

        var zero = Scene.Start(); zero.Reach(() => zero.P?.SkillPrompt?.SkillId == Hongyuan);
        var z = Enumerable.Range(0, 4).Select(i => zero.V(i).HandCount).ToArray();
        zero.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        zero.Reach(() => zero.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-targets") == true);
        zero.Answer(c => c.Targets.Count == 0); zero.Play(2);
        zero.Need(zero.V(2).HandCount == z[2] + 1 && new[] { 0, 1, 3 }.All(seat => zero.V(seat).HandCount == z[seat]),
            "An explicitly committed zero-recipient selection remains legal, still reduces the real normal draw, and issues no default or unchosen recipient draw.");
    }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Zhuge Jin command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 180 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } setup) { Accept(new SelectGeneralCommand(setup.PlayerSeat, setup.PlayerSeat == 2 ? Owner : Peer + "-" + setup.PlayerSeat, G.Revision, setup.PromptId)); continue; }
                Need(P is null, "Unexpected mobile Zhuge Jin prompt."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Missing bounded mobile Zhuge Jin boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves original reduced normal draw and the same sequential beneficiary producer."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, Hands = Enumerable.Range(0, 4).Select(i => V(i).HandCount), OwnerHp = V(2).Hp, Damage = E<DamageAppliedEvent>(), Dying = E<PlayerDyingEvent>(), OwnerSkillIds = V(2).Skills?.Select(skill => skill.Id), HongyuanResults = E<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Hongyuan), MingzheResults = E<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Mingzhe), Replacements = E<ProgramOwnerSkillsReplacedEvent>(), G.State.TurnNumber })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 2, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, r));
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-zhuge-jin", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "ol:zhuge-jin");
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { new { id = "own-discard", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "discardSelected", target = "owner", amount = 1 } } } }, triggers = new object[] { new { id = "other-turn-real-two-red-cost", window = "turnStartBeforeNormalFlow", subject = "owner", turnOwnerScope = "otherLiving", optional = true, usageScope = "game", usageLimit = 1,
                    effects = new object[] { new { op = "selectOwnedCards", target = "owner", zones = new[] { "hand" }, suits = new[] { "heart", "diamond" }, amount = 2, resultBind = "actual-red-cost" },
                        new { op = "moveBoundCards", target = "owner", sourceBind = "actual-red-cost", destination = "discardPile", awaitMovementTriggers = true } } },
                    new { id = "lose-hongyuan-during-beneficiary-child", window = "dyingEntering", subject = "owner", optional = false, usageScope = "game", usageLimit = 1,
                        effects = new object[] { new { op = "recoverTo", target = "owner", numberExpression = "integerConstant", minimumValue = 2, clampToMaxHp = true },
                            new { op = "loseOwnerSkillsAndGrant", target = "owner", skillIds = new[] { Hongyuan }, sourceBind = "standard:none" } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "first-beneficiary-paid", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", optional = false,
                    movementReasons = new[] { "skill-program.mobile:hongyuan.Draw" }, usageScope = "game", usageLimit = 1,
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } },
                        new { op = "selectTarget", target = "owner", targetKind = "anyLiving" }, new { op = "damage", target = "selectedTarget", amount = 5 } } } } } };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实回合内外红牌费用", description = "原生弃牌验证实际回合归属。" },
                [Watch] = new { name = "第一名弘援受益者", description = "原生摸牌获得孩子。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动诸葛瑾实际身份", "supporter", "ol:huanshi", "wu", 3, AdditionalSkillIds: [Hongyuan, Mingzhe, Driver], Gender: GeneralGender.Male));
            foreach (var seat in new[] { 0, 1, 3 }) b.AddGeneral(new(Peer + "-" + seat, "真实同行", "supporter", seat == 3 ? Watch : "standard:none", "wei", 4));
            b.AddDeck(new(Deck, "固定红牌正常摸牌", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 5)).ToArray() });
            b.AddMode(new(Mode, "移动诸葛瑾真实身份", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 1 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer + "-0", Peer + "-1", Peer + "-3"]));
        }
    }
}
