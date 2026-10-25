using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheNativeSkillFlowChecks
{
    public static void MouLumengFirstGiftRepeatedFireAndGrowthUseActualCampaign()
    {
        var setup = new ShanheBattleSetup(83, 1, "fixture:native-lumeng", "mou:lu-meng", 4, 0, 0, 0, 0,
            [], [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Club, 8), new(CardKind.Slash, Suit.Heart, 9)], [],
            [new("classic:ma-chao", MaxHpBonus: 6, Hp: 10) { StartingHand = [new(CardKind.Dodge, Suit.Diamond, 2)] },
             new("classic:zhao-yun", MaxHpBonus: 6, Hp: 10) { StartingHand = [] },
             new("classic:guan-yu", MaxHpBonus: 6, Hp: 10) { StartingHand = [] }], [], new string('a', 64))
            { StartingResourcesExpireOnUse = true };
        var s = Scene.Start(setup);
        var initialRange = s.Game.GetAttackRange(0);
        var first = s.Origin(0);
        s.PlayCard(first, 1);
        s.Reach(() => s.P is { Kind: DecisionKind.Yingbo, PlayerSeat: 0 } or { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(s.P is { Kind: DecisionKind.Yingbo, PlayerSeat: 0 },
            "A real Shanhe Mou Lu Meng's first Slash must enter Yingbo's native physical-card gift instead of silently losing both printed skills to an identity-only gate.");
        s.Answer(choice => choice.Parameters.GetValueOrDefault("action") == "yingbo-gift" && choice.Targets.SequenceEqual([2]));
        s.Play();
        Need(s.Events<YingboCardModeEvent>() is [{ WasUsedEarlierThisRound: false, CannotBeRespondedTo: true }] &&
            s.Events<HengyeGrowthChangedEvent>() is [{ SourceSeat: 0, CurrentGrowth: 1 }] &&
            s.Game.GetAttackRange(0) == initialRange + 1 && s.View(1).Hp == 9 &&
            s.View(1).Hand.Any(card => card.Kind == CardKind.Dodge) &&
            s.View(2).Hand.Any(card => card.Id == first) &&
            s.Events<ShanheStartingResourceDestroyedEvent>().All(fact => fact.CardId != first),
            "The first Slash bypasses an actual Dodge, grows native attack range and hands its real original to another character without destroying an acquired resource.");
        var second = s.Origin(1);
        s.PlayCard(second, 3); s.Play();
        Need(s.Events<YingboCardModeEvent>() is [_, { WasUsedEarlierThisRound: true, ConvertsDamageToFire: true, DamageBonus: 1 }] &&
            s.Events<DamageAppliedEvent>().Any(fact => fact.SourceSeat == 0 && fact.TargetSeat == 3 && fact.Amount == 2 && fact.Nature == DamageNature.Fire) &&
            s.Events<HengyeGrowthChangedEvent>().Select(fact => fact.CurrentGrowth).SequenceEqual([1, 2]) &&
            s.Game.GetAttackRange(0) == initialRange + 2 &&
            s.Game.GetHumanLegalActions().Any(action => action.CardId == s.Origin(2) && action.Kind == LegalActionKind.Slash) &&
            s.Events<ShanheStartingResourceDestroyedEvent>().Count(fact => fact.CardId == second) == 1,
            "The second real Slash consumes the grown quota, deals repeated-card fire damage, grows the next quota and retires its unclaimed temporary material once.");
        Need(s.Game.CreateSnapshot(0).Players.All(player => player.Role is Role.TeamA or Role.TeamB) &&
            s.View().TeamId == ShanheBattleSetup.PlayerTeamId &&
            s.Game.CreateSnapshot(0).Players.Skip(1).All(player => player.TeamId == ShanheBattleSetup.EnemyTeamId),
            "Enabling normal native skills preserves the campaign's actual public team assignments.");
    }

    public static void BoughtLuanjiAndYuceRunNativePaymentsAndRecovery()
    {
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var luanji = current.Rewards.Single(reward => reward.Id == "build-skill-3256");
        var yuce = current.Rewards.Single(reward => reward.Id == "build-skill-3089");
        var cards = new ShanheRewardDefinition("fixture:packet", "固定起手",
            StartingCards: [new(CardKind.Dodge, Suit.Spade, 7), new(CardKind.Dodge, Suit.Spade, 8),
                new(CardKind.Peach, Suit.Diamond, 3)]);
        var encounter = new ShanheEncounterDefinition("guard", "有闪有杀的守军",
            [new("classic:ma-chao") { StartingHand = [new(CardKind.Dodge, Suit.Spade, 2), new(CardKind.Slash, Suit.Heart, 5)] }]);
        const string buyer = "boundary:hua-xiong";
        var definition = new ShanheCampaignDefinition("fixture:native-purchases", "实际购买与技能流程", [buyer], ["shop"],
            [new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("luanji", luanji.Name, 1000, RewardId: luanji.Id, LeavesNode: false),
                 new("yuce", yuce.Name, 300, RewardId: yuce.Id, LeavesNode: false),
                 new("packet", cards.Name, RewardId: cards.Id, LeavesNode: false)]),
             new("battle", "守军", ShanheNodeKind.Battle, [], [encounter])],
            [luanji, yuce, cards], StartingGold: 4000) { StartingResourcesExpireOnUse = true };
        var session = ShanheCampaignSession.Create(definition, new(83, buyer) { UseCodexRewardPool = true },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        Accept(session.Submit(new MoveShanheNodeCommand("shop", session.Revision)));
        var paid = 0;
        foreach (var id in new[] { luanji.Id, yuce.Id, cards.Id })
        {
            var item = session.Snapshot.ShopItems.Single(item => item.Reward.Id == id);
            if (id == luanji.Id) Need(item.GoldCost == 1000, "Luanji uses its actual public skill price.");
            if (id == yuce.Id) Need(item.GoldCost == 400, "The second actual skill purchase includes its category surcharge.");
            paid += item.GoldCost;
            Accept(session.Submit(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision)));
        }
        Need(session.Snapshot.Gold == 4000 - paid && session.Snapshot.ExtraSkillIds.ToHashSet().SetEquals([luanji.SkillId!, yuce.SkillId!]) &&
            StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[110] == luanji.Id &&
            StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(current).Contains(yuce.Id),
            "Both real source skills are purchasable with the category surcharge; Luanji has its actual S5 tree node and Yuce has explicit default stock.");
        Accept(session.Submit(new ChooseShanheNodeOptionCommand("$leave", null, session.Revision)));
        Accept(session.Submit(new MoveShanheNodeCommand("battle", session.Revision)));
        var s = Scene.Start(session.PendingBattleSetup!);
        var pair = new[] { s.Origin(0), s.Origin(1) };
        s.Use(luanji.SkillId!, "same-suit-pair-as-arrow-barrage", pair, []); s.Play();
        Need(s.Events<CardActionAcceptedEvent>().Any(fact => fact.Action.ActorSeat == 0 && fact.Action.Type == CardActionType.Use &&
                fact.Action.EffectiveKind == CardKind.ArrowBarrage && fact.Action.PhysicalCards.Select(card => card.CardId).ToHashSet().SetEquals(pair)) &&
            pair.All(id => s.Events<ShanheStartingResourceDestroyedEvent>().Count(fact => fact.CardId == id) == 1) &&
            s.Events<CardRespondedEvent>().Any(fact => fact.ResponderSeat == 1 && fact.EffectiveCardKind == CardKind.Dodge),
            "The bought same-suit pair becomes a genuine Arrow Barrage, pays both actual cards once and asks the opponent for a native Dodge.");
        var hp = s.View().Hp;
        // The buyer's printed Shizhan makes the opponent use a native Duel;
        // its remaining Slash cannot be discarded as a different card category.
        s.Use("boundary:shizhan-current", "selected-actor-duel", [], [1]);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == yuce.SkillId);
        s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        s.Answer(choice => choice.Cards.SequenceEqual([s.Origin(2)]));
        s.Play();
        Need(s.Events<DamageAppliedEvent>().Count(fact => fact.SourceSeat == 1 && fact.TargetSeat == 0 && fact.Amount == 1) == 1 &&
            s.Events<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.Amount == 1) == 1 && s.View().Hp == hp &&
            s.View().Hand.Any(card => card.Id == s.Origin(2)),
            "The bought Yuce triggers after a real Duel hit, displays an actual retained card and recovers once when the real damage source cannot discard another category.");
    }

    private static void Accept(CommandResult result) => Need(result.Accepted, result.Error?.Message ?? "Native command rejected.");
    private static void Accept(ShanheCampaignResult result) => Need(result.Accepted, result.Error ?? "Campaign command rejected.");
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Game.PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public int Origin(int index) => Events<ShanheStartingResourceCreatedEvent>().Single(fact => !fact.EquipmentResource && fact.RecipeIndex == index).CardId;
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = P!; Accept(Game.Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, Game.Revision))); }
        public void Use(string skill, string binding, int[] cards, int[] targets) =>
            Accept(Game.Submit(new UseProgramSkillCommand(0, skill, binding, cards, targets, Game.Revision, P!.PromptId)));
        public void PlayCard(int id, int target)
        {
            var action = Game.GetHumanLegalActions().First(item => item.CardId == id && item.TargetSeats.SequenceEqual([target]));
            Accept(Game.Submit(new PlayCardCommand(0, id, action.TargetSeats, Game.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)));
        }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 220; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge or DecisionKind.Nullification or DecisionKind.RescueDying)
                        Answer(choice => choice.Cards.Count == 0);
                    else if (prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"))
                        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
                    else throw new InvalidOperationException("Unexpected native boundary: " + prompt.Kind);
                }
                else Accept(Game.Submit(new AdvanceOneStepCommand(Game.Revision)));
            }
            throw new InvalidOperationException("Missing native skill boundary.");
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var scene = new Scene(StandardShanheCampaignCatalog.CreateBattle(setup));
            Accept(scene.Game.Submit(new StartGameCommand())); scene.Play(); return scene;
        }
    }
}
