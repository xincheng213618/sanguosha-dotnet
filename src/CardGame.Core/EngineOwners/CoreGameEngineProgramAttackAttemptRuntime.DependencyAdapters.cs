#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramDamageHooksRuntime))]
internal sealed class CoreProgramAttackAttemptCoreProgramDamageHooksAdapter(Func<CoreProgramDamageHooksRuntime> targetFactory) : ICoreProgramAttackAttemptCoreProgramDamageHooksOperations
{
    private readonly Func<CoreProgramDamageHooksRuntime> _targetFactory = targetFactory;
    public ProgramDamageHooks DamageHooks { get => this._targetFactory().DamageHooks; }
}
