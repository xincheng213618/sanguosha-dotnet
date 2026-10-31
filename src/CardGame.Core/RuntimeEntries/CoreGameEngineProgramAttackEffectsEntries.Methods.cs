#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAttackEffectsEntries
{
    internal bool IsProgramSlashRedirectBaseTarget(NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat) => _resolve().IsProgramSlashRedirectBaseTarget(attack, redirectorSeat, targetSeat);
    internal bool IsProgramSlashRedirectTarget(NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat, bool paymentCompleted = false) => _resolve().IsProgramSlashRedirectTarget(attack, redirectorSeat, targetSeat, paymentCompleted);
    internal void ProhibitCurrentProgramResponse(ProgramSkillFrame frame) => _resolve().ProhibitCurrentProgramResponse(frame);
    internal void RedirectCurrentProgramAttack(ProgramSkillFrame frame, int targetSeat) => _resolve().RedirectCurrentProgramAttack(frame, targetSeat);
}
