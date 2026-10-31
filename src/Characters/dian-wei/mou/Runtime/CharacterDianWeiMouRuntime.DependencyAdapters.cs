#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouDianWeiRuntime))]
internal sealed class CharacterDianWeiMouMouDianWeiAdapter(Func<MouDianWeiRuntime> targetFactory) : ICharacterDianWeiMouMouDianWeiOperations
{
    private readonly Func<MouDianWeiRuntime> _targetFactory = targetFactory;
    public MouDianWeiRuntime Get() => this._targetFactory();
}
