using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangCombatChecks
{
    private const string NestedDrawWatch = "fixture:wushuang-nested-draw-watch",
        NestedGainDraw = "fixture:wushuang-nested-gain-draw";

    public static void NativeDrawThresholdDamageKeepsSuspendedBeforeDamageCold()
    {
        foreach (var (tactic, afterDamage, nestedGain, drawCount, triggerCard) in new[]
            { (2104, false, false, 1, 1), (2143, false, false, 1, 1), (2143, true, false, 1, 1),
                (2143, true, true, 1, 1), (2143, true, false, 2, 1), (2143, true, false, 2, 2) })
        {
            var registry = ContentRegistry.Build(new NestedDrawFixture(Registry.Value, tactic, afterDamage, nestedGain, drawCount, triggerCard));
            var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(26) with
                { AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false }, registry);
            Accept(game, new StartGameCommand()); Until(game, () => P(game)?.Kind == DecisionKind.SelectGeneral);
            Accept(game, new SelectGeneralCommand(0, Human, game.Revision, P(game)!.PromptId));
            Until(game, () => V(game, 0).Wushuang is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
            var opening = V(game, 0).Wushuang!.Offers.Single(offer => offer.TacticId == 2002);
            Accept(game, new BuyWushuangOfferCommand(0, opening.Id, null, game.Revision)); Play(game);
            var threshold = tactic == 2104 ? 9 : 6;
            var setupCount = threshold - triggerCard;
            Require(!E<WushuangCombatIssuedEvent>(game).Any(e => e.TacticId == tactic),
                "The verified first-owner opening has no counted owner native draw before the configured setup.");
            Use(game, "threshold-setup"); Play(game);
            Require(E<WushuangCombatIssuedEvent>(game).Count(e => e.TacticId == tactic) == setupCount,
                "The configured real setup draw places the threshold on the selected first/last native draw occurrence.");
            var ownerHp = Player(game, 0).Hp; var peerHp = Player(game, 1).Hp;
            Use(game, "hit-self"); Until(game, () => P(game)?.SkillPrompt?.SkillId == NestedDrawWatch);
            var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Driver && f.ActivationId == "hit-self");
            var nested = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.WushuangCombat?.TacticId == tactic);
            var originalPreserved = afterDamage
                ? game.ResolutionStack.OfType<DamageFrame>().Any(damage => damage.ParentFrameId == root.Id && damage.TargetSeat == 0 && damage.Amount == 1)
                : game.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>()
                    .Single(w => (w.ContinuationAttackResolutionId ?? w.ParentFrameId) == root.Id).Prevented;
            var batch = nested.WindowContext!.MovementBatch!;
            var material = batch.Movements[nested.WindowContext.MovementIndex!.Value];
            var invoice = E<NativeDrawInvocationRecordedEvent>(game).Single(draw => draw.ParentFrameId == batch.ParentFrameId &&
                draw.OwnerSeat == 0 && draw.SequenceBefore < material.Sequence && draw.SequenceAfter >= material.Sequence && draw.Reason == material.Reason);
            Require(originalPreserved && root.AttackAttempt is not null &&
                nested.AttackAttempt is { SourceSeat: 0, TargetSeat: 1, DamageAmount: 1 } &&
                nested.AttackReturn?.ParentAttackOwnerFrameId == root.Id &&
                invoice.Requested == (nestedGain ? 1 : drawCount) && invoice.ActualCount == invoice.Requested &&
                E<NativeDrawInvocationRecordedEvent>(game).Count(e => e.InvocationId == invoice.InvocationId) == 1,
                "The exact native draw child pays its sixth/ninth-card damage while retaining its original before/after-damage owner and invoice.");
            Require(drawCount != 2 || (triggerCard == 1 ? batch.NativeDrawInvocation is null : batch.NativeDrawInvocation is not null),
                "The Draw2 cases exercise both the first uncarried material and the last material with its full invocation projection.");
            Require(!nestedGain || game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == NestedGainDraw &&
                frame.WindowContext?.Window == SkillProgramTriggerWindow.CardsGained),
                "The chained case reaches its real gain-triggered native draw under the still-paid original damage draw cursor.");
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Require(Enumerable.Range(0, 2).All(seat => SnapshotJson.Serialize(V(game, seat)) == SnapshotJson.Serialize(V(restored, seat))) &&
                JsonSerializer.Serialize(game.CardMovements) == JsonSerializer.Serialize(restored.CardMovements),
                "Cold recovery preserves both private views, the real draw and both suspended damage owners.");
            game = restored;
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"); Play(game);
            Require(Player(game, 0).Hp == ownerHp - (afterDamage ? 1 : 0) && Player(game, 1).Hp == peerHp - 1 &&
                E<DamageAppliedEvent>(game).Count(e => e.SourceSeat == 0 && e.TargetSeat == 1) == 1 &&
                E<DamageAppliedEvent>(game).Count(e => e.TargetSeat == 0) == (afterDamage ? 1 : 0) &&
                E<WushuangCombatIssuedEvent>(game).Count(e => e.TacticId == tactic) == setupCount + drawCount + (nestedGain ? 1 : 0) &&
                !game.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == root.Id || f.Id == nested.Id),
                "Returning the paid child deals exactly one tactic damage, preserves the original self-damage/prevention result and never redraws or repeats the threshold.");
        }
    }

    private sealed class NestedDrawFixture(ContentRegistry source, int tactic, bool afterDamage = false, bool nestedGain = false,
        int drawCount = 1, int triggerCard = 1) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:wushuang-nested-draw-damage", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.CopyModeFeatureFactoriesFrom(source);
            foreach (var card in source.Cards.Values) builder.AddCard(card);
            foreach (var skill in source.Skills.Values.Where(skill => skill.Id != Driver)) builder.AddSkill(skill);
            foreach (var general in source.Generals.Values)
                builder.AddGeneral(general.Id == Human ? general with { AdditionalSkillIds = [NestedDrawWatch, $"wushuang:tactic-{tactic}"] } : general);
            foreach (var deck in source.Decks.Values) builder.AddDeck(deck);
            foreach (var mode in source.Modes.Values) builder.AddMode(mode);
            var setup = (tactic == 2104 ? 9 : 6) - triggerCard;
            var entry = afterDamage ? "\"window\":\"afterDamageApplied\",\"subject\":\"owner\",\"damageOccurrence\":\"perDamage\"," :
                "\"window\":\"beforeDamageApplied\",\"subject\":\"damageTarget\",";
            var prevention = afterDamage ? "" : "{\"op\":\"preventCurrentDamage\",\"target\":\"owner\"},";
            var gainGrant = nestedGain ? ",{\"op\":\"grantSkills\",\"target\":\"owner\",\"skillIds\":[\"" + NestedGainDraw + "\"]}" : "";
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"activations":[
                {"id":"threshold-setup","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"draw","target":"owner","amount":{{{setup}}}}{{{gainGrant}}}]},
                {"id":"hit-self","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"damage","target":"owner","amount":1}]}],
               "triggers":[{"id":"damage-native-draw",{{{entry}}}"optional":false,"effects":[{{{prevention}}}{"op":"draw","target":"owner","amount":{{{drawCount}}}}]}]},
              {"id":"{{{NestedDrawWatch}}}","revision":1,"triggers":[{"id":"paid-threshold-child","window":"afterDamageApplied","subject":"damageSource","damageOccurrence":"perDamage","optional":false,"condition":{"kind":"damageTargetIsOther"},"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
              {"id":"{{{NestedGainDraw}}}","revision":1,"triggers":[{"id":"gain-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.{{{Driver}}}.Draw"],"priority":1,"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "真实伤害内摸牌", description = "先防止实际伤害，再执行原生摸牌。" },
                    [NestedGainDraw] = new { name = "真实获得牌后摸牌", description = "原获得牌子窗的独立原生摸牌。" },
                    [NestedDrawWatch] = new { name = "原生摸牌战法伤害子窗", description = "等待真实战法伤害返回。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalogue = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalogue.Programs)
                builder.AddSkill(new(id, id, "Native nested draw damage regression")
                    { Program = program, ProgramPresentation = catalogue.Presentations[id] });
        }
    }
}
