using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSkillRefillStockChecks
{
    private const string Xunxun = "boundary:xunxun", Feedback = "boundary:feedback";
    private const string Owner = "fixture:skill-refill-owner", Enemy = "fixture:skill-refill-enemy";
    private const string Driver = "fixture:skill-refill-driver", Deck = "fixture:skill-refill-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new Fixture()));

    public static void PurchasedXunxunAndFeedbackKeepNativeDeckOrderAndCardGain()
    {
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var rewards = new[] { "build-skill-3061", "build-skill-3082" }
            .Select(id => current.Rewards.Single(reward => reward.Id == id)).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:skill-refill-stock", "真实续牌技能购买", [Owner], ["shop"],
        [
            new("shop", "技能商店", ShanheNodeKind.Shop, ["battle"], Choices: rewards.Select(reward =>
                new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name,
                    GoldCost: StandardShanheCampaignCatalog.ShopMetadataByRewardId[reward.Id].Price,
                    RewardId: reward.Id, LeavesNode: false)).ToArray()),
            new("battle", "续牌技能实战", ShanheNodeKind.Battle, [],
                [new("guard", "有实际手牌的守军", [new(Enemy)
                    { StartingHand = [new(CardKind.Slash, Suit.Heart, 7)] }])])
        ], rewards, StartingGold: 400, ExtraSkillSlots: 2);
        var campaign = ShanheCampaignSession.Create(definition,
            new(83, Owner) { UnlockedRewardIds = rewards.Select(reward => reward.Id).ToArray() }, _ => Registry.Value);
        Campaign(new MoveShanheNodeCommand("shop", campaign.Revision));
        foreach (var (reward, price) in rewards.Zip(new[] { 100, 300 }))
        {
            var item = campaign.Snapshot.ShopItems.Single(item => item.Reward?.Id == reward.Id);
            Need(item is { State: ShanheShopItemState.Available, RequiresSkillReplacement: false } && item.GoldCost == price,
                "The two real source versions are initially unearned and occupy two available ordinary skill slots at their authored prices.");
            Campaign(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, campaign.Revision));
        }
        Need(campaign.Snapshot.Gold == 0 && campaign.Snapshot.ExtraSkillIds.SequenceEqual([Xunxun, Feedback]) &&
            campaign.Snapshot.ExtraSkillSlots == 2 && campaign.Snapshot.TacticIds.Count == 0,
            "Actual campaign purchases pay four hundred copper and retain the exact boundary Xunxun and Feedback skill identities.");
        Campaign(new ChooseShanheNodeOptionCommand("$leave", null, campaign.Revision));
        Campaign(new MoveShanheNodeCommand("battle", campaign.Revision));
        var s = Scene.Start(campaign.PendingBattleSetup!);
        s.Reach(() => s.Prompt?.SkillPrompt?.SkillId == Xunxun);
        Need(s.View().HandCount == 0, "The actual first draw has not occurred before the purchased Xunxun optional window.");
        s.Activate();
        s.Reach(() => s.Prompt is { Kind: DecisionKind.ProgramTopReorder });
        var order = s.Prompt!.DeckReorder ?? throw new InvalidOperationException("The bought Xunxun did not expose its native private deck order.");
        var viewed = order.Cards.Select(card => card.Id).ToArray();
        Need(viewed.Length == 4 && order.RequiredTopCount == 2 && s.View().HandCount == 0,
            "The real purchased program views four existing deck entities and requires exactly two on top, instead of replacing its rule with an immediate draw.");
        var selected = new[] { viewed[1], viewed[0], viewed[3], viewed[2] };
        foreach (var id in selected) s.Answer(choice => choice.Cards.SequenceEqual([id]));
        s.Play();
        var normalDraw = s.Game.CardMovements.Where(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw).ToArray();
        Need(s.View().Hand.Select(card => card.Id).ToHashSet().SetEquals(selected.Take(2)) &&
            normalDraw.Length == 2 && normalDraw.All(move => move.From == CardLocation.DrawPile && selected.Take(2).Contains(move.CardId)) &&
            !s.View().Hand.Any(card => selected.Skip(2).Contains(card.Id)) &&
            s.Events<ProgramBindingResolvedEvent>().Count(binding => binding.SkillId == Xunxun && binding.Activated && binding.Completed) == 1,
            "The unchanged native normal draw obtains the chosen top two physical cards once, while both bottom cards stay out of hand and Xunxun completes once.");
        var sourceCard = s.View(1).Hand.Single().Id;
        s.UseIncomingDamage();
        s.Reach(() => s.Prompt?.SkillPrompt?.SkillId == Feedback);
        Need(s.Events<DamageAppliedEvent>() is [{ SourceSeat: 1, TargetSeat: 0, Amount: 1 }] && s.View().Hp == 11,
            "A genuine source-one received damage point offers the actually purchased Feedback after the owner's real HP loss.");
        s.Activate();
        s.Reach(() => s.Prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-source-card") == true);
        s.Answer(choice => choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand));
        s.Play();
        Need(s.View().HandCount == 3 && s.View().Hand.Any(card => card.Id == sourceCard) && s.View(1).HandCount == 0 &&
            s.Game.CardMovements.Count(move => move.CardId == sourceCard && move.From == CardLocation.Hand(1) &&
                move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program." + Feedback + ".MoveBoundCards") == 1 &&
            s.Events<ProgramBindingResolvedEvent>().Count(binding => binding.SkillId == Feedback && binding.Activated && binding.Completed) == 1,
            "Feedback's native source-card choice truly transfers the original source hand entity once and returns to play without replacing it with a draw or fabricating a card.");
        return;

        void Campaign(ShanheCampaignCommand command)
        { var result = campaign.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real refill skill purchase."); }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native refill skill command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = Prompt!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(predicate).Id, Game.Revision)); }
        public void Activate() => Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        public void UseIncomingDamage() => Accept(new UseProgramSkillCommand(0, Driver, "incoming", [], [1], Game.Revision, Prompt!.PromptId));
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 140; step++)
            {
                if (condition()) return;
                Need(Prompt?.PlayerSeat != 0, "The bounded refill skill scene reached an unexpected human prompt.");
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded native refill skill boundary.");
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-refill-skill-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "boundary:li-dian", "boundary:sima-yi");
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "activations":[{"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "原生受伤驱动", description = "通过原生伤害触发已购买反馈" } } }));
            builder.AddSkill(new(Driver, "原生受伤驱动", "通过原生伤害触发已购买反馈")
                { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "续牌技能购买者", "supporter", Driver, "shu", 12));
            builder.AddGeneral(new(Enemy, "有真实手牌的守军", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "原生四张选堆与正常摸牌", 0, 2, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(index =>
                new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "购入续牌技能实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
