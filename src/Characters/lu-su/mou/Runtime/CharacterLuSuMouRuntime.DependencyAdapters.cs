#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterLuSuMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterLuSuMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MouLuSuAnniversaryRuntime))]
internal sealed class CharacterLuSuMouMouLuSuAnniversaryAdapter(Func<MouLuSuAnniversaryRuntime> targetFactory) : ICharacterLuSuMouMouLuSuAnniversaryOperations
{
    private readonly Func<MouLuSuAnniversaryRuntime> _targetFactory = targetFactory;
    public MouLuSuAnniversaryRuntime Get() => this._targetFactory();
}
