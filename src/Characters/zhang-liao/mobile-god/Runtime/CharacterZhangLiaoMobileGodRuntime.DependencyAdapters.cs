#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreTrustedMovementDiagnosticsRuntime))]
internal sealed class CharacterZhangLiaoMobileGodCoreTrustedMovementDiagnosticsAdapter(Func<CoreTrustedMovementDiagnosticsRuntime> targetFactory) : ICharacterZhangLiaoMobileGodCoreTrustedMovementDiagnosticsOperations
{
    private readonly Func<CoreTrustedMovementDiagnosticsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<CardMovementRecord> CardMovements { get => this._targetFactory().CardMovements; }
}
