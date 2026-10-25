using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryPangDeMobileChecks
{
    private const string Hero = "boundary:pang-de-mobile", Skill = "boundary:jianchu-mobile", Driver = "fixture:mobile-pang-de-driver",
        Watch = "fixture:mobile-pang-de-discard", Peer = "fixture:mobile-pang-de-peer", Deck = "fixture:mobile-pang-de-deck";
    public static void NonEquipmentTrickClaimsOriginalSlashAndEquipmentForbidsDodgeAfterPaidSourceLossCold()
    {
        var trick = Scene.Start(); trick.Play();
        var original = trick.Slash(); trick.Activate();
        var discard = trick.V(1).Hand.Single(c => c.Kind == CardKind.Dismantlement).Id;
        var slot = trick.V(1).Hand.ToList().FindIndex(c => c.Id == discard);
        trick.Answer(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand" && c.Parameters.GetValueOrDefault("slot-index") == slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        trick.AtPaid(); var paid = trick.E<BoundaryPangDeDiscardPaidEvent>().Single();
        var parent = trick.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == paid.FrameId);
        trick.Need(paid.Kind == CardKind.Dismantlement && paid.CardId == discard && parent.InstructionIndex == 1 && parent.PendingMovementContinuation is { SubjectSeat: 1 } &&
            !trick.E<ProgramCurrentUsePhysicalCardsClaimedEvent>().Any(e => e.Claim.SkillId == Skill),
            "A real non-equipment Trick cost waits on its original target-discard child before issuing the mobile claim, unlike the OL nonbasic branch.");
        trick.Cold(); trick.FinishPaidLoss(); trick.Play();
        var claim = trick.E<ProgramCurrentUsePhysicalCardsClaimedEvent>().Single(e => e.Claim.SkillId == Skill).Claim;
        trick.Need(!trick.V(0).Skills!.Any(k => k.ContentId == Skill) && claim.ProducerFrameId == paid.FrameId && claim.CardUseFrameId == paid.UseId && claim.ActionId == paid.ActionId &&
            claim.CardIds.SequenceEqual([original]) && trick.V(1).Hand.Any(c => c.Id == original) && trick.V(1).Hp == 4 &&
            trick.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            trick.G.CardMovements.Count(m => m.CardId == original && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            trick.G.CardMovements.Count(m => m.CardId == original && m.From == CardLocation.Processing && m.To == CardLocation.Hand(1)) == 1 &&
            trick.E<BoundaryPangDeDiscardPaidEvent>().Count() == 1 && trick.G.GetHumanLegalActions().All(a => a.Kind != LegalActionKind.Slash),
            "Cold and actual source loss return once: the exact original Slash is claimed by the target, then still allows a real Dodge, without repaying cost or adding the OL Slash allowance.");

        var equip = Scene.Start(equipment: true); equip.Play(); equip.UseEquip(); equip.Play();
        var armor = equip.V(1).Equipment.Single().Id; var dodge = equip.V(1).Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        var slash = equip.Slash(); equip.Activate(); equip.Answer(c => c.Cards.SequenceEqual([armor])); equip.AtPaid();
        var equipPaid = equip.E<BoundaryPangDeDiscardPaidEvent>().Single(); equip.Cold(); equip.FinishPaidLoss(); equip.Play();
        equip.Need(equipPaid.Kind == CardKind.BaguaFormation && !equip.E<ProgramCurrentUsePhysicalCardsClaimedEvent>().Any(e => e.Claim.SkillId == Skill) &&
            equip.V(1).Hp == 3 && equip.V(1).Hand.Any(c => c.Id == dodge) && !equip.E<CardRespondedEvent>().Any(e => e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) &&
            !equip.E<ResponseRequestedEvent>().Any(e => e.TargetSeat == 1 && e.RequiredCardKind == CardKind.Dodge) &&
            equip.G.CardMovements.Count(m => m.CardId == armor && m.From == CardLocation.Equipment(1) && m.To == CardLocation.DiscardPile) == 1 &&
            equip.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            equip.G.GetHumanLegalActions().All(a => a.Kind != LegalActionKind.Slash),
            "An actual equipment discard instead issues original-target no-Dodge after the paid child and source loss; it leaves the available Dodge untouched, claims no Slash and grants no extra Slash use.");
    }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void AtPaid() => Reach(() => P?.SkillPrompt?.SkillId == Watch);
        public void UseEquip() => Accept(new UseProgramSkillCommand(0, Driver, "equip", [], [1], G.Revision, P!.PromptId));
        public int Slash() { var a = G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([1])); Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind)); Reach(() => P?.SkillPrompt?.SkillId == Skill); return a.CardId.Value; }
        public void Activate() { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true); }
        public void FinishPaidLoss()
        {
            Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill) == true); Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill);
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish") == true); Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish");
        }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected native mobile Pang De command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 140 && !done(); i++)
            {
                Need(P is null || !G.CreateSnapshot(P.PlayerSeat).Players[P.PlayerSeat].IsHuman, "Unexpected native mobile Pang De human boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Missing bounded mobile Pang De boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original native Slash, exact target cost and its still-paid movement child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Hand = V(0).HandCount, Hp = V(1).Hp })); }
        public static Scene Start(bool equipment = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture()); var extra = Enumerable.Range(0, 6).Select(i => "fixture:mobile-pang-de-dummy-" + i).ToArray();
            var target = new ShanheCombatantDefinition(Peer) { StartingHand = equipment ? [new(CardKind.Dodge, Suit.Heart, 2)] : [new(CardKind.Dismantlement, Suit.Club, 3), new(CardKind.Dodge, Suit.Heart, 2)] };
            var setup = new ShanheBattleSetup(101, 1, "fixture-mobile-pang-de", Hero, 4, 0, 0, 0, 0, [Driver, Watch, .. extra], [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Slash, Suit.Club, 8)], [],
                [target, new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }], [], new string('a', 64));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, r));
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-pang-de-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:ma-chao");
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { new { id = "equip", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "useRandomDeckEquipment", target = "owner", resultBind = "equipped" } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "original-target-discard", window = "discardPileReceived", subject = "owner",
                    movementReasons = new[] { "skill-program.boundary:jianchu-mobile.SelectAndMoveOwnedCard" }, optional = false, usageScope = "game", usageLimit = 1,
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } }, new { op = "replaceSkillsOnAwakening", target = "owner", skillIds = new[] { "standard:none" } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实目标装备", description = "给实际目标使用牌堆装备。" }, [Watch] = new { name = "原鞬出弃牌孩子", description = "原目标弃牌后失去来源。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            foreach (var i in Enumerable.Range(0, 6)) b.AddSkill(new("fixture:mobile-pang-de-dummy-" + i, "原生失技资格", "供真实弃牌孩子失去来源。"));
            b.AddGeneral(new(Peer, "真实防守者", "supporter", "standard:none", "wei", 4));
            b.AddDeck(new(Deck, "固定真实防具", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Spade, 2)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动鞬出原生流程", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
