using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangCombatChecks
{
    private const string TianmingCostWatch = "fixture:wushuang-tianming-cost-watch";

    public static void ActualTargetPaidTianmingDrawThresholdReturnsOriginalSlashCold()
    {
        foreach (var setupDraw in new[] { 7, 8 })
        {
        var registry = ContentRegistry.Build(new TianmingDrawFixture(Registry.Value, setupDraw));
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(26) with
            { AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false }, registry);
        Accept(game, new StartGameCommand()); Until(game, () => P(game)?.Kind == DecisionKind.SelectGeneral);
        Accept(game, new SelectGeneralCommand(0, Human, game.Revision, P(game)!.PromptId));
        Until(game, () => V(game, 0).Wushuang is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        var opening = V(game, 0).Wushuang!.Offers.Single(offer => offer.TacticId == 2002);
        Accept(game, new BuyWushuangOfferCommand(0, opening.Id, null, game.Revision)); Play(game);
        Use(game, "threshold-setup"); Play(game);
        Require(E<WushuangCombatIssuedEvent>(game).Count(e => e.TacticId == 2104) == setupDraw,
            "The actual native setup draw places the ninth-card threshold on either card of the later real Tianming Draw2.");
        var inventory = game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order().ToArray();
        Use(game, "incoming", 1);
        Until(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("offer-option")?.StartsWith("target-", StringComparison.Ordinal) == true) == true);
        Answer(game, choice => choice.Targets.SequenceEqual([0]));
        Until(game, () => P(game)?.SkillPrompt?.SkillId == "ol:tianming" &&
            P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        for (var card = 0; card < 2; card++)
        {
            Until(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "card") == true);
            Answer(game, choice => choice.Cards.Count == 1);
        }
        Until(game, () => P(game)?.SkillPrompt?.SkillId == TianmingCostWatch);
        var paid = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.UniqueHpPeer is not null);
        var receipt = paid.UniqueHpPeer!; var cost = receipt.OwnerInvoice!;
        var original = game.ResolutionStack.OfType<CardUseFrame>().Single(use => use.Id == receipt.CardUseFrameId);
        Require(cost.RequiredCount == 2 && cost.CardIds.Count == 2 && !cost.DrawIssued &&
            original.Action?.ActionId == receipt.CardActionId && original.SourceSeat == 1 &&
            original.TargetSeats.SequenceEqual([0]) && E<UniqueHpPeerCostPaidEvent>(game).Count(e => e.FrameId == paid.Id) == 1,
            "The native foreign Slash and actual-target identity own exactly one complete two-card payment before Draw2.");
        game = ColdTianmingDraw(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Until(game, () => P(game)?.SkillPrompt?.SkillId == NestedDrawWatch);
        var nested = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.WushuangCombat?.TacticId == 2104);
        var drawn = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.Id).UniqueHpPeer!.OwnerInvoice!;
        var batch = nested.WindowContext!.MovementBatch!;
        Require(drawn is { DrawIssued: true, ActualDrawCount: 2 } &&
            nested.AttackReturn?.ParentAttackOwnerFrameId == original.Id &&
            batch.ParentFrameId == paid.Id && (setupDraw == 8 ? batch.NativeDrawInvocation is null :
                batch.NativeDrawInvocation is { RequestedCount: 2, ActualCount: 2 }) &&
            !E<CardUseFinishedEvent>(game).Any(e => e.ResolutionId == original.Id) &&
            !E<DamageAppliedEvent>(game).Any(e => e.SourceSeat == 1 && e.TargetSeat == 0),
            "The real paid Draw2 triggers ninth-card damage before the original Slash, with its exact invoice and suspended card owner intact.");
        game = ColdTianmingDraw(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"); Play(game);
        Require(E<UniqueHpPeerCostPaidEvent>(game).Count(e => e.FrameId == paid.Id) == 1 &&
            E<UniqueHpPeerDrawIssuedEvent>(game).Count(e => e.FrameId == paid.Id && e.ActorSeat == 0) == 1 &&
            E<WushuangCombatIssuedEvent>(game).Count(e => e.TacticId == 2104) == setupDraw + 2 &&
            E<DamageAppliedEvent>(game).Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1 &&
            E<CardUseFinishedEvent>(game).Count(e => e.ResolutionId == original.Id) == 1 &&
            cost.CardIds.All(id => game.CardMovements.Count(move => move.CardId == id &&
                move.Reason.Value == "skill-program.ol:tianming.DiscardDrawAndOfferUniqueHpPeer") == 1) &&
            !game.ResolutionStack.Any(frame => frame.Id == paid.Id || frame.Id == nested.Id || frame.Id == original.Id) &&
            inventory.SequenceEqual(game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order()) && V(game, 0).ProcessingCardCount == 0,
            "Both cold returns keep the payment, Draw2, threshold damage and original Slash completion once and conserve every physical entity.");
        _ = ColdTianmingDraw(game, registry);
        }
    }

    private static GameEngine ColdTianmingDraw(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 2).All(seat => SnapshotJson.Serialize(V(game, seat)) == SnapshotJson.Serialize(V(restored, seat))) &&
            JsonSerializer.Serialize(game.CardMovements) == JsonSerializer.Serialize(restored.CardMovements),
            "Cold recovery retains both private views, the native actual-target payment and original Slash lineage.");
        return restored;
    }

    private sealed class TianmingDrawFixture(ContentRegistry source, int setupDraw) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:wushuang-tianming-draw-damage", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.CopyModeFeatureFactoriesFrom(source);
            foreach (var card in source.Cards.Values) builder.AddCard(card);
            foreach (var skill in source.Skills.Values.Where(skill => skill.Id != Driver)) builder.AddSkill(skill);
            foreach (var general in source.Generals.Values)
                builder.AddGeneral(general.Id == Human ? general with
                    { AdditionalSkillIds = ["ol:tianming", "wushuang:tactic-2104", TianmingCostWatch, NestedDrawWatch] } : general);
            foreach (var deck in source.Decks.Values) builder.AddDeck(deck);
            foreach (var mode in source.Modes.Values) builder.AddMode(mode);
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"activations":[
                {"id":"threshold-setup","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"draw","target":"owner","amount":{{{setupDraw}}}}]},
                {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,"effects":[{"op":"offerVirtualSlashOrDraw","target":"selectedTarget"}]}]},
              {"id":"{{{TianmingCostWatch}}}","revision":1,"triggers":[{"id":"paid-cost","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill-program.ol:tianming.DiscardDrawAndOfferUniqueHpPeer"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
              {"id":"{{{NestedDrawWatch}}}","revision":1,"triggers":[{"id":"paid-threshold-child","window":"afterDamageApplied","subject":"damageSource","damageOccurrence":"perDamage","optional":false,"condition":{"kind":"damageTargetIsOther"},"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "实际外来杀前摸牌", description = "原生摸牌后请求真实外来虚拟杀。" },
                    [TianmingCostWatch] = new { name = "天命已付费用", description = "等待真实两牌费用返回。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                    [NestedDrawWatch] = new { name = "天命摸牌战法伤害", description = "等待真实阈值伤害返回。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalogue = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalogue.Programs)
                builder.AddSkill(new(id, id, "Paid actual-target native draw damage regression")
                    { Program = program, ProgramPresentation = catalogue.Presentations[id] });
        }
    }
}
