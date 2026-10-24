using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryCaoZhangMobileChecks
{
    private const string Hero = "boundary:cao-zhang-mobile", Skill = "boundary:jiangchi-mobile", Peer = "fixture:mobile-cao-zhang-peer",
        Quiet = "fixture:mobile-cao-zhang-quiet", Extra = "fixture:mobile-cao-zhang-extra", Driver = "fixture:mobile-cao-zhang-driver", Paid = "fixture:mobile-cao-zhang-paid";

    public static void DrawModeBansUsesWithCrossbowAllowsPlayedAndExpiresBeforeNextPlay()
    {
        var s = Scene.Start(assault: false); s.Offer(); var before = s.V().Hand.Count;
        s.Choose("draw-restraint"); s.Play(); var lease = s.Facts<BoundaryJiangchiIssuedEvent>().Single();
        s.Need(s.V().Hand.Count == before + 1 && lease.Mode == "restraint" && lease.CostCard is null && s.V().Equipment.Single().Kind == CardKind.Crossbow &&
            s.Actions().All(a => a.Kind != LegalActionKind.Slash), "Actual one-card draw issues a use prohibition for this Play, including a genuinely equipped Crossbow's unlimited Slash allowance.");
        s.Drive("lose", []); s.Play();
        s.Need(s.V().Skills!.All(k => k.ContentId != Skill) && s.Actions().All(a => a.Kind != LegalActionKind.Slash),
            "Losing the source skill after issue cannot revoke the current phase's already chosen use prohibition.");
        s.Drive("duel", [1, 0]); s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        var played = s.P!.Choices.First(c => c.Cards.Count == 1 && s.V().Hand.Any(h => h.Id == c.Cards[0] && h.Kind == CardKind.Slash)).Cards.Single();
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([played])); s.Play();
        s.Need(s.Facts<DuelResponseEvent>().Any(e => e.ResponderSeat == 0 && e.UsedSlash) &&
            s.Facts<CardActionAcceptedEvent>().Any(e => e.Action.Type == CardActionType.Response && e.Action.ActorSeat == 0 && e.Action.EffectiveKind == CardKind.Slash) &&
            s.Actions().All(a => a.Kind != LegalActionKind.Slash), "Cold restores the original native Duel response: actual Played Slash is allowed while Slash uses remain forbidden.");
        s.End(); s.Play();
        s.Need(s.G.State.TurnNumber == lease.Phase.TurnNumber && s.Actions().Any(a => a.Kind == LegalActionKind.Slash) &&
            s.Facts<ActualDiscardRecoveryPhaseStartedEvent>().Where(e => e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && e.Phase.ActorSeat == 0).Last().Phase != lease.Phase &&
            s.Facts<BoundaryJiangchiIssuedEvent>().Length == 1,
            "The next actual Play in the same turn has a different phase token and no old prohibition, even after source loss prevented another Jiangchi offer.");
    }

    public static void TrueEquipmentDiscardPaidColdGrantsOnlyThisPlayTwoDistantSlashes()
    {
        var s = Scene.Start(assault: true); s.Offer(); var armor = s.V().Equipment.Single().Id; var before = s.V().Hand.Count;
        s.Choose("discard-assault"); s.Reach(() => s.P?.Choices.Any(c => c.Cards.SequenceEqual([armor])) == true); s.Answer(c => c.Cards.SequenceEqual([armor]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid);
        var mother = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        s.Need(mother.BoundaryJiangchi is { Mode: "assault" } && mother.PendingMovementContinuation is not null &&
            s.Zone(armor) == CardLocation.DiscardPile && s.V().Hp == 8 && s.V().Hand.Count == before &&
            s.Facts<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) == 0 && s.Facts<BoundaryJiangchiIssuedEvent>().Length == 0,
            "The original phase frame owns its real paid HE discard and Silver Lion recovery child; it has neither recast-drawn nor issued the benefit before that child returns.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        var lease = s.Facts<BoundaryJiangchiIssuedEvent>().Single();
        s.Need(lease is { Mode: "assault" } && lease.CostCard == armor && s.G.GetSeatDistance(0, 2) > s.G.GetAttackRange(0) &&
            s.Actions().Any(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([2])) &&
            s.G.CardMovements.Count(m => m.CardId == armor && m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile) == 1,
            "The original paid cold return issues one exact phase lease, allowing a genuinely out-of-range Slash with no second discard.");
        s.Slash(2); s.Play(); s.Drive("lose", []); s.Play(); s.Slash(2); s.Play();
        s.Need(s.V(2).Hp == 6 && s.Actions().All(a => a.Kind != LegalActionKind.Slash) && s.V().Hand.Any(c => c.Kind == CardKind.Slash),
            "The issued benefit survives source loss and permits exactly two actual distant Slash uses, leaving the third physical Slash blocked by the real quota.");
        s.End(); s.Play();
        s.Need(s.G.State.TurnNumber == lease.Phase.TurnNumber && s.Actions().Any(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([3])) &&
            s.Actions().All(a => a.Kind != LegalActionKind.Slash || !a.TargetSeats.Contains(2)),
            "The next actual Play in the same turn restores ordinary distance and allowance instead of retaining the earlier assault lease.");
        s.Slash(3); s.Play(); s.Need(s.V().Hand.Any(c => c.Kind == CardKind.Slash) && s.Actions().All(a => a.Kind != LegalActionKind.Slash) && s.Facts<BoundaryJiangchiIssuedEvent>().Length == 1,
            "The fresh Play permits one ordinary Slash and never reissues or carries over the earlier extra use.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public IReadOnlyList<LegalAction> Actions() => G.GetHumanLegalActions();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer() => Reach(() => P is { PlayerSeat: 0 } && P.Choices.Any(c => c.Parameters.GetValueOrDefault("skill-id") == Skill && c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Choose(string binding) => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate" && c.Parameters.GetValueOrDefault("binding-id") == binding);
        public void Drive(string id, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, BoundaryCaoZhangMobileChecks.Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Slash(int target)
        {
            var a = Actions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource });
        }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var i = 0; i < 150; i++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "An unexpected human prompt interrupted the native mobile Jiangchi scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The native mobile Jiangchi scene exceeded its phase boundary.");
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves the actual phase lease, prepared original frame and native paid or played response child."); G = restored;
        }
        private void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Jiangchi command."); }
        public void Need(bool value, string why)
        {
            if (!value) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, P, Stack = G.ResolutionStack.Select(f => f.Kind), Last = G.Events.TakeLast(5).Select(e => e.Payload.GetType().Name) }));
        }
        public static Scene Start(bool assault)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            string[] skills = assault ? [Quiet, Driver, Extra, Paid] : [Quiet, Driver, Extra];
            var setup = new ShanheBattleSetup(83, 1, "mobile-boundary-cao-zhang", Hero, assault ? 7 : 8, 4, 0, 0, 0, skills,
                [new(CardKind.Slash, Suit.Club, 1), new(CardKind.Slash, Suit.Spade, 2), new(CardKind.Slash, Suit.Diamond, 3), new(CardKind.Slash, Suit.Heart, 6), new(CardKind.Dodge, Suit.Club, 4)],
                [new(assault ? CardKind.SilverLion : CardKind.Crossbow, Suit.Club, 5)],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }], [new(Peer, Hp: 8) { StartingHand = [] }], new string('b', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                AdvanceAfterHumanCommands = false, UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var scene = new Scene(g, r); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-cao-zhang-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            b.AddCard(new("fixture:mobile-cao-zhang-silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Extra}}","revision":1,"triggers":[{"id":"actual-extra","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]},
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{Skill}}"],"sourceBind":"standard:none"}]},
                {"id":"duel","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"startVirtualDuel","target":"owner"}]}
              ]},
              {"id":"{{Paid}}","revision":1,"triggers":[{"id":"actual-paid","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill-program.{{Skill}}.SelectAndMoveOwnedCard"],"optional":false,"isSkillInvocation":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var names = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Extra, Driver }) names[id] = new { name = id, description = "真实阶段、失技与原生响应。" };
            names[Paid] = new { name = Paid, description = "真实弃置完成后的原母帧子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "将驰安静同桌", "", Quiet, "wei", 8));
            const string deck = "fixture:mobile-cao-zhang-deck";
            b.AddDeck(new(deck, "固定闪库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界曹彰真实阶段", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
