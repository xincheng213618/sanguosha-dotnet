using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
namespace CardGame.Core.Tests;

internal static class ShenCaoPiChecks
{
    private const string Hero = "anniversary:shen-cao-pi", Driver = "fixture:shen-cp-driver", Pulse = "fixture:shen-cp-pulse",
        Probe = "fixture:shen-cp-probe", Quiet = "fixture:shen-cp-quiet", NoDraw = "fixture:shen-cp-no-draw", Watch = "fixture:shen-cp-watch",
        Ally = "fixture:shen-cp-ally", Enemy = "fixture:shen-cp-enemy", Cao = "fixture:shen-cp-cao", Deck = "fixture:shen-cp-deck", PlayHold = "fixture:shen-cp-play-hold";
    public static void YinzhiPreludeExemptionRestoresAfterCancellationAndEndsAfterRealStart()
    {
        var s = Scene.Start(); s.Play();
        s.Use("anniversary:yinzhi", "edict", [2]); s.Play();
        var issue = s.E<YinzhiIssuedEvent>().Single();
        var player = s.Player(2);
        var restriction = player.SkillGrants.Restrictions.Single(r => r.Lifetime.Origin.FrameId == issue.FrameId);
        bool Effective() => s.G.NationalSkillsEntries.HasRuntimeSkill(player, Probe);
        s.Need(!Effective() && s.G.NationalSkillsEntries.HasOwnedRuntimeSkill(player, Probe) &&
            restriction.Match.StructurallyEquals(SkillStateMatch.NonLocked) && s.G.ResolutionStack.Count == 0,
            "The real paid Yinzhi continuation issues a non-locked restriction before its target's next actual turn.");
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(s.G)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var round = s.G.Events.LastOrDefault(e => e.Payload is RoundStartedEvent);
        const long preludeId = 900397001;
        var proof = new BeiMiHuBeforeTurnProof(preludeId, s.G.State.TurnNumber + 1,
            (round?.Payload as RoundStartedEvent)?.RoundNumber ?? 0, 2, round?.Sequence ?? 0);
        frames.Push(new BeiMiHuTurnPreludeFrame(preludeId, proof, false));
        try
        {
            s.Need(Effective() && player.SkillGrants.Restrictions.Contains(restriction),
                "The real root prelude frame temporarily exempts its upcoming owner without expiring the paid restriction.");
            frames.Replace(preludeId, new BeiMiHuTurnPreludeFrame(preludeId, proof with { UpcomingOwnerSeat = 1 }, false));
            s.Need(!Effective(), "Another upcoming owner's prelude does not exempt the restricted target.");
        }
        finally { frames.Pop(preludeId, ResolutionFrameKind.BeiMiHuTurnPrelude); }
        s.Need(!Effective() && player.SkillGrants.Restrictions.Contains(restriction) &&
            !s.E<SkillStateExpiredEvent>().Any(e => e.SourceId == restriction.SourceId),
            "Cancelling the prelude restores suppression while preserving the same unexpired restriction.");
        var issuer = s.Player(0);
        var source = issuer.SkillGrants.Grants.Single(grant => grant.SkillId == "anniversary:yinzhi");
        issuer.SkillGrants.RemoveGrant(source.GrantId);
        try
        {
            s.Need(!Effective() && player.SkillGrants.Restrictions.Contains(restriction),
                "Losing the original issuing skill does not retract its already applied state.");
            issuer.IsAlive = false;
            s.Need(!Effective() && player.SkillGrants.Restrictions.Contains(restriction),
                "The issuer's death does not retract the target's restriction before its declared deadline.");
        }
        finally { issuer.IsAlive = true; issuer.SkillGrants.Grant(source); }
        s.End();
        s.Reach(() => s.E<TurnStartedEvent>().Any(e => e.ActorSeat == 2 && e.TurnNumber > issue.ActualTurn));
        var started = s.G.Events.Single(e => e.Payload is TurnStartedEvent turn && turn.ActorSeat == 2 && turn.TurnNumber > issue.ActualTurn);
        var expired = s.G.Events.Single(e => e.Payload is SkillStateExpiredEvent state && state.SourceId == restriction.SourceId);
        s.Need(expired.Sequence > started.Sequence && !player.SkillGrants.Restrictions.Contains(restriction) && Effective() &&
            s.E<YinzhiIssuedEvent>().Single() == issue,
            "Only the real target TurnStarted enters history before expiry; the skill restores and the reward/new-card obligation remains issued.");
    }

    public static void BorrowedDamageInvocationStaysWindowBoundThroughNativeDrawCold()
    {
        const string borrowedSkill = "boundary:jianxiong";
        var s = Scene.Start(borrowScenario: true, invocation: true); s.Play();
        s.Use(Driver, "opponent-duel", [2]);
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        s.Answer(choice => choice.Cards.Count == 0);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == "anniversary:keming");
        s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "keming-borrow") == true);
        s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "keming-borrow");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == borrowedSkill);
        s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw") == true);
        s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var frame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == borrowedSkill);
        var window = s.G.ResolutionStack.OfType<DamageTriggerWindowFrame>().Single(w => w.Id == frame.WindowContext!.ParentFrameId);
        var draw = s.G.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(f => f.Context.ParentFrameId == frame.Id);
        bool CanInvoke(ProgramSkillFrame value) => s.G.ProgramLifecycleEntries.CanInvokeProgramSkill(value);
        bool OwnedOrEffective()
        {
            var player = s.Player(0);
            return s.G.NationalSkillsEntries.HasOwnedRuntimeSkill(player, borrowedSkill) ||
                s.G.NationalSkillsEntries.HasRuntimeSkill(player, borrowedSkill) ||
                s.G.NationalSkillsEntries.HasRuntimeSkillInstance(player, borrowedSkill, frame.SkillInstanceId);
        }
        s.Need(!OwnedOrEffective() && CanInvoke(frame) && frame.InvocationAuthorization is { } authorization &&
            authorization.ProgramFrameId == frame.Id && authorization.WindowId == window.Id &&
            authorization.NativeFrameId == window.ParentFrameId &&
            authorization.Candidate == window.Candidates[window.CandidateIndex].ToProgramCandidate(),
            "A genuinely borrowed damaged trigger remains unowned and ineffective, while its original window authorizes the one suspended native draw.");
        foreach (var changed in new[]
        {
            frame with { Id = frame.Id + 1 }, frame with { TriggerId = frame.TriggerId + "-wrong" },
            frame with { SkillInstanceId = frame.SkillInstanceId + "-wrong" }, frame with { GameplayHash = frame.GameplayHash + "-wrong" },
            frame with { WindowContext = frame.WindowContext! with { ParentFrameId = window.Id + 1 } },
            frame with { WindowContext = frame.WindowContext! with { OccurrenceIndex = frame.WindowContext.OccurrenceIndex + 1 } }
        })
            s.Need(!CanInvoke(changed), "The invocation permission cannot be reused by another frame, binding, instance, definition, window or occurrence.");
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(s.G)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        frames.Replace(window.Id, window with { CandidateIndex = window.CandidateIndex + 1 });
        try { s.Need(!CanInvoke(frame), "Advancing the original window cursor immediately retires its previous candidate's invocation permission."); }
        finally { frames.Replace(window.Id, window); }
        var hand = s.V(0).HandCount;
        s.Cold();
        s.Need(!OwnedOrEffective() && CanInvoke(frame) && s.P?.SkillPrompt?.SkillId == Watch,
            "Cold recovery preserves invocation permission on the original pending frame without creating a skill grant.");
        s.Skip(); s.Play();
        s.Need(s.V(0).HandCount == hand + 1 && !OwnedOrEffective() && !CanInvoke(frame) &&
            s.E<NativeDrawAttemptStartedEvent>().Count(e => e.Context.ParentFrameId == frame.Id) == 1 &&
            s.E<ProgramDrawCompletedEvent>().Count(e => e.FrameId == frame.Id && e.ActualDrawCount == 1) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == frame.Id && e.Activated && e.Completed) == 1 &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 2 && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.E<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == draw.Id) == 1,
            "The paid damage and borrowed draw finish once after recovery, and the completed native window cannot authorize another invocation.");
    }

    public static void YinzhiZhangbaUsesOnlyPublishedCompleteNewMaterialPairCold()
    {
        var s = Scene.Start(spear: true); s.Play();
        var weapon = s.V(0).Hand.Single(card => card.Kind == CardKind.ZhangbaSerpentSpear).Id;
        s.Accept(new UseProgramSkillCommand(0, Driver, "arm-target", [weapon], [2], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Use("anniversary:yinzhi", "edict", [2]); s.Play(); s.End();
        s.Reach(() => s.P is { PlayerSeat: 2 } && s.P.SkillPrompt?.SkillId == PlayHold);
        var view = s.G.CreateSnapshot(2);
        var fresh = s.G.CardMovements.Where(move => move.To == CardLocation.Hand(2) &&
            move.From == CardLocation.DrawPile && move.TurnNumber == view.TurnNumber).Select(move => move.CardId).ToHashSet();
        var action = s.Actions(2).Single(option => option.Kind == LegalActionKind.UseEquipmentEffect &&
            option.EquipmentKind == CardKind.ZhangbaSerpentSpear);
        var oldChoice = new SimpleAiBrain(2, 31, 1).ChooseActiveSkillCards(view, action with
        { SelectableCardGroupTargets = null, SelectableCardIds = view.Players[2].Hand.Select(card => card.Id).ToArray() });
        s.Need(fresh.Count == 2 && oldChoice.Count == 2 && oldChoice.All(id => !fresh.Contains(id)) &&
            s.V(2).Equipment.Any(card => card.Id == weapon) &&
            action.SelectableCardGroupTargets is { Count: > 0 } groups &&
            groups.All(group => group.CardIds.Count == 2 && group.CardIds.All(fresh.Contains)),
            "The real Yinzhi own-turn restriction leaves two cheaper old Hand cards unusable, while the published native Spear pairs contain only genuinely newly gained materials.");
        foreach (var policy in new[] { 1, 10, 11 })
        {
            var brain = new SimpleAiBrain(2, 31, policy);
            var ids = brain.ChooseActiveSkillCards(view, action);
            var targets = brain.ChooseActiveSkillTargets(view, action, ids);
            s.Need(ids.Count == 2 && ids.All(fresh.Contains) && targets.Count == 1 &&
                action.SelectableCardGroupTargets!.Any(group => group.TargetSeat == targets[0] &&
                    group.CardIds.Order().SequenceEqual(ids.Order())),
                "Every supported comparison policy selects an actual published complete material group and its compatible target.");
        }
        var mutable = fresh.ToList();
        var projected = new HumanActionView(s.G.Revision,
            [action with { SelectableCardGroupTargets = [new(mutable, 0)] }], []);
        mutable.Clear();
        var frozen = projected.LegalActions.Single().SelectableCardGroupTargets!;
        var rejected = false;
        try { ((IList<int>)frozen[0].CardIds).Clear(); } catch (NotSupportedException) { rejected = true; }
        s.Need(frozen.Count == 1 && frozen[0].CardIds.Count == 2 && rejected,
            "The public action projection copies and freezes the nested complete material group.");
        s.Cold(); s.Answer(_ => true);
        s.Reach(() => s.E<ZhangbaSerpentSpearConvertedEvent>().Any(converted => converted.IsUse));
        var issued = s.E<ZhangbaSerpentSpearConvertedEvent>().Single(converted => converted.IsUse);
        s.Need(issued.UserSeat == 2 && issued.PhysicalCardIds.Count == 2 && issued.PhysicalCardIds.All(fresh.Contains),
            "The real AI step issues one native Spear Use from its legal newly gained pair instead of retrying the illegal cheaper old pair.");
        s.Cold();
        if (s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge })
            s.Answer(choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        s.Reach(() => s.E<CardUseFinishedEvent>().Any(done => done.ResolutionId == issued.ResolutionId));
        s.Need(s.E<CardUseFinishedEvent>().Count(done => done.ResolutionId == issued.ResolutionId) == 1 &&
            issued.PhysicalCardIds.All(id => s.G.CardMovements.Count(move => move.CardId == id &&
                move.From == CardLocation.Hand(2) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1) &&
            s.V(2).Equipment.Any(card => card.Id == weapon),
            "Cold native resolution pays both real materials once, finishes once and retains the genuinely equipped source weapon.");
        s.Cold(); s.Replay();
    }
    private sealed class Fixture(bool nativeEquipment = false, bool aiEcho = false, bool spear = false, bool invocation = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shen-cp", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, invocation ? [Hero, "boundary:cao-cao"] : [Hero]);
            b.AddCard(new("fixture:shen-cp-cixiong", "雌雄双股剑", "装备牌", "攻击范围2，使用杀指定异性角色为目标后，其弃一张手牌或令你摸一张牌。", CardKind.CixiongDoubleSwords));
            if (spear)
            {
                b.AddCard(new("fixture:shen-cp-spear", "丈八蛇矛", "装备牌", "真实两张手牌转换", CardKind.ZhangbaSerpentSpear));
                b.AddCard(new("fixture:shen-cp-crossbow", "诸葛连弩", "装备牌", "真实旧手牌", CardKind.Crossbow));
            }
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":20,"priority":0},{"id":"range","query":"attackRange","operation":"add","value":10,"priority":0}],"activations":[
                {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                {"id":"arm-target","minCards":1,"maxCards":1,"sourceZones":["hand"],"cardCategories":["equipment"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"cost"},{"op":"placeSelectedEquipment","target":"selectedTarget","sourceBind":"cost"}]}]},
              {"id":"{{Pulse}}","revision":1,"triggers":[{"id":"draw-six","window":"playPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":6}]}]},
              {"id":"{{Probe}}","revision":1,"modifiers":[{"id":"empty","query":"handLimit","operation":"set","value":0,"priority":0},{"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},{"id":"unlimited","query":"slashLimit","operation":"unlimited","value":0,"priority":0}]},
              {"id":"{{NoDraw}}","revision":1,"triggers":[{"id":"no-draw","isSkillInvocation":false,"window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","isSkillInvocation":false,"window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{PlayHold}}","revision":1,"triggers":[{"id":"hold","isSkillInvocation":false,"window":"playPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"hold","options":[{"id":"continue"}]}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"watch","isSkillInvocation":false,"window":"nativeDrawStarting","subject":"owner","optional":true,"effects":[{"op":"setChainedState","target":"owner","chained":true}]}]}]}
            """;
            var labels = new[] { Driver, Pulse, Probe, Quiet, NoDraw, Watch, PlayHold }.ToDictionary(id => id, id => new { name = id, description = "神曹丕真实原生小局", optionLabels = id == PlayHold ? new Dictionary<string, string> { ["continue"] = "继续原生出牌阶段" } : new Dictionary<string, string>() });
            if (aiEcho || invocation)
            {
                var definition = JsonNode.Parse(rules)!;
                var actions = definition["skills"]![0]!["activations"]!.AsArray();
                actions.Add(JsonNode.Parse("""{"id":"opponent-duel","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"usesPerPhase":2,"effects":[{"op":"useSelectedActorDuel","target":"owner"}]}"""));
                actions.Add(JsonNode.Parse("""{"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":2}]}"""));
                rules = definition.ToJsonString();
            }
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id], Tags = id is NoDraw or Quiet or PlayHold ? SkillTag.Locked : SkillTag.None });
            b.AddGeneral(new(Ally, "实际技能来源", "", nativeEquipment ? Probe : Pulse, "wei", 12, Gender: GeneralGender.Male));
            b.AddGeneral(new(Cao, "原生奸雄来源", "", invocation ? "boundary:jianxiong" : "standard:jianxiong", "wei", 12, [Probe, PlayHold], nativeEquipment ? GeneralGender.Female : GeneralGender.Male) { CharacterId = "character:cao-cao", RulesetId = "sanguosha-anniversary" });
            b.AddGeneral(new(Enemy, "安静同桌", "", Quiet, "wei", 12, Gender: GeneralGender.Female));
            b.AddDeck(new(Deck, "固定同名实体牌堆", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard(spear ? "standard:dodge" : "standard:slash", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "神曹丕真实小局", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Ally, Cao, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(v => G.CreateSnapshot(v).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private System.Collections.IList Players => (System.Collections.IList)global::FixtureRuntimeAccess.Engine.GetField("_players", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(G)!;
        public CharacterState Player(int seat) => (CharacterState)Players[seat]!;
        public int Limit(int seat) => (int)global::FixtureRuntimeAccess.Engine.GetMethod("GetHandLimit", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(G, [Players[seat]])!;
        public IReadOnlyList<LegalAction> Actions(int seat) => (IReadOnlyList<LegalAction>)global::FixtureRuntimeAccess.Engine.GetMethod("BuildLegalActions", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(G, [Players[seat], true, null])!;
        public void Use(string skill, string activation, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, skill, activation, [], targets, G.Revision, P!.PromptId));
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Skip() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 240; i++)
            {
                if (done()) return;
                if (P?.SkillPrompt?.SkillId == Watch) Skip();
                else if (P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) End();
                else if (P is { PlayerSeat: 0, Kind: DecisionKind.ProgramTrigger } && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Skip();
                else if (P is null || P.PlayerSeat != 0) Accept(new AdvanceOneStepCommand(G.Revision));
                else throw new InvalidOperationException("Unexpected Shen Cao Pi boundary " + JsonSerializer.Serialize(P));
            }
            throw new InvalidOperationException("Bounded Shen Cao Pi occurrence not reached " + JsonSerializer.Serialize(P));
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(v => SnapshotJson.Serialize(G.CreateSnapshot(v))).ToArray(), Frames = G.ResolutionStack, Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Moves = G.CardMovements });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold restoration preserves the original source skill, borrowed candidate and private draw names."); }
        public void Replay() { var before = State(); G = GameReplay.Replay(G.CreateCheckpoint().Options, G.AcceptedCommands, registry); Need(before == State(), "Accepted-command replay preserves original damage, reserved invocation and matching entity payments."); }
        public void Accept(GameCommand c) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected command."); }
        public void Need(bool ok, string reason) { if (!ok) throw new InvalidOperationException(reason + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, P, Facts = G.Events.TakeLast(8).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) })); }
        public static Scene Start(bool nativeEquipment = false, bool borrowScenario = false, bool aiEcho = false, bool spear = false, bool invocation = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(nativeEquipment, aiEcho, spear, invocation));
            var setup = new ShanheBattleSetup(37, 1, "anniversary-cp", aiEcho ? Ally : Hero, 4, 0, 0, 0, 0, [Driver, NoDraw, Watch],
                spear ? [new(CardKind.ZhangbaSerpentSpear, Suit.Spade, 12)] : nativeEquipment ? [new(CardKind.CixiongDoubleSwords, Suit.Spade, 2)] : [new(CardKind.Slash, Suit.Heart, 8), new(CardKind.Duel, Suit.Spade, 1)], [],
                [new(Cao, Hp: borrowScenario ? 1 : null) { StartingHand = spear ? [new(CardKind.Crossbow, Suit.Spade, 1), new(CardKind.Crossbow, Suit.Club, 2)] : nativeEquipment ? [] : [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Slash, Suit.Spade, 3)] }, new(Enemy) { StartingHand = [] }],
                [new(aiEcho ? Hero : Ally, ExtraSkillIds: [NoDraw]) { StartingHand = nativeEquipment ? [new(CardKind.Slash, Suit.Spade, 7)] : [] }], new string('e', 64)) { DefaultGodFactionId = "wei" };
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
}
