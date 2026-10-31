#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreTrustedCardZoneDiagnosticsRuntime))]
internal sealed class CharacterZuoCiHappyGodCoreTrustedCardZoneDiagnosticsAdapter(Func<CoreTrustedCardZoneDiagnosticsRuntime> targetFactory) : ICharacterZuoCiHappyGodCoreTrustedCardZoneDiagnosticsOperations
{
    private readonly Func<CoreTrustedCardZoneDiagnosticsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics() => this._targetFactory().CreateCardZoneDiagnostics();
}

[Core.ProgramCapability(typeof(ProgramPhaseBoundaryRuntime))]
internal sealed class CharacterZuoCiHappyGodProgramPhaseBoundaryAdapter(Func<ProgramPhaseBoundaryRuntime> targetFactory) : ICharacterZuoCiHappyGodProgramPhaseBoundaryOperations
{
    private readonly Func<ProgramPhaseBoundaryRuntime> _targetFactory = targetFactory;
    public ProgramPhaseBoundaryRuntime Get() => this._targetFactory();
}
