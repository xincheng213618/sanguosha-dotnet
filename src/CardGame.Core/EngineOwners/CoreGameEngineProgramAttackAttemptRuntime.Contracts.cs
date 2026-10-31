#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramAttackAttemptCoreProgramDamageHooksOperations : IProgramRuntimeOperations
{
    ProgramDamageHooks DamageHooks { get; }
}
