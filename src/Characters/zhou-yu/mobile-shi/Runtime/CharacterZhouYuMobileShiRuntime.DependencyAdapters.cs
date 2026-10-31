#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ShiZhouYuRuntime))]
internal sealed class CharacterZhouYuMobileShiShiZhouYuAdapter(Func<ShiZhouYuRuntime> targetFactory) : ICharacterZhouYuMobileShiShiZhouYuOperations
{
    private readonly Func<ShiZhouYuRuntime> _targetFactory = targetFactory;
    public ShiZhouYuRuntime Get() => this._targetFactory();
}
