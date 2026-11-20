using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static class AnniversaryCuiLiePindianTargetChecks
{
    private const string Hero = "anniversary:cui-lie", Chibi = "anniversary:chibi";
    private const string Mode = "identity:anniversary-chibi-target", Deck = "fixture:anniversary-chibi-target-deck";
    private const string DrawWatch = "fixture:anniversary-zijue-native-draw";
    private const string RecoveryWatch = "fixture:anniversary-zijue-recovery";
    private static readonly string[] Generals =
        ["fixture:anniversary-chibi-user", Hero, "fixture:anniversary-chibi-far", "fixture:anniversary-chibi-near"];

    internal static void FarNativeUseOpensOriginalPindianCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        PendingDecision? Prompt() => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Chibi fixture command was rejected.");
        }
        PendingDecision Reach(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 80; step++)
            {
                if (Prompt() is { } prompt)
                {
                    if (ready(prompt)) return prompt;
                    throw new InvalidOperationException($"Unexpected Chibi fixture prompt {prompt.Kind}/{prompt.SkillPrompt?.SkillId} for seat {prompt.PlayerSeat}.");
                }
                if (game.State.Winner != Winner.None) break;
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException($"Chibi fixture missed its expected choice in {game.State.Phase}.");
        }

        Submit(new StartGameCommand());
        var play = Reach(prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (play.Kind == DecisionKind.SelectGeneral)
        {
            Submit(new SelectGeneralCommand(play.PlayerSeat, Generals[play.PlayerSeat], game.Revision, play.PromptId));
            play = Reach(prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Need(play.PlayerSeat == 0 && game.CreateSnapshot(0).Players[2].IsAlive &&
            game.GetCombatDistance(0, 2) > 1 && game.GetCombatDistance(0, 3) == 1,
            "The first native user has a living far target and an adjacent target.");
        var action = game.GetLegalActions(0).First(candidate => candidate.Kind == LegalActionKind.IronChain &&
            candidate.ConversionSource is null && candidate.TargetSeats.SequenceEqual([2, 3]));
        var ironChain = action.CardId!.Value;
        Submit(new PlayCardCommand(0, ironChain, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind));
        var offer = Reach(prompt => prompt.PlayerSeat == 1 && prompt.SkillPrompt?.SkillId == Chibi);
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == ironChain);
        var window = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(frame => frame.ParentFrameId == use.Id);
        Need(use.Action is { Type: CardActionType.Use, ActorSeat: 0 } original &&
            original.EffectiveDesignatedTargetSeats.SequenceEqual([2, 3]) && window.Action.ActionId == original.ActionId &&
            game.CardMovements.Count(move => move.CardId == ironChain && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing) == 1 && game.CreateSnapshot(0).Players[0].HandCount > 0,
            "The physical Iron Chain stays on its original finalized-target window while the user retains a Pindian input.");
        var activate = offer.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Submit(new AnswerPromptCommand(1, offer.PromptId, activate.Id, game.Revision));

        var issued = game.Events.Select(entry => entry.Payload).OfType<AnniversaryCuiLiePindianIssuedEvent>().Single();
        var chibi = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == issued.FrameId);
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var sourcePrompt = Reach(prompt => prompt is { Kind: DecisionKind.SkillModule, PlayerSeat: 1, IsPrivate: true });
        Need(issued.Actor == 0 && issued.ContestId == pair.Id &&
            chibi.AnniversaryCuiLie is { Stage: AnniversaryCuiLieStage.Pindian } receipt &&
            receipt.ParentId == window.Id && receipt.UseId == use.Id && receipt.ActionId == use.Action!.ActionId &&
            receipt.PindianId == pair.Id && pair.ParentFrameId == chibi.Id &&
            pair is { SourceSeat: 1, OpponentSeat: 0, PindianStep: PindianStep.ChooseSourceCard },
            "Accepting Chibi opens the native pair under the original Iron Chain observer frame.");
        var sourceCard = game.CreateSnapshot(1).Players[1].Hand.First().Id;
        var sourceChoice = sourcePrompt.Choices.Single(choice => choice.Cards.SequenceEqual([sourceCard]) &&
            choice.Parameters.GetValueOrDefault("action") == "pindian-source-card");
        Submit(new AnswerPromptCommand(1, sourcePrompt.PromptId, sourceChoice.Id, game.Revision));
        var committed = game.ResolutionStack.OfType<PindianFrame>().Single();
        var opponentPrompt = Reach(prompt => prompt is { Kind: DecisionKind.SkillModule, PlayerSeat: 0, IsPrivate: true });
        Need(committed.Id == pair.Id && committed.ParentFrameId == chibi.Id && committed.SourceCardId == sourceCard &&
            committed.PindianStep == PindianStep.ChooseOpponentCard && !opponentPrompt.ValidCardIds.Contains(ironChain),
            "The real source card stays on the original pair and the opponent chooses from the remaining hand.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(game.CreateSnapshot(seat))) && restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack.OfType<PindianFrame>().Single()) == JsonSerializer.Serialize(committed),
            "Cold replay retains the original pair, chosen source input, and private opponent prompt.");
    }

    internal static void PaidZijueSurvivesRecipientStartAndExpiresAtSourceStartCold()
    {
        const int source = 0, recipient = 1, amount = 2;
        const string skill = "anniversary:zijue", binding = "declare-number";
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(RecoveryWatch));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = source, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        PendingDecision? Prompt() => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Zijue fixture command was rejected.");
        }
        void Reach(Func<bool> ready, bool passPlay = false)
        {
            for (var step = 0; step < 100; step++)
            {
                if (ready()) return;
                switch (Prompt())
                {
                    case null when game.State.Winner == Winner.None:
                        Submit(new AdvanceOneStepCommand(game.Revision));
                        break;
                    case { Kind: DecisionKind.PlayCard } play when passPlay:
                        Submit(new EndPlayPhaseCommand(play.PlayerSeat, game.Revision, play.PromptId));
                        break;
                    default:
                        throw new InvalidOperationException("The Zijue fixture encountered an unexpected decision or game end.");
                }
            }
            throw new InvalidOperationException("The Zijue fixture missed its expected boundary.");
        }
        void Answer(int seat, Func<PromptChoice, bool> select)
        {
            Reach(() => Prompt() is { } prompt && prompt.PlayerSeat == seat && prompt.SkillPrompt?.SkillId == skill);
            var menu = Prompt()!;
            var choice = menu.Choices.Single(select);
            Submit(new AnswerPromptCommand(seat, menu.PromptId, choice.Id, game.Revision));
        }
        bool InRecipientHand(int card) => game.CreateSnapshot(recipient).Players[recipient].Hand.Any(item => item.Id == card);

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.SelectGeneral or DecisionKind.PlayCard });
        while (Prompt() is { Kind: DecisionKind.SelectGeneral } selection)
        {
            var general = selection.PlayerSeat switch
            {
                source => Hero, recipient => Generals[source], _ => Generals[selection.PlayerSeat]
            };
            Submit(new SelectGeneralCommand(selection.PlayerSeat, general, game.Revision, selection.PromptId));
            Reach(() => Prompt() is { Kind: DecisionKind.SelectGeneral or DecisionKind.PlayCard });
        }

        var preWound = game.CreateSnapshot(source).Players[source].Hp;
        Submit(new UseProgramSkillCommand(source, RecoveryWatch, "wound", [], [], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: source });
        var play = Prompt()!;
        var issueTurn = Facts<TurnStartedEvent>().Single(turn => turn.ActorSeat == source);
        var ordinaryDistance = game.GetCombatDistance(recipient, source);
        var sourceBefore = game.CreateSnapshot(source).Players[source];
        var payment = sourceBefore.Hand.Take(amount).Select(card => card.Id).ToArray();
        Need(play.PlayerSeat == source && sourceBefore.Hp == preWound - 1 && sourceBefore.Hp < sourceBefore.MaxHp &&
            Facts<ProgramSkillHpLostEvent>().Count(entry => entry.SkillId == RecoveryWatch && entry.TargetSeat == source && entry.Amount == 1) == 1 &&
            ordinaryDistance == 1 && payment.Length == amount,
            "A wounded Cui Lie reaches his first actual Play with two physical hand cards and an adjacent recipient.");
        Submit(new UseProgramSkillCommand(source, skill, binding, [], [recipient], game.Revision, play.PromptId));
        Answer(recipient, choice => choice.Parameters.GetValueOrDefault("value") == "2");
        foreach (var card in payment)
            Answer(source, choice => choice.Cards.SequenceEqual([card]));
        Reach(() => Prompt() is { PlayerSeat: source, SkillPrompt.SkillId: RecoveryWatch });

        var declaration = Facts<AnniversaryCuiLieNumberEvent>().Single();
        var gift = Facts<AnniversaryCuiLieGiftEvent>().Single();
        var stack = game.ResolutionStack.ToArray();
        var parent = stack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == gift.FrameId);
        var hp = stack.OfType<HpChangedTriggerWindowFrame>().Single();
        var hpIndex = Array.IndexOf(stack, hp);
        Need(parent.AnniversaryCuiLie is { Stage: AnniversaryCuiLieStage.RecoveryChildren } &&
            hpIndex > 0 && stack[hpIndex - 1].Id == parent.Id && hp.Change.ParentFrameId == parent.Id &&
            hp.ResumeFrameId == parent.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement &&
            hp.Change is { SourceSeat: source, TargetSeat: source, Kind: HpChangeKind.Recovery, Amount: 1 } &&
            hp.Change.HpBefore == sourceBefore.Hp && hp.Change.HpAfter == sourceBefore.Hp + 1 &&
            payment.All(InRecipientHand) && game.CreateSnapshot(source).Players[source].Hp == hp.Change.HpAfter &&
            Facts<AnniversaryCuiLieLeaseEvent>().Length == 0,
            "The real paid gift recovers one HP and pauses under its original program before issuing the distance benefit.");
        var watchId = Prompt()!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredHp = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.Id == hp.Id);
        var runtime = game.GetRuntimeModule<CharacterCuiLieAnniversaryRuntime>();
        Need(Prompt() is { PlayerSeat: source, SkillPrompt.SkillId: RecoveryWatch } watcher && watcher.PromptId == watchId &&
            runtime.CuiLieFirstChild(restoredParent, restoredHp) &&
            !runtime.CuiLieFirstChild(restoredParent, restoredHp with { ResumeFrameId = hp.Id }) &&
            !runtime.CuiLieFirstChild(restoredParent, restoredHp with { Change = restoredHp.Change with { ParentFrameId = hp.Id } }) &&
            !runtime.CuiLieFirstChild(restoredParent, restoredHp with { Continuation = PostEventContinuation.Program }),
            "Cold restore retains Zijue's recovery child and rejects a different resume frame, change parent, or continuation.");
        var resume = Prompt()!;
        var proceed = resume.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Submit(new AnswerPromptCommand(source, resume.PromptId, proceed.Id, game.Revision));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: source });

        var lease = Facts<AnniversaryCuiLieLeaseEvent>().Single();
        Need(declaration is { Recipient: recipient, Number: amount } && gift.Count == amount &&
            declaration.FrameId == gift.FrameId && gift.FrameId == lease.FrameId && lease.Turn == issueTurn.TurnNumber &&
            Facts<RecoveryAppliedEvent>().Single() == new RecoveryAppliedEvent(source, source, 1, sourceBefore.Hp + 1) &&
            Facts<AnniversaryCuiLieGiftEvent>().Single() == gift &&
            lease.Amount == amount && lease.Source is { OwnerSeat: source, SkillId: skill, BindingId: binding } &&
            payment.All(card => InRecipientHand(card) && game.CardMovements.Count(move =>
                move.CardId == card && move.From == CardLocation.Hand(source) && move.To == CardLocation.Hand(recipient) &&
                move.Reason.Value == "skill-program.anniversary:zijue.give") == 1) &&
            game.GetCombatDistance(recipient, source) == ordinaryDistance + amount,
            "The foreign declaration and two real transfers issue one distance benefit on Cui Lie's actual turn.");

        Reach(() => Facts<TurnStartedEvent>().Last().ActorSeat == recipient, passPlay: true);
        var foreignStart = Facts<TurnStartedEvent>().Last();
        Need(foreignStart.TurnNumber > lease.Turn && Facts<TurnStartedEvent>().Count(turn => turn.ActorSeat == source) == 1 &&
            payment.All(InRecipientHand) && game.GetCombatDistance(recipient, source) == ordinaryDistance + amount,
            "The recipient's later actual start leaves both paid cards and the issued distance benefit in place.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Facts<TurnStartedEvent>().Last() == foreignStart && Facts<AnniversaryCuiLieLeaseEvent>().Single() == lease &&
            Facts<AnniversaryCuiLieGiftEvent>().Single() == gift && payment.All(InRecipientHand) &&
            game.GetCombatDistance(recipient, source) == ordinaryDistance + amount,
            "Cold restore on the recipient's turn retains the physical payment and the original active benefit.");

        Reach(() => Facts<TurnStartedEvent>().Last().ActorSeat == source, passPlay: true);
        Need(Facts<TurnStartedEvent>().Count(turn => turn.ActorSeat == source) == 2 &&
            Facts<TurnStartedEvent>().Last().TurnNumber > foreignStart.TurnNumber &&
            game.GetCombatDistance(recipient, source) == ordinaryDistance &&
            Facts<AnniversaryCuiLieLeaseEvent>().Single() == lease && Facts<AnniversaryCuiLieGiftEvent>().Single() == gift,
            "Cui Lie's next actual turn start expires the original distance benefit.");
    }

    internal static void DeclinedGiftReturnsOriginalNativeDrawCold()
    {
        const int owner = 0, declarer = 1, count = 4;
        const string skill = "anniversary:zijue", reason = "skill-program.anniversary:zijue.draw";
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(DrawWatch));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = owner, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Zijue native draw fixture command was rejected.");
        }
        PendingDecision Reach(Func<PendingDecision, bool> expected)
        {
            for (var step = 0; step < 80; step++)
            {
                var prompt = Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision)
                    .FirstOrDefault(candidate => candidate is not null);
                if (prompt is not null)
                {
                    Need(expected(prompt), $"Unexpected Zijue native draw prompt {prompt.Kind}/{prompt.SkillPrompt?.SkillId} for seat {prompt.PlayerSeat}.");
                    return prompt;
                }
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The Zijue native draw fixture missed its expected prompt.");
        }
        void Answer(PendingDecision prompt, Func<PromptChoice, bool> choice) => Submit(new AnswerPromptCommand(
            prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(choice).Id, game.Revision));

        Submit(new StartGameCommand());
        var play = Reach(prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (play.Kind == DecisionKind.SelectGeneral)
        {
            var general = play.PlayerSeat switch
            {
                owner => Hero, declarer => Generals[owner], _ => Generals[play.PlayerSeat]
            };
            Submit(new SelectGeneralCommand(play.PlayerSeat, general, game.Revision, play.PromptId));
            play = Reach(prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Need(play.PlayerSeat == owner, "The real Cui Lie starts his own Play phase.");
        var handBefore = game.CreateSnapshot(owner).Players[owner].HandCount;
        Submit(new UseProgramSkillCommand(owner, skill, "declare-number", [], [declarer], game.Revision, play.PromptId));
        Answer(Reach(prompt => prompt is { PlayerSeat: declarer, SkillPrompt.SkillId: skill }),
            choice => choice.Parameters.GetValueOrDefault("value") == "4");
        Answer(Reach(prompt => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: skill }),
            choice => choice.Parameters.GetValueOrDefault("verb") == "draw");
        Reach(prompt => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: DrawWatch });

        var declaration = Facts<AnniversaryCuiLieNumberEvent>().Single();
        var stack = game.ResolutionStack.ToArray();
        var parent = stack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == declaration.FrameId);
        var attempt = stack.OfType<NativeDrawAttemptFrame>().Single();
        var attemptIndex = Array.IndexOf(stack, attempt);
        Need(declaration is { Recipient: declarer, Number: count } && attemptIndex > 0 && stack[attemptIndex - 1].Id == parent.Id &&
            parent.AnniversaryCuiLie is { Stage: AnniversaryCuiLieStage.Drawing, Number: count } &&
            attempt.Context is { RecipientSeat: owner, RequestedCount: count } && attempt.Context.Reason.Value == reason &&
            attempt.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation && parent.NativeDraw?.ActiveAttemptId == attempt.Id &&
            Facts<NativeDrawAttemptStartedEvent>().Count(entry => entry.Context == attempt.Context && entry.Return == attempt.Return) == 1 &&
            Facts<AnniversaryCuiLieDrawReturnedEvent>().Length == 0 && game.CardMovements.All(move => move.Reason.Value != reason) &&
            game.CreateSnapshot(owner).Players[owner].HandCount == handBefore,
            "Declining the gift attaches an unpaid native request for the declared four to the original Zijue frame.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredAttempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == attempt.Id);
        var runtime = game.GetRuntimeModule<CharacterCuiLieAnniversaryRuntime>();
        Need(restoredAttempt.Context == attempt.Context && runtime.CuiLieFirstChild(restoredParent, restoredAttempt) &&
            !runtime.CuiLieFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { ParentFrameId = attempt.Id } }) &&
            !runtime.CuiLieFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { RecipientSeat = declarer } }) &&
            !runtime.CuiLieFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { RequestedCount = count - 1 } }),
            "Cold restore retains Zijue's original request and rejects a different parent, recipient, or count.");
        Answer(Reach(prompt => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: DrawWatch }),
            choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: owner });

        var returned = Facts<AnniversaryCuiLieDrawReturnedEvent>().Single();
        var drawn = game.CardMovements.Where(move => move.Reason.Value == reason).ToArray();
        Need(returned is { Recipient: owner, Requested: count, Actual: count } &&
            returned.FrameId == parent.Id && returned.Before == attempt.Context.SequenceBefore &&
            drawn.Length == count && drawn.Select(move => move.CardId).Distinct().Count() == count &&
            drawn.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner) &&
                move.Sequence > returned.Before && move.Sequence <= returned.After) &&
            game.CreateSnapshot(owner).Players[owner].HandCount == handBefore + count &&
            Facts<ProgramSkillResolvedEvent>().Count(entry => entry.FrameId == parent.Id && entry.Completed) == 1 &&
            Facts<AnniversaryCuiLieGiftEvent>().Length == 0 && Facts<AnniversaryCuiLieLeaseEvent>().Length == 0,
            "The original Zijue resumes once, draws four physical cards for Cui Lie, and issues no gift or distance benefit.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture(string? watcher = null) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-chibi-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            if (watcher is not null)
            {
                var isRecovery = watcher == RecoveryWatch;
                var trigger = isRecovery ? "zijue-recovery" : "zijue-real-draw";
                var window = isRecovery ? "afterHealthChanged" : "nativeDrawStarting";
                var condition = isRecovery ? """{"kind":"healthChangeKindIs","hpChangeKind":"recovery"}""" : """{"kind":"phaseIsPlay"}""";
                var binding = isRecovery ? "recovery" : "draw";
                var title = isRecovery ? "观察赀爵恢复" : "观察赀爵摸牌";
                var description = isRecovery ? "暂停于原生体力变化。" : "暂停于原生摸牌请求。";
                var activations = isRecovery ? """
                    ,"activations":[{"id":"wound","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving",
                     "usesPerTurn":1,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]
                    """ : string.Empty;
                var catalog = SkillProgramCatalog.Load($$"""
                    {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{watcher}}","revision":1{{activations}},"triggers":[
                      {"id":"{{trigger}}","window":"{{window}}","subject":"owner","condition":{{condition}},
                       "optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"{{binding}}",
                        "options":[{"id":"continue"}]}]}]}]}
                    """, JsonSerializer.Serialize(new
                    {
                        schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                        skills = new Dictionary<string, object> { [watcher] = new { name = title, description,
                            optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } }
                    }));
                builder.AddSkill(new(watcher, catalog.Presentations[watcher].Name, catalog.Presentations[watcher].Description)
                    { Program = catalog.Programs[watcher], ProgramPresentation = catalog.Presentations[watcher] });
            }
            CharacterFixtureContent.Register(watcher is null ? builder : new WatchedOwner(builder, watcher), Hero);
            foreach (var general in Generals.Where(id => id != Hero))
                builder.AddGeneral(new(general, "固定同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "斥避固定牌堆", 2, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 24)
                    .Select(_ => new ContentDeckPhysicalCard("standard:iron_chain", Suit.Club, 8)).ToArray()
            });
            builder.AddMode(new(Mode, "斥避原生拼点", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class WatchedOwner(IContentRegistryBuilder builder, string watcher) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => builder.AddGeneral(general.Id == Hero
            ? general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], watcher] }
            : general);
        public void AddSkill(ContentSkillDefinition skill) => builder.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => builder.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => builder.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => builder.AddMode(mode);
    }
}
