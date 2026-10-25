using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryZhangFeiMobileChecks
{
    private const string Hero = "boundary:zhang-fei-mobile", Paoxiao = "boundary:paoxiao-mobile", Liyong = "boundary:liyong-mobile",
        Owner = "fixture:mobile-zhang-fei-owner", Driver = "fixture:mobile-zhang-fei-driver", Paid = "fixture:mobile-zhang-fei-paid",
        Ordinary = "fixture:mobile-zhang-fei-ordinary", Locked = "fixture:mobile-zhang-fei-locked", Mode = "identity:classic-mobile-zhang-fei";

    public static void RealDodgeNextNativeUseTargetRightsPaidHpColdAndActualPhaseExpiry()
    {
        var s = Scene.Start(); s.Play();
        var first = s.V(0).Hand[0].Id;
        s.Need(s.G.GetLegalActions(0).All(a => a.Kind != LegalActionKind.Slash || a.CardId != first || !a.TargetSeats.Contains(2)),
            "The first real Slash has ordinary distance; it cannot count its own future declaration to unlock Paoxiao.");
        s.Slash(first, [1]); s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 });
        var firstUse = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Action?.ActorSeat == 0 && f.CardKind == CardKind.Slash);
        s.Answer(c => c.Cards.Count == 1); s.Play();
        var reserved = s.E<BoundaryLiyongReservedEvent>().Single().Reserve;
        s.Need(reserved.CancelledUseId == firstUse.Id && reserved.CancelledActionId == firstUse.Action!.ActionId && reserved.TargetSeat == 1 &&
            s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) == 1 && s.V(1).Hp == 5,
            "One actually paid Dodge cancels the first native Slash and reserves Liyong in that exact actual Play phase.");
        var second = s.V(0).Hand[0].Id; var hp = s.V(0).Hp;
        var peerOne = s.V(1).HandCount; var peerTwo = s.V(2).HandCount;
        var responseCount = s.E<CardRespondedEvent>().Count();
        var movesOne = s.G.CardMovements.Count(m => m.From == CardLocation.Hand(1));
        var movesTwo = s.G.CardMovements.Count(m => m.From == CardLocation.Hand(2));
        s.Slash(second, [1, 2]); s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.BoundaryLiyong is not null);
        var useId = use.Id; var actionId = use.Action!.ActionId;
        var penalty = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.BoundaryLiyongPenalty is not null);
        var payment = penalty.BoundaryLiyongPenalty!;
        s.Need(use.TargetSeats.SequenceEqual([1, 2]) && use.BoundaryLiyong!.Reserve == reserved &&
            use.FinalTargetSlashReceipts!.Where(r => r.SkillId == Liyong).OrderBy(r => r.TargetSeat).Select(r => r.TargetSeat).SequenceEqual([1, 2]) &&
            use.FinalTargetSlashReceipts.Where(r => r.SkillId == Liyong).All(r => r.CardUseFrameId == useId && r.ActionId == actionId && r.DamageBonus == 1) &&
            payment is { Paid: true, TargetSeat: 1 } && payment.CardUseFrameId == useId && payment.CardActionId == actionId &&
            s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any(f => f.Change.ParentFrameId == penalty.Id && f.ResumeFrameId == penalty.Id) &&
            s.V(1).Hp == 3 && s.V(0).Hp == hp - 1 && s.E<BoundaryLiyongPenaltyPaidEvent>().Count() == 1,
            "The next actual Use reaches the distant second target, issues both target-local +1 receipts and pays one native HP loss after the first surviving target; its genuine HP child still owns the original penalty frame.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid);
        s.Need(s.V(2).Hp == 3 && s.V(0).Hp == hp - 2 && s.E<BoundaryLiyongPenaltyPaidEvent>().Count() == 2,
            "Returning the already-paid first HP child permits exactly one independent payment for the second surviving direct target.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.E<CardRespondedEvent>().Count() == responseCount &&
            s.G.CardMovements.Count(m => m.From == CardLocation.Hand(1)) == movesOne && s.G.CardMovements.Count(m => m.From == CardLocation.Hand(2)) == movesTwo &&
            s.E<CurrentTurnNonLockedSkillSuppressionIssuedEvent>().Where(e => e.Suppression.Source.SkillId == Liyong).Select(e => e.Suppression.TargetSeat).Order().SequenceEqual([1, 2]) &&
            !s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Ordinary) && s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Locked) == 2 &&
            s.V(1).HandCount == peerOne + 1 && s.V(2).HandCount == peerTwo + 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId) == 1 && s.E<BoundaryLiyongUseIssuedEvent>().Count() == 1 &&
            s.G.CardMovements.Count(m => m.CardId == second && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "Both targets retain their real Dodges without a response window, their ordinary damage triggers are suppressed while locked triggers still draw, and cold never repays either HP or physical Slash material.");

        var third = s.V(0).Hand[0].Id;
        s.Slash(third, [1]); s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 });
        s.Answer(c => c.Cards.Count == 1); s.Play();
        s.Need(s.E<BoundaryLiyongReservedEvent>().Count() == 2 && s.E<BoundaryLiyongUseIssuedEvent>().Count() == 1,
            "An ordinary later Slash can be Dodged and reserve a fresh entitlement without stacking the already-consumed first one.");
        var turn = s.G.State.TurnNumber;
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && s.G.State.TurnNumber > turn);
        var next = s.V(0).Hand[0].Id;
        s.Need(s.G.GetLegalActions(0).All(a => a.Kind != LegalActionKind.Slash || a.CardId != next || !a.TargetSeats.Contains(2)) &&
            s.E<CurrentTurnNonLockedSkillSuppressionsExpiredEvent>().Any(),
            "The actual next Play resets Paoxiao's first-Slash range and expires both issued target suppressions.");
        var beforeHp = s.V(0).Hp;
        s.Use("damage-peer", 2); s.Play();
        s.Need(s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Ordinary) == 1 && s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Locked) == 3 &&
            s.V(0).Hp == beforeHp && s.E<BoundaryLiyongPenaltyPaidEvent>().Count() == 2,
            "After the real turn boundary the same ordinary target skill works again; unrelated native skill damage neither consumes nor pays a Liyong Slash entitlement.");
        s.Slash(next, [1]); s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 });
        s.Need(s.E<BoundaryLiyongUseIssuedEvent>().Count() == 1 && s.G.ResolutionStack.OfType<CardUseFrame>().All(f => f.BoundaryLiyong is null),
            "The prior phase's unused reservation cannot empower the first Slash in a new actual Play phase.");
        s.Answer(c => c.Cards.Count == 1); s.Play();
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Slash(int material, int[] targets)
        {
            var action = G.GetLegalActions(0).First(a => a.Kind == LegalActionKind.Slash && a.CardId == material && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Zhang Fei command."); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 240; step++)
            {
                if (goal()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } choose) { Accept(new SelectGeneralCommand(choose.PlayerSeat, choose.PlayerSeat == 0 ? Owner : Peer(choose.PlayerSeat), G.Revision, choose.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                Need(P is not { Kind: DecisionKind.ProgramTrigger or DecisionKind.RespondDodge or DecisionKind.PlayCard }, "Unexpected native Zhang Fei prompt.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Mobile Zhang Fei missed its bounded native boundary.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold retains original Use, target-local rights, already-paid penalty and the real HP child.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { Pending = P?.Kind, Seat = P?.PlayerSeat, Skill = P?.SkillPrompt?.SkillId, G.State.CurrentSeat, G.State.Phase, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var game = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var scene = new Scene(registry, game); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-zhang-fei-peer-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-zhang-fei-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            object DamageDraw(string id) => new { id, revision = 1, triggers = new[] { new { id = "draw-after-real-damage", window = "afterDamageApplied", subject = "owner", damageOccurrence = "perDamage", optional = false,
                effects = new[] { new { op = "draw", target = "owner", amount = 1 } } } } };
            var skills = new object[] {
                new { id = Driver, revision = 1,
                    activations = new[] { new { id = "damage-peer", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "damage", target = "selectedTarget", amount = 1 } } } },
                    viewAs = new[] { new { id = "real-heart-slash", inputKinds = Array.Empty<string>(), inputSuits = new[] { "heart" }, outputKind = "slash", forPlay = true, forResponse = false } },
                    modifiers = new[] { new { id = "one-extra-native-target", query = "cardTargetCount", operation = "add", value = 1, cardKinds = new[] { "slash", "fireSlash", "thunderSlash" }, priority = 0 } } },
                new { id = Paid, revision = 1, triggers = new[] { new { id = "real-hp-child", window = "afterHealthChanged", subject = "owner", optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } } } },
                DamageDraw(Ordinary), DamageDraw(Locked)
            };
            var labels = new Dictionary<string, object> {
                [Driver] = new { name = "真实红牌杀与伤害", description = "真实实体付款、原生双目标和一伤害驱动" },
                [Paid] = new { name = "真实失体力子窗", description = "实际付款后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                [Ordinary] = new { name = "非锁定受伤摸牌", description = "实际受伤后摸一张" }, [Locked] = new { name = "锁定受伤摸牌", description = "实际受伤后摸一张" }
            };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id], Tags = id == Locked ? SkillTag.Locked : SkillTag.None });
            builder.AddGeneral(new(Owner, "移动界张飞原生场景", "supporter", Paoxiao, "shu", 4, AdditionalSkillIds: [Liyong, Driver, Paid]));
            var peers = Enumerable.Range(1, 4).Select(Peer).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "原生真实目标", "supporter", Ordinary, "wu", 5, AdditionalSkillIds: [Locked]));
            const string deck = "fixture:mobile-zhang-fei-deck";
            builder.AddDeck(new(deck, "固定真实红色闪实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 13)).ToArray() });
            builder.AddMode(new(Mode, "移动界张飞原生流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
