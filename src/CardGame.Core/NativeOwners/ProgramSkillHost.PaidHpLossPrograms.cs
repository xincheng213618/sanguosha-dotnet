#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public SkillProgramStepOutcome ChooseOwnerHpLoss(ProgramSkillFrame frame, string resultBind) => getCoreGameEnginePaidHpLossProgramsRuntime().BeginHpLossQuantity(frame, resultBind);
    public SkillProgramStepOutcome DrawPaidHpLoss(ProgramSkillFrame frame, string sourceBind) => getCoreGameEnginePaidHpLossProgramsRuntime().DrawProgramPaidHpLoss(frame, sourceBind);
    public void GrantPaidHpLoss(ProgramSkillFrame frame, string sourceBind, bool distance) => getCoreGameEnginePaidHpLossProgramsRuntime().GrantProgramPaidHpLoss(frame, sourceBind, distance);
}
