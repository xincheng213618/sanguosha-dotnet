#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAttackEffectsEntries
{
    internal bool IsProgramSlashRedirectBaseTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat) => _resolve().IsProgramSlashRedirectBaseTarget(attack, redirectorSeat, targetSeat);
    internal bool IsProgramSlashRedirectTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat, bool paymentCompleted = false) => _resolve().IsProgramSlashRedirectTarget(attack, redirectorSeat, targetSeat, paymentCompleted);
    internal void ProhibitCurrentProgramResponse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ProhibitCurrentProgramResponse(frame);
    internal void RedirectCurrentProgramAttack(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().RedirectCurrentProgramAttack(frame, targetSeat);
}
