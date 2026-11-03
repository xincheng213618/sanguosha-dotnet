using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static partial class SkillProgramTargetOrderChecks
{
    public static void Run()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(),
            new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(),
            new StandardClassicGeneralPackage(), new Fixture());
        // Seed 7 deals exactly the weapon and a Dodge to the lord in this fixed deck.
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, HumanSeat = 0, HumanRole = Role.Lord, PlayerCount = 5,
            ModeId = "identity:classic-target-order-5", UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        Require(game.Submit(new StartGameCommand()).Accepted, "Fixture did not start.");
        var hand = game.CreateSnapshot(0, true).Players[0].Hand;
        Require(hand.Count == 2 && hand.Any(card => card.Kind == CardKind.Dodge), "Fixed fixture hand changed.");
        var weapon = hand.Single(card => card.Kind == CardKind.FangtianHalberd);
        Require(game.Submit(new PlayCardCommand(0, weapon.Id, [], game.Revision,
            game.PendingDecision!.PromptId)).Accepted, "Fixture failed to equip.");
        if (game.PendingDecision?.Kind != DecisionKind.PlayCard)
            Require(game.Submit(new AdvanceCommand(game.Revision)).Accepted, "Fixture did not resume play.");
        var selected = game.GetHumanLegalActions().Single(action =>
            action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([1, 3]));
        Require(game.Submit(new PlayCardCommand(0, selected.CardId!.Value, selected.TargetSeats,
            game.Revision, game.PendingDecision!.PromptId, selected.PlayedCardKind)
            { ConversionSource = selected.ConversionSource }).Accepted, "Converted multi-target Slash was rejected.");
        for (var step = 0; step < 10 && game.PendingDecision?.Kind != DecisionKind.ProgramTrigger; step++)
            Require(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "Target prepass did not advance.");
        Require(game.PendingDecision?.Kind == DecisionKind.ProgramTrigger, "Final targets did not open a program window.");
        var redirects = game.Events.Select(item => item.Payload).OfType<ProgramCardTriggerResolvedEvent>()
            .Where(item => item.SkillId == "target-order:liuli" && item.Activated).ToArray();
        var use = game.Events.Select(item => item.Payload).OfType<CardActionAcceptedEvent>()
            .Single(item => item.Action.Type == CardActionType.Use &&
                item.Action.ActorSeat == 0 && item.Action.EffectiveKind == CardKind.Slash &&
                item.Action.PhysicalCards.Any(card => card.CardId == selected.CardId)).Action;
        Require(redirects.Length == 2 && use.TargetSeats.SequenceEqual([2, 4]),
            $"Both Liuli choices must finish before triggers; redirected targets must stay distinct: redirects={redirects.Length}, targets={string.Join(',', use.TargetSeats)}.");
        Require(!game.Events.Any(item => item.Payload is CardRespondedEvent),
            "No target may respond before all conversion-use triggers finish.");
        var paused = game.CreateCheckpoint();
        game = GameReplay.Restore(paused, registry);
        var seenTargets = new List<int>();
        while (game.PendingDecision is { Kind: DecisionKind.ProgramTrigger } prompt)
        {
            seenTargets.Add(prompt.TargetSeat!.Value);
            var skip = prompt.Choices.Single(choice => choice.Parameters["program-action"] == "skip");
            Require(game.Submit(new AnswerPromptCommand(0, prompt.PromptId, skip.Id, game.Revision)).Accepted,
                "Target trigger could not be skipped.");
        }
        Require(seenTargets.SequenceEqual([2, 4]), "Triggers must bind only to the final opponents in target order.");
        for (var step = 0; step < 50 && game.ResolutionStack.Count > 0; step++)
            Require(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "Prepared attack did not finish.");
        Require(game.ResolutionStack.Count == 0 &&
                game.Events.Select(item => item.Payload).OfType<ProgramCardTriggerResolvedEvent>()
                    .Count(item => item.SkillId == "target-order:liuli" && item.Activated) == 2,
            $"Prepared targets must not run Liuli again during effect resolution: stack={string.Join(',', game.ResolutionStack.Select(frame => frame.GetType().Name))}, redirects={game.Events.Select(item => item.Payload).OfType<ProgramCardTriggerResolvedEvent>().Count(item => item.SkillId == "target-order:liuli" && item.Activated)}, generals={string.Join(',', game.CreateSnapshot(0, true).Players.Select(item => item.Seat + ":" + item.GeneralId))}.");
        var replay = GameReplay.Restore(game.CreateCheckpoint(), registry);
        Require(SnapshotJson.Serialize(game.State) == SnapshotJson.Serialize(replay.State),
            "Multi-target trigger and Liuli continuation did not replay exactly.");
        NativeRedirectChangesOnlyItsCurrentExecutionSlot(game);
        NativeUseResultsKeepOriginalParentSelection(game);
        RedirectTargetsAndPaymentsSharePostCostRange();
    }

    private static void NativeUseResultsKeepOriginalParentSelection(GameEngine game)
    {
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var capture = capabilities.Get<IProgramCardUseResultCapture>();
        var worlds = capabilities.Get<IKernelCreateProgramExpressionWorldOperations>();
        var program = capabilities.Get<IProgramContentQueries>().Skill("target-order:results").Program!;
        var selection = program.Activations.Single().Effects[2];
        var participants = capabilities.Get<IMatchParticipantStateAccess>();
        var selector = game.GetRuntimeModule<ProgramTargetSelectionRuntime>();
        var parent = new ProgramSkillFrame(frames.NextId(), 0, program.Id, "result-test", program.GameplayHash, 2,
            [], Array.AsReadOnly(new[] { 1 }));
        ProgramSkillFrame Parent() => (ProgramSkillFrame)frames.Find(parent.Id)!;
        using var document = JsonDocument.Parse("""["useResultTargets","native-result"]""");
        var expression = ProgramExpressions.ParseParticipantSet(document.RootElement, "native-result");
        IReadOnlyList<int>? ResultTargets() => ProgramExpressions.EvaluateParticipantSet(expression, worlds.CreateProgramExpressionWorld(Parent()));
        var priorTop = frames.Top;
        frames.Push(parent);
        try
        {
            Require(expression.Type == ProgramExpressionType.ParticipantSet && ResultTargets() is null,
                "An undelivered native result remains unknown instead of becoming a target or an invented empty result.");
            foreach (var (targets, dead) in new (int[], bool)[] { ([2, 4], false), ([2, 2, 3], false), ([2, 2], false), ([4], true), ([], false) })
            {
                frames.Replace(parent.Id, Parent() with { InstructionIndex = 2, SelectedTargetSeats = [1] });
                var issued = new[] { 1 };
                var sink = new ProgramCardUseResultSink(parent.Id, 2, "native-result", 1, 3, issued);
                issued[0] = 4;
                var child = new CardUseFrame(frames.NextId(), 2, 0, CardKind.FireSlash, targets,
                    Step: ResolutionFrameStep.Completed, PhysicalCardIds: [])
                {
                    ProgramResultSink = sink,
                    Action = targets.Length == 0 ? null : new CardActionContext(frames.NextId(), null,
                        CardActionType.Use, 2, 3, null, null, null, CardKind.FireSlash, targets, [], [])
                };
                frames.Push(child);
                try
                {
                    capture.Capture(child);
                    var result = Parent().CardUseResults.Single(r => r.Bind == "native-result");
                    Require(Parent().SelectedTargetSeats.SequenceEqual([1]) && result.NativeUseId == child.Id &&
                            result.InstructionIndex == 2 && result.ActionId == child.Action?.ActionId &&
                            result.DeclarerSeat == 1 && result.ActorSeat == 2 && result.ProviderSeat == 3 &&
                            result.Kind == CardKind.FireSlash && result.IssuedTargetSeats.SequenceEqual([1]) &&
                            result.TargetSeats.SequenceEqual(targets) && ResultTargets()!.SequenceEqual(targets.Distinct()),
                        "One original parent/cursor receives the explicit actual native result, preserving declarer, final actor, provider and kind separately from its unchanged input; raw slots retain duplicates while the expression reads a participant set.");
                    frames.Replace(parent.Id, Parent() with { InstructionIndex = 3 });
                    capture.Capture(child with { TargetSeats = [4] });
                    Require(ReferenceEquals(Parent().CardUseResults.Single(), result),
                        "Repeated completion cannot overwrite an already delivered result after the parent cursor advances.");
                }
                finally { frames.Pop(child.Id, child.Kind); }
                var delivered = Parent().CardUseResults.Single();
                capture.Capture(child);
                Require(ReferenceEquals(Parent().CardUseResults.Single(), delivered),
                    "A repeated native completion after its child has popped remains an exact no-op.");
                var player = participants.At(4); var alive = player.IsAlive;
                try
                {
                    // A shared read/selection contract, restored before leaving
                    // the controlled-frame check; this is not command recovery.
                    if (dead) player.IsAlive = false;
                    var eligible = ProgramExpressions.EvaluateBool(selection.Condition.Expression!, worlds.CreateProgramExpressionWorld(Parent()));
                    var uniqueLiving = targets.Distinct().Count() == 1 && !dead;
                    Require(eligible == uniqueLiving,
                        "The explicit result guard distinguishes one living participant from empty, multiple and dead outcomes, while duplicate native slots count as one role.");
                    if (eligible == true)
                        Require(selector.SelectProgramTargets(parent.Id, 0, selection.TargetKind!.Value, 1, 1, null,
                                SkillProgramTargetAiOrder.Stable) == SkillProgramStepOutcome.Continue &&
                                Parent().SelectedTargetSeats.SequenceEqual(targets.Distinct()),
                            "An explicit JSON singleton replacement consumes its actual result automatically without opening another target choice.");
                    else Require(Parent().SelectedTargetSeats.SequenceEqual([1]),
                        "A skipped empty, multiple or dead result cannot overwrite the original input selection or reuse it for the suffix.");
                }
                finally { player.IsAlive = alive; }
            }
            Require(Parent().CardUseResults.Single().ActionId is null && ResultTargets() is { Count: 0 },
                "A genuine Action-null completion delivers a known empty target result without manufacturing an Action identity.");
            frames.Replace(parent.Id, Parent() with { InstructionIndex = 2 });
            foreach (var mismatch in new[] { "cursor", "parent", "unfinished" })
            {
                var unrelated = parent with { Id = frames.NextId() };
                if (mismatch == "parent") frames.Push(unrelated);
                var child = new CardUseFrame(frames.NextId(), 1, 0, CardKind.Slash, [2],
                    Step: mismatch == "unfinished" ? ResolutionFrameStep.Declared : ResolutionFrameStep.Completed)
                { ProgramResultSink = new(parent.Id, mismatch == "cursor" ? 3 : 2, "native-result", 1, 3, [1]) };
                frames.Push(child);
                try
                {
                    var before = Parent(); var rejected = false;
                    try { capture.Capture(child); } catch (InvalidOperationException) { rejected = true; }
                    Require(rejected && ReferenceEquals(Parent(), before),
                        "Native result delivery rejects an unrelated direct parent, wrong instruction cursor or unfinished use before changing original bindings.");
                }
                finally
                {
                    frames.Pop(child.Id, child.Kind);
                    if (mismatch == "parent") frames.Pop(unrelated.Id, unrelated.Kind);
                }
            }
        }
        finally { frames.Pop(parent.Id, parent.Kind); }
        Require(ReferenceEquals(frames.Top, priorTop), "The native result boundary check restores its original owning stack.");
    }

    private static void NativeRedirectChangesOnlyItsCurrentExecutionSlot(GameEngine game)
    {
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var native = game.GetRuntimeModule<CoreNativeCardUseFramesRuntime>();
        var debits = game.GetRuntimeModule<CoreGameEngineCardUseDebitsRuntime>();
        var priorTop = frames.Top;
        var action = new CardActionContext(90001, null, CardActionType.Use, 0, 0, null, null, null,
            CardKind.Slash, [1, 3], [], [], designatedTargetSeats: [1, 3]);
        // Different original slots can now hold the same actual seat. No private
        // character ledger is needed to identify which slot the native cursor owns.
        var owner = new CardUseFrame(frames.NextId(), 0, 0, CardKind.Slash, Array.AsReadOnly(new[] { 2, 2 }),
            TargetIndex: 1) { Action = action };
        frames.Push(owner);
        try
        {
            native.RedirectCardUseTarget(owner.Id, 1, 2, 4);
            var changed = (CardUseFrame)frames.Find(owner.Id)!;
            Require(changed.TargetSeats.SequenceEqual([2, 4]) && changed.TargetIndex == 1 &&
                    owner.TargetSeats.SequenceEqual([2, 2]) && ReferenceEquals(changed.Action, action) &&
                    action.TargetSeats.SequenceEqual([1, 3]) && action.EffectiveDesignatedTargetSeats.SequenceEqual([1, 3]),
                "A repeated actual seat redirects only the current native slot; it must not rewrite the primary slot, prior frame or immutable designated action.");
            var completion = game.GetRuntimeModule<CoreGameEngineCompletedCategoryUseProofRuntime>();
            CardActionContext Completed(long id = 90001, int actor = 0, int provider = 0,
                IReadOnlyList<int>? targets = null, IReadOnlyList<CardActionCost>? materials = null) =>
                new(id, null, CardActionType.Use, actor, provider, null, null, null, CardKind.Slash,
                    targets ?? [1, 3], materials ?? [], [], designatedTargetSeats: [1, 3]);
            Require(completion.HasExactAcceptedActualHandGainUse(changed, action) &&
                    !completion.HasExactAcceptedActualHandGainUse(changed, Completed(id: 90002)) &&
                    !completion.HasExactAcceptedActualHandGainUse(changed, Completed(actor: 1)) &&
                    !completion.HasExactAcceptedActualHandGainUse(changed, Completed(provider: 1)) &&
                    !completion.HasExactAcceptedActualHandGainUse(changed, Completed(targets: [2, 4])) &&
                    !completion.HasExactAcceptedActualHandGainUse(changed, Completed(materials: [new(90001, CardKind.Slash, CardLocation.Hand(0))])),
                "Completion consumes the exact current native Action independently of redirected execution slots; a different Action, actor, provider, ordered targets or material cannot substitute for it.");
            foreach (var (index, previous) in new[] { (0, 2), (1, 2), (-1, 4), (2, 4) })
            {
                var rejected = false;
                try { native.RedirectCardUseTarget(owner.Id, index, previous, 3); }
                catch (InvalidOperationException) { rejected = true; }
                Require(rejected && ReferenceEquals(frames.Find(owner.Id), changed),
                    "The shared native boundary rejects an unrelated cursor, stale old seat and out-of-range slot before changing any execution target.");
            }
            Require(debits.RecordSlashUseDebit(changed.Id, action.ActorSeat),
                "The shared debit boundary reports a real ordinary Slash payment.");
            var debit = debits.GetCardUseDebit(action.ActionId) ??
                throw new InvalidOperationException("A successful native debit must retain its original Action identity.");
            Require(debit.CardActionId == action.ActionId && debit.ActorSeat == action.ActorSeat &&
                    debit.Query == SkillRuleQuery.SlashLimit && debits.IsCardUseDebitActive(action.ActionId),
                "The actual debit receipt belongs to its original actor and Action, independently of target allowance queries.");
            Require(debits.TryRefundCardUseDebit(debit) && !debits.TryRefundCardUseDebit(debit) &&
                    debits.GetCardUseDebit(action.ActionId) == debit && !debits.IsCardUseDebitActive(action.ActionId),
                "Refund retires the active payment once while preserving the fact that this Action was debited.");
            var doubleDebitRejected = false;
            try { _ = debits.RecordSlashUseDebit(changed.Id, action.ActorSeat); }
            catch (InvalidOperationException) { doubleDebitRejected = true; }
            var journal = game.GetRuntimeModule<CoreGameEngineProgramEventHistoryRuntime>();
            var recordedCount = journal.ProgramEventHistory<CardUseDebitRecordedEvent>().Count(e => e.Debit == debit);
            var refundedCount = journal.ProgramEventHistory<CardUseDebitRefundedEvent>().Count(e => e.Debit == debit);
            Require(doubleDebitRejected && !debits.IsCardUseDebitActive(action.ActionId) &&
                    recordedCount == 1 && refundedCount == 1,
                $"A refunded Action cannot debit again; the original shared journal retains precisely one payment and one refund fact, including uncommitted native calls: blocked={doubleDebitRejected}, recorded={recordedCount}, refunded={refundedCount}.");
        }
        finally
        {
            if (debits.GetCardUseDebit(action.ActionId) is { } debit && debits.IsCardUseDebitActive(action.ActionId))
                _ = debits.TryRefundCardUseDebit(debit);
            frames.Pop(owner.Id, owner.Kind);
        }
        Require(ReferenceEquals(frames.Top, priorTop), "The controlled native-slot check restores its original owning stack.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("target-order-fixture", new Version(1, 0, 0),
            [new PackageDependency("standard-classic-generals", new Version(1, 30, 0))]);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load(SkillProgramTestJson.CurrentSchema("""
                {"schemaVersion":__RULES_SCHEMA__,"skills":[
                  {"id":"target-order:results","revision":1,"activations":[{"id":"result-test","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"useVirtualSlash","target":"selectedTarget","resultBind":"native-result"},{"op":"selectTargets","target":"owner","targetKind":"anyLiving","minimumTargets":1,"maximumTargets":1,"targetAiOrder":"stable","allTargets":true,"replaceSelection":true,"targetSetExpr":["useResultTargets","native-result"],"condition":{"kind":"expression","expr":["and",["==",["count",["useResultTargets","native-result"],true],1],["==",["count",["useResultTargets","native-result"],["alive","it"]],1]]}},{"op":"draw","target":"selectedTarget","amount":1,"condition":{"kind":"expression","expr":["and",["==",["count",["useResultTargets","native-result"],true],1],["==",["count",["useResultTargets","native-result"],["alive","it"]],1]]}}]}]},
                  {"id":"target-order:swap","revision":1,"viewAs":[{"id":"slash","inputKinds":["dodge"],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false}]},
                  {"id":"target-order:draw","revision":1,"minimumRulesVersion": 171,"triggers":[{"id":"after-use","window":"cardUseTargetsFinalized","ownerRelation":"conversionSource","sourceSkillId":"target-order:swap","sourceViewAsId":"slash","optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"target-order:liuli","revision":1,"minimumRulesVersion":172,"triggers":[{"id":"redirect-slash","window":"slashTargetRedirecting","ownerRelation":"target","cardKinds":["slash","fireSlash","thunderSlash"],"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"slashRedirectable"},{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand","equipment"],"count":1,"destination":"discardPile","awaitMovementTriggers":true,"slashRedirectPostCostRange":true},{"op":"redirectCurrentAttack","target":"selectedTarget"}]}]}
                ]}
                """), """
                {"schemaVersion":3,"skills":{
                  "target-order:results":{"name":"原生结果","description":"共享目标结果"},
                  "target-order:swap":{"name":"转换","description":"测试转换"},
                  "target-order:draw":{"name":"摸牌","description":"测试触发"},
                  "target-order:liuli":{"name":"流离","description":"测试重定向"}
                }}
                """);
            foreach (var program in catalog.Programs.Values)
            {
                var text = catalog.Presentations[program.Id];
                builder.AddSkill(new ContentSkillDefinition(program.Id, text.Name, text.Description) { Program = program });
            }
            var generals = Enumerable.Range(0, 5).Select(index => $"target-order:general-{index}").ToArray();
            foreach (var (id, index) in generals.Select((id, index) => (id, index)))
                builder.AddGeneral(new ContentGeneralDefinition(id, "测试", "zhao_yun", "target-order:swap",
                    AdditionalSkillIds: index is 2 or 3
                        ? ["target-order:draw", "target-order:liuli"]
                        : ["target-order:draw"]));
            builder.AddDeck(new ContentDeckRecipe("target-order:deck", "测试", 2, 0,
                [new ContentDeckCardCount("classic:fangtian-halberd", 8),
                 new ContentDeckCardCount("standard:dodge", 24), new ContentDeckCardCount("standard:peach", 28)]));
            builder.AddMode(new ContentModeDefinition("identity:classic-target-order-5", "测试", 5, 5,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1,
                    [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                DeckId: "target-order:deck", GeneralCandidateCount: 1, GeneralPoolIds: generals));
        }
    }
    private static void Require(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }
}
