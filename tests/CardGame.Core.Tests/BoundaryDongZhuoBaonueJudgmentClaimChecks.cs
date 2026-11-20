using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class BoundaryDongZhuoBaonueJudgmentClaimChecks
{
    private const string Dong = "boundary:dong-zhuo", Baonue = "boundary:baonue-current";
    private const string Reason = "skill.boundary.baonue-current";
    private const string Earlier = "audit:baonue-earlier-claim", EarlierBinding = "claim-original";
    private const string Driver = "fixture:baonue-driver", Source = "fixture:baonue-source";
    private const string Target = "fixture:baonue-target", Deck = "fixture:baonue-deck";

    internal static void BaonueRecoversAfterAnEarlierClaimOfTheSameJudgmentCardCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var play = Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Accept(game, new UseProgramSkillCommand(0, Driver, "hit", [], [2], game.Revision, play.PromptId));

        var invoke = Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Baonue });
        Answer(game, invoke, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        var earlier = Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.ProgramJudgmentTrigger });
        var window = game.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Single();
        var judgment = Facts<JudgmentResolvedEvent>(game).Single(fact => fact.Reason == Reason);
        var cardId = judgment.CardId ?? throw new InvalidOperationException("Baonue must reveal a real card.");
        Require(Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 2, Amount: 1 } &&
            judgment is { TargetSeat: 1, Suit: Suit.Spade } && judgment.ResolutionId == window.Judgment.JudgmentFrameId &&
            window.Candidates.Skip(window.CandidateIndex).Select(candidate => candidate.SkillId).SequenceEqual([Earlier, Baonue]) &&
            game.CreateSnapshot(1).Players[1].Hp == 4 &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == cardId).Location == CardLocation.Judgment(1),
            "Actual Qun damage opens the wounded Dong Zhuo's own spade judgment with both original finalized candidates.");

        var control = Cold(game, content);
        Answer(control, earlier, choice => choice.Parameters.GetValueOrDefault("action") == "program-judgment-trigger-skip");
        AssertReturned(control, judgment, cardId, Baonue);

        Answer(game, earlier, choice => choice.Parameters.GetValueOrDefault("action") == "program-judgment-trigger-activate");
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Earlier } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"));
        AssertClaim(game, judgment, cardId, Earlier);
        Require(!Facts<RecoveryAppliedEvent>(game).Any() &&
            game.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Single().Id == window.Id,
            "The earlier finalized candidate has really received the same entity before Baonue starts.");

        game = Cold(game, content);
        var resume = Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Earlier });
        Answer(game, resume, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        AssertReturned(game, judgment, cardId, Earlier);
    }

    private static void AssertReturned(GameEngine game, JudgmentResolvedEvent judgment, int cardId, string claimant)
    {
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        AssertClaim(game, judgment, cardId, claimant);
        Require(Facts<RecoveryAppliedEvent>(game).ToArray() is [{ SourceSeat: 1, TargetSeat: 1, Amount: 1, RemainingHp: 5 }] &&
            game.CreateSnapshot(1).Players[1].Hp == 5 &&
            Facts<JudgmentRequestedEvent>(game).Count(fact => fact.ResolutionId == judgment.ResolutionId && fact.Reason == Reason) == 1 &&
            Facts<ProgramJudgmentTriggerResolvedEvent>(game).Count(fact => fact.JudgmentFrameId == judgment.ResolutionId &&
                fact.SkillId == Baonue && fact.Activated) == 1 &&
            game.ResolutionStack.All(frame => frame is not JudgmentFrame and not ProgramJudgmentTriggerWindowFrame),
            "Baonue must resolve the real one-point recovery and return its original judgment even if that entity is already claimed.");
    }

    private static void AssertClaim(GameEngine game, JudgmentResolvedEvent judgment, int cardId, string claimant)
    {
        var claims = Facts<ProgramJudgmentCardClaimedEvent>(game).Where(fact => fact.JudgmentFrameId == judgment.ResolutionId).ToArray();
        var delivered = game.CardMovements.Where(move => move.CardId == cardId && move.From == CardLocation.Judgment(1)).ToArray();
        Require(claims is [{ OwnerSeat: 1 }] && claims[0].SkillId == claimant && claims[0].CardId == cardId &&
            delivered is [var move] && move.To == CardLocation.Hand(1) && move.Reason.Value == $"skill-program.{claimant}.claimJudgmentCard" &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == cardId).Location == CardLocation.Hand(1),
            "The original judgment entity is transferred exactly once by the accepted claimant.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision Reach(GameEngine game, Func<PendingDecision, bool> ready)
    {
        for (var step = 0; step < 120; step++)
        {
            var prompt = Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).PendingDecision)
                .FirstOrDefault(item => item is not null);
            if (prompt is not null)
            {
                Require(ready(prompt), $"Unexpected public Baonue fixture prompt: {prompt.PlayerSeat} {prompt.Prompt}.");
                return prompt;
            }
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Baonue fixture did not reach the next public decision.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> select) =>
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));

    private static GameEngine Cold(GameEngine original, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(original.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(original.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(original.ResolutionStack) &&
            Enumerable.Range(0, original.PlayerCount).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(original.CreateSnapshot(seat))),
            "Cold public replay preserves the exact judgment, movement and decision.");
        return restored;
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var boss = new ShanheCombatantDefinition(Dong, Hp: 4, ExtraSkillIds: [Earlier])
        {
            StartingHand = [], QualifiedPrintedLordSkillIds = [Baonue]
        };
        var setup = new ShanheBattleSetup(83, 1, "baonue-original-judgment", Source, 4, 0, 0, 0, 0, [], [],
            [], [boss, new(Target) { StartingHand = [] }], [], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        return game;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-baonue-judgment-claim", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Dong);
            var rules = $$"""
                {"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"hit","minCards":0,"maxCards":0,"minTargets":1,
                    "maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{Earlier}}","revision":1,"triggers":[{"id":"{{EarlierBinding}}","window":"judgmentFinalized",
                    "subject":"owner","optional":true,"judgmentReasons":["{{Reason}}"],"excludedReasons":[],
                    "suits":["spade"],"minimumRank":1,"maximumRank":13,
                    "effects":[{"op":"claimJudgmentCard","target":"owner"},
                      {"op":"chooseOption","target":"owner","resultBind":"received","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "真实伤害", description = "对另一名角色造成1点伤害。" },
                    [Earlier] = new
                    {
                        name = "先得判定牌", description = "判定牌生效后，你可以获得此牌。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" }
                    }
                }
            });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
            {
                var shown = catalog.Presentations[id];
                builder.AddSkill(new(id, shown.Name, shown.Description) { Program = program, ProgramPresentation = shown });
            }
            builder.AddGeneral(new(Source, "群势力伤害来源", "supporter", Driver, "qun", 4));
            builder.AddGeneral(new(Target, "伤害目标", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定黑桃判定", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 12).Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, rank + 1)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "界董卓暴虐原判定牌", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Source, Dong, Target], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
