#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreGameEngineModeAllegianceRuntime))]
internal sealed class CoreTeamActivitiesCoreModeAllegianceAdapter(Func<CoreGameEngineModeAllegianceRuntime> targetFactory) : ICoreTeamActivitiesCoreModeAllegianceOperations
{
    private readonly Func<CoreGameEngineModeAllegianceRuntime> _targetFactory = targetFactory;
    public IMatchModeAllegiancePolicy ModeAllegiancePolicy { get => this._targetFactory().ModeAllegiancePolicy; }
}
