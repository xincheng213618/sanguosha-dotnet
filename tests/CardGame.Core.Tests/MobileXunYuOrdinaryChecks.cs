using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileXunYuOrdinaryChecks
{
    private const string Quhu = "mobile:quhu", Jieming = "mobile:jieming", Owner = "fixture:ordinary-mobile-xun-yu",
        Driver = "fixture:ordinary-mobile-xun-yu-damage", Clock = "fixture:ordinary-mobile-xun-yu-extra-play",
        Watch = "fixture:ordinary-mobile-xun-yu-refill-child", Deck = "fixture:ordinary-mobile-xun-yu-deck",
        Mode = "identity:classic-ordinary-mobile-xun-yu", DrawReason = "skill-program.hand-count.draw";

    public static void QuhuUsesSeparateActualPlayQuotasAndOriginalPindianDamageSource()
    {
        var s = Scene.Start(extraPlay: true); var turn = s.G.State.TurnNumber; var hp = s.V(0).Hp;
        s.Need(s.V(0).HandCount == 2 && s.V(1).HandCount == 2 && s.V(1).Hp > hp,
            "A fixed real extra Play starts with two genuine equal-rank hand cards and a higher-HP opponent.");
        var first = s.V(0).Hand[0].Id; s.Contest(); s.FinishContest();
        s.Need(s.V(0).Hp == hp - 1 && s.V(0).HandCount == 1 && s.V(1).HandCount == 1 && !s.CanContest &&
            s.E<PindianResultDeterminedEvent>().Single().Result is { SourceSeat: 0, OpponentSeat: 1, SourceRank: 7, OpponentRank: 7, SourceWon: false } &&
            s.E<DamageAppliedEvent>().Single() is { SourceSeat: 1, TargetSeat: 0, Amount: 1 } &&
            s.G.CardMovements.Count(m => m.CardId == first && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "A tied native contest pays its original material once and the actual opponent deals the loss damage; another otherwise legal contest is unavailable in this same Play.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.G.State.TurnNumber == turn && s.CanContest && s.V(0).HandCount == 1 && s.V(1).HandCount == 1 &&
            s.E<ProgramPhaseScheduledEvent>().Any(e => e.SkillId == Clock && e.Phase == TurnPhase.Play && !e.Started),
            "The inserted Play returns to the separate ordinary Play in the same actual turn, reopening precisely one Quhu allowance rather than retaining classic's whole-turn limit.");
        s.Contest(); s.FinishContest();
        s.Need(s.G.State.TurnNumber == turn && s.V(0).Hp == hp - 2 && s.V(0).HandCount == 0 && s.V(1).HandCount == 0 &&
            s.E<PindianResultDeterminedEvent>().Length == 2 && s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 1 && e.TargetSeat == 0 && e.Amount == 1) == 2 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.SkillId == Quhu && e.Completed) == 2 &&
            s.G.CardMovements.Where(m => m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing).Select(m => m.CardId).Distinct().Count() == 2,
            "Both native Play phases complete their distinct physical Pindian and opponent-sourced damage exactly once.");
    }

    public static void JiemingEachDamagePointCapsRecipientRefillAndReturnsNativeDrawCold()
    {
        var s = Scene.Start(extraPlay: false); var hp = s.V(0).Hp;
        s.Need(s.V(1) is { MaxHp: 8, HandCount: 0 } && s.V(2) is { MaxHp: 3, HandCount: 0 },
            "The real recipients have maximum HP above five and below five, with no starting or normal-draw cards.");
        s.Accept(new UseProgramSkillCommand(0, Driver, "hurt-two", [], [], s.G.Revision, s.P!.PromptId)); s.OfferJieming();
        s.Activate(); s.Answer(c => c.Targets.SequenceEqual([1])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Jieming);
        var movement = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single();
        var ids = s.V(1).Hand.Select(c => c.Id).ToArray();
        s.Need(s.V(0).Hp == hp - 2 && s.E<DamageAppliedEvent>() is [{ TargetSeat: 0, Amount: 2 }] && ids.Length == 5 &&
            s.V(2).HandCount == 0 && root.SelectedTargetSeats.SequenceEqual([1]) &&
            movement.Batch.ParentFrameId == root.Id && s.E<CardsDrawnEvent>() is [{ PlayerSeat: 1, Count: 5 }] &&
            s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1) && m.Reason.Value == DrawReason) == 5 &&
            movement.Batch.Movements.Count > 0 && movement.Batch.Movements.All(m => ids.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1) && m.Reason.Value == DrawReason),
            "The first point owns an actual five-entity refill; its original native gain child observes one real DrawCards movement batch from that exact five-card draw. " +
            JsonSerializer.Serialize(new { root.Id, root.InstructionIndex, Targets = root.SelectedTargetSeats, movement.Batch.ParentFrameId,
                BatchCount = movement.Batch.Movements.Count, DrawFacts = s.E<CardsDrawnEvent>() }));
        var rootId = root.Id; s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.OfferJieming();
        s.Activate(); s.Answer(c => c.Targets.SequenceEqual([2])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        s.Need(s.V(1).Hand.Select(c => c.Id).SequenceEqual(ids) && s.V(2).HandCount == 3 &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Jieming).Id != rootId,
            "The second actual damage point offers a distinct Jieming frame and refills a different recipient to its own maximum HP three, without repeating the first five-card gain.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.E<DamageAppliedEvent>().Length == 1 && s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Jieming && e.Activated && e.Completed) == 2 &&
            s.G.CardMovements.Count(m => m.Reason.Value == DrawReason && m.To == CardLocation.Hand(1)) == 5 &&
            s.G.CardMovements.Count(m => m.Reason.Value == DrawReason && m.To == CardLocation.Hand(2)) == 3 &&
            ids.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1)) == 1),
            "Cold returns complete each point once: one damage event produces two real capped refills of five and three, with no repeated entities or synthetic discard-to-five.");
    }

    private static string Peer(int seat) => "fixture:ordinary-mobile-xun-yu-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool CanContest => G.GetLegalActions(0).Any(a => a.ProgramSkillId == Quhu && a.SelectableTargetSeats!.Contains(1));
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected real ordinary mobile Xun Yu command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing actual ordinary Xun Yu choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Contest() => Accept(new UseProgramSkillCommand(0, Quhu, "contest", [V(0).Hand[0].Id], [1], G.Revision, P!.PromptId));
        public void FinishContest()
        {
            Reach(() => P?.PlayerSeat == 1 && G.ResolutionStack.LastOrDefault() is PindianFrame);
            Answer(c => c.Cards.Count == 1); OfferJieming(); Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); Play();
        }
        public void OfferJieming() => Reach(() => P?.SkillPrompt?.SkillId == Jieming && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 160; step++)
            {
                if (ready()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } choose)
                { Accept(new SelectGeneralCommand(choose.PlayerSeat, choose.PlayerSeat == 0 ? Owner : Peer(choose.PlayerSeat), G.Revision, choose.PromptId)); continue; }
                Need(P is null, "Unexpected ordinary Xun Yu native boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded ordinary Xun Yu boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves both damage-point cursor and original native five-card draw/recipient/physical entities.");
        }
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.Phase, OwnerHp = V(0).Hp,
            Hands = Enumerable.Range(0, 4).Select(i => V(i).HandCount).ToArray(), Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
        public static Scene Start(bool extraPlay)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(extraPlay));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, r), r);
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture(bool extraPlay) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-xun-yu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:xun-yu");
            var definitions = new object[] {
                new { id = Driver, revision = 1, activations = new[] { new { id = "hurt-two", usesPerTurn = (int?)null, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                    targetKind = "anyLiving", effects = new[] { new { op = "damage", target = "owner", amount = 2 } } } } },
                new { id = Clock, revision = 1, triggers = new[] { new { id = "one-native-extra-play", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false,
                    isSkillInvocation = false, priority = 1000, usageScope = "game", usageLimit = 1, effects = new[] { new { op = "insertPhase", target = "owner", phase = "play", phaseContinuation = "beforeNormalPreparation" } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "paid-native-refill-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                    movementReasons = new[] { DrawReason }, movementOccurrence = "perBatch", optional = false, isSkillInvocation = false, usageScope = "game", usageLimit = 1,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "实际两点伤害", description = "原生一次2伤，不手工改变HP。", activationLabels = new Dictionary<string, string> { ["hurt-two"] = "实际受到2伤" } },
                [Clock] = new { name = "一次真实额外出牌阶段", description = "返回正常准备流后再进入本回合普通Play。" },
                [Watch] = new { name = "实际节命补牌子窗", description = "只在真实补牌实体已进入接收者手牌后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "返回原节命" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "真实普通移动荀彧", "supporter", Quhu, "wei", 3, extraPlay ? [Jieming, Driver, Clock] : [Jieming, Driver]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实补牌参与者", "supporter", "standard:none", "wei", seat == 2 ? 3 : 8, extraPlay ? [] : [Watch]));
            b.AddDeck(new(Deck, "真实同点手牌与补牌实体", extraPlay ? 2 : 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(Mode, "普通移动荀彧实际阶段与逐点封顶补牌", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
