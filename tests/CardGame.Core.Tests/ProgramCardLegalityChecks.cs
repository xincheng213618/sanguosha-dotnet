using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ProgramCardLegalityChecks
{
    public static void OrderedStagesAndLiveQueries()
    {
        var supplied = Methods(typeof(Fixture)).Reverse().ToList();
        var catalog = ProgramCardLegalityCatalog.Discover(supplied);
        supplied.Clear();
        Need(catalog.Registrations is IDictionary<ProgramCardLegalityStage, IReadOnlyList<ProgramCardLegalityRegistration>> { IsReadOnly: true } &&
            catalog.Registrations.All(pair => pair.Value is IList<ProgramCardLegalityRegistration> { IsReadOnly: true } &&
                pair.Value.Select(row => row.Order).SequenceEqual(pair.Value.Select(row => row.Order).Distinct().Order())) &&
            catalog.Registrations[ProgramCardLegalityStage.BeforeTurnUseBan].Select(row => row.Order).SequenceEqual([10, 20]) &&
            catalog.Registrations[ProgramCardLegalityStage.EntityPhysicalBan].Select(row => row.Order).SequenceEqual([10]),
            "Each native stage retains its own frozen explicit order and admission set.");
        try { ((IList<ProgramCardLegalityRegistration>)catalog.Registrations[ProgramCardLegalityStage.BeforeTurnUseBan]).Clear(); throw new Exception("Expected frozen metadata."); }
        catch (NotSupportedException) { }

        var a = new Fixture(catalog);
        var b = new Fixture(catalog);
        var calls = new Func<Fixture, bool>[]
        {
            fixture => fixture.Legality.IsUseForbidden(ProgramCardLegalityStage.BeforeIntrinsicUseBan, fixture.Use),
            fixture => fixture.Legality.IsUseForbidden(ProgramCardLegalityStage.BeforeTurnUseBan, fixture.Use),
            fixture => fixture.Legality.IsActionForbidden(ProgramCardLegalityStage.BeforeLegalActionKind, fixture.Action),
            fixture => fixture.Legality.IsActionForbidden(ProgramCardLegalityStage.AfterLegalActionKind, fixture.Action),
            fixture => fixture.Legality.IsMaterialForbidden(fixture.Material),
            fixture => fixture.Legality.IsViewAsForbidden(fixture.ViewAs),
            fixture => fixture.Legality.IsConfiguredConversionForbidden(ProgramCardLegalityStage.NamedConversionBan, fixture.Configured),
            fixture => fixture.Legality.IsConfiguredConversionForbidden(ProgramCardLegalityStage.ConfiguredConversionBan, fixture.Configured),
            fixture => fixture.Legality.IsConfiguredConversionForbidden(ProgramCardLegalityStage.ConversionUsageBan, fixture.Configured),
            fixture => fixture.Legality.IsConversionSourceForbidden(ProgramCardLegalityStage.DyingConversionBan, fixture.Source),
            fixture => fixture.Legality.IsConversionSourceForbidden(ProgramCardLegalityStage.ConversionSourceBan, fixture.Source),
            fixture => fixture.Legality.IsPhysicalForbidden(ProgramCardLegalityStage.HandPhysicalBan, 2, 7),
            fixture => fixture.Legality.IsNativeResponseForbidden(fixture.NativeResponse),
            fixture => fixture.Legality.AllowsNativeSlashAlias(fixture.NativeSlashAlias),
            fixture => fixture.Legality.IsNativeSlashOutputForbidden(fixture.NativeSlashConversion),
            fixture => fixture.Legality.IsUnspecifiedNativeSlashForbidden(fixture.NativeSlashConversion)
        };
        foreach (var call in calls)
        foreach (var first in new[] { false, true })
        foreach (var last in new[] { false, true })
        {
            a.Trace.Clear(); a.BlockFirst = first; a.BlockLast = last;
            Need(call(a) == (first || last) && a.Trace.SequenceEqual(first ? ["first"] : ["first", "last"]),
                "Each live query retains short circuit order and independently rechecks dynamic state.");
            Need(b.Trace.Count == 0, "Shared metadata must bind delegates to the querying match.");
        }
        foreach (var first in new[] { false, true })
        foreach (var last in new[] { false, true })
        {
            a.Trace.Clear(); a.BlockFirst = first; a.BlockLast = last;
            Need(a.Legality.IsPhysicalForbidden(ProgramCardLegalityStage.EntityPhysicalBan, 2, 7) == last &&
                a.Trace.SequenceEqual(["last"]), "Entity-only queries must not acquire the hand-only physical prohibition.");
        }
        a.Trace.Clear(); a.BlockFirst = false; a.BlockLast = false;
        Need(a.NativeResponse.Source is null && !a.NativeResponse.DyingUse, "Native response defaults must remain an absent conversion and a non-dying response.");
        a.NativeResponse = a.NativeResponse with { Source = a.Source.Source, DyingUse = true };
        Need(!a.Legality.IsNativeResponseForbidden(a.NativeResponse) && a.Trace.SequenceEqual(["first", "last"]),
            "Native response queries preserve explicit conversion and dying-use arguments without changing material identity.");
        a.Trace.Clear(); a.BlockFirst = true; b.BlockFirst = false; b.BlockLast = false;
        Need(calls[0](a) && !calls[0](b) && b.Trace.SequenceEqual(["first", "last"]), "Match-bound policy state remains independent.");
        Need(a.MaterialEnumerations == 0, "Dispatch must forward a lazy material sequence without enumerating or copying it.");
        Need(a.Bound.Distinct(StringComparer.Ordinal).Count() == a.Bound.Count && a.Bound.Count == b.Bound.Count,
            "Methods registered at multiple native stages bind once per engine, not once per query.");

        a.Trace.Clear();
        var nativeVariants = a.Legality.NativeSlashVariants(a.NativeSlashMaterial);
        Need(a.Trace.SequenceEqual(["variants-first", "variants-last"]) &&
            nativeVariants.Select(v => v.EffectiveKind).SequenceEqual([CardKind.Slash, CardKind.FireSlash]) &&
            nativeVariants is IList<SlashUseVariant> { IsReadOnly: true },
            "Native Slash variants concatenate in explicit order and expose a frozen result.");
        a.FirstVariants.Clear(); a.LastVariants.Clear();
        Need(nativeVariants.Select(v => v.EffectiveKind).SequenceEqual([CardKind.Slash, CardKind.FireSlash]),
            "A module's mutable temporary result cannot later change published native variants.");
        a.Trace.Clear();
        Need(a.Legality.SelectNativeSlashKind(a.SelectedSlash) == CardKind.ThunderSlash &&
            a.Trace.SequenceEqual(["select-first", "select-last"]),
            "Each selected native Slash policy receives the previous kind while preserving the original entity and source.");
        a.Trace.Clear();
        Need(a.Legality.SelectAiNativeSlashKind(a.AiSlash) == CardKind.ThunderSlash &&
            a.Trace.SequenceEqual(["ai-first", "ai-last"]),
            "AI native Slash kind selection preserves its published decision and ordered fallback.");

        a.Trace.Clear(); a.DropProjection = false;
        Need(a.Legality.ProjectAction(a.Action) is { Description: "fixture:first:last" } &&
            a.Trace.SequenceEqual(["project-first", "project-last"]), "Action projections receive the previous action and retain native context.");
        a.Trace.Clear(); a.DropProjection = true;
        Need(a.Legality.ProjectAction(a.Action) is null && a.Trace.SequenceEqual(["project-first"]),
            "A removed legal action must never enter another projection or later restriction stage.");

        a.Trace.Clear(); a.Legality.ValidateSubmittedUse(a.Submitted);
        Need(a.Trace.SequenceEqual(["validate-first", "validate-last"]), "Submit validation preserves the conversion chain and every ordered validator.");
        a.Trace.Clear(); a.Failure = new InvalidOperationException("original native validation failure");
        try { a.Legality.ValidateSubmittedUse(a.Submitted); throw new Exception("Expected native validation failure."); }
        catch (InvalidOperationException error) when (ReferenceEquals(error, a.Failure))
        { Need(a.Trace.SequenceEqual(["validate-first"]), "Original submit errors stop later policies without reflection wrapping."); }
        a.Trace.Clear();
        try { _ = a.Legality.IsUseForbidden(ProgramCardLegalityStage.BeforeTurnUseBan, a.Use); throw new Exception("Expected original query failure."); }
        catch (InvalidOperationException error) when (ReferenceEquals(error, a.Failure))
        { Need(a.Trace.SequenceEqual(["first"]), "Original query failures retain their identity and order."); }

        var empty = new ProgramCardLegality(ProgramCardLegalityCatalog.Discover([]), (_, _) => throw new Exception("Unexpected empty bind."));
        Need(!empty.IsUseForbidden(ProgramCardLegalityStage.BeforeTurnUseBan, a.Use) &&
            !empty.IsMaterialForbidden(a.Material) && !empty.IsViewAsForbidden(a.ViewAs) &&
            !empty.IsConfiguredConversionForbidden(ProgramCardLegalityStage.NamedConversionBan, a.Configured) &&
            !empty.IsConversionSourceForbidden(ProgramCardLegalityStage.ConversionSourceBan, a.Source) &&
            !empty.IsPhysicalForbidden(ProgramCardLegalityStage.HandPhysicalBan, 2, 7) &&
            !empty.IsPhysicalForbidden(ProgramCardLegalityStage.EntityPhysicalBan, 2, 7) &&
            !empty.IsNativeResponseForbidden(a.NativeResponse) &&
            !empty.AllowsNativeSlashAlias(a.NativeSlashAlias) &&
            empty.NativeSlashVariants(a.NativeSlashMaterial) is { Count: 0 } emptyVariants && emptyVariants is IList<SlashUseVariant> { IsReadOnly: true } &&
            !empty.IsNativeSlashOutputForbidden(a.NativeSlashConversion) && !empty.IsUnspecifiedNativeSlashForbidden(a.NativeSlashConversion) &&
            empty.SelectNativeSlashKind(a.SelectedSlash) == a.SelectedSlash.Fallback &&
            empty.SelectAiNativeSlashKind(a.AiSlash) == a.AiSlash.Fallback &&
            ReferenceEquals(empty.ProjectAction(a.Action), a.Action.Action), "Empty policies preserve native legality and action identity.");
        empty.ValidateSubmittedUse(a.Submitted);
        var staticCatalog = ProgramCardLegalityCatalog.Discover(Methods(typeof(StaticQuery)));
        var stateless = new ProgramCardLegality(staticCatalog, (method, type) => method.CreateDelegate(type));
        Need(stateless.IsUseForbidden(ProgramCardLegalityStage.BeforeTurnUseBan, a.Use), "Stateless policies retain the same contract.");
        Reject(typeof(Duplicate), "Duplicate card legality order");
        Reject(typeof(InvalidStage), "valid stage");
        Reject(typeof(InvalidOrder), "positive order");
        Reject(typeof(InvalidResult), "invalid handler signature");
        Reject(typeof(OptionalArgument), "invalid handler signature");
        Reject(typeof(GenericQuery), "invalid handler signature");
        Reject(typeof(RefArgument), "invalid handler signature");
        var production = ProgramCardLegalityCatalog.Discover(ProgramRuntimeModuleCatalog.Default.Methods);
        Need(catalog.Registrations.Where(pair => pair.Value.Count > 0).All(pair => production.Registrations[pair.Key].Count > 0),
            "Every exercised production native stage retains its module-owned policies; unregistered stages remain valid.");
        AppliedSuitRestrictionsKeepTheirRecipientAndOriginalBoundary();

        static void Reject(Type type, string expected)
        {
            try { _ = ProgramCardLegalityCatalog.Discover(Methods(typeof(Fixture)).Concat(Methods(type))); }
            catch (InvalidOperationException error) when (error.Message.Contains(expected, StringComparison.Ordinal)) { return; }
            throw new InvalidOperationException($"Did not reject {type.Name}: {expected}");
        }
    }

    private static void AppliedSuitRestrictionsKeepTheirRecipientAndOriginalBoundary()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, UseInteractiveSetup = false,
            AdvanceAfterHumanCommands = false }, StandardContentRegistry.Create());
        Need(game.Submit(new StartGameCommand()).Accepted, "The shared restriction fixture must start through its command boundary.");
        for (var step = 0; step < 20 && game.State.TurnNumber == 0; step++)
            Need(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "The shared restriction fixture must enter an actual turn.");
        Need(game.State.TurnNumber > 0, "Applied restrictions require a real current turn.");
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var journal = capabilities.Get<IProgramEventJournalQueries>();
        var events = capabilities.Get<IProgramRuntimeEvents>();
        var restrictions = capabilities.Get<IAppliedCardActionRestrictions>();
        var lifetimes = game.GetRuntimeModule<ProgramSkillChangeRuntime>();
        var turn = capabilities.Get<IProgramTurnQueries>();
        var store = (TurnCardUseEffectStore)typeof(GameEngine)
            .GetField("_turnCardUseEffects", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var issuer = capabilities.Get<IMatchParticipantStateAccess>().At(0);
        var source = issuer.SkillGrants.Grants.First();
        var program = capabilities.Get<IProgramContentQueries>().Skill(source.SkillId).Program!;
        var activation = program.Activations.FirstOrDefault(binding => binding.Effects.Count > 0);
        var trigger = activation is null ? program.Triggers.First(binding => binding.Effects.Count > 0) : null;
        var bindingId = activation?.Id ?? trigger!.Id!;
        ProgramSkillFrame IssuedFrame() => new(frames.NextId(), 0, source.SkillId, bindingId, program.GameplayHash, 1, [], [])
            { SkillInstanceId = source.SkillInstanceId, TriggerId = trigger?.Id };
        var alive = issuer.IsAlive;
        var frame = IssuedFrame();
        frames.Push(frame);
        try
        {
            var cutoff = journal.Cursor;
            var spade = restrictions.GrantNextActualUseSuit(frame, 1, Suit.Spade);
            Need(spade.AffectedSeat == 1 && spade.Source.OwnerSeat == 0 && spade.PhysicalMaterialCount == 1 &&
                spade.Lifetime!.Until.Single(end => end.Event == SkillStateBoundaryEvent.ActualUse).IssuedOccurrence == cutoff,
                "A shared next-use restriction freezes the actual recipient, one-material rule and original journal cutoff independently of its issuer.");
            Need(ReferenceEquals(spade, restrictions.GrantNextActualUseSuit(frame, 1, Suit.Spade)),
                "The same original instruction reuses its applied state after publishing facts without refreshing the boundary cutoff.");
            frames.Pop(frame.Id, frame.Kind);
            frames.Push(frame = IssuedFrame());
            var heart = restrictions.GrantNextActualUseSuit(frame, 1, Suit.Heart);
            var provider = restrictions.GrantNextActualUseSuit(frame, 0, Suit.Heart);
            var beforeQueries = journal.Cursor;
            Need(restrictions.IsMaterialUseForbidden(1, [1], Suit.Spade) && restrictions.IsMaterialUseForbidden(1, [1], Suit.Heart) &&
                !restrictions.IsMaterialUseForbidden(1, [1], Suit.Club) && !restrictions.IsMaterialUseForbidden(2, [1], Suit.Spade) &&
                !restrictions.IsMaterialUseForbidden(1, [], Suit.Spade) && !restrictions.IsMaterialUseForbidden(1, [1, 2], Suit.Spade) &&
                !restrictions.IsMaterialUseForbidden(1, [1], null) && !restrictions.IsMaterialUseForbidden(1, [1], Suit.None) &&
                restrictions.IsMaterialUseForbidden(0, [1], Suit.Heart) && journal.Cursor == beforeQueries,
                "Only the actual recipient's final effective suit with one physical material is forbidden; zero/multi-material, unknown and suitless uses stay distinct, and queries emit no facts.");
            Need(!store.IsCardUseForbidden(turn.TurnNumber, turn.TurnOwnerSeat, 1, CardKind.Slash, CardActionType.Use),
                "A suit-specific material restriction cannot become an unconditional card-kind prohibition.");
            store.GrantActionProhibition(turn.TurnNumber, turn.TurnOwnerSeat, frame.Id, 99, spade.Source,
                [CardKind.Slash], [CardActionType.Use], affectedSeat: 1);
            Need(store.IsCardUseForbidden(turn.TurnNumber, turn.TurnOwnerSeat, 1, CardKind.Slash, CardActionType.Use) &&
                !store.IsCardUseForbidden(turn.TurnNumber, turn.TurnOwnerSeat, 1, CardKind.Peach, CardActionType.Use),
                "The existing kind-only prohibition still answers its original kind query independently of applied suit state.");

            issuer.SkillGrants.RemoveGrant(source.GrantId); issuer.IsAlive = false;
            lifetimes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.Death, 0, SkillStateBoundaryTiming.Before, 1));
            Need(restrictions.IsMaterialUseForbidden(1, [1], Suit.Spade),
                "Source loss and death cannot withdraw an already applied recipient restriction.");
            events.AdvanceFact(new CardActionAcceptedEvent(new(900001, null, CardActionType.Use, 1, 2, null, null, null,
                CardKind.Slash, [2], [], [], effectiveSuit: Suit.Club)));
            Need(restrictions.IsMaterialUseForbidden(1, [1], Suit.Spade),
                "An accepted-only ordinary card attempt does not consume its actor's next actual-use state.");
            lifetimes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.ActualUse, 1, SkillStateBoundaryTiming.Before, cutoff));
            lifetimes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.ActualUse, 1, SkillStateBoundaryTiming.After, journal.Cursor + 1));
            lifetimes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.ActualUse, 2, SkillStateBoundaryTiming.Before, journal.Cursor + 1));
            Need(restrictions.IsMaterialUseForbidden(1, [1], Suit.Spade) && restrictions.IsMaterialUseForbidden(1, [1], Suit.Heart),
                "The issuing occurrence, wrong event side and another actor's use do not consume recipient states.");
            var occurrence = journal.Cursor + 1;
            lifetimes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.ActualUse, 1, SkillStateBoundaryTiming.Before, occurrence));
            lifetimes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.ActualUse, 1, SkillStateBoundaryTiming.Before, occurrence));
            Need(!restrictions.IsMaterialUseForbidden(1, [1], Suit.Spade) && !restrictions.IsMaterialUseForbidden(1, [1], Suit.Heart) &&
                restrictions.IsMaterialUseForbidden(0, [1], Suit.Heart) &&
                journal.Complete().OfType<CardActionProhibitionExpiredEvent>().Count(fact => fact.GrantSequence == spade.GrantSequence) == 1 &&
                journal.Complete().OfType<CardActionProhibitionExpiredEvent>().Count(fact => fact.GrantSequence == heart.GrantSequence) == 1,
                "One actual use removes every outstanding recipient restriction once while leaving the material provider's independent state intact.");
            lifetimes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.TurnEnded, game.State.CurrentSeat,
                SkillStateBoundaryTiming.After, game.State.TurnNumber));
            Need(restrictions.IsMaterialUseForbidden(0, [1], Suit.Heart),
                "The actual turn's ending window still retains its issued state until the declared turn-return boundary.");
            store.ExpireTurn(turn.TurnNumber, turn.TurnOwnerSeat);
            Need(restrictions.IsMaterialUseForbidden(0, [1], Suit.Heart) &&
                !store.IsCardUseForbidden(turn.TurnNumber, turn.TurnOwnerSeat, 1, CardKind.Slash, CardActionType.Use),
                "Legacy turn cleanup removes the old kind ban while the applied state waits for actual turn return.");
            var end = provider.Lifetime!.Until.Single(boundary => boundary.Event == SkillStateBoundaryEvent.ActualTurnReturned);
            Need(end.Timing == SkillStateBoundaryTiming.After,
                "The production next-use state remains issued until the actual turn-return fact has entered the shared journal.");
            lifetimes.ProcessSkillStateBoundary(new(end.Event, end.Seat, SkillStateBoundaryTiming.Before, end.IssuedOccurrence));
            Need(restrictions.IsMaterialUseForbidden(0, [1], Suit.Heart),
                "Before actual turn return, the original recipient restriction still applies to any nested real use.");
            lifetimes.ProcessSkillStateBoundary(new(end.Event, end.Seat, SkillStateBoundaryTiming.After, end.IssuedOccurrence));
            Need(!restrictions.IsMaterialUseForbidden(0, [1], Suit.Heart) &&
                journal.Complete().OfType<CardActionProhibitionExpiredEvent>().Count(fact => fact.GrantSequence == provider.GrantSequence) == 1,
                "The shared actual-turn-return lifetime expires an unused state without depending on its source or a character history ledger.");
        }
        finally
        {
            issuer.IsAlive = alive;
            if (issuer.SkillGrants.Grants.All(grant => grant.GrantId != source.GrantId)) issuer.SkillGrants.Grant(source);
            frames.Pop(frame.Id, frame.Kind);
        }
        AppliedSuitRestrictionDefinitionsRejectAmbiguousShapes();
    }

    private static void AppliedSuitRestrictionDefinitionsRejectAmbiguousShapes()
    {
        const string id = "fixture:applied-suit-shape";
        var effect = JsonNode.Parse("""
            {"op":"grantTurnCardActionProhibition","target":"selectedTarget","actionTypes":["use"],"suits":["heart"],"physicalMaterialCount":1,
             "until":[{"event":"actualUse","participant":{"kind":"target"},"timing":"before","skipIssued":true},
                      {"event":"actualTurnReturned","participant":{"kind":"turnOwner"},"timing":"before"}]}
            """)!;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = id, description = "共享下一次实际使用花色限制" } } });
        string Rules(JsonNode node) => JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[] { new { id, revision = 1, activations = new[] { new { id = "apply", minCards = 0, maxCards = 0,
                minTargets = 1, maxTargets = 1, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { node } } } } } });
        _ = SkillProgramCatalog.Load(Rules(effect), presentation);
        foreach (var mixedKinds in new[] { true, false })
        {
            var invalid = effect.DeepClone();
            if (mixedKinds) invalid["cardKinds"] = new JsonArray("slash");
            else invalid["actionTypes"] = new JsonArray("response");
            var rejected = false;
            try { _ = SkillProgramCatalog.Load(Rules(invalid), presentation); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "The shared loader rejects mixed kind/suit restrictions and response-only action restrictions with a next-use lifetime.");
        }
    }

    private static MethodInfo[] Methods(Type type) => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture
    {
        internal List<string> Trace { get; } = [];
        internal List<string> Bound { get; } = [];
        internal bool BlockFirst { get; set; }
        internal bool BlockLast { get; set; }
        internal bool DropProjection { get; set; }
        internal Exception? Failure { get; set; }
        internal int MaterialEnumerations { get; private set; }
        internal ProgramCardLegality Legality { get; }
        internal ProgramCardUseQuery Use { get; } = new(2, CardKind.FireSlash, CardActionType.Response, 123);
        internal ProgramLegalActionQuery Action { get; }
        internal ProgramCardMaterialQuery Material { get; }
        internal ProgramViewAsLegalityQuery ViewAs { get; }
        internal ProgramConfiguredViewAsQuery Configured { get; }
        internal ProgramViewAsSourceQuery Source { get; }
        internal ProgramSubmittedUseQuery Submitted { get; }
        internal ProgramNativeResponseQuery NativeResponse { get; set; }
        internal ProgramNativeSlashAliasQuery NativeSlashAlias { get; }
        internal ProgramNativeSlashMaterialQuery NativeSlashMaterial { get; }
        internal ProgramNativeSlashConversionQuery NativeSlashConversion { get; }
        internal ProgramSelectedNativeSlashQuery SelectedSlash { get; }
        internal ProgramAiNativeSlashQuery AiSlash { get; }
        internal List<SlashUseVariant> FirstVariants { get; } = [new(CardKind.Slash, null, false)];
        internal List<SlashUseVariant> LastVariants { get; } = [new(CardKind.FireSlash, null, true)];

        internal Fixture(ProgramCardLegalityCatalog catalog)
        {
            var owner = new CharacterState
            {
                Seat = 2, Name = "fixture", IsHuman = true, Role = Role.Rebel,
                RoleRevealed = false, General = new("fixture", "Fixture", "fixture", []),
                GeneralSelected = true, GeneralRevealed = true, MaxHp = 4, Hp = 3
            };
            var card = new Card(7, CardKind.Slash, Suit.Club, 8);
            var source = new CardConversionSource("fixture:conversion", "fixture", 2, "fixture:instance");
            Action = new(owner, new(LegalActionKind.Slash, card.Id, 1, "fixture", CardKind.FireSlash), CardKind.FireSlash, card);
            Material = new(2, CardKind.FireSlash, Materials(), true);
            ViewAs = new(owner, CardKind.FireSlash, [7, 8], source, true, false, 1);
            // These fixtures exercise argument forwarding only; no definition is
            // inspected by a callback, so no content loader fixture is needed.
            Configured = new(owner, card, null!, null!, true, false, 1);
            Source = new(owner, card, source, true, false);
            Submitted = new(2, CardKind.FireSlash, [7, 8], [source]);
            NativeResponse = new(owner, CardKind.Dodge, [7, 8]);
            NativeSlashAlias = new(owner, Configured.Instance, Configured.Rule, CardKind.ThunderSlash, true);
            NativeSlashMaterial = new(owner, card);
            NativeSlashConversion = new(owner, card, CardKind.FireSlash, source);
            SelectedSlash = new(owner, card, CardKind.Slash, source);
            AiSlash = new(new(DecisionKind.RespondSlash, 2, "native fixture", [7], []), card, CardKind.Slash);
            Legality = new(catalog, (method, type) =>
            {
                Bound.Add(method.Name);
                return method.CreateDelegate(type, this);
            });
        }

        private IEnumerable<int> Materials() { MaterialEnumerations++; yield return 7; yield return 8; }
        private bool Visit(bool first)
        {
            Trace.Add(first ? "first" : "last");
            if (Failure is { } error) throw error;
            return first ? BlockFirst : BlockLast;
        }
        [ProgramCardLegality(ProgramCardLegalityStage.BeforeIntrinsicUseBan, 10)]
        [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 10)]
        private bool UseFirst(ProgramCardUseQuery query) { Need(query == Use, "Use parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.BeforeIntrinsicUseBan, 20)]
        [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 20)]
        private bool UseLast(ProgramCardUseQuery query) { Need(query == Use, "Use parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.BeforeLegalActionKind, 10)]
        [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 10)]
        private bool ActionFirst(ProgramLegalActionQuery query) { Need(query == Action, "Action parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.BeforeLegalActionKind, 20)]
        [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 20)]
        private bool ActionLast(ProgramLegalActionQuery query) { Need(query == Action, "Action parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 10)]
        private bool MaterialFirst(ProgramCardMaterialQuery query) { Need(query == Material, "Lazy material parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 20)]
        private bool MaterialLast(ProgramCardMaterialQuery query) { Need(query == Material, "Lazy material parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 10)]
        private bool ViewAsFirst(ProgramViewAsLegalityQuery query) { Need(query == ViewAs, "ViewAs parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 20)]
        private bool ViewAsLast(ProgramViewAsLegalityQuery query) { Need(query == ViewAs, "ViewAs parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.NamedConversionBan, 10)]
        [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 10)]
        [ProgramCardLegality(ProgramCardLegalityStage.ConversionUsageBan, 10)]
        private bool ConfiguredFirst(ProgramConfiguredViewAsQuery query) { Need(query == Configured, "Configured conversion parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.NamedConversionBan, 20)]
        [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 20)]
        [ProgramCardLegality(ProgramCardLegalityStage.ConversionUsageBan, 20)]
        private bool ConfiguredLast(ProgramConfiguredViewAsQuery query) { Need(query == Configured, "Configured conversion parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.DyingConversionBan, 10)]
        [ProgramCardLegality(ProgramCardLegalityStage.ConversionSourceBan, 10)]
        private bool SourceFirst(ProgramViewAsSourceQuery query) { Need(query == Source, "Conversion source parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.DyingConversionBan, 20)]
        [ProgramCardLegality(ProgramCardLegalityStage.ConversionSourceBan, 20)]
        private bool SourceLast(ProgramViewAsSourceQuery query) { Need(query == Source, "Conversion source parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.HandPhysicalBan, 10)]
        private bool PhysicalFirst(int actorSeat, int cardId)
        { Need(actorSeat == 2 && cardId == 7, "Physical entity parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.HandPhysicalBan, 20)]
        [ProgramCardLegality(ProgramCardLegalityStage.EntityPhysicalBan, 10)]
        private bool PhysicalLast(int actorSeat, int cardId)
        { Need(actorSeat == 2 && cardId == 7, "Physical entity parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 10)]
        private bool ResponseFirst(ProgramNativeResponseQuery query)
        { Need(query == NativeResponse, "Native response parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 20)]
        private bool ResponseLast(ProgramNativeResponseQuery query)
        { Need(query == NativeResponse, "Native response parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.NativeSlashAliasAllowance, 10)]
        private bool AliasFirst(ProgramNativeSlashAliasQuery query)
        { Need(query == NativeSlashAlias, "Native Slash alias parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.NativeSlashAliasAllowance, 20)]
        private bool AliasLast(ProgramNativeSlashAliasQuery query)
        { Need(query == NativeSlashAlias, "Native Slash alias parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.NativeSlashVariants, 10)]
        private IReadOnlyList<SlashUseVariant> VariantsFirst(ProgramNativeSlashMaterialQuery query)
        { Need(query == NativeSlashMaterial, "Native Slash variant material changed."); Trace.Add("variants-first"); return FirstVariants; }
        [ProgramCardLegality(ProgramCardLegalityStage.NativeSlashVariants, 20)]
        private IReadOnlyList<SlashUseVariant> VariantsLast(ProgramNativeSlashMaterialQuery query)
        { Need(query == NativeSlashMaterial, "Native Slash variant material changed."); Trace.Add("variants-last"); return LastVariants; }
        [ProgramCardLegality(ProgramCardLegalityStage.NativeSlashOutputBan, 10)]
        [ProgramCardLegality(ProgramCardLegalityStage.UnspecifiedNativeSlashBan, 10)]
        private bool SlashConversionFirst(ProgramNativeSlashConversionQuery query)
        { Need(query == NativeSlashConversion, "Native Slash conversion parameters changed."); return Visit(true); }
        [ProgramCardLegality(ProgramCardLegalityStage.NativeSlashOutputBan, 20)]
        [ProgramCardLegality(ProgramCardLegalityStage.UnspecifiedNativeSlashBan, 20)]
        private bool SlashConversionLast(ProgramNativeSlashConversionQuery query)
        { Need(query == NativeSlashConversion, "Native Slash conversion parameters changed."); return Visit(false); }
        [ProgramCardLegality(ProgramCardLegalityStage.SelectedNativeSlashKind, 10)]
        private CardKind SelectFirst(ProgramSelectedNativeSlashQuery query)
        { Need(query == SelectedSlash, "Selected native Slash initial context changed."); Trace.Add("select-first"); return CardKind.FireSlash; }
        [ProgramCardLegality(ProgramCardLegalityStage.SelectedNativeSlashKind, 20)]
        private CardKind SelectLast(ProgramSelectedNativeSlashQuery query)
        { Need(query == (SelectedSlash with { Fallback = CardKind.FireSlash }), "Selected native Slash lost its previous kind."); Trace.Add("select-last"); return CardKind.ThunderSlash; }
        [ProgramCardLegality(ProgramCardLegalityStage.AiNativeSlashKind, 10)]
        private CardKind AiFirst(ProgramAiNativeSlashQuery query)
        { Need(query == AiSlash, "AI native Slash initial decision changed."); Trace.Add("ai-first"); return CardKind.FireSlash; }
        [ProgramCardLegality(ProgramCardLegalityStage.AiNativeSlashKind, 20)]
        private CardKind AiLast(ProgramAiNativeSlashQuery query)
        { Need(query == (AiSlash with { Fallback = CardKind.FireSlash }), "AI native Slash lost its previous kind."); Trace.Add("ai-last"); return CardKind.ThunderSlash; }
        [ProgramCardLegality(ProgramCardLegalityStage.LegalActionProjection, 10)]
        private LegalAction? ProjectFirst(ProgramLegalActionQuery query)
        {
            Need(query == Action, "Initial projection parameters changed."); Trace.Add("project-first");
            return DropProjection ? null : query.Action with { Description = query.Action.Description + ":first" };
        }
        [ProgramCardLegality(ProgramCardLegalityStage.LegalActionProjection, 20)]
        private LegalAction? ProjectLast(ProgramLegalActionQuery query)
        {
            Need(query == (Action with { Action = Action.Action with { Description = "fixture:first" } }), "Previous action or native projection context changed.");
            Trace.Add("project-last"); return query.Action with { Description = query.Action.Description + ":last" };
        }
        [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 10)]
        private void ValidateFirst(ProgramSubmittedUseQuery query)
        {
            Need(query == Submitted, "Submit parameters changed."); Trace.Add("validate-first");
            if (Failure is { } error) throw error;
        }
        [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 20)]
        private void ValidateLast(ProgramSubmittedUseQuery query) { Need(query == Submitted, "Submit parameters changed."); Trace.Add("validate-last"); }
    }

    private sealed class StaticQuery
    { [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 10)] private static bool Query(ProgramCardUseQuery query) => query.ActorSeat == 2; }
    private sealed class Duplicate
    { [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 10)] private bool Query(ProgramCardUseQuery query) => false; }
    private sealed class InvalidStage
    { [ProgramCardLegality((ProgramCardLegalityStage)999, 10)] private bool Query(ProgramCardUseQuery query) => false; }
    private sealed class InvalidOrder
    { [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 0)] private bool Query(ProgramCardUseQuery query) => false; }
    private sealed class InvalidResult
    { [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 999)] private int Query(ProgramCardUseQuery query) => 0; }
    private sealed class OptionalArgument
    { [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 999)] private bool Query(ProgramCardUseQuery query = default) => false; }
    private sealed class GenericQuery
    { [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 999)] private bool Query<T>(ProgramCardUseQuery query) => false; }
    private sealed class RefArgument
    { [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 999)] private bool Query(ref ProgramCardUseQuery query) => false; }
}
