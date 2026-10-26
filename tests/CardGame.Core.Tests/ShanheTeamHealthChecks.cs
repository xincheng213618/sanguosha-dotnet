using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheTeamHealthChecks
{
    private const string Owner = "fixture:team-health-owner", Ally = "fixture:team-health-ally", Enemy = "fixture:team-health-enemy";
    private const string Driver = "fixture:team-health-driver", Watch = "fixture:team-health-watch", Quiet = "fixture:team-health-quiet";
    private const string Deck = "fixture:team-health-deck";

    public static void ActualAlliedHealthBonusPreservesWoundsAndBypassesRecoveryModifiers()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheTeamHealthPackage(), new Fixture());
        var reward = new ShanheRewardDefinition("actual-team-health", "队·体")
            { Rarity = 3, TacticSkillId = ShanheTeamRankPrograms.TeamHealthSkillId };
        var definition = new ShanheCampaignDefinition("fixture:team-health", "队·体实战", [Owner], ["battle"],
            [new("battle", "队友试阵", ShanheNodeKind.Battle, [],
                [new("guard", "敌军", [new(Enemy, Hp: 8) { StartingHand = [] }],
                    [new(Ally, MaxHpBonus: 2, Hp: 7) { StartingHand = [] },
                     new(Ally, Hp: 2) { StartingHand = [] },
                     new(Ally, MaxHpBonus: 4, Hp: 14) { StartingHand = [] }])])], [reward]);
        var session = ShanheCampaignSession.Create(definition, new(67, Owner)
            { EquippedTacticIds = [reward.Id] }, _ => registry);
        var entered = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
        Need(entered.Accepted, entered.Error ?? "The real carried allied-health tactic could not enter battle.");
        var setup = session.PendingBattleSetup! with { Hp = 5 };
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 16, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        var before = game.CreateSnapshot(0).Players.ToArray();
        Accept(game, new StartGameCommand()); Play(game);
        var after = game.CreateSnapshot(0).Players;
        Need(after.Where(p => p.Seat is >= 1 and <= 3).All(p =>
                p.MaxHp == before[p.Seat].MaxHp + 3 && p.Hp == before[p.Seat].Hp + 3 &&
                p.MaxHp - p.Hp == before[p.Seat].MaxHp - before[p.Seat].Hp && p.Armor == 0) &&
            after[0].Hp == before[0].Hp && after[0].MaxHp == before[0].MaxHp &&
            after[4].Hp == before[4].Hp && after[4].MaxHp == before[4].MaxHp &&
            Events<RecoveryAppliedEvent>(game).Length == 0,
            "The real initial tactic adds three maximum/current HP to each of three actual allies, preserves wounds and leaves source/enemy unchanged; recovery bonuses never enlarge it. " + HealthFacts(game));
        var changes = Events<MaximumHpChangedEvent>(game).Where(e => e.SkillId == ShanheTeamRankPrograms.TeamHealthSkillId).ToArray();
        Need(changes.Length == 3 && changes.All(e => e.Delta == 3) &&
            changes.Select(e => e.PlayerSeat).Order().SequenceEqual([1, 2, 3]) &&
            Enumerable.Range(1, 3).All(seat => Events<ProgramBindingResolvedEvent>(game).Any(e =>
                e.OwnerSeat == seat && e.SkillId == Watch && e.BindingId == "after-health" && e.Activated && e.Completed)) &&
            game.ResolutionStack.All(frame => frame is not ProgramSkillFrame and not HpChangedTriggerWindowFrame),
            "Each ally receives one attributed maximum-health change and its genuine health observers finish before returning to the player's Play boundary. " + HealthFacts(game));

        Accept(game, new UseProgramSkillCommand(0, Driver, "heal-one", [], [2], game.Revision, game.PendingDecision!.PromptId));
        Play(game);
        Need(Events<RecoveryAppliedEvent>(game) is [{ TargetSeat: 2, Amount: 5, RemainingHp: 10 }] &&
            game.CreateSnapshot(0).Players[2].Armor == 1,
            "The same ally's live recovery +4 and after-recovery observer still apply to a genuine one-point healing instruction, proving the health privilege bypassed them.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, game.PendingDecision!.PromptId)); Play(game);
        Need(Events<MaximumHpChangedEvent>(game).Count(e => e.SkillId == ShanheTeamRankPrograms.TeamHealthSkillId) == 3 &&
            game.CreateSnapshot(0).Players.Select(p => p.MaxHp).SequenceEqual(after.Select(p => p.MaxHp)),
            "A later genuine turn cannot initialize an already carried health privilege again.");
    }

    private static T[] Events<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static string HealthFacts(GameEngine game) => JsonSerializer.Serialize(new
    {
        players = game.CreateSnapshot(0).Players.Select(p => new { p.Seat, p.Hp, p.MaxHp, p.Armor }),
        changes = Events<MaximumHpChangedEvent>(game), recoveries = Events<RecoveryAppliedEvent>(game)
    });
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The allied-health native command was rejected."); }
    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }
    private static void Play(GameEngine game)
    {
        for (var step = 0; step < 180; step++)
        {
            if (game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            Need(game.State.Winner == Winner.None, "Allied-health fixture ended before the original Play boundary.");
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Allied-health native children failed to return to the original Play boundary.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:team-health", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"heal-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"recover","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{{Watch}}}","revision":1,
                   "cardPolicies":[{"id":"true-healing-plus-four","kind":"ownRecoveryAmountBonus","cardKinds":[],"value":4}],
                   "triggers":[
                    {"id":"after-health","window":"afterHealthChanged","subject":"owner","optional":false,
                     "effects":[{"op":"draw","target":"owner","amount":1}]},
                    {"id":"after-healing","window":"afterHpRecovered","subject":"owner","optional":false,
                     "effects":[{"op":"grantArmor","target":"owner","amount":1}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[
                    {"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                     "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Watch, Quiet }.ToDictionary(id => id, _ => new { name = "队·体流程验证", description = "真实体力、回复与子流程" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "队·体使用者", "supporter", Driver, "shu", 12));
            builder.AddGeneral(new(Ally, "同行队友", "supporter", Watch, "shu", 10) { AdditionalSkillIds = [Quiet] });
            builder.AddGeneral(new(Enemy, "敌军", "supporter", Quiet, "wei", 10));
            builder.AddDeck(new(Deck, "真实实体牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "队·体实战", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 4, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
