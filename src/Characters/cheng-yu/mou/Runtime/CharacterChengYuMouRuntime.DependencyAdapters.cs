#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(MouChengYuAnniversaryRuntime))]
internal sealed class CharacterChengYuMouMouChengYuAnniversaryAdapter(Func<MouChengYuAnniversaryRuntime> targetFactory) : ICharacterChengYuMouMouChengYuAnniversaryOperations
{
    private readonly Func<MouChengYuAnniversaryRuntime> _targetFactory = targetFactory;
    public MouChengYuAnniversaryRuntime Get() => this._targetFactory();
}
