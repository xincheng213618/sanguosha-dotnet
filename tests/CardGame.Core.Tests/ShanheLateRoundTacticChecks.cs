using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheLateRoundTacticChecks
{
    private const string Owner = "fixture:late-round-stock-owner", Peer = "fixture:late-round-stock-peer";
    private const string Driver = "fixture:late-round-stock-driver", Quiet = "fixture:late-round-stock-quiet", Deck = "fixture:late-round-stock-deck";

    public static void DamageAndSlashQuotaChangeOnlyAtTheirActualGlobalRoundThresholds()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheLateRoundTacticsPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "fixture:late-round-stock", Owner, 8, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Spade, 3), new(CardKind.FireSlash, Suit.Heart, 4), new(CardKind.ColdSlash, Suit.Spade, 5),
                new(CardKind.Slash, Suit.Club, 6), new(CardKind.Slash, Suit.Club, 7), new(CardKind.Slash, Suit.Club, 8),
                new(CardKind.IronChain, Suit.Club, 9)], [],
            Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray(), [], new string('c', 64))
        {
            TacticSkillIds = [StandardShanheCampaignCatalog.CurrentYuzhan2SkillId,
                StandardShanheCampaignCatalog.CurrentYuzhan3SkillId, StandardShanheCampaignCatalog.CurrentBattleLearning3SkillId]
        };
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 16, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        var s = new Scene(game); s.Accept(new StartGameCommand()); s.Play();
        s.Card(CardKind.Slash, [1], 1);
        Need(!game.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Slash),
            "The first actual round has no late-tactic damage and retains the ordinary one-Slash quota.");
        s.NextOwnPlay();
        s.Card(CardKind.IronChain, [1, 2], null); s.Card(CardKind.FireSlash, [1], 2);
        Need(s.Hits().TakeLast(2).Select(e => (e.TargetSeat, e.Amount)).SequenceEqual([(1, 2), (2, 2)]) &&
            !game.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Slash),
            "At real round two only rare Yuzhan activates; its final card damage propagates once while the ordinary quota remains one.");
        s.NextOwnPlay();
        s.Card(CardKind.ColdSlash, [1], 3); s.Card(CardKind.Slash, [1], 3); s.Card(CardKind.Slash, [1], 3);
        Need(s.Hits().TakeLast(3).All(e => e.Amount == 3) && s.View().Hand.Any(c => c.Kind == CardKind.Slash) &&
            !game.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Slash) &&
            s.Events<ProgramCardDamageModifiedEvent>().Count(e => e.Source.SkillId == StandardShanheCampaignCatalog.CurrentYuzhan2SkillId) == 4 &&
            s.Events<ProgramCardDamageModifiedEvent>().Count(e => e.Source.SkillId == StandardShanheCampaignCatalog.CurrentYuzhan3SkillId) == 3,
            "Round three activates both independent +1 card-damage sources and exactly two extra Slashes, including Cold Slash; a fourth real material remains unusable.");
    }

    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        private GameEngine G { get; } = game;
        public PlayerSnapshot View() => G.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public DamageAppliedEvent[] Hits() => Events<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real late-round tactic command."); }
        public void Play()
        {
            for (var step = 0; step < 180; step++)
            {
                if (G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(G.State.Winner == Winner.None, "Late-round source checks ended before reaching their bounded native flow.");
                if (G.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is
                        "pass" or "take-damage" or "no-nullification") ?? prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The late-round material check did not return to Play.");
        }
        public void Card(CardKind kind, int[] targets, int? expectedDamage)
        {
            var card = View().Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            if (expectedDamage is { } expected)
                Need(G.PredictPublicActionDamage(0, action, targets[0]).DamageAmount == expected,
                    "Public prediction and actual source thresholds must use the same real global round.");
            Accept(new PlayCardCommand(0, card.Id, targets, G.Revision, G.PendingDecision!.PromptId, action.PlayedCardKind, action.TargetCardId)); Play();
            if (expectedDamage is { } dealt)
                Need(Hits().Last().Amount == dealt, "The native actual damage did not match its source's round threshold.");
        }
        public void NextOwnPlay()
        {
            var round = Events<RoundStartedEvent>().Last().RoundNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, G.PendingDecision!.PromptId)); Play();
            Need(Events<RoundStartedEvent>().Last().RoundNumber == round + 1, "Threshold checks must cross a real global round, rather than a fabricated local counter.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:late-round-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddCard(new("fixture:late-round-stock-cold", "冰杀", "基本牌", CardCatalog.Get(CardKind.ColdSlash).Description, CardKind.ColdSlash));
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"hand","query":"handLimit","operation":"add","value":20,"priority":0}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "真实轮次战法驱动", description = "原生回合和实体杀" }) }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "后期战法使用者", "supporter", Driver, "qun", 8));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "备用实体", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "后期战法原生战斗", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
