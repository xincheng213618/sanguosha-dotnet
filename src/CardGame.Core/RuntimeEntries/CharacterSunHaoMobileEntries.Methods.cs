#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunHaoMobileEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileSunHaoDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginMobileSunHaoDraw(frame);
    internal void ContinueMobileSunHaoDraw(long id) => _resolve().ContinueMobileSunHaoDraw(id);
    internal bool HasQualifiedMobileGuiming(global::CardGame.Core.CharacterState owner) => _resolve().HasQualifiedMobileGuiming(owner);
    internal bool IsMobileSunHaoDrawMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementRecord movement) => _resolve().IsMobileSunHaoDrawMovement(frame, movement);
    internal bool MobileSunHaoDrawFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().MobileSunHaoDrawFirstChild(frame, child);
    internal bool ResumeMobileSunHaoDraw(long id) => _resolve().ResumeMobileSunHaoDraw(id);
    internal void ReturnMobileSunHaoDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileSunHaoDraw(supplied, outcome);
    internal bool ReturnMobileSunHaoDrawMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnMobileSunHaoDrawMovement(frame);
}
