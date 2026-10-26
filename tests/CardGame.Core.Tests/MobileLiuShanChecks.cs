using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileLiuShanChecks
{
    private const string Skill = "mobile:xiangle", Actor = "fixture:ordinary-mobile-liu-shan-actor",
        Late = "fixture:ordinary-mobile-liu-shan-late-gain", Paid = "fixture:ordinary-mobile-liu-shan-paid",
        Deck = "fixture:ordinary-mobile-liu-shan-deck", Mode = "identity:classic-ordinary-mobile-liu-shan",
        CostReason = "skill-program.mobile:xiangle.SelectAndMoveOwnedCard";
    private static readonly Lazy<ContentCardDefinition> Halberd = new(() => StandardContentRegistry.CreateWithClassicGenerals().GetCard("classic:fangtian-halberd"));

    public static void FinalizedBasicPaymentColdAndOnlyOwnerIneffectiveBeforeLaterGain()
    {
        foreach (var pay in new[] { true, false })
        {
            var s = Scene.Start(halberd: !pay); var hp = s.V(1).Hp; var otherHp = s.V(2).Hp;
            if (!pay)
            {
                var equip = s.G.GetLegalActions(0).Single(a => a.Kind == LegalActionKind.Equip);
                s.Accept(new PlayCardCommand(0, equip.CardId!.Value, equip.TargetSeats, s.G.Revision, s.P!.PromptId, equip.PlayedCardKind)); s.Play();
                s.Need(s.V(0).Hand is [{ Kind: CardKind.FireSlash }] && s.V(0).Equipment is [{ Kind: CardKind.FangtianHalberd }],
                    "A real equipped Halberd and last physical Slash supply the native two-target use, leaving no basic card available for Xiangle's price.");
            }
            var slash = s.V(0).Hand.First(c => c.Kind == CardKind.FireSlash).Id;
            var fee = pay ? s.V(0).Hand.Single(c => c.Id != slash).Id : 0;
            s.Accept(new PlayCardCommand(0, slash, pay ? [1] : [1, 2], s.G.Revision, s.P!.PromptId));
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill);
            var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
            s.Need(root.OwnerSeat == 1 && root.WindowContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: 1, CardUse: { ActorSeat: 0 } } &&
                s.E<CardsDrawnEvent>().Length == 0 && s.E<DamageAppliedEvent>().Length == 0 && s.V(0).HandCount == (pay ? 1 : 0),
                "The target owner's Xiangle asks the actual actor at the finalized-target window, before any later target-effect gain or damage can supply a new basic card.");
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == (pay ? "pay" : "decline"));
            if (pay)
            {
                s.Reach(() => s.P?.Choices.Any(c => c.Cards.SequenceEqual([fee])) == true);
                s.Answer(c => c.Cards.SequenceEqual([fee])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid);
                s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == root.Id && f.SkillId == Skill) &&
                    s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any() && s.Zone(fee) == CardLocation.DiscardPile &&
                    s.G.CardMovements.Count(m => m.CardId == fee && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == CostReason) == 1 &&
                    s.V(0).HandCount == 0 && s.E<CardsDrawnEvent>().Length == 0,
                    "The genuine actor's original basic card has paid one discard and owns a real movement child while the finalized-target parent still precedes the late draw.");
                s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            }
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Late);
            var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == slash);
            s.Need(use.Action is { ActorSeat: 0, Type: CardActionType.Use, EffectiveKind: CardKind.FireSlash, PhysicalCards: [var material] } && material.CardId == slash &&
                use.Action.TargetSeats.SequenceEqual(pay ? [1] : [1, 2]) && use.TargetSeats.SequenceEqual(pay ? [1] : [1, 2]) &&
                (pay ? use.IneffectiveTargetSeats is null or { Count: 0 } : use.IneffectiveTargetSeats?.SequenceEqual([1]) == true) &&
                s.V(0).HandCount == 1 && s.E<CardsDrawnEvent>() is [{ PlayerSeat: 0, Count: 1 }] && s.E<DamageAppliedEvent>().Length == 0 &&
                s.E<ProgramCardEffectNullifiedEvent>().Count(e => e.SkillId == Skill && e.OwnerSeat == 1 && e.CardUseFrameId == use.Id) == (pay ? 0 : 1),
                "The later real gain arrives only after the original finalized choice; refusal preserves every declared target while marking just Liu Shan ineffective, and payment marks none.");
            if (!pay) s.Cold(); var useId = use.Id;
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
            s.Need(s.V(1).Hp == hp - (pay ? 1 : 0) && s.V(2).Hp == otherHp - (pay ? 0 : 1) && s.V(0).HandCount == 1 &&
                s.E<DamageAppliedEvent>().Count(e => e.SourceCardUseResolutionId == useId && e.SourceSeat == 0 && e.TargetSeat == (pay ? 1 : 2) && e.Amount == 1) == 1 &&
                s.E<DamageAppliedEvent>().All(e => e.SourceCardUseResolutionId != useId || e.TargetSeat == (pay ? 1 : 2)) &&
                s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId && e.CardId == slash) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                (!pay || s.G.CardMovements.Count(m => m.CardId == fee && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == CostReason) == 1),
                "Cold never repeats the price or physical Slash: the paid target takes one real damage; the unpaid multi-target use skips only Liu Shan and still damages its other actual target once.");
        }
    }

    private static string Peer(int seat) => "fixture:ordinary-mobile-liu-shan-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected real ordinary Liu Shan command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing actual ordinary Liu Shan choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 180; step++)
            {
                if (ready()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } choose)
                { Accept(new SelectGeneralCommand(choose.PlayerSeat, choose.PlayerSeat == 0 ? Actor : Peer(choose.PlayerSeat), G.Revision, choose.PromptId)); continue; }
                if (P is { Kind: DecisionKind.RespondDodge }) { Answer(c => c.Cards.Count == 0 && c.Parameters.GetValueOrDefault("response") == "take-damage"); continue; }
                Need(P is null, "Unexpected ordinary Liu Shan native boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded ordinary Liu Shan boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves exact original Slash targets, owner-only ineffective mark and once-paid basic entity in its native child.");
        }
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.Phase,
            Hands = Enumerable.Range(0, 4).Select(i => V(i).HandCount).ToArray(), Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
        public static Scene Start(bool halberd)
        {
            var stock = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:fire_slash", Suit.Heart, 7)).ToArray();
            if (halberd)
            {
                var probe = Boot(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock)));
                stock[probe.V(0).Hand[0].Id - 1] = new("classic:fangtian-halberd", Suit.Diamond, 12);
            }
            return Boot(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock)));
        }
        private static Scene Boot(ContentRegistry r)
        {
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, r), r);
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture(IReadOnlyList<ContentDeckPhysicalCard> stock) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-liu-shan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:liu-shan", "classic:liu-shan", "classic:liu-bei", "boundary:liu-bei", "boundary:liu-bei-mobile");
            b.AddCard(Halberd.Value);
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } };
            var definitions = new object[] {
                new { id = Late, revision = 1, triggers = new[] { new { id = "actual-later-target-effect-gain", window = "cardUseBeforeTargetEffects", ownerRelation = "actor",
                    cardKinds = new[] { "slash", "fireSlash", "thunderSlash", "coldSlash" }, optional = false, isSkillInvocation = false, usageScope = "game", usageLimit = 1,
                    effects = new object[] { new { op = "draw", target = "owner", amount = 1 }, Pause() } } } },
                new { id = Paid, revision = 1, triggers = new[] { new { id = "actual-basic-cost-child", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" },
                    movementReasons = new[] { CostReason }, movementOccurrence = "perBatch", optional = false, isSkillInvocation = false,
                    usageScope = "game", usageLimit = 1, effects = new[] { Pause() } } } }
            };
            var labels = new Dictionary<string, object> { [Late] = new { name = "真实后续目标效果收益", description = "指定目标后享乐必须先于此真实摸牌。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续原杀" } },
                [Paid] = new { name = "真实基本牌支付子窗", description = "原使用者弃基本牌已付后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "返回原享乐" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Actor, "真实杀使用者", "supporter", Late, "wei", 4, [Paid]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), seat == 1 ? "真实普通移动享乐" : "真实其他杀目标", "supporter", seat == 1 ? Skill : "standard:none", "shu", 8));
            b.AddDeck(new(Deck, "真实基本费用与杀实体", 2, 0, []) { PhysicalCards = stock });
            b.AddMode(new(Mode, "普通移动刘禅指定后享乐", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Actor, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
