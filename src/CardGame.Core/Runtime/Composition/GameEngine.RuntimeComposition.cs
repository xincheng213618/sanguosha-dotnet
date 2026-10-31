#nullable enable

using CardGame.Core.Campaigns;
using System.Reflection;

namespace CardGame.Core;
[ProgramPassiveFactRequirements("mou-ol:yangwei", ProgramTrackedFact.NativeDrawInvocation | ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase)]
public sealed partial class GameEngine
{

    /// <summary>Trusted rule diagnostics, not a player projection. Returned nested collections are frozen.</summary>
    public IReadOnlyList<ProgramEndingHandCountHistory> GetProgramEndingHandCountHistoryDiagnostics() => GetRuntimeModule<EngineOwners.CoreEndingHandCountProgramsRuntime>().GetProgramEndingHandCountHistoryDiagnostics();
    private IModeBattleRulesHost CreateActivityBattleRulesHost() => this.ComposeNativeActivityBattleRulesHost();
    private IWenheRuleFlowHost CreateWenheRuleFlowHost() => this.ComposeNativeWenheRuleFlowHost();
    private IGuanduEventFlowHost CreateGuanduEventFlowHost() => this.ComposeNativeGuanduEventFlowHost();

    /// <summary>Returns immutable rule metadata for a published conversion source in this match's mode.</summary>
    public SkillProgramViewAs? GetCardConversionDefinition(CardConversionSource source) => GetRuntimeModule<EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime>().GetCardConversionDefinition(source);

    // Interim native alias to CardMovementBatchStore's original suppression stack.
    private readonly Stack<long> _suppressedCardMovedEventBatchIds;
    // Match options and content are frozen before the first equipment query.
    // The cache contains immutable definitions only, never card or frame state.
    private EquipmentContentRegistry? _equipmentRules;
    private EquipmentContentRegistry EquipmentRules => _equipmentRules ??=
        BuiltInEquipmentContent.CreateForMatch((_modeRuntime.Find<IMatchModeEquipmentProfile>() ??
            throw new InvalidOperationException("The selected mode requires an explicitly installed equipment profile."))
            .EquipmentRecipe, _contentRegistry, _contentRegistry.EquipmentPackages);
    /// <summary>
    /// An internal rules failure may have changed mutable state before it threw.
    /// A faulted session must be reconstructed from a trusted command prefix.
    /// </summary>
    public bool IsFaulted =>GetRuntimeModule<EngineOwners.CoreCommandSessionQueriesRuntime>().IsFaulted;

    /// <summary>
    /// The last complete command prefix captured before a failing operation,
    /// when this session was driven entirely through Submit.
    /// </summary>
    public GameCheckpoint? LastTrustedCheckpoint =>GetRuntimeModule<EngineOwners.CoreCommandSessionQueriesRuntime>().LastTrustedCheckpoint;

    private void EnsureSessionHealthy() => _commandSession.EnsureHealthy();
    /// <summary>Builds both UI queries together without enumerating legal actions twice.</summary>
    public HumanActionView GetHumanActionView()
=>GetRuntimeModule<EngineOwners.CoreGameEngineHandGuidanceRuntime>().GetHumanActionView();

    /// <summary>
    /// Explains the current human hand. Legal actions remain authoritative;
    /// this query does not advance a frame, consume randomness or publish events.
    /// </summary>
    public IReadOnlyList<HandCardGuidance> GetHumanHandGuidance() =>GetRuntimeModule<EngineOwners.CoreHandCardGuidanceProjectionRuntime>().GetHumanHandGuidance();
    private IHellEncounterHost CreateHellEncounterHost() => this.ComposeNativeHellEncounterHost();
    // A match-local immutable service cache, never a second allegiance or pending-state store.
    private IMatchModeAllegiancePolicy? _allegiancePolicy;
    private IMatchModeAllegiancePolicy ModeAllegiancePolicy => _allegiancePolicy ??=
        _modeRuntime.Find<IMatchModeAllegiancePolicy>() ??
            throw new InvalidOperationException("The mode requires its installed allegiance policy.");
    private MatchModeRuntime CreateMatchModeRuntime()
    {
        var context = new ModeFeatureCreationContext(_options, _modeDefinition, _contentRegistry, CreateModeHostServices());
        var runtime = new MatchModeRuntime(ModeFeatureComposition.Create(context).ToArray());
        runtime.ValidateNativePorts();
        return runtime;
    }
    private IModeEncounterHost CreateModeEncounterHost() => this.ComposeNativeModeEncounterHost();
    private ModeHostRegistry CreateModeHostServices() => new ModeHostRegistry()
        .Register<IWushuangModeHost>(() => this.ComposeNativeWushuangModeHost())
        .Register<IModeSetupRandom>(() => this.ComposeNativeModeSetupRandom())
        .Register<IModeEncounterHost>(CreateModeEncounterHost)
        .Register<IHellEncounterHost>(CreateHellEncounterHost)
        .Register<INianChallengeHost>(CreateNianChallengeHost)
        .Register<IStandardBattleHost>(CreateStandardBattleHost)
        .Register<IModeBattleRulesHost>(CreateActivityBattleRulesHost)
        .Register<IWenheRuleFlowHost>(CreateWenheRuleFlowHost)
        .Register<IGuanduEventFlowHost>(CreateGuanduEventFlowHost)
        .Register<IModeNationalRevealHost>(CreateModeNationalRevealHost)
        .Register<IModeSetupEquipmentHost>(CreateModeSetupEquipmentHost);
    private IModeNationalRevealHost CreateModeNationalRevealHost() => this.ComposeNativeModeNationalRevealHost();
    private readonly ModeRuleSet _modeRules;
    private readonly MatchModeRuntime _modeRuntime;

    private MatchParticipantSetup? _participantSetup;
    private MatchParticipantSetup ParticipantSetup => _participantSetup ??= new(_contentRegistry, _generalPool);
    private IModeSetupEquipmentHost CreateModeSetupEquipmentHost() => this.ComposeNativeModeSetupEquipmentHost();

    /// <summary>Only the acting seat receives its legal actions; this query never advances the match.</summary>
    public IReadOnlyList<LegalAction> GetLegalActions(int actorSeat)
=>GetRuntimeModule<EngineOwners.CoreNativeLegalActionsRuntime>().GetLegalActions(actorSeat);
    private INianChallengeHost CreateNianChallengeHost() => this.ComposeNativeNianChallengeHost();
    public bool IsNianClockRunning =>GetRuntimeModule<EngineOwners.CoreNianModeProjectionRuntime>().IsNianClockRunning;
    public int? NianRemainingMilliseconds =>GetRuntimeModule<EngineOwners.CoreNianModeProjectionRuntime>().NianRemainingMilliseconds;
    private ObserverDispatcher? _observers;
    private ObserverDispatcher Observers =>
        _observers ??= new(_pendingNotifications, _observerFailures, this.ComposeNativeObserverSource());
    private readonly ProgramPipelineHooks _programPipelineHooks;
    internal sealed class ProgramContinuationCatalog
    {
        internal sealed record Registration(ProgramContinuationKind Kind, int Order, string Method, Func<GameEngine, long, bool>? Resume, Func<GameEngine, ProgramSkillFrame, bool>? Movement);
        private readonly Registration[] _resume;
        private readonly Registration[] _movement;
        private readonly Registration[] _movementTail;
        internal IReadOnlyList<Registration> Registrations { get; }

        internal ProgramContinuationCatalog(IEnumerable<Registration> registrations)
        {
            var entries = registrations.OrderBy(item => item.Kind).ThenBy(item => item.Order).ToArray();
            if (entries.Length == 0 || entries.Any(item => !Enum.IsDefined(item.Kind) || item.Order < 0 || string.IsNullOrWhiteSpace(item.Method) || (item.Kind == ProgramContinuationKind.Resume ? item.Resume is null || item.Movement is not null : item.Movement is null || item.Resume is not null)))
                throw new InvalidOperationException("Program continuations require nonempty, ordered, typed registrations.");
            if (entries.GroupBy(item => (item.Kind, item.Order)).FirstOrDefault(group => group.Count() != 1)is { } duplicate)
                throw new InvalidOperationException($"Duplicate program continuation {duplicate.Key.Kind} order {duplicate.Key.Order}: " + string.Join(", ", duplicate.Select(item => item.Method)) + ".");
            Registrations = Array.AsReadOnly(entries);
            _resume = entries.Where(item => item.Kind == ProgramContinuationKind.Resume).ToArray();
            _movement = entries.Where(item => item.Kind == ProgramContinuationKind.Movement).ToArray();
            _movementTail = entries.Where(item => item.Kind == ProgramContinuationKind.MovementTail).ToArray();
        }

        internal static ProgramContinuationCatalog Discover(IEnumerable<MethodInfo> methods)
        {
            var entries = new List<Registration>();
            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<ProgramContinuationAttribute>()is not { } registration)
                    continue;
                var parameters = method.GetParameters();
                if (method.DeclaringType != typeof(GameEngine) && !ProgramRuntimeModuleCatalog.IsDeclared(method.DeclaringType) || method.DeclaringType == typeof(GameEngine) && method.IsStatic || method.IsGenericMethod || method.ReturnType != typeof(bool) || parameters.Length != 1 || parameters[0].ParameterType != typeof(ProgramSkillFrame) && (registration.Kind != ProgramContinuationKind.Resume || parameters[0].ParameterType != typeof(long)))
                    throw new InvalidOperationException($"Program continuation '{method.Name}' has an invalid host signature.");
                Func<GameEngine, long, bool>? resume = null;
                Func<GameEngine, ProgramSkillFrame, bool>? movement = null;
                if (parameters[0].ParameterType == typeof(long))
                    resume = method.DeclaringType == typeof(GameEngine) ? method.CreateDelegate<Func<GameEngine, long, bool>>() : (engine, id) => ((Func<long, bool>)engine.BindRuntimeHandler(method, typeof(Func<long, bool>)))(id);
                else
                {
                    var invoke = method.DeclaringType == typeof(GameEngine) ? method.CreateDelegate<Func<GameEngine, ProgramSkillFrame, bool>>() : (Func<GameEngine, ProgramSkillFrame, bool>)((engine, frame) => ((Func<ProgramSkillFrame, bool>)engine.BindRuntimeHandler(method, typeof(Func<ProgramSkillFrame, bool>)))(frame));
                    if (registration.Kind == ProgramContinuationKind.Resume)
                        resume = (engine, frameId) => invoke(engine, engine.ProgramLifecycleEntries.GetActiveProgramFrame(frameId));
                    else
                        movement = invoke;
                }

                entries.Add(new(registration.Kind, registration.Order, method.Name, resume, movement));
            }

            return new(entries);
        }

        internal bool TryResume(GameEngine engine, long frameId)
        {
            // A declined handler may replace the frame. Do not cache a frame here:
            // each handler resolves its current owner just as the executor does.
            foreach (var entry in _resume)
                if (entry.Resume!(engine, frameId))
                    return true;
            return false;
        }

        internal bool TryReturnMovement(GameEngine engine, ProgramSkillFrame frame, bool afterPayment = false)
        {
            // Movement callbacks share the exact child-return receipt captured by
            // their caller; do not manufacture a new frame or repay its cost.
            foreach (var entry in afterPayment ? _movementTail : _movement)
                if (entry.Movement!(engine, frame))
                    return true;
            return false;
        }
    }
    private ProgramEventHistoryIndex? _programHistory;
    // Native owners exist before rule modules can be resolved.
    private ProgramEventHistoryIndex ProgramHistory => _programHistory ??= new(_events, _pendingEvents);
    private ProgramIssuedTriggerRuntime? _programIssuedTriggers;
    private ProgramIssuedTriggerRuntime ProgramIssuedTriggers => _programIssuedTriggers ??=
        new(ProgramIssuedTriggerCatalog.Default, ResolveRuntimeHandlerTarget);

    private ProgramTriggerProofRuntime? _programTriggerProofs;
    private ProgramTriggerProofRuntime ProgramTriggerProofs => _programTriggerProofs ??=
        new(ProgramTriggerProofCatalog.Default, ResolveRuntimeHandlerTarget);
    private ProgramInteractionRouter? _programInteractions;
    private ProgramInteractionRouter ProgramInteractions => _programInteractions ??= new(ProgramInteractionCatalog.Default, BindRuntimeHandler, this.ProgramLifecycleEntries.ResolveProgramActivationChoice, _modeRuntime.PromptAnswers);
    private ProgramSelectionValidationRegistry? _programSelectionValidators;
    private ProgramSelectionValidationRegistry ProgramSelectionValidators =>
        _programSelectionValidators ??= new(EngineOwners.CoreGameEngineProgramSelectionValidationDefinitions.ProgramSelectionValidationMetadata.Value, ProgramSelectionQueries);
    private ProgramTargetResolverRegistry? _programTargetResolvers;
    private ProgramTargetResolverRegistry ProgramTargetResolvers =>
        _programTargetResolvers ??= new(EngineOwners.CoreGameEngineProgramTargetResolversDefinitions.ProgramTargetResolverMetadata.Value, ProgramSelectionQueries);

    /// <summary>Pure estimate of the viewer's proposed card and visible table; never issues an attempt or consumes usage.</summary>
    public PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, LegalAction action, int targetSeat)
=>GetRuntimeModule<EngineOwners.CoreGameEnginePublicDamagePredictionRuntime>().PredictPublicActionDamage(viewerSeat, action, targetSeat);
    private readonly ProgramRuleObservers _programRuleObservers;
    private RuleProgressionRuntime RuleProgression => _ruleProgression ??= new(_programRuleObservers, this.ComposeNativeRuleProgressionOperations());

    private void AdvanceEventRulesAndQueueFact(IGameEvent payload) => RuleProgression.AdvanceFact(payload);
    private void AdvanceRulesAndPublishState() => RuleProgression.AdvanceState();

    private void AdvanceToHumanBoundary() => _matchExecutionLoop.AdvanceToHumanBoundary();
    private ShanheCampaignSession? _shanheMarket;
    public bool HasPendingShanheMarketPurchase
{        get =>GetRuntimeModule<EngineOwners.CoreShanheMarketProjectionRuntime>().HasPendingShanheMarketPurchase;}

    /// <summary>The expedition trader belongs to its human owner; it contains no enemy private state.</summary>
    public ShanheCampaignSnapshot? CreateShanheMarketSnapshot(int viewerSeat)
=>GetRuntimeModule<EngineOwners.CoreShanheMarketProjectionRuntime>().CreateShanheMarketSnapshot(viewerSeat);

    /// <summary>Trusted-host result, imported by the campaign only after its battle journal was verified.</summary>
    public ShanheBattleMarketTransfer? CreateShanheMarketTransfer()
=>GetRuntimeModule<EngineOwners.CoreShanheMarketProjectionRuntime>().CreateShanheMarketTransfer();
    private ProgramLegalityRouter? _programLegality;
    private ProgramLegalityRouter ProgramLegality => _programLegality ??= new(EngineOwners.CoreGameEngineSkillProgramsDefinitions.ProgramLegalityMetadata.Value, this, ResolveRuntimeHandlerTarget);
    private IStandardBattleHost CreateStandardBattleHost()
    {
        if (_options.ShanheBattle is not null && _contentRegistry.Skills.TryGetValue(ShanheSourceDongdangRules.SkillId, out var definition))
            ShanheSourceDongdangRules.ValidateDefinition(definition);
        return this.ComposeNativeStandardBattleHost();
    }
    public bool IsWushuangMode =>GetRuntimeModule<EngineOwners.CoreWushuangModeProjectionRuntime>().IsWushuangMode;
    public bool IsWushuangClockRunning =>GetRuntimeModule<EngineOwners.CoreWushuangModeProjectionRuntime>().IsWushuangClockRunning;
    public int? WushuangRemainingMilliseconds =>GetRuntimeModule<EngineOwners.CoreWushuangModeProjectionRuntime>().WushuangRemainingMilliseconds;
    public WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat) =>GetRuntimeModule<EngineOwners.CoreWushuangModeProjectionRuntime>().CreateWushuangSnapshot(viewerSeat);
    private ProgramActionRouter? _programActions;
    private ProgramActionRouter ProgramActions => _programActions ??= new(EngineOwners.CoreProgramActionModulesDefinitions.ProgramActionMetadata.Value,
        BindRuntimeHandler);
    // Legacy nested adapters and explicit independent modules share one contract
    // directory; independent hook/capability entry points share one match instance.
    private ProgramCapabilityRegistry? _programCapabilities;
    private ProgramCapabilityRegistry ProgramCapabilities =>_programCapabilities ??= MatchRuntime.Capabilities;
    private ProgramCardLegality? _cardLegality;
    private ProgramCardLegality CardLegality => _cardLegality ??= new(EngineOwners.CoreProgramCardLegalityDefinitions.CardLegalityMetadata.Value,
        BindRuntimeHandler);
    private ProgramCardUsePayments? _cardUsePayments;
    private ProgramCardUsePayments CardUsePayments => _cardUsePayments ??= new(EngineOwners.CoreProgramCardUsePaymentsDefinitions.CardUsePaymentMetadata.Value,
        row => BindRuntimeHandler(row.Method, row.DelegateType));
    private ProgramDamageHooks? _damageHooks;
    private ProgramDamageHooks DamageHooks => _damageHooks ??= new(EngineOwners.CoreProgramDamageHooksDefinitions.DamageHookMetadata.Value, row => BindRuntimeHandler(row.Method, row.DelegateType), _modeRuntime.DamageRules, attack => this.ProgramDamageHooksEntries.CaptureModeDamageFacts((IDamageAttempt)attack));
    private ProgramDistanceRules? _distanceRules;
    private ProgramDistanceRules DistanceRules => _distanceRules ??= new(EngineOwners.CoreProgramDistanceRulesDefinitions.DistanceRuleMetadata.Value,
        BindRuntimeHandler);
    private ProgramNativeReturns? _nativeReturns;
    private ProgramNativeReturns NativeReturns => _nativeReturns ??= new(EngineOwners.CoreProgramNativeReturnsDefinitions.NativeReturnMetadata.Value,
        BindRuntimeHandler);
    private ProgramPhaseUsedProjections? _phaseUsedProjections;
    private ProgramPhaseUsedProjections PhaseUsedProjections => _phaseUsedProjections ??= new(EngineOwners.CoreProgramPhaseUsedProjectionsDefinitions.PhaseUsedProjectionMetadata.Value,
        method => (Func<int, string, int, int>)BindRuntimeHandler(method, typeof(Func<int, string, int, int>)));
    private ProgramRuntimeHooks? _runtimeHooks;
    private ProgramRuntimeHooks RuntimeHooks => _runtimeHooks ??= new(EngineOwners.CoreProgramRuntimeHooksDefinitions.RuntimeHookMetadata.Value,
        BindRuntimeHandler, _modeRuntime.RuntimeHooks);
internal static IReadOnlyList<MethodInfo> RuntimeModuleMethods => ProgramRuntimeModuleCatalog.Default.Methods;
    private MatchRuntime? _matchRuntime;
    private MatchRuntime MatchRuntime => _matchRuntime ??= new(ProgramRuntimeModuleCatalog.Default,
        ProgramCapabilityCatalog.Default, new MatchRuntimeAdapterFactory(this),
        Enumerable.ToArray(Enumerable.Concat(new MatchCoreCapabilityProvider(_cardZones, ProgramHistory, _cardMovementBatches, _contentRegistry,
            _players, _cardMovements, _cardSnapshotCache, turnEffects: _turnCardUseEffects, random: _random,
            skillObservations: _skillGrantRevisions).Bindings, __GeneratedActualStateOwnerBindings())),
        resolutionScheduler: _resolutionScheduler);
    internal object GetRuntimeModuleInstance(Type type) => MatchRuntime.GetModule(type);
    internal T GetRuntimeModule<T>() where T : class => (T)GetRuntimeModuleInstance(typeof(T));
    internal Delegate BindRuntimeHandler(MethodInfo method, Type delegateType) => MatchRuntime.BindHandler(method, delegateType);
    private object? ResolveRuntimeHandlerTarget(MethodInfo method) => MatchRuntime.HandlerTarget(method);
internal sealed class MatchRuntimeAdapterFactory(GameEngine engine) : IMatchRuntimeAdapterFactory
    {
        public object CreateAdapter(ProgramCapabilityCatalog.Registration registration) => registration.CreateAdapter(engine);
        public Delegate BindCoreHandler(MethodInfo method, Type delegateType) => method.CreateDelegate(delegateType, engine);
        public object CoreHandlerTarget(MethodInfo method) => engine;
    }
    private ProgramSelectionRegistry? _programSelections;
    private ProgramSelectionRegistry ProgramSelections => _programSelections ??= new(EngineOwners.CoreProgramSelectionModulesDefinitions.ProgramSelectionMetadata.Value, ProgramSelectionQueries);
    private IProgramSelectionQueries? _programSelectionQueries;
    private IProgramSelectionQueries ProgramSelectionQueries => _programSelectionQueries ??= this.ComposeNativeSelectionQueries();
    private ProgramTargetRules? _targetRules;
    private ProgramTargetRules TargetRules => _targetRules ??= new(EngineOwners.CoreProgramTargetRulesDefinitions.TargetRuleMetadata.Value,
        BindRuntimeHandler);
    private ProgramTargetWindowProtocols? _targetWindowProtocols;
    private ProgramTargetWindowProtocols TargetWindowProtocols => _targetWindowProtocols ??= new(EngineOwners.CoreProgramTargetWindowProtocolsDefinitions.TargetWindowMetadata.Value,
        BindRuntimeHandler);
    private ProgramViewContributors? _viewContributors;
    private ProgramViewContributors ViewContributors => _viewContributors ??= new(EngineOwners.CoreProgramViewContributorsDefinitions.ViewContributionMetadata.Value,
        row => BindRuntimeHandler(row.Method, row.DelegateType));
    private IPendingGameEventOutput? _pendingGameEventOutput;
    private IPendingGameEventOutput PendingEventOutput => _pendingGameEventOutput ??= new PendingGameEventOutput(() => _started, payload => this.PublicPlayedCardsEntries.Value.CapturePublicPlayedCard(payload), payload => this.ProgramEventHistoryEntries.Value.AppendPendingProgramEvent(payload), () => _pendingEvents[^1], () => checked(_eventSequence + _pendingEvents.Count), (payload, sequence) => this.SimaYiMouEntries.CaptureMouSmSecretFact(payload, sequence));
    public bool IsMoLuBuOpeningPhase =>GetRuntimeModule<EngineOwners.CharacterLuBuOlMagicRuntime>().IsMoLuBuOpeningPhase;
}
