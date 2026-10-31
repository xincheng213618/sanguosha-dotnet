namespace CardGame.Core;
/// <summary>
/// A synchronous, explicit state machine. Advance runs AI turns until it reaches
/// a human decision. WPF stays responsive by calling these short methods itself;
/// no UI type, dispatcher, timer, or blocking wait exists in the rules assembly.
/// </summary>
public sealed partial class GameEngine
{
    private ref readonly GameOptions _options => ref _matchDefinition.ReferenceToOptions;

    private readonly ContentRegistry _contentRegistry;
    private ref readonly ContentModeDefinition _modeDefinition => ref _matchDefinition.ReferenceToModeDefinition;
    private ref readonly IReadOnlyList<GeneralDefinition> _generalPool => ref _matchDefinition.ReferenceToGeneralPool;

    private readonly DeterministicRandom _random;
    private ref readonly int _playerCount => ref _matchDefinition.ReferenceToPlayerCount;
    private ref readonly List<CharacterState> _players => ref _matchParticipants.ReferenceToPlayers;
    private ref readonly List<GeneralDefinition> _availableGenerals => ref _matchSetup.ReferenceToAvailableGenerals;
    private ref readonly List<int> _selectionOrder => ref _matchSetup.ReferenceToSelectionOrder;

    private readonly CardZoneStore _cardZones;
    private ref readonly List<GameLogEntry> _log => ref _gameLog.ReferenceToLog;
    private ref readonly List<AiThoughtRecord> _aiThoughts => ref _aiDiagnostics.ReferenceToAiThoughts;
    private ref readonly List<AiGeneralThought> _aiGeneralThoughts => ref _aiDiagnostics.ReferenceToAiGeneralThoughts;
    private ref readonly List<CardMovementRecord> _cardMovements => ref _cardMovementJournal.ReferenceToCardMovements;

    private readonly ResolutionScheduler _resolutionScheduler;
    private readonly FrameStore _resolutionStack;
    private readonly MatchExecutionLoop _matchExecutionLoop;
    private ref readonly List<EventEnvelope> _events => ref _commandJournalStorage.ReferenceToEvents;
    private ref readonly List<IGameEvent> _pendingEvents => ref _commandJournalStorage.ReferenceToPendingEvents;
    private ref readonly List<ObserverFailure> _observerFailures => ref _commandJournalStorage.ReferenceToObserverFailures;
    private ref readonly Queue<EngineNotification> _pendingNotifications => ref _commandJournalStorage.ReferenceToPendingNotifications;
    private ref readonly Dictionary<int, SimpleAiBrain> _aiBrains => ref _aiBrains2.ReferenceToAiBrains;
    private ref readonly Dictionary<int, CardKind> _judgmentEffectiveCardKinds => ref _judgmentEffectiveCards.ReferenceToJudgmentEffectiveCardKinds;

    private readonly SkillRuntimeStateStore _skillRuntimeState = new();
    private readonly TurnCardUseEffectStore _turnCardUseEffects = new();
    private readonly MatchSkillBindingIndex _skillBindingIndex;
    private ref bool _pendingStatePublication => ref _commandJournalStorage.ReferenceToPendingStatePublication;
    private ref EngineStatus _status => ref _matchOutcome.ReferenceToStatus;
    private ref Winner _winner => ref _matchOutcome.ReferenceToWinner;
    private ref string? _winnerTeamId => ref _matchOutcome.ReferenceToWinnerTeamId;
    private ref string? _winnerFactionId => ref _matchOutcome.ReferenceToWinnerFactionId;
    private ref TurnPhase _phase => ref _actualTurnProgress.ReferenceToPhase;
    private ref int _turnNumber => ref _actualTurnProgress.ReferenceToTurnNumber;
    private ref int _currentSeat => ref _actualTurnProgress.ReferenceToCurrentSeat;
    private ref int _slashCountThisTurn => ref _currentTurnStatistics.ReferenceToSlashCountThisTurn;
    private ref int _playPhaseKillCountByCurrentPlayer => ref _currentTurnStatistics.ReferenceToPlayPhaseKillCountByCurrentPlayer;
    private ref ActualTurnProgression _turnProgression => ref _actualTurnProgress.ReferenceToTurnProgression;
    private ref int _playPhaseDamageDealtByCurrentPlayer => ref _currentTurnStatistics.ReferenceToPlayPhaseDamageDealtByCurrentPlayer;
    // Guzheng ledger: hand cards the current turn owner moved into the discard
    // pile during this turn's discard phase. Scoped to its owning turn number so
    // an interrupted turn can never serve a stale pool to the next boundary.
    private ref readonly List<int> _discardPhaseHandDiscardIds => ref _actualDiscardLedger.ReferenceToDiscardPhaseHandDiscardIds;
    private ref int _discardPhaseHandDiscardTurnNumber => ref _actualDiscardLedger.ReferenceToDiscardPhaseHandDiscardTurnNumber;
    private ref bool _usedOrPlayedSlashDuringPlayPhase => ref _currentTurnStatistics.ReferenceToUsedOrPlayedSlashDuringPlayPhase;
    private ref bool _woodenOxUsedThisTurn => ref _currentTurnStatistics.ReferenceToWoodenOxUsedThisTurn;
    private ref int _logSequence => ref _gameLog.ReferenceToLogSequence;
    private ref int _thoughtSequence => ref _aiDiagnostics.ReferenceToThoughtSequence;
    private ref int _movementSequence => ref _cardMovementJournal.ReferenceToMovementSequence;
    private ref long _eventSequence => ref _commandJournalStorage.ReferenceToEventSequence;
    private ref int _initialCardCount => ref _physicalCardPopulation.ReferenceToInitialCardCount;
    private ref int _initialHandSize => ref _matchDefinition.ReferenceToInitialHandSize;
    private ref int _drawPerTurn => ref _matchDefinition.ReferenceToDrawPerTurn;
    private ref long _nextPromptId => ref _playerDecision.ReferenceToNextPromptId;
    private ref int _selectionIndex => ref _matchSetup.ReferenceToSelectionIndex;
    private ref bool _started => ref _actualTurnProgress.ReferenceToStarted;
    private ref bool _setupComplete => ref _matchSetup.ReferenceToSetupComplete;
    private ref PendingDecision? _pendingDecisionBacking => ref _playerDecision.ReferenceToPendingDecisionBacking;
    private ref DelayedTurnEffects _pendingTurnDelayedEffects => ref _delayedTurnEffects.ReferenceToPendingTurnDelayedEffects;

    private readonly CardMovementBatchStore _cardMovementBatches;
    // Interim aliases for native batch metadata producers; storage belongs to the batch store.
    private readonly List<CardMovementBatchContext> _pendingCardsMovedBatches;
    private readonly Stack<long> _activeCardMovementBatchIds;
    private GameEngine(GameOptions options, ContentRegistry contentRegistry)
    {
        _cardMovementBatches = new();
        _pendingCardsMovedBatches = _cardMovementBatches.PendingBatches;
        _activeCardMovementBatchIds = _cardMovementBatches.ActiveBatchIds;
        _suppressedCardMovedEventBatchIds = _cardMovementBatches.SuppressedEventBatchIds;
        _resolutionScheduler = new(this.ComposeNativeResolutionLifecycle());
        _resolutionStack = _resolutionScheduler.Frames;
        _matchExecutionLoop = new(this.ComposeNativeMatchExecutionHost());
        options = EngineOwners.CoreGameEngineShanheCampaignsRuntime.FreezeShanheBattleOptions(options);
        if (options.HumanSeats is { } humans)
            options = options with
            {
                HumanSeats = Array.AsReadOnly(humans.ToArray())
            };
        ArgumentNullException.ThrowIfNull(contentRegistry);
        EngineOwners.CoreMatchSetupDefinitionsRuntime.ValidateOptions(options, contentRegistry);
        _matchDefinition.InitializeOptions(options);
        _contentRegistry = contentRegistry;
        _matchDefinition.InitializePlayerCount(options.PlayerCount);
        _cardZones = new CardZoneStore(_playerCount);
        _observedSkillGrantRevisions = _skillGrantRevisions.Revisions;
        _random = new DeterministicRandom(options.Seed);
        _matchDefinition.InitializeModeDefinition(EngineOwners.CoreMatchSetupDefinitionsRuntime.ResolveModeDefinition(contentRegistry, options));
        _modeRuntime = CreateMatchModeRuntime();
        _modeRules = (_modeRuntime.Find<IMatchModeRuleProvider>() ?? throw new InvalidOperationException("The mode requires its installed rule profile.")).Rules;
        _modeRuntime.EnsureRequiredFeatures(_modeRules.RequiredFeatureContracts);
        this.ProgramRuntimeHooksEntries.Value.ValidateModeRuntimeHookPriorities();
        _programCapabilities = MatchRuntime.Capabilities;
        _programRuleObservers = new ProgramRuleObservers(EngineOwners.CoreGameEngineRuleProgressionDefinitions.ProgramRuleObserverRegistrations.Value, BindRuntimeHandler, _modeDefinition.Id, _modeRuntime.RuleObservers);
        _programPipelineHooks = new ProgramPipelineHooks(EngineOwners.CoreGameEnginePipelineHooksDefinitions.ProgramPipelineHookRegistrations.Value, this);
        _programLegality = new(EngineOwners.CoreGameEngineSkillProgramsDefinitions.ProgramLegalityMetadata.Value, this, ResolveRuntimeHandlerTarget);
        _programSelections = new(EngineOwners.CoreProgramSelectionModulesDefinitions.ProgramSelectionMetadata.Value, ProgramSelectionQueries);
        _programSelectionValidators = new(EngineOwners.CoreGameEngineProgramSelectionValidationDefinitions.ProgramSelectionValidationMetadata.Value, ProgramSelectionQueries);
        _programTargetResolvers = new(EngineOwners.CoreGameEngineProgramTargetResolversDefinitions.ProgramTargetResolverMetadata.Value, ProgramSelectionQueries);
        _programActions = new(EngineOwners.CoreProgramActionModulesDefinitions.ProgramActionMetadata.Value, BindRuntimeHandler);
        _nativeReturns = new(EngineOwners.CoreProgramNativeReturnsDefinitions.NativeReturnMetadata.Value, BindRuntimeHandler);
        (_modeRuntime.Find<IMatchModeConfigurationValidator>() ?? throw new InvalidOperationException("The mode requires its installed configuration validator.")).ValidateOptions();
        _matchDefinition.InitializeGeneralPool(EngineOwners.CoreMatchParticipantSetupRuntime.CreateRuntimeGeneralPool(contentRegistry, _modeDefinition));
        _skillBindingIndex = new MatchSkillBindingIndex(this.ActivityOrdinaryTricksEntries.ActivityRuntimeSkillDefinition, this.NativeDamageHandlesEntries.Value.IsNationalWarMode, player => player.SkillGrants.HasEnabledSkill(contentRegistry.ProgramDependencies.HpSensitiveSkillIds), (owner, grant) => this.WushuangEntries.IsWushuangSkillGrantQualified(grant) && (grant.IsModeRule || !owner.IdentityConcealed && this.CurrentTurnSkillSuppressionsEntries.IsCurrentTurnSkillGrantQualified(owner, grant) && this.HuCheErMouEntries.MouHuCheErEquipmentGrantQualified(owner, grant) && this.GuanduGeneralsEntries.GuanduGeneralGrantQualified(owner, grant) && this.LordSkillProjectionsEntries.IsProjectedGrantQualified(owner, grant) && this.PrivateGeneralLibrariesEntries.IsGeneralLibraryGrantQualified(owner, grant) && this.HpConditionalSkillGrantsEntries.Value.IsHpConditionalGrantQualified(owner, grant) && this.HeQiOlEntries.IsEquipmentSuitGrantQualified(owner, grant) && this.DaQiaoXiaoQiaoOlEntries.IsDaQiaoXiaoQiaoLuoyanGrantQualified(owner, grant)), this.CurrentTurnSkillSuppressionsEntries.CaptureCurrentTurnQualificationStamp, derivedProjectionStamp: player => this.HuCheErMouEntries.CaptureMouHuCheErEquipmentQualificationStamp(this.DaQiaoXiaoQiaoOlEntries.CaptureDaQiaoXiaoQiaoLuoyanQualificationStamp(player, player.IdentityConcealed ? long.MinValue : player.SkillGrants.HasProjectedGrants ? this.PrivateGeneralLibrariesEntries.CaptureCombinedProjectionDependencyStamp() : 0)), dualGenerals: _modeDefinition.DualGenerals, additionalPrintedLordQualification: this.ScenarioPrintedLordQualificationsEntries.HasShanhePrintedLordQualifications ? this.ScenarioPrintedLordQualificationsEntries.IsShanhePrintedLordGrantQualified : null, participants: _matchParticipants, cards: _cardZones, phases: _programCapabilities.Get<IActualDiscardRecoveryPhaseQueries>(), frames: _programCapabilities.Get<IProgramRuntimeFrames>(), health: this.HealthQueriesEntries);
        var deckDefinition = EngineOwners.CoreMatchParticipantSetupRuntime.ResolveDeckDefinition(contentRegistry, options.DeckId ?? _modeDefinition.DeckId);
        _initialHandSize = deckDefinition.InitialHandSize;
        _drawPerTurn = deckDefinition.DrawPerTurn;
        this.MatchParticipantSetupEntries.SetupPlayers();
        var skillStateTemplates = SkillStateApplications.Compile(contentRegistry);
        foreach (var player in _players)
            player.SkillGrants.BindSkillStates(player.Seat, skillStateTemplates, _players);
        this.ShanheMarketEntries.InitializeShanheBattleMarket();
        if (this.PrivateGeneralLibrariesEntries.HasPrivateGeneralLibraryCapability)
            foreach (var player in _players)
                player.GeneralLibraryGenderQuery = this.PrivateGeneralLibrariesEntries.GetPrivateGeneralLibraryGender;
        this.MatchParticipantSetupEntries.SetupDeck();
        _currentSeat = this.MatchParticipantSetupEntries.GetStartingSeat();
        if (!options.UseInteractiveSetup)
        {
            this.MatchParticipantSetupEntries.AssignAutomaticGodFactions();
            this.NationalSkillsEntries.InitializeDualGeneralHealth();
            foreach (var player in _players)
                this.DaQiaoXiaoQiaoHappyGodEntries.BindHappyQiaoHealth(player);
            this.MatchParticipantSetupEntries.DealInitialHands();
            foreach (var player in _players)
                this.IdentityConcealmentEntries.InitializeSelectedIdentityConcealment(player);
            _setupComplete = !options.ConfirmOpeningHand || options.HumanSeat < 0;
            this.ConversionStatesEntries.InitializeStructuredConversionSkills();
        }

        foreach (var player in _players.Where(player => !player.IsHuman))
        {
            _aiBrains[player.Seat] = new SimpleAiBrain(player.Seat, unchecked(options.Seed * 397) ^ (player.Seat + 1), options.AiPolicyVersion);
        }
    }

    public static GameEngine CreateStandard(GameOptions? options, ContentRegistry contentRegistry) => new(options ?? new GameOptions(), contentRegistry);
    /// <summary>The deterministic match seed for local diagnostics and replay labels.</summary>
    public int Seed => GetRuntimeModule<EngineOwners.CoreMatchDefinitionQueriesRuntime>().Seed;
    /// <summary>
    /// Monotonically increasing public state revision. Player commands must carry
    /// the revision they observed so stale decisions cannot overwrite newer state.
    /// </summary>
    public long Revision => GetRuntimeModule<EngineOwners.CoreCommandSessionQueriesRuntime>().Revision;
    /// <summary>
    /// The immutable content registry used by this match.
    /// </summary>
    public ContentRegistry ContentRegistry => GetRuntimeModule<EngineOwners.CoreMatchDefinitionQueriesRuntime>().ContentRegistry;
    /// <summary>The player count selected by the identity mode adapter.</summary>
    public int PlayerCount => GetRuntimeModule<EngineOwners.CoreMatchDefinitionQueriesRuntime>().PlayerCount;

    /// <summary>
    /// Returns the shortest distance between two seats on the table ring. This
    /// first rules query intentionally ignores horses and dead-seat removal.
    /// </summary>
    public int GetSeatDistance(int sourceSeat, int targetSeat) => GetRuntimeModule<EngineOwners.CoreNativeRuleQueriesRuntime>().GetSeatDistance(sourceSeat, targetSeat);
    /// <summary>
    /// Returns the public combat distance after removing dead seats and applying
    /// the source's offensive horse and target's defensive horse. The raw seat
    /// ring query above remains available for compatibility and diagnostics.
    /// </summary>
    public int GetCombatDistance(int sourceSeat, int targetSeat) => GetRuntimeModule<EngineOwners.CoreNativeRuleQueriesRuntime>().GetCombatDistance(sourceSeat, targetSeat);
    /// <summary>
    /// Returns the current public attack range. Formal weapon definitions use
    /// their printed range from rules v13; older replays retain bonus semantics.
    /// </summary>
    public int GetAttackRange(int sourceSeat) => GetRuntimeModule<EngineOwners.CoreNativeRuleQueriesRuntime>().GetAttackRange(sourceSeat);
    /// <summary>The namespaced mode definition selected for this match.</summary>
    public string ModeId => GetRuntimeModule<EngineOwners.CoreMatchDefinitionQueriesRuntime>().ModeId;
    /// <summary>A snapshot filtered for the configured human seat.</summary>
    public GameSnapshot State => GetRuntimeModule<EngineOwners.CorePlayerViewProjectionRuntime>().State;
    public PendingDecision? PendingDecision => GetRuntimeModule<EngineOwners.CorePlayerDecisionProjectionRuntime>().PendingDecision;
    public IReadOnlyList<GameLogEntry> Log => GetRuntimeModule<EngineOwners.CorePublicJournalProjectionRuntime>().Log;
    public IReadOnlyList<AiThoughtRecord> AiThoughts => GetRuntimeModule<EngineOwners.CoreAiDiagnosticProjectionRuntime>().AiThoughts;
    /// <summary>Trusted-host diagnostics for private AI general selection.</summary>
    public IReadOnlyList<AiGeneralThought> AiGeneralThoughts => GetRuntimeModule<EngineOwners.CoreAiDiagnosticProjectionRuntime>().AiGeneralThoughts;
    /// <summary>Trusted-host movement ledger. Do not include it in a player network payload.</summary>
    public IReadOnlyList<CardMovementRecord> CardMovements => GetRuntimeModule<EngineOwners.CoreTrustedMovementDiagnosticsRuntime>().CardMovements;
    /// <summary>Observer callback failures captured after committed operations.</summary>
    public IReadOnlyList<ObserverFailure> ObserverFailures => GetRuntimeModule<EngineOwners.CoreCommandSessionQueriesRuntime>().ObserverFailures;
    /// <summary>Trusted-host typed event stream; do not send it as a player view.</summary>
    public IReadOnlyList<EventEnvelope> Events => GetRuntimeModule<EngineOwners.CorePublicJournalProjectionRuntime>().Events;
    /// <summary>
    /// Accepted trusted-host command journal for deterministic local replay.
    /// Rejected commands and legacy adapter calls are intentionally omitted.
    /// Never include this journal in a player network payload.
    /// </summary>
    public IReadOnlyList<GameCommand> AcceptedCommands => GetRuntimeModule<EngineOwners.CoreCommandSessionQueriesRuntime>().AcceptedCommands;

    /// <summary>
    /// Captures a trusted-host checkpoint for a state reached entirely through
    /// the command boundary. Legacy compatibility calls are intentionally not
    /// inferred as commands, so a mixed or legacy-driven state fails loudly.
    /// </summary>
    public GameCheckpoint CreateCheckpoint() => GetRuntimeModule<EngineOwners.CoreCommandSessionQueriesRuntime>().CreateCheckpoint();
    /// <summary>
    /// Trusted-host view of the in-flight data-only resolution stack. It is not
    /// included in ordinary player snapshots.
    /// </summary>
    public IReadOnlyList<ResolutionFrame> ResolutionStack => GetRuntimeModule<EngineOwners.CoreTrustedResolutionDiagnosticsRuntime>().ResolutionStack;

    public event Action<GameSnapshot>? StateChanged;
    public event Action<GameLogEntry>? LogAdded;
    public event Action<AiThoughtRecord>? AiThoughtAdded;
    public event Action<AiGeneralThought>? AiGeneralThoughtAdded;
    public event Action<CardMovementRecord>? CardMoved;
    /// <summary>Raised after a typed event batch has been committed.</summary>
    public event Action<EventEnvelope>? EventCommitted;
    /// <summary>
    /// Submits one data-only command through the exclusive game boundary.
    /// Player-input errors are typed results, not exceptions.
    /// </summary>
    public CommandResult Submit(GameCommand? command)
    {
        if (this.CommandSessionQueriesEntries.Value._isExecutingPublicOperation)
        {
            return Reject(CommandErrorCode.ReentrantOperation, "GameEngine is dispatching a committed operation; submit again after the callback returns.");
        }

        EnsureSessionHealthy();
        if (command is null)
        {
            return Reject(CommandErrorCode.NullCommand, "A command is required.");
        }

        if (command.ExpectedRevision != _revision)
        {
            return Reject(CommandErrorCode.StaleRevision, $"The command expects revision {command.ExpectedRevision}, but the current revision is {_revision}.");
        }

        if (command is DiscardCardsCommand discardInput)
        {
            command = discardInput with
            {
                CardIds = discardInput.CardIds?.ToArray() ?? []
            };
        }
        else if (command is PlayCardCommand playInput)
        {
            command = playInput with
            {
                TargetSeats = playInput.TargetSeats?.ToArray() ?? [],
                AdditionalConversionSources = playInput.AdditionalConversionSources is { } sources ? Array.AsReadOnly(sources.ToArray()) : null
            };
        }
        else if (command is UseEquipmentEffectCommand equipmentInput)
        {
            command = equipmentInput with
            {
                CardIds = equipmentInput.CardIds?.ToArray() ?? [],
                TargetSeats = equipmentInput.TargetSeats?.ToArray() ?? []
            };
        }
        else if (command is UseProgramSkillCommand programInput)
        {
            command = programInput with
            {
                CardIds = programInput.CardIds?.ToArray() ?? [],
                TargetSeats = programInput.TargetSeats?.ToArray() ?? []
            };
        }

        _commandAwaitingCommit = EngineOwners.CoreSnapshotValueProjectionRuntime.CloneCommand(command);
        try
        {
            if (_modeRuntime.TrySubmit(command, out var modeResult))
                return modeResult!;
            return command switch
            {
                StartGameCommand start => this.CommandInputAdaptersEntries.SubmitStart(start),
                AdvanceCommand advance => this.CommandInputAdaptersEntries.SubmitAdvance(advance, singleStep: false),
                AdvanceOneStepCommand step => this.CommandInputAdaptersEntries.SubmitAdvance(step, singleStep: true),
                SelectGeneralCommand selectGeneral => this.CommandInputAdaptersEntries.SubmitSelectGeneral(selectGeneral),
                RequestGeneralRosterCommand roster => this.OpeningCustomizationEntries.Value.SubmitGeneralRoster(roster),
                PlayCardCommand play => this.CommandInputAdaptersEntries.SubmitPlayCard(play),
                RecastCardCommand recast => this.CardActionsEntries.SubmitRecast(recast),
                UseProgramSkillCommand program => this.SkillProgramsEntries.SubmitUseProgramSkill(program),
                UseEquipmentEffectCommand equipment => this.CommandInputAdaptersEntries.SubmitUseEquipmentEffect(equipment),
                BuyShanheMarketItemCommand market => this.ShanheMarketEntries.SubmitBuyShanheMarketItem(market),
                RefreshShanheMarketCommand refreshMarket => this.ShanheMarketEntries.SubmitRefreshShanheMarket(refreshMarket),
                EndPlayPhaseCommand end => this.CommandInputAdaptersEntries.SubmitEndPlay(end),
                DiscardCardsCommand discard => this.DiscardEntries.SubmitDiscardCards(discard),
                AnswerPromptCommand answer => this.CommandInputAdaptersEntries.SubmitPromptAnswer(answer.ActorSeat, answer.Prompt, answer.Choice),
                _ => Reject(CommandErrorCode.UnsupportedCommand, "The command type is not supported by this engine.")};
        }
        finally
        {
            _commandAwaitingCommit = null;
        }
    }

    private CommandResult Accept(Action operation)
    {
        var result = ExecuteExclusive(() =>
        {
            try
            {
                operation();
            }
            catch (ModeFlowInterruption interruption)
            {
                _modeRuntime.CompleteFlowInterruption(interruption);
                if (_commandAwaitingCommit is AdvanceCommand || _options.AdvanceAfterHumanCommands)
                    AdvanceToHumanBoundary();
            }

            _modeRuntime.TryCompleteMatch(ModeCompletionBoundary.AfterCommand);
            if (_winner != Winner.None && _status != EngineStatus.Completed && this.PendingDecisionPolicyPipelineEntries._pendingDecision is null && _resolutionStack.Count == 0)
            {
                CompleteGame();
            }
        });
        return new CommandResult(true, null, _revision, result);
    }

    private CommandResult Reject(CommandErrorCode code, string message) => new(false, new CommandError(code, message), _revision, _commandSession.ResultBeingDelivered ?? this.CommandResultProjectionEntries.Value.BuildResult());
    public IReadOnlyList<LegalAction> GetLegalActions() => GetRuntimeModule<EngineOwners.CoreNativeLegalActionsRuntime>().GetLegalActions();
    public IReadOnlyList<LegalAction> GetHumanLegalActions() => GetRuntimeModule<EngineOwners.CoreNativeLegalActionsRuntime>().GetHumanLegalActions();
    /// <summary>
    /// Creates a viewer-safe snapshot. Pass revealAll only for diagnostics/tests or
    /// an explicit post-game reveal screen.
    /// </summary>
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => GetRuntimeModule<EngineOwners.CorePlayerViewProjectionRuntime>().CreateSnapshot(viewerSeat, revealAll);
    /// <summary>
    /// Returns every physical card and its exact location for trusted diagnostics,
    /// invariant tests, and future replay tooling. This is intentionally not a player view.
    /// </summary>
    public IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics() => GetRuntimeModule<EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime>().CreateCardZoneDiagnostics();
    public string SerializeState(bool revealAll = false) => GetRuntimeModule<EngineOwners.CorePlayerViewProjectionRuntime>().SerializeState(revealAll);
    private void CompleteGame()
    {
        this.SoleTargetQuarantineEntries.SettleGameEndedSoleTargetQuarantines();
        foreach (var player in _players)
        {
            player.RoleRevealed = true;
            player.TeamRevealed = this.NativeDamageHandlesEntries.Value.IsTeamMode;
            if (this.NativeDamageHandlesEntries.Value.IsNationalWarMode)
            {
                this.NationalGeneralRevealEntries.RevealNationalInformation(player);
            }
        }

        _phase = TurnPhase.Finished;
        _status = EngineStatus.Completed;
        this.PendingDecisionPolicyPipelineEntries._pendingDecision = null;
        _pendingCardsMovedBatches.Clear();
        _pendingHpChanges.Clear();
        this.MatchLogEntries.AddLog("GameEnded", (_modeRuntime.Find<IMatchModePresentation>() ?? throw new InvalidOperationException("The mode requires its installed presentation feature.")).WinnerLog(this.ModePresentationEntries.Value.CaptureModeWinnerPresentationContext()));
        AdvanceEventRulesAndQueueFact(new GameEndedEvent(_winner, this.NativeDamageHandlesEntries.Value.IsTeamMode ? _winnerTeamId : null, this.NativeDamageHandlesEntries.Value.IsNationalWarMode ? _winnerFactionId : null) { WinnerSeat = _winnerSeat });
    }

    private void PublishState()
    {
        _pendingStatePublication = true;
    }

    private void QueueGameEvent(IGameEvent payload) => PendingEventOutput.Queue(payload);
    private readonly CardSnapshotCache _cardSnapshotCache = new();
    [Flags]
    internal enum DelayedTurnEffects
    {
        None = 0,
        SkipDrawPhase = 1,
        SkipPlayPhase = 2,
        SkipJudgmentPhase = 4,
        SkipDiscardPhase = 8
    }

    internal enum TargetCardEffect
    {
        Discard,
        Take
    }

    internal sealed record DyingCompletionReceipt(long FrameId, long ParentFrameId, int VictimSeat, DyingContinuationKind Continuation);
    internal readonly record struct JudgmentCompletionReceipt(JudgmentFrame Frame, bool Succeeded);
}
