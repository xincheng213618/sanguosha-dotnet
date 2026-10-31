#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramContinuationPermissionsRuntime
{
    internal bool CanContinueProgramAfterOwnerDeath(ProgramSkillFrame frame) => _coreProgramRuntimeHooksOperations.RuntimeHooks.CanContinue(ProgramRuntimePhase.AfterOwnerDeath, frame);
    internal bool CanContinueProgramAfterSkillLoss(ProgramSkillFrame frame) => _coreProgramRuntimeHooksOperations.RuntimeHooks.CanContinue(ProgramRuntimePhase.AfterSkillLoss, frame);
}
