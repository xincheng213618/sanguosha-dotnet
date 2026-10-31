#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CommandSessionRuntime(EngineStates.CoreMatchDefinitionState matchDefinitionState, Func<ContentRegistry> getContentRegistry, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, EngineStates.CoreCommandJournalStorageState commandJournalStorageState, Func<RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries> getCoreGameEnginePrivateGeneralLibrariesRuntime, Func<RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries> getCoreGameEngineLordSkillProjectionsRuntime, Func<RuntimeEntries.CoreGameEngineHpConditionalSkillGrantsEntries> getCoreGameEngineHpConditionalSkillGrantsRuntime, Func<RuntimeEntries.CharacterHeQiOlEntries> getCharacterHeQiOlRuntime, Func<RuntimeEntries.CoreCommandResultProjectionEntries> getCoreCommandResultProjectionRuntime, Func<RuntimeEntries.CharacterSimaYiMouEntries> getCharacterSimaYiMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime) : ICommandSessionRuntime
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
    public void RefreshDecisionRevision() => getCoreCommandResultProjectionRuntime().RefreshPendingDecisionRevision();
    public EventNotification? [] PreparePublicEvents(EventNotification[] events)
    {
        // This character's privacy policy remains at its existing native
        // projection boundary; the core session depends only on event slots.
        var projection = getCharacterSimaYiMouRuntime().PrepareMouSmPublicProjection();
        return projection is null ? events : events.Select(item => EngineOwners.CharacterSimaYiMouRuntime.MouSmPublicEvent(item.Event, projection)is { } projected ? new EventNotification(projected) : null).ToArray();
    }

    public void ClearPendingEvents() => getCoreGameEngineProgramEventHistoryRuntime().ClearPendingProgramEvents();
    public void ClearPendingPublication() => commandJournalStorageState.ReferenceToPendingStatePublication = false;
    public void MarkFaulted() => matchOutcomeState.ReferenceToStatus = EngineStatus.Faulted;
}
