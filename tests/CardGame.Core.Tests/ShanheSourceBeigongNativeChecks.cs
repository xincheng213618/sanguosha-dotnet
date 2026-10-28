using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceBeigongNativeChecks
{
    private const string Dingxi = StandardShanheSourceBeigongDuoPackage.DingxiSkillId, Huojie = StandardShanheSourceBeigongDuoPackage.HuojieSkillId;
    private const string Actor = "fixture:source-beigong-owner", Peer = "fixture:source-beigong-peer", Deck = "fixture:source-beigong-deck";
    private const string Entry = "fixture:source-beigong-entry", EndingDraw = "fixture:source-beigong-ending-draw", Hit = "fixture:source-beigong-hit", Gain = "fixture:source-beigong-gain";
    private const string Prevent = "fixture:source-beigong-prevent-first", Retrial = "classic:guicai", Quiet = "fixture:source-beigong-quiet";

    public static void OriginalDingxiEntitiesReturnAndHuojieContinuesPastPreventedLightningUntilActualDamage()
    {
        var s = Scene.Start();
        var cards = s.V().Hand.Where(c => c.Kind == CardKind.Duel).Select(c => c.Id).ToArray();
        Need(cards.Length == 4 && s.Pile().Length == 0, "The fresh three-seat encounter starts with four actual Duel materials and no fabricated pile.");
        foreach (var id in cards)
        {
            s.UseDuel(id, 1); s.OfferDingxi();
            var f = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(p => p.SourceDingxi is not null);
            var original = s.E<CardUseDeclaredEvent>().Single(u => u.ResolutionId == f.SourceDingxi!.OriginalUseId);
            var qualified = s.E<SourceDingxiDiscardQualifiedEvent>().Single(e => e.BatchId == f.SourceDingxi!.BatchId && e.CardId == id);
            var entry = s.E<NativeTrueUseProcessingEntryEvent>().Single(e => e.ParentFrameId == original.ResolutionId && e.CardId == id);
            Need(f.SourceDingxi is { Stage: SourceDingxiStage.Choose, LeftSeat: 2 } &&
                s.P!.Choices.All(c => c.Cards.SequenceEqual([id]) && c.Targets.SequenceEqual([2])) &&
                s.Zone(id) == CardLocation.DiscardPile && original.CardId == id && original.SourceSeat == 0 &&
                original.CardKind == CardKind.Duel && qualified.OriginalUseId == original.ResolutionId &&
                qualified.OwnerSeat == 0 && qualified.OriginalActionId == f.SourceDingxi.OriginalActionId &&
                entry.ParentActionId == qualified.OriginalActionId && entry.From == CardLocation.Hand(0) &&
                entry.Kind == NativeTrueUseDiscardKind.Use && entry.ProviderSeat == 0 && entry.CardKind == CardKind.Duel &&
                s.G.CardMovements.Count(m => m.Sequence == entry.EntrySequence && m.CardId == id &&
                    m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(m => m.Sequence == qualified.DiscardSequence && m.CardId == id &&
                    m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                s.E<TargetsConfirmedEvent>().Any(e => e.ResolutionId == original.ResolutionId && e.TargetSeats.SequenceEqual([1])) &&
                s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == original.ResolutionId && e.CardId == id) == 1 &&
                s.G.ResolutionStack.OfType<CardUseFrame>().All(u => u.Id != original.ResolutionId),
                "The original real use has finished and popped; its exact declaration, action/material entry and discard invoice qualify only the same available damage card for the decreasing-seat nearest living actor. " + s.Diagnostic());
            s.Answer(c => c.Cards.SequenceEqual([id]));
            if (id == cards[0])
            {
                s.Until(() => s.P?.SkillPrompt?.SkillId == Entry);
                var paid = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == f.Id);
                var r = paid.SourceDingxi!;
                var native = s.G.ResolutionStack.OfType<CardUseFrame>().Single(u => u.Id == r.UseId);
                Need(r is { Stage: SourceDingxiStage.NativeUse, CardId: var card } && card == id &&
                    native.SourceDingxiUse is { ProgramFrameId: var owner } && owner == paid.Id &&
                    native.Action is { Type: CardActionType.Use, ConversionChain.Count: 0, PhysicalCards: [var cost] } &&
                    cost.CardId == id && cost.From == CardLocation.Processing && native.TargetSeats.SequenceEqual([2]) &&
                    s.Zone(id) == CardLocation.Processing &&
                    s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.DiscardPile && m.To == CardLocation.Processing &&
                        m.Reason.Value == "skill-program.source-dingxi.enter") == 1 &&
                    s.E<SourceDingxiEnteredEvent>() is [{ CardId: var entered }] && entered == id &&
                    s.E<SourceDingxiIssuedEvent>() is [{ Origin.CardId: var issuedCard }] && issuedCard == id &&
                    s.E<SourceDingxiReturnedEvent>().Length == 0 &&
                    s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == original.ResolutionId && e.CardId == id) == 1 &&
                    s.E<CardUseFinishedEvent>().All(e => e.ResolutionId != native.Id),
                    "The second real Duel use waits for its committed-use child on the same original material from Processing; its first use is already finished, while this second native use has not returned.");
                s.Continue();
            }
            s.Play();
            var issued = s.E<SourceDingxiIssuedEvent>().Single(e => e.Origin.CardId == id).Origin;
            Need(issued.ProgramFrameId == f.Id && issued.OriginalUseId == original.ResolutionId && issued.LeftSeat == 2 &&
                issued.OriginalActionId == qualified.OriginalActionId && issued.Kind == CardKind.Duel && issued.UseId != original.ResolutionId &&
                s.E<SourceDingxiReturnedEvent>().Count(e => e.ProgramFrameId == f.Id && e.CardId == id && e.UseId == issued.UseId) == 1 &&
                s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == issued.UseId && e.CardId == id) == 1 &&
                s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == original.ResolutionId && e.CardId == id) == 1 &&
                s.E<SourceDingxiStoredEvent>().Count(e => e.ProgramFrameId == f.Id && e.CardId == id) == 1 &&
                s.Pile().Contains(id) && s.Zone(id).Zone == CardZoneKind.PublicPersistentPile,
                "The whole second actual use returns once and stores its same original physical entity; its already completed initial use is neither extended nor completed twice. " + s.Diagnostic());
        }
        var publicView = s.G.CreateSnapshot(1).Players[0];
        Need(s.Pile().Order().SequenceEqual(cards.Order()) && publicView.PublicPersistentPileSkillId == Dingxi &&
                publicView.PublicPersistentPileCards!.Select(c => c.Id).Order().SequenceEqual(cards.Order()),
            "All four real original entities are visible in the source-owned public Dingxi pile, exceeding the initial three actors.");
        var hand = s.V().HandCount;
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == EndingDraw);
        var draw = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SourceDingxi?.Stage == SourceDingxiStage.DrawChildren);
        Need(draw.SkillId == Dingxi && s.V().HandCount == hand + 4 && s.Pile().Length == 4 &&
            s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.source-dingxi.draw") == 4 &&
            s.E<ProgramBindingResolvedEvent>().All(e => e.FrameId != draw.Id),
            "The actual ending phase draws the current pile size and waits on its real gain child without consuming the public pile.");
        s.Continue();

        s.RejudgeWithActualSpade();
        s.Until(() => s.P?.SkillPrompt?.SkillId == Prevent);
        var huojie = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SourceHuojie is not null);
        var hp = s.V().Hp;
        Need(huojie.SourceHuojie is { MaximumJudgments: 4, Done: 1, Stage: SourceHuojieStage.DamageChildren } &&
            s.Pile().Length == 4 && s.E<SourceHuojieReceivedDamageEvent>().Length == 0 &&
            s.E<SourceHuojieJudgmentIssuedEvent>() is [{ Maximum: 4, Ordinal: 1 }],
            "At the next actual Play start, four stored cards exceed three original actors; the first real Lightning hit has not yet dealt damage or obtained the pile.");
        s.Continue();
        s.RejudgeWithActualSpade(() => Need(s.V().Hp == hp && s.Pile().Length == 4 && s.E<ProgramDamagePreventedEvent>().Count(e => e.SkillId == Prevent) == 1 &&
            s.E<SourceHuojieJudgmentIssuedEvent>().Select(e => e.Ordinal).SequenceEqual([1, 2]) &&
            s.E<SourceHuojieReceivedDamageEvent>().Length == 0 && s.E<SourceHuojieCompletedEvent>().Length == 0,
            "Preventing the first real hit preserves the pile and reaches the second actual replacement decision before its payment. " + s.Diagnostic()));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Hit);
        Need(s.V().Hp == hp - 3 && s.Pile().Length == 4 && s.E<SourceHuojieReceivedDamageEvent>() is [{ ProgramFrameId: var parent, Amount: 3 }] && parent == huojie.Id &&
            s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 0 && e.Nature == DamageNature.Thunder && e.SourceLess && e.Amount == 3) == 1 &&
            s.E<SourceHuojieCompletedEvent>().Length == 0,
            "Only actual unprevented three-point source-less Thunder damage earns the pile, after its real damage child finishes.");
        s.Continue(); s.Until(() => s.P?.SkillPrompt?.SkillId == Gain);
        var gaining = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == huojie.Id);
        Need(gaining.SourceHuojie is { Stage: SourceHuojieStage.GainChildren, Done: 2 } &&
            s.Pile().Length == 0 && cards.All(id => s.Zone(id) == CardLocation.Hand(0)) &&
            s.E<ProgramBindingResolvedEvent>().All(e => e.FrameId != huojie.Id),
            "The original four entities really enter Hand and their gain child suspends the same paid Huojie parent before it returns.");
        s.Continue(); s.Play();
        Need(s.E<SourceHuojieCompletedEvent>() is [{ ProgramFrameId: var completed, Judgments: 2, ReceivedDamage: true }] && completed == huojie.Id &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == huojie.Id && e.Completed) == 1 &&
            cards.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.To == CardLocation.Hand(0) &&
                m.Reason.Value == "skill-program.source-huojie.obtain") == 1) && s.G.State.Winner == Winner.None,
            "The real damage and gain children return once; Huojie stops after actual damage and resumes the actual Play phase with the original cards.");
    }

    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int[] Pile() => V().PublicPersistentPiles is { } piles
            ? piles.Where(p => p.SourceSkillId == Dingxi).SelectMany(p => p.Cards.Select(c => c.Id)).ToArray()
            : V().PublicPersistentPileSkillId == Dingxi ? (V().PublicPersistentPileCards ?? []).Select(c => c.Id).ToArray() : [];
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, (r.Error?.Message ?? "Rejected source2450 command.") + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> match)
        { var p = P!; var choice = p.Choices.FirstOrDefault(match); Need(choice is not null, "Missing source2450 actual choice. " + Diagnostic()); Accept(new AnswerPromptCommand(0, p.PromptId, choice!.Id, G.Revision)); }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void UseDuel(int id, int target)
        { var a = G.GetHumanLegalActions().Single(a => a.CardId == id && a.ConversionSource is null && a.TargetSeats.SequenceEqual([target])); Accept(new PlayCardCommand(0, id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)); }
        public void OfferDingxi()
        {
            Until(() => P?.SkillPrompt?.SkillId == Dingxi);
            if (P!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Until(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "source-dingxi-card") == true);
        }
        public void RejudgeWithActualSpade(Action? beforePayment = null)
        {
            Until(() => P?.Kind == DecisionKind.ProgramJudgmentReplacement);
            beforePayment?.Invoke();
            var card = V().Hand.First(c => c.Kind == CardKind.Dodge && c.Suit == Suit.Spade && c.Rank is >= 2 and <= 9);
            Answer(c => c.Cards.SequenceEqual([card.Id]));
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var i = 0; i < 260; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The fresh source2450 short encounter stays live. " + Diagnostic());
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.SkillPrompt?.SkillId == Entry) Continue();
                    else if (p.SkillPrompt?.SkillId == Retrial && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"))
                        Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
                    else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage"))
                        Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    else throw new InvalidOperationException("Unexpected source2450 native boundary. " + Diagnostic());
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source2450 exceeded bounded native progression. " + Diagnostic());
        }
        public string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, P,
            Stack = G.ResolutionStack.Select(f => new { f.Id, Kind = f.GetType().Name }),
            Recent = G.Events.TakeLast(5).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start()
        {
            var setup = new ShanheBattleSetup(211, 1, "source-beigong-native", Actor, 30, 0, 0, 0, 0, [],
                [new(CardKind.Duel, Suit.Club, 1), new(CardKind.Duel, Suit.Spade, 1), new(CardKind.Duel, Suit.Diamond, 1), new(CardKind.Duel, Suit.Heart, 1),
                 new(CardKind.Dodge, Suit.Spade, 3), new(CardKind.Dodge, Suit.Spade, 4)], [],
                [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }], [], new string('d', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            { PlayerGeneralId = StandardShanheSourceBeigongDuoPackage.BeigongBaseId, StartingCards = [],
                EnemyRoster = [new("classic:guan-yu"), new("classic:guan-yu")] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Actor, Peer]).Distinct().ToArray() } : mode);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-beigong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Entry}}","revision":1,"triggers":[{"id":"actual-use-child","window":"cardUseCommitted","ownerRelation":"actor","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{EndingDraw}}","revision":1,"triggers":[{"id":"real-ending-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.source-dingxi.draw"],"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Hit}}","revision":1,"triggers":[{"id":"actual-received-lightning","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Gain}}","revision":1,"triggers":[{"id":"real-pile-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.source-huojie.obtain"],"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Prevent}}","revision":1,"triggers":[{"id":"one-real-prevention","window":"beforeDamageApplied","subject":"damageTarget","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]},{"op":"preventCurrentDamage","target":"owner"}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Entry, EndingDraw, Hit, Gain, Prevent, Quiet }.ToDictionary(id => id, id => new
                    { name = id, description = "北宫来源原生成功流程", optionLabels = new[] { Entry, EndingDraw, Hit, Gain, Prevent }.Contains(id)
                        ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() }) }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Actor, "定西祸结真实持有者", "supporter", Dingxi, "qun", 30, [Huojie, Entry, EndingDraw, Hit, Gain, Prevent, Retrial]));
            builder.AddGeneral(new(Peer, "存活原生响应角色", "supporter", Quiet, "wei", 20));
            builder.AddDeck(new(Deck, "小型真实摸牌堆", 0, 2, [new("standard:dodge", 60)]));
        }
    }
}
