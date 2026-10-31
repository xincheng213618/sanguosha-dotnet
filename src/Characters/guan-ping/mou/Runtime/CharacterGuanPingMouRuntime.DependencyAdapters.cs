#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouGuanPingRuntime))]
internal sealed class CharacterGuanPingMouMouGuanPingAdapter(Func<MouGuanPingRuntime> targetFactory) : ICharacterGuanPingMouMouGuanPingOperations
{
    private readonly Func<MouGuanPingRuntime> _targetFactory = targetFactory;
    public MouGuanPingRuntime Get() => this._targetFactory();
}
