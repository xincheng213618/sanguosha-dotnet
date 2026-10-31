#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CommandSessionRuntime(global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, global::CardGame.Core.EngineStates.CoreCommandJournalStorageState commandJournalStorageState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries> getCoreGameEnginePrivateGeneralLibrariesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries> getCoreGameEngineLordSkillProjectionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpConditionalSkillGrantsEntries> getCoreGameEngineHpConditionalSkillGrantsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHeQiOlEntries> getCharacterHeQiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCoreInvariantsEntries> getCoreNativeCoreInvariantsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreCommandResultProjectionEntries> getCoreCommandResultProjectionRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries> getCharacterSimaYiMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime) : ICommandSessionRuntime
{
    public GameOptions Options => matchDefinitionState.ReferenceToOptions;
    public string ModeId => matchDefinitionState.ReferenceToModeDefinition.Id;
    public IReadOnlyList<string> ContentPackages => GameReplay.GetPackageSignatures(getContentRegistry());
    public string ContentHash => GameReplay.GetContentHash(getContentRegistry());
    public CommandOutcome Outcome => new(matchOutcomeState.ReferenceToStatus, matchOutcomeState.ReferenceToWinner, matchOutcomeState.ReferenceToWinnerSeat, getCoreNativeDamageHandlesRuntime().IsTeamMode ? matchOutcomeState.ReferenceToWinnerTeamId : null, getCoreNativeDamageHandlesRuntime().IsNationalWarMode ? matchOutcomeState.ReferenceToWinnerFactionId : null);
    public long EventSequence { get => commandJournalStorageState.ReferenceToEventSequence; set => commandJournalStorageState.ReferenceToEventSequence = value; }

    public void SynchronizeDerivedState()
    {
        getCoreGameEnginePrivateGeneralLibrariesRuntime().SynchronizePrivateGeneralLibraries();
        getCoreGameEngineLordSkillProjectionsRuntime().SynchronizeLordSkillProjections();
        getCoreGameEngineHpConditionalSkillGrantsRuntime().SynchronizeHpConditionalSkillGrants();
        getCharacterHeQiOlRuntime().SynchronizeEquipmentSuitSkillGrants();
    }

    public void AssertInvariants() => getCoreNativeCoreInvariantsRuntime().AssertCoreInvariants();
    public void RefreshDecisionRevision() => getCoreCommandResultProjectionRuntime().RefreshPendingDecisionRevision();
    public EventNotification? [] PreparePublicEvents(EventNotification[] events)
    {
        // This character's privacy policy remains at its existing native
        // projection boundary; the core session depends only on event slots.
        var projection = getCharacterSimaYiMouRuntime().PrepareMouSmPublicProjection();
        return projection is null ? events : events.Select(item => global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime.MouSmPublicEvent(item.Event, projection)is { } projected ? new EventNotification(projected) : null).ToArray();
    }

    public void ClearPendingEvents() => getCoreGameEngineProgramEventHistoryRuntime().ClearPendingProgramEvents();
    public void ClearPendingPublication() => commandJournalStorageState.ReferenceToPendingStatePublication = false;
    public void MarkFaulted() => matchOutcomeState.ReferenceToStatus = EngineStatus.Faulted;
}
