using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    private const string PreparationShopDriver = "fixture:wushuang-preparation-shop-driver";
    private const string PreparationShopWatch = "fixture:wushuang-preparation-shop-watch";

    public static void YinbingMixedStorageKeepsDestroyedShopPaymentCold()
    {
        var registry = ContentRegistry.Build(new PreparationShopFixture(StandardWushuangContentRegistry.Create()));
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(277) with
        { AiPolicyVersion = AiDecisionPolicy.RebelCoordinationVersion, AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false, MaxTurns = 12 }, registry);
        Accept(game, new StartGameCommand()); Drive(game, () => P(game)?.Kind == DecisionKind.SelectGeneral);
        Accept(game, new SelectGeneralCommand(0, Human, game.Revision, P(game)!.PromptId));
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id); Shopping(game);
        var offer = Shop(game).Offers.Single(item => item.CardOfferId == 4186);
        Buy(game, offer.Id);
        var equipment = E<WushuangShopEquipmentCreatedEvent>(game).Single(fact => fact.OfferId == offer.Id).CardId;
        Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
        Accept(game, new UseProgramSkillCommand(0, PreparationShopDriver, "acquire-native-yinbing", [], [],
            game.Revision, P(game)!.PromptId)); Play(game);
        var hand = game.CreateSnapshot(0).Players[0].Hand.First().Id;
        void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = P(game)!;
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
        }
        End(game);
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == "ol:yinbing" &&
            P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Drive(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("pile-action") == "finish") == true);
        Answer(choice => choice.Cards.SequenceEqual([equipment]));
        Answer(choice => choice.Cards.SequenceEqual([hand]));
        Answer(choice => choice.Parameters.GetValueOrDefault("pile-action") == "finish");
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == PreparationShopWatch);
        var producer = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.PublicPilePreparation is not null);
        var receipt = producer.PublicPilePreparation!;
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(game).Single(fact => fact.CardId == equipment);
        Require(producer.SkillId == "ol:yinbing" && receipt is
                { Operation: SkillProgramEffectOp.StoreNonBasicOwnedPublicPile, Stage: PublicPilePreparationStage.PaymentChildren, FrozenCount: 2 } &&
            receipt.PaidIds.SequenceEqual([equipment, hand]) &&
            receipt.PaidFrom.SequenceEqual([CardLocation.Equipment(0), CardLocation.Hand(0)]) &&
            destroyed.ParentFrameId == producer.Id && destroyed.MovementSequence > receipt.Before && destroyed.MovementSequence <= receipt.After &&
            game.CardMovements.Single(move => move.Sequence == destroyed.MovementSequence) is { From.Zone: CardZoneKind.Equipment, To.Zone: CardZoneKind.OutsideGame } &&
            game.CreateSnapshot(0).Players[0].PublicPersistentPileCards!.Select(card => card.Id).SequenceEqual([hand]),
            "The real mixed Yinbing payment preserves both original materials, while native shop destruction stores only the ordinary card in the actual pile.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"); Shopping(game);
        Require(E<PublicPilePreparationPaidEvent>(game).Count(fact => fact.ProgramFrameId == producer.Id) == 1 &&
            E<WushuangShopEquipmentDestroyedEvent>(game).Count(fact => fact.CardId == equipment) == 1 &&
            game.CardMovements.Count(move => move.CardId == equipment && move.From == CardLocation.Equipment(0)) == 1 &&
            game.CardMovements.Count(move => move.CardId == hand && move.To == receipt.Pile!.Location) == 1 &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == equipment).Location == CardLocation.OutsideGame &&
            game.CreateSnapshot(0).Players[0].PublicPersistentPileCards!.Select(card => card.Id).SequenceEqual([hand]) &&
            game.ResolutionStack.All(frame => frame.Id != producer.Id),
            "Cold returns to the original ending once, without repaying equipment, inventing a pile entity or restarting the paid parent.");
        Cold(game, registry);
    }

    private sealed class PreparationShopFixture(ContentRegistry source) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:wushuang-preparation-shop", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.CopyModeFeatureFactoriesFrom(source);
            foreach (var card in source.Cards.Values) builder.AddCard(card);
            foreach (var skill in source.Skills.Values) builder.AddSkill(skill);
            foreach (var general in source.Generals.Values) builder.AddGeneral(general);
            foreach (var deck in source.Decks.Values) builder.AddDeck(deck);
            foreach (var mode in source.Modes.Values.Where(mode => mode.Id != WushuangMode.ModeId)) builder.AddMode(mode);
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{PreparationShopDriver}}","revision":1,"activations":[{"id":"acquire-native-yinbing","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"grantSkills","target":"owner","skillIds":["ol:yinbing","ol:juedi"]}]}]},
              {"id":"{{PreparationShopWatch}}","revision":1,"triggers":[{"id":"paid-mixed-storage","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["skill-program.ol:yinbing.public-pile-preparation.payment"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [PreparationShopDriver] = new { name = "原生引兵驱动", description = "取得实际公开牌区技能。" },
                    [PreparationShopWatch] = new { name = "引兵真实付款子窗", description = "保持实际混合材料支付的原父帧。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Human, "实际引兵储存者", "", PreparationShopDriver, "qun", 30,
                ["classic:qingguo", "classic:lianying", PreparationShopWatch]));
            const string peer = "fixture:wushuang-preparation-peer", deckId = "fixture:wushuang-preparation-deck";
            builder.AddGeneral(new(peer, "固定无双对手", "", "classic:qianxun", "qun", 30));
            builder.AddDeck(new(deckId, "固定非基本实体牌", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 120).Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(WushuangMode.ModeId, "原生引兵购买装备回归", 2, 2, new Dictionary<string, int>(), deckId, 2,
                [Human, peer], ContentModeKind.Team, new Dictionary<string, int> { [WushuangMode.PlayerTeamId] = 1, [WushuangMode.AiTeamId] = 1 }));
        }
    }
}
