#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouSunShangxiangRuntime))]
internal sealed class CharacterSunShangxiangMouMobileMouSunShangxiangAdapter(Func<MouSunShangxiangRuntime> targetFactory) : ICharacterSunShangxiangMouMobileMouSunShangxiangOperations
{
    private readonly Func<MouSunShangxiangRuntime> _targetFactory = targetFactory;
    public MouSunShangxiangRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreWushuangModeProjectionRuntime))]
internal sealed class CharacterSunShangxiangMouMobileCoreWushuangModeProjectionAdapter(Func<CoreWushuangModeProjectionRuntime> targetFactory) : ICharacterSunShangxiangMouMobileCoreWushuangModeProjectionOperations
{
    private readonly Func<CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterSunShangxiangMouMobileCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterSunShangxiangMouMobileCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
