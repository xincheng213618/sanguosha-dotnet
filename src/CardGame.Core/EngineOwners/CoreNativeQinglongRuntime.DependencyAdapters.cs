#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramCardLegalityRuntime))]
internal sealed class CoreNativeQinglongCoreProgramCardLegalityAdapter(Func<CoreProgramCardLegalityRuntime> targetFactory) : ICoreNativeQinglongCoreProgramCardLegalityOperations
{
    private readonly Func<CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}
