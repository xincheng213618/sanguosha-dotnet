#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheLuanxinEntries
{
    internal void AssignShanheLuanxinNextDraw(ActualDiscardRecoveryPhaseStartedEvent started) => _resolve().AssignShanheLuanxinNextDraw(started);
    internal bool CanOfferShanheLuanxinMovement(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheLuanxinMovement(candidate, trigger, context);
    internal bool HasShanheLuanxinForeignHandDiscard(int owner, CardMovementBatchContext batch) => _resolve().HasShanheLuanxinForeignHandDiscard(owner, batch);
    internal bool HasShanheLuanxinOtherHandGain(int owner, CardMovementBatchContext batch) => _resolve().HasShanheLuanxinOtherHandGain(owner, batch);
    internal void QueueShanheLuanxinNextDraw(ProgramSkillFrame supplied) => _resolve().QueueShanheLuanxinNextDraw(supplied);
    internal int ShanheLuanxinNextDrawBonus(CharacterState player) => _resolve().ShanheLuanxinNextDrawBonus(player);
}
