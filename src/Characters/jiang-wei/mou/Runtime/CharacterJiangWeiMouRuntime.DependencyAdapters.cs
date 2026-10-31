#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouJiangWeiAnniversaryRuntime))]
internal sealed class CharacterJiangWeiMouMouJiangWeiAnniversaryAdapter(Func<MouJiangWeiAnniversaryRuntime> targetFactory) : ICharacterJiangWeiMouMouJiangWeiAnniversaryOperations
{
    private readonly Func<MouJiangWeiAnniversaryRuntime> _targetFactory = targetFactory;
    public MouJiangWeiAnniversaryRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterJiangWeiMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterJiangWeiMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
