using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileWangCanChecks
{
    private const string Qiai = "mobile:qiai", Shanxi = "mobile:shanxi-wang-can", DriverId = "fixture:mobile-wang-can-driver", WatchId = "fixture:mobile-wang-can-watch";
    private const string Mode = "identity:classic-mobile-wang-can", GiftReason = "skill-program.mobile-wang-can.xi-gift";
    public static void MarkedRecoveryExactTwoHeGiftAndQiaiRecipientDrawCold()
    {
        var s = Scene.Start(); s.Mark(1); s.Play(0); s.Driver("equip", [1]); s.Play(0);
        var equipment = s.V(1).Equipment.Single().Id; var handCard = s.V(1).Hand.First().Id; var ownerHand = s.V(0).HandCount;
        s.Driver("hurt-two", [1]); s.Play(0); s.Driver("recover-two", [1]); s.Reach(() => s.Branch("gift"));
        s.Need(s.P!.PlayerSeat == 1 && s.V(1).Hp == s.V(1).MaxHp && s.E<MobileWangCanXiQualifiedEvent>().Length == 1,
            "A single real recovery of two HP issues one consequence to the actual marked foreign participant, after HP has recovered.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "gift");
        s.Answer(c => c.Cards.SequenceEqual([equipment]) && c.Parameters.GetValueOrDefault("branch") == "card");
        s.Need(!s.Branch("pay"), "One card cannot pay the exact two-card Xi obligation.");
        s.Answer(c => c.Cards.SequenceEqual([handCard]) && c.Parameters.GetValueOrDefault("branch") == "card");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "pay");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == WatchId || s.P is { Kind: DecisionKind.PlayCard });
        s.Need(s.P?.SkillPrompt?.SkillId == WatchId && s.P.PlayerSeat == 0 && s.V(0).HandCount == ownerHand + 2 &&
            s.V(1).Equipment.Count == 0 && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MobileWangCanXi is { Stage: MobileWangCanXiStage.GiftChildren, ActualCount: 2 }),
            "Both exact HE entities are genuinely transferred before the native gain child, while the original Shanxi payment frame remains paid.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play(0);
        s.Need(s.E<MobileWangCanXiPaidEvent>() is [{ Gift: true, ActualCount: 2 }] && s.V(1).Hp == s.V(1).MaxHp &&
            s.G.CardMovements.Count(m => m.Reason.Value == GiftReason && m.From.OwnerSeat == 1 && m.To == CardLocation.Hand(0)) == 2,
            "Paid recovery preserves the single genuine two-card transfer and does not also lose HP.");
        var beforeQiai = s.V(0).HandCount;
        s.Accept(new UseProgramSkillCommand(0, Qiai, "nonbasic-gift", [equipment], [2], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Qiai && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "draw"));
        s.Need(s.P!.PlayerSeat == 2 && s.V(2).Hand.Any(c => c.Id == equipment) && s.V(0).HandCount == beforeQiai - 1 &&
            s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "recover"),
            "Qiai first genuinely gives its nonbasic material, then the recipient chooses either reward, including recovery at full HP.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "draw"); s.Play(0);
        s.Need(s.V(0).HandCount == beforeQiai + 1 && s.V(0).Hp == s.V(0).MaxHp &&
            !s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("skill-id") == Qiai),
            "The recipient's draw branch draws two actual cards only for Wang Can, without also recovering or repeating the phase-limited activation.");
    }
    public static void XiMovesOnlyToUnmarkedOthersAndEachActualRecoveryLosesOneHp()
    {
        var s = Scene.Start(); s.Mark(1); s.Play(0); s.Driver("hurt-two", [1]); s.Play(0); s.End();
        s.Reach(() => s.Branch("mark"));
        s.Need(s.P!.ValidTargetSeats.Contains(2) && !s.P.ValidTargetSeats.Contains(1) && !s.P.ValidTargetSeats.Contains(0),
            "At the next actual Play start the existing holder and Wang Can himself cannot receive the transferred Xi.");
        s.Mark(2); s.Play(0);
        s.Need((s.V(1).Markers?.SingleOrDefault(m => m.Kind == PlayerMarkerKind.MobileWangCanXi)?.Count ?? 0) == 0 &&
            s.V(2).Markers?.SingleOrDefault(m => m.Kind == PlayerMarkerKind.MobileWangCanXi)?.Count == 1,
            "The actual public marker moves instead of leaving an additional Xi on its former holder.");
        s.Driver("recover-two", [1]); s.Play(0);
        s.Need(s.E<MobileWangCanXiQualifiedEvent>().Length == 0, "The former holder's real recovery no longer issues any Xi obligation.");
        s.Driver("hurt-two", [2]); s.Play(0); s.Driver("recover-two", [2]); s.Reach(() => s.Branch("lose-hp"));
        s.Need(s.P!.PlayerSeat == 2 && s.V(2).Hp == s.V(2).MaxHp, "The new holder chooses after its own actual two-HP recovery.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "lose-hp"); s.Play(0);
        s.Need(s.V(2).Hp == s.V(2).MaxHp - 1 && s.E<MobileWangCanXiPaidEvent>() is [{ Gift: false, ActualCount: 1 }] &&
            s.E<MobileWangCanXiQualifiedEvent>() is [{ TargetSeat: 2 }], "Declining the exact two-card gift loses only one HP for the whole recovery event after cold return.");
        s.Driver("recover-two", [2]); s.Reach(() => s.Branch("lose-hp")); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "lose-hp"); s.Play(0);
        s.Need(s.V(2).Hp == s.V(2).MaxHp - 1 && s.E<MobileWangCanXiPaidEvent>().Length == 2 &&
            s.E<MobileWangCanXiQualifiedEvent>().Select(e => e.ChangeId).Distinct().Count() == 2,
            "A later distinct actual recovery triggers again, once per recovery without a per-turn cap or a per-point penalty.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Branch(string branch) => P?.SkillPrompt?.SkillId == Shanxi && P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == branch);
        public void Mark(int seat) { Reach(() => Branch("mark")); Answer(c => c.Parameters.GetValueOrDefault("branch") == "mark" && c.Targets.SequenceEqual([seat])); }
        public void Driver(string id, int[] targets) => Accept(new UseProgramSkillCommand(0, DriverId, id, [], targets, G.Revision, P!.PromptId));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "The required native prompt choice is missing."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision)); }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Actual Wang Can command rejected."); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 220 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, General(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } discard) { Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision)); continue; }
                if (P is { } p)
                {
                    var choice = p.SkillPrompt?.SkillId == Shanxi ? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "activate") : null;
                    choice ??= p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == "continue") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") ?? p.Choices.First();
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded actual Wang Can boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.CardMovements.SequenceEqual(copy.CardMovements) && G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))),
                "Cold recovery preserves the original marked recovery, private two-card selection and paid native child."); G = copy;
        }
        public void Need(bool ok, string text)
        {
            if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { P, G.State.TurnNumber, G.State.CurrentSeat,
                Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).IsAlive, V(i).Hp, V(i).HandCount, Equipment = V(i).Equipment.Count }).ToArray(),
                Facts = G.Events.Select(e => e.Payload).Where(e => e is MobileWangCanXiIssuedEvent or MobileWangCanXiQualifiedEvent or MobileWangCanXiPaidEvent or ProgramBindingResolvedEvent).TakeLast(10).Select(e => JsonSerializer.Serialize(e, e.GetType())).ToArray() }));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry), registry);
            s.Accept(new StartGameCommand()); s.Reach(() => s.Branch("mark")); return s;
        }
    }
    private static string General(int seat) => "fixture:mobile-wang-can-general-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-wang-can", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:wang-can");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{DriverId}}","revision":1,"modifiers":[{"id":"keep-hands","query":"handLimit","operation":"add","value":20,"priority":0}],"activations":[
                {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"gear"}]},
                {"id":"hurt-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":2}]},
                {"id":"recover-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"selectedTarget","amount":2}]}
              ]},
              {"id":"{{WatchId}}","revision":1,"triggers":[{"id":"paid-two-card-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["{{GiftReason}}"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,
                "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-pause","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [DriverId] = new { name = "真实王粲准备", description = "实际装备、失去体力、回复。" },
                    [WatchId] = new { name = "真实善檄得牌子窗", description = "付款完成后的实际得牌。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            for (var seat = 0; seat < 4; seat++) b.AddGeneral(new(General(seat), "固定真实参与者", "supporter", seat == 0 ? Qiai : "standard:none", "wei", seat == 0 ? 3 : 4,
                seat == 0 ? [Shanxi, DriverId, WatchId] : [WatchId]));
            const string deck = "fixture:mobile-wang-can-deck";
            b.AddDeck(new(deck, "固定真实材料", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2))
                .Concat(Enumerable.Range(0, 8).Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 1))).ToArray() });
            b.AddMode(new(Mode, "移动王粲原生流程", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
