#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MoSimaYiRuntime))]
internal sealed class CharacterSimaYiOlMagicMoSimaYiAdapter(Func<MoSimaYiRuntime> targetFactory) : ICharacterSimaYiOlMagicMoSimaYiOperations
{
    private readonly Func<MoSimaYiRuntime> _targetFactory = targetFactory;
    public MoSimaYiRuntime Get() => this._targetFactory();
}
