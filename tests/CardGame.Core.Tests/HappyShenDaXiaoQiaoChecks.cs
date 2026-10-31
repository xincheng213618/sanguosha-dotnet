using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

public static class HappyShenDaXiaoQiaoChecks
{
    private const string Hero = "happy:shen-da-qiao-xiao-qiao", Peer = "fixture:happy-qiao-peer", Driver = "fixture:happy-qiao-driver",
        NoDraw = "fixture:happy-qiao-no-draw", Quiet = "fixture:happy-qiao-quiet", Deck = "fixture:happy-qiao-deck", GainHold = "fixture:happy-qiao-gain",
        MaximumCounter = "fixture:happy-qiao-maximum-counter", MaximumHold = "fixture:happy-qiao-maximum-hold";

    public static void WushuangFactsPreserveUndefinedSisterHpAndRejectScalarLimitCold()
    {
        var s = Scene.Start(); s.Play();
        s.Use("damage", [0]); s.PoolBoundary(); s.Pool(HappyQiaoPool.XiaoQiao); s.Play();
        s.Need(s.Health == new HappyQiaoDualHealth(3, 2, 3), "A real native damage receipt creates different sister pools.");
        var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
        IWushuangModeHost Host() => (IWushuangModeHost)s.G.ComposeNativeWushuangModeHost()!;
        void CheckFactsAndLimit()
        {
            var host = Host(); var facts = host.Players.ToArray();
            s.Need(facts.Length == 4 && facts[0].Hp is null && facts.Skip(1).All(p => p.Hp is not null) &&
                facts[0].IsAlive && facts[0].Seat == 0,
                "Pure mode identity/shop facts can traverse unequal pools, preserving undefined HP instead of choosing a sister or aggregate.");
            var featureType = System.Reflection.Assembly.Load("CardGame.Modes.Wushuang")
                .GetType("CardGame.Modes.Wushuang.WushuangModeFeature", throwOnError: true)!;
            var feature = Activator.CreateInstance(featureType, flags, null, [17, s.Definitions, host], null)!;
            var revision = s.G.Revision; var ended = s.E<WushuangClockExpiredEvent>().Length; var rejected = false;
            try { featureType.GetMethod("CompleteAtLimit", flags)!.Invoke(feature, ["undefined-health legacy mode", true]); }
            catch (System.Reflection.TargetInvocationException exception) when (exception.InnerException is InvalidOperationException error &&
                error.Message.Contains("one defined current HP", StringComparison.Ordinal)) { rejected = true; }
            s.Need(rejected && s.G.Revision == revision && s.G.State.Winner == Winner.None && s.E<WushuangClockExpiredEvent>().Length == ended &&
                s.Health == new HappyQiaoDualHealth(3, 2, 3),
                "An unsupported scalar survival limit rejects before scoring, retiring frames, publishing a winner or changing either pool.");
        }
        CheckFactsAndLimit(); s.Cold(); CheckFactsAndLimit();
    }

    public static void ExpressionQuantitySelectsItsOriginalHealthPoolAndReturnsColdOnce()
    {
        foreach (var pool in Enum.GetValues<HappyQiaoPool>())
        {
            var s = Scene.Start(); s.Play();
            s.Use("damage", [0]); s.PoolBoundary(); s.Pool(HappyQiaoPool.XiaoQiao); s.Play();
            s.Need(s.Health == new HappyQiaoDualHealth(3, 2, 3), "The quantity query really has two different current HP values.");
            if (pool == HappyQiaoPool.DaQiao)
            {
                var player = ((IReadOnlyList<CharacterState>)global::FixtureRuntimeAccess.Engine.GetField("_players",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.GetValue(s.G)!)[0];
                var world = new SkillStateConditionWorld(new(0, 0), player, health: s.G.HealthQueriesEntries);
                using var condition = JsonDocument.Parse("[\"==\",[\"hp\",\"owner\"],3]");
                var expression = ProgramExpressions.Parse(condition.RootElement, "fixture.while");
                var reads = new List<(int? Hp, int? LostHp, bool? Active)>();
                s.G.HealthQueriesEntries.HappyQiaoActionQueryUnion(() =>
                {
                    reads.Add((world.Stat("hp", 0), world.Stat("lostHp", 0), ProgramExpressions.EvaluateBool(expression, world)));
                    return Array.Empty<LegalAction>();
                });
                s.Need(reads.SequenceEqual(new (int?, int?, bool?)[] { (3, 0, true), (2, 1, false) }),
                    "A continuous condition reuses the current pool selected by the existing health query, including its lost HP value.");
            }
            var hand = s.V.HandCount; var expected = s.Health.Hp(pool);
            var previousQueries = s.E<HappyQiaoHealthRequestedEvent>().Count(e => e.ReturnKind == HappyQiaoHealthReturnKind.Query);
            s.Use("draw-current-hp", []); s.PoolBoundary();
            var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Driver);
            var frameId = parent.Id;
            s.Need(s.E<ProgramAmountEvaluatedEvent>().All(e => e.FrameId != frameId) &&
                s.E<HappyQiaoHealthRequestedEvent>().Count(e => e.ReturnKind == HappyQiaoHealthReturnKind.Query) == previousQueries + 1,
                "A live HP amount requests the original owning pool before issuing any quantity or drawing cards.");
            s.Cold(); s.Pool(pool); s.Play();
            s.Need(s.V.HandCount == hand + expected && s.Health == new HappyQiaoDualHealth(3, 2, 3) &&
                s.E<ProgramAmountEvaluatedEvent>().Where(e => e.FrameId == frameId).Select(e => e.Amount).SequenceEqual([expected]) &&
                s.E<HappyQiaoHealthRequestedEvent>().Count(e => e.ReturnKind == HappyQiaoHealthReturnKind.Query) == previousQueries + 1 &&
                s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == frameId && e.Completed) == 1,
                "Choosing either real pool yields its exact current HP once, preserving both pools and the single paid program after cold return.");
            s.Cold();
        }
    }

    private sealed class Fixture(bool fullSkills = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-happy-qiao-health", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            if (fullSkills) CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{NoDraw}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{GainHold}}","revision":1,"triggers":[{"id":"gain","window":"cardsGained","subject":"owner","optional":true,"isSkillInvocation":false,"destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill.happy.shutu.chosen-red"],"effects":[{"op":"setChainedState","target":"owner","chained":true}]}]},
              {"id":"{{MaximumCounter}}","revision":1,"cardPolicies":[{"id":"native-first-round-slash-maximum","kind":"firstRoundCardDamageBecomesMaximumHpLoss","cardKinds":["slash"]}],"triggers":[{"id":"real-other-card","window":"playEnding","subject":"owner","turnOwnerScope":"otherLiving","optional":false,"isSkillInvocation":false,"effects":[{"op":"useVirtualSlash","target":"owner","targetRef":{"kind":"eventTarget"} }]}]},
              {"id":"{{MaximumHold}}","revision":1,"triggers":[{"id":"actual-pool-maximum","window":"afterHealthChanged","subject":"owner","condition":{"kind":"healthChangeKindIs","hpChangeKind":"maximumHp"},"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"maximum","options":[{"id":"continue"}]}]}]},
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                {"id":"recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"owner","amount":1}]},
                {"id":"recover-to-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recoverTo","target":"owner","numberExpression":"integerConstant","minimumValue":2,"clampToMaxHp":true}]},
                {"id":"draw-current-hp","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amountExpr":["hp","owner"],"onUnknownAmount":"cancelBinding"}]},
                {"id":"loss-three","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":3}]},
                {"id":"discard-two","minCards":2,"maxCards":2,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":2}]}
              ]}
            ]}
            """;
            var presentations = new[] { NoDraw, Quiet, Driver, GainHold, MaximumCounter, MaximumHold }.ToDictionary(id => id, id => new { name = id, description = "真实双池原生健康与三技能原帧子返回",
                optionLabels = id == MaximumHold ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() });
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = p, ProgramPresentation = catalog.Presentations[id] });
            if (!fullSkills) b.AddGeneral(new(Hero, "双池原生健康角色", "", NoDraw, "god", 3, [Driver]) { Gender = GeneralGender.Female });
            b.AddGeneral(new(Peer, "安静健康旁观角色", "", NoDraw, "wei", 12, [Quiet]));
            b.AddDeck(new(Deck, "固定实体牌", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard("standard:slash", fullSkills && i >= 40 ? i % 2 == 0 ? Suit.Heart : Suit.Diamond : Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "欢乐双池健康小局", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public ContentRegistry Definitions => registry;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public HappyQiaoDualHealth Health => V.HappyQiaoHealth!;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Use(string id, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Menu(HappyQiaoStage stage) => Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "happy-qiao-skills") == true &&
            G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.HappyQiao is not null)?.HappyQiao?.Stage == stage);
        public void ActivateShutu()
        {
            Reach(() => P?.SkillPrompt?.SkillId == "happy:shutu" && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        }
        public void Card(int id, LegalActionKind kind, int? target = null)
        {
            var action = G.GetHumanLegalActions().Single(a => a.CardId == id && a.Kind == kind && a.TargetSeats.SequenceEqual(target is { } seat ? [seat] : Array.Empty<int>()));
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
            { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void PoolBoundary() => Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "happy-qiao-health") == true);
        public void Pool(HappyQiaoPool pool) => Answer(c => c.Parameters.GetValueOrDefault("pool") == pool.ToString());
        public void Peach() => Answer(c => c.Cards.Count == 1 && V.Hand.Any(card => card.Id == c.Cards[0] && card.Kind == CardKind.Peach));
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing exact dual-pool health choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var n = 0; n < 100; n++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 }) Need(false, "Unexpected Qiao health boundary " + JsonSerializer.Serialize(new
                {
                    P, Qualified = E<HappyQiaoRedHealthQualifiedEvent>(), Started = E<HappyQiaoDamageHealthStartedEvent>(),
                    Returned = E<HappyQiaoDamageHealthReturnedEvent>(), NativeIssued = E<HappyQiaoUseIssuedEvent>(), NativeReturned = E<HappyQiaoUseReturnedEvent>(),
                    Actions = E<CardActionAcceptedEvent>().Select(e => e.Action).Where(a => a.ActorSeat == 0)
                        .Select(a => new { a.ActionId, a.Type, a.EffectiveKind, a.EffectiveSuit, a.EffectiveIsRed }),
                    Damage = E<DamageAppliedEvent>()
                }));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Missed bounded Qiao health flow.");
        }
        public void Cold()
        {
            var views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray();
            var frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(views.SequenceEqual(Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i)))) &&
                frames.SequenceEqual(G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))), "Cold preserves both pools, original selection invoice and same-pool dying cursor.");
        }
        public void Accept(GameCommand c)
        { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected dual-pool health command."); }
        public void Need(bool ok, string why)
        { if (!ok) throw new InvalidOperationException(why + JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, P, Health = V.HappyQiaoHealth, Facts = G.Events.TakeLast(12).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) })); }
        public static Scene Start(bool fullSkills = false, bool maximum = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(fullSkills));
            var peer = new ShanheCombatantDefinition(Peer, Hp: 12) { StartingHand = fullSkills ? [new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Dodge, Suit.Club, 3)] : [] };
            var duelPartner = fullSkills ? peer with { StartingHand = [.. peer.StartingHand ?? [], new(CardKind.Slash, Suit.Club, 8)] } : peer;
            if (maximum) duelPartner = duelPartner with { ExtraSkillIds = [MaximumCounter] };
            var setup = new ShanheBattleSetup(47, 1, "happy-qiao-health", Hero, 3, 0, 0, 0, 0, [],
                fullSkills ? [new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Slash, Suit.Diamond, 6), new(CardKind.Slash, Suit.Club, 7), new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Diamond, 4), new(CardKind.Duel, Suit.Club, 9)] :
                    [new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Peach, Suit.Heart, 4), new(CardKind.Peach, Suit.Heart, 6)], [], [peer, peer], [duelPartner], new string('a', 64))
            { DefaultGodFactionId = "wu" };
            if (fullSkills) setup = setup with { ExtraSkillIds = [NoDraw, Driver, GainHold] };
            if (maximum) setup = setup with { ExtraSkillIds = [MaximumHold], StartingCards = [new(CardKind.Dodge, Suit.Club, 2)] };
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
}
