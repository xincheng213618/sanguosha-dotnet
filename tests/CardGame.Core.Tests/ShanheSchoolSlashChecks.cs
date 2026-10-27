using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSchoolSlashChecks
{
    private const string Owner = "fixture:school-slash-owner", Peer = "fixture:school-slash-peer";
    private const string Driver = "fixture:school-slash-driver", Quiet = "fixture:school-slash-quiet";
    private const string Deck = "fixture:school-slash-deck", Component = "fixture:school-slash-component";
    private const string Identity = "fixture:school-slash-identity";
    private static readonly CardKind[] Slashes = [CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash, CardKind.Slash];

    public static void ProductionSixComponentRecipeDispatchesItsDerivedSchoolProgram()
    {
        var source = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var school = source.Schools!.Single(s => s.Id == 3);
        var components = Enumerable.Range(1, 4).SelectMany(rarity => source.Rewards
            .Where(reward => reward.SchoolMembership is { SchoolId: 3 } member && member.Rarity == rarity)
            .OrderBy(reward => reward.SchoolMembership!.SourceId).Take(school.Requirements[rarity - 1])).ToArray();
        Need(components.Length == 6 && components.All(r => r.TacticSkillId != ShanheSchoolEffects.MultiSlashSkillId) &&
            components.Select(r => r.SchoolMembership!.SourceId).Distinct().Count() == 6,
            "The production multi-Slash recipe consists of six distinct concrete tactic sources, never a standalone set-effect purchase.",
            new { Requirements = school.Requirements, Available = source.Rewards.Where(r => r.SchoolMembership?.SchoolId == 3)
                .Select(r => new { r.Id, r.Name, Member = r.SchoolMembership }), Chosen = components.Select(r => r.SchoolMembership) });
        var definition = source with
        {
            Id = "fixture:school-slash-production", StartingNodeIds = ["battle"],
            Nodes = [new("battle", "真实套装试阵", ShanheNodeKind.Battle, [],
                [new("guard", "固定步卒", [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }])])]
        };
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:cao-cao")
        {
            UseCodexRewardPool = true, EquippedTacticIds = components.Select(r => r.Id).ToArray(),
            UnlockedRewardIds = source.Rewards.Select(r => r.Id).ToArray()
        }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Need(session.GetSchoolProgress().Single(p => p.School.Id == 3) is { IsComplete: true } progress &&
            progress.OwnedCounts.SequenceEqual([0, 2, 2, 2]),
            "The source-backed full production recipe completes its actual two rare, two epic, two legendary buckets.");
        var entered = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
        Need(entered.Accepted, entered.Error ?? "The production school fixture could not enter its real battle.");
        var setup = session.PendingBattleSetup!;
        Need(setup.ActiveSchoolIds.Contains(3) && setup.TacticSkillIds.Contains(ShanheSchoolEffects.MultiSlashSkillId),
            "The real campaign entry carries the completed school and its native derived tactic to battle.");
        var scene = new Scene(StandardShanheCampaignCatalog.CreateBattle(setup));
        scene.Accept(new StartGameCommand()); scene.Play();
        Need(scene.View().Skills!.Count(skill => skill.ContentId == ShanheSchoolEffects.MultiSlashSkillId) == 1,
            "The production factory discovers and grants the multi-Slash school program exactly once without a fixture-only registry shortcut.");
    }

    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PlayerSnapshot View() => Game.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "A real school-Slash command was rejected."); }
        public void Play()
        {
            for (var step = 0; step < 180; step++)
            {
                if (Game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(Game.State.Winner == Winner.None, "The school-Slash scene ended before returning to its native Play flow.");
                if (Game.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is
                        "pass" or "no-nullification" or "take-damage" or "take-duel-damage") ?? prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The bounded school-Slash continuation did not return to Play.");
        }
        public void Slash(CardKind kind)
        {
            var action = Game.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.CardId is { } id &&
                View().Hand.Any(c => c.Id == id && c.Kind == kind) && a.TargetSeats.SequenceEqual([1]) && a.ConversionSource is null);
            Accept(new PlayCardCommand(0, action.CardId!.Value, [1], Game.Revision, Game.PendingDecision!.PromptId, action.PlayedCardKind)); Play();
        }
        public static Scene Start(bool initiallyComplete)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheMarketPackage(),
                new StandardShanheSchoolBattlePackage(), new Fixture(false));
            ShanheRewardDefinition component = new(Component, "真实多刀普通件")
                { Rarity = 1, CampaignEconomy = new(ReplacedEquipmentGold: 1), SchoolMembership = new(2048, 3, 1) };
            var definition = new ShanheCampaignDefinition("fixture:school-slash", "多刀套装实战", [Owner], ["shop"],
                [new("shop", "常驻商店", ShanheNodeKind.Shop, ["battle"],
                    Choices: [new("buy-component", "多刀件", 100, RewardId: Component, LeavesNode: false)]),
                 new("battle", "四人真实战斗", ShanheNodeKind.Battle, [], [new("guards", "三名守军",
                    Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray())])],
                [component], StartingGold: 500)
                { Schools = [new(3, "多刀流", 2232, "出杀次数无限", [1, 0, 0, 0])] };
            var session = ShanheCampaignSession.Create(definition, new(83, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true, EquippedTacticIds = initiallyComplete ? [Component] : [] }, _ => registry);
            var entered = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
            Need(entered.Accepted, entered.Error ?? "The actual multi-Slash campaign did not enter its battle.");
            var setup = session.PendingBattleSetup! with
                { StartingCards = Slashes.Select((kind, index) => new ShanheStartingCard(kind, Suit.Club, index + 1)).ToArray() };
            var engine = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var s = new Scene(engine); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
        public static Scene StartOrdinary()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheSchoolBattlePackage(), new Fixture(true));
            var engine = GameEngine.CreateStandard(new()
            {
                Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Identity,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var s = new Scene(engine); s.Accept(new StartGameCommand());
            for (var step = 0; step < 80 && engine.PendingDecision is not { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }; step++)
                s.Accept(new AdvanceOneStepCommand(engine.Revision));
            Need(engine.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }, "The ordinary fixture must expose its actual human general choice.");
            s.Accept(new SelectGeneralCommand(0, Owner, engine.Revision, engine.PendingDecision!.PromptId)); s.Play(); return s;
        }
    }

    private sealed class Fixture(bool ordinary) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-school-slash", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"grant-unlimited","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"grantSkills","target":"owner","skillIds":["{{{ShanheSchoolEffects.MultiSlashSkillId}}}"]}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "真实多刀驱动", description = "只赋予程序，不直接更改出杀次数" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "真实杀使用者", "supporter", Driver, "shu", 30,
                ordinary ? [ShanheSchoolEffects.MultiSlashSkillId] : []));
            builder.AddGeneral(new(Peer, "相邻与远处守军", "supporter", Quiet, "wei", 30));
            for (var index = 1; index <= 3; index++) builder.AddGeneral(new(Peer + index, "普通模式守军", "supporter", Quiet, "wei", 30));
            builder.AddCard(new("fixture:school-cold-slash", "冰杀", "基本牌", CardCatalog.Get(CardKind.ColdSlash).Description, CardKind.ColdSlash));
            builder.AddDeck(new(Deck, "固定真实实体", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard(ordinary ? "standard:slash" : "standard:dodge", Suit.Spade, i % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河图四人阵", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
            builder.AddMode(new(Identity, "普通身份场", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer + "1", Peer + "2", Peer + "3"]));
        }
    }
}
