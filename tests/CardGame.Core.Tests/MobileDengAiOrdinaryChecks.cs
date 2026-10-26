using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileDengAiOrdinaryChecks
{
    private const string Tuntian = "mobile:tuntian", Zaoxian = "boundary:zaoxian-mobile", Jixi = "boundary:deng-ai-jixi-mobile";
    private const string Driver = "fixture:mobile-deng-ai-ordinary-driver", Watch = "fixture:mobile-deng-ai-ordinary-watch";
    private const string Mode = "identity:classic-mobile-deng-ai-ordinary", JudgmentReason = "skill.mobile.tuntian";

    public static void NativeDodgeLossJudgesHeartToDiscardAndReturnsOriginalResponseCold()
    {
        var s = Scene.Start();
        s.Use("discard-one", [s.V(0).Hand[0].Id]); s.Play(0);
        s.Need(!s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Tuntian), "An actual own-turn hand discard does not offer ordinary mobile Tuntian.");
        s.End(); s.Play(1);
        var heart = s.V(1).Hand.First(c => c.Suit == Suit.Heart).Id; s.Top(heart);
        var hp = s.V(0).Hp; var dodge = s.V(0).Hand.First(c => c.Kind == CardKind.Dodge).Id;
        s.NativeConversion("slash", [0]); s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 });
        s.Answer(c => c.Cards.SequenceEqual([dodge]) && c.Parameters.GetValueOrDefault("response") == "dodge");
        s.TuntianToFinalJudgment();
        var response = s.Response(CardKind.Dodge, dodge, ProgramCardContinuation.Dodge);
        var nativeId = response.ResponseCompletion!.ParentFrameId;
        s.Need(s.G.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Single().Judgment is { CardId: var id, Suit: Suit.Heart } && id == heart &&
            s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == nativeId && f.SourceSeat == 1 && f.CardKind == CardKind.Slash),
            "The real off-turn Dodge payment freezes the chosen Heart judgment while its exact original Slash and response-cost return are still suspended.");
        s.Cold(); s.Continue(); s.Play(1);
        s.Need(s.V(0).Hp == hp && s.V(0).AuthorityCount == 0 && s.V(0).Hand.All(c => c.Id != heart) && s.Zone(heart) == CardLocation.DiscardPile &&
            s.G.CardMovements.Count(m => m.CardId == heart && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile &&
                m.Reason.Value == "skill-program.mobile:tuntian.MoveBoundCards") == 1 &&
            !s.G.CardMovements.Any(m => m.CardId == heart && m.To == CardLocation.Hand(0)) &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == nativeId) == 1 &&
            s.E<CardResponseCompletedEvent>().Count(e => e.ActionId == response.Action.ActionId) == 1 &&
            s.E<ProgramCardTriggerResolvedEvent>().Count(e => e.SkillId == Watch && e.TriggerId == "actual-response-finished" && e.OwnerSeat == 0 && e.Activated) == 1 &&
            !s.G.ResolutionStack.Any(f => f.Id == response.Id || f.Id == nativeId),
            "Cold ordinary Tuntian discards the original Heart once, never gains it, and returns once to the successfully Dodged native Slash.");
        s.AssertSinglePayment(dodge, CardMoveReasons.Respond, CardMoveReasons.ResponseFinished);
    }

    public static void NativeNullificationLossStoresFieldReturnsAndSharedAwakeningUsesItCold()
    {
        var s = Scene.Start(); var maximum = s.V(0).MaxHp; s.End(); s.Play(1);
        var fields = new List<int>(); var field = s.V(1).Hand.First(c => c.Suit == Suit.Spade).Id; fields.Add(field); s.Top(field);
        var hp = s.V(0).Hp; var counter = s.V(0).Hand.First(c => c.Kind == CardKind.Nullification).Id;
        s.NativeConversion("duel", [0]); s.Reach(() => s.P is { Kind: DecisionKind.Nullification, PlayerSeat: 0 });
        s.Answer(c => c.Cards.SequenceEqual([counter]) && c.Parameters.GetValueOrDefault("response") == "nullification");
        s.TuntianToFinalJudgment();
        var response = s.Response(CardKind.Nullification, counter, ProgramCardContinuation.NullificationResponse);
        var windowId = response.ResponseCompletion!.ParentFrameId;
        var nativeId = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.Duel && f.SourceSeat == 1).Id;
        s.Need(s.G.ResolutionStack.OfType<NullificationWindowFrame>().Any(f => f.Id == windowId) &&
            s.G.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Single().Judgment is { CardId: var id, Suit: Suit.Spade } && id == field,
            "A real Nullification Use loses its original hand entity and pauses a non-Heart Tuntian judgment on the native counterspell's typed cost child.");
        s.Cold(); s.Continue(); s.Play(1);
        s.Need(s.V(0).Hp == hp && s.V(0).AuthorityCount == 1 && s.Zone(field) == CardLocation.Authority(0) &&
            s.E<NullificationResolvedEvent>().Count(e => e.WindowFrameId == windowId && e.EffectNullified) == 1 &&
            s.E<CardResponseCompletedEvent>().Count(e => e.ActionId == response.Action.ActionId) == 1 &&
            s.E<ProgramCardTriggerResolvedEvent>().Count(e => e.SkillId == Watch && e.TriggerId == "actual-response-finished" && e.OwnerSeat == 0 && e.Activated) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == nativeId) == 1 &&
            !s.G.ResolutionStack.Any(f => f.Id == response.Id || f.Id == windowId || f.Id == nativeId),
            "The non-Heart entity really becomes an ordinary field before cold return finishes exactly one effective counterspell and cancels the original Duel.");
        s.AssertSinglePayment(counter, CardMoveReasons.Nullification, CardMoveReasons.NullificationFinished);

        for (var i = 0; i < 2; i++)
        {
            var next = s.V(1).Hand.FirstOrDefault(c => c.Suit != Suit.Heart);
            s.Need(next is not null, "The next ordinary field judgment needs one real non-Heart top material: " +
                JsonSerializer.Serialize(s.V(1).Hand.Select(c => new { c.Id, c.Kind, c.Suit }).ToArray()));
            field = next!.Id; fields.Add(field); s.Top(field);
            s.Use("discard-other", [], [0]); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            s.Answer(_ => true); s.TuntianToFinalJudgment(); s.Continue(); s.Play(1);
        }
        s.Need(s.V(0).AuthorityCount == 3 && fields.All(id => s.Zone(id) == CardLocation.Authority(0)) &&
            s.G.GetCombatDistance(0, 2) == 1 && s.G.GetCombatDistance(2, 0) == 2,
            "Three actual off-turn hand losses store three original non-Heart entities; only Deng Ai's outgoing distance receives the field reduction.");
        s.End(); s.Play(0);
        s.Need(s.V(0).MaxHp == maximum - 1 && s.V(0).Skills!.Any(k => k.ContentId == Jixi) &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Zaoxian && e.Completed) == 1,
            "The genuine next preparation reads ordinary mobile Authority fields, awakens once and grants the exactly reused native Jixi through its original skill source.");
        var actions = s.G.GetLegalActions(0);
        var action = actions.FirstOrDefault(a => a.PlayedCardKind == CardKind.Snatch && a.ConversionSource?.SkillId == Jixi && a.TargetSeats.SequenceEqual([2]));
        s.Need(action is not null, "Awakened Jixi must publish a real field-backed Snatch to the opposite living seat: " + JsonSerializer.Serialize(new {
            Skills = s.V(0).Skills!.Select(k => k.ContentId).ToArray(),
            Fields = fields.Select(id => new { CardId = id, Zone = s.Zone(id) }).ToArray(),
            Actions = actions.Where(a => a.ConversionSource?.SkillId == Jixi || a.CardId is { } id && fields.Contains(id))
                .Take(12).Select(a => new { a.CardId, a.PlayedCardKind, Source = a.ConversionSource, a.TargetSeats }).ToArray() }));
        var material = action!.CardId!.Value; s.Native(action!); s.Play(0);
        s.Need(fields.Contains(material) && s.V(0).AuthorityCount == 2 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Authority(0) && m.To == CardLocation.Processing) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardId == material && e.CardKind == CardKind.Snatch) == 1,
            "Reused Jixi consumes one real ordinary field and completes one genuine distance-two native Snatch without requiring boundary Tuntian ownership.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Native ordinary mobile Deng Ai command rejected."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing published ordinary mobile Deng Ai choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void End() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Use(string id, int[] cards, int[]? targets = null) => Accept(new UseProgramSkillCommand(P!.PlayerSeat, Driver, id, cards, targets ?? [], G.Revision, P.PromptId));
        public void Top(int id) { Use("top", [id]); Play(1); }
        public void NativeConversion(string id, int[] targets) => Native(G.GetLegalActions(P!.PlayerSeat).First(a =>
            a.ConversionSource is { SkillId: Driver } source && source.BindingId == id && a.TargetSeats.SequenceEqual(targets)));
        public void Native(LegalAction a) => Accept(new PlayCardCommand(P!.PlayerSeat, a.CardId!.Value, a.TargetSeats, G.Revision, P.PromptId, a.PlayedCardKind, a.TargetCardId)
            { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources ?? [] });
        public void TuntianToFinalJudgment()
        {
            Reach(() => P?.SkillPrompt?.SkillId == Tuntian && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P?.SkillPrompt?.SkillId == Watch);
        }
        public ProgramCardTriggerWindowFrame Response(CardKind kind, int card, ProgramCardContinuation continuation)
        {
            var root = G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Tuntian);
            var moved = G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(f => f.Id == root.WindowContext!.ParentFrameId);
            var found = G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.ResponseCompletion is not null);
            Need(found is not null, "The fixture's real response-completed consumer must retain the native response-cost return: " + JsonSerializer.Serialize(
                G.ResolutionStack.Select(f => new { f.Id, Kind = f.Kind.ToString(), NativeParent = f is CardsMovedTriggerWindowFrame m ? m.Batch.ParentFrameId :
                    f is ProgramCardTriggerWindowFrame w ? w.ParentFrameId : (long?)null,
                    Action = f is ProgramCardTriggerWindowFrame a ? new { a.Action.ActionId, a.Action.ActorSeat, a.Action.ProviderSeat, a.Action.EffectiveKind, a.Continuation, a.CompletedResponseReturn } : null }).ToArray()));
            var response = found!;
            Need(root.WindowContext!.MovementBatch!.Movements.Any(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) &&
                moved.ResumeResponseCompletionFrameId == response.Id && response.ResponseCompletion is { CostsDrained: false, ActiveCostChildFrameId: var child } r &&
                child == moved.Id && r.OriginalContinuation == continuation && r.ParentFrameId == response.ParentFrameId &&
                response.Action is { Type: CardActionType.Response, ActorSeat: 0, ProviderSeat: 0, ResponderSeat: 0 } a && a.EffectiveKind == kind &&
                a.PhysicalCards is [var cost] && cost.CardId == card && cost.From == CardLocation.Hand(0),
                "Ordinary mobile Tuntian is owned by the exact native hand-loss batch and original physical response's typed cost child.");
            return response;
        }
        public void AssertSinglePayment(int card, CardMoveReason enter, CardMoveReason finish) => Need(
            G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == enter) == 1 &&
            G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == finish) == 1 &&
            E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Tuntian && e.Completed) >= 1,
            "The original physical response enters and finishes once; Tuntian does not charge it again.");
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 260 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } general) { Accept(new SelectGeneralCommand(general.PlayerSeat, General(general.PlayerSeat), G.Revision, general.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } d)
                {
                    var kept = d.PlayerSeat == 0
                        ? new[] { CardKind.Dodge, CardKind.Nullification }.Select(kind => V(0).Hand.FirstOrDefault(c => c.Kind == kind)?.Id).OfType<int>().ToHashSet()
                        : [];
                    var cards = d.ValidCardIds.Where(id => !kept.Contains(id)).Take(d.RequiredCardCount).ToArray();
                    Need(cards.Length == d.RequiredCardCount, "The real discard can pay its complete excess while retaining one response entity of each required kind.");
                    Accept(new DiscardCardsCommand(d.PlayerSeat, cards, d.PromptId, G.Revision)); continue;
                }
                if (P is { } p)
                {
                    var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == "continue") ??
                        p.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? p.Choices.FirstOrDefault();
                    Need(choice is not null, "A native boundary has no published default choice.");
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded ordinary mobile Deng Ai boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.CardMovements.SequenceEqual(copy.CardMovements) && G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))
                    .SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))),
                "Cold recovery preserves the exact response invoice, original movement child and frozen native judgment."); G = copy;
        }
        public void Need(bool ok, string text)
        {
            if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat,
                Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId, Choices = p.Choices.Take(6).Select(c => new { c.Cards, c.Targets, c.Parameters }).ToArray(), Count = p.Choices.Count } : null,
                Facts = G.Events.Select(e => e.Payload).Where(e => e is ProgramBindingStartedEvent or ProgramBindingResolvedEvent or ProgramJudgmentOutcomeRecordedEvent or
                    JudgmentResolvedEvent or CardResponseCompletionStartedEvent or CardResponseCompletedEvent).TakeLast(10).Select(e => JsonSerializer.Serialize(e, e.GetType())).ToArray() }));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry), registry);
            s.Accept(new StartGameCommand()); s.Play(0); return s;
        }
    }
    private static string General(int seat) => "fixture:mobile-deng-ai-ordinary-general-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-deng-ai-ordinary", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:deng-ai", "boundary:deng-ai-mobile");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"discard-one","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]},
                {"id":"top","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"top"},{"op":"moveBoundCards","target":"owner","sourceBind":"top","destination":"drawPileTop","awaitMovementTriggers":true}]},
                {"id":"discard-other","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand","equipment"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}
              ],"viewAs":[
                {"id":"slash","inputKinds":[],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false},
                {"id":"duel","inputKinds":[],"inputSuits":[],"outputKind":"duel","forPlay":true,"forResponse":false,"useOnly":true,"singleCardTrickUse":true}
              ]},
              {"id":"{{Watch}}","revision":1,"triggers":[
                {"id":"final-judgment-child","window":"judgmentFinalized","subject":"owner","suits":["spade","club","heart","diamond","none"],"minimumRank":1,"maximumRank":13,"judgmentReasons":["{{JudgmentReason}}"],"excludedReasons":[],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"judgment-pause","options":[{"id":"continue"}]}]},
                {"id":"actual-response-finished","window":"cardResponseCompleted","ownerRelation":"actor","cardKinds":["dodge","nullification"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"response-pause","options":[{"id":"continue"}]}]}
              ]}
            ]}
            """;
            var labels = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实原生响应准备", description = "原生手牌、弃牌、置顶与合法转化。" },
                    [Watch] = new { name = "真实屯田判定子窗", description = "生效判定保留原响应父帧。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, labels);
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            for (var seat = 0; seat < 4; seat++) b.AddGeneral(new(General(seat), "固定真实参与者", "supporter", seat == 0 ? Tuntian : "standard:none", "wei", seat == 0 ? 4 : 6,
                seat == 0 ? [Zaoxian, Driver, Watch] : [Driver]));
            const string deck = "fixture:mobile-deng-ai-ordinary-deck";
            b.AddDeck(new(deck, "普通移动实体", 12, 0, []) { PhysicalCards = Enumerable.Range(0, 80).SelectMany(_ => new ContentDeckPhysicalCard[]
                { new("standard:dodge", Suit.Spade, 7), new("standard:nullification", Suit.Heart, 9), new("standard:slash", Suit.Club, 2) }).ToArray() });
            b.AddMode(new(Mode, "移动普通邓艾原生响应", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 },
                deck, GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
