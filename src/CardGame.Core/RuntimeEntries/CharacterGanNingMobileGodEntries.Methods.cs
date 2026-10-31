#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingMobileGodEntries
{
    internal bool AllowsMobileCampClaim(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsMobileCampClaim(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome ClaimMobileCampHand(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ClaimMobileCampHand(supplied);
    internal bool IsMobileCampClaimContext(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsMobileCampClaimContext(frame);
    internal bool RetainMobileCampClaim(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RetainMobileCampClaim(frame);
    internal bool ReturnMobileCampClaim(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnMobileCampClaim(frame);
    internal bool ValidMobileCampClaim(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidMobileCampClaim(frame);
}
