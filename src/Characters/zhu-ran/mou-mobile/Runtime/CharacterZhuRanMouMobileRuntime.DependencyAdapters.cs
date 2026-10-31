#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterZhuRanMouMobileCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterZhuRanMouMobileCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MouZhuRanRuntime))]
internal sealed class CharacterZhuRanMouMobileMouZhuRanAdapter(Func<MouZhuRanRuntime> targetFactory) : ICharacterZhuRanMouMobileMouZhuRanOperations
{
    private readonly Func<MouZhuRanRuntime> _targetFactory = targetFactory;
    public MouZhuRanRuntime Get() => this._targetFactory();
}
