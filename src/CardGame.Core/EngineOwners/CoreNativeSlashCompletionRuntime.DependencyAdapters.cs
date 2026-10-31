#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramDamageHooksRuntime))]
internal sealed class CoreNativeSlashCompletionCoreProgramDamageHooksAdapter(Func<CoreProgramDamageHooksRuntime> targetFactory) : ICoreNativeSlashCompletionCoreProgramDamageHooksOperations
{
    private readonly Func<CoreProgramDamageHooksRuntime> _targetFactory = targetFactory;
    public ProgramDamageHooks DamageHooks { get => this._targetFactory().DamageHooks; }
}
