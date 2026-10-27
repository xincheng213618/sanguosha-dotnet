using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class GhostRuleChecks
{
    private const string GuanpingMoyan = StandardShanheGuanpingCompanionPackage.MoyanSkillId;
    private const string GuanpingJudgmentReason = "shanhe-guanping-moyan";
    private const string GuanpingActor = "fixture:guanping-moyan-actor", GuanpingFriend = "fixture:guanping-moyan-friend";
    private const string GuanpingEnemy = "fixture:guanping-moyan-enemy", GuanpingFireEnemy = "fixture:guanping-moyan-fire-enemy";
    private const string LongyinPayment = "skill-program.classic:longyin.SelectAndMoveOwnedCard";

    public static void MandatoryGuanpingMoyanReturnsToPaidLongyinCold()
    {
        foreach (var (suit, lethal) in new[] { (Suit.Heart, false), (Suit.Spade, false), (Suit.Heart, true) })
        {
            var (g, registry) = StartGuanpingMoyan(suit, lethal: lethal);
            Require(V(g, 2) is { GeneralId: GuanpingEnemy, Hp: 10 } && V(g, 3).GeneralId == GuanpingFireEnemy &&
                V(g, 3).Hp == (lethal ? 1 : 10), "The distinct original-Slash and fire-damage targets start with their authored native HP.");
            var cost = V(g, 1).Hand.Single().Id;
            var material = UseCard(g, CardKind.Slash, 2);
            MoyanUntil(ref g, () => P(g)?.SkillPrompt?.SkillId == "classic:longyin");
            Answer(g, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            MoyanUntil(ref g, () => P(g)?.SkillPrompt?.SkillId == "classic:longyin" && P(g)!.Choices.Any(choice => choice.Cards.SequenceEqual([cost])));
            ColdGuanpingMoyan(ref g, registry);
            Answer(g, choice => choice.Cards.SequenceEqual([cost]));
            MoyanUntil(ref g, () => MoyanJudgments(g).Length == 1);
            Require(MoyanPayments(g).Length == 1 && MoyanPayments(g).Single().CardId == cost &&
                V(g, 2).Hp == (suit == Suit.Heart ? 10 : 9) && E<ProgramBindingStartedEvent>(g).Count(item => item.SkillId == GuanpingMoyan) == 1 &&
                g.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == "classic:longyin") == (suit == Suit.Heart),
                "The real payment invokes mandatory Moyan once: red pauses on its target with the original Longyin frame, while black may finish and return to the original Slash synchronously. " +
                JsonSerializer.Serialize(new { lethal, suit, cost, Payments = MoyanPayments(g).Select(move => move.CardId), TargetHp = V(g, 2).Hp,
                    MoyanBindings = E<ProgramBindingStartedEvent>(g).Count(item => item.SkillId == GuanpingMoyan),
                    Programs = g.ResolutionStack.OfType<ProgramSkillFrame>().Select(frame => new { frame.Id, frame.SkillId, frame.OwnerSeat, frame.InstructionIndex }) }));
            ColdGuanpingMoyan(ref g, registry);
            ResolveMoyanToPlay(ref g, registry, suit, expectedJudgments: 1, fireTarget: 3);
            Require(V(g, 2).Hp == 9 && V(g, 3).Hp == (suit == Suit.Heart ? lethal ? 0 : 9 : 10) && V(g, 3).IsAlive != lethal &&
                MoyanPayments(g).Length == 1 && E<CardUseDebitRefundedEvent>(g).Count() == 1 &&
                E<ProgramBindingResolvedEvent>(g).Count(item => item.SkillId == "classic:longyin" && item.Activated && item.Completed) == 1 &&
                E<ProgramBindingResolvedEvent>(g).Count(item => item.SkillId == GuanpingMoyan && item.Completed) == 1 &&
                g.CardMovements.Count(move => move.CardId == material && move.To == CardLocation.DiscardPile) == 1,
                "Cold resumes return through one Longyin payment/refund and one original Slash; only the red mandatory judgment adds its separate fire damage.");
            var bound = E<ProgramBindingStartedEvent>(g).Single(item => item.SkillId == GuanpingMoyan);
            Require(g.Events.Where(item => item.Payload is CardMovedEvent move && move.CardId == cost && move.Reason.Value == LongyinPayment).Single().Sequence <
                g.Events.Single(item => item.Payload is JudgmentRequestedEvent judgment && judgment.Reason == GuanpingJudgmentReason).Sequence &&
                bound.OwnerSeat == 1,
                "The mandatory child is attributable to the actual teammate's already-paid card, not to the active actor or a second cost.");
            if (lethal)
            {
                Require(E<PlayerDyingEvent>(g).Any(item => item.VictimSeat == 3 && item.KillerSeat == 1) &&
                    g.State.Winner == Winner.None && V(g, 2).IsAlive,
                    "Lethal Moyan completes the enemy's nested dying/death before returning to the original Slash against the surviving enemy.");
                ColdGuanpingMoyan(ref g, registry);
                continue;
            }

            // The same production owner then loses one real Hand and one Equipment
            // entity in a single native movement batch; it must judge once, not per zone.
            Use(g, "equip", 1); MoyanUntil(ref g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            var equipment = V(g, 1).Equipment.Single().Id;
            var hand = V(g, 1).Hand.First().Id;
            Use(g, "discard-mixed", 1);
            MoyanUntil(ref g, () => P(g)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
            Answer(g, choice => choice.Cards.SequenceEqual([equipment]));
            Answer(g, choice => choice.Cards.SequenceEqual([hand]));
            MoyanUntil(ref g, () => MoyanJudgments(g).Length == 2);
            ColdGuanpingMoyan(ref g, registry);
            ResolveMoyanToPlay(ref g, registry, suit, expectedJudgments: 2, fireTarget: 3);
            var mixed = g.CardMovements.Where(move => move.Reason.Value == "skill-program." + Driver + ".MoveBoundCards" && move.From.OwnerSeat == 1).ToArray();
            Require(mixed.Length == 2 && mixed.Select(move => move.From.Zone).ToHashSet().SetEquals([CardZoneKind.Hand, CardZoneKind.Equipment]) &&
                mixed.All(move => move.To == CardLocation.DiscardPile) && MoyanJudgments(g).Length == 2 &&
                E<ProgramBindingStartedEvent>(g).Count(item => item.SkillId == GuanpingMoyan) == 2,
                "A single actual mixed HE loss batch owns one mandatory judgment, independently of the earlier Longyin batch.");
        }

        var (own, ownRegistry) = StartGuanpingMoyan(Suit.Heart, ownerIsActor: true);
        Use(own, "equip", 0); MoyanUntil(ref own, () => P(own) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var ownEquipment = V(own, 0).Equipment.Single().Id;
        var ownHand = V(own, 0).Hand.First().Id;
        Use(own, "discard-self-mixed", 0);
        MoyanUntil(ref own, () => P(own)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        Answer(own, choice => choice.Cards.SequenceEqual([ownEquipment]));
        Answer(own, choice => choice.Cards.SequenceEqual([ownHand]));
        MoyanUntil(ref own, () => P(own) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Require(MoyanJudgments(own).Length == 0 && !E<ProgramBindingStartedEvent>(own).Any(item => item.SkillId == GuanpingMoyan) &&
            own.CardMovements.Count(move => move.From.OwnerSeat == 0 && move.To == CardLocation.DiscardPile &&
                move.Reason.Value == "skill-program." + Driver + ".MoveBoundCards") == 2,
            "The same native mixed loss during Guan Ping's own turn does not invoke Moyan or make a judgment.");
        ColdGuanpingMoyan(ref own, ownRegistry);
    }

    private static JudgmentRequestedEvent[] MoyanJudgments(GameEngine game) => E<JudgmentRequestedEvent>(game)
        .Where(judgment => judgment.Reason == GuanpingJudgmentReason).ToArray();
    private static CardMovementRecord[] MoyanPayments(GameEngine game) => game.CardMovements.Where(move => move.Reason.Value == LongyinPayment).ToArray();

    private static void MoyanUntil(ref GameEngine game, Func<bool> condition)
    {
        for (var step = 0; step < 180; step++)
        {
            if (condition()) return;
            var prompt = P(game);
            Require(prompt?.SkillPrompt?.SkillId != GuanpingMoyan || prompt.Choices.All(choice =>
                choice.Parameters.GetValueOrDefault("program-action") == "red-judgment-fire-target"),
                "Mandatory Moyan never exposes an activate, decline or pass choice.");
            Require(prompt is not { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The expected Guan Ping child cannot be skipped by returning to ordinary Play.");
            if (prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die") == true)
                Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The native Guan Ping child did not reach its bounded checkpoint: " + JsonSerializer.Serialize(P(game)));
    }

    private static void ResolveMoyanToPlay(ref GameEngine game, ContentRegistry registry, Suit suit, int expectedJudgments, int fireTarget)
    {
        var local = game;
        MoyanUntil(ref local, () => P(local)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "red-judgment-fire-target") == true ||
            P(local) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        if (suit == Suit.Heart)
        {
            Require(P(local)?.SkillPrompt?.SkillId == GuanpingMoyan && P(local)!.Choices.All(choice => choice.Targets.Count == 1 &&
                choice.Targets[0] != 1 && choice.Parameters.GetValueOrDefault("program-action") == "red-judgment-fire-target"),
                "A red mandatory judgment asks only which other living character takes its fire damage.");
            ColdGuanpingMoyan(ref local, registry);
            var lethal = V(local, fireTarget).Hp == 1;
            Answer(local, choice => choice.Targets.SequenceEqual([fireTarget]));
            if (lethal)
            {
                MoyanUntil(ref local, () => P(local) is { PlayerSeat: 0, Kind: DecisionKind.RescueDying });
                Require(local.ResolutionStack.OfType<DyingFrame>().Any(frame => frame.VictimSeat == fireTarget) &&
                    local.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == GuanpingMoyan && frame.AttackAttempt is not null) &&
                    local.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == "classic:longyin"),
                    "The real nested lethal damage retains both the paid Longyin parent and the original Moyan damage owner while rescue is pending.");
                ColdGuanpingMoyan(ref local, registry);
                Answer(local, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
            }
        }
        MoyanUntil(ref local, () => P(local) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Require(MoyanJudgments(local).Length == expectedJudgments && E<DamageAppliedEvent>(local)
            .Count(damage => damage.SourceSeat == 1 && damage.Nature == DamageNature.Fire && damage.Amount == 1) ==
                (suit == Suit.Heart ? expectedJudgments : 0),
            "Each red result deals exactly one fire damage and each black result causes no fire damage.");
        game = local;
    }

    private static void ColdGuanpingMoyan(ref GameEngine game, ContentRegistry registry)
    {
        var checkpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), registry);
        Require(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == checkpoint &&
            game.CardMovements.SequenceEqual(restored.CardMovements) && JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
            "Cold recovery preserves the paid Longyin frame, its native parent Use, the mandatory judgment and physical card invoice.");
        game = restored;
    }

    private static (GameEngine, ContentRegistry) StartGuanpingMoyan(Suit suit, bool ownerIsActor = false, bool lethal = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardShanheGuanpingCompanionPackage(), new GuanpingMoyanFixture(suit, lethal));
        IReadOnlyList<ShanheStartingCard> cards = lethal ? [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Peach, Suit.Heart, 3)] : [new(CardKind.Slash, Suit.Heart, 7)];
        var setup = new ShanheBattleSetup(17, 1, "mandatory-moyan", ownerIsActor ? StandardShanheGuanpingCompanionPackage.EliteId : GuanpingActor,
            4, 0, 0, 0, 0, [Driver], cards, [],
            [new(GuanpingEnemy) { StartingHand = [] }, new(GuanpingFireEnemy) { StartingHand = [] }],
            [new(ownerIsActor ? GuanpingFriend : StandardShanheGuanpingCompanionPackage.EliteId) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 4)] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 3, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        Accept(game, new StartGameCommand());
        var local = game; MoyanUntil(ref local, () => P(local) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        return (local, registry);
    }

    private sealed class GuanpingMoyanFixture(Suit suit, bool lethal) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-guanping-moyan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                 "modifiers":[{"id":"range","query":"attackRange","operation":"add","value":5,"priority":0}],"activations":[
                  {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                   "effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipment"}]},
                  {"id":"discard-mixed","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"selectOwnedCards","target":"selectedTarget","amount":2,"zones":["hand","equipment"],"resultBind":"mixed"},
                              {"op":"moveBoundCards","target":"owner","sourceBind":"mixed","destination":"discardPile","awaitMovementTriggers":true}]},
                  {"id":"discard-self-mixed","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                   "effects":[{"op":"selectOwnedCards","target":"owner","amount":2,"zones":["hand","equipment"],"resultBind":"mixed"},
                              {"op":"moveBoundCards","target":"owner","sourceBind":"mixed","destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "实际魔炎场景", description = "原生装备与混合失牌" } } }));
            builder.AddSkill(new(Driver, "实际魔炎场景", "原生装备与混合失牌") { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(GuanpingActor, "出杀队友", "", null, "shu", 4));
            builder.AddGeneral(new(GuanpingFriend, "同队角色", "", null, "shu", 4));
            builder.AddGeneral(new(GuanpingEnemy, "敌方角色", "", null, "wei", 10));
            builder.AddGeneral(new(GuanpingFireEnemy, "魔炎目标", "", null, "wei", lethal ? 1 : 10));
            builder.AddDeck(new("fixture:guanping-moyan-deck", "同色真实判定", 4, 0, [])
            { PhysicalCards = Enumerable.Range(0, 140).Select(index => new ContentDeckPhysicalCard(index < 120 ? "standard:dodge" : "standard:crossbow", suit, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "关平真实龙吟与魔炎", 4, 4, new Dictionary<string, int>(), "fixture:guanping-moyan-deck",
                GeneralPoolIds: [GuanpingActor, GuanpingFriend, GuanpingEnemy, GuanpingFireEnemy, StandardShanheGuanpingCompanionPackage.EliteId], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
