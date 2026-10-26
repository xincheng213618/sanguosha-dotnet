using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSkillStockChecks
{
    private const string Enyuan = "classic:enyuan", Wangxi = "classic:wangxi";
    private const string Owner = "fixture:skill-stock-owner", Enemy = "fixture:skill-stock-enemy";
    private const string Driver = "fixture:skill-stock-driver", Quiet = "fixture:skill-stock-quiet", Deck = "fixture:skill-stock-deck";
    private static readonly Lazy<ContentRegistry> BattleRegistry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardActiveSkillExpansionPackage(true), new StandardRescueSkillExpansionPackage(),
        new StandardClassicGeneralPackage(), new Fixture()));

    public static void BoughtEnyuanAndWangxiUsePerPointPaymentsAndRealCardGains()
    {
        var catalog = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var enyuan = catalog.Rewards.Single(reward => reward.Id == "build-skill-3087");
        var wangxi = catalog.Rewards.Single(reward => reward.Id == "build-skill-3120");
        Need(enyuan.SkillId == Enyuan && wangxi.SkillId == Wangxi,
            "The actual stocked sources must grant the complete ordinary Enyuan and Wangxi versions.");
        var definition = new ShanheCampaignDefinition("fixture:skill-stock", "购入技能实战", [Owner], ["shop"],
        [
            new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("enyuan", enyuan.Name, GoldCost: 300, RewardId: enyuan.Id, LeavesNode: false),
                 new("wangxi", wangxi.Name, GoldCost: 600, RewardId: wangxi.Id, LeavesNode: false)]),
            new("battle", "守军", ShanheNodeKind.Battle, [],
                [new("guard", "固定守军", [new(Enemy) { StartingHand =
                    [new(CardKind.Dodge, Suit.Spade, 2)] }])])
        ], [enyuan, wangxi], StartingGold: 2000, ExtraSkillSlots: 2);
        var session = ShanheCampaignSession.Create(definition,
            new(83, Owner) { UseCodexRewardPool = true, UnlockedRewardIds = [enyuan.Id, wangxi.Id] }, _ => BattleRegistry.Value);
        Accept(session.Submit(new MoveShanheNodeCommand("shop", session.Revision)));
        var first = session.Snapshot.ShopItems.Single(item => item.Reward!.Id == enyuan.Id);
        Need(first.GoldCost == 300, "The first actual Enyuan purchase pays its source price.");
        Accept(session.Submit(new ChooseShanheNodeOptionCommand(first.ChoiceId, null, session.Revision)));
        var second = session.Snapshot.ShopItems.Single(item => item.Reward!.Id == wangxi.Id);
        Need(second.GoldCost == 700, "The second actual skill purchase adds the existing hundred-copper category surcharge.");
        Accept(session.Submit(new ChooseShanheNodeOptionCommand(second.ChoiceId, null, session.Revision)));
        Need(session.Snapshot.Gold == 1000 && session.Snapshot.ExtraSkillSlots == 2 &&
            session.Snapshot.ExtraSkillIds.SequenceEqual([Enyuan, Wangxi]),
            "Both paid stock skills fill the two real extra slots without granting additional capacity.");
        Accept(session.Submit(new ChooseShanheNodeOptionCommand("$leave", null, session.Revision)));
        Accept(session.Submit(new MoveShanheNodeCommand("battle", session.Revision)));
        var scene = Scene.Start(session.PendingBattleSetup!);
        Need(scene.View(0).Skills!.Any(skill => skill.ContentId == Enyuan) &&
            scene.View(0).Skills!.Any(skill => skill.ContentId == Wangxi),
            "The purchased extra skills become actual native skill instances in the next campaign battle.");

        var ownHp = scene.View(0).Hp;
        var enemyHp = scene.View(1).Hp;
        var ownHand = scene.View(0).HandCount;
        var enemyHand = scene.View(1).HandCount;
        scene.Incoming(acceptRepayment: true);
        var gift = scene.G.CardMovements.Where(move => move.Reason.Value == "skill-program." + Enyuan + ".SelectAndMoveOwnedCard").ToArray();
        Need(scene.Facts<DamageAppliedEvent>() is [{ SourceSeat: 1, TargetSeat: 0, Amount: 2 }] &&
            scene.Resolved(Enyuan, "repay-each-damage") == 2 && scene.Resolved(Wangxi, "after-taking-damage") == 0 &&
            scene.RepaymentOffers == 2 && scene.WangxiOffers == 2 &&
            gift.Length == 2 && gift[0].CardId == gift[1].CardId &&
            gift[0].From == CardLocation.Hand(1) && gift[0].To == CardLocation.Processing &&
            gift[1].From == CardLocation.Processing && gift[1].To == CardLocation.Hand(0) &&
            scene.G.CreateCardZoneDiagnostics().Single(card => card.CardId == gift[0].CardId).CardKind == CardKind.Dodge &&
            scene.View(0).Hand.Single(card => card.Id == gift[0].CardId).Suit != Suit.Heart &&
            scene.Facts<ProgramSkillHpLostEvent>().Count(fact => fact.SkillId == Enyuan && fact.TargetSeat == 1 && fact.Amount == 1) == 1 &&
            scene.View(0).Hp == ownHp - 2 && scene.View(1).Hp == enemyHp - 1 &&
            scene.Draws(Wangxi, 0) == 0 && scene.Draws(Wangxi, 1) == 0 &&
            scene.View(0).HandCount == ownHand + 1 && scene.View(1).HandCount == enemyHand - 1,
            "One actual two-point hit offers both bought skills per point: with Wangxi declined, the ordinary NPC gives its only non-heart hand card to the first accepted Enyuan, then loses one HP when the second point finds no card. " +
            JsonSerializer.Serialize(new { scene.RepaymentOffers, scene.WangxiOffers, gift,
                Enyuan = scene.Resolved(Enyuan, "repay-each-damage"), Wangxi = scene.Resolved(Wangxi, "after-taking-damage"),
                OwnHp = scene.View(0).Hp, EnemyHp = scene.View(1).Hp, OwnHand = scene.View(0).HandCount,
                EnemyHand = scene.View(1).HandCount, DrawOwn = scene.Draws(Wangxi, 0), DrawOther = scene.Draws(Wangxi, 1),
                Damage = scene.Facts<DamageAppliedEvent>(), Loss = scene.Facts<ProgramSkillHpLostEvent>() }));

        scene.Incoming(acceptRepayment: false);
        Need(scene.RepaymentOffers == 4 && scene.WangxiOffers == 4 &&
            scene.Resolved(Enyuan, "repay-each-damage") == 2 && scene.Resolved(Wangxi, "after-taking-damage") == 2 &&
            scene.Facts<DamageAppliedEvent>().Select(fact => (fact.SourceSeat, fact.TargetSeat, fact.Amount))
                .SequenceEqual([(1, 0, 2), (1, 0, 2)]) &&
            scene.View(0).Hp == ownHp - 4 && scene.View(1).Hp == enemyHp - 1 &&
            scene.Draws(Wangxi, 0) == 2 && scene.Draws(Wangxi, 1) == 2 &&
            scene.G.CardMovements.Where(move => move.Reason.Value == "skill-program." + Wangxi + ".Draw")
                .Select(move => move.CardId).Distinct().Count() == 4 &&
            scene.View(0).HandCount == ownHand + 3 && scene.View(1).HandCount == enemyHand + 1,
            "A second real two-point hit independently offers both skills again: declining Enyuan pays nothing, while accepting both Wangxi points draws two distinct actual cards for each participant.");

        var before = scene.G.CardMovements.Count;
        var sourceHand = scene.View(1).HandCount;
        scene.Use("take-two"); scene.Play();
        var taken = scene.G.CardMovements.Skip(before)
            .Where(move => move.Reason.Value == "skill-program." + Driver + ".take-selected-target-cards").ToArray();
        Need(taken.Length == 2 && taken.Select(move => move.CardId).Distinct().Count() == 2 &&
            taken.All(move => move.From == CardLocation.Hand(1) && move.To == CardLocation.Hand(0)) &&
            scene.Resolved(Enyuan, "reward-card-giver") == 1 && scene.Draws(Enyuan, 1) == 1 && scene.Draws(Enyuan, 0) == 0 &&
            scene.View(1).HandCount == sourceHand - 1 && scene.View(0).HandCount == ownHand + 5 &&
            scene.Facts<DamageAppliedEvent>().Length == 2,
            "Obtaining two real cards from one other owner in the actual atomic batch offers the bought Enyuan once and pays exactly one native Draw1 back to that source, without inventing another damage or repayment.");
    }

    private static void Accept(ShanheCampaignResult result) => Need(result.Accepted, result.Error ?? "Rejected real skill-stock campaign command.");
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => Enumerable.Range(0, 2).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        private bool _acceptRepayment;
        public int RepaymentOffers { get; private set; }
        public int WangxiOffers { get; private set; }
        public PlayerSnapshot View(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] Facts<T>() => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public int Resolved(string skill, string binding) => Facts<ProgramBindingResolvedEvent>()
            .Count(fact => fact.SkillId == skill && fact.BindingId == binding && fact.Activated && fact.Completed);
        public int Draws(string skill, int seat) => G.CardMovements.Count(move => move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(seat) && move.Reason.Value == "skill-program." + skill + ".Draw");
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual skill-stock battle command."); }
        public void Use(string activation) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], [1], G.Revision, P!.PromptId));
        public void Incoming(bool acceptRepayment) { _acceptRepayment = acceptRepayment; Use("incoming"); Play(); }
        public void Play()
        {
            for (var step = 0; step < 220; step++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
                    if (choice is not null)
                    {
                        var activate = true;
                        if (choice.Parameters.GetValueOrDefault("skill-id") == Enyuan &&
                            choice.Parameters.GetValueOrDefault("binding-id") == "repay-each-damage")
                        { RepaymentOffers++; activate = _acceptRepayment; }
                        else if (choice.Parameters.GetValueOrDefault("skill-id") == Wangxi &&
                            choice.Parameters.GetValueOrDefault("binding-id") == "after-taking-damage")
                        { WangxiOffers++; activate = !_acceptRepayment; }
                        if (!activate) choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault("program-action") == "skip");
                    }
                    choice ??= prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") is
                        "select-and-move-owned-card" or "take-other-card" or "select-target");
                    Need(choice is not null, "Unexpected skill-stock prompt: " + JsonSerializer.Serialize(prompt));
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded real skill-stock play boundary: " + JsonSerializer.Serialize(P));
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, BattleRegistry.Value));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-skill-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "activations":[
                    {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":2}]},
                    {"id":"take-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"takeSelectedTargetCards","target":"selectedTarget","amount":2}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "购入技能原生驱动", description = "两点实际伤害与两张原始实体获得" } } }));
            builder.AddSkill(new(Driver, "购入技能原生驱动", "实际伤害与获得牌")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddSkill(new(Quiet, "普通守军", "没有额外能力"));
            builder.AddGeneral(new(Owner, "技能购入者", "supporter", Driver, "qun", 12));
            builder.AddGeneral(new(Enemy, "守军", "supporter", Quiet, "wei", 12));
            builder.AddDeck(new(Deck, "零常规摸牌实体牌堆", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "购入技能真实流程", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
