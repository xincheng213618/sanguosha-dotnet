#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ParticipantSetupContext(EngineStates.CoreMatchDefinitionState matchDefinitionState, Func<RuntimeEntries.CoreGameEngineMultiplayerEntries> getCoreGameEngineMultiplayerRuntime, Func<RuntimeEntries.CoreGameEngineGuanduActivitiesEntries> getCoreGameEngineGuanduActivitiesRuntime, EngineStates.CoreGameEngineShenwuActivitiesState gameEngineShenwuActivitiesState, EngineStates.CoreMatchSetupState matchSetupState, EngineStates.CoreGameEngineGuanduActivitiesState gameEngineGuanduActivitiesState) : IMatchParticipantSetupQueries, IMatchParticipantSetupState
{
    public int SeatCount => matchDefinitionState.ReferenceToPlayerCount;

    public bool IsHuman(int seat) => getCoreGameEngineMultiplayerRuntime().IsHumanControlledSeat(seat);
    public bool IsGuanduCommander(int seat) => getCoreGameEngineGuanduActivitiesRuntime().IsGuanduCommander(seat);
    public IReadOnlyList<string>? SharedGods => gameEngineShenwuActivitiesState.ReferenceToShenwuDraft?.SharedGods;
    public string? LeaderId => gameEngineShenwuActivitiesState.ReferenceToShenwuDraft?.LeaderId;
    public GuanduBattleConfiguredEvent? GuanduConfiguration => getCoreGameEngineGuanduActivitiesRuntime().GuanduConfiguration;

    public void SetInitialActor(int? seat) => matchSetupState.ReferenceToModeInitialActorSeat = seat;
    public void SetSharedDraft(IReadOnlyList<string> sharedGods, string leaderId) => gameEngineShenwuActivitiesState.ReferenceToShenwuDraft = new(sharedGods, leaderId);
    public void SetGuanduConfiguration(GuanduBattleConfiguredEvent? configuration) => gameEngineGuanduActivitiesState.ReferenceToGuanduInitialConfiguration = configuration;
}
