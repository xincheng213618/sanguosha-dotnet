#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingMobileGodEntries
{
    internal bool AllowsMobileCampClaim(ProgramTriggerLegalityContext gate) => _resolve().AllowsMobileCampClaim(gate);
    internal SkillProgramStepOutcome ClaimMobileCampHand(ProgramSkillFrame supplied) => _resolve().ClaimMobileCampHand(supplied);
    internal bool IsMobileCampClaimContext(ProgramSkillFrame frame) => _resolve().IsMobileCampClaimContext(frame);
    internal bool RetainMobileCampClaim(ProgramSkillFrame frame) => _resolve().RetainMobileCampClaim(frame);
    internal bool ReturnMobileCampClaim(ProgramSkillFrame frame) => _resolve().ReturnMobileCampClaim(frame);
    internal bool ValidMobileCampClaim(ProgramSkillFrame frame) => _resolve().ValidMobileCampClaim(frame);
}
