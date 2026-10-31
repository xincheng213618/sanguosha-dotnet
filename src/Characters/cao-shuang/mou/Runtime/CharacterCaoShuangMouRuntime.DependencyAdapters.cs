#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouCaoShuangAnniversaryRuntime))]
internal sealed class CharacterCaoShuangMouMouCaoShuangAnniversaryAdapter(Func<MouCaoShuangAnniversaryRuntime> targetFactory) : ICharacterCaoShuangMouMouCaoShuangAnniversaryOperations
{
    private readonly Func<MouCaoShuangAnniversaryRuntime> _targetFactory = targetFactory;
    public MouCaoShuangAnniversaryRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterCaoShuangMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterCaoShuangMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
