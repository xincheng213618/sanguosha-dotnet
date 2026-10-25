using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhangBaoChecks
{
    private const string Owner = "fixture:mobile-zhang-bao-owner", Driver = "fixture:mobile-zhang-bao-driver", Observer = "fixture:mobile-zhang-bao-draw", Mode = "identity:mobile-zhang-bao";
    public static void MatchingSuitDrawsTwiceThenDiscardsAndResumes()
    {
        var s = Scene.Start(mobile: true);
        var weapon = s.V.Hand.First(c => c.Suit == Suit.Club);
        var action = s.G.GetLegalActions(0).First(a => a.CardId == weapon.Id);
        s.Accept(new PlayCardCommand(0, weapon.Id, action.TargetSeats, s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.G.GetLegalActions(0).Where(a => a.ProgramSkillId == s.Curse).All(a => !a.SelectableCardIds.Contains(weapon.Id)), "Mobile Zhoufu cannot pay an equipped entity.");
        s.Deposit();
        s.UseByPeer(Suit.Club, expectDraw: false);
        s.Need(s.Draws == 0 && s.CurseView!.ActualDrawCount == 0, "A same-color different-suit actual use does not trigger mobile Yingbing.");
        s.UseByPeer(Suit.Spade, expectDraw: true); s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "choose-option"); s.Play();
        s.Need(s.Draws == 1 && s.CurseView!.ActualDrawCount == 1, "The first matching-suit native use draws exactly once and retains its curse.");
        s.UseByPeer(Suit.Spade, expectDraw: true); s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "choose-option"); s.Play();
        s.Need(s.Draws == 2 && s.CurseView is null && s.G.CardMovements.Count(m => m.CardId == s.CurseId && m.To == CardLocation.DiscardPile && m.Reason.Value == "program.source-curse.remove") == 1 && !s.V.Hand.Any(c => c.Id == s.CurseId),
            "The second actual draw removes the original curse to discard exactly once after its paid draw child, without obtaining it.");
        var hp = s.G.CreateSnapshot(0).Players[1].Hp;
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.G.Events.Any(e => e.Payload is SourceCurseHpLossIssuedEvent));
        s.Reach(() => !s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SourceCurseReceipt is not null));
        s.Need(s.G.CreateSnapshot(0).Players[1].Hp == hp - 1, "The actual ended turn loses HP once for this removed curse under the official mobile wording."); s.Cold();
    }
    public static void ExistingOlColorMatchStillObtainsCurse()
    {
        var s = Scene.Start(mobile: false); s.Deposit();
        s.UseByPeer(Suit.Club, expectDraw: true); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "choose-option"); s.Play();
        s.UseByPeer(Suit.Club, expectDraw: true); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "choose-option"); s.Play();
        s.Need(s.Draws == 2 && s.CurseView is null && s.V.Hand.Any(c => c.Id == s.CurseId) && s.G.CardMovements.Count(m => m.CardId == s.CurseId && m.Reason.Value == "program.source-curse.obtain") == 1,
            "Absent optional parameters preserve OL's same-color reward and obtain destination.");
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry, bool mobile)
    {
        public GameEngine G = game;
        public string Curse => mobile ? "mobile:zhoufu" : "ol:zhoufu";
        public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public SourceCurseSnapshot? CurseView => G.CreateSnapshot(0).Players[1].SourceCurses?.Single();
        public int CurseId;
        public int Draws => G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == "program.source-curse.draw");
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Zhang Bao command."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Deposit()
        {
            CurseId = V.Hand.First(c => c.Suit == Suit.Spade).Id;
            Accept(new UseProgramSkillCommand(0, Curse, "place-source-curse", [CurseId], [1], G.Revision, P!.PromptId)); Play();
        }
        public void UseByPeer(Suit suit, bool expectDraw)
        {
            Accept(new UseProgramSkillCommand(0, Driver, "equip-user", [], [1], G.Revision, P!.PromptId));
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            Answer(c => c.Cards.Count == 1 && V.Hand.Single(x => x.Id == c.Cards[0]).Suit == suit);
            if (expectDraw) Reach(() => P?.SkillPrompt?.SkillId == Observer);
            else Play();
        }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 160 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.DiscardCards } discard)
                {
                    Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision));
                    continue;
                }
                if (P is { } p)
                {
                    Need(p.Kind != DecisionKind.PlayCard, "The bounded driver must not play an unrelated turn.");
                    var choice = p.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0 && (c.Id.Value.Contains("skip", StringComparison.Ordinal) || c.Id.Value.Contains("pass", StringComparison.Ordinal))) ?? p.Choices.First();
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded mobile Zhang Bao boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(copy.ResolutionStack) && G.CardMovements.SequenceEqual(copy.CardMovements), "Paid curse child cold restoration preserves its viewers, original frame and movements."); G = copy;
        }
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { P, Hand = V.Hand.Select(c => new { c.Id, c.Suit }), Draws })); }
        public static Scene Start(bool mobile)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture(mobile));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, r), r, mobile);
            s.Accept(new StartGameCommand()); s.Accept(new SelectGeneralCommand(0, Owner, s.G.Revision, s.P!.PromptId)); s.Play(); return s;
        }
    }
    private sealed class Fixture(bool mobile) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-zhang-bao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var catalog = SkillProgramCatalog.Load(Rules.Replace("$SCHEMA$", SkillProgramCatalog.RulesSchemaVersion.ToString()), """{"schemaVersion":3,"skills":{"fixture:mobile-zhang-bao-driver":{"name":"真实转交装备","description":"由原生接收者真实使用"},"fixture:mobile-zhang-bao-draw":{"name":"摸牌后暂停","description":"保留已付款摸牌子流程","optionLabels":{"continue":"继续"}}}}""");
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = p });
            b.AddGeneral(new(Owner, "移动张宝实体驱动", "supporter", mobile ? "mobile:zhoufu" : "ol:zhoufu", "qun", 8, [mobile ? "mobile:yingbing" : "ol:yingbing", Driver, Observer]));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:mobile-zhang-bao-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "固定真实同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-zhang-bao-deck";
            b.AddDeck(new(deck, "固定两种黑色花色装备", 12, 0, []) { PhysicalCards = Enumerable.Range(0, 96).Select(i => new ContentDeckPhysicalCard("standard:crossbow", i % 2 == 0 ? Suit.Spade : Suit.Club, 7)).ToArray() });
            b.AddMode(new(Mode, "移动张宝同花色原生用牌", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 }, deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
    private const string Rules = """
    {"schemaVersion":$SCHEMA$,"skills":[
     {"id":"fixture:mobile-zhang-bao-driver","revision":1,"activations":[{"id":"equip-user","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand"],"cardCategories":["equipment"],"count":1,"destination":"selectedTargetHand","targetRef":{"kind":"selectedTarget"},"resultBind":"equipment","revealBeforeMove":true,"awaitMovementTriggers":true},{"op":"useBoundCardByTarget","target":"selectedTarget","sourceBind":"equipment"}]}]},
     {"id":"fixture:mobile-zhang-bao-draw","revision":1,"triggers":[{"id":"paid-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["program.source-curse.draw"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}
    ]}
    """;
}
