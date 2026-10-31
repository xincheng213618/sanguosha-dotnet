#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunJianBoundaryMobileEntries
{
    internal SkillProgramStepOutcome BeginMobileSunJianPolu(ProgramSkillFrame supplied) => _resolve().BeginMobileSunJianPolu(supplied);
    internal void ContinueMobileSunJianPolu(long id) => _resolve().ContinueMobileSunJianPolu(id);
    internal bool KeepMobileSunJianPolu(ProgramSkillFrame frame) => _resolve().KeepMobileSunJianPolu(frame);
    internal int MobileSunJianInvocationCount(ProgramSkillFrame frame) => _resolve().MobileSunJianInvocationCount(frame);
    internal bool ProveMobileSunJianPoluRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().ProveMobileSunJianPoluRecovery(frame, recovery);
    internal bool ResumeMobileSunJianPolu(long id) => _resolve().ResumeMobileSunJianPolu(id);
    internal void ReturnMobileSunJianPoluDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnMobileSunJianPoluDraw(frame, outcome);
    internal bool ReturnMobileSunJianPoluMovement(ProgramSkillFrame frame) => _resolve().ReturnMobileSunJianPoluMovement(frame);
    internal bool ValidMobileSunJianPolu(ProgramSkillFrame frame) => _resolve().ValidMobileSunJianPolu(frame);
}
