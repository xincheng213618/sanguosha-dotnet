#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunJianBoundaryMobileEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileSunJianPolu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMobileSunJianPolu(supplied);
    internal void ContinueMobileSunJianPolu(long id) => _resolve().ContinueMobileSunJianPolu(id);
    internal bool KeepMobileSunJianPolu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepMobileSunJianPolu(frame);
    internal int MobileSunJianInvocationCount(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MobileSunJianInvocationCount(frame);
    internal bool ProveMobileSunJianPoluMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().ProveMobileSunJianPoluMovement(frame, effect, pending);
    internal bool ProveMobileSunJianPoluRecovery(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ProveMobileSunJianPoluRecovery(frame, recovery);
    internal bool ResumeMobileSunJianPolu(long id) => _resolve().ResumeMobileSunJianPolu(id);
    internal void ReturnMobileSunJianPoluDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileSunJianPoluDraw(frame, outcome);
    internal bool ReturnMobileSunJianPoluMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnMobileSunJianPoluMovement(frame);
    internal bool ValidMobileSunJianPolu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidMobileSunJianPolu(frame);
}
