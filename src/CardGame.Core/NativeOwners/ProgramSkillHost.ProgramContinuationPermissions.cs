#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public bool CanContinueAfterOwnerDeath(ProgramSkillFrame frame) => getCoreGameEngineProgramContinuationPermissionsRuntime().CanContinueProgramAfterOwnerDeath(frame);
    public bool CanContinueAfterSkillLoss(ProgramSkillFrame frame) => getCoreGameEngineProgramContinuationPermissionsRuntime().CanContinueProgramAfterSkillLoss(frame);
}
