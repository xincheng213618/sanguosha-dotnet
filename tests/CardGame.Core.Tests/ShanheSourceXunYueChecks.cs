using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceXunYueChecks
{
    private const string Skill = StandardShanheSourceXunYuePackage.QiangzhiSkillId;
    private const string Deck = "fixture:source-xunyue-deck", Peer = "fixture:source-xunyue-peer", Ending = "fixture:source-xunyue-ending";

    public static void QiangzhiRevealsOneOriginalHandCardAndOnlyDrawsWithinItsSelectedPlayPhase()
    {
        var s = Scene.Start();
        var originals = s.G.CreateSnapshot(1).Players[1].Hand.Select(c => c.Id).ToArray();
        Need(s.V is { Hp: 3, MaxHp: 3 } && s.V.GeneralId == StandardShanheSourceXunYuePackage.BaseId &&
            s.Events<ProgramCardsRevealedEvent>().All(e => e.SkillId != Skill),
            "The actual source4871 three-HP body can choose strong recognition before any hand card has been revealed.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Targets.SequenceEqual([1])));
        s.Answer(c => c.Targets.SequenceEqual([1])); s.Play();
        var shown = s.Events<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Skill).Cards;
        Need(shown is { Count: 1 } && originals.Contains(shown[0].Id) && shown[0].Kind == CardKind.Dodge &&
            s.G.CreateSnapshot(1).Players[1].Hand.Select(c => c.Id).Order().SequenceEqual(originals.Order()) &&
            s.BooleanStates.Single(x => x.StateId == "shown-basic").Value && s.Draws == 0,
            "Strong recognition publicly reveals exactly one original opposing hand entity, leaves the actual hand intact and records only that card's Basic category.");

        s.Use(CardKind.Crossbow); s.Play();
        Need(s.Draws == 0, "A real Equipment use does not offer or draw for the revealed Basic category.");
        var slash = s.Use(CardKind.Slash);
        s.Until(() => s.IsInvocation);
        Need(s.Draws == 0 && s.G.CardMovements.Count(m => m.CardId == slash &&
                m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "The real matching Slash pays its original card once before offering the optional strong-recognition draw.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Play();
        var declared = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == slash);
        Need(s.Draws == 1 && s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == declared.ResolutionId) == 1,
            "Accepting the matching-use child draws exactly one actual card and returns to finish the original paid Slash.");

        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Ending);
        Need(s.G.State.TurnNumber == 1 && s.G.State.CurrentSeat == 0 &&
            s.BooleanStates.All(x => !x.Value) && s.Draws == 1,
            "The source484 category is already cleared at the real own Ending boundary, before the same turn ends.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue-ending");
        s.Until(() => s.G.State.TurnNumber == 3 && s.IsInvocation);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play();
        s.Use(CardKind.Slash); s.Play();
        Need(s.Draws == 1 && s.Events<ProgramCardsRevealedEvent>().Count(e => e.SkillId == Skill) == 1 &&
            s.BooleanStates.All(x => !x.Value),
            "A later real Play phase starts fresh: declining its reveal leaves a matching Basic use without an inherited offer or draw.");
        NativeNullificationUseReturnsToItsOriginalTrick();
    }

    private static void NativeNullificationUseReturnsToItsOriginalTrick()
    {
        var s = Scene.Start(counterspell: true);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Targets.SequenceEqual([1])));
        s.Answer(c => c.Targets.SequenceEqual([1])); s.Play();
        Need(s.Events<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Skill).Cards is [{ Kind: CardKind.DrawTwo }] &&
            s.BooleanStates.Single(x => x.StateId == "shown-trick").Value,
            "A separate fresh source484 scene records the actual single opposing Trick entity for this Play phase.");

        var duelId = s.Use(CardKind.Duel);
        s.Until(() => s.IsInvocation);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        s.Until(() => s.P is { Kind: DecisionKind.Nullification, PlayerSeat: 0 });
        var parent = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == duelId);
        var counterWindow = s.G.ResolutionStack.OfType<NullificationWindowFrame>().Single();
        var nullificationId = s.V.Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        Need(counterWindow.ParentFrameId == parent.ResolutionId && s.Draws == 0,
            "Declining the parent Duel's optional draw leaves its original native counterspell window and does not draw.");
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "nullification" && c.Cards.SequenceEqual([nullificationId]));
        s.Until(() => s.IsInvocation);
        var responseWindow = s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(f =>
            f.Action.EffectiveKind == CardKind.Nullification && f.CompletedResponseReturn is { IsCommitted: true });
        Need(s.G.State.CurrentSeat == 0 && s.G.State.Phase == TurnPhase.Play && s.Draws == 0 &&
            responseWindow.ParentFrameId == counterWindow.Id &&
            responseWindow.CompletedResponseReturn is { Kind: ProgramCompletedResponseKind.Nullification } &&
            responseWindow.Action is { Type: CardActionType.Response, ActorSeat: 0, ProviderSeat: 0, ResponderSeat: 0, RequesterSeat: null } &&
            responseWindow.Action.PhysicalCards.Select(c => c.CardId).SequenceEqual([nullificationId]) &&
            s.G.CardMovements.Count(m => m.CardId == nullificationId && m.From == CardLocation.Hand(0) &&
                m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Nullification) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == nullificationId && m.From == CardLocation.Processing &&
                m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.NullificationFinished) == 1 &&
            s.Events<CardUseFinishedEvent>().All(e => e.ResolutionId != parent.ResolutionId),
            "The matching response-use pays its real Nullification entity once, then source484 suspends that same committed response and original Duel before either returns.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Play();
        Need(s.Draws == 1 && s.Events<NullificationRespondedEvent>().Count(e =>
                e.WindowFrameId == counterWindow.Id && e.NullificationCardId == nullificationId && e.ResponderSeat == 0) == 1 &&
            s.Events<NullificationResolvedEvent>().Count(e => e.WindowFrameId == counterWindow.Id &&
                e.ResolutionId == parent.ResolutionId && e.EffectCardId == duelId && e.EffectNullified) == 1 &&
            s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == parent.ResolutionId && e.CardId == duelId) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == duelId && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == duelId && m.From == CardLocation.Processing &&
                m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == nullificationId && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            !s.Events<DamageAppliedEvent>().Any() && !s.G.ResolutionStack.OfType<NullificationWindowFrame>().Any(),
            "The real response-use draws exactly one and returns to resolve the same counterspell and cancel/finish its parent Duel, with no repeated payment or damage.");
    }

    private static void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public bool IsInvocation => P?.SkillPrompt?.SkillId == Skill &&
            P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public IReadOnlyList<ProgramBooleanStateSnapshot> BooleanStates =>
            V.SkillRuntimeStates!.Single(x => x.SkillId == Skill).BooleanStates!;
        public int Draws => G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.From == CardLocation.DrawPile &&
            m.Reason.Value == "skill-program." + Skill + ".Draw");
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, (result.Error?.Message ?? "Rejected native source2480 command.") + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Need(p.PlayerSeat == 0, "Only the actual human answers the source2480 decision."); Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public int Use(CardKind kind)
        {
            var card = V.Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null &&
                (kind != CardKind.Slash || a.TargetSeats.SequenceEqual([1])));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            return card.Id;
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None && P is null,
                    "The bounded source2480 flow encountered an unexpected human offer or ended a battle. " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded source2480 flow missed its real boundary. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new
        { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Pending = P, Draws, Stack = G.ResolutionStack.Select(f => new { f.Id, Type = f.GetType().Name }) });
        public static Scene Start(bool counterspell = false)
        {
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-xunyue", StandardShanheSourceXunYuePackage.BaseId,
                3, 0, 0, 0, 0, [Ending],
                counterspell ? [new(CardKind.Duel, Suit.Spade, 1), new(CardKind.Nullification, Suit.Club, 12)] :
                    [new(CardKind.Crossbow, Suit.Club, 5), new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Slash, Suit.Club, 8)], [],
                [new(Peer) { StartingHand = counterspell ? [new(CardKind.DrawTwo, Suit.Heart, 7)] :
                    [new(CardKind.Dodge, Suit.Spade, 2), new(CardKind.Dodge, Suit.Club, 3)] }], [], new string('a', 64));
            var production = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            { EnemyRoster = [new("classic:guan-yu")], ExtraSkillIds = [] });
            var registry = production.WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId
                ? mode with { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Append(Peer).Distinct(StringComparer.Ordinal).ToArray() } : mode);
            var s = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry));
            s.Accept(new StartGameCommand()); s.Until(() => s.IsInvocation); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-xunyue", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var skills = new[] { new
            {
                id = Ending, revision = 1, triggers = new[] { new
                {
                    id = "after-real-play", window = "turnEnding", subject = "owner", turnOwnerScope = "own", optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "ending", options = new[] { new { id = "continue-ending" } } } }
                } }
            } };
            var catalog = SkillProgramCatalog.Load(
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Ending] = new { name = "实际结束阶段", description = "在真实出牌阶段结束后停留。",
                        optionLabels = new Dictionary<string, string> { ["continue-ending"] = "继续结束阶段" } } } }));
            builder.AddSkill(new(Ending, "实际结束阶段", "在真实出牌阶段结束后停留。")
            { Program = catalog.Programs[Ending], ProgramPresentation = catalog.Presentations[Ending] });
            builder.AddGeneral(new(Peer, "真实持牌目标", "supporter", null, "wei", 12));
            builder.AddDeck(new(Deck, "仅供真实强识摸牌的闪", 0, 0, [new("standard:dodge", 24)]));
        }
    }
}
