#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineModeAllegianceRuntime))]
internal sealed class CoreTeamActivityProgramsCoreModeAllegianceAdapter(Func<CoreGameEngineModeAllegianceRuntime> targetFactory) : ICoreTeamActivityProgramsCoreModeAllegianceOperations
{
    private readonly Func<CoreGameEngineModeAllegianceRuntime> _targetFactory = targetFactory;
    public IMatchModeAllegiancePolicy ModeAllegiancePolicy { get => this._targetFactory().ModeAllegiancePolicy; }
}
