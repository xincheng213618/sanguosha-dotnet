using System.Reflection;
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
        RedirectTargetsAndPaymentsSharePostCostRange();
    }

    private static void NativeRedirectChangesOnlyItsCurrentExecutionSlot(GameEngine game)
    {
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var native = game.GetRuntimeModule<CoreNativeCardUseFramesRuntime>();
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
        }
        finally { frames.Pop(owner.Id, owner.Kind); }
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
                  {"id":"target-order:swap","revision":1,"viewAs":[{"id":"slash","inputKinds":["dodge"],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false}]},
                  {"id":"target-order:draw","revision":1,"minimumRulesVersion": 171,"triggers":[{"id":"after-use","window":"cardUseTargetsFinalized","ownerRelation":"conversionSource","sourceSkillId":"target-order:swap","sourceViewAsId":"slash","optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"target-order:liuli","revision":1,"minimumRulesVersion":172,"triggers":[{"id":"redirect-slash","window":"slashTargetRedirecting","ownerRelation":"target","cardKinds":["slash","fireSlash","thunderSlash"],"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"slashRedirectable"},{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand","equipment"],"count":1,"destination":"discardPile","awaitMovementTriggers":true,"slashRedirectPostCostRange":true},{"op":"redirectCurrentAttack","target":"selectedTarget"}]}]}
                ]}
                """), """
                {"schemaVersion":3,"skills":{
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
