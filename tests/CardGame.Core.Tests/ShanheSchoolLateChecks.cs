using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSchoolLateChecks
{
    private const string Owner = "fixture:school-late-owner", Peer = "fixture:school-late-peer";
    private const string Driver = "fixture:school-late-driver", Quiet = "fixture:school-late-quiet";
    private const string Deck = "fixture:school-late-deck", Component = "fixture:school-late-component";

    public static void GlobalRoundsFirstDeclarationsMissesAndChainsUseOneNativeCardBonus()
    {
        foreach (var dodgeFirstInRoundTwo in new[] { false, true })
        {
            var s = Scene.Start(complete: true);
            s.Card(CardKind.Slash, [1], 1); s.VirtualSlash();
            Need(s.Hits().Select(e => e.Amount).SequenceEqual([1, 1]),
                "No round has elapsed during round one, so both first and subsequent real Slashes keep their base damage.");

            s.NextOwnPlay();
            if (dodgeFirstInRoundTwo) s.GiveDodge();
            else s.Card(CardKind.IronChain, [1, 2], null);
            var before = s.Hits().Length;
            s.Card(CardKind.FireSlash, [1], 2);
            if (dodgeFirstInRoundTwo)
                Need(s.Hits().Length == before && s.Events<CardUseDeclaredEvent>().Any(e => e.SourceSeat == 0 && e.CardKind == CardKind.FireSlash),
                    "The actual first Fire Slash is dodged but still consumes its frozen first-declaration position.");
            else
                Need(s.Hits().Skip(before).Select(e => (e.TargetSeat, e.Amount)).SequenceEqual([(1, 2), (2, 2)]) &&
                    s.Events<ChainedDamagePropagatedEvent>() is [{ Amount: 2 }],
                    "The first round-two Slash gains one card-damage point; its chain propagates the final value without applying the school again.");
            s.VirtualSlash();
            Need(s.Hits().Last().Amount == 1,
                "The following true virtual Slash cannot claim the first-use bonus, even after that first card missed.");

            s.NextOwnPlay();
            s.Card(CardKind.ColdSlash, [1], 3); s.VirtualSlash();
            Need(s.Hits().TakeLast(2).Select(e => e.Amount).SequenceEqual([3, 1]) &&
                s.Events<ProgramCardDamageModifiedEvent>().Count(e => e.Source.SkillId == ShanheSchoolEffects.LateSlashSkillId) ==
                    (dodgeFirstInRoundTwo ? 1 : 2),
                "At the real third global round the first Cold Slash gains two, while its later virtual use does not; zero-growth round one emits no fake damage change.");
        }
    }

    public static void PaidCompletionKeepsTheCurrentRoundLedgerAndUnearnedCopiesStayInactive()
    {
        var s = Scene.Start(complete: false);
        s.NextOwnPlay();
        s.Card(CardKind.Slash, [1], 1);
        s.BuyComponent();
        s.Card(CardKind.FireSlash, [1], 1);
        Need(s.Hits().Select(e => e.Amount).SequenceEqual([1, 1]) &&
            s.Events<ShanheMarketPurchaseCommittedEvent>() is [{ Price: 100 }] &&
            s.View().Skills!.Count(skill => skill.ContentId == ShanheSchoolEffects.LateSlashSkillId) == 1 &&
            s.Game.CreateShanheMarketSnapshot(0) is { Gold: 400, ExtraSkillIds.Count: 0 },
            "Buying the missing real component immediately grants one slot-free school program without resetting already declared Slashes or paying twice.");
        s.NextOwnPlay(); s.Card(CardKind.ColdSlash, [1], 3);
        Need(s.Hits().Last().Amount == 3,
            "Live completion uses the battle's real elapsed rounds, rather than starting another round counter at purchase.");

        var unearned = Scene.Start(complete: false, grantUnearned: true);
        unearned.NextOwnPlay(); unearned.NextOwnPlay(); unearned.Card(CardKind.ColdSlash, [1], 1);
        Need(unearned.View().Skills!.Any(skill => skill.ContentId == ShanheSchoolEffects.LateSlashSkillId) &&
            unearned.Events<ProgramCardDamageModifiedEvent>().All(e => e.Source.SkillId != ShanheSchoolEffects.LateSlashSkillId),
            "A real granted copy of the program cannot impersonate a completed campaign school at a later round.");
    }

    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        private PendingDecision? Pending => Game.PendingDecision;
        public PlayerSnapshot View() => Game.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public DamageAppliedEvent[] Hits() => Events<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected late-school native command."); }
        private void ReachPlay()
        {
            for (var step = 0; step < 180; step++)
            {
                if (Pending is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(Game.State.Winner == Winner.None, "The bounded late-school scene ended before returning to its native Play flow.");
                if (Pending is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is
                        "pass" or "take-damage" or "take-duel-damage" or "no-nullification") ?? prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The late-school continuation did not return within its native bound.");
        }
        public void Card(CardKind kind, int[] targets, int? damage)
        {
            var card = View().Hand.First(c => c.Kind == kind);
            var action = Game.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            if (damage is { } expected)
            {
                var revision = Game.Revision;
                for (var query = 0; query < 3; query++)
                    Need(Game.PredictPublicActionDamage(0, action, targets[0]).DamageAmount == expected,
                        "Public late-school prediction must use the same actual round and declaration prefix as the native damage pipeline.");
                Need(Game.Revision == revision, "Repeated public predictions cannot consume the first-Slash qualification.");
            }
            Accept(new PlayCardCommand(0, card.Id, targets, Game.Revision, Pending!.PromptId, action.PlayedCardKind, action.TargetCardId)); ReachPlay();
        }
        public void VirtualSlash()
        { Accept(new UseProgramSkillCommand(0, Driver, "virtual", [], [1], Game.Revision, Pending!.PromptId)); ReachPlay(); }
        public void GiveDodge()
        {
            var card = View().Hand.Single(c => c.Kind == CardKind.Dodge);
            Accept(new UseProgramSkillCommand(0, Driver, "gift", [card.Id], [1], Game.Revision, Pending!.PromptId)); ReachPlay();
        }
        public void BuyComponent()
        {
            var item = Game.CreateShanheMarketSnapshot(0)!.ShopItems.Single(i => i.Reward?.Id == Component);
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, Game.Revision)); ReachPlay();
        }
        public void NextOwnPlay()
        {
            var round = Events<RoundStartedEvent>().Last().RoundNumber;
            Accept(new EndPlayPhaseCommand(0, Game.Revision, Pending!.PromptId)); ReachPlay();
            Need(Events<RoundStartedEvent>().Last().RoundNumber == round + 1,
                "A personal-turn transition must genuinely cross the native global-round boundary before growth increases.");
        }
        public static Scene Start(bool complete, bool grantUnearned = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheMarketPackage(),
                new StandardShanheSchoolLatePackage(), new Fixture());
            var component = new ShanheRewardDefinition(Component, "真实后期流普通件")
                { Rarity = 1, CampaignEconomy = new(ReplacedEquipmentGold: 1), SchoolMembership = new(2152, 8, 1) };
            var definition = new ShanheCampaignDefinition("fixture:school-late", "后期流实战", [Owner], ["shop"],
                [new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"],
                    Choices: [new("buy-component", "普通件", 100, RewardId: Component, LeavesNode: false)]),
                 new("battle", "三名守军", ShanheNodeKind.Battle, [], [new("guards", "守军",
                    Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray())])],
                [component], StartingGold: 500)
                { Schools = [new(8, "后期流", 2255, "每过一轮则首张杀伤害+1", [1, 0, 0, 0])] };
            var campaign = ShanheCampaignSession.Create(definition, new(31, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true, EquippedTacticIds = complete ? [Component] : [] }, _ => registry);
            var moved = campaign.Submit(new MoveShanheNodeCommand("battle", campaign.Revision));
            Need(moved.Accepted, moved.Error ?? "Actual late-school ownership could not enter battle.");
            var setup = campaign.PendingBattleSetup! with
            {
                ExtraSkillIds = grantUnearned ? [ShanheSchoolEffects.LateSlashSkillId] : [],
                StartingCards = [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.FireSlash, Suit.Heart, 9),
                    new(CardKind.ColdSlash, Suit.Spade, 10), new(CardKind.IronChain, Suit.Club, 11), new(CardKind.Dodge, Suit.Diamond, 2)]
            };
            var engine = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 16, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(engine); scene.Accept(new StartGameCommand()); scene.ReachPlay(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:school-late", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddCard(new("fixture:school-late-cold", "冰杀", "基本牌", CardCatalog.Get(CardKind.ColdSlash).Description, CardKind.ColdSlash));
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"quota","query":"slashLimit","operation":"add","value":10,"priority":0},
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"hand","query":"handLimit","operation":"add","value":20,"priority":0}],
                   "activations":[{"id":"virtual","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}]},
                    {"id":"gift","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"giveSelected","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "后期流真实轮次驱动", description = "真实使用和原生回合" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "后期套装持有者", "supporter", Driver, "qun", 8));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "备用真实实体", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "后期流原生战斗", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
