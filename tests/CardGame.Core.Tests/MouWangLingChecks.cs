using System.Runtime.CompilerServices;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

namespace CardGame.Core.Tests;

internal static class MouWangLingChecks
{
    private const string Hero = "mou:wang-ling", Hold = "fixture:mouwl-gift-hold", Quiet = "fixture:mouwl-quiet";

    public static void ActualEndingGiftsFiveNamesAwakenAndRetainDuplicatesCold()
    {
        var s = Scene.Start(false);
        s.Play();
        var firstTurn = s.E<TurnStartedEvent>().Single();
        s.Keep = s.V().Hand.Single(c => c.Kind == CardKind.FireAttack).Id;
        var names = new[] { CardKind.Slash, CardKind.DrawTwo, CardKind.Duel, CardKind.IronChain };
        var cards = names.Select(kind => s.V().Hand.Single(c => c.Kind == kind).Id).ToArray();
        s.Native(cards[0], 3); s.Play();
        s.Native(cards[1]); s.Play();
        s.Native(cards[2], 2); s.Play();
        s.Native(cards[3], 1); s.Play();
        s.End(); s.BeginJichou();
        for (var i = 0; i < cards.Length; i++)
        {
            var card = cards[i];
            s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Cards.SequenceEqual([card]));
            var seat = i; s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "recipient" && c.Targets.SequenceEqual([seat]));
        }
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "confirm");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
        s.Need(s.E<MouWangLingGiftEvent>() is [{ RecipientSeat: 0, FirstName: true }] &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MouWangLing is { Stage: MouWangLingStage.GiftChildren, Cursor: 1, Paid: true }),
            "A genuine first DiscardPile gift opens its native gain child before the other three gifts or Draw4.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold && s.G.ResolutionStack.OfType<NativeDrawAttemptFrame>().Any());
        var stack = s.G.ResolutionStack.ToArray();
        var drawing = stack.OfType<ProgramSkillFrame>().Single(frame => frame.MouWangLing is not null);
        var attempt = stack.OfType<NativeDrawAttemptFrame>().Single();
        var index = Array.IndexOf(stack, attempt);
        var gifts = s.E<MouWangLingGiftEvent>();
        s.Need(drawing is { OwnerSeat: 0, SkillId: "mou:jichou", MouWangLing:
                { Stage: MouWangLingStage.Drawing, Paid: true, Cursor: 4, FirstNames: 4, Requested: 4, Draw: null } receipt } &&
            drawing.WindowContext?.Window == SkillProgramTriggerWindow.TurnEnding && receipt.ActualTurn == firstTurn.TurnNumber &&
            gifts.Select(e => e.RecipientSeat).SequenceEqual([0, 1, 2, 3]) && gifts.Select(e => e.CardId).SequenceEqual(cards) &&
            gifts.All(e => e.FrameId == drawing.Id && e.FirstName && s.G.CardMovements.Count(move =>
                move.CardId == e.CardId && move.Sequence == e.Sequence && move.From == CardLocation.DiscardPile &&
                move.To == CardLocation.Hand(e.RecipientSeat)) == 1) &&
            index > 0 && stack[index - 1].Id == drawing.Id && drawing.NativeDraw?.ActiveAttemptId == attempt.Id &&
            attempt.Context.ParentFrameId == drawing.Id && attempt.Context is { RecipientSeat: 0, RequestedCount: 4 } &&
            attempt.Context.Reason.Value == "skill-program.mou:jichou.draw" && attempt.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation &&
            s.E<MouWangLingDrawReturnedEvent>().Length == 0 && s.E<TurnStartedEvent>().SequenceEqual([firstTurn]),
            "Four completed physical gifts bind the original Ending frame to a native Draw4 before another actual turn begins.");
        s.Cold();
        var restoredParent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == drawing.Id);
        var restoredAttempt = s.G.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == attempt.Id);
        s.Need(restoredAttempt.Context == attempt.Context && s.E<MouWangLingGiftEvent>().SequenceEqual(gifts) &&
            CoreGameEngineNativeDrawAttemptsRuntime.MatchesProgramDrawAttempt(restoredParent, restoredAttempt, 0, 4) &&
            !CoreGameEngineNativeDrawAttemptsRuntime.MatchesProgramDrawAttempt(restoredParent,
                restoredAttempt with { Context = restoredAttempt.Context with { RecipientSeat = 1 } }, 0, 4) &&
            !CoreGameEngineNativeDrawAttemptsRuntime.MatchesProgramDrawAttempt(restoredParent,
                restoredAttempt with { Context = restoredAttempt.Context with { RequestedCount = 3 } }, 0, 4),
            "Cold replay keeps the original Draw4 cursor and rejects an altered recipient or count.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.G.State.TurnNumber == 5 && s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var drawn = s.G.CardMovements.Where(move => move.Reason.Value == "skill-program.mou:jichou.draw").ToArray();
        s.Need(s.E<MouWangLingGiftEvent>().SequenceEqual(gifts) &&
            s.E<MouWangLingDrawReturnedEvent>().Single() == new MouWangLingDrawReturnedEvent(drawing.Id, 4, 4, 0, attempt.Id) &&
            drawn.Length == 4 && drawn.Select(move => move.CardId).Distinct().Count() == 4 &&
            drawn.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Sequence > attempt.Context.SequenceBefore) &&
            s.E<MouWangLingCompletedEvent>().Count(e => e.FrameId == drawing.Id && e.FirstNames == 4) == 1 &&
            !s.E<MouWangLingAwakeningAcceptedEvent>().Any(),
            "Four actual distinct gifts to four selected living roles pay native Draw4 once, survive Cold, and do not awaken early.");
        s.Native(s.Keep, 3);
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.FireAttackDiscard });
        s.Answer(c => c.Cards.Count == 0); s.Play(); s.End(); s.BeginJichou();
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Cards.SequenceEqual([s.Keep]));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "recipient" && c.Targets.SequenceEqual([0]));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "confirm");
        s.Reach(() => s.V().MaxHp == 5 && s.V().Skills?.Any(k => k.Id == "mou:zifu") == true);
        s.Need(s.E<MouWangLingGiftEvent>().Count(e => e.FirstName) == 5 && s.E<MouWangLingGiftEvent>().Select(e => e.Name).Distinct().Count() == 5 &&
            s.E<MouWangLingAwakeningAcceptedEvent>() is [{ OwnerSeat: 0, Names: 5 }] && s.V().Hp == 5 &&
            s.E<MouWangLingDrawReturnedEvent>().Count(e => e.Requested == 1 && e.Actual == 1) == 1,
            "The fifth genuine used name is gifted at the next Ending, draws only its new-name reward, and the formal >=5 awakening really adds max HP, recovers and grants Zifu.");

        // The granted skill's distinct-name retention is the new behavior here;
        // a legal starting grant isolates it from unrelated ordinary draw turns.
        var z = Scene.Start(true);
        z.Reach(() => z.P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "keep"));
        z.Need(z.E<MouWangLingDrawReturnedEvent>() is [{ Requested: 2, Actual: 2 }] && z.V().Hand.Count == 4,
            "Zifu really supplements the two normal drawn cards to max HP4 before retaining names.");
        var retained = z.P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "keep").Cards.Single();
        z.Answer(c => c.Cards.SequenceEqual([retained])); z.Play();
        z.Need(z.V().Hand is [{ Id: var remaining, Kind: CardKind.Dodge }] && remaining == retained &&
            z.E<MouWangLingKeptEvent>().Length == 1 && z.E<MouWangLingDiscardedEvent>().Length == 3 &&
            z.E<NativeLogicalDiscardInvocationRecordedEvent>().Any(e => e.Reason.Value == "skill-program.mou:zifu.discard" && e.ActualCount == 3 && e.HandCount == 3),
            "Zifu keeps the actually selected Dodge and discards its other three same-name entities through one native logical-discard invocation.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public int Keep;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void BeginJichou()
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate" && c.Parameters.GetValueOrDefault("skill-id") == "mou:jichou"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "card"));
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Native(int card, int? target = null)
        {
            var a = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == card && a.ConversionSource is null && a.Kind != LegalActionKind.Recast &&
                (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            Need(a is not null, "The physical used card has an actual canonical native action.");
            Accept(new PlayCardCommand(0, card, a!.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId));
        }
        public void Answer(Func<PromptChoice, bool> match)
        {
            var p = P!; var c = p.Choices.FirstOrDefault(match);
            Need(p.PlayerSeat == 0 && c is not null, "Only a genuinely published human choice can be answered.");
            Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision));
        }
        public void Accept(GameCommand command)
        {
            try { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Actual command rejected."); }
            catch (Exception ex) { throw new InvalidOperationException(ex.Message + Diagnostic(), ex); }
        }
        public void Reach(Func<bool> done, [CallerLineNumber] int line = 0)
        {
            for (var i = 0; i < 220; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "Unexpected terminal cutoff at " + line);
                if (P is not { PlayerSeat: 0 } p) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (p.Kind == DecisionKind.PlayCard) End();
                else if (p.Kind == DecisionKind.DiscardCards)
                    Accept(new DiscardCardsCommand(0, p.ValidCardIds.OrderBy(id => id == Keep ? 1 : 0).Take(p.RequiredCardCount).ToArray(), p.PromptId, G.Revision));
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected native human boundary at " + line + Diagnostic());
            }
            throw new InvalidOperationException("Bounded actual Wang Ling flow not reached at " + line + Diagnostic());
        }
        public void Cold()
        {
            var before = SnapshotJson.Serialize(G.CreateSnapshot(0));
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == SnapshotJson.Serialize(G.CreateSnapshot(0)), "The paid first gift and the remaining gift cursor survive Cold unchanged.");
        }
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + Diagnostic()); }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Prompt = P, Owner = V(), Actions = G.GetHumanLegalActions(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Facts = G.Events.Where(e => e.Payload.GetType().Name.StartsWith("MouWangLing", StringComparison.Ordinal) || e.Payload is NativeLogicalDiscardInvocationRecordedEvent or CardUseDeclaredEvent or TurnStartedEvent or TurnEndedEvent).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(bool zifu)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var own = zifu ? Array.Empty<ShanheStartingCard>() : new ShanheStartingCard[] {
                new(CardKind.Slash, Suit.Spade, 7), new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.Duel, Suit.Spade, 7),
                new(CardKind.IronChain, Suit.Club, 7), new(CardKind.FireAttack, Suit.Heart, 7) };
            var setup = new ShanheBattleSetup(7, 1, zifu ? "mouwl-retain" : "mouwl-gifts", Hero, 4, 0, 0, 0, 0, zifu ? ["mou:zifu"] : [Hold], own, [],
                [new("fixture:mouwl-enemy-a", Hp: 12) { StartingHand = [] }, new("fixture:mouwl-enemy-b", Hp: 12) { StartingHand = [] }],
                [new("fixture:mouwl-ally", Hp: 12) { StartingHand = [] }], new string('f', 64));
            var g = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 10 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mou-wang-ling", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            foreach (var kind in new[] { CardKind.Slash, CardKind.Dodge, CardKind.DrawTwo, CardKind.Duel, CardKind.IronChain, CardKind.FireAttack })
            { var c = CardCatalog.Get(kind); b.AddCard(new("fixture:mouwl-" + kind, c.DisplayName, c.CategoryName, c.Description, kind)); }
            var continueEffect = new { op = "chooseOption", target = "owner", resultBind = "gift-hold", options = new[] { new { id = "continue" } } };
            var definitions = new object[] {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "afterNormalDraw", subject = "owner", optional = false, effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                new { id = Hold, revision = 1,
                    states = new[] { new { id = "waiting-for-draw", initialValue = false, visibility = "private", resetScope = "game", reacquirePolicy = "preserveUntilGameEnd" } },
                    triggers = new object[] {
                        new { id = "first-gift", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                            movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.mou:jichou.give" },
                            optional = false, usageScope = "game", usageLimit = 1, effects = new object[] {
                                continueEffect, new { op = "setBooleanState", target = "owner", stateId = "waiting-for-draw", value = true } } },
                        new { id = "paid-draw-request", window = "nativeDrawStarting", subject = "owner", optional = false, isSkillInvocation = false,
                            usageScope = "game", usageLimit = 1, condition = new { kind = "booleanState", stateId = "waiting-for-draw", expectedValue = true },
                            effects = new[] { continueEffect } } } }
            };
            var labels = new Dictionary<string, object> {
                [Quiet] = new { name = "安静真实同行", description = "正常摸牌后跳过出牌阶段。" },
                [Hold] = new { name = "实际分赠子窗", description = "第一次实际获得集筹赠牌后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续实际分赠" } }
            };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            foreach (var id in new[] { "fixture:mouwl-ally", "fixture:mouwl-enemy-a", "fixture:mouwl-enemy-b" }) b.AddGeneral(new(id, "真实同行", "", "standard:none", "wei", 12, [Quiet]));
            b.AddDeck(new("fixture:mouwl-deck", "固定真实闪牌堆", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 70).Select(_ => new ContentDeckPhysicalCard("fixture:mouwl-" + CardKind.Dodge, Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "谋王凌真实主流程", 4, 4, new Dictionary<string, int>(), "fixture:mouwl-deck", GeneralPoolIds: [Hero, "fixture:mouwl-ally", "fixture:mouwl-enemy-a", "fixture:mouwl-enemy-b"], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
