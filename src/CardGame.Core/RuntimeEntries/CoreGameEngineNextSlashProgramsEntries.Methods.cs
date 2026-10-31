#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextSlashProgramsEntries
{
    internal void DrawProgramRecoveryReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrawProgramRecoveryReceipt(frame);
    internal void ExpireNextSlashDamage(int turnNumber, int turnSeat) => _resolve().ExpireNextSlashDamage(turnNumber, turnSeat);
    internal void FreezeNextSlashDamage(long frameId) => _resolve().FreezeNextSlashDamage(frameId);
    internal int FrozenNextSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().FrozenNextSlashDamage(attack);
    internal void RecoverProgramToMaximum(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RecoverProgramToMaximum(frame);
    internal void ReserveProgramNextSlashDamage(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReserveProgramNextSlashDamage(frame);
    internal bool HasNextSlashDamageCapability { get => _resolve().HasNextSlashDamageCapability; }
}
