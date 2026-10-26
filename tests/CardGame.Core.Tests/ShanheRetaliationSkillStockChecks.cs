using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheRetaliationSkillStockChecks
{
    private const string Tuifeng = "ol:tuifeng", Chengxiang = "classic:chengxiang";
    private const string Owner = "fixture:retaliation-stock-owner", Enemy = "fixture:retaliation-stock-enemy";
    private const string Driver = "fixture:retaliation-stock-driver", Quiet = "fixture:retaliation-stock-quiet";
    private const string Deck = "fixture:retaliation-stock-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new Fixture()));

    public static void PurchasedTuifengStoresEachDamagePointAndCashOutAddsRealSlashUses()
    {
        var s = PurchaseAndEnterBattle("build-skill-3158", Tuifeng);
        s.Play();
        var firstTurn = s.Game.State.TurnNumber;
        var stored = s.View().Hand.Select(card => card.Id).ToArray();
        Need(stored.Length == 2 && s.Pile().Count == 0, "The unrelated printed general starts with two genuine normal-draw cards and no Feng.");
        s.UseIncomingDamage();
        foreach (var id in stored)
        {
            s.Reach(() => s.At(Tuifeng, "activate"));
            s.Activate();
            s.Reach(() => s.Action("select-owned-cards"));
            s.Answer(choice => choice.Cards.SequenceEqual([id]));
        }
        s.Play();
        Need(s.Events<DamageAppliedEvent>() is [{ SourceSeat: 1, TargetSeat: 0, Amount: 2 }] && s.View().Hp == 10 &&
            s.View().HandCount == 0 && s.Pile().Select(card => card.Id).ToHashSet().SetEquals(stored) &&
            s.Events<ProgramBindingResolvedEvent>().Count(binding => binding.SkillId == Tuifeng && binding.Activated && binding.Completed) == 2 &&
            stored.All(id => s.Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                move.To.Zone == CardZoneKind.PublicPersistentPile && move.Reason.Value == StoreReason) == 1),
            "One real two-point damage event offers two separately completed purchased Tuifeng invocations, each storing its selected original hand entity as Feng exactly once.");

        s.NextOwnPlay();
        var cashOut = s.Events<PublicPileCashOutResolvedEvent>().Single(item => item.Operation == SkillProgramEffectOp.CashOutPublicPile);
        var draws = s.Game.CardMovements.Where(move => move.To == CardLocation.Hand(0) && move.Reason.Value == CashOutDrawReason).ToArray();
        var normalDraws = s.Game.CardMovements.Where(move => move.To == CardLocation.Hand(0) &&
            move.TurnNumber == s.Game.State.TurnNumber && move.Reason == CardMoveReasons.Draw).ToArray();
        Need(s.Game.State.TurnNumber > firstTurn && s.Pile().Count == 0 &&
            cashOut is { FrozenCount: 2, DrawActual: 4, SlashLimitGranted: 2 } && draws.Length == 4 &&
            draws.All(move => move.From == CardLocation.DrawPile) && draws.Select(move => move.CardId).Distinct().Count() == 4 &&
            normalDraws.Length == 2 && normalDraws.All(move => move.From == CardLocation.DrawPile) &&
            stored.All(id => s.Game.CardMovements.Count(move => move.CardId == id && move.From.Zone == CardZoneKind.PublicPersistentPile &&
                move.To == CardLocation.DiscardPile && move.Reason.Value == CashOutPaymentReason) == 1) &&
            s.View().HandCount == 6,
            "The next actual preparation removes both Feng to discard once, draws four real entities through Tuifeng, grants two Slash uses, then performs the separate normal two-card draw.");

        var slashes = s.View().Hand.Take(3).Select(card => card.Id).ToArray();
        foreach (var id in slashes) s.Slash(id);
        Need(s.View(1).Hp == 9 && s.View().HandCount == 3 &&
            s.Events<CardUseDeclaredEvent>().Count(item => item.SourceSeat == 0 && item.CardKind == CardKind.Slash) == 3 &&
            slashes.All(id => s.Game.CardMovements.Any(move => move.CardId == id && move.To == CardLocation.DiscardPile)) &&
            !s.Game.GetHumanLegalActions().Any(action => action.Kind == LegalActionKind.Slash),
            "The bought skill's temporary modifier permits three genuine physical Slashes in that turn and rejects a fourth despite three more Slashes in hand.");
    }

    public static void PurchasedChengxiangRevealsOncePerDamageAndMovesLegalSubset()
    {
        var s = PurchaseAndEnterBattle("build-skill-3086", Chengxiang);
        s.Play();
        var originalHand = s.View().Hand.Select(card => card.Id).ToArray();
        s.UseIncomingDamage();
        s.Reach(() => s.At(Chengxiang, "activate"));
        s.Activate();
        s.Reach(() => s.Action("select-subset"));
        var reveal = s.Events<ProgramCardsRevealedEvent>().Single(item => item.SkillId == Chengxiang);
        Need(s.Events<DamageAppliedEvent>() is [{ SourceSeat: 1, TargetSeat: 0, Amount: 2 }] && s.View().Hp == 10 &&
            reveal.OwnerSeat == 0 && reveal.Cards.Count == 4 && reveal.Cards.All(card => card.Rank == 4) &&
            reveal.Cards.Select(card => card.Id).Distinct().Count() == 4 &&
            s.Prompt!.Choices.Any(choice => choice.Cards.Count == 0) && s.Prompt.Choices.Any(choice => choice.Cards.Count == 3) &&
            s.Prompt.Choices.All(choice => choice.Cards.Sum(id => reveal.Cards.Single(card => card.Id == id).Rank) <= 13) &&
            s.Prompt.Choices.All(choice => choice.Cards.Count < 4),
            "A single two-point damage event offers one ordinary Chengxiang reveal of four existing cards; its real subset choices include zero and three cards but exclude the sixteen-point whole pool.");
        var selected = s.Prompt.Choices.First(choice => choice.Cards.Count == 3).Cards.ToArray();
        s.Answer(choice => choice.Cards.SequenceEqual(selected));
        s.Play();
        var discarded = reveal.Cards.Single(card => !selected.Contains(card.Id)).Id;
        var subset = s.Events<ProgramCardSubsetSelectedEvent>().Single(item => item.SkillId == Chengxiang);
        Need(s.Events<ProgramCardsRevealedEvent>().Count(item => item.SkillId == Chengxiang) == 1 &&
            s.Events<ProgramBindingResolvedEvent>().Count(item => item.SkillId == Chengxiang && item.Activated && item.Completed) == 1 &&
            subset.RankSum == 12 && subset.CardIds.SequenceEqual(selected) && s.View().HandCount == originalHand.Length + 3 &&
            s.View().Hand.Select(card => card.Id).ToHashSet().SetEquals(originalHand.Concat(selected)) &&
            !s.View().Hand.Any(card => card.Id == discarded) && s.Game.CreateSnapshot(0).ProcessingCardCount == 0 &&
            reveal.Cards.All(card => s.Game.CardMovements.Count(move => move.CardId == card.Id &&
                move.From == CardLocation.DrawPile && move.To == CardLocation.Processing) == 1) &&
            selected.All(id => s.Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Processing &&
                move.To == CardLocation.Hand(0) && move.Reason.Value == ChengxiangMoveReason) == 1) &&
            s.Game.CardMovements.Count(move => move.CardId == discarded && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason.Value == ChengxiangMoveReason) == 1,
            "The purchased ordinary version resolves once per damage: the chosen twelve-point subset enters hand as the same three entities and the one unchosen entity goes to discard once, rather than treating each damage point as another reveal or replacing the gain with a draw.");
    }

    private const string StoreReason = "skill-program." + Tuifeng + ".StoreBoundCardsInPublicPile.payment";
    private const string CashOutPaymentReason = "skill-program." + Tuifeng + ".CashOutPublicPile.payment";
    private const string CashOutDrawReason = "skill-program." + Tuifeng + ".CashOutPublicPile.draw";
    private const string ChengxiangMoveReason = "skill-program." + Chengxiang + ".MoveBoundCards";

    private static Scene PurchaseAndEnterBattle(string rewardId, string skillId)
    {
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var reward = current.Rewards.Single(item => item.Id == rewardId);
        var definition = new ShanheCampaignDefinition("fixture:retaliation-skill-stock", "真实受伤技能购买", [Owner], ["shop"],
        [
            new("shop", "技能商店", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("buy", reward.Name, GoldCost: StandardShanheCampaignCatalog.ShopMetadataByRewardId[rewardId].Price,
                    RewardId: rewardId, LeavesNode: false)]),
            new("battle", "购入受伤技能实战", ShanheNodeKind.Battle, [], [new("guard", "静默守军", [new(Enemy)])])
        ], [reward], StartingGold: 600, ExtraSkillSlots: 1);
        var campaign = ShanheCampaignSession.Create(definition, new(83, Owner) { UnlockedRewardIds = [rewardId] }, _ => Registry.Value);
        Campaign(new MoveShanheNodeCommand("shop", campaign.Revision));
        var item = campaign.Snapshot.ShopItems.Single(item => item.Reward?.Id == rewardId);
        Need(item is { State: ShanheShopItemState.Available, RequiresSkillReplacement: false, GoldCost: 600 } && reward.SkillId == skillId,
            "The exact current-season source is sold at its real six-hundred-copper price into an empty ordinary skill slot.");
        Campaign(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, campaign.Revision));
        Need(campaign.Snapshot.Gold == 0 && campaign.Snapshot.ExtraSkillIds.SequenceEqual([skillId]) &&
            campaign.Snapshot.ExtraSkillSlots == 1 && campaign.Snapshot.TacticIds.Count == 0,
            "The real shop purchase pays once and retains the exact ordinary skill identity in its available skill slot.");
        Campaign(new ChooseShanheNodeOptionCommand("$leave", null, campaign.Revision));
        Campaign(new MoveShanheNodeCommand("battle", campaign.Revision));
        var scene = Scene.Start(campaign.PendingBattleSetup!);
        Need(scene.View().GeneralId == Owner && scene.View().Skills!.Any(skill => skill.ContentId == skillId),
            "The real campaign battle grants the purchased program to the fixture general whose printed skill is only the damage driver.");
        return scene;

        void Campaign(ShanheCampaignCommand command)
        { var result = campaign.Submit(command); Need(result.Accepted, result.Error ?? "Rejected actual retaliation skill purchase."); }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public IReadOnlyList<CardSnapshot> Pile() => View().PublicPersistentPiles?.SingleOrDefault(pile => pile.SourceSkillId == Tuifeng)?.Cards
            ?? (View().PublicPersistentPileSkillId == Tuifeng ? View().PublicPersistentPileCards : null) ?? [];
        public bool Action(string action) => Prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == action) == true;
        public bool At(string skill, string action) => Prompt?.SkillPrompt?.SkillId == skill && Action(action);
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native retaliation skill command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = Prompt!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(predicate).Id, Game.Revision)); }
        public void Activate() => Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        public void UseIncomingDamage() => Accept(new UseProgramSkillCommand(0, Driver, "incoming", [], [1], Game.Revision, Prompt!.PromptId));
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void NextOwnPlay()
        {
            var turn = Game.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, Game.Revision, Prompt!.PromptId));
            Reach(() => Game.State.TurnNumber > turn && Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public void Slash(int id)
        {
            var action = Game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.Slash && item.CardId == id && item.TargetSeats.SequenceEqual([1]));
            Accept(new PlayCardCommand(0, id, action.TargetSeats, Game.Revision, Prompt!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 180; step++)
            {
                if (condition()) return;
                Need(Prompt?.PlayerSeat != 0, "Unexpected human prompt in the bounded retaliation scene: " + JsonSerializer.Serialize(Prompt));
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded native retaliation skill boundary: " + JsonSerializer.Serialize(Prompt));
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
        public PackageManifest Manifest { get; } = new("fixture-shanhe-retaliation-skill-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:li-tong", "classic:cao-chong");
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
                        [Driver] = new { name = "真实两点受伤驱动", description = "用原生两点伤害触发已购受伤技能" },
                        [Quiet] = new { name = "静默守军", description = "跳过出牌阶段以观察下一本人真实回合" }
                    } }));
            foreach (var (id, name) in new[] { (Driver, "真实两点受伤驱动"), (Quiet, "静默守军") })
                builder.AddSkill(new(id, name, name) { Program = programs.Programs[id], ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "受伤技能购买者", "supporter", Driver, "shu", 12));
            builder.AddGeneral(new(Enemy, "静默守军", "supporter", Quiet, "wei", 12));
            builder.AddDeck(new(Deck, "固定四点实体杀牌堆", 0, 2, [])
            { PhysicalCards = Enumerable.Range(0, 48).Select(index => new ContentDeckPhysicalCard("standard:slash", (Suit)(index % 4), 4)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "购入受伤技能实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
