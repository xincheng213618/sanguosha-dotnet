#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(OlShenZhenJiRuntime))]
internal sealed class CharacterZhenJiOlGodOlShenZhenJiAdapter(Func<OlShenZhenJiRuntime> targetFactory) : ICharacterZhenJiOlGodOlShenZhenJiOperations
{
    private readonly Func<OlShenZhenJiRuntime> _targetFactory = targetFactory;
    public OlShenZhenJiRuntime Get() => this._targetFactory();
}
