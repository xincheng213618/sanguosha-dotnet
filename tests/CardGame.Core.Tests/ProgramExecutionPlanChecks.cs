using System.Text.Json;
using CardGame.Core;

internal static class ProgramExecutionPlanChecks
{

    public static void PlansFreezeInstructionsAndRejectAmbiguousBindings()
    {
        var loaded = Load();
        var effects = loaded.Activations.Single().Effects.ToArray();
        var first = effects[0];
        var plan = new ProgramExecutionPlan(loaded.Id, loaded.GameplayHash,
            ProgramInstructionSourceKind.Activation, "mutable-input", effects);
        effects[0] = effects[1];
        Require(ReferenceEquals(plan.GetInstruction(0).Effect, first),
            "Mutating the construction array must not rewrite a cached instruction.");

        var duplicate = new SkillProgram(loaded.Id, loaded.Revision, loaded.GameplayHash,
            loaded.RuntimeVersion, loaded.MinimumRulesVersion, loaded.Modifiers, loaded.ViewAs,
            [loaded.Activations.Single(), loaded.Activations.Single()], loaded.Triggers,
            loaded.Contributions, loaded.CardIdentities);
        RequireThrows<InvalidOperationException>(() =>
            new ProgramInstructionResolver().Resolve(duplicate, ProgramInstructionSourceKind.Activation, "draw"));

        var trigger = loaded.Triggers[0];
        var duplicateTrigger = CopyTrigger(trigger.Id, [null!]);
        var duplicateError = RequireThrows<InvalidOperationException>(() =>
            new ProgramInstructionResolver().Resolve(WithTriggers(trigger, duplicateTrigger),
                ProgramInstructionSourceKind.Trigger, trigger.Id));
        Require(duplicateError.Message == $"Skill program '{loaded.Id}' contains duplicate binding '{trigger.Id}'.",
            "A duplicate trigger must fail before its malformed instructions are compiled.");
        var nullIdError = RequireThrows<ArgumentNullException>(() =>
            new ProgramInstructionResolver().Resolve(WithTriggers(CopyTrigger(null, trigger.Effects)),
                ProgramInstructionSourceKind.Trigger, trigger.Id));
        Require(nullIdError.ParamName == "bindingId",
            "A null trigger ID must retain the execution plan's original argument boundary.");
        RedirectPaymentPlansFollowOriginalInstructionIndices();
        NativeUseResultBindingsAreTypedAndReadAfterTheirProducer();

        SkillProgram WithTriggers(params SkillProgramTrigger[] triggers) => new(loaded.Id, loaded.Revision,
            loaded.GameplayHash, loaded.RuntimeVersion, loaded.MinimumRulesVersion, loaded.Modifiers,
            loaded.ViewAs, loaded.Activations, triggers, loaded.Contributions, loaded.CardIdentities);

        SkillProgramTrigger CopyTrigger(string? id, IReadOnlyList<SkillProgramEffect> instructions) => new(id!,
            trigger.Window, trigger.SourceSkillId, trigger.SourceViewAsId, trigger.Subject, trigger.Suits,
            trigger.MinimumRank, trigger.MaximumRank, trigger.ExcludedReasons, trigger.JudgmentReasons,
            trigger.JudgmentSource, trigger.CardKinds, trigger.SourceZones, trigger.MovementOccurrence,
            trigger.DamageOccurrence, trigger.DrawPhaseMode, trigger.Optional, trigger.Condition, instructions);
    }

    private static void NativeUseResultBindingsAreTypedAndReadAfterTheirProducer()
    {
        const string select = """{"op":"selectTarget","target":"owner","targetKind":"otherLiving"}""";
        const string use = """{"op":"useVirtualSlash","target":"selectedTarget","resultBind":"native-result"}""";
        const string guard = """{"kind":"expression","expr":["==",["count",["useResultTargets","native-result"],true],1]}""";
        var suffix = "{\"op\":\"draw\",\"target\":\"owner\",\"amount\":1,\"condition\":" + guard + "}";
        SkillProgram Parse(string effects, bool active = false)
        {
            var binding = new Dictionary<string, object> { ["id"] = "use", ["effects"] = JsonSerializer.Deserialize<JsonElement>("[" + effects + "]") };
            if (active)
            {
                binding["minCards"] = binding["maxCards"] = binding["minTargets"] = binding["maxTargets"] = 0;
                binding["targetKind"] = "otherLiving";
                binding["usesPerTurn"] = 1;
            }
            else { binding["window"] = "playPhaseStarting"; binding["subject"] = "owner"; binding["optional"] = true; }
            var definition = new Dictionary<string, object> { ["id"] = "fixture:use-result-plan", ["revision"] = 1,
                [active ? "activations" : "triggers"] = new[] { binding } };
            return SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { definition } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { ["fixture:use-result-plan"] = new { name = "Native result", description = "Explicit original-frame result" } } }))
                .Programs["fixture:use-result-plan"];
        }
        foreach (var producer in new[]
        {
            use,
            """{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true,"resultBind":"native-result"}""",
            """{"op":"offerVirtualSlashOrDraw","target":"selectedTarget","resultBind":"native-result"}""",
            """{"op":"offerUnlimitedVirtualSlash","target":"owner","resultBind":"native-result"}"""
        })
        {
            var active = producer.Contains("offerUnlimitedVirtualSlash", StringComparison.Ordinal);
            var parsed = Parse(string.Join(',', select, producer, suffix), active);
            Require((active ? parsed.Activations.Single().Effects : parsed.Triggers.Single().Effects)[1].ResultBind == "native-result" &&
                    parsed.GameplayHash != Parse(string.Join(',', select, producer, suffix).Replace("native-result", "another-result", StringComparison.Ordinal), active).GameplayHash,
                "The four existing use instructions declare typed native results whose producer and later expression reads participate in the gameplay fingerprint.");
        }
        foreach (var invalid in new[]
        {
            string.Join(',', select, suffix, use), string.Join(',', select, use, use),
            string.Join(',', select, use[..^1] + ",\"condition\":" + guard + "}"),
            string.Join(',', select, """{"op":"captureNumber","target":"owner","resultBind":"native-result","valueExpr":1}""", use),
            string.Join(',', select, use, """{"op":"captureNumber","target":"owner","resultBind":"native-result","valueExpr":1}""")
        }) RequireThrows<InvalidOperationException>(() => Parse(invalid));
        using var wrongType = JsonDocument.Parse("""["useResultTargets",1]""");
        RequireThrows<InvalidOperationException>(() => ProgramExpressions.ParseParticipantSet(wrongType.RootElement, "native-result"));
    }

    private static void RedirectPaymentPlansFollowOriginalInstructionIndices()
    {
        const string select = """{"op":"selectTarget","target":"owner","targetKind":"slashRedirectable"}""";
        const string pay = """{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand","equipment"],"count":1,"destination":"discardPile","awaitMovementTriggers":true,"slashRedirectPostCostRange":true}""";
        const string redirect = """{"op":"redirectCurrentAttack","target":"selectedTarget"}""";
        const string draw = """{"op":"draw","target":"owner","amount":1}""";
        SkillProgram Parse(string effects, string window = "slashTargetRedirecting") => SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:redirect-plan","revision":1,
              "triggers":[{"id":"redirect","window":"{{window}}","ownerRelation":"target","cardKinds":["slash"],
                "optional":true,"effects":[{{effects}}]}]}]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { ["fixture:redirect-plan"] = new { name = "Redirect", description = "Shared target/payment relation" } } }))
            .Programs["fixture:redirect-plan"];
        var program = Parse(string.Join(',', select, pay, draw, redirect, draw));
        var plan = new ProgramInstructionResolver().Resolve(program, ProgramInstructionSourceKind.Trigger, "redirect");
        var relation = plan.Features.TargetPayments.ForSelection(0);
        Require(relation is { SelectionIndex: 0, PaymentIndex: 1, RedirectIndex: 3 } &&
                ReferenceEquals(relation, plan.Features.TargetPayments.ForPayment(1)) &&
                ReferenceEquals(relation, plan.Features.TargetPayments.ForRedirect(3)) &&
                plan.Features.TargetPayments.ForPayment(2) is null,
            "A shared relation compiles the original zero-based instructions once, preserving independent effects between payment and redirect and after it.");
        foreach (var invalid in new[]
        {
            string.Join(',', pay, redirect), string.Join(',', select, pay, draw),
            string.Join(',', draw, select, pay, redirect),
            string.Join(',', select.Replace("\"targetKind\":\"slashRedirectable\"", "\"targetKind\":\"slashRedirectable\",\"zones\":[\"hand\"]", StringComparison.Ordinal), pay, redirect),
            string.Join(',', select, pay, select, redirect), string.Join(',', select, pay, pay, redirect),
            string.Join(',', select, pay.Replace("\"awaitMovementTriggers\":true", "\"awaitMovementTriggers\":false", StringComparison.Ordinal), redirect),
            string.Join(',', select, pay.Replace("\"count\":1", "\"count\":2", StringComparison.Ordinal), redirect)
        }) RequireThrows<InvalidOperationException>(() => Parse(invalid));
        RequireThrows<InvalidOperationException>(() => Parse(string.Join(',', select, pay, redirect), "cardUseTargetsFinalized"));
    }

    private static SkillProgram Load() => SkillProgramCatalog.Load(
        SkillProgramTestJson.CurrentSchema("""
        {"schemaVersion":__RULES_SCHEMA__,"skills":[{"id":"fixture:plan","revision":1,"minimumRulesVersion": 171,
          "activations":[{"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
            "targetKind":"otherLiving","usesPerTurn":1,"effects":[
              {"op":"draw","target":"owner","amount":1},
              {"op":"recover","target":"owner","amount":1}]}],
          "triggers":[
            {"id":"draw","window":"turnEnding","subject":"owner","optional":true,
             "effects":[{"op":"draw","target":"owner","amount":1}]},
            {"id":"judgment-draw","window":"judgmentFinalized","subject":"owner",
             "suits":["club"],"minimumRank":1,"maximumRank":13,"excludedReasons":[],
             "optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}
          ]}]}
        """),
        """{"schemaVersion":3,"skills":{"fixture:plan":{"name":"Plan","description":"Plan fixture"}}}""")
        .Programs["fixture:plan"];

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static TException RequireThrows<TException>(Action action) where TException : Exception
    {
        try { action(); }
        catch (TException error) { return error; }
        throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
    }
}
