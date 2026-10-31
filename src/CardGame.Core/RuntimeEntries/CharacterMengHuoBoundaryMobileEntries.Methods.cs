#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMengHuoBoundaryMobileEntries
{
    internal bool CanMobileOrOlRedDiscardRecoverySelect(global::CardGame.Core.ProgramSkillFrame frame, int seat) => _resolve().CanMobileOrOlRedDiscardRecoverySelect(frame, seat);
    internal bool MobileMengHuoZaiqiDrawMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().MobileMengHuoZaiqiDrawMovement(frame, effect, pending);
    internal bool ValidMobileOrOlRedDiscardRecoveryWindow(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidMobileOrOlRedDiscardRecoveryWindow(frame);
}
