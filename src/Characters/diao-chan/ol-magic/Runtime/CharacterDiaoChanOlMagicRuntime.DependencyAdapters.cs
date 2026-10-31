#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramNativeReturnsRuntime))]
internal sealed class CharacterDiaoChanOlMagicCoreProgramNativeReturnsAdapter(Func<CoreProgramNativeReturnsRuntime> targetFactory) : ICharacterDiaoChanOlMagicCoreProgramNativeReturnsOperations
{
    private readonly Func<CoreProgramNativeReturnsRuntime> _targetFactory = targetFactory;
    public ProgramNativeReturns NativeReturns { get => this._targetFactory().NativeReturns; }
}
