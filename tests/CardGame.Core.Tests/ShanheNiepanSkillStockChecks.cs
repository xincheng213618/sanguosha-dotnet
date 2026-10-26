using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheNiepanSkillStockChecks
{
    private const string Niepan = StandardShanheS5YuduPackage.NiepanId;
    private const string Owner = "fixture:niepan-stock-owner", Enemy = "fixture:niepan-stock-enemy";
    private const string Driver = "fixture:niepan-stock-driver", Deck = "fixture:niepan-stock-deck";
    private const string DiscardReason = "skill-program." + Niepan + ".DiscardOwnedZoneCards";
    private const string DrawReason = "skill-program." + Niepan + ".Draw";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardActiveSkillExpansionPackage(includeJijiu: true),
        new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
        new StandardShanheS5YuduPackage(), new Fixture()));

    public static void PurchasedNiepanClearsRealOwnedCardsAndResumesLethalDamageParentAlive()
    {
        const string rewardId = "build-skill-3094";
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var reward = current.Rewards.Single(item => item.Id == rewardId);
        var definition = new ShanheCampaignDefinition("fixture:shanhe-niepan-stock", "真实涅槃购买与濒死自救", [Owner], ["shop"],
        [
            new("shop", "技能集市", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("buy", reward.Name, GoldCost: StandardShanheCampaignCatalog.ShopMetadataByRewardId[rewardId].Price,
                    RewardId: rewardId, LeavesNode: false)]),
            new("battle", "濒死自救实战", ShanheNodeKind.Battle, [],
                [new("guard", "真实双人阵容", [new(Enemy) { StartingHand = [] }])])
        ], [reward], StartingGold: 600, ExtraSkillSlots: 1);
        var campaign = ShanheCampaignSession.Create(definition,
            new(83, Owner) { UseCodexRewardPool = true, UnlockedRewardIds = [rewardId] }, _ => Registry.Value);
        Campaign(new MoveShanheNodeCommand("shop", campaign.Revision));
        var item = campaign.Snapshot.ShopItems.Single();
        Need(item is { State: ShanheShopItemState.Available, GoldCost: 600, RequiresSkillReplacement: false } &&
            item.Reward?.Id == rewardId && reward.SkillId == Niepan,
            "The real codex merchant offers the exact current-season Niepan source for six hundred copper and one available ordinary skill slot.");
        Campaign(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, campaign.Revision));
        Need(campaign.Snapshot.Gold == 0 && campaign.Snapshot.ExtraSkillSlots == 1 &&
            campaign.Snapshot.ExtraSkillIds.SequenceEqual([Niepan]) && campaign.Snapshot.TacticIds.Count == 0,
            "The actual purchase pays once and grants only the precise native Niepan identity in the existing skill slot.");
        Campaign(new ChooseShanheNodeOptionCommand("$leave", null, campaign.Revision));
        Campaign(new MoveShanheNodeCommand("battle", campaign.Revision));
        var s = Scene.Start(campaign.PendingBattleSetup!);
        s.Play();
        var players = s.Game.CreateSnapshot(0).Players;
        Need(players.Count == 2 && players.Count(player => player.IsHuman) == 1 && players[0].GeneralId == Owner &&
            players[0].Skills!.Any(skill => skill.ContentId == Niepan) &&
            players[0].Skills!.All(skill => skill.ContentId is Driver or Niepan) &&
            players[1].GeneralId == Enemy && !players[1].IsHuman && players[1].TeamId != players[0].TeamId,
            "A normal two-seat Shanhe battle grants the purchased skill to an unrelated printed owner with only its damage driver, without Yudu, Poison or boundary companion skills.");

        s.EquipCrossbow();
        Need(s.View() is { Hp: 4, MaxHp: 4, HandCount: 1, Equipment.Count: 1, Judgment.Count: 0, Armor: 0 } &&
            s.View().Equipment.Single().Kind == CardKind.Crossbow,
            "The owner genuinely equips one simple Crossbow and retains one original hand entity before entering dying; no judgment matrix or removal-trigger equipment is needed.");
        var original = s.View().Hand.Select(card => (CardId: card.Id, Location: CardLocation.Hand(0)))
            .Concat(s.View().Equipment.Select(card => (CardId: card.Id, Location: CardLocation.Equipment(0)))).ToArray();
        s.Lethal();
        s.Reach(() => s.Prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "program-trigger" &&
            choice.Parameters.GetValueOrDefault("skill-id") == Niepan) == true);
        var parent = s.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Driver);
        Need(s.View() is { Hp: -1, IsAlive: true, IsFaceDown: true, IsChained: true, Armor: 0, HandCount: 1, Equipment.Count: 1 } &&
            s.Events<DamageAppliedEvent>() is [{ SourceSeat: 1, TargetSeat: 0, Amount: 5, RemainingHp: -1 }] &&
            s.Events<PlayerDyingEvent>() is [{ VictimSeat: 0, KillerSeat: 1 }] &&
            !s.Events<ProgramSkillResolvedEvent>().Any(item => item.SkillId == Driver) && s.Events<ArmorGrantedEvent>().Length == 0,
            "The real source-one lethal damage leaves the face-down chained owner at minus one HP and pauses its original driver before the post-damage armor instruction; the bought exact Niepan is offered in its real self-dying response.");
        s.AcceptNiepan();
        s.Play();

        var discard = s.Events<ProgramOwnedZoneCardsDiscardedEvent>().Single(item => item.SkillId == Niepan);
        var binding = s.Events<ProgramBindingResolvedEvent>().Single(item => item.SkillId == Niepan);
        var draws = s.Game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();
        var discardMoves = s.Game.CardMovements.Where(move => move.Reason.Value == DiscardReason).ToArray();
        // RecoverTo commits RecoveryApplied directly when no replacement is queued.
        var recovery = s.Events<RecoveryAppliedEvent>().Single();
        var armor = s.Events<ArmorGrantedEvent>().Single();
        Need(s.View() is { IsAlive: true, IsFaceDown: false, IsChained: false, HandCount: 3, Equipment.Count: 0, Judgment.Count: 0, Armor: 1 } &&
            s.View().Hp == Math.Min(s.View().MaxHp, 3) && s.Game.State.Winner == Winner.None &&
            discard is { OwnerSeat: 0, Zones: [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], CardCount: 2 } &&
            discard.FrameId == binding.FrameId && discardMoves.Length == original.Length &&
            original.All(card => discardMoves.Count(move => move.CardId == card.CardId && move.From == card.Location && move.To == CardLocation.DiscardPile) == 1) &&
            original.All(card => s.Game.CardMovements.Count(move => move.CardId == card.CardId && move.To == CardLocation.DiscardPile) == 1) &&
            draws.Length == 3 && draws.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) &&
            draws.Select(move => move.CardId).Distinct().Count() == 3 && !draws.Any(move => original.Any(card => card.CardId == move.CardId)) &&
            s.View().Hand.Select(card => card.Id).ToHashSet().SetEquals(draws.Select(move => move.CardId)) &&
            s.Events<CardsDrawnEvent>() is [{ PlayerSeat: 0, Count: 3 }] &&
            recovery is { SourceSeat: 0, TargetSeat: 0, Amount: 4, RemainingHp: 3 } &&
            binding is { OwnerSeat: 0, Window: SkillProgramTriggerWindow.SelfDyingResponse, Activated: true, Completed: true } &&
            s.Events<SkillUsageConsumedEvent>().Where(item => item.SkillId == Niepan).ToArray() is
                [{ SkillOwnerSeat: 0, Scope: SkillUsageScope.Game, Count: 1 }] &&
            s.Events<DyingResolvedEvent>() is [{ VictimSeat: 0, Survived: true }] && s.Events<PlayerDiedEvent>().Length == 0 &&
            armor is { PlayerSeat: 0, Amount: 1, ArmorBefore: 0, ArmorAfter: 1 } && armor.ProgramFrameId == parent.Id &&
            s.Events<ProgramSkillResolvedEvent>().Single(item => item.SkillId == Driver) is { ActivationId: "lethal", Completed: true } resolved && resolved.FrameId == parent.Id &&
            s.Game.ResolutionStack.All(frame => frame.Id != parent.Id && frame.Id != binding.FrameId) &&
            s.View().Skills!.All(skill => skill.ContentId is Driver or Niepan),
            "The purchased game-limited Niepan discards each original HE entity exactly once, restores face-up unchained state, truly draws three new cards, recovers four HP from minus one to three and completes one trigger. The original lethal parent then resumes alive and grants its one armor once, without an extra Yudu or boundary skill.");

        var ordered = new[]
        {
            s.Sequence<ProgramOwnedZoneCardsDiscardedEvent>(item => item.FrameId == binding.FrameId),
            s.Sequence<ProgramChainedStateSetEvent>(item => item.SkillId == Niepan && !item.IsChained),
            s.Sequence<CardsDrawnEvent>(item => item.PlayerSeat == 0),
            s.Sequence<RecoveryAppliedEvent>(item => item.TargetSeat == 0),
            s.Sequence<ProgramBindingResolvedEvent>(item => item.FrameId == binding.FrameId),
            s.Sequence<DyingResolvedEvent>(item => item.VictimSeat == 0),
            s.Sequence<ArmorGrantedEvent>(item => item.ProgramFrameId == parent.Id),
            s.Sequence<ProgramSkillResolvedEvent>(item => item.FrameId == parent.Id)
        };
        Need(ordered.Zip(ordered.Skip(1)).All(pair => pair.First < pair.Second),
            "The committed event order retains discard, state restoration, native Draw three, recovery, completed Niepan, surviving dying return, and the original parent's armor and completion in that order.");
        return;

        void Campaign(ShanheCampaignCommand command)
        { var result = campaign.Submit(command); Need(result.Accepted, result.Error ?? "Rejected actual Niepan stock purchase."); }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View() => Game.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public long Sequence<T>(Func<T, bool> predicate) => Game.Events.Single(item => item.Payload is T fact && predicate(fact)).Sequence;
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Niepan stock command."); }
        public void EquipCrossbow()
        {
            var action = Game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.Equip && item.CardId is not null);
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, Game.Revision, Prompt!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void Lethal() => Accept(new UseProgramSkillCommand(0, Driver, "lethal", [], [1], Game.Revision, Prompt!.PromptId));
        public void AcceptNiepan()
        {
            var prompt = Prompt!;
            var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault("response") == "program-trigger" &&
                item.Parameters.GetValueOrDefault("skill-id") == Niepan);
            Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
        }
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 160; step++)
            {
                if (condition()) return;
                Need(Prompt?.PlayerSeat != 0, "Unexpected human prompt in the bounded bought-Niepan scene: " + JsonSerializer.Serialize(Prompt));
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded bought-Niepan boundary: " + JsonSerializer.Serialize(Prompt));
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-niepan-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "activations":[{"id":"lethal","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"setFaceState","target":"owner","faceDown":true},
                      {"op":"setChainedState","target":"owner","chained":true},
                      {"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":5},
                      {"op":"grantArmor","target":"owner","amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "真实致命伤害与存活续指令", description = "翻面、横置并接受敌军致命伤害，自救后继续获得护甲" } } }));
            builder.AddSkill(new(Driver, "真实致命伤害与存活续指令", "真实濒死后继续原生父技能")
                { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "购入涅槃的普通武将", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Enemy, "无自救牌的敌军", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "确定连弩实体与零自然摸牌", 2, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(index => new ContentDeckPhysicalCard("standard:crossbow", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "购买涅槃后真实濒死", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
