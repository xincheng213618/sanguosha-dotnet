using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    private const string SaoguShopWatch = "fixture:saogu-shop-paid-child";

    public static void SaoguPurchasedEquipmentKeepsOriginalDiscardAndPaidChildCold()
    {
        foreach (var delegateFee in new[] { false, true })
        {
            var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages:
                [new Fixture(false, ["wushuang:skill-3252", "ol:saogu", SaoguShopWatch], aiQuota: 0), new SaoguShopWatchFixture()]);
            var g = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(277) with
            { AdvanceAfterHumanCommands = false }, registry);
            Accept(g, new StartGameCommand());
            Drive(g, () => P(g)?.Kind == DecisionKind.SelectGeneral);
            Accept(g, new SelectGeneralCommand(0, Human, g.Revision, P(g)!.PromptId));
            Drive(g, () => Shop(g) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
            Buy(g, Shop(g).Offers[0].Id); Shopping(g);
            var offer = Shop(g).Offers.Single(item => item.CardOfferId == 4186); Buy(g, offer.Id);
            var gear = E<WushuangShopEquipmentCreatedEvent>(g).Single(e => e.OfferId == offer.Id).CardId;
            Accept(g, new FinishWushuangShopCommand(0, g.Revision)); Play(g);

            void Answer(Func<PromptChoice, bool> predicate)
            {
                var prompt = P(g)!;
                Accept(g, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(predicate).Id, g.Revision));
            }
            if (delegateFee)
            {
                End(g);
                Drive(g, () => P(g)?.SkillPrompt?.SkillId == "ol:saogu" &&
                    P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
                Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
                Drive(g, () => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "delegate") == true);
                Answer(c => c.Cards.SequenceEqual([gear]) && c.Targets.SequenceEqual([1]));
            }
            else
            {
                Accept(g, new UseProgramSkillCommand(0, "ol:saogu", "sweep", [], [], g.Revision, P(g)!.PromptId));
                Drive(g, () => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "discard") == true);
                Answer(c => c.Cards.SequenceEqual([gear]));
                Cold(g, registry);
                Answer(c => c.Parameters.GetValueOrDefault("branch") == "discard");
            }
            Drive(g, () => P(g)?.SkillPrompt?.SkillId == SaoguShopWatch);
            var parent = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.DuanJiongSaogu is not null);
            var receipt = parent.DuanJiongSaogu!;
            var invoice = E<DuanJiongSaoguEntityPaidEvent>(g).Single(e => e.CardId == gear);
            var destroyed = E<WushuangShopEquipmentDestroyedEvent>(g).Single(e => e.CardId == gear);
            var movement = g.CardMovements.Single(m => m.Sequence == destroyed.MovementSequence);
            Require(receipt.Paid && receipt.Ending == delegateFee && invoice.DelegateFee == delegateFee &&
                invoice.FrameId == parent.Id && invoice.Sequence == movement.Sequence && invoice.From == CardLocation.Equipment(0) &&
                movement.CardId == gear && movement.From == invoice.From && movement.To == CardLocation.OutsideGame &&
                movement.Reason.Value == "skill-program.duan-jiong.saogu.discard" && destroyed.ParentFrameId == parent.Id &&
                E<DuanJiongSaoguDiscardSuitEvent>(g).Count(e => e.CardId == gear && e.MovementSequence == movement.Sequence &&
                    e.OwnerSeat == 0 && e.Phase == receipt.Phase && e.Suit == invoice.Suit) == 1,
                "The real purchased equipment pays its exact sweep or delegation invoice once, destroys on the original paid frame, and still records its actual-phase discarded suit.");
            Cold(g, registry);
            g = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
            Require(JsonSerializer.Serialize(g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == parent.Id).DuanJiongSaogu) == JsonSerializer.Serialize(receipt),
                "Cold restoration retains the original paid Saogu receipt while the native equipment-loss child is still pending.");
            Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            Drive(g, () => E<DuanJiongSaoguCompletedEvent>(g).Any(e => e.FrameId == parent.Id));
            Require(E<DuanJiongSaoguCompletedEvent>(g).Count(e => e.FrameId == parent.Id) == 1,
                "The genuine loss child completes its original Saogu sweep or delegation exactly once.");
            var activeReturns = E<ProgramSkillResolvedEvent>(g).Count(e => e.FrameId == parent.Id && e.Completed);
            var triggeredReturns = E<ProgramBindingResolvedEvent>(g).Count(e => e.FrameId == parent.Id && e.Activated && e.Completed);
            Require(delegateFee ? activeReturns == 0 && triggeredReturns == 1 : activeReturns == 1 && triggeredReturns == 0,
                $"The exact original parent returns through its native {(delegateFee ? "Ending trigger" : "Play activation")} completion: active={activeReturns}, triggered={triggeredReturns}.");
            Require(E<DuanJiongSaoguEntityPaidEvent>(g).Count(e => e.FrameId == parent.Id && e.CardId == gear) == 1 &&
                E<WushuangShopEquipmentDestroyedEvent>(g).Count(e => e.CardId == gear) == 1 &&
                g.CardMovements.Count(m => m.CardId == gear && m.From == CardLocation.Equipment(0)) == 1 &&
                !g.CardMovements.Any(m => m.CardId == gear && (m.To == CardLocation.DiscardPile ||
                    m.From == CardLocation.OutsideGame && m.Sequence > movement.Sequence)),
                "The genuine loss child returns cold and completes its original sweep or delegation once, without repeating payment or resurrecting the destroyed equipment.");
            Cold(g, registry);
        }
    }

    private sealed class SaoguShopWatchFixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:saogu-shop-paid-child", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{SaoguShopWatch}}}","revision":1,"triggers":[
                  {"id":"paid","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["skill-program.duan-jiong.saogu.discard"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [SaoguShopWatch] = new { name = "扫谷真实装备付款子窗", description = "等待原生装备销毁付款返回。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(SaoguShopWatch, "扫谷真实装备付款子窗", "真实装备销毁后等待原始付款返回。")
                { Program = catalog.Programs[SaoguShopWatch], ProgramPresentation = catalog.Presentations[SaoguShopWatch] });
        }
    }
}
