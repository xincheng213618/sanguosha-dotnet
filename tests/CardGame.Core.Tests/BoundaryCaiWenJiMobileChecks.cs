using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryCaiWenJiMobileChecks
{
    private const string Hero = "boundary:cai-wen-ji-mobile", Beige = "boundary:beige-mobile", Duanchang = "boundary:duanchang-mobile",
        Peer = "fixture:mobile-cwj-peer", Quiet = "fixture:mobile-cwj-quiet", Driver = "fixture:mobile-cwj-driver",
        Paid = "fixture:mobile-cwj-paid", Final = "fixture:mobile-cwj-final", Recovery = "fixture:mobile-cwj-recovery";
    private const string Reason = "skill.boundary.beige-mobile", Payment = "skill-program.boundary:beige-mobile.SelectAndMoveOwnedCard",
        SourceDiscard = "skill-program.boundary:beige-mobile.MoveBoundCards";

    public static void PayBeforeJudgmentFourSuitsActualDamageRecoveryAndCold()
    {
        RejectRecoveryOutsideAfterDamage();
        RejectMismatchedJudgmentDiscard();
        foreach (var suit in new[] { Suit.Heart, Suit.Diamond, Suit.Club, Suit.Spade })
        {
            var s = Scene.Start(suit); s.Play(); var cost = s.V().Equipment.Single().Id;
            s.Native(s.V().Hand.Single(c => c.Kind == CardKind.Alcohol).Id, 0); s.Play();
            s.Native(s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id, 3); s.Offer();
            s.Need(s.V(3).Hp == 6 && s.Facts<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == 3 && e.Amount == 2) &&
                s.Judgments().Length == 0 && s.Zone(cost) == CardLocation.Equipment(0),
                "A real alcohol-boosted Slash deals two damage before the optional mobile Beige offer; neither payment nor judgment has happened.");
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Reach(() => s.P?.Choices.Any(c => c.Cards.SequenceEqual([cost])) == true); s.Answer(c => c.Cards.SequenceEqual([cost]));
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid);
            s.Need(s.Root is { InstructionIndex: 1, PendingMovementContinuation: not null } paid && paid.WindowContext is { Amount: 2, TargetSeat: 3, SourceSeat: 0 } &&
                s.Zone(cost) == CardLocation.DiscardPile && s.Judgments().Length == 0 && s.V(3).Hp == 6,
                "The original after-damage frame pays one real equipment card and awaits its movement child before any judgment.");
            if (suit == Suit.Heart) s.Cold(); s.Continue();
            s.Reach(() => s.P?.Choices.Any(c => c.Targets.SequenceEqual([3])) == true);
            s.Answer(c => c.Targets.SequenceEqual([3]));
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Final);
            s.Need(s.Judgments() is [{ TargetSeat: 3 }] && s.V(3).Hp == 6 && s.V(3).Hand.Count == 0 && !s.V().IsFaceDown &&
                s.G.CardMovements.Count(m => m.Reason.Value == Payment) == 1,
                "Only the paid frame's injured target makes the real judgment; no suit benefit overtakes its native finalized-judgment child.");
            if (suit == Suit.Heart) s.Cold(); s.Continue();
            if (suit == Suit.Heart)
            {
                s.Reach(() => s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any(h => h.Change.Kind == HpChangeKind.Recovery && h.Change.TargetSeat == 3));
                s.Need(s.V(3).Hp == 8 && s.Root?.WindowContext?.Amount == 2 && s.Facts<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 3 && e.Amount == 2),
                    "Heart recovers the frozen two actual damage points through the original native recovery child, rather than a fixed one or the victim's total lost HP.");
                s.Cold();
            }
            else if (suit == Suit.Club)
            {
                var cards = s.V().Hand.Take(2).Select(c => c.Id).ToArray();
                s.Reach(() => s.P?.Choices.Any(c => c.Cards.SequenceEqual([cards[0]])) == true);
                s.Answer(c => c.Cards.SequenceEqual([cards[0]]));
                s.Need(cards.All(id => s.Zone(id) == CardLocation.Hand(0)) && s.G.CardMovements.Count(m => m.Reason.Value == Payment) == 1,
                    "The damage source stages its first of two HE discards without prematurely moving or charging a single card.");
                s.Answer(c => c.Cards.SequenceEqual([cards[1]]));
            }
            s.Play();
            var result = s.Facts<JudgmentResolvedEvent>().Single(e => e.Reason == Reason);
            s.Need(result.TargetSeat == 3 && result.Suit == suit && s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile) == 1 &&
                s.Zone(result.CardId!.Value) == CardLocation.DiscardPile && s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Beige && e.Activated && e.Completed) == 1,
                "The native final suit returns to the original paid frame, cleans its own judgment card and completes once without a second initial cost.");
            s.Need(suit switch
            {
                Suit.Heart => s.V(3).Hp == 8 && s.Facts<ProgramRecoveryAppliedEvent>().Where(e => e.ProgramFrameId == s.BeigeFrameId).ToArray() is
                    [{ Receipt: { RequestedAmount: 2, SourceSeat: 0, TargetSeat: 3, HpBefore: 6, HpAfter: 8 } }],
                Suit.Diamond => s.V(3).Hand.Count == 3 && s.Facts<CardsDrawnEvent>().Where(e => e.PlayerSeat == 3).Sum(e => e.Count) == 3,
                Suit.Club => s.G.CardMovements.Count(m => m.Reason.Value == SourceDiscard && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 2,
                Suit.Spade => s.V().IsFaceDown && !s.V(3).IsFaceDown,
                _ => false
            }, "All four mobile results use the actual injured target or damage source: recover X, draw three, discard two, or source turnover.");
        }
    }

    public static void DuanchangLosesAllGeneralSkillsAndPreservesEquipment()
    {
        var s = Scene.Start(Suit.Club, death: true); s.Play(); s.Drive("acquire"); s.Play();
        var generalSkills = new[] { Quiet, Recovery, Driver, "classic:wusheng", "classic:paoxiao" };
        s.Need(generalSkills.All(id => s.V().Skills!.Any(k => k.ContentId == id)),
            "The real killer begins with ordinary, locked and genuinely acquired general skills before Duanchang.");
        s.Native(s.V().Hand.First(c => c.Kind == CardKind.Slash).Id, 3); s.Play();
        var lost = s.Facts<CharacterSkillsLostEvent>().Single(e => e.Seat == 0);
        s.Need(!s.V(3).IsAlive && lost.SourceSeat == 3 && generalSkills.All(lost.SkillIds.Contains) &&
            s.V().Skills!.All(k => !generalSkills.Contains(k.ContentId)) && s.V().Equipment is [{ Kind: CardKind.Crossbow }] &&
            s.Judgments().Length == 0 && s.Facts<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Duanchang && e.Completed),
            "The real dead mobile Cai Wen Ji invokes native Duanchang: all current general skills, including acquired and locked skills, are lost while her killer's actual weapon survives.");
        s.Cold();
        var next = s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id;
        s.Need(s.G.GetHumanLegalActions().Any(a => a.CardId == next && a.Kind == LegalActionKind.Slash),
            "Cold preserves complete general-skill loss while the surviving Crossbow still grants its real equipment Slash allowance.");
    }

    private static void RejectRecoveryOutsideAfterDamage()
    {
        const string id = "fixture:mobile-cwj-invalid-recovery";
        var json = JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[] { new { id, revision = 1, triggers = new[] { new
            {
                id = "wrong-window", window = "judgmentFinalized", subject = "owner", judgmentReasons = new[] { "fixture:judgment" },
                suits = new[] { "heart" }, excludedReasons = Array.Empty<string>(), minimumRank = 1, maximumRank = 13, optional = false,
                effects = new[] { new { op = "recover", target = "owner", targetRef = new { kind = "eventTarget" }, numberExpression = "afterDamageAmount" } }
            } } } }
        });
        var presentation = JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = "静态窗口合同", description = "判定事件不能冒充原伤害值。" } }
        });
        try { SkillProgramCatalog.Load(json, presentation); }
        catch (InvalidOperationException e) when (e.Message.Contains("AfterDamageApplied", StringComparison.OrdinalIgnoreCase)) { return; }
        throw new InvalidOperationException("The loader must reject afterDamageAmount in a non-damage window before execution.");
    }

    private static void RejectMismatchedJudgmentDiscard()
    {
        const string id = "fixture:mobile-cwj-guarded-discard";
        foreach (var consumerSuit in new string?[] { "spade", null })
        {
            var move = new Dictionary<string, object>
            { ["op"] = "moveBoundCards", ["target"] = "owner", ["sourceBind"] = "selected", ["destination"] = "discardPile" };
            if (consumerSuit is not null) move["condition"] = new { kind = "judgmentOutcomeMatchesSuits", sourceBind = "outcome", suits = new[] { consumerSuit } };
            object[] effects =
            [
                new { op = "selectTarget", target = "owner", targetKind = "eventTarget" },
                new { op = "startJudgment", target = "selectedTarget", judgmentReason = "fixture:judgment", resultBind = "card", outcomeBind = "outcome", visibility = "public" },
                new { op = "selectOwnedCards", target = "owner", targetRef = new { kind = "eventSource" }, zones = new[] { "hand", "equipment" }, amount = 2, resultBind = "selected",
                    condition = new { kind = "judgmentOutcomeMatchesSuits", sourceBind = "outcome", suits = new[] { "club" } } },
                move
            ];
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { new
            { id, revision = 1, triggers = new[] { new { id = "guard", window = "afterDamageApplied", subject = "any", damageOccurrence = "perDamage", optional = false, effects } } } } });
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [id] = new { name = "判定分支合同", description = "只能消费同一判定分支创建的本人弃牌绑定。" } } });
            try { SkillProgramCatalog.Load(rules, presentation); }
            catch (InvalidOperationException e) when (e.Message.Contains("matching judgment outcome branch", StringComparison.Ordinal)) { continue; }
            throw new InvalidOperationException("A mismatched-suit or unconditional consumer must not use a conditional judgment-owned card selection.");
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Beige);
        public long BeigeFrameId => Facts<ProgramBindingStartedEvent>().Single(e => e.SkillId == Beige).FrameId;
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public JudgmentRequestedEvent[] Judgments() => Facts<JudgmentRequestedEvent>().Where(e => e.Reason == Reason).ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Beige && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Drive(string id) => Accept(new UseProgramSkillCommand(0, BoundaryCaiWenJiMobileChecks.Driver, id, [], [], G.Revision, P!.PromptId));
        public void Native(int id, int target)
        {
            var a = G.GetHumanLegalActions().First(a => a.CardId == id &&
                (a.TargetSeats.SequenceEqual([target]) || target == 0 && a.Kind == LegalActionKind.Alcohol && a.TargetSeats.Count == 0));
            Accept(new PlayCardCommand(0, id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var i = 0; i < 160; i++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human prompt interrupted the native mobile Beige scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Native mobile Beige scene exceeded its fixed boundary steps.");
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves the original paid frame, native judgment or recovery children and all actual skill-loss sources."); G = restored;
        }
        private void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Cai Wen Ji command."); }
        public void Need(bool value, string why)
        {
            if (!value) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, P, Stack = G.ResolutionStack.Select(f => f.Kind), Last = G.Events.TakeLast(5).Select(e => e.Payload.GetType().Name) }));
        }
        public static Scene Start(Suit suit, bool death = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(suit));
            var setup = new ShanheBattleSetup(83, 1, "mobile-boundary-cai-wen-ji", death ? Peer : Hero, 8, death ? 0 : 5, 0, 0, 0,
                death ? [Driver, "classic:wusheng"] : [Quiet, Paid, Final],
                death ? [new(CardKind.Slash, Suit.Club, 6), new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Dodge, Suit.Heart, 8)] :
                    [new(CardKind.Alcohol, Suit.Heart, 4), new(CardKind.Slash, Suit.Club, 6), new(CardKind.Dodge, Suit.Spade, 7), new(CardKind.Dodge, Suit.Club, 8), new(CardKind.Dodge, Suit.Diamond, 9)],
                [new(CardKind.Crossbow, Suit.Club, 5)],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(death ? Hero : Peer, Hp: death ? 1 : 8) { StartingHand = [] }],
                [new(Peer, Hp: 8) { StartingHand = [] }], new string('d', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                AdvanceAfterHumanCommands = false, UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var scene = new Scene(g, r); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(Suit suit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-boundary-cai-wen-ji", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:guan-yu", "classic:zhang-fei");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"acquire","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["classic:paoxiao"]}]}]},
              {"id":"{{Paid}}","revision":1,"triggers":[{"id":"paid","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["{{Payment}}"],"optional":false,"isSkillInvocation":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-return","options":[{"id":"continue"}]}]}]},
              {"id":"{{Final}}","revision":1,"triggers":[{"id":"final","window":"judgmentFinalized","subject":"any","judgmentReasons":["{{Reason}}"],"suits":["heart","diamond","club","spade"],"excludedReasons":[],"minimumRank":1,"maximumRank":13,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"final-return","options":[{"id":"continue"}]}]}]},
              {"id":"{{Recovery}}","revision":1,"triggers":[{"id":"recovery","window":"afterHpRecovered","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"recovery-return","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var names = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Driver }) names[id] = new { name = id, description = "安静的真实牌局与原生授技。" };
            foreach (var id in new[] { Paid, Final, Recovery }) names[id] = new { name = id, description = "真实付款、判定和回复子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "悲歌安静同桌", "", Quiet, "wei", 8, [Recovery]));
            const string deck = "fixture:mobile-cwj-deck";
            b.AddDeck(new(deck, "固定判定库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", suit, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界蔡文姬真实伤害与判定", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
