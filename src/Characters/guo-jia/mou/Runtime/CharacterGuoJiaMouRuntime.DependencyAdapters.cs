#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouGuoJiaRuntime))]
internal sealed class CharacterGuoJiaMouMouGuoJiaAdapter(Func<MouGuoJiaRuntime> targetFactory) : ICharacterGuoJiaMouMouGuoJiaOperations
{
    private readonly Func<MouGuoJiaRuntime> _targetFactory = targetFactory;
    public MouGuoJiaRuntime Get() => this._targetFactory();
}
