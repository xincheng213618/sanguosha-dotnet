using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheYijiChecks
{
    public static void BoughtSkillDistributesPrivateTopCardsPerActualDamagePoint()
    {
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var longhun = current.Rewards.Single(reward => reward.Id == "build-skill-3023");
        var yiji = current.Rewards.Single(reward => reward.Id == "build-skill-3081");
        const string buyer = "boundary:hua-xiong";
        var packet = new ShanheRewardDefinition("fixture:longhun-materials", "双红实体牌", StartingCards:
            [new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Dodge, Suit.Heart, 4),
             new(CardKind.Dodge, Suit.Diamond, 5), new(CardKind.Dodge, Suit.Diamond, 6)]);
        var encounter = new ShanheEncounterDefinition("guard", "双点伤害守军",
            [new("classic:ma-chao", MaxHpBonus: 12, Hp: 16, ExtraSkillIds: ["classic:jiaozi"])
                { StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Nullification, Suit.Spade, 9), 16).ToArray() }]);
        var definition = new ShanheCampaignDefinition("fixture:reaction-purchases", "真实购买与反伤续航", [buyer], ["shop"],
            [new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("longhun", longhun.Name, 1000, RewardId: longhun.Id, LeavesNode: false),
                 new("yiji", yiji.Name, 600, RewardId: yiji.Id, LeavesNode: false),
                 new("packet", packet.Name, RewardId: packet.Id, LeavesNode: false)]),
             new("battle", "守军", ShanheNodeKind.Battle, [], [encounter])],
            [longhun, yiji, packet], StartingGold: 2500, ExtraSkillSlots: 3);
        var session = ShanheCampaignSession.Create(definition, new(83, buyer) { UseCodexRewardPool = true },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        Accept(session.Submit(new MoveShanheNodeCommand("shop", session.Revision)));
        foreach (var (id, price) in new[] { (longhun.Id, 1000), (yiji.Id, 700), (packet.Id, 0) })
        {
            var item = session.Snapshot.ShopItems.Single(item => item.Reward?.Id == id);
            Need(item.GoldCost == price, "Actual skill stock uses the public price plus its paid category surcharge.");
            Accept(session.Submit(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision)));
        }
        Need(session.Snapshot.Gold == 800 && session.Snapshot.ExtraSkillIds.ToHashSet().SetEquals([longhun.SkillId!, yiji.SkillId!]) &&
            StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[107] == longhun.Id &&
            StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(current).Contains(yiji.Id),
            "Paid skills occupy their real extra slots, with source tree 107 and explicit default Yiji inventory.");
        Accept(session.Submit(new ChooseShanheNodeOptionCommand("$leave", null, session.Revision)));
        Accept(session.Submit(new MoveShanheNodeCommand("battle", session.Revision)));
        var s = new Scene(StandardShanheCampaignCatalog.CreateBattle(session.PendingBattleSetup!));
        s.Accept(new StartGameCommand()); s.Play();
        var hp = s.View().Hp;
        s.Use("boundary:shizhan-current", "selected-actor-duel", [], [1]);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == yiji.SkillId);
        Need(s.Events<DamageAppliedEvent>().Any(fact => fact.SourceSeat == 1 && fact.TargetSeat == 0 && fact.Amount == 2),
            "The actual native Duel and the source's unique-most-hand Jiaozi cause two real damage points.");
        var hand = s.View().Hand.Select(card => card.Id).ToHashSet();
        s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "yiji-give") == true);
        var shown = s.Game.CreateSnapshot(0).PrivateRevealedCards!.ToArray();
        var ids = shown.Select(card => card.Id).ToArray();
        Need(ids.Length == 2 && s.View().Hand.Select(card => card.Id).ToHashSet().SetEquals(hand) &&
            s.Game.CreateSnapshot(1).PrivateRevealedCards is null or { Count: 0 } &&
            s.Game.CreateSnapshot(1).PendingDecision?.Choices.Count is null or 0,
            "Yiji privately views the real top two cards, without drawing them first or exposing its choices to the other viewer.");
        var gift = shown.First(card => card.Kind != CardKind.Dodge).Id;
        s.Answer(choice => choice.Cards.SequenceEqual([gift]) && choice.Targets.SequenceEqual([1]));
        s.Reach(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "yiji-give") == true);
        s.Answer(choice => choice.Cards.SequenceEqual([ids.Single(id => id != gift)]) && choice.Targets.SequenceEqual([0]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == yiji.SkillId &&
            s.P!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"));
        s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play();
        Need(s.View(1).Hand.Any(card => card.Id == gift) && s.View().Hand.Any(card => card.Id == ids.Single(id => id != gift)) &&
            ids.All(id => s.Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Processing) == 1 && s.Game.CardMovements.Count(move => move.CardId == id &&
                move.From == CardLocation.Processing && (move.To == CardLocation.Hand(0) || move.To == CardLocation.Hand(1))) == 1) &&
            s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.SkillId == yiji.SkillId && fact.Activated) == 1 &&
            s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.SkillId == yiji.SkillId && !fact.Activated) == 1 &&
            s.Game.CreateSnapshot(0).PrivateRevealedCards is null or { Count: 0 },
            "Each point has its own optional invocation; the first two cards reach arbitrary living recipients once, and declining the second point does not peek again.");

        var hearts = new[] { s.Origin(0), s.Origin(1) };
        s.Use(longhun.SkillId!, "two-heart", hearts, []); s.Play();
        Need(s.View().Hp == hp && s.Events<RecoveryAppliedEvent>().Any(fact => fact.TargetSeat == 0 && fact.Amount == 2),
            "The actually purchased S5 Longhun uses both red hearts for its native two-point recovery.");
        var diamonds = new[] { s.Origin(2), s.Origin(3) };
        s.Use(longhun.SkillId!, "two-diamond", diamonds, [1]); s.Play();
        // Longhun's two points also receive the target's unique-most-hand Jiaozi bonus.
        Need(s.Events<DamageAppliedEvent>().Any(fact => fact.SourceSeat == 0 && fact.TargetSeat == 1 &&
                fact.Amount == 3 && fact.Nature == DamageNature.Fire),
            "The bought two-diamond Fire Slash adds one damage before the real target's Jiaozi adds another.");
        Need(hearts.Concat(diamonds).All(id => s.Game.CardMovements.Count(move => move.CardId == id &&
                move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
                s.Game.CardMovements.Count(move => move.CardId == id && move.To == CardLocation.DiscardPile) == 1),
            "Both red pairs pay and discard each actual material once.");
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void Accept(ShanheCampaignResult result) => Need(result.Accepted, result.Error ?? "Campaign command rejected.");
    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Game.PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public int Origin(int index) => Game.CardMovements.Where(move => move.From == CardLocation.OutsideGame &&
            move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal).OrderBy(move => move.Sequence).ElementAt(index).CardId;
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native command rejected."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, Game.Revision)); }
        public void Use(string skill, string binding, int[] cards, int[] targets) =>
            Accept(new UseProgramSkillCommand(0, skill, binding, cards, targets, Game.Revision, P!.PromptId));
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
                    else throw new InvalidOperationException("Unexpected Yiji boundary: " + prompt.Kind);
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing native Yiji boundary.");
        }
    }
}
