#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(BoundaryYuanShuRuntime))]
internal sealed class CoreLordSkillProjectionsBoundaryYuanShuAdapter(Func<BoundaryYuanShuRuntime> targetFactory) : ICoreLordSkillProjectionsBoundaryYuanShuOperations
{
    private readonly Func<BoundaryYuanShuRuntime> _targetFactory = targetFactory;
    public BoundaryYuanShuRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreGameEngineModeAllegianceRuntime))]
internal sealed class CoreLordSkillProjectionsCoreModeAllegianceAdapter(Func<CoreGameEngineModeAllegianceRuntime> targetFactory) : ICoreLordSkillProjectionsCoreModeAllegianceOperations
{
    private readonly Func<CoreGameEngineModeAllegianceRuntime> _targetFactory = targetFactory;
    public IMatchModeAllegiancePolicy ModeAllegiancePolicy { get => this._targetFactory().ModeAllegiancePolicy; }
}
