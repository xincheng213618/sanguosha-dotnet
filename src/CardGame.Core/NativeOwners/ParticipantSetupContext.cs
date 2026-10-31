#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ParticipantSetupContext(global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineMultiplayerEntries> getCoreGameEngineMultiplayerRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduActivitiesEntries> getCoreGameEngineGuanduActivitiesRuntime, global::CardGame.Core.EngineStates.CoreGameEngineShenwuActivitiesState gameEngineShenwuActivitiesState, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreGameEngineGuanduActivitiesState gameEngineGuanduActivitiesState) : IMatchParticipantSetupQueries, IMatchParticipantSetupState
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
