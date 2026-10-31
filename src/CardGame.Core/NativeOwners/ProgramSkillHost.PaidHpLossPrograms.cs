#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public SkillProgramStepOutcome ChooseOwnerHpLoss(ProgramSkillFrame frame, string resultBind) => getCoreGameEnginePaidHpLossProgramsRuntime().Value.BeginHpLossQuantity(frame, resultBind);
    public SkillProgramStepOutcome DrawPaidHpLoss(ProgramSkillFrame frame, string sourceBind) => getCoreGameEnginePaidHpLossProgramsRuntime().Value.DrawProgramPaidHpLoss(frame, sourceBind);
    public void GrantPaidHpLoss(ProgramSkillFrame frame, string sourceBind, bool distance) => getCoreGameEnginePaidHpLossProgramsRuntime().Value.GrantProgramPaidHpLoss(frame, sourceBind, distance);
}
