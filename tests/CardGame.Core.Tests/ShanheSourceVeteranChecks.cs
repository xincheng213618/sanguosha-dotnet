using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceVeteranChecks
{
    private const string Zhanjue = "classic:zhanjue", Lianying = "classic:lianying";
    private const string Target = "fixture:source-veteran-target", Deck = "fixture:source-veteran-deck";

    public static void AllHandZhanjuePaysOnceReturnsLianyingAndCompletesOriginalDuel()
    {
        var setup = new ShanheBattleSetup(61, 1, "source-veteran-native",
            StandardShanheSourceVeteranPackage.MediumGeneralId, 4, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3), new(CardKind.Dodge, Suit.Diamond, 4)],
            [], [new(Target)], [], new string('a', 64));
        var production = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { EnemyRoster = [new("classic:guan-yu")] });
        var registry = production.WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId
            ? mode with { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Append(Target).Distinct(StringComparer.Ordinal).ToArray() } : mode);
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2
        }, registry);
        var scene = new Scene(game);
        scene.Submit(new StartGameCommand()); scene.Play();
        Need(scene.View(0).HandCount == 3 && scene.View(0).Hp == 4,
            "The actual medium soldier body starts with three specified hand entities and its source four HP.");

        for (var duel = 1; duel <= 2; duel++)
        {
            var paid = scene.View(0).Hand.Select(card => card.Id).Order().ToArray();
            scene.Submit(new UseProgramSkillCommand(0, Zhanjue, "all-hand-duel", paid, [], game.Revision, scene.P!.PromptId));
            scene.Answer(choice => choice.Targets.SequenceEqual([1]) && choice.Parameters.GetValueOrDefault("card-kind") == nameof(CardKind.Duel));
            var conversion = scene.Events<ProgramViewAsConvertedEvent>().Last(fact => fact.SkillId == Zhanjue);
            // Capture the issued use before advancing any native response. A
            // later movement prompt must not be used to rediscover an earlier
            // frame that may already have returned; its frozen identity is the
            // conversion's actual use id throughout this check.
            var originalUse = game.ResolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == conversion.FrameId) ??
                throw new InvalidOperationException("The accepted conversion must retain its issued Duel before native advancement: " + scene.Diagnostic(conversion.FrameId));
            scene.Until(() => scene.P?.SkillPrompt?.SkillId == Lianying);
            var payingUse = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == originalUse.Id);
            var paymentChild = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>()
                .Single(frame => frame.ResumePaidCardUseFrameId == originalUse.Id);
            Need(conversion.PhysicalCardIds.Order().SequenceEqual(paid) && conversion.OwnerSeat == 0 && conversion.OutputKind == CardKind.Duel &&
                originalUse.Action is { ProviderSeat: 0 } action && action.PhysicalCards.Select(card => card.CardId).Order().SequenceEqual(paid) &&
                payingUse.Action?.ActionId == action.ActionId && payingUse.RecoveryPaidContinuation is
                    { Kind: RecoveryPaidCardUseKind.Trick, ActionKind: LegalActionKind.Duel } &&
                payingUse.PaymentMovementReceipt is { } payment && payment.OwnerFrameId == originalUse.Id &&
                payment.ActionId == action.ActionId && payment.Batch.Id == paymentChild.Batch.Id &&
                paymentChild.Batch.ParentFrameId == originalUse.Id && paymentChild.ResumeProgramFrameId is null &&
                paymentChild.Batch.Movements.Select(move => move.CardId).Order().SequenceEqual(paid) &&
                scene.View(0).HandCount == 0 && scene.Events<DamageAppliedEvent>().Length == duel - 1 &&
                scene.Events<DuelResponseEvent>().Length == duel - 1 &&
                paid.All(id => game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing) == 1),
                "Each real all-hand Duel pays the entire original hand once and suspends its own use before response or damage for the soldier's real Lianying child: " + scene.Diagnostic(conversion.FrameId));
            scene.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            scene.Play();
            var responses = scene.Events<DuelResponseEvent>();
            var damages = scene.Events<DamageAppliedEvent>();
            Need(scene.View(0).HandCount == 2 && scene.View(1).Hp == 8 - duel &&
                responses.Length == duel && responses.Last() is { ResponderSeat: 1, UsedSlash: false } && responses.Last().ResolutionId == originalUse.Id &&
                damages.Length == duel && damages.Last() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } &&
                game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program.classic:lianying.Draw") == duel &&
                game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program.classic:zhanjue.completed-participant-draw") == duel &&
                game.CardMovements.Count(move => move.To == CardLocation.Hand(1) && move.Reason.Value == "skill-program.classic:zhanjue.completed-participant-draw") == duel &&
                paid.All(id => game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Processing &&
                    move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished) == 1) &&
                scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == originalUse.Id) == 1 &&
                !game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.Id == paymentChild.Id) &&
                !game.ResolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == originalUse.Id),
                "Lianying returns one actual card, the original Duel deals one real damage and finishes each paid entity once, then Zhanjue gives each actual participant its completion draw.");
            Need(game.GetHumanLegalActions().Any(action => action.ProgramSkillId == Zhanjue) == (duel == 1),
                "Lianying's separate draw does not exhaust Zhanjue: its own first completion draw leaves a second use legal and its own second disables the phase.");
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public PendingDecision? P => Enumerable.Range(0, 2).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot View(int seat) => game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public string Diagnostic(long useId) => JsonSerializer.Serialize(new
        {
            useId, prompt = P, hand = View(0).HandCount,
            stack = game.ResolutionStack.Select(frame => new { frame.Id, frame.Kind }),
            responses = Events<DuelResponseEvent>(), damages = Events<DamageAppliedEvent>(),
            finished = Events<CardUseFinishedEvent>(),
            movementWindows = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(frame => new
            {
                frame.Id, frame.Batch.ParentFrameId, frame.Batch.AwaitingProgramFrameId,
                frame.Batch.OriginSkillId, frame.Batch.OriginSkillInstanceId, frame.Batch.OriginOwnerSeat,
                frame.Batch.SourceCounts,
                frame.ResumeProgramFrameId, frame.ResumePaidCardUseFrameId,
                movements = frame.Batch.Movements.Select(move => new { move.CardId, move.CardKind, move.Sequence, move.From, move.To, move.Reason, move.Disclosure })
            })
        });
        public void Submit(GameCommand command)
        { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected source veteran command."); }
        public void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = P!;
            Need(prompt.PlayerSeat == 0, "Only the actual human player answers an external veteran prompt.");
            Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
        }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 100; step++)
            {
                if (done()) return;
                Need(P is null || !View(P.PlayerSeat).IsHuman, "Unexpected veteran boundary: " + JsonSerializer.Serialize(P));
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The bounded veteran native flow did not reach its required boundary.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-veteran", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Target, "真实决斗目标", "supporter", null, "wei", 8));
            builder.AddDeck(new(Deck, "无随机初手与通常摸牌", 0, 0, [new("standard:dodge", 40)]));
        }
    }
}
