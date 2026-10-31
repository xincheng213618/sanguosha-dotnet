#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MobileSimaRenjieRuntime))]
internal sealed class CharacterSimaYiMobileGodMobileSimaRenjieAdapter(Func<MobileSimaRenjieRuntime> targetFactory) : ICharacterSimaYiMobileGodMobileSimaRenjieOperations
{
    private readonly Func<MobileSimaRenjieRuntime> _targetFactory = targetFactory;
    public MobileSimaRenjieRuntime Get() => this._targetFactory();
}
