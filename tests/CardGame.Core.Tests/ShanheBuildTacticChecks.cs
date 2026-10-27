using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBuildTacticChecks
{
    private const string Owner = "fixture:build-tactic-owner", Driver = "fixture:build-tactic-driver";
    private const string Quiet = "fixture:build-tactic-quiet", Deck = "fixture:build-tactic-deck";
    private const string JudgmentProbe = "fixture:build-tactic-judgment-probe";
    private const string Cuixue = StandardShanheCampaignCatalog.CurrentCuixue2SkillId;
    private const string Sougua = StandardShanheCampaignCatalog.CurrentSougua3SkillId;
    private const string Fenjin = StandardShanheCampaignCatalog.CurrentFenjinSkillId;
    private const string Yuanzhu = StandardShanheCampaignCatalog.CurrentYuanzhu3SkillId;

    public static void TigerWineFiniteOrdinaryQuotaConsumesRealUsesAndRecoversCold()
    {
        foreach (var invalid in new[] { "{\"id\":\"quota\",\"kind\":\"additionalAlcoholUses\",\"value\":0,\"cardKinds\":[\"alcohol\"]}",
                     "{\"id\":\"quota\",\"kind\":\"additionalAlcoholUses\",\"value\":2,\"cardKinds\":[\"slash\"]}" })
        {
            var rejected = false;
            try { _ = SkillProgramCatalog.Load($$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:wine-shape","revision":1,"cardPolicies":[{{invalid}}]}]}""",
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { ["fixture:wine-shape"] = new { name = "酒次数", description = "静态形状" } } })); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "The shared finite wine policy rejects a non-positive quota or unrelated static qualifiers in the loader.");
        }
        foreach (var (skill, extra) in new[] { (StandardShanheCampaignCatalog.CurrentHugujiuSkillId, 1),
                     (StandardShanheCampaignCatalog.CurrentHugujiu2SkillId, 2) })
        {
            var cards = Enumerable.Range(0, extra + 2).Select(_ => new ShanheStartingCard(CardKind.Alcohol, Suit.Spade, 3))
                .Concat(Enumerable.Range(0, extra + 1).Select(_ => new ShanheStartingCard(CardKind.Slash, Suit.Heart, 7))).ToArray();
            var s = Scene.Start(false, 10, [skill], cards, slashBonus: 5);
            for (var use = 0; use < 1 + extra; use++)
            {
                s.Wine(s.V().Hand.First(card => card.Kind == CardKind.Alcohol).Id);
                s.Slash(s.V().Hand.First(card => card.Kind == CardKind.Slash).Id, 3);
                if (use == 0) s = s.Cold();
            }
            var blocked = s.V().Hand.Single(card => card.Kind == CardKind.Alcohol).Id;
            var revision = s.G.Revision;
            var rejected = s.G.Submit(new PlayCardCommand(0, blocked, [], revision, s.G.PendingDecision!.PromptId));
            Need(!s.G.GetHumanLegalActions().Any(action => action.CardId == blocked && action.Kind == LegalActionKind.Alcohol) &&
                !rejected.Accepted && s.G.Revision == revision && s.V().Hand.Any(card => card.Id == blocked) &&
                s.E<JianjiOrdinaryAlcoholPaidEvent>().Count(fact => fact.ActorSeat == 0 && fact.CountsQuota) == 1 + extra,
                "Tiger wine grants exactly its finite extra uses; cold replay retains actual paid uses and a rejected excess cannot consume another card.");
            s.NextOwnPlay();
            s.Wine(blocked);
            Need(s.E<JianjiOrdinaryAlcoholPaidEvent>().Last().ActualTurn == s.G.State.TurnNumber,
                "A new actual turn restores the finite ordinary wine allowance.");
        }
    }

    public static void FirstRoundSlashDrawAndReflectionKillsPayNativeRecovery()
    {
        var s = Scene.Start(logistics: false, hp: 4,
            [Cuixue, Sougua, StandardShanheCampaignCatalog.CurrentTouxiSkillId,
                StandardShanheCampaignCatalog.CurrentJingjijiaSkillId]);
        var slash = s.V().Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var fire = s.V().Hand.Single(card => card.Kind == CardKind.FireSlash).Id;
        var thunder = s.V().Hand.Single(card => card.Kind == CardKind.ThunderSlash).Id;
        var initialHand = s.V().HandCount;
        s.Slash(slash, 1);
        Need(s.DrawCount(Cuixue) == 2 && s.V().HandCount == initialHand + 1,
            "The first genuinely damaging Slash pays one native Draw2, after its real entity leaves hand.");
        s.Slash(fire, 1);
        Need(!s.G.CreateSnapshot(0).Players[1].IsAlive && s.V().Hp == 8 && s.DrawCount(Cuixue) == 2 &&
            s.E<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.Amount == 4) == 1,
            "A second elemental Slash shares the same round draw quota; its real kill instead pays Recover4.");

        s.Use("lose-six", []);
        Need(s.V().Hp == 2, "A real six-HP loss prepares the reflection sustain boundary without manufacturing damage.");
        s.Use("incoming-one", [2]);
        Need(s.V().Hp == 5 && !s.G.CreateSnapshot(0).Players[2].IsAlive &&
            s.E<DamageAppliedEvent>().Any(fact => fact.SourceSeat == 0 && fact.TargetSeat == 2 && fact.Amount == 1 && fact.SourceCardUseResolutionId is null) &&
            s.E<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.Amount == 4) == 2 && s.DrawCount(Cuixue) == 2,
            "Native received damage reflects once, that skill damage really kills its source, and the killer recovers four without pretending the reflection was a Slash.");
        s.NextOwnPlay();
        s.Slash(thunder, 3);
        Need(s.DrawCount(Cuixue) == 4 && s.E<SkillUsageConsumedEvent>().Count(fact => fact.SkillId == Cuixue && fact.SkillOwnerSeat == 0) == 2,
            "A later genuine round restores the shared Slash damage reward once.");
    }

    public static void LogisticsKeepStartHpThresholdDrawPhaseAndEndingPaymentsSeparate()
    {
        var s = Scene.Start(logistics: true, hp: 3,
            [Fenjin, Yuanzhu, StandardShanheCampaignCatalog.CurrentMuniuLiumaSkillId,
                StandardShanheCampaignCatalog.CurrentJingjijiaSkillId]);
        Need(s.V().Hp == 2 && s.V().HandLimit == 1 && s.V().HandCount == 6 &&
            s.DrawCount(Fenjin) == 2 && s.NormalDrawCount() == 4 && !s.E<DamageAppliedEvent>().Any(),
            "At three HP, Fenjin really loses one HP and draws two; Muniu only raises the normal phase draw from two to four and reduces capacity by one, without triggering reflection.");
        s.NextOwnPlay();
        Need(s.V().Hp == 2 && s.V().HandLimit == 1 && s.V().HandCount == 8 &&
            s.DrawCount(Fenjin) == 2 && s.DrawCount(Yuanzhu) == 3 && s.NormalDrawCount() == 8 &&
            s.E<ProgramSkillHpLostEvent>().Count(fact => fact.SkillId == Fenjin && fact.Amount == 1) == 1,
            "The real discard leaves one card, own ending draws three, enemy endings pay nothing, and the next four-card draw occurs with Fenjin disabled at exactly two HP.");
    }

    public static void FenjinSecondTierAndQinmianUseNativeHpAndJudgmentBoundariesForward()
    {
        const string fenjin2 = StandardShanheCampaignCatalog.CurrentFenjin2SkillId;
        const string qinmian = StandardShanheCampaignCatalog.CurrentQinmianSkillId;
        var control = Scene.Start(logistics: true, hp: 4, [fenjin2], startingCards: [], observeJudgment: true);
        Need(control.V().Hp == 4 && control.DrawCount(fenjin2) == 0 &&
            control.DrawCount(JudgmentProbe) == 1 && control.NormalDrawCount() == 2 &&
            !control.E<ProgramSkillHpLostEvent>().Any(fact => fact.SkillId == fenjin2) && !control.E<DamageAppliedEvent>().Any(),
            "At exactly four HP, Fenjin II does not activate; the real judgment boundary pays the probe's one card before the unchanged two-card normal draw.");

        var active = Scene.Start(logistics: true, hp: 5, [fenjin2, qinmian], startingCards: [], observeJudgment: true);
        Need(active.V().Hp == 4 && active.V().HandCount == 3 && active.DrawCount(fenjin2) == 1 &&
            active.DrawCount(JudgmentProbe) == 0 && active.NormalDrawCount() == 2 &&
            active.E<ProgramSkillHpLostEvent>().Where(fact => fact.SkillId == fenjin2)
                .Select(fact => fact.Amount).SequenceEqual([1]) && !active.E<DamageAppliedEvent>().Any(),
            "At five HP, Fenjin II really loses exactly one HP and draws one card; Qinmian suppresses the judgment probe while preserving both normal draw cards, without creating damage.");

        control.NextOwnPlay();
        active.NextOwnPlay();
        Need(control.V().Hp == 4 && control.DrawCount(fenjin2) == 0 &&
            control.DrawCount(JudgmentProbe) == 2 && control.NormalDrawCount() == 4 &&
            !control.E<ProgramSkillHpLostEvent>().Any(fact => fact.SkillId == fenjin2) && !control.E<DamageAppliedEvent>().Any(),
            "A second actual turn enters judgment and normal draw again, but Fenjin II still does not activate at the four-HP boundary.");
        Need(active.V().Hp == 4 && active.DrawCount(fenjin2) == 1 && active.DrawCount(JudgmentProbe) == 0 &&
            active.NormalDrawCount() == 4 && active.E<ProgramSkillHpLostEvent>().Where(fact => fact.SkillId == fenjin2)
                .Select(fact => fact.Amount).SequenceEqual([1]) && !active.E<DamageAppliedEvent>().Any(),
            "After five HP became four, the next actual turn neither repeats Fenjin II's loss nor draws another bonus card; Qinmian again skips judgment without skipping the normal draw.");
    }

    public static void PurchasedTiersEnterNativeOpeningArmorAndPhysique()
    {
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var ids = new[] { "tactic-build-2056", "tactic-build-2073", "tactic-build-2070" };
        var rewards = ids.Select(id => current.Rewards.Single(reward => reward.Id == id)).ToArray();
        const string buyer = "classic:cao-cao";
        var definition = new ShanheCampaignDefinition("fixture:armor-stock", "真实购买与开局", [buyer], ["shop"],
            [new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices: rewards.Select(reward =>
                new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name,
                    StandardShanheCampaignCatalog.ShopMetadataByRewardId[reward.Id].Price,
                    RewardId: reward.Id, LeavesNode: false)).ToArray()),
             new("battle", "步卒", ShanheNodeKind.Battle, [],
                [new("guard", "步卒", [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }])])],
            rewards, StartingGold: 3000);
        var session = ShanheCampaignSession.Create(definition, new(83, buyer) { UseCodexRewardPool = true },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        Pay(new MoveShanheNodeCommand("shop", session.Revision));
        foreach (var (id, price) in ids.Zip(new[] { 700, 600, 700 }))
        {
            var item = session.Snapshot.ShopItems.Single(item => item.Reward?.Id == id);
            Need(item.GoldCost == price && !item.RequiresSkillReplacement,
                "Three real tactic tiers retain source prices plus the paid tactic-category surcharge without occupying extra skill slots.");
            Pay(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision));
        }
        Need(session.Snapshot.Gold == 1000 && session.Snapshot.TacticIds.ToHashSet().SetEquals(ids) &&
            session.Snapshot.ExtraSkillIds.Count == 0 && StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[102] == ids[0] &&
            !StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(current).Contains(ids[0]) &&
            StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(current).Contains(ids[1]),
            "The fixed 102 reward requires its real research binding; a random-node source tier is explicit default inventory, not a fabricated fixed node.");
        Pay(new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Pay(new MoveShanheNodeCommand("battle", session.Revision));
        var game = StandardShanheCampaignCatalog.CreateBattle(session.PendingBattleSetup!);
        Submit(new StartGameCommand());
        for (var step = 0; step < 100 && game.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
            Submit(new AdvanceOneStepCommand(game.Revision));
        var owner = game.CreateSnapshot(0).Players[0];
        Need(game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && owner.MaxHp == 7 && owner.Hp == 7 && owner.Armor == 6 &&
            game.Events.Select(item => item.Payload).OfType<ArmorGrantedEvent>().Where(fact => fact.PlayerSeat == 0)
                .Select(fact => fact.Amount).Order().SequenceEqual([2, 4]) &&
            game.Events.Select(item => item.Payload).OfType<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.Amount == 3) == 1,
            "The actual purchased opening tier grants four armor, the first real round grants two more, and physique increases maximum HP before its separate real three-point recovery.");
        return;

        void Pay(ShanheCampaignCommand command)
        { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Actual stock purchase was rejected."); }
        void Submit(GameCommand command)
        { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Actual opening command was rejected."); }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private static string Peer(int seat) => "fixture:build-tactic-peer-" + seat;

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] E<T>() => G.Events.Select(fact => fact.Payload).OfType<T>().ToArray();
        public int DrawCount(string skillId) => G.CardMovements.Count(move => move.To == CardLocation.Hand(0) &&
            move.Reason.Value == "skill-program." + skillId + ".Draw");
        public int NormalDrawCount() => G.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw);
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native build-tactic command."); }
        private void Until(Func<bool> condition)
        {
            for (var step = 0; step < 300; step++)
            {
                if (condition()) return;
                if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices[0].Id, G.Revision));
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native build-tactic boundary.");
        }
        private void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Slash(int cardId, int target)
        {
            var action = G.GetHumanLegalActions().Single(candidate => candidate.CardId == cardId &&
                candidate.TargetSeats.SequenceEqual([target]) && candidate.Kind == LegalActionKind.Slash);
            Accept(new PlayCardCommand(0, cardId, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void Wine(int cardId)
        {
            var action = G.GetHumanLegalActions().Single(candidate => candidate.CardId == cardId && candidate.Kind == LegalActionKind.Alcohol);
            Accept(new PlayCardCommand(0, cardId, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public Scene Cold()
        {
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(G.CreateSnapshot(0)), "Cold wine quota retains the actual prepared player view.");
            return new(restored, registry);
        }
        public void Use(string activation, int[] targets)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, G.Revision, P!.PromptId)); Play(); }
        public void NextOwnPlay()
        {
            var turn = G.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
            Until(() => G.State.TurnNumber > turn && P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public static Scene Start(bool logistics, int hp, string[] tactics, IReadOnlyList<ShanheStartingCard>? startingCards = null,
            int slashBonus = 0, bool observeJudgment = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheLegendaryTacticsPackage(),
                new StandardShanheBuildTacticsPackage(), new StandardShanheWineTacticsPackage(), new Fixture(logistics));
            var setup = new ShanheBattleSetup(83, 1, "fixture:build-tactics", Owner, hp, 0, 0, 0, slashBonus,
                observeJudgment ? [JudgmentProbe] : [],
                startingCards ?? (logistics ? [] : [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.FireSlash, Suit.Spade, 8), new(CardKind.ThunderSlash, Suit.Spade, 9)]), [],
                [new(Peer(1), Hp: 2) { StartingHand = [] }, new(Peer(2), Hp: 1) { StartingHand = [] }, new(Peer(3)) { StartingHand = [] }],
                [], new string('b', 64)) { TacticSkillIds = tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(bool logistics) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-build-tactics", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"lose-six","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":6}]},
                    {"id":"incoming-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{JudgmentProbe}}","revision":1,"triggers":[{"id":"judgment-probe","window":"judgmentPhaseStarting","subject":"owner","turnOwnerScope":"own","optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, JudgmentProbe }.ToDictionary(id => id, _ => new { name = "原生构筑驱动", description = "真实伤害、真实失去体力与真实阶段" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "构筑使用者", "supporter", Driver, "qun", 10));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new(Peer(seat), "固定敌军", "supporter", Quiet, "wei", 16));
            builder.AddDeck(new(Deck, "固定实体闪", 0, logistics ? 2 : 0, [])
            { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "战法构筑实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
