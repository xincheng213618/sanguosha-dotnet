#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramDistanceRulesRuntime))]
internal sealed class CoreNativeBorrowedSwordCoreProgramDistanceRulesAdapter(Func<CoreProgramDistanceRulesRuntime> targetFactory) : ICoreNativeBorrowedSwordCoreProgramDistanceRulesOperations
{
    private readonly Func<CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CoreNativeBorrowedSwordCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICoreNativeBorrowedSwordCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[Core.ProgramCapability(typeof(CoreProgramCardLegalityRuntime))]
internal sealed class CoreNativeBorrowedSwordCoreProgramCardLegalityAdapter(Func<CoreProgramCardLegalityRuntime> targetFactory) : ICoreNativeBorrowedSwordCoreProgramCardLegalityOperations
{
    private readonly Func<CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}
