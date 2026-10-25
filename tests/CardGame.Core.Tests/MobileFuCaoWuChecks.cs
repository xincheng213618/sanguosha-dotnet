using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileFuCaoWuChecks
{
    private const string Fu = "mobile:zhuikong", Cao = "mobile:sidi", Wu = "mobile:benxi", Driver = "fixture:mobile-fu-cao-wu-driver",
        Prime = "fixture:mobile-fu-cao-wu-prime", Watch = "fixture:mobile-fu-cao-wu-watch", Extra = "fixture:mobile-fu-cao-wu-extra",
        Owner = "fixture:mobile-fu-cao-wu-owner", Mode = "identity:classic-mobile-fu-cao-wu";

    public static void FuHuangHouActualTurnPindianPaidChildAndDirectedDistanceCold()
    {
        var s = Scene.Start("fu"); s.Play(0); s.End(0); s.Offer(Fu); s.Skip(); s.Play(1); s.End(1); s.Offer(Fu);
        var window = s.G.ResolutionStack.OfType<ForeignActualTurnStartWindowFrame>().Single();
        s.Need(window.OwnerSeat == 2 && window.ActualTurnNumber == s.G.State.TurnNumber && s.G.GetCombatDistance(2, 0) == 2,
            "Wounded ordinary Zhuikong offers at the real foreign turn boundary before that actor's preparation or Play, with a genuinely distant opponent.");
        s.Activate(); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
        var id = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Fu).Id;
        s.Answer(c => c.Targets.SequenceEqual([2]));
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "pindian-source-card") == true);
        var own = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single(); s.Answer(c => c.Cards.SequenceEqual([own]));
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "pindian-card") == true);
        var other = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single(); s.Answer(c => c.Cards.SequenceEqual([other]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var movementWindow = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last();
        var results = s.E<PindianResultDeterminedEvent>().Where(e => e.FrameId == movementWindow.Batch.ParentFrameId && e.SkillId == Fu).ToArray();
        s.Need(results.Length == 1, "The actual payment observer must name its original native Pindian result: " +
            JsonSerializer.Serialize(new { movementWindow.Batch.ParentFrameId, movementWindow.Batch.Movements,
                Results = s.E<PindianResultDeterminedEvent>().ToArray(), Frames = s.G.ResolutionStack.Select(f => new { f.Id, f.Kind }).ToArray(), OriginalProgram = id }));
        var result = results.Single().Result;
        s.Need(s.G.CardMovements.Count(m => m.CardId == own && m.Reason == CardMoveReasons.PindianReveal) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == other && m.Reason == CardMoveReasons.PindianReveal) == 1 &&
            s.Location(own) == CardLocation.DiscardPile && s.Location(other) == CardLocation.DiscardPile &&
            result.SourceSeat == 0 && result.OpponentSeat == 2 && result.SourceCardId == own && result.OpponentCardId == other && !result.SourceWon &&
            movementWindow.Batch.Movements.Any(m => m.CardId == own && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing &&
                m.Reason == CardMoveReasons.PindianReveal && s.G.CardMovements.Contains(m)) &&
            !s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == id) &&
            !s.E<ForeignTurnContestOriginFrozenEvent>().Any(e => e.Origin.ProgramFrameId == id) &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == id && e.SkillId == Fu && e.Activated && e.Completed) == 1,
            "The real movement observer belongs to the paid native Pindian frame: both exact entities paid once, the ordinary typed result already completed the original Zhuikong binding, and no specialized foreign-contest receipt was forged.");
        s.Cold(); s.Continue(); s.Play(2);
        var grant = s.E<DirectedTurnCardPolicyGrantedEvent>().Single(e => e.Policy.Source.SkillId == Fu).Policy;
        s.Need(grant.ParentFrameId == id && grant.ActorSeat == 2 && grant.TargetSeat == 0 && grant.Effects == DirectedTurnCardPolicyEffect.IgnoreDistance &&
            s.G.GetCombatDistance(2, 0) == 1 && s.G.GetCombatDistance(0, 2) == 2 &&
            s.G.CardMovements.Count(m => m.CardId == own && m.Reason == CardMoveReasons.PindianReveal) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == other && m.Reason == CardMoveReasons.PindianReveal) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Fu && e.Activated && e.Completed) == 1,
            "A tied real contest takes the not-won branch once: only the turn actor's final distance to Fu becomes one, and cold does not repay either card.");
        s.End(2); s.Offer(Fu); s.Skip(); s.Play(3);
        s.Need(s.G.GetCombatDistance(2, 0) == 2, "The original directed distance expires with that actual turn.");
    }

    public static void CaoZhenDodgeTopPileAndActualPlayQuotaPaidCold()
    {
        var s = Scene.Start("cao"); s.Play(0);
        s.Use(0, "virtual-slash", [1]); s.Reach(() => s.P?.Kind == DecisionKind.RespondDodge && s.P.PlayerSeat == 1);
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "dodge"); s.Offer(Cao); s.Activate(); s.Play(0);
        var qian = s.G.CreateCardZoneDiagnostics().Single(c => c.Location == CardLocation.Authority(0)).CardId;
        s.Need(s.G.CardMovements.Count(m => m.CardId == qian && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == qian && m.From == CardLocation.Processing && m.To == CardLocation.Authority(0)) == 1,
            "Another actor's real Dodge during Cao's actual turn stores exactly one real top entity in the persistent Qian pile.");
        s.End(0); s.Offer(Cao); s.Activate();
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-source-card") == true);
        s.Answer(c => c.Cards.SequenceEqual([qian])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var paid = s.E<MobileCaoZhenSidiAppliedEvent>().Single();
        s.Need(paid.AffectedSeat == 1 && paid.PaidCardId == qian && s.Location(qian) == CardLocation.DiscardPile &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Cao).Id == paid.ProgramFrameId &&
            s.G.CardMovements.Count(m => m.CardId == qian && m.Reason.Value == "skill.mobile-cao-zhen.sidi.cost") == 1,
            "The other actor's real inserted Play pays the selected original Qian and freezes its exact phase before the native movement child.");
        s.Cold(); s.Continue(); s.Play(1);
        s.Need(!s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "slash" && c.Cards.Count == 1),
            "The actual first Play's signed minus-one quota removes otherwise available native converted Slashes.");
        s.End(1); s.Play(1);
        s.Need(s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "slash" && c.Cards.Count == 1) &&
            s.G.CardMovements.Count(m => m.CardId == qian && m.Reason.Value == "skill.mobile-cao-zhen.sidi.cost") == 1 &&
            s.E<MobileCaoZhenSidiAppliedEvent>().Count() == 1,
            "The next natural Play of the same actual actor and turn regains its ordinary Slash allowance; cold never pays another Qian or extends the old phase.");
    }

    public static void WuYiFixedArmorAndOneExtraNativeSlashTargetCold()
    {
        var s = Scene.Start("wu"); s.Play(0); var firstHp = s.V(1).Hp; var secondHp = s.V(2).Hp;
        var action = s.G.GetHumanLegalActions().First(a => a.CardId is not null && a.PlayedCardKind == CardKind.Slash && a.TargetSeats.SequenceEqual([1]));
        var card = action.CardId!.Value;
        s.Accept(new PlayCardCommand(0, card, action.TargetSeats, s.G.Revision, s.P!.PromptId, action.PlayedCardKind, action.TargetCardId)
            { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Wu && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("enhancement-option") == nameof(CurrentCardEnhancement.ExtraTarget)));
        var frame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Wu);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == frame.CardEnhancementDraft!.CardUseFrameId); var id = use.Id;
        s.Need(s.G.GetCombatDistance(0, 2) == 1 && use.IgnoresArmor && use.Enhancements.HasFlag(CurrentCardEnhancement.IgnoreArmor) &&
            frame.CardEnhancementDraft!.Selected.SequenceEqual([CurrentCardEnhancement.IgnoreArmor]) &&
            s.P!.Choices.All(c => c.Parameters.GetValueOrDefault("enhancement-option") is nameof(CurrentCardEnhancement.ExtraTarget) or "finish"),
            "The first real in-turn use shortens distance before qualification, commits the mandatory armor benefit, and offers only the authentic optional extra Slash target.");
        s.Answer(c => c.Parameters.GetValueOrDefault("enhancement-option") == nameof(CurrentCardEnhancement.ExtraTarget));
        s.Answer(c => c.Targets.SequenceEqual([2])); s.Play(0);
        s.Need(s.PaidColdCount == 1 && s.V(1).Hp == firstHp - 1 && s.V(2).Hp == secondHp - 1 &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceCardUseResolutionId == id && e.Amount == 1) == 2 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == id) == 1 &&
            s.E<CurrentCardEnhancedEvent>().Count(e => e.CardUseFrameId == id && e.Enhancement == CurrentCardEnhancement.IgnoreArmor) == 1 &&
            s.E<CurrentCardEnhancedEvent>().Count(e => e.CardUseFrameId == id && e.Enhancement == CurrentCardEnhancement.ExtraTarget && e.ExtraTargetSeat == 2) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1,
            "The original paid Slash survives its real payment child cold and resolves two actual targets exactly once, without choosing unrelated classic Benxi benefits.");
        s.End(0); s.Play(1); s.Need(s.G.GetCombatDistance(0, 2) == 2, "The committed turn-distance reduction ends with that actual turn.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry, string scenario)
    {
        public GameEngine G = game;
        public int PaidColdCount;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public CardLocation Location(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, p.SkillPrompt } : null })); }
        public void Accept(GameCommand command) { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile command."); }
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Skip() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Offer(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Use(int seat, string action, int[] targets) => Accept(new UseProgramSkillCommand(seat, Driver, action, [], targets, G.Revision, P!.PromptId));
        public void Reach(Func<bool> goal)
        {
            for (var i = 0; i < 200 && !goal(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p)
                { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-fu-cao-wu-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (scenario == "wu" && P?.SkillPrompt?.SkillId == Watch) { PaidColdCount++; Cold(); Continue(); continue; }
                if (P is { Kind: DecisionKind.RespondDodge }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage"); continue; }
                if (P is { } unexpected) throw new InvalidOperationException("Unexpected mobile native boundary: " + JsonSerializer.Serialize(new { unexpected.Kind, unexpected.PlayerSeat, unexpected.SkillPrompt, unexpected.Choices }));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(goal(), "Missing fixed native mobile boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Checkpoint JSON keeps original paid native frames, all views, causal events and physical movements unchanged.");
        }
        public static Scene Start(string scenario)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scenario));
            var game = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0,
                HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry);
            var scene = new Scene(game, registry, scenario); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(string scenario) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-fu-cao-wu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:fu-huang-hou", "classic:fu-huanghou", "mobile:cao-zhen", "mobile:wu-yi");
            object[] skills = [
                new { id = Prime, revision = 1, triggers = new[] { new { id = "wound", window = "gameStarting", subject = "owner", optional = false,
                    effects = new[] { new { op = "loseHp", target = "owner", amount = 1 } } } } },
                new { id = Extra, revision = 1, triggers = new[] { new { id = "actual-extra-play", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false,
                    usageScope = "game", usageLimit = 1, effects = new[] { new { op = "insertPhase", target = "owner", phase = "play", phaseContinuation = "beforeNormalPreparation" } } } } },
                new { id = Driver, revision = 1,
                    viewAs = new[] { new { id = "dodge-as-native-slash", sourceZones = new[] { "hand" }, inputKinds = new[] { "dodge" }, inputSuits = Array.Empty<string>(), outputKind = "slash", forPlay = true, forResponse = false } },
                    activations = new[] { new { id = "virtual-slash", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "useVirtualCard", target = "selectedTarget", outputKind = "slash", targetRestriction = "distanceUnlimitedAgainstTarget" } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "original-payment-child", window = "cardsMoved", subject = "owner",
                    sourceZones = new[] { scenario == "cao" ? "authority" : "hand" }, movementOccurrence = "perBatch", optional = false,
                    movementReasons = new[] { scenario == "cao" ? "skill.mobile-cao-zhen.sidi.cost" : scenario == "fu" ? CardMoveReasons.PindianReveal.Value : CardMoveReasons.Use.Value },
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "continued", options = new[] { new { id = "continue" } } } } } } }
            ];
            var labels = new Dictionary<string, object> { [Prime] = new { name = "原生开局受伤", description = "真实失去一点体力。" },
                [Extra] = new { name = "实际前置出牌阶段", description = "同回合同角色的独立Play。" },
                [Driver] = new { name = "原生杀驱动", description = "真实用牌与响应入口。" },
                [Watch] = new { name = "原付款移动孩子", description = "付款实体已离开原区。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            var main = scenario == "fu" ? Fu : scenario == "cao" ? Cao : Wu;
            b.AddGeneral(new(Owner, "本批移动版真实持有者", "supporter", main, "wei", 4, scenario == "fu" ? [Driver, Watch, Prime] : [Driver, Watch]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-fu-cao-wu-peer-" + i).ToArray();
            for (var i = 1; i < 5; i++) b.AddGeneral(new(peers[i - 1], "真实同桌", "supporter", Driver, "wu", 4, scenario == "cao" && i == 1 ? [Extra] : []));
            const string deck = "fixture:mobile-fu-cao-wu-deck";
            b.AddDeck(new(deck, "固定真实同点数库存", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "本批原生真实身份", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1,
                [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
