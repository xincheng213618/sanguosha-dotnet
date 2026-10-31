#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public bool CanContinueAfterOwnerDeath(ProgramSkillFrame frame) => getCoreGameEngineProgramContinuationPermissionsRuntime().Value.CanContinueProgramAfterOwnerDeath(frame);
    public bool CanContinueAfterSkillLoss(ProgramSkillFrame frame) => getCoreGameEngineProgramContinuationPermissionsRuntime().Value.CanContinueProgramAfterSkillLoss(frame);
}
