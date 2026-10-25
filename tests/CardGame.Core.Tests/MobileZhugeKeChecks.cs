using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhugeKeChecks
{
    private const string Hero = "mobile:zhuge-ke", Aocai = "mobile:aocai", Duwu = "mobile:duwu", Owner = "fixture:mobile-zhuge-ke-owner",
        Mode = "identity:classic-mobile-zhuge-ke", Driver = "fixture:mobile-zhuge-ke-driver", Quiet = "fixture:mobile-zhuge-ke-quiet",
        Cost = "fixture:mobile-zhuge-ke-cost", Hp = "fixture:mobile-zhuge-ke-hp", DiscardReason = "program.dynamic-target-hp.discard";

    public static void FatalDuwuPaysOnceUntilActualTurnEndsAndEmptyAocaiViewsTwoCold()
    {
        var s = Scene.Start(); s.Play(0); s.Prepare(); var turn = s.G.State.TurnNumber; var ownerHp = s.V().Hp; var cost = s.V().Hand.Single().Id;
        s.Need(ownerHp == 4 && s.V(2).Hp == 1 && s.V(3).Hp == 1 && s.HasDuwu(3) && !s.HasDuwu(2) &&
            s.G.GetCombatDistance(0, 3) == 1 && s.G.GetCombatDistance(0, 2) == 2 && s.G.GetAttackRange(0) == 1,
            "A supported four-human identity table prepares real HP1 targets through native damage, keeps one actual HE cost, and offers the real mobile Duwu activation in range.");
        s.Accept(new UseProgramSkillCommand(0, Duwu, "target-hp-cost", [], [3], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "dynamic-discard-damage") == true);
        s.Answer(c => c.Cards.SequenceEqual([cost])); s.Hold(Cost); var root = s.Root; var id = root.Id;
        s.Need(root.DynamicDiscardDamage is { Stage: DynamicDiscardDamageStage.Paid, FrozenTargetHp: 1, FrozenDistance: 1, FrozenAttackRange: 1, TargetSeat: 3, DamageFrameId: null } invoice &&
            invoice.CardIds.SequenceEqual([cost]) && root.PendingMovementContinuation is not null && s.Zone(cost) == CardLocation.DiscardPile &&
            s.V().HandCount == 0 && s.V(3).Hp == 1 && s.V(3).IsAlive && s.E<DynamicDiscardDamagePenaltyEvent>().Length == 0,
            "The original native target-HP frame pays exactly one real hand card and suspends at its actual discard child before issuing damage or any dying penalty.");
        s.Cold(); s.Continue(); s.Hold(Hp);
        var paid = s.Root.DynamicDiscardDamage!; var penalty = s.E<DynamicDiscardDamagePenaltyEvent>().Single();
        var child = s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(h => h.ResumeFrameId == id);
        s.Need(s.Root.Id == id && paid is { Stage: DynamicDiscardDamageStage.PenaltyIssued, DyingSurvived: false } &&
            paid.DyingFrameId == penalty.DyingFrameId && paid.DamageFrameId == penalty.DamageFrameId && penalty.TargetSeat == 3 && penalty.FrameId == id &&
            !s.V(3).IsAlive && s.V().Hp == ownerHp - 1 && s.G.State.Winner == Winner.None &&
            child.Continuation == PostEventContinuation.Program && child.Change is { Kind: HpChangeKind.Loss, TargetSeat: 0, Amount: 1 } && child.Change.ParentFrameId == id &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.FrameId == id && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.E<SkillUsageConsumedEvent>().Count(e => e.SkillId == Duwu && e.UsageId == "dynamic-discard-damage-blocked" && e.Scope == SkillUsageScope.Turn) == 1,
            "The exact unsaved native victim has died without ending the table: mobile Duwu still loses one owner HP, records one actual-turn block, and pauses in its original paid HP-loss child.");
        s.Cold(); s.Continue(); s.Play(0); s.Drive("gain-one"); s.Play(0);
        s.Need(s.V().HandCount == 1 && s.V(2).Hp == 1 && s.G.GetCombatDistance(0, 2) <= s.G.GetAttackRange(0) && !s.HasDuwu(2) && s.G.State.TurnNumber == turn &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == DiscardReason) == 1 &&
            s.E<DynamicDiscardDamagePenaltyEvent>().Count(e => e.FrameId == id) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == id && e.Completed) == 1,
            "Cold returns through the same paid attack once. Even after a genuine draw funds another adjacent HP1 target, Duwu remains unavailable for the rest of this actual turn.");
        s.Stock(1); s.Play(0); s.End(); s.Play(1);
        var slash = s.V(1).Hand.Single(c => c.Kind == CardKind.Slash).Id; s.NativeSlash(1, slash, 0);
        s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 } && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("deck-basic-source") == "activate"));
        var original = s.P!; var incoming = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == slash).Id;
        s.Need(s.V().HandCount == 0 && s.G.CreateCardZoneDiagnostics().Count(c => c.Location == CardLocation.DrawPile) == 4,
            "An actual foreign physical Slash creates an empty-hand Dodge need while four real cards remain on top, so a two-card result cannot be explained by a short pile.");
        s.Answer(c => c.Parameters.GetValueOrDefault("deck-basic-source") == "activate");
        var view = s.G.ResolutionStack.OfType<RequestedDeckBasicFrame>().Single(); var top = view.CardIds.ToArray();
        s.Need(view.Receipt is { ViewedCount: 2, ActorSeat: 0, Intent: RequestedDeckBasicIntent.Dodge } && top.Length == 2 &&
            view.Receipt.Source.SkillId == Aocai && view.Receipt.OwnerFrameId == incoming && view.OriginalDecision.PromptId == original.PromptId &&
            s.G.CreateSnapshot(0).PrivateRevealedCards!.Select(c => c.Id).SequenceEqual(top) &&
            Enumerable.Range(1, 3).All(seat => s.G.CreateSnapshot(seat).PrivateRevealedCards is null && s.G.CreateSnapshot(seat).PendingDecision is null),
            "Empty-hand mobile Aocai privately views exactly two, preserving the original issued response request; it never inherits OL's four-card empty-hand branch.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("deck-basic-source") == "card"); s.Play(1);
        var response = s.E<RequestedDeckBasicPaidEvent>().Single(e => e.SkillId == Aocai);
        s.Need(response.OwnerFrameId == incoming && response.ActorSeat == 0 && response.CardKind == CardKind.Dodge && top.Contains(response.CardId) &&
            s.G.CardMovements.Count(m => m.CardId == response.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing) == 1 &&
            top.Where(card => card != response.CardId).All(card => s.Zone(card) == CardLocation.DrawPile) &&
            s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 0 && e.CardId == response.CardId && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == incoming && e.CardId == slash) == 1 && s.V().Hp == ownerHp - 1 && s.V().HandCount == 0,
            "The same cold-restored native response pays one chosen deck entity once, actually Dodges the original Slash, and leaves the unchosen viewed card in the pile without adding it to hand.");
        s.End(); s.Play(0); s.Drive("gain-one"); s.Play(0);
        s.Need(s.G.State.TurnNumber > turn && s.V().HandCount == 1 && s.G.GetCombatDistance(0, 2) <= s.G.GetAttackRange(0) && s.HasDuwu(2) &&
            s.E<DynamicDiscardDamagePenaltyEvent>().Count(e => e.FrameId == id) == 1 && s.V().Hp == ownerHp - 1,
            "After real foreign turns finish, a newly funded adjacent HP1 target is legal again in the owner's next actual turn; the old death and HP-loss invoice is never paid twice.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public ProgramSkillFrame Root => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Duwu);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public bool HasDuwu(int target) => G.GetLegalActions(0).Any(a => a.ProgramSkillId == Duwu && a.ProgramActivationId == "target-hp-cost" && a.SelectableTargetSeats.Contains(target));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Hold(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill);
        public void End() { var p = P!; Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Drive(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Stock(int amount) => Accept(new UseProgramSkillCommand(0, Driver, amount == 4 ? "stock-four" : "stock-one", V().Hand.Where(c => c.Kind == CardKind.Dodge).Take(amount).Select(c => c.Id).ToArray(), [], G.Revision, P!.PromptId));
        public void Prepare()
        {
            Stock(4); Play(0);
            Accept(new UseProgramSkillCommand(0, Driver, "supply-slash", [V().Hand.Single(c => c.Kind == CardKind.Slash).Id], [1], G.Revision, P!.PromptId)); Play(0);
            Drive("prepare-wound", 3); Play(0); Drive("prepare-wound", 2); Play(0);
        }
        public void NativeSlash(int actor, int card, int target)
        {
            var a = G.GetLegalActions(actor).Single(a => a.CardId == card && a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(actor, card, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(match).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 220; i++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.RescueDying }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die"); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                Need(P is null, "Unexpected native mobile Zhuge Ke boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native mobile Zhuge Ke scene missed its exact original child boundary.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves original HE payment, actual fatal dying return, paid owner penalty and native private response request.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected native mobile Zhuge Ke command."); }
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, Players = Enumerable.Range(0, 4).Select(V).Select(p => new { p.Seat, p.Hp, p.HandCount, p.IsAlive }), Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var g = GameEngine.CreateStandard(new() { Seed = 71, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-zhuge-ke-peer-" + seat;

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-zhuge-ke", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } };
            object Stock(string id, int count) => new { id, minCards = count, maxCards = count, sourceZones = new[] { "hand" }, cardKinds = new[] { "dodge" }, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, usesPerGame = (int?)null,
                effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "stock" }, new { op = "moveBoundCards", target = "owner", sourceBind = "stock", destination = "drawPileTop", awaitMovementTriggers = true } } };
            var skills = new object[] {
                new { id = Driver, revision = 1, modifiers = new[] { new { id = "fixed-six-opening", query = "initialHandSize", operation = "add", value = 6, priority = 0 } }, activations = new object[] {
                    Stock("stock-four", 4), Stock("stock-one", 1),
                    new { id = "supply-slash", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, cardKinds = new[] { "slash" }, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null, usesPerGame = (int?)null, effects = new[] { new { op = "giveSelected", target = "selectedTarget", amount = 1 } } },
                    new { id = "prepare-wound", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null, usesPerGame = (int?)null, effects = new[] { new { op = "damage", target = "selectedTarget", amount = 3 } } },
                    new { id = "gain-one", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, usesPerGame = (int?)null, effects = new[] { new { op = "draw", target = "owner", amount = 1 } } } } },
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false, effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Cost, revision = 1, triggers = new[] { new { id = "real-paid-hand-cost", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" }, movementOccurrence = "perOwnerBatch", movementReasons = new[] { DiscardReason }, optional = false, isSkillInvocation = false, effects = new[] { Pause() } } } },
                new { id = Hp, revision = 1, triggers = new[] { new { id = "actual-owner-penalty", window = "afterHpLost", subject = "owner", optional = false, isSkillInvocation = false, effects = new[] { Pause() } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "固定原生准备", description = "实际发牌、放回牌堆、给杀及原生预伤" }, [Quiet] = new { name = "固定正常摸牌", description = "保留实际牌堆顺序" } };
            foreach (var id in new[] { Cost, Hp }) labels[id] = new { name = "原生实付孩子", description = "真实费用或失去体力后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动诸葛恪固定身份", "supporter", Aocai, "wu", 3, AdditionalSkillIds: [Duwu, Driver, Quiet, Cost, Hp], Gender: GeneralGender.Male));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "固定原生同桌", "supporter", Quiet, "wei", 4));
            const string deck = "fixture:mobile-zhuge-ke-deck";
            b.AddDeck(new(deck, "固定六张真实实体", 0, 2, []) { PhysicalCards = new[] { new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7) }.Concat(Enumerable.Range(0, 5).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, i + 2))).ToArray() });
            b.AddMode(new(Mode, "移动诸葛恪原生费用与私看", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
