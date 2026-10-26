using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheCappedRefillSkillChecks
{
    private const string Skill = StandardShanheCappedRefillSkillsPackage.SkillId;
    private const string Owner = "fixture:capped-refill-owner", Ally = "fixture:capped-refill-ally", Enemy = "fixture:capped-refill-enemy";
    private const string Driver = "fixture:capped-refill-driver", Quiet = "fixture:capped-refill-quiet", Deck = "fixture:capped-refill-deck";
    private const string DrawReason = "skill-program.hand-count.draw";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheCappedRefillSkillsPackage(), new Fixture()));

    public static void PurchasedJiemingOffersEachDamagePointAndCapsAlliedRefillAtFive()
    {
        const string rewardId = "build-skill-3083";
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var reward = current.Rewards.Single(item => item.Id == rewardId);
        var definition = new ShanheCampaignDefinition("fixture:shanhe-capped-refill", "真实节命购买与补牌", [Owner], ["shop"],
        [
            new("shop", "技能商店", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("buy", reward.Name, GoldCost: StandardShanheCampaignCatalog.ShopMetadataByRewardId[rewardId].Price,
                    RewardId: rewardId, LeavesNode: false)]),
            new("battle", "盟友补牌实战", ShanheNodeKind.Battle, [],
                [new("guard", "三人原生阵容", [new(Enemy) { StartingHand = [] }],
                    [new(Ally) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 1)] }])])
        ], [reward], StartingGold: 600, ExtraSkillSlots: 1);
        var campaign = ShanheCampaignSession.Create(definition,
            new(83, Owner) { UnlockedRewardIds = [rewardId] }, _ => Registry.Value);
        Campaign(new MoveShanheNodeCommand("shop", campaign.Revision));
        var item = campaign.Snapshot.ShopItems.Single(item => item.Reward?.Id == rewardId);
        Need(item is { State: ShanheShopItemState.Available, RequiresSkillReplacement: false, GoldCost: 600 } && reward.SkillId == Skill,
            "The exact current-season Jieming source is purchasable for its real six-hundred-copper price into one ordinary skill slot.");
        Campaign(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, campaign.Revision));
        Need(campaign.Snapshot.Gold == 0 && campaign.Snapshot.ExtraSkillSlots == 1 &&
            campaign.Snapshot.ExtraSkillIds.SequenceEqual([Skill]) && campaign.Snapshot.TacticIds.Count == 0,
            "The real purchase pays once and grants the exact capped-refill ordinary skill without consuming a tactic position or inventing another skill slot.");
        Campaign(new ChooseShanheNodeOptionCommand("$leave", null, campaign.Revision));
        Campaign(new MoveShanheNodeCommand("battle", campaign.Revision));
        var s = Scene.Start(campaign.PendingBattleSetup!);
        s.Play();
        var players = s.Game.CreateSnapshot(0).Players;
        Need(players.Count == 3 && players.Count(player => player.IsHuman) == 1 && players[0].GeneralId == Owner &&
            players[0].Skills!.Any(skill => skill.ContentId == Skill) &&
            players[1] is { GeneralId: Ally, MaxHp: 8, HandCount: 1, IsHuman: false } &&
            players[1].TeamId == players[0].TeamId && players[2].TeamId != players[0].TeamId,
            "The actual Shanhe encounter grants Jieming to an unrelated printed owner and seats its real one-card, eight-max-HP AI ally before the opposing AI.");
        var allyInitial = s.View(1).Hand.Single().Id;
        var ownerHand = s.View().Hand.Select(card => card.Id).ToArray();
        var beforeMovements = s.Game.CardMovements.Count;
        var beforeDrawEvents = s.Events<CardsDrawnEvent>().Length;
        s.UseIncomingDamage();
        s.Reach(() => s.At("activate"));
        Need(s.Events<DamageAppliedEvent>() is [{ SourceSeat: 2, TargetSeat: 0, Amount: 2 }] && s.View().Hp == 10,
            "One genuine two-point received damage event opens the first independently optional Jieming opportunity after real HP loss.");
        s.Activate();
        s.Reach(() => s.At("select-target"));
        s.SelectAlly();
        s.Reach(() => s.At("activate"));

        var firstGains = s.Game.CardMovements.Skip(beforeMovements).ToArray();
        var firstResolved = s.Events<ProgramBindingResolvedEvent>().Single(binding => binding.SkillId == Skill && binding.Activated && binding.Completed);
        Need(s.View(1).HandCount == 5 && s.View(1).Hand.Any(card => card.Id == allyInitial) &&
            firstGains.Length == 4 && firstGains.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) && move.Reason.Value == DrawReason) &&
            firstGains.Select(move => move.CardId).Distinct().Count() == 4 &&
            firstGains.Select(move => move.Sequence).SequenceEqual(Enumerable.Range(firstGains[0].Sequence, 4)) &&
            s.View(1).Hand.Select(card => card.Id).ToHashSet().SetEquals(firstGains.Select(move => move.CardId).Append(allyInitial)) &&
            s.Events<CardsDrawnEvent>().Skip(beforeDrawEvents).ToArray() is [{ PlayerSeat: 1, Count: 4 }] &&
            s.Events<ProgramBindingStartedEvent>().Count(binding => binding.SkillId == Skill) == 1 &&
            s.View().Hand.Select(card => card.Id).SequenceEqual(ownerHand),
            "Before the second optional activation, the first independent binding has completed exactly one native four-card draw batch: the ally retains its original entity and reaches five rather than its eight maximum HP, while the owner gains no cards.");

        var fullHand = s.View(1).Hand.Select(card => card.Id).ToArray();
        var movementCount = s.Game.CardMovements.Count;
        var drawCount = s.Events<CardsDrawnEvent>().Length;
        s.Activate();
        s.Reach(() => s.At("select-target"));
        Need(s.Prompt!.Choices.Any(choice => choice.Targets.SequenceEqual([1])),
            "The second damage point independently permits choosing the same living ally even though its hand is already at the five-card cap.");
        s.SelectAlly();
        s.Play();
        var completed = s.Events<ProgramBindingResolvedEvent>().Where(binding => binding.SkillId == Skill && binding.Activated && binding.Completed).ToArray();
        Need(completed.Length == 2 && completed.Select(binding => binding.FrameId).Distinct().Count() == 2 &&
            completed.Any(binding => binding.FrameId == firstResolved.FrameId) &&
            completed.All(binding => binding.OwnerSeat == 0 && binding.Window == SkillProgramTriggerWindow.AfterDamageApplied) &&
            s.Events<ProgramBindingStartedEvent>().Count(binding => binding.SkillId == Skill) == 2 &&
            s.View(1).Hand.Select(card => card.Id).SequenceEqual(fullHand) && s.View(1).HandCount == 5 &&
            s.Game.CardMovements.Count == movementCount && s.Events<CardsDrawnEvent>().Length == drawCount &&
            s.Events<DamageAppliedEvent>().Length == 1 && s.View().Hand.Select(card => card.Id).SequenceEqual(ownerHand),
            "The second separately accepted damage-point opportunity completes on its own frame without drawing, discarding or moving anything when the same ally already holds five; the first four-card draw is never repeated.");
        return;

        void Campaign(ShanheCampaignCommand command)
        { var result = campaign.Submit(command); Need(result.Accepted, result.Error ?? "Rejected actual capped-refill skill purchase."); }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public bool At(string action) => Prompt?.SkillPrompt?.SkillId == Skill &&
            Prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == action);
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native capped-refill command."); }
        private void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = Prompt!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(predicate).Id, Game.Revision)); }
        public void Activate() => Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        public void SelectAlly() => Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target" && choice.Targets.SequenceEqual([1]));
        public void UseIncomingDamage() => Accept(new UseProgramSkillCommand(0, Driver, "incoming", [], [2], Game.Revision, Prompt!.PromptId));
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 160; step++)
            {
                if (condition()) return;
                Need(Prompt?.PlayerSeat != 0, "Unexpected human prompt in the bounded capped-refill scene: " + JsonSerializer.Serialize(Prompt));
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded capped-refill boundary: " + JsonSerializer.Serialize(Prompt));
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-capped-refill", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"incoming","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":2}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner",
                    "optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Driver] = new { name = "真实两点受伤驱动", description = "通过原生伤害逐点触发购入的节命" },
                        [Quiet] = new { name = "静默参战者", description = "跳过出牌阶段" }
                    } }));
            foreach (var (id, name) in new[] { (Driver, "真实两点受伤驱动"), (Quiet, "静默参战者") })
                builder.AddSkill(new(id, name, name) { Program = programs.Programs[id], ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "节命购买者", "supporter", Driver, "shu", 12));
            builder.AddGeneral(new(Ally, "高体力上限盟友", "supporter", Quiet, "shu", 8));
            builder.AddGeneral(new(Enemy, "静默敌军", "supporter", Quiet, "wei", 12));
            builder.AddDeck(new(Deck, "确定实体补牌牌堆", 0, 2, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(index => new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "节命购买后三人实战", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
