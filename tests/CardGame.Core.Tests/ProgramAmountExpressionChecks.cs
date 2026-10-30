using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ProgramAmountExpressionChecks
{
    private const string Skill = "fixture:amount-expression", ColdSkill = "fixture:amount-cold",
        Watch = "fixture:amount-watch", HealthSkill = "fixture:amount-health", HealthWatch = "fixture:amount-health-watch",
        NativeGate = "fixture:amount-native-request",
        Mode = "fixture:amount-mode";
    private const string DrawReason = "skill-program." + Skill + ".Draw";
    private const string ColdDrawReason = "skill-program." + ColdSkill + ".Draw";

    public static void LoaderHashAndTypedEvaluation()
    {
        var compact = Load("""{"op":"draw","target":"owner","amountExpr":["-",4,["handCount","owner"]]}""");
        var spaced = Load("""{"op":"draw","target":"owner","amountExpr":[ "-", 4, [ "handCount", "owner" ] ]}""");
        var changed = Load("""{"op":"draw","target":"owner","amountExpr":["-",5,["handCount","owner"]]}""");
        Need(compact.GameplayHash == spaced.GameplayHash && compact.GameplayHash != changed.GameplayHash &&
            compact.Activations.Single().Effects.Single().AmountExpression is { Type: ProgramExpressionType.Int, Op: "-" },
            "Typed amount expressions join the canonical gameplay hash, independently of JSON whitespace.");
        var loadedArgs = compact.Activations.Single().Effects.Single().AmountExpression!.Args;
        Need(loadedArgs is IList<ProgramExpression> { IsReadOnly: true }, "Loaded expression arguments expose a frozen collection.");
        try
        {
            ((IList<ProgramExpression>)loadedArgs)[0] = loadedArgs[1];
            throw new InvalidOperationException("A caller changed an expression after its gameplay fingerprint was computed.");
        }
        catch (NotSupportedException) { }
        var one = new ProgramExpression("int", ProgramExpressionType.Int, [], literal: 1);
        var two = new ProgramExpression("int", ProgramExpressionType.Int, [], literal: 2);
        var sourceList = new List<ProgramExpression> { one, two };
        var fromList = new ProgramExpression("+", ProgramExpressionType.Int, sourceList);
        sourceList.Clear();
        var sourceArray = new[] { one, two };
        var fromArray = new ProgramExpression("+", ProgramExpressionType.Int, sourceArray);
        sourceArray[0] = two;
        Need(fromList.Args.Count == 2 && ReferenceEquals(fromList.Args[0], one) &&
            ReferenceEquals(fromArray.Args[0], one),
            "Constructing an expression detaches its arguments from caller-owned lists and arrays.");
        foreach (var op in new[] { "draw", "recover", "loseHp" })
        {
            _ = Load($$"""{"op":"{{op}}","target":"owner","amountExpr":1}""");
            Reject($$"""{"op":"{{op}}","target":"owner","amount":1,"amountExpr":1}""");
            Reject($$"""{"op":"{{op}}","target":"owner","numberExpression":"ownerLostHp","amountExpr":1}""");
            Reject($$"""{"op":"{{op}}","target":"owner","amountExpr":true}""");
            Reject($$"""{"op":"{{op}}","target":"owner","amountExpr":"owner"}""");
        }
        Reject("""{"op":"damage","target":"owner","amountExpr":1}""");
        Reject("""{"op":"revealTopCards","target":"owner","amountExpr":1,"resultBind":"shown","visibility":"public"}""");
        Reject("""{"op":"draw","target":"owner","amountExpr":1,"resultBind":"drawn"}""");
        Reject("""{"op":"draw","target":"owner","amountExpr":["floorDiv",1,true]}""");
        Reject("""{"op":"draw","target":"owner","amountExpr":["handCount","it"]}""");
        var captured = Load("""
            {"op":"captureNumber","target":"owner","resultBind":"saved","valueExpr":["handCount","owner"]},
            {"op":"draw","target":"owner","amountExpr":["number","saved"]}
            """);
        Need(captured.Activations.Single().Effects[0] is
            { Op: SkillProgramEffectOp.CaptureNumber, ResultBind: "saved", ValueExpression.Type: ProgramExpressionType.Int },
            "A captured integer is declared by its producer before a later expression reads it.");
        _ = Load("""
            {"op":"revealTopCards","target":"owner","amount":1,"resultBind":"shown","visibility":"public"},
            {"op":"captureNumber","target":"owner","resultBind":"suits","valueExpr":["boundSuitCount","shown"]},
            {"op":"draw","target":"owner","amountExpr":["number","suits"]},
            {"op":"moveBoundCards","target":"owner","sourceBind":"shown","destination":"discardPile"}
            """);
        Reject("""{"op":"draw","target":"owner","amountExpr":["number","missing"]}""");
        Reject("""{"op":"draw","target":"owner","amountExpr":["boundCount","missing"]}""");
        Reject("""{"op":"draw","target":"owner","amountExpr":["boundSuitCount","missing"]}""");
        Reject("""{"op":"captureNumber","target":"owner","resultBind":"saved","valueExpr":true}""");
        Reject("""
            {"op":"captureNumber","target":"owner","resultBind":"saved","valueExpr":1},
            {"op":"captureNumber","target":"owner","resultBind":"saved","valueExpr":2}
            """);
        Reject("""
            {"op":"revealTopCards","target":"owner","amount":1,"resultBind":"saved","visibility":"public"},
            {"op":"captureNumber","target":"owner","resultBind":"saved","valueExpr":1}
            """);
        foreach (var window in new[] { "dyingEntered", "dyingExited" })
        {
            var transition = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skill}}","revision":1,"triggers":[
                 {"id":"transition","window":"{{window}}","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
                """;
            _ = SkillProgramCatalog.Load(transition, Presentation(Skill));
            RejectRules(transition.Replace("\"amount\":1", "\"amountExpr\":1"),
                "dying transition triggers require mandatory constant owner draws");
        }
        var shared = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skill}}","revision":1,"activations":[
             {"id":"shared","otherCharactersOnly":true,"minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving",
              "usesPerTurn":null,"usesPerPhase":1,"effects":[{"op":"draw","target":"activator","amount":1}]}]}]}
            """;
        _ = SkillProgramCatalog.Load(shared, Presentation(Skill));
        RejectRules(shared.Replace("\"amount\":1", "\"amountExpr\":1"),
            "shared activations require zero selected input, a phase quota and unconditional activator effects");
        var deep = "1";
        for (var level = 0; level <= ProgramExpressions.MaximumDepth; level++) deep = "[\"+\",1," + deep + "]";
        Reject("{\"op\":\"draw\",\"target\":\"owner\",\"amountExpr\":" + deep + "}");

        var owner = new PlayerSkillContext(0, 4, 4, 2, TurnPhase.Play, HandLimit: 7);
        var world = new SkillContextExpressionWorld(owner, null);
        Need(Int("""["-",["handLimit","owner"],["handCount","owner"]]""", world) == 5 &&
            Int("""["floorDiv",-3,2]""", world) == -2 &&
            Int("""["floorDiv",3,0]""", world) is null &&
            Int("""["handCount","eventTarget"]""", world) is null,
            "Hand limits share the public read world; floor division rounds down and absent facts or division by zero remain unknown.");
        var effects = new[]
        {
            Effect("""{"op":"draw","target":"owner","amount":1}"""),
            Effect("""{"op":"draw","target":"owner","amountExpr":["-",4,["handCount","owner"]]}""")
        };
        Need(ProgramCompositionAi.Estimate(effects, owner).Hint.OwnerDraw == 2 &&
            ProgramCompositionAi.Estimate([Effect("""{"op":"draw","target":"owner","amountExpr":["-",["handLimit","owner"],["handCount","owner"]]}""")], owner).Hint.OwnerDraw == 5,
            "AI reads the same quantity facts and updates its owner hand estimate between instructions.");
        var equipment = Effect("""{"op":"draw","target":"owner","amountExpr":["+",["floorDiv",["equipmentCount","owner"],2],1]}""");
        foreach (var count in new[] { 0, 1, 2, 3, 4, 9, 10 })
            Need(ProgramCompositionAi.Estimate([equipment], owner, publicContext: new(1, EquipmentCardCount: count)).Hint.OwnerDraw == count / 2 + 1,
                "AI retains the known public equipment count, including expanded equipment capacity.");
        Need(ProgramCompositionAi.Estimate([Effect("""{"op":"loseHp","target":"owner","amountExpr":["hp","eventTarget"]}""")], owner).Hint.OwnerHpLoss == 0,
            "Unknown quantity facts do not invent a health payment in AI estimates.");
    }

    public static void AiCancellationPreservesPaidPrefixAndSkipFallback()
    {
        var owner = new PlayerSkillContext(0, 4, 4, 2, TurnPhase.Play);
        const string unknown = """["hp","eventTarget"]""";
        ProgramAiEstimate Estimate(params string[] middle)
        {
            var prefix = """{"op":"loseHp","target":"owner","amount":1},{"op":"draw","target":"owner","amount":1},""";
            var tail = """,{"op":"draw","target":"owner","amount":2}""";
            return ProgramCompositionAi.Estimate(Load(prefix + string.Join(",", middle) + tail).Activations.Single().Effects, owner);
        }
        string AmountStep(string op, string expression, string policy) =>
            "{\"op\":\"" + op + "\",\"target\":\"owner\",\"amountExpr\":" + expression +
            (policy.Length == 0 ? "" : ",\"onUnknownAmount\":\"" + policy + "\"") +
            (op == "revealTopCards" ? ",\"resultBind\":\"shown\",\"visibility\":\"public\"" : "") + "}";
        var capture = Estimate("""{"op":"captureNumber","target":"owner","resultBind":"saved","valueExpr":["hp","eventTarget"]}""");
        Need(capture.Hint is { OwnerDraw: 1, OwnerHpLoss: 1 },
            "An unknown capture cancels the estimate path without erasing its already paid HP loss or completed draw.");
        foreach (var op in new[] { "draw", "recover", "loseHp", "revealTopCards" })
        {
            var cleanup = op == "revealTopCards"
                ? new[] { """{"op":"moveBoundCards","target":"owner","sourceBind":"shown","destination":"discardPile"}""" }
                : Array.Empty<string>();
            var cancelled = Estimate([AmountStep(op, unknown, "cancelBinding"), .. cleanup]);
            Need(cancelled.Hint is { OwnerDraw: 1, OwnerHpLoss: 1, OwnerRecovery: 0 },
                "Unknown cancelBinding " + op + " must exclude the later draw while retaining the executed prefix.");
            var zero = Estimate([AmountStep(op, "0", "cancelBinding"), .. cleanup]);
            Need(zero.Hint is { OwnerDraw: 3, OwnerHpLoss: 1, OwnerRecovery: 0 },
                "A known zero quantity is not a cancellation and must retain the later draw.");
            if (op == "revealTopCards") continue; // Mandatory result bindings cannot use skip-on-unknown.
            var skipped = Estimate(AmountStep(op, unknown, ""));
            Need(skipped.Hint is { OwnerDraw: 3, OwnerHpLoss: 1, OwnerRecovery: 0 },
                "Default skip only omits the unknown " + op + " and still estimates the following draw.");
        }
    }

    public static void LiveAmountsSkipUnknownAndApplyHealth()
    {
        var (game, registry) = Create();
        var initialHand = View(game, 0).HandCount;
        Need(initialHand < 6, "The bounded fixture starts below its target hand size.");
        Use(game, "live"); Play(game);
        Need(View(game, 0).HandCount == 6 && Moves(game, DrawReason) == 6 - initialHand,
            "The second draw reads the hand after the first draw, rather than evaluating both amounts at activation time.");
        var hp = View(game, 0).Hp;
        var movements = game.CardMovements.Count;
        Use(game, "unknown-and-negative"); Play(game);
        Need(View(game, 0).HandCount == 7 && View(game, 0).Hp == hp &&
            game.CardMovements.Count == movements + 1 &&
            Events<ProgramSkillResolvedEvent>(game).Last(e => e.SkillId == Skill) is { Completed: true },
            "Unknown and negative draw/recovery/HP-loss steps do nothing, while the following known step completes the same program.");
        var healthStart = game.Events.Count;
        Use(game, "health"); Play(game);
        Need(View(game, 0).Hp == hp - 1 &&
            game.Events.Skip(healthStart).Select(e => e.Payload).OfType<ProgramSkillHpLostEvent>().Single() is { TargetSeat: 0, Amount: 2 } &&
            game.Events.Skip(healthStart).Select(e => e.Payload).OfType<RecoveryAppliedEvent>().Single() is { TargetSeat: 0, Amount: 1 },
            "Expression quantities drive real two-HP loss and one-HP recovery through the native health mechanisms.");
        var selectedBefore = View(game, 1).HandCount;
        Use(game, "target", [1]); Play(game);
        Need(View(game, 1).HandCount == 4 && Moves(game, DrawReason, 1) == 4 - selectedBefore,
            "A selected-target expression uses its selected participant's current hand, rather than the owner.");
        var first = View(game, 1).HandCount; var second = View(game, 2).HandCount;
        Use(game, "targets", [1, 2]); Play(game);
        Need(View(game, 1).HandCount == first + 2 && View(game, 2).HandCount == second + 2,
            "A known integer quantity preserves the selected-target set and gives each recipient the native draw once.");
        Use(game, "limit"); Play(game);
        Need(View(game, 0).HandCount == View(game, 0).Hp + 6,
            "The live hand-limit read includes the preceding real turn modifier rather than using raw HP or maximum HP. " +
            JsonSerializer.Serialize(new { View(game, 0).HandCount, View(game, 0).Hp, View(game, 0).MaxHp,
                Amounts = Events<ProgramAmountEvaluatedEvent>(game).Where(e => e.SkillId == Skill), Modifiers = Events<TurnRuleModifierGrantedEvent>(game) }));
        _ = Cold(game, registry);
    }

    public static void PaidDrawChildrenResumeColdWithoutReevaluation()
    {
        var (game, registry) = Create(nativeDraw: true);
        var before = View(game, 0).HandCount;
        var requested = 6 - before;
        Use(game, "cold", skill: ColdSkill);
        Reach(game, () => game.PendingDecision?.SkillPrompt?.SkillId == Watch);
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == ColdSkill);
        var parentId = parent.Id;
        Need(parent.NativeDraw is { } paid && paid.RequestedCount == requested &&
            ProgramExpressionAmounts.IssuedAmount(parent) == requested && Moves(game, ColdDrawReason) == requested,
            "The amount is already frozen and every original draw entity has moved before the gained-card child asks its choice.");
        var restored = Cold(game, registry);
        foreach (var branch in new[] { game, restored })
        {
            FinishChildren(branch);
            var requests = Events<NativeDrawAttemptStartedEvent>(branch).Where(e => e.Context.Reason.Value == ColdDrawReason).ToArray();
            Need(requests is [{ Context.RequestedCount: var count }] && count == requested &&
                Moves(branch, ColdDrawReason) == requested && View(branch, 0).HandCount > before + requested &&
                Events<ProgramSkillResolvedEvent>(branch).Count(e => e.FrameId == parentId && e.Completed) == 1 &&
                !branch.ResolutionStack.Any(f => f.Id == parentId),
                "A child really changes the hand, but resuming the paid parent never recomputes the quantity, reissues its native request, or pays twice.");
        }
        Need(State(game) == State(restored), "Cold replay retains the exact paid draw, observer choices, native facts and all player views.");
    }

    public static void CapturedNumbersAndZeroCardBindingsRemainAvailableAndCold()
    {
        var (game, registry) = Create();
        var initial = View(game, 0).HandCount;
        Use(game, "capture-binding"); Play(game);
        var captured = Events<ProgramExpressionNumberCapturedEvent>(game).Single(e => e.SkillId == Skill && e.Name == "saved-hand");
        Need(captured.Value == initial && View(game, 0).HandCount == initial * 2 + 1 &&
            Events<ProgramAmountEvaluatedEvent>(game).Single(e => e.FrameId == captured.FrameId).Amount == initial,
            "Capturing the current hand before a real draw preserves the original number, including zero, for the later amount expression.");
        _ = Cold(game, registry);
        var hand = View(game, 0).HandCount; var movements = game.CardMovements.Count;
        Use(game, "zero-bindings"); Play(game);
        var completed = Events<ProgramSkillResolvedEvent>(game).Last(e => e.SkillId == Skill && e.ActivationId == "zero-bindings");
        var counts = Events<ProgramExpressionNumberCapturedEvent>(game).Where(e => e.FrameId == completed.FrameId).ToArray();
        Need(completed.Completed && View(game, 0).HandCount == hand + 1 && game.CardMovements.Count == movements + 1 &&
            counts.Select(e => e.Name).SequenceEqual(["empty-draw-count", "empty-reveal-count", "empty-suit-count"]) &&
            counts.All(e => e.Value == 0),
            "Zero draw and zero reveal both produce real empty bindings; bound counts and suits remain readable and the following sentinel draw runs once.");
        _ = Cold(game, registry);
    }

    public static void PaidHealthChildrenResumeColdWithoutReevaluation()
    {
        var (game, registry) = Create();
        var hp = View(game, 0).Hp;
        Use(game, "health-cold", skill: HealthSkill);
        Reach(game, () => game.PendingDecision?.SkillPrompt?.SkillId == HealthWatch);
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == HealthSkill);
        var parentId = parent.Id;
        Need(View(game, 0).HandCount == 2 && View(game, 0).Hp == hp - 2 &&
            ProgramExpressionAmounts.IssuedAmount(parent) == 2 &&
            game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any(),
            "Two actual lost HP are already paid when the health-change child pauses the expression instruction.");
        var restored = Cold(game, registry);
        foreach (var branch in new[] { game, restored })
        {
            AnswerChild(branch, HealthWatch);
            Reach(branch, () => branch.PendingDecision?.SkillPrompt?.SkillId == HealthWatch);
            Need(View(branch, 0).HandCount == 3 && View(branch, 0).Hp == hp &&
                ProgramExpressionAmounts.IssuedAmount(branch.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == HealthSkill)) == 3 &&
                Events<ProgramSkillHpLostEvent>(branch).Single(e => e.FrameId == parentId).Amount == 2 &&
                Events<RecoveryAppliedEvent>(branch).Single() is { TargetSeat: 0, Amount: 2 },
                "The first child changes the hand before recovery evaluates three; the native recovery clamps to the two missing HP without repeating the loss.");
            var afterRecovery = Cold(branch, registry);
            foreach (var finished in new[] { branch, afterRecovery })
            {
                FinishChildren(finished, HealthWatch);
                Need(View(finished, 0).HandCount == 5 && View(finished, 0).Hp == hp &&
                    Events<ProgramSkillHpLostEvent>(finished).Count(e => e.FrameId == parentId) == 1 &&
                    Events<RecoveryAppliedEvent>(finished).Length == 1 &&
                    Events<ProgramAmountEvaluatedEvent>(finished).Where(e => e.FrameId == parentId).Select(e => e.Amount).SequenceEqual([2, 3]) &&
                    Events<ProgramSkillResolvedEvent>(finished).Count(e => e.FrameId == parentId && e.Completed) == 1,
                    "Both paid health returns execute the following draw once, despite observer children changing the expression's hand-count operand.");
            }
            Need(State(branch) == State(afterRecovery), "Cold replay at the paid recovery child preserves its exact health continuation.");
        }
        Need(State(game) == State(restored), "Cold replay at the paid HP-loss child preserves both health payments and observer returns.");

        // A real Alcohol self-rescue offer keeps this no-Peach scene at the
        // human rescue boundary; an empty hand is automatically passed by Core.
        var (fatal, fatalRegistry) = Create(fatalRescue: true);
        var requested = View(fatal, 0).Hp + 1;
        Use(fatal, "fatal", skill: HealthSkill);
        Reach(fatal, () => fatal.PendingDecision is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
        var losing = fatal.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == HealthSkill);
        var fatalFrameId = losing.Id;
        Need(ProgramExpressionAmounts.IssuedAmount(losing) == requested && View(fatal, 0).Hp == -1 &&
            Events<ProgramSkillHpLostEvent>(fatal).Single(e => e.FrameId == fatalFrameId) is { Amount: var paid, RemainingHp: -1 } && paid == requested,
            "The fatal expression freezes HP plus one and pays it once before the real rescue decision, retaining negative HP.");
        var fatalRestored = Cold(fatal, fatalRegistry);
        foreach (var branch in new[] { fatal, fatalRestored })
        {
            var producer = branch.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == fatalFrameId);
            var dying = branch.ResolutionStack.OfType<DyingFrame>().Last();
            // Exercise the typed proof with the real paid frames and history; this
            // read-only bridge does not manufacture or replace any runtime state.
            var proof = typeof(GameEngine).GetMethod("TypedRecoveryProgramDyingMatches",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!;
            Need(proof.Invoke(branch, [producer, dying, Events<IGameEvent>(branch)]) is true,
                "The typed dying invoice must match the issued expression amount, including after cold replay.");
            for (var step = 0; branch.State.Winner == Winner.None; step++)
            {
                Need(step < 80 && !branch.IsFaulted, "The no-Peach dying fixture must finish its native responder cycle.");
                if (branch.PendingDecision is { } rescue)
                {
                    Need(rescue is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 }, "Only the human rescue pass is expected.");
                    var pass = rescue.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("response") == "let-die") ??
                        rescue.Choices.Single(c => c.Parameters.GetValueOrDefault("response") == "pass");
                    Accept(branch, new AnswerPromptCommand(0, rescue.PromptId, pass.Id, branch.Revision));
                }
                else Accept(branch, new AdvanceOneStepCommand(branch.Revision));
            }
            // Ordinary death normalizes stored HP to zero; its paid loss invoice
            // must still retain the negative value observed at RescueDying.
            Need(!View(branch, 0).IsAlive && View(branch, 0).Hp == 0 &&
                Events<ProgramSkillHpLostEvent>(branch).Where(e => e.FrameId == fatalFrameId).Select(e => e.Amount).SequenceEqual([requested]) &&
                Events<ProgramSkillHpLostEvent>(branch).Single(e => e.FrameId == fatalFrameId).RemainingHp == -1 &&
                Events<ProgramAmountEvaluatedEvent>(branch).Where(e => e.FrameId == fatalFrameId).Select(e => e.Amount).SequenceEqual([requested]) &&
                Events<PlayerDiedEvent>(branch).Count(e => e.VictimSeat == 0) == 1,
                "Passing the real rescue cycle reaches death and game over without recomputing the now-negative HP operand or paying again. " +
                JsonSerializer.Serialize(new { View(branch, 0).IsAlive, View(branch, 0).Hp, branch.State.Winner,
                    Lost = Events<ProgramSkillHpLostEvent>(branch).Where(e => e.FrameId == fatalFrameId),
                    Amounts = Events<ProgramAmountEvaluatedEvent>(branch).Where(e => e.FrameId == fatalFrameId), Deaths = Events<PlayerDiedEvent>(branch) }));
        }
        Need(State(fatal) == State(fatalRestored), "Cold replay at the fatal expression keeps the exact death, terminal views and committed event history.");
    }

    private static SkillProgram Load(string effect) => SkillProgramCatalog.Load($$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skill}}","revision":1,"activations":[
          {"id":"test","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{{effect}}]}]}]}
        """, Presentation(Skill)).Programs[Skill];
    private static SkillProgramEffect Effect(string effect) => Load(effect).Activations.Single().Effects.Single();
    private static void Reject(string effect)
    {
        try { _ = Load(effect); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("Invalid amount expression was accepted: " + effect);
    }
    private static void RejectRules(string rules, string contract)
    {
        try { _ = SkillProgramCatalog.Load(rules, Presentation(Skill)); }
        catch (InvalidOperationException error) when (error.Message.Contains(contract, StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException("A dynamic amount bypassed its static contract: " + contract);
    }
    private static int? Int(string json, IProgramExpressionWorld world)
    {
        using var document = JsonDocument.Parse(json);
        return ProgramExpressions.EvaluateInt(ProgramExpressions.ParseInt(document.RootElement, "amountExpr"), world);
    }
    private static string Presentation(params string[] ids) => JsonSerializer.Serialize(new
    {
        schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
        skills = ids.ToDictionary(id => id, id =>
        {
            var entry = new Dictionary<string, object> { ["name"] = "数量表达式", ["description"] = "当前公开数量及原生子返回" };
            if (id is Watch or HealthWatch) entry["optionLabels"] = new { resume = "继续" };
            return entry;
        })
    });
    private static PlayerSnapshot View(GameEngine game, int seat) => game.CreateSnapshot(seat).Players[seat];
    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static int Moves(GameEngine game, string reason, int seat = 0) => game.CardMovements.Count(m => m.Reason.Value == reason && m.To == CardLocation.Hand(seat));
    private static void Use(GameEngine game, string activation, int[]? targets = null, string skill = Skill) =>
        Accept(game, new UseProgramSkillCommand(0, skill, activation, [], targets ?? [], game.Revision, game.PendingDecision!.PromptId));
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Amount-expression command rejected."); }
    private static void Reach(GameEngine game, Func<bool> done)
    {
        for (var step = 0; step < 160; step++)
        {
            if (done()) return;
            Need(game.PendingDecision is null, "Unexpected prompt in amount-expression fixture: " + game.PendingDecision?.Kind);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The amount-expression fixture exceeded its bounded step budget.");
    }
    private static void Play(GameEngine game) => Reach(game, () => game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void AnswerChild(GameEngine game, string skill)
    {
        var prompt = game.PendingDecision ?? throw new InvalidOperationException("Missing paid child prompt.");
        Need(prompt.SkillPrompt?.SkillId == skill, "Only the owned child may ask its continuation.");
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single().Id, game.Revision));
    }
    private static void FinishChildren(GameEngine game, string skill = Watch)
    {
        for (var step = 0; step < 160; step++)
        {
            if (game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            if (game.PendingDecision is { } prompt)
            {
                AnswerChild(game, skill);
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The paid draw did not return to Play.");
    }
    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    { game.Revision, Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray(), Events = game.Events });
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(State(game) == State(restored), "Amount-expression checkpoint replay must preserve views and committed facts.");
        return restored;
    }
    private static (GameEngine, ContentRegistry) Create(bool nativeDraw = false, bool fatalRescue = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(nativeDraw, fatalRescue));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
        Accept(game, new StartGameCommand()); Play(game); return (game, registry);
    }
    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }

    private sealed class Fixture(bool nativeDraw, bool fatalRescue) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-amount-expression", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                 {"id":"{{Skill}}","revision":1,"activations":[
                  {"id":"live","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"draw","target":"owner","amount":1},{"op":"draw","target":"owner","amountExpr":["-",6,["handCount","owner"]]}]},
                  {"id":"unknown-and-negative","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"draw","target":"owner","amountExpr":["hp","eventTarget"]},{"op":"recover","target":"owner","amountExpr":["hp","eventTarget"]},
                    {"op":"loseHp","target":"owner","amountExpr":["hp","eventTarget"]},{"op":"draw","target":"owner","amountExpr":-3},
                    {"op":"recover","target":"owner","amountExpr":-3},{"op":"loseHp","target":"owner","amountExpr":-3},{"op":"draw","target":"owner","amount":1}]},
                  {"id":"health","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"loseHp","target":"owner","amountExpr":["+",1,1]},{"op":"recover","target":"owner","amountExpr":["+",0,1]}]},
                  {"id":"target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                    {"op":"draw","target":"selectedTarget","amountExpr":["-",4,["handCount","selectedTarget"]]}]},
                  {"id":"targets","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"draw","target":"selectedTargets","amountExpr":["+",1,1]}]},
                  {"id":"limit","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"grantTurnRuleModifier","target":"owner","ruleQuery":"handLimit","ruleOperation":"add","amount":6},
                    {"op":"draw","target":"owner","amountExpr":["-",["handLimit","owner"],["handCount","owner"]]}]},
                  {"id":"capture-binding","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"captureNumber","target":"owner","resultBind":"saved-hand","valueExpr":["handCount","owner"]},
                    {"op":"draw","target":"owner","amount":1},{"op":"draw","target":"owner","amountExpr":["number","saved-hand"]}]},
                  {"id":"zero-bindings","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"draw","target":"owner","amountExpr":0,"resultBind":"empty-draw","onUnknownAmount":"cancelBinding"},
                    {"op":"revealTopCards","target":"owner","amountExpr":0,"resultBind":"empty-reveal","visibility":"public","onUnknownAmount":"cancelBinding"},
                    {"op":"captureNumber","target":"owner","resultBind":"empty-draw-count","valueExpr":["boundCount","empty-draw"]},
                    {"op":"captureNumber","target":"owner","resultBind":"empty-reveal-count","valueExpr":["boundCount","empty-reveal"]},
                    {"op":"captureNumber","target":"owner","resultBind":"empty-suit-count","valueExpr":["boundSuitCount","empty-reveal"]},
                    {"op":"moveBoundCards","target":"owner","sourceBind":"empty-reveal","destination":"discardPile"},
                    {"op":"draw","target":"owner","amountExpr":["+",["number","empty-draw-count"],["number","empty-reveal-count"]]},
                    {"op":"draw","target":"owner","amount":1}]}]},
                 {"id":"{{ColdSkill}}","revision":1,"activations":[
                  {"id":"cold","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"draw","target":"owner","amountExpr":["-",6,["handCount","owner"]]}]}]},
                 {"id":"{{HealthSkill}}","revision":1,"activations":[
                  {"id":"health-cold","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"draw","target":"owner","amount":2},{"op":"grantSkills","target":"owner","skillIds":["{{HealthWatch}}"]},
                    {"op":"loseHp","target":"owner","amountExpr":["handCount","owner"]},
                    {"op":"recover","target":"owner","amountExpr":["handCount","owner"]},{"op":"draw","target":"owner","amount":1}]},
                  {"id":"fatal","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"loseHp","target":"owner","amountExpr":["+",["hp","owner"],1]}]}]},
                 {"id":"{{HealthWatch}}","revision":1,"triggers":[
                  {"id":"paid-health-child","window":"afterHealthChanged","subject":"owner","optional":false,"hpChangeOccurrence":"perEvent","effects":[
                    {"op":"chooseOption","target":"owner","resultBind":"resume","options":[{"id":"resume"}]},{"op":"draw","target":"owner","amount":1}]}]},
                 {"id":"{{NativeGate}}","revision":1,"triggers":[
                  {"id":"native-query","window":"nativeDrawStarting","subject":"owner","optional":false,"effects":[
                    {"op":"setChainedState","target":"owner","chained":false}]}]},
                 {"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"paid-child","window":"cardsGained","subject":"owner","optional":false,"movementOccurrence":"perBatch","destinationZones":["hand"],
                   "movementReasons":["{{ColdDrawReason}}"],"effects":[
                    {"op":"chooseOption","target":"owner","resultBind":"resume","options":[{"id":"resume"}]},{"op":"draw","target":"owner","amount":1}]}]}]}
                """, Presentation(Skill, ColdSkill, Watch, HealthSkill, HealthWatch, NativeGate));
            foreach (var (id, program) in catalog.Programs)
                // The mandatory no-op state read supplies a real request subscriber
                // without requiring unrelated human choices or replacing any cards.
                if (id != NativeGate || nativeDraw)
                    builder.AddSkill(new(id, id, "数量表达式机制验证") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            var generals = Enumerable.Range(0, 4).Select(i => $"fixture:amount-general-{i}").ToArray();
            foreach (var id in generals) builder.AddGeneral(new(id, "数量验证", "supporter", Skill, "wei", 9,
                nativeDraw ? [ColdSkill, Watch, HealthSkill, NativeGate] : [ColdSkill, Watch, HealthSkill]));
            builder.AddDeck(new("fixture:amount-deck", "静默牌堆", fatalRescue ? 1 : 0, 0,
                [new(fatalRescue ? "standard:alcohol" : "standard:dodge", 100)]));
            builder.AddMode(new(Mode, "数量表达式验证", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 1 },
                "fixture:amount-deck", GeneralCandidateCount: 1, GeneralPoolIds: generals));
        }
    }
}
