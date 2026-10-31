#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MoZhangFeiRuntime))]
internal sealed class CharacterZhangFeiOlMagicMoZhangFeiAdapter(Func<MoZhangFeiRuntime> targetFactory) : ICharacterZhangFeiOlMagicMoZhangFeiOperations
{
    private readonly Func<MoZhangFeiRuntime> _targetFactory = targetFactory;
    public MoZhangFeiRuntime Get() => this._targetFactory();
}
