using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileDongBaiChecks
{
    private const string Skill = "mobile:lianzhu", Xiahui = "mobile:xiahui", Owner = "fixture:mobile-dong-bai-owner",
        Driver = "fixture:mobile-dong-bai-driver", Hold = "fixture:mobile-dong-bai-gift-child",
        Mode = "identity:classic-mobile-dong-bai-fixture", Deck = "fixture:mobile-dong-bai-deck";

    public static void RedEquipmentGiftRelaysAfterOtherGiftColdAndLocksUntilHpLoss()
    {
        var s = Scene.Start(); s.Play(0); var entity = s.EquipRed(); var before = s.V(0).Hand.Count;
        s.Gift(entity, 2); s.Relay(2);
        var issued = s.Facts<MobileLianzhuIssuedEvent>().Single();
        s.Need(issued.From == CardLocation.Equipment(0) && s.Zone(entity) == CardLocation.Hand(2) &&
            s.Facts<ProgramCardsRevealedEvent>().Any(e => e.SkillId == Skill && e.Cards.Any(c => c.Id == entity)) &&
            s.Facts<XiaHuiRestrictionGrantedEvent>().Any(e => e.Restriction.CardId == entity && e.Restriction.RecipientSeat == 2 && e.Restriction.Source.SkillId == Xiahui),
            "A real red equipment entity is shown and delivered through its actual non-hand source before the holder's relay; mobile Xiahui also locks that red entity.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "give");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("card-", StringComparison.Ordinal) == true) == true);
        var cost = s.P!.Choices.First(c => c.Cards.Count == 1).Cards[0]; s.Answer(c => c.Cards.SequenceEqual([cost]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
        s.Need(s.Zone(cost) == CardLocation.Hand(0) && s.Zone(entity) == CardLocation.Hand(2) &&
            s.Facts<MobileLianzhuSegmentPaidEvent>().Any(e => e.Stage == MobileLianzhuStage.GiftChildren && e.CardId == cost),
            "The holder first really gives another owned card to Dong Bai; the designated entity is not passed before that paid gift's native child returns.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Relay(1);
        s.Need(s.Zone(entity) == CardLocation.Hand(1) && s.G.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.Hand(0)) == 1,
            "After cold return the designated real entity passes once to the previous living seat, and the other-card gift is never repaid.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "draw"); s.Play(0);
        s.Need(s.V(0).Hand.Count == before + 3 && s.Facts<MobileLianzhuGainProcessedEvent>().Length == 2 &&
            s.Facts<MobileLianzhuSegmentPaidEvent>().Count(e => e.Stage == MobileLianzhuStage.DrawChildren) == 1,
            "Only the second actual holder selects draw two, after the one other-card payment and one pass.");
        s.End(0); s.Play(1);
        var actions = s.G.GetLegalActions(1);
        s.Need(s.V(1).Hand.Any(c => c.Id == entity) && actions.Count > 0 && actions.All(a => a.CardId != entity),
            "The already issued red-entity lock survives the original Play ending and blocks its recipient's actual native equipment use.");
        var hp = s.V(1).Hp; s.Drive(1, "lose-one"); s.Play(1);
        s.Need(s.V(1).Hp == hp - 1 && s.Facts<XiaHuiRestrictionReleasedEvent>()
            .Any(e => e.CardId == entity && e.RecipientSeat == 1 && e.Reason == "recipient-hp-decreased"),
            "The recipient's real HP decrease releases the same original red-entity restriction.");
        s.Need(s.G.GetLegalActions(1).Any(a => a.CardId == entity),
            "The real recipient's own Play prompt now offers its original red physical equipment.");
        s.Equip(1, entity); s.Play(1);
        s.Need(s.Zone(entity) == CardLocation.Equipment(1), "The recipient actually equips that same released red entity through the native command.");
    }

    public static void HolderFollowsEntityForDistanceQuotaAndDamageRecovery()
    {
        var s = Scene.Start(); s.Play(0); var entity = s.EquipRed();
        s.Gift(entity, 2); s.Relay(2); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "draw"); s.Play(0);
        var hp = s.V(2).Hp; s.Slash(2); s.Recovery();
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play(0);
        var second = s.V(0).Hand.First(c => c.Kind == CardKind.Slash).Id;
        var targets = s.G.GetHumanLegalActions().Where(a => a.CardId == second && a.Kind != LegalActionKind.Recast).SelectMany(a => a.TargetSeats).ToArray();
        s.Need(targets.Contains(2) && !targets.Contains(1) && s.Zone(entity) == CardLocation.Hand(2),
            "After the actual first Slash spends its ordinary quota, the distant current holder remains a legal target while an ordinary peer does not gain that privilege.");
        s.Slash(2); s.Recovery(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Relay(0); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "draw"); s.Play(0);
        s.Need(s.V(2).Hp == hp - 2 && s.Zone(entity) == CardLocation.Hand(0) &&
            s.Facts<MobileLianzhuSegmentPaidEvent>().Count(e => e.Stage == MobileLianzhuStage.RecoverChildren && e.CardId == entity) == 1 &&
            s.V(0).Hand.Where(c => c.Kind == CardKind.Slash).All(c => !s.G.GetHumanLegalActions()
                .Any(a => a.CardId == c.Id && a.TargetSeats.Contains(2))),
            "The optional second damage return obtains the same real card, processes its new owner's gain once, and removes the former holder's distance/quota permission immediately.");
    }
    private static string Peer(int seat) => "fixture:mobile-dong-bai-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int card) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location;
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Relay(int seat) => Reach(() => P is { } p && p.PlayerSeat == seat && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "mobile-lianzhu-relay"));
        public void Recovery() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public int EquipRed()
        {
            var card = V(0).Hand.First(c => c.Kind == CardKind.Crossbow && c.Suit == Suit.Heart).Id;
            Equip(0, card); Play(0); return card;
        }
        public void Equip(int seat, int card) => Accept(new PlayCardCommand(seat, card, [], G.Revision, P!.PromptId));
        public void Gift(int card, int target) => Accept(new UseProgramSkillCommand(0, Skill, "reveal-and-give", [card], [target], G.Revision, P!.PromptId));
        public void Drive(int seat, string activation) => Accept(new UseProgramSkillCommand(seat, Driver, activation, [], [], G.Revision, P!.PromptId));
        public void Slash(int target)
        { var card = V(0).Hand.First(c => c.Kind == CardKind.Slash).Id; Accept(new PlayCardCommand(0, card, [target], G.Revision, P!.PromptId)); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 220 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p)
                { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer(p.PlayerSeat), G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification })
                { var response = P!; Accept(new AnswerPromptCommand(response.PlayerSeat, response.PromptId, response.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage").Id, G.Revision)); continue; }
                Need(P is null, "Unexpected native remade Dong Bai boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The small remade Dong Bai fixture missed its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the original phase entity, actual other-card payment and native child before the outstanding pass.");
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, Hand = V(0).Hand })); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native remade Dong Bai command."); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var g = GameEngine.CreateStandard(new() { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-dong-bai", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:dong-bai");
            var c = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"lose-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]},
              {"id":"{{Hold}}","revision":1,"triggers":[{"id":"paid-other-gift","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch",
                "movementReasons":["skill-program.mobile:lianzhu.GiftChildren"],"optional":false,"isSkillInvocation":false,
                "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "实际体力减少", description = "原生HP下降以解除实体限制。" },
                    [Hold] = new { name = "实际给牌子窗", description = "其他牌已经交付，连诛牌尚未传出。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description)
                { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "董白移动正式重制", "supporter", Skill, "qun", 20, [Xiahui, Driver], GeneralGender.Female));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实连诛持有者", "supporter", Driver, "wei", 20, [Hold]));
            string[] cards = ["standard:crossbow", "standard:slash", "standard:dodge"];
            b.AddDeck(new(Deck, "固定实体与红色装备", 12, 0, []) { PhysicalCards = Enumerable.Range(0, 120)
                .Select(i => new ContentDeckPhysicalCard(cards[i % 3], i % 3 == 0 ? Suit.Heart : Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "移动董白实际传牌与来源", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
