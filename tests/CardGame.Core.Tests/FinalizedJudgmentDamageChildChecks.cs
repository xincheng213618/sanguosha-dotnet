using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangCombatChecks
{
    private const string NestedJudgmentReactive = "fixture:finalized-damage-response-judgment",
        NestedJudgmentWatch = "fixture:finalized-damage-response-watch";

    public static void FinalizedDamageResponseJudgmentReplacesItsOwnMaterialCold()
    {
        var registry = ContentRegistry.Build(new FinalizedDamageJudgmentFixture(Registry.Value));
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(26) with
            { AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false }, registry);
        Accept(game, new StartGameCommand()); Until(game, () => P(game)?.Kind == DecisionKind.SelectGeneral);
        Accept(game, new SelectGeneralCommand(0, Human, game.Revision, P(game)!.PromptId));
        Until(game, () => V(game, 0).Wushuang is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        var opening = V(game, 0).Wushuang!.Offers.Single(offer => offer.TacticId == 2002);
        Accept(game, new BuyWushuangOfferCommand(0, opening.Id, null, game.Revision)); Play(game);
        Use(game, "outer-judgment"); Until(game, () => P(game)?.Kind == DecisionKind.ProgramJudgmentReplacement);
        var outer = game.ResolutionStack.OfType<JudgmentFrame>().Single();
        var oldRevision = game.Revision;
        try
        {
            typeof(GameEngine).GetMethod("BeginJudgment", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(game, [null, 0, "forged-parallel-judgment", 0L, null, JudgmentContinuationKind.ProgramSkill,
                    null, 0, "forged", SkillProgramCardSetVisibility.Public, null, null]);
            throw new InvalidOperationException("An unrelated parallel judgment bypassed its native guard.");
        }
        catch (TargetInvocationException error) when (error.InnerException is InvalidOperationException failure &&
            failure.Message == "The engine cannot resolve two judgments at once.") { }
        Require(game.Revision == oldRevision && game.ResolutionStack.OfType<JudgmentFrame>().Single().Id == outer.Id,
            "An unresolved judgment rejects an unrelated concurrent judgment without mutation.");
        Answer(game, choice => choice.Cards.Count == 0);
        Until(game, () => P(game)?.Choices.Any(choice => choice.Targets.SequenceEqual([1])) == true);
        Answer(game, choice => choice.Targets.SequenceEqual([1]));
        Until(game, () => P(game)?.Kind == DecisionKind.ProgramJudgmentReplacement &&
            game.ResolutionStack.OfType<JudgmentFrame>().Count() == 2);
        var inner = game.ResolutionStack.OfType<JudgmentFrame>().Single(judgment => judgment.Id != outer.Id);
        var material = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        var producer = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.TriggerId == "finalized-strike");
        Require(game.ResolutionStack.OfType<JudgmentFrame>().Single(j => j.Id == outer.Id) is { Succeeded: false, CardId: var outerCard } &&
            outerCard == outer.CardId && inner.ParentFrameId != outer.ParentFrameId &&
            producer.AttackAttempt is { DamageWasApplied: true, TargetSeat: 1, DamageAmount: 1 } &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == outer.CardId).Location == CardLocation.Judgment(0),
            "The finalized original material remains on its own judgment while its paid damage response opens a distinct inner judgment.");
        game = ColdNestedJudgment(game, registry);
        var forged = GameReplay.Restore(game.CreateCheckpoint(), registry);
        var frames = (FrameStore)typeof(GameEngine).GetField("_resolutionStack", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(forged)!;
        frames.Replace(inner with { ParentFrameId = outer.ParentFrameId });
        var rejected = false;
        try { typeof(GameEngine).GetMethod("AssertCoreInvariants", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(forged, null); }
        catch (TargetInvocationException error) when (error.InnerException is InvalidOperationException) { rejected = true; }
        Require(rejected, "Cold state cannot reparent the inner judgment away from the exact paid damage response.");
        Answer(game, choice => choice.Cards.SequenceEqual([material]));
        Until(game, () => P(game)?.SkillPrompt?.SkillId == NestedJudgmentWatch);
        Require(game.ResolutionStack.OfType<JudgmentFrame>().Single(j => j.Id == inner.Id).CardId == material &&
            game.ResolutionStack.OfType<JudgmentFrame>().Single(j => j.Id == outer.Id).CardId == outer.CardId &&
            game.CardMovements.Count(move => move.CardId == material && move.From == CardLocation.Hand(0) &&
                move.Reason == CardMoveReasons.ProgramJudgmentReplace) == 1 &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == inner.CardId).Location == CardLocation.DiscardPile,
            "Guicai pays one actual own hand entity to replace only the inner result; the original finalized card is untouched.");
        game = ColdNestedJudgment(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"); Play(game);
        Require(!game.ResolutionStack.Any(frame => frame is JudgmentFrame or ProgramJudgmentTriggerWindowFrame or DamageFrame) &&
            V(game, 0).ProcessingCardCount == 0 &&
            E<JudgmentRequestedEvent>(game).Count(e => e.Reason == "fixture:outer-judgment") == 1 &&
            E<JudgmentRequestedEvent>(game).Count(e => e.Reason == "fixture:inner-judgment") == 1 &&
            E<DamageAppliedEvent>(game).Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1 &&
            new[] { outer.CardId, inner.CardId, material }.All(id =>
                game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location == CardLocation.DiscardPile),
            "Both cold returns settle their own judgment materials once, finish the original damage once and leave no processing entities.");
        _ = ColdNestedJudgment(game, registry);
    }

    private static GameEngine ColdNestedJudgment(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 2).All(seat => SnapshotJson.Serialize(V(game, seat)) == SnapshotJson.Serialize(V(restored, seat))) &&
            JsonSerializer.Serialize(game.CardMovements) == JsonSerializer.Serialize(restored.CardMovements),
            "Cold recovery preserves both private views and all physical movements at the exact nested judgment boundary.");
        return restored;
    }

    private sealed class FinalizedDamageJudgmentFixture(ContentRegistry source) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:finalized-damage-child-judgment", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.CopyModeFeatureFactoriesFrom(source);
            foreach (var card in source.Cards.Values) builder.AddCard(card);
            foreach (var skill in source.Skills.Values.Where(skill => skill.Id != Driver)) builder.AddSkill(skill);
            foreach (var general in source.Generals.Values)
                builder.AddGeneral(general.Id == Human ? general with { AdditionalSkillIds = ["classic:guicai", NestedJudgmentWatch] } :
                    general.Id == "fixture:wushuang-combat-peer" ? general with { SkillId = NestedJudgmentReactive } : general);
            foreach (var deck in source.Decks.Values) builder.AddDeck(deck);
            foreach (var mode in source.Modes.Values) builder.AddMode(mode);
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"outer-judgment","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"startJudgment","target":"owner","judgmentReason":"fixture:outer-judgment","resultBind":"outer","visibility":"public"},{"op":"moveBoundCards","target":"owner","sourceBind":"outer","destination":"discardPile"}]}],
               "triggers":[{"id":"finalized-strike","window":"judgmentFinalized","subject":"owner","excludedReasons":[],"judgmentReasons":["fixture:outer-judgment"],"suits":["spade"],"minimumRank":1,"maximumRank":13,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"damage","target":"selectedTarget","amount":1,"nature":"thunder"}]}]},
              {"id":"{{{NestedJudgmentReactive}}}","revision":1,"triggers":[{"id":"judge-after-damage","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,"effects":[{"op":"startJudgment","target":"owner","judgmentReason":"fixture:inner-judgment","resultBind":"inner","visibility":"public"},{"op":"moveBoundCards","target":"owner","sourceBind":"inner","destination":"discardPile"}]}]},
              {"id":"{{{NestedJudgmentWatch}}}","revision":1,"triggers":[{"id":"paid-inner-result","window":"judgmentFinalized","subject":"any","excludedReasons":[],"judgmentReasons":["fixture:inner-judgment"],"suits":["spade","heart","club","diamond"],"minimumRank":1,"maximumRank":13,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "判定后实际雷伤", description = "以真实判定结果造成雷伤。" },
                    [NestedJudgmentReactive] = new { name = "实际伤害后判定", description = "原伤害响应独立判定。" },
                    [NestedJudgmentWatch] = new { name = "内层判定已支付结果", description = "等待真实内层判定返回。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalogue = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalogue.Programs)
                builder.AddSkill(new(id, id, "Finalized damage child judgment regression")
                    { Program = program, ProgramPresentation = catalogue.Presentations[id] });
        }
    }
}
