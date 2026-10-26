using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheErshengsanChecks
{
    private const string Owner = "fixture:ershengsan-owner", Enemy = "fixture:ershengsan-enemy";
    private const string Deck = "fixture:ershengsan-deck", Gain = "fixture:ershengsan-gain", Fengzi = "ol:fengzi";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheMarketPackage(), new StandardShanheErshengsanPackage(), new Fixture()));

    public static void PaidBonusFreezesEachResolvedDrawAndReturnsItsGainChildren()
    {
        var s = Scene.Start();
        var initialHand = s.View().HandCount;
        Need(s.Events<CardsDrawnEvent>().All(e => e.Count == 0),
            "Registering the future card policy does not add a card to the ordinary zero-card draw phase.");
        var first = s.Use();
        s.Until(s.GainPrompt);
        var original = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == first);
        var issued = s.Events<NativeCardEffectDrawStartedEvent>().Single(e => e.UseFrameId == original.ResolutionId);
        Need(issued.RequestedCount == 2 && issued.RecipientSeat == 0 &&
            s.UseFrame(original.ResolutionId).NativeCardEffectDraw is { RequestedCount: 2, Outcome.ActualDrawIds.Count: 2 },
            "An unowned source 2030 does not enhance the real resolved DrawTwo request.");

        // The request has already drawn its exact entities; buy during its first
        // real gain child and require that same child/receipt to survive payment.
        var child = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single().Id;
        s.Buy();
        Need(s.Market().Gold == 700 && s.GainPrompt() &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single().Id == child &&
            s.UseFrame(original.ResolutionId).NativeCardEffectDraw is { RequestedCount: 2, Outcome.ActualDrawIds.Count: 2 } &&
            s.View().Skills!.Count(skill => skill.ContentId == StandardShanheErshengsanPackage.SkillId) == 1 &&
            s.Events<ShanheMarketPurchaseCommittedEvent>() is [{ Price: 300 }] &&
            s.Events<ShanheMarketPurchaseAppliedEvent>().Length == 1,
            "The actual 300-coin purchase grants the skill immediately and preserves the original already-issued two-card request and child.");
        s.Play();
        s.AssertPaidUse(first, 2, 1);
        Need(s.View().HandCount == initialHand - 1 + 2 && s.SeenDraws.Count == 2,
            "The interrupted unowned request finishes two gain children without retroactive extra cards.");

        var second = s.Use(); s.Play(); s.AssertPaidUse(second, 3, 1);
        var next = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == second);
        Need(s.Events<NativeCardEffectDrawStartedEvent>().Single(e => e.UseFrameId == next.ResolutionId) is
                { RequestedCount: 3, RecipientSeat: 0 } &&
            s.View().HandCount == initialHand - 2 + 5 && s.SeenDraws.Count == 5 &&
            s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) &&
                m.Reason == CardMoveReasons.Draw) == 5,
            "The next actual paid DrawTwo immediately draws three, each original native gain child returns once, and no unrelated draw changes.");
    }

    public static void NullificationAndFengziRetainOriginalPaidUseAndEffectQuantity()
    {
        var cancelled = Scene.Start(); cancelled.Buy();
        var before = cancelled.View().HandCount;
        var counter = cancelled.View().Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        var card = cancelled.Use();
        cancelled.Until(() => cancelled.G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        cancelled.Answer(c => c.Cards.SequenceEqual([counter]) && !c.Parameters.ContainsKey("conversion-skill-id"));
        cancelled.Play();
        var use = cancelled.Events<CardUseDeclaredEvent>().Single(e => e.CardId == card);
        Need(cancelled.Events<NullificationResolvedEvent>() is [{ EffectNullified: true }] &&
            !cancelled.Events<NativeCardEffectDrawStartedEvent>().Any(e => e.UseFrameId == use.ResolutionId) &&
            !cancelled.Events<NativeCardEffectDrawAppliedEvent>().Any(e => e.UseFrameId == use.ResolutionId) &&
            cancelled.SeenDraws.Count == 0 && cancelled.View().HandCount == before - 2 &&
            cancelled.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.ResolutionId) == 1 &&
            cancelled.G.CardMovements.Count(m => m.CardId == counter && m.From == CardLocation.Hand(0) &&
                m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Nullification) == 1 &&
            cancelled.G.CardMovements.Count(m => m.CardId == counter && m.From == CardLocation.Processing &&
                m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.NullificationFinished) == 1,
            "The real paid counterspell cancels the owned DrawTwo before any draw request; use completion never awards a substitute extra card.");
        cancelled.AssertMaterial(card);

        var repeated = Scene.Start(fengzi: true); repeated.Buy();
        var hand = repeated.View().HandCount;
        var cost = repeated.View().Hand.Single(c => c.Kind == CardKind.Dismantlement).Id;
        var twice = repeated.Use(); repeated.Play(payFengzi: true); repeated.AssertPaidUse(twice, 3, 2);
        Need(repeated.Events<ExtraCardEffectIssuedEvent>().Length == 1 && repeated.SeenDraws.Count == 6 &&
            repeated.View().HandCount == hand - 2 + 6 &&
            repeated.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(0) &&
                m.To == CardLocation.DiscardPile) == 1,
            "Production Fengzi pays one real separate Trick cost, repeats the original native effect for another three cards, and never pays the root card again.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public HashSet<int> SeenDraws { get; } = [];
        private HashSet<long> SeenBatches { get; } = [];
        public PlayerSnapshot View() => G.CreateSnapshot(0).Players[0];
        public ShanheCampaignSnapshot Market() => G.CreateShanheMarketSnapshot(0)!;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardUseFrame UseFrame(long id) => G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == id);
        public bool GainPrompt() => G.PendingDecision?.SkillPrompt?.SkillId == Gain &&
            G.PendingDecision.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "gained");
        public void Submit(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real Ershengsan command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = G.PendingDecision!;
            var choice = prompt.Choices.FirstOrDefault(predicate) ?? throw new InvalidOperationException(JsonSerializer.Serialize(prompt));
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
        }
        public void Until(Func<bool> done, bool payFengzi = false)
        {
            for (var step = 0; step < 160; step++)
            {
                if (done()) return;
                if (GainPrompt())
                {
                    var window = G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single();
                    var owner = UseFrame(window.ResumeNativeCardEffectDrawFrameId!.Value);
                    var receipt = owner.NativeCardEffectDraw!;
                    var outcome = receipt.Outcome;
                    Need(receipt.Stage == NativeCardEffectDrawStage.DrawingChildren && outcome is not null &&
                        window.Batch.ParentFrameId == owner.Id && SeenBatches.Add(window.Batch.Id) &&
                        window.Batch.Movements.Count == 1 && window.Batch.Movements.All(m =>
                            outcome.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile &&
                            m.To == CardLocation.Hand(0) && SeenDraws.Add(m.CardId)) &&
                        G.CardMovements.Last(m => m.CardId == owner.CardId).To == CardLocation.Processing &&
                        !Events<CardUseFinishedEvent>().Any(e => e.ResolutionId == owner.Id),
                        "Each real private DrawOne gain child returns to its exact original paid DrawTwo before root material cleanup.");
                    Answer(c => c.Parameters.GetValueOrDefault("option-id") == "gained");
                }
                else if (G.PendingDecision is { PlayerSeat: 0 } prompt)
                {
                    if (payFengzi && prompt.SkillPrompt?.SkillId == Fengzi &&
                        prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"))
                        Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
                    else if (payFengzi && prompt.SkillPrompt?.SkillId == Fengzi &&
                        prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "cost"))
                        Answer(c => c.Parameters.GetValueOrDefault("branch") == "cost" &&
                            c.Cards.Any(id => View().Hand.Any(card => card.Id == id && card.Kind == CardKind.Dismantlement)));
                    else if (prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip"))
                        Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                    else if (prompt.Kind == DecisionKind.Nullification)
                        Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
                    else throw new InvalidOperationException("Unexpected Ershengsan prompt: " + JsonSerializer.Serialize(prompt));
                }
                else
                {
                    Need(G.State.Winner == Winner.None, "The bounded draw scene cannot end while an original paid use is waiting.");
                    Submit(new AdvanceOneStepCommand(G.Revision));
                }
            }
            throw new InvalidOperationException("The real Ershengsan effect did not reach its bounded native continuation.");
        }
        public void Play(bool payFengzi = false) => Until(() => G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, payFengzi);
        public void Buy()
        {
            var item = Market().ShopItems.Single(i => i.Reward?.Id == StandardShanheCampaignCatalog.CurrentErshengsanRewardId);
            Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision));
            Until(() => !G.HasPendingShanheMarketPurchase);
        }
        public int Use()
        {
            var action = G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.DrawTwo && a.CardId is > 0);
            var id = action.CardId!.Value;
            Submit(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, G.PendingDecision!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            return id;
        }
        public void AssertMaterial(int id)
        {
            Need(G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
                G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
                "The original physical DrawTwo is paid and cleaned once.");
        }
        public void AssertPaidUse(int id, int count, int rounds)
        {
            var use = Events<CardUseDeclaredEvent>().Single(e => e.CardId == id);
            var started = Events<NativeCardEffectDrawStartedEvent>().Where(e => e.UseFrameId == use.ResolutionId).ToArray();
            var applied = Events<NativeCardEffectDrawAppliedEvent>().Where(e => e.UseFrameId == use.ResolutionId).ToArray();
            Need(started.Length == rounds && started.All(e => e.RequestedCount == count && e.RecipientSeat == 0) &&
                applied.Length == rounds && applied.All(e => e.ActualDrawCount == count && e.RecipientSeat == 0) &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.ResolutionId) == 1 &&
                !G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == use.ResolutionId),
                "Each genuine original effect owns its frozen quantity and returns before exactly one root finish.");
            AssertMaterial(id);
        }
        public static Scene Start(bool fengzi = false)
        {
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentErshengsanRewardId, "二生三")
                    { TacticSkillId = StandardShanheErshengsanPackage.SkillId, Rarity = 2 },
                new("supplies", "原生无中试牌", StartingCards: [new(CardKind.DrawTwo, Suit.Heart, 3),
                    new(CardKind.DrawTwo, Suit.Heart, 4), new(CardKind.Dismantlement, Suit.Spade, 5),
                    new(CardKind.Nullification, Suit.Spade, 11)])
            ];
            var definition = new ShanheCampaignDefinition("fixture:ershengsan", "真实无中增强", [Owner], ["supplies"],
                [new("supplies", "实体补给", ShanheNodeKind.Event, ["shop", "battle"],
                    Choices: [new("take", "领取实体补给", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"],
                    Choices: [new("buy", "二生三", 300, RewardId: rewards[0].Id, LeavesNode: false)]),
                 new("battle", "固定守军", ShanheNodeKind.Battle, [],
                    [new("guard", "守军", [new(Enemy) { StartingHand = [] }])])], rewards, StartingGold: 1000);
            var campaign = ShanheCampaignSession.Create(definition, new(23, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { revision => new MoveShanheNodeCommand("supplies", revision),
                  revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                  revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = campaign.Submit(command(campaign.Revision)); Need(result.Accepted, result.Error ?? "The real draw scene could not enter battle."); }
            var setup = campaign.PendingBattleSetup! with { ExtraSkillIds = fengzi ? [Fengzi] : [] };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-ershengsan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:wang-rong");
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Gain}}","revision":1,
                  "triggers":[{"id":"real-native-draw-gain","window":"cardsGained","subject":"owner",
                    "condition":{"kind":"phaseIsPlay"},"destinationZones":["hand"],"movementOccurrence":"perBatch",
                    "movementReasons":["{{CardMoveReasons.Draw.Value}}"],"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"gained"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Gain] = new { name = "原生摸牌子窗口", description = "真实获得实体牌。",
                        optionLabels = new Dictionary<string, string> { ["gained"] = "继续原摸牌" } } } }));
            builder.AddSkill(new(Gain, "原生摸牌子窗口", "真实获得实体牌。")
                { Program = catalog.Programs[Gain], ProgramPresentation = catalog.Presentations[Gain] });
            builder.AddGeneral(new(Owner, "无中使用者", "supporter", Gain, "wu", 20));
            builder.AddGeneral(new(Enemy, "固定守军", "supporter", "standard:none", "wei", 20));
            builder.AddDeck(new(Deck, "零自然摸牌实体闪", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "二生三实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
