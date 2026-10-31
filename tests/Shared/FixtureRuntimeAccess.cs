#nullable enable
using System.Globalization;
using System.Reflection;
using CardGame.Core;

// Test-only reflection routing. This stores metadata, never a match owner.
internal static class FixtureRuntimeAccess
{
    internal static Type Engine { get; } = new EngineType();
    internal static string OriginalName(MethodInfo method) => method is RoutedMethod routed ? routed.OriginalName : method.Name;
    private const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    private sealed record Route(string Name, string Kind, string Owner, string Target, string Proxy, string Entry, string Composition, string Signature, bool Static);
    private static readonly Route[] Routes = [
new Route("AdvanceOneStepCore", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime", "AdvanceOneStepCore", "CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries", "AdvanceOneStepCore", "", "M:CardGame.Core.GameEngine.AdvanceOneStepCore", false),
new Route("StartCore", "method", "CardGame.Core.EngineOwners.CoreMatchSetupRuntime", "StartCore", "CardGame.Core.RuntimeEntries.CoreMatchSetupEntries", "StartCore", "", "M:CardGame.Core.GameEngine.StartCore", false),
new Route("FlushNotifications", "method", "CardGame.Core.EngineOwners.CoreMatchLogRuntime", "FlushNotifications", "CardGame.Core.RuntimeEntries.CoreMatchLogEntries", "FlushNotifications", "", "M:CardGame.Core.GameEngine.FlushNotifications(CardGame.Core.GameSnapshot)", false),
new Route("EvaluateSlashUseLimit", "method", "CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime", "EvaluateSlashUseLimit", "CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries", "EvaluateSlashUseLimit", "", "M:CardGame.Core.GameEngine.EvaluateSlashUseLimit(CardGame.Core.CharacterState,System.Nullable{System.Int32},System.Nullable{CardGame.Core.EquipmentSlot})", false),
new Route("_thoughtSequence", "field", "CardGame.Core.EngineStates.CoreAiDiagnosticsState", "_thoughtSequence", "", "", "_aiDiagnostics", "F:CardGame.Core.GameEngine._thoughtSequence", false),
new Route("_aiThoughts", "field", "CardGame.Core.EngineStates.CoreAiDiagnosticsState", "_aiThoughts", "", "", "_aiDiagnostics", "F:CardGame.Core.GameEngine._aiThoughts", false),
new Route("_actionQuery", "field", "CardGame.Core.EngineStates.CoreGameEngineActionQueriesState", "_actionQuery", "", "", "_actionQueries", "F:CardGame.Core.GameEngine._actionQuery", false),
new Route("_aiBrains", "field", "CardGame.Core.EngineStates.CoreAiBrainsState", "_aiBrains", "", "", "_aiBrains2", "F:CardGame.Core.GameEngine._aiBrains", false),
new Route("_beneficiarySuitShields", "field", "CardGame.Core.EngineStates.CoreGameEngineDeckEquipmentProgramsState", "_beneficiarySuitShields", "", "", "_deckEquipmentPrograms", "F:CardGame.Core.GameEngine._beneficiarySuitShields", false),
new Route("_cardMovements", "field", "CardGame.Core.EngineStates.CoreCardMovementJournalState", "_cardMovements", "", "", "_cardMovementJournal", "F:CardGame.Core.GameEngine._cardMovements", false),
new Route("_cardUseDebitPhaseInstanceId", "field", "CardGame.Core.EngineStates.CoreGameEngineCardUseDebitsState", "_cardUseDebitPhaseInstanceId", "", "", "_cardUseDebits2", "F:CardGame.Core.GameEngine._cardUseDebitPhaseInstanceId", false),
new Route("_currentSeat", "field", "CardGame.Core.EngineStates.CoreActualTurnProgressState", "_currentSeat", "", "", "_actualTurnProgress", "F:CardGame.Core.GameEngine._currentSeat", false),
new Route("_currentTurnSkillSuppressions", "field", "CardGame.Core.EngineStates.CoreGameEngineCurrentTurnSkillSuppressionsState", "_currentTurnSkillSuppressions", "", "", "_currentTurnSkillSuppressions2", "F:CardGame.Core.GameEngine._currentTurnSkillSuppressions", false),
new Route("_deferredHandAlignments", "field", "CardGame.Core.EngineStates.CoreGameEngineDeferredHandAlignmentsState", "_deferredHandAlignments", "", "", "_deferredHandAlignments2", "F:CardGame.Core.GameEngine._deferredHandAlignments", false),
new Route("_deferredPublicPileDeposits", "field", "CardGame.Core.EngineStates.CoreGameEngineDeferredCardsState", "_deferredPublicPileDeposits", "", "", "_deferredCards", "F:CardGame.Core.GameEngine._deferredPublicPileDeposits", false),
new Route("_directedTurnCardPolicies", "field", "CardGame.Core.EngineStates.CoreDirectedTurnCardPoliciesState", "_directedTurnCardPolicies", "", "", "_directedTurnCardPolicies2", "F:CardGame.Core.GameEngine._directedTurnCardPolicies", false),
new Route("_events", "field", "CardGame.Core.EngineStates.CoreCommandJournalStorageState", "_events", "", "", "_commandJournalStorage", "F:CardGame.Core.GameEngine._events", false),
new Route("_generalPool", "field", "CardGame.Core.EngineStates.CoreMatchDefinitionState", "_generalPool", "", "", "_matchDefinition", "F:CardGame.Core.GameEngine._generalPool", false),
new Route("_generatedPhysicalCardIds", "field", "CardGame.Core.EngineStates.CoreGameEngineAdvancedWeaponsState", "_generatedPhysicalCardIds", "", "", "_advancedWeapons", "F:CardGame.Core.GameEngine._generatedPhysicalCardIds", false),
new Route("_initialCardCount", "field", "CardGame.Core.EngineStates.CorePhysicalCardPopulationState", "_initialCardCount", "", "", "_physicalCardPopulation", "F:CardGame.Core.GameEngine._initialCardCount", false),
new Route("_issuedPlayPhaseUseProhibitions", "field", "CardGame.Core.EngineStates.CoreGameEngineIssuedPlayUsePoliciesState", "_issuedPlayPhaseUseProhibitions", "", "", "_issuedPlayUsePolicies", "F:CardGame.Core.GameEngine._issuedPlayPhaseUseProhibitions", false),
new Route("_judgmentEffectiveCardKinds", "field", "CardGame.Core.EngineStates.CoreJudgmentEffectiveCardsState", "_judgmentEffectiveCardKinds", "", "", "_judgmentEffectiveCards", "F:CardGame.Core.GameEngine._judgmentEffectiveCardKinds", false),
new Route("_movementSequence", "field", "CardGame.Core.EngineStates.CoreCardMovementJournalState", "_movementSequence", "", "", "_cardMovementJournal", "F:CardGame.Core.GameEngine._movementSequence", false),
new Route("_pendingDecisionBacking", "field", "CardGame.Core.EngineStates.CorePlayerDecisionState", "_pendingDecisionBacking", "", "", "_playerDecision", "F:CardGame.Core.GameEngine._pendingDecisionBacking", false),
new Route("_pendingEvents", "field", "CardGame.Core.EngineStates.CoreCommandJournalStorageState", "_pendingEvents", "", "", "_commandJournalStorage", "F:CardGame.Core.GameEngine._pendingEvents", false),
new Route("_pendingHpChanges", "field", "CardGame.Core.EngineStates.CoreGameEngineHpChangeProgramsState", "_pendingHpChanges", "", "", "_hpChangePrograms", "F:CardGame.Core.GameEngine._pendingHpChanges", false),
new Route("_phase", "field", "CardGame.Core.EngineStates.CoreActualTurnProgressState", "_phase", "", "", "_actualTurnProgress", "F:CardGame.Core.GameEngine._phase", false),
new Route("_players", "field", "CardGame.Core.EngineStates.CoreMatchParticipantsState", "_players", "", "", "_matchParticipants", "F:CardGame.Core.GameEngine._players", false),
new Route("_programBooleanStates", "field", "CardGame.Core.EngineStates.CoreGameEngineProgramPersistentStateState", "_programBooleanStates", "", "", "_programPersistentState", "F:CardGame.Core.GameEngine._programBooleanStates", false),
new Route("_programPlayDamageHandLimits", "field", "CardGame.Core.EngineStates.CoreGameEngineProgramPersistentCombatPoliciesState", "_programPlayDamageHandLimits", "", "", "_programPersistentCombatPolicies", "F:CardGame.Core.GameEngine._programPlayDamageHandLimits", false),
new Route("_publicPersistentPiles", "field", "CardGame.Core.EngineStates.CoreGameEnginePublicPileSourcesState", "_publicPersistentPiles", "", "", "_publicPileSources", "F:CardGame.Core.GameEngine._publicPersistentPiles", false),
new Route("_roundNumber", "field", "CardGame.Core.EngineStates.CoreActualRoundProgressState", "_roundNumber", "", "", "_actualRoundProgress", "F:CardGame.Core.GameEngine._roundNumber", false),
new Route("_status", "field", "CardGame.Core.EngineStates.CoreMatchOutcomeState", "_status", "", "", "_matchOutcome", "F:CardGame.Core.GameEngine._status", false),
new Route("_turnNumber", "field", "CardGame.Core.EngineStates.CoreActualTurnProgressState", "_turnNumber", "", "", "_actualTurnProgress", "F:CardGame.Core.GameEngine._turnNumber", false),
new Route("_turnProgression", "field", "CardGame.Core.EngineStates.CoreActualTurnProgressState", "_turnProgression", "", "", "_actualTurnProgress", "F:CardGame.Core.GameEngine._turnProgression", false),
new Route("ActualHandGainEntities", "method", "CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime", "ActualHandGainEntities", "CardGame.Core.RuntimeEntries.CoreGameEngineActualHandGainProgramsEntries", "ActualHandGainEntities", "", "M:CardGame.Core.GameEngine.ActualHandGainEntities(CardGame.Core.CardMovementBatchContext,System.Int32)", false),
new Route("AdvanceRuntimeProgram", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime", "AdvanceRuntimeProgram", "CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries", "AdvanceRuntimeProgram", "", "M:CardGame.Core.GameEngine.AdvanceRuntimeProgram(System.Int64)", false),
new Route("AnniversaryJichaoUnlocked", "method", "CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime", "AnniversaryJichaoUnlocked", "CardGame.Core.RuntimeEntries.CharacterSunQuanAnniversaryGodEntries", "AnniversaryJichaoUnlocked", "", "M:CardGame.Core.GameEngine.AnniversaryJichaoUnlocked(System.Int32,System.String)", false),
new Route("AvailableDeclarationCard", "method", "CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime", "AvailableDeclarationCard", "CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries", "AvailableDeclarationCard", "", "M:CardGame.Core.GameEngine.AvailableDeclarationCard(CardGame.Core.CharacterState)", false),
new Route("AvailableEffectiveFactionIds", "method", "CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime", "AvailableEffectiveFactionIds", "CardGame.Core.RuntimeEntries.CoreGameEngineNativeFactionChangesEntries", "AvailableEffectiveFactionIds", "", "M:CardGame.Core.GameEngine.AvailableEffectiveFactionIds", false),
new Route("BeginActionQuery", "method", "CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime", "BeginActionQuery", "CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries", "BeginActionQuery", "", "M:CardGame.Core.GameEngine.BeginActionQuery", false),
new Route("BeginJudgment", "method", "CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime", "BeginJudgment", "CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries", "BeginJudgment", "", "M:CardGame.Core.GameEngine.BeginJudgment(CardGame.Core.NativeOwners.CardAttackHandle,System.Int32,System.String,System.Int64,System.Nullable{CardGame.Core.CardKind},CardGame.Core.JudgmentContinuationKind,CardGame.Core.Card,System.Nullable{System.Int32},System.String,System.Nullable{CardGame.Core.SkillProgramCardSetVisibility},CardGame.Core.OriginalUseLightningAttachment,System.Nullable{System.Int32})", false),
new Route("BuildLegalActions", "method", "CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime", "BuildLegalActions", "CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries", "BuildLegalActions", "", "M:CardGame.Core.GameEngine.BuildLegalActions(CardGame.Core.CharacterState,System.Boolean,System.Nullable{System.Int32})", false),
new Route("CalculateSlashUseLimit", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime", "CalculateSlashUseLimit", "CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries", "CalculateSlashUseLimit", "", "M:CardGame.Core.GameEngine.CalculateSlashUseLimit(CardGame.Core.CharacterState,System.Nullable{System.Int32},System.Nullable{CardGame.Core.EquipmentSlot})", false),
new Route("CaptureDyingSuitsOriginalCursor", "method", "CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime", "CaptureDyingSuitsOriginalCursor", "CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries", "CaptureDyingSuitsOriginalCursor", "", "M:CardGame.Core.GameEngine.CaptureDyingSuitsOriginalCursor(CardGame.Core.DyingFrame,CardGame.Core.ProgramLifecycleTriggerWindowFrame)", false),
new Route("CaptureProgramTriggerFacts", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime", "CaptureProgramTriggerFacts", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries", "CaptureProgramTriggerFacts", "", "M:CardGame.Core.GameEngine.CaptureProgramTriggerFacts(CardGame.Core.CharacterState)", false),
new Route("CaptureProgramTriggerFacts", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime", "CaptureProgramTriggerFacts", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries", "CaptureProgramTriggerFacts2", "", "M:CardGame.Core.GameEngine.CaptureProgramTriggerFacts(CardGame.Core.CharacterState,CardGame.Core.CardActionContext)", false),
new Route("CapturedDeclarationOrigin", "method", "CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime", "CapturedDeclarationOrigin", "CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries", "CapturedDeclarationOrigin", "", "M:CardGame.Core.GameEngine.CapturedDeclarationOrigin(System.Int32,CardGame.Core.CardLocation)", false),
new Route("CapturedWholeActualTurnActorUseOrdinal", "method", "CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime", "CapturedWholeActualTurnActorUseOrdinal", "CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries", "CapturedWholeActualTurnActorUseOrdinal", "", "M:CardGame.Core.GameEngine.CapturedWholeActualTurnActorUseOrdinal(CardGame.Core.CardActionContext)", false),
new Route("CardUsePaymentMaterialsMatch", "method", "CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime", "CardUsePaymentMaterialsMatch", "CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries", "CardUsePaymentMaterialsMatch", "", "M:CardGame.Core.GameEngine.CardUsePaymentMaterialsMatch(CardGame.Core.CardUseFrame,CardGame.Core.CardMovementBatchContext)", false),
new Route("CreateAiSkillContext", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime", "CreateAiSkillContext", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicDamagePredictionEntries", "CreateAiSkillContext", "", "M:CardGame.Core.GameEngine.CreateAiSkillContext(CardGame.Core.CharacterState,System.Int32,CardGame.Core.GameSnapshot)", false),
new Route("CreateDeckFromRegistry", "method", "CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime", "CreateDeckFromRegistry", "", "", "", "M:CardGame.Core.GameEngine.CreateDeckFromRegistry(CardGame.Core.ContentRegistry,CardGame.Core.ContentDeckRecipe)", true),
new Route("CreateProgramAiPublicContext", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime", "CreateProgramAiPublicContext", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries", "CreateProgramAiPublicContext", "", "M:CardGame.Core.GameEngine.CreateProgramAiPublicContext(CardGame.Core.CharacterState)", false),
new Route("CreateProgramAiPublicContext", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime", "CreateProgramAiPublicContext", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries", "CreateProgramAiPublicContext2", "", "M:CardGame.Core.GameEngine.CreateProgramAiPublicContext(CardGame.Core.SkillProgramTrigger,CardGame.Core.CharacterState,System.Int32)", false),
new Route("CreateProgramAwareSkillStateSnapshot", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramStateProjectionRuntime", "CreateProgramAwareSkillStateSnapshot", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramStateProjectionEntries", "CreateProgramAwareSkillStateSnapshot", "", "M:CardGame.Core.GameEngine.CreateProgramAwareSkillStateSnapshot(CardGame.Core.CharacterState,System.String)", false),
new Route("CreatePublicPersistentPileSnapshots", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime", "CreatePublicPersistentPileSnapshots", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries", "CreatePublicPersistentPileSnapshots", "", "M:CardGame.Core.GameEngine.CreatePublicPersistentPileSnapshots(System.Int32)", false),
new Route("CuiYanDamageTargets", "method", "CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime", "CuiYanDamageTargets", "CardGame.Core.RuntimeEntries.CharacterCuiYanOlEntries", "CuiYanDamageTargets", "", "M:CardGame.Core.GameEngine.CuiYanDamageTargets(System.Int32)", false),
new Route("DyingSuitsCursorHash", "method", "CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime", "DyingSuitsCursorHash", "", "", "", "M:CardGame.Core.GameEngine.DyingSuitsCursorHash``1(``0)", true),
new Route("EnabledContentSkillIds", "method", "CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime", "EnabledContentSkillIds", "CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries", "EnabledContentSkillIds", "", "M:CardGame.Core.GameEngine.EnabledContentSkillIds(CardGame.Core.CharacterState)", false),
new Route("EvaluateDrawCount", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime", "EvaluateDrawCount", "CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries", "EvaluateDrawCount", "", "M:CardGame.Core.GameEngine.EvaluateDrawCount(CardGame.Core.CharacterState)", false),
new Route("EvaluateHandLimit", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime", "EvaluateHandLimit", "CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries", "EvaluateHandLimit", "", "M:CardGame.Core.GameEngine.EvaluateHandLimit(CardGame.Core.CharacterState,System.Boolean)", false),
new Route("EvaluateProgramActivationCondition", "method", "CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime", "EvaluateProgramActivationCondition", "CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries", "EvaluateProgramActivationCondition", "", "M:CardGame.Core.GameEngine.EvaluateProgramActivationCondition(CardGame.Core.SkillProgramCondition,CardGame.Core.PlayerSkillContext,System.String,System.String)", false),
new Route("ExpireCurrentTurnNonLockedSkillSuppressions", "method", "CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime", "ExpireCurrentTurnNonLockedSkillSuppressions", "CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries", "ExpireCurrentTurnNonLockedSkillSuppressions", "", "M:CardGame.Core.GameEngine.ExpireCurrentTurnNonLockedSkillSuppressions(System.Int32,System.Int32)", false),
new Route("FindOwnedPlayableCard", "method", "CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime", "FindOwnedPlayableCard", "CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries", "FindOwnedPlayableCard", "", "M:CardGame.Core.GameEngine.FindOwnedPlayableCard(CardGame.Core.CharacterState,System.Nullable{System.Int32})", false),
new Route("GetDiscardEligibleHand", "method", "CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime", "GetDiscardEligibleHand", "CardGame.Core.RuntimeEntries.CoreGameEngineDeferredCardsEntries", "GetDiscardEligibleHand", "", "M:CardGame.Core.GameEngine.GetDiscardEligibleHand(CardGame.Core.CharacterState)", false),
new Route("GetEffectiveFactionId", "method", "CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime", "GetEffectiveFactionId", "CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries", "GetEffectiveFactionId", "", "M:CardGame.Core.GameEngine.GetEffectiveFactionId(CardGame.Core.CharacterState)", false),
new Route("GetHandComparisonPublicRuleStates", "method", "CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime", "GetHandComparisonPublicRuleStates", "CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries", "GetHandComparisonPublicRuleStates", "", "M:CardGame.Core.GameEngine.GetHandComparisonPublicRuleStates(CardGame.Core.CharacterState,System.String)", false),
new Route("GetHandLimit", "method", "CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime", "GetHandLimit", "CardGame.Core.RuntimeEntries.CoreNativeRuleQueriesEntries", "GetHandLimit", "", "M:CardGame.Core.GameEngine.GetHandLimit(CardGame.Core.CharacterState)", false),
new Route("GetProgramBooleanState", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime", "GetProgramBooleanState", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries", "GetProgramBooleanState", "", "M:CardGame.Core.GameEngine.GetProgramBooleanState(CardGame.Core.ProgramSkillFrame,System.String)", false),
new Route("GetProgramBooleanState", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime", "GetProgramBooleanState", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries", "GetProgramBooleanState2", "", "M:CardGame.Core.GameEngine.GetProgramBooleanState(System.Int32,System.String,System.String,System.String)", false),
new Route("GetProgramTargetSeats", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime", "GetProgramTargetSeats", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries", "GetProgramTargetSeats", "", "M:CardGame.Core.GameEngine.GetProgramTargetSeats(System.Int32,CardGame.Core.SkillProgramTargetKind,CardGame.Core.ProgramSkillWindowContext,System.Nullable{CardGame.Core.PlayerMarkerKind},CardGame.Core.ProgramParticipantReference)", false),
new Route("GetPublicLivingFactionCount", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicFactionCountsRuntime", "GetPublicLivingFactionCount", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicFactionCountsEntries", "GetPublicLivingFactionCount", "", "M:CardGame.Core.GameEngine.GetPublicLivingFactionCount", false),
new Route("GetSkillBindingShard", "method", "CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime", "GetSkillBindingShard", "CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries", "GetSkillBindingShard", "", "M:CardGame.Core.GameEngine.GetSkillBindingShard(CardGame.Core.CharacterState)", false),
new Route("GetSlashUseLimit", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime", "GetSlashUseLimit", "CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries", "GetSlashUseLimit", "", "M:CardGame.Core.GameEngine.GetSlashUseLimit(CardGame.Core.CharacterState,System.Nullable{System.Int32},System.Nullable{CardGame.Core.EquipmentSlot})", false),
new Route("GrantProgramArmor", "method", "CardGame.Core.EngineOwners.CoreProgramArmorOperationsRuntime", "GrantProgramArmor", "CardGame.Core.RuntimeEntries.CoreProgramArmorOperationsEntries", "GrantProgramArmor", "", "M:CardGame.Core.GameEngine.GrantProgramArmor(CardGame.Core.ProgramSkillFrame)", false),
new Route("HasIssuedPlayPhaseUseBan", "method", "CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime", "HasIssuedPlayPhaseUseBan", "CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries", "HasIssuedPlayPhaseUseBan", "", "M:CardGame.Core.GameEngine.HasIssuedPlayPhaseUseBan(System.Int32)", false),
new Route("HasOwnedProgramChildSubtree", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime", "HasOwnedProgramChildSubtree", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries", "HasOwnedProgramChildSubtree", "", "M:CardGame.Core.GameEngine.HasOwnedProgramChildSubtree(System.Int64)", false),
new Route("HasRuntimeSkillInstance", "method", "CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime", "HasRuntimeSkillInstance", "CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries", "HasRuntimeSkillInstance", "", "M:CardGame.Core.GameEngine.HasRuntimeSkillInstance(CardGame.Core.CharacterState,System.String,System.String)", false),
new Route("HeQiTargetAllowed", "method", "CardGame.Core.EngineOwners.CharacterHeQiOlRuntime", "HeQiTargetAllowed", "CardGame.Core.RuntimeEntries.CharacterHeQiOlEntries", "HeQiTargetAllowed", "", "M:CardGame.Core.GameEngine.HeQiTargetAllowed(System.Int32,System.Int32)", false),
new Route("IsCardUseForbidden", "method", "CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime", "IsCardUseForbidden", "CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries", "IsCardUseForbidden", "", "M:CardGame.Core.GameEngine.IsCardUseForbidden(System.Int32,CardGame.Core.CardKind,CardGame.Core.CardActionType,System.Boolean,System.Nullable{System.Int64})", false),
new Route("IsDyingSuitsProgramDying", "method", "CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime", "IsDyingSuitsProgramDying", "CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries", "IsDyingSuitsProgramDying", "", "M:CardGame.Core.GameEngine.IsDyingSuitsProgramDying", false),
new Route("IsRuntimeAcquiredSkill", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramStateProjectionRuntime", "IsRuntimeAcquiredSkill", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramStateProjectionEntries", "IsRuntimeAcquiredSkill", "", "M:CardGame.Core.GameEngine.IsRuntimeAcquiredSkill(CardGame.Core.CharacterState,System.String)", false),
new Route("IssueCurrentTurnNonLockedSkillSuppression", "method", "CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime", "IssueCurrentTurnNonLockedSkillSuppression", "CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries", "IssueCurrentTurnNonLockedSkillSuppression", "", "M:CardGame.Core.GameEngine.IssueCurrentTurnNonLockedSkillSuppression(CardGame.Core.ProgramSkillFrame,System.Int32)", false),
new Route("LiveDeclarationPayment", "method", "CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime", "LiveDeclarationPayment", "CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries", "LiveDeclarationPayment", "", "M:CardGame.Core.GameEngine.LiveDeclarationPayment(System.Int32)", false),
new Route("MatchesDeferredTurnEndPreludeChild", "method", "CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime", "MatchesDeferredTurnEndPreludeChild", "CardGame.Core.RuntimeEntries.CoreGameEngineDeferredTurnEndPreludesEntries", "MatchesDeferredTurnEndPreludeChild", "", "M:CardGame.Core.GameEngine.MatchesDeferredTurnEndPreludeChild(CardGame.Core.DeferredTurnEndFrame,CardGame.Core.CardsMovedTriggerWindowFrame)", false),
new Route("MatchingActualHandGainIndexes", "method", "CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime", "MatchingActualHandGainIndexes", "CardGame.Core.RuntimeEntries.CoreGameEngineActualHandGainProgramsEntries", "MatchingActualHandGainIndexes", "", "M:CardGame.Core.GameEngine.MatchingActualHandGainIndexes(CardGame.Core.CardMovementBatchContext,System.Int32,CardGame.Core.CardLocation)", false),
new Route("MoveCard", "method", "CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime", "MoveCard", "CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries", "MoveCard", "", "M:CardGame.Core.GameEngine.MoveCard(CardGame.Core.Card,CardGame.Core.CardLocation,CardGame.Core.CardLocation,CardGame.Core.CardMoveReason,System.Action{CardGame.Core.CardMovementRecord},System.Boolean,CardGame.Core.CardMovementDisclosure)", false),
new Route("PairedHandRecastObserverRoot", "method", "CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime", "PairedHandRecastObserverRoot", "CardGame.Core.RuntimeEntries.CoreGameEnginePairedHandRecastObserversEntries", "PairedHandRecastObserverRoot", "", "M:CardGame.Core.GameEngine.PairedHandRecastObserverRoot", false),
new Route("PredictPublicIncomingDamage", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime", "PredictPublicIncomingDamage", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicDamagePredictionEntries", "PredictPublicIncomingDamage", "", "M:CardGame.Core.GameEngine.PredictPublicIncomingDamage(CardGame.Core.GameEngine.IDamageAttempt,System.Int32,System.Nullable{System.Int32},System.Nullable{System.Int32},System.Nullable{System.Int32})", false),
new Route("PrepareProgramSkillChange", "method", "CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime", "PrepareProgramSkillChange", "CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries", "PrepareProgramSkillChange", "", "M:CardGame.Core.GameEngine.PrepareProgramSkillChange(CardGame.Core.ProgramSkillFrame)", false),
new Route("ProjectTypedCommittedSlashPaymentCursor", "method", "CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime", "ProjectTypedCommittedSlashPaymentCursor", "CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries", "ProjectTypedCommittedSlashPaymentCursor", "", "M:CardGame.Core.GameEngine.ProjectTypedCommittedSlashPaymentCursor", false),
new Route("ProjectTypedResponseCompletionHealthCursor", "method", "CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime", "ProjectTypedResponseCompletionHealthCursor", "CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries", "ProjectTypedResponseCompletionHealthCursor", "", "M:CardGame.Core.GameEngine.ProjectTypedResponseCompletionHealthCursor", false),
new Route("PublicDeclarationDescription", "method", "CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime", "PublicDeclarationDescription", "CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries", "PublicDeclarationDescription", "", "M:CardGame.Core.GameEngine.PublicDeclarationDescription(CardGame.Core.Card,System.String)", false),
new Route("PublicPileCards", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime", "PublicPileCards", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries", "PublicPileCards", "", "M:CardGame.Core.GameEngine.PublicPileCards(CardGame.Core.PublicPersistentPileSource)", false),
new Route("PublicPileCards", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime", "PublicPileCards", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries", "PublicPileCards2", "", "M:CardGame.Core.GameEngine.PublicPileCards(System.Int32)", false),
new Route("PublicPileProgramCount", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime", "PublicPileProgramCount", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries", "PublicPileProgramCount", "", "M:CardGame.Core.GameEngine.PublicPileProgramCount(System.Int32,System.String,System.String)", false),
new Route("PushRuntimeFrame", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime", "PushRuntimeFrame", "CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries", "PushRuntimeFrame", "", "M:CardGame.Core.GameEngine.PushRuntimeFrame(CardGame.Core.ResolutionFrame)", false),
new Route("RebuildAcceptedActualHandGainUse", "method", "CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime", "RebuildAcceptedActualHandGainUse", "CardGame.Core.RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries", "RebuildAcceptedActualHandGainUse", "", "M:CardGame.Core.GameEngine.RebuildAcceptedActualHandGainUse(CardGame.Core.CardUseFrame,CardGame.Core.CardActionContext,CardGame.Core.CardActionContext,System.Collections.Generic.IReadOnlyList{CardGame.Core.IGameEvent},System.Int32)", false),
new Route("ReferencedPublicPileSources", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime", "ReferencedPublicPileSources", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries", "ReferencedPublicPileSources", "", "M:CardGame.Core.GameEngine.ReferencedPublicPileSources(System.Int32,System.String,System.String)", false),
new Route("ReplaceRuntimeFrame", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime", "ReplaceRuntimeFrame", "CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries", "ReplaceRuntimeFrame", "", "M:CardGame.Core.GameEngine.ReplaceRuntimeFrame(System.Int64,CardGame.Core.ResolutionFrame)", false),
new Route("ReplaceRuntimeTop", "method", "CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime", "ReplaceRuntimeTop", "CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries", "ReplaceRuntimeTop", "", "M:CardGame.Core.GameEngine.ReplaceRuntimeTop(CardGame.Core.ResolutionFrame)", false),
new Route("SinglePublicPileSource", "method", "CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime", "SinglePublicPileSource", "CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries", "SinglePublicPileSource", "", "M:CardGame.Core.GameEngine.SinglePublicPileSource(System.Int32)", false),
new Route("ToJudgmentSnapshot", "method", "CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime", "ToJudgmentSnapshot", "CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries", "ToJudgmentSnapshot", "", "M:CardGame.Core.GameEngine.ToJudgmentSnapshot(CardGame.Core.Card)", false),
new Route("ToSnapshot", "method", "CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime", "ToSnapshot", "CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries", "ToSnapshot", "", "M:CardGame.Core.GameEngine.ToSnapshot(CardGame.Core.Card)", false),
new Route("TryAdvancePairedHandRecastSubtree", "method", "CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime", "TryAdvancePairedHandRecastSubtree", "CardGame.Core.RuntimeEntries.CoreGameEnginePairedHandRecastObserversEntries", "TryAdvancePairedHandRecastSubtree", "", "M:CardGame.Core.GameEngine.TryAdvancePairedHandRecastSubtree", false),
new Route("TryBeginBeforeDamageProgramWindow", "method", "CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime", "TryBeginBeforeDamageProgramWindow", "CardGame.Core.RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries", "TryBeginBeforeDamageProgramWindow", "", "M:CardGame.Core.GameEngine.TryBeginBeforeDamageProgramWindow(System.Int32,System.Int32,System.Int32,CardGame.Core.DamageNature,CardGame.Core.BeforeDamageProgramContinuation,System.Boolean)", false),
new Route("TypedDamageProgramObserver", "method", "CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime", "TypedDamageProgramObserver", "CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries", "TypedDamageProgramObserver", "", "M:CardGame.Core.GameEngine.TypedDamageProgramObserver(System.Int64)", false),
new Route("TypedRecoveryProgramDyingMatches", "method", "CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime", "TypedRecoveryProgramDyingMatches", "CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries", "TypedRecoveryProgramDyingMatches", "", "M:CardGame.Core.GameEngine.TypedRecoveryProgramDyingMatches(CardGame.Core.ProgramSkillFrame,CardGame.Core.DyingFrame,System.Collections.Generic.IReadOnlyList{CardGame.Core.IGameEvent})", false),
new Route("UnclaimedDeclarationPayment", "method", "CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime", "UnclaimedDeclarationPayment", "CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries", "UnclaimedDeclarationPayment", "", "M:CardGame.Core.GameEngine.UnclaimedDeclarationPayment(System.Int32,System.Int32)", false),
new Route("ValidNativeDrawInvocation", "method", "CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime", "ValidNativeDrawInvocation", "CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries", "ValidNativeDrawInvocation", "", "M:CardGame.Core.GameEngine.ValidNativeDrawInvocation(CardGame.Core.NativeDrawInvocationProof)", false),
new Route("ActiveCardAttack", "property", "CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime", "ActiveCardAttack", "CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries", "ActiveCardAttack", "", "P:CardGame.Core.GameEngine.ActiveCardAttack", false),
new Route("_resolutionSequence", "property", "CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime", "_resolutionSequence", "CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries", "_resolutionSequence", "", "P:CardGame.Core.GameEngine._resolutionSequence", false)
,
new Route("ProgramSkillHost", "type", "CardGame.Core.NativeOwners.ProgramSkillHost", "", "", "", "", "T:CardGame.Core.GameEngine.ProgramSkillHost", true),
new Route("WushuangModeHost", "type", "CardGame.Core.NativeOwners.WushuangModeHost", "", "", "", "", "T:CardGame.Core.GameEngine.WushuangModeHost", true)    ];
    private static Type ProductionType(string name) => typeof(GameEngine).Assembly.GetType(name, throwOnError: true)!;
    private static Type MetadataOwner(Route route) => ProductionType(route.Proxy.Length == 0 ? route.Owner : route.Proxy);
    private static string MetadataName(Route route) => route.Entry.Length == 0 ? route.Target : route.Entry == "this" ? "Item" : route.Entry;
    private static bool Includes(Route route, BindingFlags flags) => (flags & BindingFlags.NonPublic) != 0 &&
        (route.Static ? (flags & BindingFlags.Static) != 0 : (flags & BindingFlags.Instance) != 0);
    private static bool SameName(string first, string second, BindingFlags flags) => string.Equals(first, second,
        (flags & BindingFlags.IgnoreCase) != 0 ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    private static bool SameType(Type first, Type second) => first == second ||
        first.IsGenericParameter && second.IsGenericParameter && first.GenericParameterPosition == second.GenericParameterPosition &&
            (first.DeclaringMethod is null) == (second.DeclaringMethod is null) ||
        first.HasElementType && second.HasElementType && first.IsByRef == second.IsByRef && first.IsPointer == second.IsPointer &&
            first.IsArray == second.IsArray && (!first.IsArray || first.GetArrayRank() == second.GetArrayRank()) && SameType(first.GetElementType()!, second.GetElementType()!) ||
        first.IsConstructedGenericType && second.IsConstructedGenericType && first.GetGenericTypeDefinition() == second.GetGenericTypeDefinition() &&
            first.GetGenericArguments().Zip(second.GetGenericArguments()).All(pair => SameType(pair.First, pair.Second));
    private static bool SameSignature(MethodInfo first, MethodInfo second) => first.IsStatic == second.IsStatic &&
        first.GetGenericArguments().Length == second.GetGenericArguments().Length && first.GetParameters().Length == second.GetParameters().Length &&
        first.GetParameters().Zip(second.GetParameters()).All(pair => SameType(pair.First.ParameterType, pair.Second.ParameterType));
    private static string DocType(Type type)
    {
        if (type.IsByRef) return DocType(type.GetElementType()!) + "@";
        if (type.IsPointer) return DocType(type.GetElementType()!) + "*";
        if (type.IsArray) return DocType(type.GetElementType()!) + (type.GetArrayRank() == 1 ? "[]" : "[" + string.Join(",", Enumerable.Repeat("0:", type.GetArrayRank())) + "]");
        if (type.IsGenericParameter) return (type.DeclaringMethod is null ? "`" : "``") + type.GenericParameterPosition;
        if (!type.IsGenericType) return (type.FullName ?? type.Name).Replace('+', '.');
        var definition = System.Text.RegularExpressions.Regex.Replace(type.GetGenericTypeDefinition().FullName!, "`[0-9]+", "").Replace('+', '.');
        return definition + "{" + string.Join(",", type.GetGenericArguments().Select(DocType)) + "}";
    }
    private static MethodInfo TargetMethod(Route route)
    {
        var methods = MetadataOwner(route).GetMethods(All).Where(method => method.Name == MetadataName(route)).ToArray();
        if (methods.Length == 1) return methods[0];
        var expected = route.Signature[(route.Signature.IndexOf("GameEngine.", StringComparison.Ordinal) + "GameEngine.".Length)..];
        return methods.Single(method => route.Name + (method.GetGenericArguments().Length == 0 ? "" : "``" + method.GetGenericArguments().Length) +
            (method.GetParameters().Length == 0 ? "" : "(" + string.Join(",", method.GetParameters().Select(parameter => DocType(parameter.ParameterType))) + ")") == expected);
    }
    private static object? Target(Route route, object? original)
    {
        if (route.Static) return null;
        if (original is not GameEngine game) throw new TargetException(original is null ? "Non-static method requires a target." : "Object does not match target type.");
        if (route.Composition.Length != 0)
        {
            var field = typeof(GameEngine).GetField(route.Composition, All) ?? throw new MissingFieldException(typeof(GameEngine).FullName, route.Composition);
            var actual = field.GetValue(game) ?? throw new InvalidOperationException("The actual state owner has not been composed.");
            if (actual.GetType() != ProductionType(route.Owner)) throw new InvalidOperationException("State route resolved a different actual owner.");
            return actual;
        }
        if (route.Proxy.Length == 0) throw new InvalidOperationException("An instance runtime route must use its actual deferred entry.");
        var proxy = ProductionType(route.Proxy); var owner = ProductionType(route.Owner);
        var entry = typeof(GameEngine).GetMethod("GetDeferredRuntimeEntry", All)!.MakeGenericMethod(proxy, owner);
        var factory = proxy.GetMethod("Create", All)!.CreateDelegate(entry.GetParameters()[0].ParameterType);
        return entry.Invoke(game, [factory]); // caches a resolver; does not construct the module
    }
    private static IEnumerable<MethodInfo> Methods(string? name, BindingFlags flags)
    {
        var selected = Routes.Where(route => route.Kind == "method" && (name is null || SameName(route.Name, name, flags)) && Includes(route, flags))
            .Select(route => (Route: route, Method: TargetMethod(route))).ToArray();
        foreach (var method in typeof(GameEngine).GetMethods(flags).Where(method => (name is null || SameName(method.Name, name, flags)) &&
            !selected.Any(selected => selected.Route.Name == method.Name && SameSignature(method, selected.Method)))) yield return method;
        foreach (var selectedMethod in selected)
        {
            yield return new RoutedMethod(selectedMethod.Method, selectedMethod.Route);
        }
    }
    private sealed class EngineType() : TypeDelegator(typeof(GameEngine))
    {
        protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
        {
            var methods = Methods(name, bindingAttr).Where(method => callConvention == CallingConventions.Any || (method.CallingConvention & callConvention) == callConvention).ToArray();
            if (methods.Length == 0) return null;
            if (types is not null) return (MethodInfo?)(binder ?? Type.DefaultBinder).SelectMethod(bindingAttr, methods, types, modifiers);
            return methods.Length switch { 0 => null, 1 => methods[0], _ => throw new AmbiguousMatchException() };
        }
        protected override MethodInfo? GetMethodImpl(string name, int genericParameterCount, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
        {
            var methods = Methods(name, bindingAttr).Where(method => method.GetGenericArguments().Length == genericParameterCount &&
                (callConvention == CallingConventions.Any || (method.CallingConvention & callConvention) == callConvention)).ToArray();
            if (methods.Length == 0) return null;
            if (types is null) return methods.Length == 1 ? methods[0] : throw new AmbiguousMatchException();
            return (MethodInfo?)(binder ?? Type.DefaultBinder).SelectMethod(bindingAttr, methods, types, modifiers);
        }
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => Methods(null, bindingAttr).ToArray();
        public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
        {
            var route = Routes.SingleOrDefault(route => route.Kind == "field" && SameName(route.Name, name, bindingAttr) && Includes(route, bindingAttr));
            return route is null ? typeof(GameEngine).GetField(name, bindingAttr) : new RoutedField(ProductionType(route.Owner).GetField(route.Target, All)!, route);
        }
        protected override PropertyInfo? GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers)
        {
            var route = Routes.SingleOrDefault(route => route.Kind == "property" && SameName(route.Name, name, bindingAttr) && Includes(route, bindingAttr));
            if (route is null) return base.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);
            var properties = MetadataOwner(route).GetProperties(All).Where(property => property.Name == MetadataName(route)).ToArray();
            if (properties.Length == 0) return null;
            var property = types is null && returnType is null ? properties.SingleOrDefault() : (binder ?? Type.DefaultBinder).SelectProperty(bindingAttr, properties, returnType, types, modifiers);
            return property is null ? null : new RoutedProperty(property, route);
        }
        public override Type? GetNestedType(string name, BindingFlags bindingAttr)
        {
            var route = Routes.SingleOrDefault(route => route.Kind == "type" && route.Name == name);
            return route is not null && (bindingAttr & BindingFlags.NonPublic) != 0 ? ProductionType(route.Owner) : typeof(GameEngine).GetNestedType(name, bindingAttr);
        }
    }
    private sealed class RoutedMethod(MethodInfo method, Route route) : MethodInfo
    {
        internal string OriginalName => route.Name;
        public override string Name => method.Name;
        public override Type? DeclaringType => method.DeclaringType;
        public override Type? ReflectedType => method.ReflectedType;
        public override Module Module => method.Module;
        public override RuntimeMethodHandle MethodHandle => method.MethodHandle;
        public override MethodAttributes Attributes => method.Attributes;
        public override Type ReturnType => method.ReturnType;
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => method.ReturnTypeCustomAttributes;
        public override ParameterInfo ReturnParameter => method.ReturnParameter;
        public override bool IsGenericMethod => method.IsGenericMethod;
        public override bool IsGenericMethodDefinition => method.IsGenericMethodDefinition;
        public override bool ContainsGenericParameters => method.ContainsGenericParameters;
        public override MethodImplAttributes GetMethodImplementationFlags() => method.GetMethodImplementationFlags();
        public override ParameterInfo[] GetParameters() => method.GetParameters();
        public override Type[] GetGenericArguments() => method.GetGenericArguments();
        public override MethodInfo GetGenericMethodDefinition() => new RoutedMethod(method.GetGenericMethodDefinition(), route);
        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => new RoutedMethod(method.MakeGenericMethod(typeArguments), route);
        public override MethodInfo GetBaseDefinition() => method.GetBaseDefinition();
        public override object[] GetCustomAttributes(bool inherit) => method.GetCustomAttributes(inherit);
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => method.GetCustomAttributes(attributeType, inherit);
        public override bool IsDefined(Type attributeType, bool inherit) => method.IsDefined(attributeType, inherit);
        public override object? Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture) => method.Invoke(Target(route, obj), invokeAttr, binder, parameters, culture);
        public override Delegate CreateDelegate(Type delegateType) => route.Static ? method.CreateDelegate(delegateType) : throw new NotSupportedException("An instance fixture route requires its original Engine target.");
        public override Delegate CreateDelegate(Type delegateType, object? target) => method.CreateDelegate(delegateType,
            route.Static || target is not GameEngine ? target : Target(route, target));
    }
    private sealed class RoutedField(FieldInfo field, Route route) : FieldInfo
    {
        public override string Name => field.Name;
        public override Type? DeclaringType => field.DeclaringType;
        public override Type? ReflectedType => field.ReflectedType;
        public override FieldAttributes Attributes => field.Attributes;
        public override RuntimeFieldHandle FieldHandle => field.FieldHandle;
        public override Type FieldType => field.FieldType;
        public override object[] GetCustomAttributes(bool inherit) => field.GetCustomAttributes(inherit);
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => field.GetCustomAttributes(attributeType, inherit);
        public override bool IsDefined(Type attributeType, bool inherit) => field.IsDefined(attributeType, inherit);
        public override object? GetValue(object? obj) => field.GetValue(Target(route, obj));
        public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, CultureInfo? culture) => field.SetValue(Target(route, obj), value, invokeAttr, binder, culture);
    }
    private sealed class RoutedProperty(PropertyInfo property, Route route) : PropertyInfo
    {
        public override string Name => property.Name;
        public override Type? DeclaringType => property.DeclaringType;
        public override Type? ReflectedType => property.ReflectedType;
        public override PropertyAttributes Attributes => property.Attributes;
        public override bool CanRead => property.CanRead;
        public override bool CanWrite => property.CanWrite;
        public override Type PropertyType => property.PropertyType;
        public override MethodInfo[] GetAccessors(bool nonPublic) => property.GetAccessors(nonPublic).Select(method => (MethodInfo)new RoutedMethod(method, route)).ToArray();
        public override MethodInfo? GetGetMethod(bool nonPublic) => property.GetGetMethod(nonPublic) is { } method ? new RoutedMethod(method, route) : null;
        public override MethodInfo? GetSetMethod(bool nonPublic) => property.GetSetMethod(nonPublic) is { } method ? new RoutedMethod(method, route) : null;
        public override ParameterInfo[] GetIndexParameters() => property.GetIndexParameters();
        public override object[] GetCustomAttributes(bool inherit) => property.GetCustomAttributes(inherit);
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => property.GetCustomAttributes(attributeType, inherit);
        public override bool IsDefined(Type attributeType, bool inherit) => property.IsDefined(attributeType, inherit);
        public override object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture) => property.GetValue(Target(route, obj), invokeAttr, binder, index, culture);
        public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture) => property.SetValue(Target(route, obj), value, invokeAttr, binder, index, culture);
    }
}
