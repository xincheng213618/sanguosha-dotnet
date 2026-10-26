using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileDianWeiChecks
{
    private const string Hero = "mobile:dian-wei", Skill = "mobile:qiangxi", Owner = "fixture:ordinary-mobile-dian-wei-owner";
    private const string Peer = "fixture:ordinary-mobile-dian-wei-peer-", Watch = "fixture:ordinary-mobile-dian-wei-watch";
    private const string Extra = "fixture:ordinary-mobile-dian-wei-extra", Driver = "fixture:ordinary-mobile-dian-wei-driver";
    private const string Mode = "identity:classic-ordinary-mobile-dian-wei", CostReason = "skill-program.mobile:qiangxi.DiscardSelected";
    private const string Weapon = "discard-weapon-and-damage", Hp = "lose-hp-and-damage";

    public static void WeaponCostTargetPairsPublicationSubmissionAndPaidCold()
    {
        var s = Scene.Start(); s.Play();
        var turn = s.G.State.TurnNumber;
        var equipped = s.V(0).Hand.Single(c => c.Kind == CardKind.QinggangSword).Id;
        var hand = s.V(0).Hand.Single(c => c.Kind == CardKind.Crossbow).Id;
        s.Submit(new PlayCardCommand(0, equipped, [], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.Zone(equipped) == CardLocation.Equipment(0) && s.G.GetCombatDistance(0, 2) == 2 && s.G.GetAttackRange(0) == 2,
            "The real physical equipped sword supplies the ordinary distant-target range.");
        var action = s.G.GetHumanActionView().LegalActions.Single(a => a.ProgramSkillId == Skill && a.ProgramActivationId == Weapon);
        s.Need(action.SelectableCardTargets is { } pairs && pairs.Contains(new(hand, 2)) && pairs.Contains(new(equipped, 1)) &&
            !pairs.Contains(new(equipped, 2)),
            "Published weapon/target pairs preserve the equipped sword for a hand cost and exclude its range when it is itself the cost.");
        var before = s.Dump();
        var rejected = s.G.Submit(new UseProgramSkillCommand(0, Skill, Weapon, [equipped], [2], s.G.Revision, s.P!.PromptId));
        s.Need(!rejected.Accepted && before == s.Dump(),
            "Submitting the unpublished equipped-weapon/distant-target pair rejects atomically before payment or quota consumption.");

        var distantHp = s.V(2).Hp;
        s.Use(Weapon, [hand], 2); s.Reach(() => s.Watching("hand-cost"));
        var first = s.Root!;
        s.Need(first.MobileOrdinaryQiangxi is { WeaponCardId: var card, WeaponFrom: var from, RangeWithoutCost: 2 } && card == hand && from == CardLocation.Hand(0) &&
            first.SelectedCardPayment is { MovementCommitted: true } && s.V(2).Hp == distantHp && s.Zone(equipped) == CardLocation.Equipment(0),
            "The hand weapon pays natively on its original frame while the equipped sword's frozen range remains available.");
        s.Continue(); s.Play();
        s.Need(s.V(2).Hp == distantHp - 1 && s.Zone(hand) == CardLocation.DiscardPile && s.G.GetAttackRange(0) == 2 &&
            !s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill),
            "One real distant damage follows the hand cost, and both payment branches share the once-per-Play quota.");

        s.Submit(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.G.State.TurnNumber == turn, "The next real Play returns within the same actual turn.");
        action = s.G.GetHumanActionView().LegalActions.Single(a => a.ProgramSkillId == Skill && a.ProgramActivationId == Weapon);
        s.Need(action.SelectableCardTargets is { } secondPairs && secondPairs.Contains(new(equipped, 1)) && !secondPairs.Contains(new(equipped, 2)),
            "The fresh phase quota retains the exact post-cost range constraint for the equipment weapon.");
        var adjacentHp = s.V(1).Hp;
        s.Use(Weapon, [equipped], 1); s.Reach(() => s.Watching("equipment-cost"));
        var second = s.Root!; var frameId = second.Id;
        s.Need(second.MobileOrdinaryQiangxi is { WeaponCardId: var secondCard, WeaponFrom: var secondFrom, RangeWithoutCost: 1 } &&
            secondCard == equipped && secondFrom == CardLocation.Equipment(0) && s.Zone(equipped) == CardLocation.DiscardPile &&
            second.SelectedCardPayment is { InstructionIndex: 1, MovementCommitted: true } && s.V(1).Hp == adjacentHp,
            "Discarding the genuine equipped weapon freezes bare range and pauses its paid movement child before damage.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.V(1).Hp == adjacentHp - 1 && s.G.GetAttackRange(0) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == equipped && m.From == CardLocation.Equipment(0) && m.Reason.Value == CostReason) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == hand && m.From == CardLocation.Hand(0) && m.Reason.Value == CostReason) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == frameId && e.Completed) == 1 &&
            !s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill),
            "Cold returns to the exact original equipped-cost frame, pays once and deals one damage without renewing the shared quota.");
    }

    public static void LastHpNativeDyingDeathThenOriginalDamageCold()
    {
        var s = Scene.Start(hp: true); s.Play();
        s.Need(s.V(0).Hp > 1 && s.V(0).Role != Role.Lord, "The ordinary identity fixture starts alive above one HP without a Lord HP bonus.");
        while (s.V(0).Hp > 1)
        {
            s.Submit(new UseProgramSkillCommand(0, Driver, "lose-one", [], [], s.G.Revision, s.P!.PromptId)); s.Play();
        }
        var targetHp = s.V(1).Hp;
        s.Use(Hp, [], 1);
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RescueDying });
        var parent = s.Root!; var frameId = parent.Id;
        s.Need(parent.MobileOrdinaryQiangxi is { HpBefore: 1, WeaponCardId: null } && parent.InstructionIndex == 1 &&
            s.V(0).Hp == 0 && s.V(1).Hp == targetHp && s.G.ResolutionStack.OfType<DyingFrame>().Any() &&
            s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "peach" && c.Cards.Count == 1) &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.FrameId == frameId && e.Amount == 1 && e.RemainingHp == 0) == 1 &&
            !s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0),
            "The last-HP cost enters the genuine native rescue window before any target damage; it is HP loss, not self damage.");
        s.Cold(); s.Leave();
        s.Reach(() => s.E<ProgramSkillResolvedEvent>().Any(e => e.FrameId == frameId && e.Completed));
        var facts = s.G.Events.Select(e => e.Payload).ToArray();
        var death = Array.FindIndex(facts, e => e is PlayerDiedEvent { VictimSeat: 0 });
        var hit = Array.FindIndex(facts, e => e is DamageAppliedEvent { TargetSeat: 1, Amount: 1 });
        s.Need(!s.V(0).IsAlive && s.G.State.Winner == Winner.None && death >= 0 && hit > death && s.V(1).Hp == targetHp - 1 &&
            s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 1) == 1 &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.FrameId == frameId && e.Amount == 1) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == frameId && e.Completed) == 1,
            "With no rescue and no game winner, the actual death resolves first, then the already-paid original Qiangxi tail deals its single damage.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public bool Watching(string id) => P?.SkillPrompt?.SkillId == Watch && G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.TriggerId == id;
        public void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Ordinary mobile Dian Wei command rejected.");
        }
        public void Use(string activation, IReadOnlyList<int> cards, int target) =>
            Submit(new UseProgramSkillCommand(0, Skill, activation, cards, [target], G.Revision, P!.PromptId));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Leave() => Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die");
        private void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "Missing native prompt choice.");
            Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 200; step++)
            {
                if (done()) return;
                var p = P;
                if (p is { Kind: DecisionKind.SelectGeneral }) Submit(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer + p.PlayerSeat, G.Revision, p.PromptId));
                else if (p is { Kind: DecisionKind.DiscardCards }) Submit(new DiscardCardsCommand(p.PlayerSeat, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, G.Revision));
                else if (p is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } && V(p.PlayerSeat).IsHuman) Submit(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId));
                else if (p is { Kind: DecisionKind.RescueDying } && V(p.PlayerSeat).IsHuman) Leave();
                else if (p is not null && V(p.PlayerSeat).IsHuman && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue")) Continue();
                else if (p is not null && V(p.PlayerSeat).IsHuman && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The bounded actual ordinary mobile Dian Wei flow missed its named boundary.");
        }
        public string Dump() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves all four views, the original paid native cost/dying frame, exact scalar range/source receipt and entity movements.");
        }
        public void Need(bool ok, string reason)
        {
            if (!ok) throw new InvalidOperationException(reason + " " + JsonSerializer.Serialize(new
            {
                G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner,
                Pending = P is { } p ? new { p.Kind, p.PlayerSeat, SkillId = p.SkillPrompt?.SkillId } : null,
                Hp = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).IsAlive, V(i).Role }),
                Frames = G.ResolutionStack.Select(f => new { f.Id, Kind = f.GetType().Name, Receipt = (f as ProgramSkillFrame)?.MobileOrdinaryQiangxi }),
                NativeHp = E<ProgramSkillHpLostEvent>(), Dying = G.ResolutionStack.OfType<DyingFrame>().Select(f => new { f.Id, f.VictimSeat, f.ResponderIndex }),
                RecentCommands = G.AcceptedCommands.TakeLast(8).Select(c => c.GetType().Name),
                Movements = G.CardMovements.TakeLast(8)
            }));
        }
        public static Scene Start(bool hp = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(hp));
            GameOptions options;
            if (hp)
                options = new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Rebel, ModeId = Mode,
                    UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 12 };
            else
            {
                var peer = new ShanheCombatantDefinition(Peer + 1, Hp: 8) { StartingHand = [] };
                var setup = new ShanheBattleSetup(37, 1, "ordinary-mobile-dian-wei-native-pairs", Hero, 4, 0, 0, 0, 0, [Watch, Extra],
                    [new(CardKind.QinggangSword, Suit.Spade, 6), new(CardKind.Crossbow, Suit.Club, 1)], [], [peer, peer], [peer], new string('d', 64));
                options = new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                    HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                    UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 };
            }
            var game = GameEngine.CreateStandard(options, registry); var scene = new Scene(registry, game);
            scene.Submit(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(bool hp) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-dian-wei", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var definitions = new List<JsonNode>
            {
                JsonNode.Parse("""
                {"id":"fixture:ordinary-mobile-dian-wei-watch","revision":1,"triggers":[
                  {"id":"hand-cost","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile:qiangxi.DiscardSelected"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"hand-paid","options":[{"id":"continue"}]}]},
                  {"id":"equipment-cost","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile:qiangxi.DiscardSelected"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"equipment-paid","options":[{"id":"continue"}]}]},
                  {"id":"hp-cost","window":"afterHpLost","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"hp-paid","options":[{"id":"continue"}]}]}
                ]}
                """)!,
                JsonNode.Parse("""
                {"id":"fixture:ordinary-mobile-dian-wei-extra","revision":1,"triggers":[{"id":"real-inserted-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]}
                """)!,
                JsonNode.Parse("""
                {"id":"fixture:ordinary-mobile-dian-wei-driver","revision":1,"activations":[{"id":"lose-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]}
                """)!
            };
            var labels = definitions.ToDictionary(n => n["id"]!.GetValue<string>(), n => (object)(n["id"]!.GetValue<string>() == Watch
                ? new Dictionary<string, object> { ["name"] = "真实付款子窗", ["description"] = "原生HE实体费用及HP费用", ["optionLabels"] = new Dictionary<string, string> { ["continue"] = "继续" } }
                : new Dictionary<string, object> { ["name"] = "真实阶段及体力准备", ["description"] = "原生额外Play与逐点失去体力" }));
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "普通移动典韦原生边界", "supporter", Skill, "wei", 4, [Watch, Driver], GeneralGender.Male));
            for (var i = 1; i < 4; i++) builder.AddGeneral(new(Peer + i, "固定其他角色", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:ordinary-mobile-dian-wei-deck";
            builder.AddDeck(new(deck, "固定原生响应实体", hp ? 5 : 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard(hp ? "standard:peach" : "standard:dodge", Suit.Heart, 7)).ToArray() });
            if (hp) builder.AddMode(new(Mode, "普通强袭原生濒死与死亡尾部", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 1 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer + 1, Peer + 2, Peer + 3]));
            else builder.AddMode(new(ShanheBattleSetup.BattleModeId, "普通强袭实体武器费用与真实阶段", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Peer + 1], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
