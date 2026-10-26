using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSchoolDrawChecks
{
    private const string Owner = "fixture:school-draw-owner", Peer = "fixture:school-draw-peer";
    private const string Driver = "fixture:school-draw-driver", Quiet = "fixture:school-draw-quiet";
    private const string Deck = "fixture:school-draw-deck", Component = "fixture:school-draw-component";
    private const string Threshold = "shanhe:current-shenlong-baiwei-2104";

    public static void InitialAndPaidCompletionRefillWithRealDrawsAndNativeChildren()
    {
        foreach (var initiallyComplete in new[] { true, false })
        {
            var s = Scene.Start(initiallyComplete);
            Need(s.View().HandCount == (initiallyComplete ? 10 : 6),
                "A carried completed draw school refills the opening hand to eight before the ordinary two-card Draw phase; an incomplete school does not.", s.Details());
            if (!initiallyComplete)
            {
                var before = s.Game.PendingDecision!;
                var item = s.Game.CreateShanheMarketSnapshot(0)!.ShopItems.Single(i => i.Reward?.Id == Component);
                s.Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, s.Game.Revision));
                s.Play();
                Need(s.View().HandCount == 8 && s.Game.CreateShanheMarketSnapshot(0)!.Gold == 400 &&
                    s.Events<ShanheMarketPurchaseCommittedEvent>().Length == 1 &&
                    s.Events<ShanheMarketPurchaseAppliedEvent>().Length == 1 &&
                    s.Game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } after && after.PromptId != before.PromptId,
                    "Paying the missing real component activates the native opening refill immediately and returns once to the original Play flow.");
            }
            Need(s.View().Skills!.Count(skill => skill.ContentId == ShanheSchoolEffects.DrawSkillId) == 1 &&
                s.Game.CreateShanheMarketSnapshot(0)!.ExtraSkillIds.Count == 0,
                "The completed school grants one derived tactic without occupying an ordinary purchased-skill slot.");
            s.Discard(3); s.Discard(1); s.Discard(1);
            Need(s.View() is { HandCount: 8, Armor: 3 } &&
                s.Events<ProgramSkillResolvedEvent>().Count(e => e.SkillId == Driver && e.Completed) == 3 &&
                s.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == ShanheSchoolEffects.DrawSkillId &&
                    e.BindingId == "hand-loss-refill" && e.Completed && e.Activated) == 3,
                "Each genuine hand-loss batch refills only its deficit, then resumes its original active instruction exactly once.");
            var invoices = s.Events<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat == 0).ToArray();
            Need(invoices.Sum(e => e.ActualCount) == 9 &&
                s.Events<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).ToArray() is [{ TargetSeat: 1, Amount: 1 }] &&
                s.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Threshold && e.Completed && e.Activated) == 1,
                "The refill uses genuine native draws: the ninth actual drawn material triggers exactly one acquired Shenlong Baiwei child.",
                new { initiallyComplete, State = s.Details(), Hits = s.Events<DamageAppliedEvent>(),
                    Threshold = s.Events<NativeDrawThresholdProgressedEvent>() });
            Need(invoices.All(e => s.Game.CardMovements.Where(m => m.Sequence > e.SequenceBefore && m.Sequence <= e.SequenceAfter &&
                    m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason == e.Reason)
                    .Select(m => m.CardId).Distinct().Count() == e.ActualCount) &&
                s.Game.ResolutionStack.All(frame => frame is not ProgramSkillFrame and not DamageFrame),
                "Every counted draw invoice has the exact physical draw-pile materials and all native child frames have returned.");
        }
    }

    public static void UnownedManualGrantCannotActivateTheDrawSchool()
    {
        var s = Scene.Start(false);
        s.Use("grant-refill", []);
        Need(s.View().Skills!.Any(skill => skill.ContentId == ShanheSchoolEffects.DrawSkillId),
            "The negative fixture really grants the same program, rather than merely omitting its registry package.");
        s.Discard(3);
        Need(s.View() is { HandCount: 3, Armor: 1 } &&
            s.Events<ProgramBindingResolvedEvent>().Where(e => e.SkillId == ShanheSchoolEffects.DrawSkillId).All(e => !e.Activated) &&
            s.Events<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat == 0).Sum(e => e.ActualCount) == 2,
            "Possessing the program alone cannot impersonate an actually completed Shanhe school or manufacture refill draws.", s.Details());
    }

    public static void DepletedDeckRefillsOnlyAvailableMaterialsAndReturns()
    {
        var s = Scene.Start(true, 6);
        Need(s.View().HandCount == 6,
            "With only six real entities, opening refill and ordinary draw cannot manufacture an eight-card hand.");
        var before = s.Events<ProgramBindingResolvedEvent>().Length;
        s.Discard(3);
        Need(s.View() is { HandCount: 6, Armor: 1 } &&
            s.Events<ProgramBindingResolvedEvent>().Skip(before).Count(e => e.SkillId == ShanheSchoolEffects.DrawSkillId) == 1 &&
            s.Game.CardMovements.Where(m => m.To == CardLocation.Hand(0)).Select(m => m.CardId).Distinct().Count() == 6 &&
            s.Game.ResolutionStack.All(frame => frame is not ProgramSkillFrame),
            "A finite refill draws only the actual reshuffled three discarded entities, then stops below eight without retrying or retaining a child.");
    }

    public static void SharedHandCountDrawKeepsTheSelectedMaximumCapAndZeroDeficit()
    {
        var s = Scene.Start(false);
        Need(s.View(1) is { MaxHp: 5, HandCount: 0 }, "The shared refill target begins with a real five-HP maximum and no hand cards.");
        s.Use("refill-peer", [], [1]);
        var active = s.Events<ProgramSkillResolvedEvent>().Single(e => e.SkillId == Driver && e.ActivationId == "refill-peer");
        var draws = s.Events<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat == 1).ToArray();
        Need(s.View(1).HandCount == 5 && s.View().Armor == 1 && active.Completed &&
            draws is [{ Requested: 5, ActualCount: 5 } draw] && draw.ParentFrameId == active.FrameId &&
            draw.Reason.Value == "skill-program.hand-count.draw" &&
            s.Game.CardMovements.Count(move => move.Sequence > draw.SequenceBefore && move.Sequence <= draw.SequenceAfter &&
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) && move.Reason == draw.Reason) == 5,
            "The shared selected-target operation caps an eight-card request at the target's real five-HP maximum and pays one exact native invoice.");
        s.Use("refill-peer", [], [1]);
        Need(s.View(1).HandCount == 5 && s.View().Armor == 2 &&
            s.Events<NativeDrawInvocationRecordedEvent>().Count(e => e.OwnerSeat == 1) == 1 &&
            s.Events<ProgramSkillResolvedEvent>().Count(e => e.SkillId == Driver && e.ActivationId == "refill-peer" && e.Completed) == 2 &&
            s.Game.ResolutionStack.All(frame => frame is not ProgramSkillFrame),
            "Repeating a shared refill at its capped target produces no zero-card invoice and resumes the following instruction once without retaining a draw cursor.");
    }

    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public object Details() => new { Owner = View(), Draws = Events<NativeDrawInvocationRecordedEvent>(),
            Bindings = Events<ProgramBindingResolvedEvent>(), Uses = Events<ProgramSkillResolvedEvent>() };
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native school-draw command."); }
        public void Play()
        {
            for (var step = 0; step < 120; step++)
            {
                if (Game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(Game.State.Winner == Winner.None, "School-draw scene ended before returning to its original Play flow.");
                if (Game.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is
                        "pass" or "no-nullification" or "take-damage" or "take-duel-damage") ?? prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The bounded draw-school scene did not return to Play.");
        }
        public void Use(string activation, int[] cards, int[]? targets = null)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, cards, targets ?? [], Game.Revision, Game.PendingDecision!.PromptId)); Play(); }
        public void Discard(int count) => Use(count == 3 ? "discard-three" : "discard-one", View().Hand.Take(count).Select(c => c.Id).ToArray());

        public static Scene Start(bool initiallyComplete, int deckCount = 64)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheMarketPackage(),
                new StandardShanheSchoolDrawPackage(), new StandardShanheDrawThresholdPackage(), new Fixture(deckCount));
            ShanheRewardDefinition[] rewards =
            [
                new(Component, "过牌流真实普通件") { Rarity = 1, CampaignEconomy = new(ReplacedEquipmentGold: 1), SchoolMembership = new(2011, 2, 1) },
                new("threshold", "神龙摆尾") { TacticSkillId = Threshold, Rarity = 3 }
            ];
            var definition = new ShanheCampaignDefinition("fixture:school-draw", "过牌套装实战", [Owner], ["shop"],
                [new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"],
                    Choices: [new("buy-component", "普通件", 100, RewardId: Component, LeavesNode: false)]),
                 new("battle", "真实战斗", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(Peer) { StartingHand = [] }])])],
                rewards, StartingGold: 500)
                { Schools = [new(2, "过牌流", 2231, "手牌少于8时补满8", [1, 0, 0, 0])] };
            var session = ShanheCampaignSession.Create(definition, new(83, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true,
                    EquippedTacticIds = initiallyComplete ? [Component, "threshold"] : ["threshold"] }, _ => registry);
            var entered = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
            Need(entered.Accepted, entered.Error ?? "The actual draw-school campaign did not enter its battle.");
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var s = new Scene(game); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(int deckCount) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-school-draw", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"discard-three","minCards":3,"maxCards":3,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"discardSelected","target":"owner","amount":3},{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"discard-one","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"discardSelected","target":"owner","amount":1},{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"grant-refill","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"grantSkills","target":"owner","skillIds":["{{{ShanheSchoolEffects.DrawSkillId}}}"]}]},
                    {"id":"refill-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"drawToHandCount","target":"selectedTarget","amount":8,"capAtTargetMaxHp":true},{"op":"grantArmor","target":"owner","amount":1}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "真实手牌移动驱动", description = "原生弃牌、摸牌和子流程" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "过牌套装持有者", "supporter", Driver, "shu", 30));
            builder.AddGeneral(new(Peer, "真实敌军", "supporter", Quiet, "wei", 5));
            builder.AddDeck(new(Deck, "有限真实实体", 4, 2, [])
                { PhysicalCards = Enumerable.Range(0, deckCount).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, i % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "过牌流原生战斗", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
