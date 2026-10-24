using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryChenQunMobileChecks
{
    private const string Hero = "boundary:chen-qun-mobile", Dingpin = "boundary:dingpin-mobile", Faen = "boundary:faen-mobile",
        Quiet = "fixture:boundary-chen-qun-quiet", Skip = "fixture:boundary-chen-qun-skip", Driver = "fixture:boundary-chen-qun-driver",
        CostHold = "fixture:boundary-chen-qun-cost", JudgmentHold = "fixture:boundary-chen-qun-judgment", GainHold = "fixture:boundary-chen-qun-gain", Peer = "fixture:boundary-chen-qun-peer";

    public static void HeartRefundOwnDiscardAndDiamondFaenBothDirectionsCold()
    {
        var s = Scene.Start(Suit.Heart); s.Play(); var cost = s.V().Hand.First(c => c.Kind == CardKind.Dodge).Id;
        s.Dingpin(cost, 3); s.Cost();
        s.Need(s.Zone(cost) == CardLocation.DiscardPile && s.Root?.BoundaryChenQunDingpin is { Stage: BoundaryDingpinStage.Prepared } &&
            s.Facts<JudgmentRequestedEvent>().Length == 0 && s.Facts<NativeLogicalDiscardMaterialCompletedEvent>().Count(e => e.CardId == cost) == 1,
            "Dingpin pays the exact original HE entity through the native logical-discard path before its cost child returns and before judgment is issued.");
        s.Cold(); s.Release(); s.Judgment(); s.Cold(); s.Release(); s.Play();
        s.Need(s.Facts<BoundaryDingpinResultEvent>() is [{ Suit: Suit.Heart }] && s.Facts<BoundaryDingpinHeartExceptionEvent>() is [{ CardId: var excepted }] && excepted == cost &&
            s.Facts<NativeLogicalDiscardMaterialCompletedEvent>().Count(e => e.CardId == cost) == 1 && !s.V().IsFaceDown &&
            s.DingpinAction()!.SelectableCardIds!.Any(id => s.V().Hand.Single(c => c.Id == id).Kind == CardKind.Dodge),
            "A real heart final result exempts only this original cost invoice without erasing global discard facts, permits another Basic cost and does not turn Chen Qun over.");
        var other = s.V().Hand.First(c => c.Kind == CardKind.Dodge).Id; s.Driver("discard-one", other); s.Play();
        s.Need(s.DingpinAction()!.SelectableCardIds!.All(id => s.V().Hand.Single(c => c.Id == id).Kind is not (CardKind.Dodge or CardKind.Slash)),
            "Another actual Basic discard in the same turn remains disqualifying after the earlier heart exception.");
        var chain = s.V().Hand.Single(c => c.Kind == CardKind.IronChain).Id; s.Recast(chain); s.Play();
        s.Need(s.DingpinAction()!.SelectableCardIds!.Contains(s.V().Hand.Single(c => c.Kind == CardKind.Dismantlement).Id),
            "Native Iron Chain recast is neither a use nor a discard and does not spend the Trick category.");

        s = Scene.Start(Suit.Diamond); s.Play(); cost = s.V().Hand.First(c => c.Kind == CardKind.Dodge).Id;
        s.Dingpin(cost, 0); s.Cost(); s.Release(); s.Judgment(); s.Release(); s.Faen(); s.AcceptFaen(); s.Play();
        var gained = s.FaenDraws();
        s.Need(s.V().IsFaceDown && gained == 1 && s.Facts<BoundaryDingpinHeartExceptionEvent>().Length == 0 && s.Facts<BoundaryDingpinTargetBannedEvent>().Length == 0,
            "The actual diamond judgment turns the owner face down, and mobile Faen offers its draw on that direction as well as face up.");
        s.Driver("flip"); s.Faen(); s.AcceptFaen(); s.Play();
        s.Need(!s.V().IsFaceDown && s.FaenDraws() == gained + 1, "The next real flip back to face up offers another mobile Faen draw.");
        s.Driver("chain"); s.Faen(); s.AcceptFaen(); s.Play();
        s.Driver("chain"); s.Play(); s.Driver("unchain"); s.Play();
        s.Need(!s.V().IsChained && s.FaenDraws() == gained + 2,
            "Entering chain offers one native Faen draw; setting the same chained state again and clearing chain do not create another occurrence.");
    }

    public static void BlackDrawCurrentHpCapAndTurnTargetTypeRestrictionsCold()
    {
        foreach (var hp in new[] { 2, 8 })
        {
            var s = Scene.Start(Suit.Spade, hp); s.Play();
            s.Native(s.V().Hand.Single(c => c.Kind == CardKind.Crossbow).Id); s.Play();
            var equipment = s.V().Hand.Single(c => c.Kind == CardKind.QinggangSword).Id;
            s.Need(!s.DingpinAction()!.SelectableCardIds!.Contains(equipment), "A real equipment use has already spent Equipment for this actual turn's Dingpin payment.");
            var cost = s.V().Hand.First(c => c.Kind == CardKind.Dodge).Id; var before = s.V(3).Hand.Count;
            s.Dingpin(cost, 3); s.Cost(); s.Release(); s.Judgment(); s.Release(); s.Gain();
            var requested = Math.Min(hp, 3);
            s.Need(s.Root?.BoundaryChenQunDingpin is { Stage: BoundaryDingpinStage.DrawChildren, Requested: var count } && count == requested &&
                s.V(3).Hand.Count == before + requested && s.Facts<BoundaryDingpinTargetBannedEvent>().Length == 0,
                "The actual black judgment draws min(current HP,3), independent of lost HP, before its original paid draw child returns and before its target restriction is issued.");
            if (hp == 8) s.Cold(); s.FinishGains(); s.Play();
            s.Need(s.Facts<BoundaryDingpinTargetBannedEvent>() is [{ Target: 3 }] && s.Facts<BoundaryDingpinDrawReturnedEvent>() is [{ Requested: var drawn }] && drawn == requested &&
                !s.DingpinAction()!.SelectableTargetSeats!.Contains(3) && s.DingpinAction()!.SelectableTargetSeats!.Contains(0) &&
                s.G.CardMovements.Count(m => m.CardId == cost && m.Reason.Value == "skill-program.boundary:dingpin-mobile.MoveBoundCards") == 1,
                "The paid native draw child returns once, then only its original target is banned while self and other living targets remain legal.");
            s.Native(s.V().Hand.Single(c => c.Kind == CardKind.IronChain).Id, 1); s.Faen(); s.SkipFaen(); s.Play();
            s.Need(s.DingpinAction() is null, "The actual Trick use, earlier Equipment use and Basic discard collectively exhaust this turn's three payable types.");
            s.End(); s.Play();
            s.Need(s.DingpinAction()?.SelectableTargetSeats?.Contains(3) == true,
                "Both the previous black target restriction and previous-turn used/discarded type restrictions expire at the next actual turn.");
        }
    }

    private sealed class Scene(GameEngine engine, ContentRegistry registry)
    {
        public GameEngine G = engine;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == BoundaryChenQunMobileChecks.Dingpin);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public LegalAction? DingpinAction() => G.GetHumanLegalActions().SingleOrDefault(a => a.ProgramSkillId == BoundaryChenQunMobileChecks.Dingpin);
        public int FaenDraws() => G.CardMovements.Count(m => m.Reason.Value == "skill-program.boundary:faen-mobile.Draw" && m.To == CardLocation.Hand(0));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Cost() => Reach(() => P?.SkillPrompt?.SkillId == CostHold);
        public void Judgment() => Reach(() => P?.SkillPrompt?.SkillId == JudgmentHold);
        public void Gain() => Reach(() => P?.SkillPrompt?.SkillId == GainHold);
        public void FinishGains() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, settleGains: true);
        public void Faen() => Reach(() => P?.SkillPrompt?.SkillId == BoundaryChenQunMobileChecks.Faen);
        public void Release() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void SkipFaen() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        public void AcceptFaen()
        {
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, acceptFaen: true);
        }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Dingpin(int card, int target) => Accept(new UseProgramSkillCommand(0, BoundaryChenQunMobileChecks.Dingpin, "judge-unused-category", [card], [target], G.Revision, P!.PromptId));
        public void Driver(string id, int? card = null) => Accept(new UseProgramSkillCommand(0, BoundaryChenQunMobileChecks.Driver, id, card is { } c ? [c] : [], [], G.Revision, P!.PromptId));
        public void Recast(int id) => Accept(new RecastCardCommand(0, id, G.Revision, P!.PromptId));
        public void Native(int id, params int[] targets)
        {
            var a = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == id && a.Kind != LegalActionKind.Recast && a.TargetSeats.SequenceEqual(targets));
            Need(a is not null, "The fixed scenario retains its exact real native card use.");
            Accept(new PlayCardCommand(0, id, a!.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var choice = p.Choices.Single(predicate); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision)); }
        public void Reach(Func<bool> done, bool acceptFaen = false, bool settleGains = false)
        {
            for (var i = 0; i < 240; i++)
            {
                if (done()) return;
                if (settleGains && P?.SkillPrompt?.SkillId == GainHold)
                {
                    Need(Root?.BoundaryChenQunDingpin is { Stage: BoundaryDingpinStage.DrawChildren } && Facts<BoundaryDingpinTargetBannedEvent>().Length == 0,
                        "Every actual gain batch returns to the original paid draw before its black-result target restriction is issued.");
                    Release(); continue;
                }
                if (acceptFaen && P?.SkillPrompt?.SkillId == BoundaryChenQunMobileChecks.Faen)
                {
                    var prompt = P; var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "activate") ??
                        prompt.Choices.Single(c => c.Targets.Count == 1);
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision)); continue;
                }
                Need(P is null, "The actual native Dingpin scene reached an unexpected decision.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The bounded native Dingpin scene missed its next original boundary.");
        }
        public void Cold()
        {
            var state = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == state, "Cold preserves the exact paid original discard, native judgment and draw continuation with all player views.");
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements });
        public void Need(bool ok, string why)
        {
            if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, P,
                Root = Root?.BoundaryChenQunDingpin, Last = G.Events.TakeLast(15).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) }));
        }
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected original Dingpin command."); }
        public static Scene Start(Suit suit, int hp = 8)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(suit));
            ShanheStartingCard[] hand = [new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Slash, Suit.Spade, 7),
                new(CardKind.Crossbow, Suit.Club, 1), new(CardKind.QinggangSword, Suit.Spade, 6), new(CardKind.IronChain, Suit.Club, 9), new(CardKind.Dismantlement, Suit.Spade, 11)];
            var setup = new ShanheBattleSetup(29, 1, "boundary-chen-qun", Hero, 3, 0, 0, 0, 0, [Quiet, BoundaryChenQunMobileChecks.Driver, CostHold, JudgmentHold], hand, [],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: hp) { StartingHand = [] }], [new(Peer, Hp: 8) { StartingHand = [] }], new string('b', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture(Suit judgmentSuit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-chen-qun-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
             {"id":"{{Skip}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
             {"id":"{{CostHold}}","revision":1,"triggers":[{"id":"paid","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill-program.boundary:dingpin-mobile.MoveBoundCards"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
             {"id":"{{JudgmentHold}}","revision":1,"triggers":[{"id":"final","window":"judgmentFinalized","subject":"any","judgmentReasons":["skill.boundary.dingpin-mobile"],"suits":["spade","heart","club","diamond"],"minimumRank":1,"maximumRank":13,"excludedReasons":[],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"final","options":[{"id":"continue"}]}]}]},
             {"id":"{{GainHold}}","revision":1,"triggers":[{"id":"paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary-chen-qun.dingpin.draw"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
             {"id":"{{Driver}}","revision":1,"activations":[
              {"id":"discard-one","minCards":1,"maxCards":1,"sourceZones":["hand","equipment"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"discard"},{"op":"moveBoundCards","target":"owner","sourceBind":"discard","destination":"discardPile","awaitMovementTriggers":true}]},
              {"id":"flip","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"turnOver","target":"owner"}]},
              {"id":"chain","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"owner","chained":true}]},
              {"id":"unchain","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]}
            ]}
            """;
            var names = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Skip, Driver })
                names[id] = new { name = id, description = "固定实际原生阶段和状态。" };
            foreach (var id in new[] { CostHold, JudgmentHold, GainHold })
                names[id] = new { name = id, description = "固定实际原生阶段、成本、判定及移动子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "定品固定原生同桌", "", Quiet, "wei", 8, [Skip, GainHold]));
            const string deck = "fixture:boundary-chen-qun-deck";
            b.AddDeck(new(deck, "定品固定花色库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", judgmentSuit, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "界陈群实际原生判定", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
