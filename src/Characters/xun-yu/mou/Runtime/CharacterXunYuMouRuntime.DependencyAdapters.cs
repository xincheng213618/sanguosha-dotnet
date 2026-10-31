#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouXunYuAnniversaryRuntime))]
internal sealed class CharacterXunYuMouMouXunYuAnniversaryAdapter(Func<MouXunYuAnniversaryRuntime> targetFactory) : ICharacterXunYuMouMouXunYuAnniversaryOperations
{
    private readonly Func<MouXunYuAnniversaryRuntime> _targetFactory = targetFactory;
    public MouXunYuAnniversaryRuntime Get() => this._targetFactory();
}
