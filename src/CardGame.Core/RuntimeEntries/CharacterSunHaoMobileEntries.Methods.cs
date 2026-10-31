#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunHaoMobileEntries
{
    internal SkillProgramStepOutcome BeginMobileSunHaoDraw(ProgramSkillFrame frame) => _resolve().BeginMobileSunHaoDraw(frame);
    internal void ContinueMobileSunHaoDraw(long id) => _resolve().ContinueMobileSunHaoDraw(id);
    internal bool HasQualifiedMobileGuiming(CharacterState owner) => _resolve().HasQualifiedMobileGuiming(owner);
    internal bool IsMobileSunHaoDrawMovement(ProgramSkillFrame frame, CardMovementRecord movement) => _resolve().IsMobileSunHaoDrawMovement(frame, movement);
    internal bool MobileSunHaoDrawFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().MobileSunHaoDrawFirstChild(frame, child);
    internal bool ResumeMobileSunHaoDraw(long id) => _resolve().ResumeMobileSunHaoDraw(id);
    internal void ReturnMobileSunHaoDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnMobileSunHaoDraw(supplied, outcome);
    internal bool ReturnMobileSunHaoDrawMovement(ProgramSkillFrame frame) => _resolve().ReturnMobileSunHaoDrawMovement(frame);
}
