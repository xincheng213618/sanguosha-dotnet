#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramDistanceRulesRuntime))]
internal sealed class CharacterLiuFengBoundaryCoreProgramDistanceRulesAdapter(Func<CoreProgramDistanceRulesRuntime> targetFactory) : ICharacterLiuFengBoundaryCoreProgramDistanceRulesOperations
{
    private readonly Func<CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterLiuFengBoundaryCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterLiuFengBoundaryCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterLiuFengBoundaryCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterLiuFengBoundaryCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
