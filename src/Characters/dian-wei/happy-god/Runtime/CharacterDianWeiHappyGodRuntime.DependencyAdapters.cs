#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramLifecycleWindowRuntime))]
internal sealed class CharacterDianWeiHappyGodProgramLifecycleWindowAdapter(Func<ProgramLifecycleWindowRuntime> targetFactory) : ICharacterDianWeiHappyGodProgramLifecycleWindowOperations
{
    private readonly Func<ProgramLifecycleWindowRuntime> _targetFactory = targetFactory;
    public ProgramLifecycleWindowRuntime Get() => this._targetFactory();
}
