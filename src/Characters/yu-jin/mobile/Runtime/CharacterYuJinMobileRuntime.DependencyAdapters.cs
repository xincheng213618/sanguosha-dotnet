#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramPaidCardRuntime))]
internal sealed class CharacterYuJinMobileProgramPaidCardAdapter(Func<ProgramPaidCardRuntime> targetFactory) : ICharacterYuJinMobileProgramPaidCardOperations
{
    private readonly Func<ProgramPaidCardRuntime> _targetFactory = targetFactory;
    public ProgramPaidCardRuntime Get() => this._targetFactory();
}
