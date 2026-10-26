using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileJiKangChecks
{
    private const string Qing = "mobile:qingxian", Jue = "mobile:juexiang", Can = "mobile:canyun", Driver = "fixture:mobile-ji-kang-driver", Watch = "fixture:mobile-ji-kang-watch";
    private const string Mode = "identity:classic-mobile-ji-kang", Discard = "skill-program.mobile-ji-kang.discard";
    public static void PaidEquipmentComparisonUsesThreeNativeBranchesAndFullCountCold()
    {
        var s = Scene.Start(); s.Equip(0, 2); s.Equip(2, 1); s.Equip(3, 2); s.Driver("hurt", [1]); s.Play(0);
        var hand = s.V(0).HandCount; var peerHand = s.V(2).HandCount; var hp1 = s.V(1).Hp; var hp3 = s.V(3).Hp;
        var cost = new[] { s.V(0).Equipment.First().Id }.Concat(s.V(0).Hand.Take(2).Select(c => c.Id)).ToArray();
        s.StartPulse(Qing, [1, 2, 3], cost); var root = s.Root!.Id;
        s.Watch(0); s.Need(s.V(0).Equipment.Count == 1 && s.V(1).Hp == hp1 && s.V(3).Hp == hp3 && s.Root!.Id == root,
            "Qingxian discards the actual three cards, including its own equipment, before comparing or changing any selected target.");
        s.Cold(); s.Continue(); s.Watch(2);
        s.Need(s.V(1).Hp == hp1 + 1 && s.V(2).HandCount == peerHand + 1 && s.V(3).Hp == hp3 && s.Root!.Id == root,
            "The post-cost equipment comparison first recovers the lower population, then the equal population draws one genuine card before the later greater population resolves.");
        s.Cold(); s.Continue(); s.Play(0);
        var comparisons = s.E<MobileJiKangProgressEvent>().Where(e => e.FrameId == root && e.Step == "compare").ToArray();
        s.Need(comparisons.Select(e => (e.Subject, e.Value, e.Other)).SequenceEqual([(1, 1, 0), (2, 1, 1), (3, 1, 2)]) &&
            s.V(3).Hp == hp3 - 1 && s.V(0).HandCount == hand - 2 + 1 &&
            s.G.CardMovements.Count(m => m.Reason.Value == Discard && m.From.OwnerSeat == 0) == 3 &&
            s.E<MobileJiKangProgressEvent>().Count(e => e.FrameId == root && e.Step == "draw-return" && e.Subject == 0 && e.Value == 1) == 1,
            "All three actual branches compare against the equipment left after payment, and selecting the original HP count pays exactly one owner draw without repeating any cost or target.");
    }
    public static void DeathPunishmentPrecedesRealRecipientGrantsAndClubPaymentCold()
    {
        var s = Scene.Start(death: true); s.Equip(0, 2); s.Equip(1, 1); s.Equip(3, 1);
        var deadClub = s.V(1).Equipment.First().Id; var club = s.V(3).Equipment.First().Id; var killerHp = s.V(0).Hp;
        s.Driver("kill", [1]); s.Watch(0); var root = s.Root!.Id;
        s.Need(s.V(0).Equipment.Count == 0 && s.V(0).Hp == killerHp && !s.Has(2, Can),
            "The genuine owner's death first discards the killer's full equipment population, and its real loss child precedes the killer's HP loss and recipient reward.");
        s.Cold(); s.Continue(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Jue && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "recipient"));
        s.Need(s.V(0).Hp == killerHp - 1 && !s.Has(2, Can), "Only after the actual equipment children return does the killer lose one HP, before the optional inheritance choice.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "recipient" && c.Targets.SequenceEqual([2]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Jue && s.P.PlayerSeat == 2 && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "club"));
        s.Need(s.Has(2, Can) && !s.Has(2, Jue) && s.V(1).Equipment.Count == 0 &&
            s.G.CardMovements.Last(m => m.CardId == deadClub).To == CardLocation.DiscardPile &&
            s.P!.Choices.All(c => !c.Cards.Contains(deadClub)) && s.P.Choices.Any(c => c.Cards.SequenceEqual([club])),
            "The chosen living participant genuinely acquires Canyun first; the dead owner's equipment has already been cleaned into the discard pile and is unavailable, while the living participant's actual field Club remains eligible.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "club" && c.Cards.SequenceEqual([club])); s.Watch(3);
        s.Need(!s.V(3).Equipment.Any(c => c.Id == club) && !s.Has(2, Jue) && s.Has(2, Can) && s.Root!.Id == root,
            "The beneficiary's actual foreign field-card discard and its original movement child finish before Juexiang is inherited.");
        s.Cold(); s.Continue(); s.Play(0);
        s.Need(s.Has(2, Can) && s.Has(2, Jue) && !s.V(1).IsAlive &&
            s.E<MobileJiKangProgressEvent>().Count(e => e.FrameId == root && e.Step == "lose-hp" && e.Subject == 0) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == club && m.Reason.Value == Discard && m.From == CardLocation.Equipment(3)) == 1 &&
            s.E<SkillsAcquiredEvent>().Any(e => e.PlayerSeat == 2 && e.SourceSkillId == Jue && e.SkillIds.Contains(Can)) &&
            s.E<SkillsAcquiredEvent>().Any(e => e.PlayerSeat == 2 && e.SourceSkillId == Jue && e.SkillIds.Contains(Jue)),
            "Both permanent skills are granted to the real beneficiary through the shared mutation helper, while punishment and the paid Club material remain exactly once after cold return.");
    }
    public static void CanyunRemembersPaidTargetsOnItsActualSourceAcrossPhasesCold()
    {
        var s = Scene.Start(); s.Driver("grant-canyun", []); s.Play(0);
        s.StartPulse(Can, [1], [s.V(0).Hand.First().Id]); s.Watch(1); s.Continue(); s.Play(0);
        var first = s.E<MobileJiKangTargetIssuedEvent>().Single(e => e.Source.SkillId == Can);
        s.End(); s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        s.Use(Can); s.Reach(() => s.P?.SkillPrompt?.SkillId == Can && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pick-target"));
        s.Need(s.P!.Choices.All(c => c.Parameters.GetValueOrDefault("branch") != "pick-target" || !c.Targets.Contains(1)) && s.P.Choices.Any(c => c.Targets.SequenceEqual([2])),
            "The next actual Play phase keeps the already-paid target1 excluded only by the genuine acquired Canyun source and permits a different living participant.");
        s.Cold(); s.PickAndPay([2], [s.V(0).Hand.First().Id]); s.Watch(2); s.Continue(); s.Play(0);
        var used = s.E<MobileJiKangTargetIssuedEvent>().Where(e => e.Source.SkillId == Can).ToArray();
        s.Need(used.Length == 2 && used.All(e => e.Source == first.Source) && used.Select(e => e.TargetSeat).SequenceEqual([1, 2]),
            "Canyun pays and records each actual target once on its original acquisition identity, without an owner-wide target flag or a duplicate paid use after cold recovery.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.MobileJiKang is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public bool Has(int seat, string skill) => V(seat).Skills!.Any(s => s.ContentId == skill);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Use(string skill) => Accept(new UseProgramSkillCommand(0, skill, "equipment-comparison", [], [], G.Revision, P!.PromptId));
        public void StartPulse(string skill, int[] targets, int[] cards) { Use(skill); PickAndPay(targets, cards); }
        public void PickAndPay(int[] targets, int[] cards)
        {
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pick-target") == true);
            foreach (var target in targets) Answer(c => c.Parameters.GetValueOrDefault("branch") == "pick-target" && c.Targets.SequenceEqual([target]));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "targets-done");
            foreach (var card in cards) Answer(c => c.Parameters.GetValueOrDefault("branch") == "pick-card" && c.Cards.SequenceEqual([card]));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "cards-done");
        }
        public void Driver(string activation, int[] targets) => Accept(new UseProgramSkillCommand(0, MobileJiKangChecks.Driver, activation, [], targets, G.Revision, P!.PromptId));
        public void Equip(int target, int count) { for (var i = 0; i < 16 && V(target).Equipment.Count < count; i++) { Driver("equip", [target]); Play(0); } Need(V(target).Equipment.Count == count, "The fixed real equipment pool reaches its bounded native setup population."); }
        public void Watch(int seat)
        {
            bool ActualChild() => P?.SkillPrompt?.SkillId == MobileJiKangChecks.Watch && P.PlayerSeat == seat && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(() => ActualChild() || P is { Kind: DecisionKind.PlayCard } || P?.SkillPrompt?.SkillId == Jue && P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "recipient"));
            Need(ActualChild(), "The required real equipment/Draw child was bypassed before the next native action or bequest recipient choice.");
        }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() { var p = P!; Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual mobile Ji Kang command."); }
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 320 && !ready(); step++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, General(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } discard) { Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision)); continue; }
                if (P is { } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") ??
                        prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == "continue") ??
                        prompt.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? prompt.Choices.First();
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded actual mobile Ji Kang native boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))) && G.CardMovements.SequenceEqual(copy.CardMovements),
                "Cold recovery preserves the exact original paid frame, equipment movement, native health/Draw children and permanent grants."); G = copy;
        }
        public void Need(bool ok, string text)
        {
            if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new
            {
                P, Turn = G.State.TurnNumber, Current = G.State.CurrentSeat,
                Players = Enumerable.Range(0, 4).Select(seat => new { Seat = seat, V(seat).GeneralId, V(seat).IsAlive, V(seat).Hp, Equipment = V(seat).Equipment.Count }).ToArray(),
                Receipts = G.ResolutionStack.OfType<ProgramSkillFrame>().Where(f => f.MobileJiKang is not null).Select(f => new { f.Id, f.SkillId, f.WindowContext, f.MobileJiKang }).ToArray(),
                Facts = G.Events.Select(e => e.Payload).Where(e => e is MobileJiKangIssuedEvent or MobileJiKangProgressEvent or ProgramBindingResolvedEvent || e.GetType().Name is "DamageAppliedEvent" or "PlayerDiedEvent").TakeLast(12).Select(e => JsonSerializer.Serialize(e, e.GetType())).ToArray()
            }));
        }
        public static Scene Start(bool death = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(death));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry), registry);
            s.Accept(new StartGameCommand()); s.Play(0); return s;
        }
    }
    private static string General(int seat) => "fixture:mobile-ji-kang-general-" + seat;
    private sealed class Fixture(bool death) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-ji-kang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:ji-kang");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"retain-fixture-hands","query":"handLimit","operation":"add","value":20,"priority":0}],"activations":[
                {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"gear"}]},
                {"id":"hurt","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":1}]},
                {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
                {"id":"grant-canyun","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"grantSkills","target":"owner","skillIds":["mobile:canyun"]}]}
              ]},
              {"id":"{{Watch}}","revision":1,"triggers":[
                {"id":"native-equal-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.mobile-ji-kang.equal-draw"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"equal-pause","options":[{"id":"continue"}]}]},
                {"id":"native-equipment-discard-child","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementReasons":["{{Discard}}"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"equipment-pause","options":[{"id":"continue"}]}]}
              ]}
            ]}
            """;
            var names = new Dictionary<string, object> { [Driver] = new { name = "原生嵇康准备", description = "真实装备、损血、击杀与共享授技。" },
                [Watch] = new { name = "原生嵇康子窗", description = "付款或摸牌的真实子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            for (var seat = 0; seat < 4; seat++)
            {
                var hero = seat == (death ? 1 : 0); var extra = new List<string> { Watch }; if (seat == 0) extra.Add(Driver); if (hero) extra.Add(Jue);
                b.AddGeneral(new(General(seat), "固定真实参与者", "supporter", hero ? Qing : "standard:none", hero ? "wei" : "shu", hero && !death ? 2 : death && seat == 0 ? 8 : 4, extra));
            }
            const string deck = "fixture:mobile-ji-kang-deck";
            var cards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2))
                .Concat(Enumerable.Range(0, 12).Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 1)))
                .Concat(Enumerable.Range(0, 12).Select(_ => new ContentDeckPhysicalCard("standard:renwang_shield", Suit.Club, 2))).ToArray();
            b.AddDeck(new(deck, "固定真实牌与两类装备", 8, 0, []) { PhysicalCards = cards });
            b.AddMode(new(Mode, "移动嵇康真实结算", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
