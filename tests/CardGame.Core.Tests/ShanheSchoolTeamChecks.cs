using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSchoolTeamChecks
{
    private const string Owner = "fixture:school-team-owner", Fighter = "fixture:school-team-fighter";
    private const string QuietAlly = "fixture:school-team-quiet-ally", Enemy = "fixture:school-team-enemy";
    private const string Driver = "fixture:school-team-driver", Range = "fixture:school-team-range";
    private const string Quiet = "fixture:school-team-quiet", Health = "fixture:school-team-health", Gain = "fixture:school-team-gain";
    private const string Deck = "fixture:school-team-deck", Ordinary = "team:ordinary-school-team-fixture";
    private const string Component = "fixture:school-team-component", Filler = "fixture:school-team-filler";

    public static void InitialSharingUsesTheRankFloorPreservesWoundsAndDealsRealOpeningCards()
    {
        foreach (var rank in new[] { 1, 4, 5 })
        {
            var s = Scene.Start(rank, true);
            var grade = ShanheGeneralRanks.GetBonuses(Math.Max(3, rank));
            for (var seat = 1; seat <= 3; seat++)
            {
                Need(s.View(seat).MaxHp == 8 + grade.MaxHp && s.View(seat).Hp == 4 + grade.MaxHp &&
                     s.View(seat).HandCount == FixedCount(seat) + 2 + grade.InitialHand,
                    "A completed school shares only the rank privileges, preserving each ally's four-HP wound and its authored opening cards. " + s.Details());
                Need(s.Game.CardMovements.Count(m => m.To == CardLocation.Hand(seat) && m.Reason == CardMoveReasons.InitialDeal) ==
                         FixedCount(seat) + 2 + grade.InitialHand &&
                     s.Events<NativeDrawInvocationRecordedEvent>().All(e => e.OwnerSeat != seat),
                    "Initial rank hand privileges are genuine initial dealing, not extra Draw instructions or a replacement of a fixed opening hand.");
            }
            Need(s.View(1).Hand.Count(c => c.Kind == CardKind.Slash) == 5 && s.View(4) is { MaxHp: 30, Hp: 30, HandCount: 0 } &&
                 s.View(0).MaxHp == 9 + ShanheGeneralRanks.GetBonuses(rank).MaxHp,
                "The fixed five Slashes survive, the enemy gains no rank benefits, and the player's separate starting tactic bonuses remain personal.");
            var firstTurn = s.Events<TurnStartedEvent>().Last().TurnNumber;
            s.NextOwnPlay();
            var allyTurn = s.Events<TurnStartedEvent>().First(e => e.ActorSeat == 1 && e.TurnNumber > firstTurn).TurnNumber;
            Need(s.Game.CardMovements.Count(m => m.TurnNumber == allyTurn && m.From == CardLocation.DrawPile &&
                         m.To == CardLocation.Hand(1) && m.Reason == CardMoveReasons.Draw) == 3 + grade.Draw &&
                 s.Events<CardUseDeclaredEvent>().Count(e => e.SourceSeat == 1 && e.CardKind == CardKind.Slash) == 3 + grade.Slash,
                "The ally's real normal draw and physical Slash quota add its authored bonuses and shared rank exactly once, excluding the player's unrelated +3 draw/+2 Slash build. " + s.Details());
        }
    }

    public static void PaidCompletionInitializesLivingAlliesOnceAndReturnsAfterNativeChildren()
    {
        var s = Scene.Start(4, false);
        var before = Enumerable.Range(1, 3).Select(s.View).ToArray();
        var healthBindings = s.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Health && e.Activated && e.Completed);
        var gainBindings = s.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Gain && e.Activated && e.Completed);
        var movementBefore = s.Game.CardMovements.Last().Sequence;
        var bindingBefore = s.Events<ProgramBindingResolvedEvent>().Length;
        var maximumBefore = s.Events<MaximumHpChangedEvent>().Length;
        var healthApplicationsBefore = s.Events<ShanheAlliedHealthAppliedEvent>().Length;
        var frames = s.Game.ResolutionStack.Select(f => f.Id).ToArray();
        var prompt = s.Game.PendingDecision!;
        s.Buy(Component);
        for (var seat = 1; seat <= 3; seat++)
            Need(s.View(seat).MaxHp == before[seat - 1].MaxHp + 1 && s.View(seat).Hp == before[seat - 1].Hp + 1 &&
                 s.View(seat).HandCount == before[seat - 1].HandCount + 2 && s.View(seat).Armor == before[seat - 1].Armor + 4,
                "A paid fourth-rank school increases each living ally's health and maximum together, draws two, and drains the two distinct health facts plus both actual single-card gain batches. " + s.Details());
        Need(s.Game.CreateShanheMarketSnapshot(0) is { Gold: 400, ExtraSkillSlots: 0, ExtraSkillIds.Count: 0 } &&
             s.Events<ShanheMarketPurchaseCommittedEvent>().Length == 1 && s.Events<ShanheMarketPurchaseAppliedEvent>().Length == 1 &&
             s.Game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } resumed && resumed.PromptId != prompt.PromptId &&
             s.Game.ResolutionStack.Select(f => f.Id).SequenceEqual(frames),
            "The original paid market frame returns to the same Play stack, with one payment and no ordinary learned-skill slot.");
        Need(s.Events<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat is >= 1 and <= 3)
                 .OrderBy(e => e.OwnerSeat).Select(e => (e.OwnerSeat, e.ActualCount)).SequenceEqual([(1, 2), (2, 2), (3, 2)]) &&
             s.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Health && e.Activated && e.Completed) == healthBindings + 6 &&
             s.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Gain && e.Activated && e.Completed) == gainBindings + 6,
            "All three recipients have exact two-card draw invoices, with separate completed children for maximum-HP, HP setting, and each real one-card batch. " + s.Details());
        for (var seat = 1; seat <= 3; seat++)
        {
            var healthChildren = s.Events<ProgramBindingResolvedEvent>().Skip(bindingBefore)
                .Where(e => e.OwnerSeat == seat && e.SkillId == Health && e.Activated && e.Completed).ToArray();
            var gainChildren = s.Events<ProgramBindingResolvedEvent>().Skip(bindingBefore)
                .Where(e => e.OwnerSeat == seat && e.SkillId == Gain && e.Activated && e.Completed).ToArray();
            var movements = s.Game.CardMovements.Where(m => m.Sequence > movementBefore && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat)).ToArray();
            Need(healthChildren.Select(e => e.BindingId).Order(StringComparer.Ordinal).SequenceEqual(["maximum-child", "setting-child"]) &&
                 gainChildren.Length == 2 && gainChildren.Select(e => e.FrameId).Distinct().Count() == 2 &&
                 movements.Length == 2 && movements.Select(m => m.CardId).Distinct().Count() == 2 &&
                 s.Events<MaximumHpChangedEvent>().Skip(maximumBefore).Count(e => e.PlayerSeat == seat && e.Delta == 1 &&
                     e.MaximumHp == before[seat - 1].MaxHp + 1 && e.SkillId == ShanheSchoolEffects.TeamRankSkillId) == 1 &&
                 s.Events<ShanheAlliedHealthAppliedEvent>().Skip(healthApplicationsBefore).Count(e => e.RecipientSeat == seat &&
                     e.MaximumBefore == before[seat - 1].MaxHp && e.MaximumAfter == before[seat - 1].MaxHp + 1 &&
                     e.HpBefore == before[seat - 1].Hp && e.HpAfter == before[seat - 1].Hp + 1) == 1,
                "Each recipient's typed maximum and setting children correspond to the actual health application; its two one-card-condition gain children correspond to two distinct drawn materials. " + s.Details());
        }
        var stable = Enumerable.Range(1, 3).Select(seat => (s.View(seat).Hp, s.View(seat).MaxHp, s.View(seat).HandCount, s.View(seat).Armor)).ToArray();
        s.Buy(Filler);
        Need(Enumerable.Range(1, 3).Select(seat => (s.View(seat).Hp, s.View(seat).MaxHp, s.View(seat).HandCount, s.View(seat).Armor)).SequenceEqual(stable) &&
             s.Game.CreateShanheMarketSnapshot(0)!.Gold == 200 &&
             s.View(0).Skills!.Count(skill => skill.ContentId == ShanheSchoolEffects.TeamRankSkillId) == 1 &&
             s.Events<NativeDrawInvocationRecordedEvent>().Count(e => e.OwnerSeat is >= 1 and <= 3) == 3,
            "Buying another real tactic preserves the established source instead of initializing the rank health or hand benefit again.");
    }

    public static void FifthRankAlliesAndPlayerHaveIndependentExpeditionRebirth()
    {
        var s = Scene.Start(5, true);
        Need(Enumerable.Range(1, 3).All(seat => s.View(seat).Skills!.Count(skill =>
                skill.ContentId == ShanheTeamRankPrograms.SharedRebirthSkillId) == 1),
            "Each actual ally receives exactly one native rank rebirth program.");
        foreach (var beneficiary in new[] { 2, 3, 0 })
        {
            var beneficiaryBefore = s.Game.CreateSnapshot(beneficiary).Players[beneficiary];
            var originalHand = beneficiaryBefore.Hand.Select(c => c.Id).ToArray();
            Need(originalHand.Length == beneficiaryBefore.HandCount,
                "The hand-preservation assertion reads the actual beneficiary's own private snapshot, not another viewer's redacted hand.");
            if (beneficiary != 0) s.Use("fall-target", [beneficiary]);
            else
            {
                s.BeginUse("fall-owner", []); s.RescueOwner();
                var prompt = s.Game.PendingDecision!;
                var choice = prompt.Choices.Single(c => c.Parameters.GetValueOrDefault("response") == "program-trigger" &&
                    c.Parameters.GetValueOrDefault("skill-id") == ShanheGeneralRanks.RebirthSkillId);
                s.Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, s.Game.Revision)); s.Play();
            }
            var usages = s.Events<SkillUsageConsumedEvent>().Where(e => e.SkillOwnerSeat == beneficiary &&
                (ShanheTeamRankPrograms.IsPlayerExpeditionRebirthUsage(e) ||
                 ShanheTeamRankPrograms.IsAllyExpeditionRebirthUsage(s.Setup!, e))).ToArray();
            Need(s.View(beneficiary) is { Hp: 5, IsAlive: true } && s.View(beneficiary).HandCount == originalHand.Length + 5 &&
                 originalHand.All(id => s.Game.CreateSnapshot(beneficiary).Players[beneficiary].Hand.Any(c => c.Id == id)) && usages is [var used] &&
                 used.SkillId == (beneficiary == 0 ? ShanheGeneralRanks.RebirthSkillId : ShanheTeamRankPrograms.SharedRebirthSkillId) &&
                 s.Events<NativeDrawInvocationRecordedEvent>().Count(e => e.OwnerSeat == beneficiary && e.Requested == 5 && e.ParentFrameId is not null) == 1,
                "Ally A, ally B and then the player each use their own real entitlement, recover to five and gain five additional physical cards. " + s.Details());
        }
        Need(s.Events<SkillUsageConsumedEvent>().Count(ShanheTeamRankPrograms.IsPlayerExpeditionRebirthUsage) == 1 &&
             s.Events<SkillUsageConsumedEvent>().Count(e => ShanheTeamRankPrograms.IsAllyExpeditionRebirthUsage(s.Setup!, e)) == 2,
            "The player's usage and the two independently attributed allied usages remain separate expedition facts.");
        s.Use("fall-target", [2]);
        Need(!s.View(2).IsAlive && s.Events<SkillUsageConsumedEvent>().Count(e => e.SkillOwnerSeat == 2 &&
                ShanheTeamRankPrograms.IsAllyExpeditionRebirthUsage(s.Setup!, e)) == 1,
            "An ally that already used its privilege really dies on its next fatal loss.");
        s.BeginUse("fall-owner", []); s.CompleteSpentOwnerDeath();
        Need(!s.View(0).IsAlive && s.Events<PlayerDiedEvent>().Count(e => e.VictimSeat == 0) == 1 &&
             s.Events<SkillUsageConsumedEvent>().Count(ShanheTeamRankPrograms.IsPlayerExpeditionRebirthUsage) == 1,
            "The player's own privilege also cannot be spent a second time.");
        Need(!ShanheTeamRankPrograms.IsAllyExpeditionRebirthUsage(s.Setup!, new(4,
                ShanheTeamRankPrograms.SharedRebirthSkillId, "campaign-rebirth", SkillUsageScope.Game, 1)),
            "An enemy's same-named fake usage cannot spend an allied privilege.");

        var next = Scene.Start(5, false, usedSeats: [2], playerSpent: true);
        next.Buy(Component);
        Need(next.View(0).Skills!.All(skill => skill.ContentId != ShanheGeneralRanks.RebirthSkillId) &&
             next.View(2).Skills!.All(skill => skill.ContentId != ShanheTeamRankPrograms.SharedRebirthSkillId) &&
             new[] { 1, 3 }.All(seat => next.View(seat).Skills!.Count(skill =>
                 skill.ContentId == ShanheTeamRankPrograms.SharedRebirthSkillId) == 1),
            "A new native entry with spent player and ally A still grants only the other eligible allies when the school is bought during battle.");
        next.Use("fall-target", [3]);
        Need(next.View(3) is { Hp: 5, IsAlive: true } && next.Events<SkillUsageConsumedEvent>().Any(e =>
                e.SkillOwnerSeat == 3 && ShanheTeamRankPrograms.IsAllyExpeditionRebirthUsage(next.Setup!, e)),
            "Another ally's inherited entitlement works even after the player's expedition privilege was spent before this encounter.");
        next.Use("fall-target", [2]);
        Need(!next.View(2).IsAlive && next.Events<SkillUsageConsumedEvent>().All(e => e.SkillOwnerSeat != 2 ||
                !ShanheTeamRankPrograms.IsAllyExpeditionRebirthUsage(next.Setup!, e)),
            "An ally's previously spent expedition entitlement is not refreshed by the new battle or the live school purchase.");
    }

    public static void UnownedManualGrantAndOrdinaryTeamsCannotReceiveRankPrivileges()
    {
        var s = Scene.Start(4, false);
        var before = Enumerable.Range(1, 3).Select(seat => (s.View(seat).Hp, s.View(seat).MaxHp, s.View(seat).HandCount)).ToArray();
        s.Use("grant-school", []);
        Need(s.View(0).Skills!.Any(skill => skill.ContentId == ShanheSchoolEffects.TeamRankSkillId) &&
             Enumerable.Range(1, 3).Select(seat => (s.View(seat).Hp, s.View(seat).MaxHp, s.View(seat).HandCount)).SequenceEqual(before),
            "A genuine manual program grant cannot stand in for owning the school or issue opening health and hand benefits.");
        s.NextOwnPlay();
        var allyTurn = s.Events<TurnStartedEvent>().First(e => e.ActorSeat == 1).TurnNumber;
        Need(s.Game.CardMovements.Count(m => m.TurnNumber == allyTurn && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1) &&
                    m.Reason == CardMoveReasons.Draw) == 3 && s.Events<CardUseDeclaredEvent>().Count(e => e.SourceSeat == 1 && e.CardKind == CardKind.Slash) == 3,
            "The unearned program adds neither rank draw nor rank Slash quota to the ally's actual native turn.");
        var normal = Scene.StartOrdinary();
        var team = normal.View(0).TeamId;
        Need(normal.View(0).Skills!.Any(skill => skill.ContentId == ShanheSchoolEffects.TeamRankSkillId) && team is not null &&
             normal.Game.CreateSnapshot(0).Players.Any(p => p.Seat != 0 && p.TeamId == team) &&
             normal.Game.CreateSnapshot(0).Players.Where(p => p.Seat != 0 && p.TeamId == team).All(p => p is { Hp: 4, MaxHp: 4 } &&
                 normal.Game.CardMovements.Count(m => m.To == CardLocation.Hand(p.Seat) && m.Reason == CardMoveReasons.InitialDeal) == 4) &&
             normal.Events<ShanheTeamRankActivatedEvent>().Length == 0,
            "The exact printed program in an ordinary public team match supplies no rank health or initial-deal benefit, even if an AI ally has already taken its normal turn. " + normal.Details());
    }

    private static int FixedCount(int seat) => seat == 1 ? 5 : seat == 2 ? 1 : 0;
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ShanheBattleSetup? setup)
    {
        public GameEngine Game { get; } = game;
        public ShanheBattleSetup? Setup { get; } = setup;
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public string Details() => JsonSerializer.Serialize(new { players = Game.CreateSnapshot(0).Players,
            draws = Events<NativeDrawInvocationRecordedEvent>(), usages = Events<SkillUsageConsumedEvent>(),
            uses = Events<CardUseDeclaredEvent>(), bindings = Events<ProgramBindingResolvedEvent>() });
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "A native team-school command failed."); }
        private void Advance()
        {
            if (Game.PendingDecision is { PlayerSeat: 0 } prompt)
            {
                var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is
                    "pass" or "let-die" or "take-damage" or "no-nullification" or "take-duel-damage");
                Need(choice is not null, "Unexpected human native team-school decision: " + JsonSerializer.Serialize(prompt));
                Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
            }
            else Accept(new AdvanceOneStepCommand(Game.Revision));
        }
        public void Play()
        {
            for (var step = 0; step < 600; step++)
            {
                if (Game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(Game.State.Winner == Winner.None, "The team-school fixture ended before returning to the original Play boundary."); Advance();
            }
            throw new InvalidOperationException("The bounded team-school flow did not return to Play. " + Details());
        }
        public void BeginUse(string activation, int[] targets) =>
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, Game.Revision, Game.PendingDecision!.PromptId));
        public void Use(string activation, int[] targets) { BeginUse(activation, targets); Play(); }
        public void NextOwnPlay() { Accept(new EndPlayPhaseCommand(0, Game.Revision, Game.PendingDecision!.PromptId)); Play(); }
        public void Buy(string reward)
        { var item = Game.CreateShanheMarketSnapshot(0)!.ShopItems.Single(i => i.Reward?.Id == reward); Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, Game.Revision)); Play(); }
        public void RescueOwner()
        {
            for (var step = 0; step < 240 && Game.PendingDecision is not { Kind: DecisionKind.RescueDying, PlayerSeat: 0 }; step++) Advance();
            Need(Game.PendingDecision is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 }, "The genuine owner HP loss must expose its real dying response.");
        }
        public void CompleteSpentOwnerDeath()
        {
            for (var step = 0; step < 240 && View(0).IsAlive; step++)
            {
                Need(Game.State.Status != EngineStatus.Completed,
                    "A completed spent-rebirth fixture must not continue with an allegedly living owner.");
                if (Game.PendingDecision is { PlayerSeat: 0 } prompt)
                    Need(prompt.Choices.All(c => c.Parameters.GetValueOrDefault("skill-id") != ShanheGeneralRanks.RebirthSkillId),
                        "A real rescue prompt after spending the player's own entitlement cannot offer its original rebirth.");
                Advance();
            }
            Need(!View(0).IsAlive, "With no remaining real rescue source, the spent-rebirth owner must actually die. " + Details());
        }
        public static Scene Start(int rank, bool complete, int[]? usedSeats = null, bool playerSpent = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheMarketPackage(),
                new StandardShanheSchoolTeamPackage(), new StandardShanheRankPrivilegePackage(), new StandardShanheDrawThresholdPackage(), new Fixture(false));
            ShanheRewardDefinition member = new(Component, "真实队友普通件") { Rarity = 1, CampaignEconomy = new(ReplacedEquipmentGold: 1), SchoolMembership = new(2201, 6, 1) };
            ShanheRewardDefinition filler = new(Filler, "另一份真实经济战法") { Rarity = 1, CampaignEconomy = new(ReplacedEquipmentGold: 2) };
            ShanheCombatantDefinition[] allies =
            [
                new ShanheCombatantDefinition(Fighter, MaxHpBonus: 2, Hp: 4, InitialHandBonus: 2, DrawBonus: 1, SlashBonus: 2)
                    { StartingHand = Enumerable.Range(0, 5).Select(n => new ShanheStartingCard(CardKind.Slash, Suit.Spade, n + 1)).ToArray() },
                new(QuietAlly, MaxHpBonus: 2, Hp: 4, InitialHandBonus: 2, DrawBonus: 1, SlashBonus: 2) { StartingHand = [new(CardKind.Dodge)] },
                new(QuietAlly, MaxHpBonus: 2, Hp: 4, InitialHandBonus: 2, DrawBonus: 1, SlashBonus: 2) { StartingHand = [] }
            ];
            var definition = new ShanheCampaignDefinition("fixture:team-school", "真实队友套装", [Owner], ["shop"],
                [new("shop", "常驻商店", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy-member", "实际普通件", 100, RewardId: Component, LeavesNode: false), new("buy-filler", "其他战法", 100, RewardId: Filler, LeavesNode: false)]),
                 new("battle", "盟友与守军", ShanheNodeKind.Battle, [], [new("guard", "真实五人阵", [new(Enemy) { StartingHand = [] }], allies)])],
                [member, filler], StartingGold: 500, ExtraSkillSlots: 0, StartingBonuses: new(MaxHp: 3, InitialHand: 4, Draw: 3, Slash: 2))
                { Schools = [new(6, "队友流", 2235, "共享等阶效果，最低3阶", [1, 0, 0, 0])] };
            var campaign = ShanheCampaignSession.Create(definition, new(71, Owner)
            { PlayerRank = rank, UseCodexRewardPool = true, PersistentMarket = true, EquippedTacticIds = complete ? [Component] : [] }, _ => registry);
            var moved = campaign.Submit(new MoveShanheNodeCommand("battle", campaign.Revision)); Need(moved.Accepted, moved.Error ?? "The actual team-school campaign could not enter battle.");
            var setup = ShanheBattleSetup.Freeze(campaign.PendingBattleSetup! with
            {
                AllyRankRebirthUsedSeats = usedSeats ?? [],
                RebirthAvailable = !playerSpent && campaign.PendingBattleSetup!.RebirthAvailable,
                ExtraSkillIds = campaign.PendingBattleSetup!.ExtraSkillIds.Where(id =>
                    !playerSpent || id != ShanheGeneralRanks.RebirthSkillId).ToArray()
            });
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(engine, setup); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
        public static Scene StartOrdinary()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheSchoolTeamPackage(), new StandardShanheDrawThresholdPackage(), new Fixture(true));
            var engine = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = Ordinary, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(engine, null); s.Accept(new StartGameCommand());
            for (var step = 0; step < 100 && engine.PendingDecision is not { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 }; step++) s.Advance();
            Need(engine.PendingDecision is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 }, "The ordinary team fixture must expose its actual general choice.");
            s.Accept(new SelectGeneralCommand(0, Owner, engine.Revision, engine.PendingDecision!.PromptId)); s.Play(); return s;
        }
    }

    private sealed class Fixture(bool ordinary) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-school-team", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"fall-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":20}]},
                    {"id":"fall-owner","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":20}]},
                    {"id":"grant-school","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["{{{ShanheSchoolEffects.TeamRankSkillId}}}"]}]}]},
                  {"id":"{{{Range}}}","revision":1,"modifiers":[{"id":"enemy-in-range","query":"attackRange","operation":"add","value":10,"priority":0}]},
                  {"id":"{{{Health}}}","revision":1,"triggers":[
                    {"id":"maximum-child","window":"afterHealthChanged","subject":"owner","optional":false,"condition":{"kind":"healthChangeKindIs","hpChangeKind":"maximumHp"},"effects":[{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"setting-child","window":"afterHealthChanged","subject":"owner","optional":false,"condition":{"kind":"healthChangeKindIs","hpChangeKind":"setting"},"effects":[{"op":"grantArmor","target":"owner","amount":1}]}]},
                  {"id":"{{{Gain}}}","revision":1,"triggers":[{"id":"gain-child","window":"cardsGained","subject":"owner","optional":false,"destinationZones":["hand"],"movementOccurrence":"perBatch","condition":{"kind":"compare","left":{"kind":"movedCardCount"},"operator":"equal","right":{"kind":"integerConstant","value":1}},"effects":[{"op":"grantArmor","target":"owner","amount":1}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Range, Health, Gain, Quiet }.ToDictionary(id => id, _ => new { name = "真实队友流程驱动", description = "原生体力、摸牌、用牌和濒死" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "套装持有者", "supporter", Driver, "shu", 6)
                { AdditionalSkillIds = ordinary ? [ShanheSchoolEffects.TeamRankSkillId] : [] });
            builder.AddGeneral(new(Fighter, "实际出杀盟友", "supporter", Range, "shu", 6) { AdditionalSkillIds = [Health, Gain] });
            builder.AddGeneral(new(QuietAlly, "静默盟友", "supporter", Quiet, "shu", 6) { AdditionalSkillIds = [Health, Gain] });
            builder.AddGeneral(new(Enemy, "敌军", "supporter", Quiet, "wei", 30));
            for (var n = 1; n <= 3; n++) builder.AddGeneral(new("fixture:school-team-ordinary-peer-" + n, "普通阵容", "supporter", Quiet, "wei", 6));
            builder.AddDeck(new(Deck, "实际闪牌堆", ordinary ? 4 : 0, 2, [])
                { PhysicalCards = Enumerable.Range(0, 160).Select(n => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, n % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河图队友五人阵", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Fighter, QuietAlly, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 4, [ShanheBattleSetup.EnemyTeamId] = 1 }));
            builder.AddMode(new(Ordinary, "普通公开团队", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, "fixture:school-team-ordinary-peer-1", "fixture:school-team-ordinary-peer-2", "fixture:school-team-ordinary-peer-3"],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
