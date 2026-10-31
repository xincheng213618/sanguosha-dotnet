#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheLuanxinEntries
{
    internal void AssignShanheLuanxinNextDraw(global::CardGame.Core.ActualDiscardRecoveryPhaseStartedEvent started) => _resolve().AssignShanheLuanxinNextDraw(started);
    internal bool CanOfferShanheLuanxinMovement(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheLuanxinMovement(candidate, trigger, context);
    internal bool HasShanheLuanxinForeignHandDiscard(int owner, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().HasShanheLuanxinForeignHandDiscard(owner, batch);
    internal bool HasShanheLuanxinOtherHandGain(int owner, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().HasShanheLuanxinOtherHandGain(owner, batch);
    internal void QueueShanheLuanxinNextDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().QueueShanheLuanxinNextDraw(supplied);
    internal int ShanheLuanxinNextDrawBonus(global::CardGame.Core.CharacterState player) => _resolve().ShanheLuanxinNextDrawBonus(player);
}
