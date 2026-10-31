#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLostHpOwnedGiftsEntries
{
    internal SkillProgramStepOutcome AwaitLostHpOwnedGiftMovements(long frameId) => _resolve().AwaitLostHpOwnedGiftMovements(frameId);
    internal void FinishLostHpOwnedGift(ProgramSkillFrame frame) => _resolve().FinishLostHpOwnedGift(frame);
    internal bool IsLostHpOwnedGiftProgramDying() => _resolve().IsLostHpOwnedGiftProgramDying();
    internal bool IsValidLostHpOwnedGift(ProgramSkillFrame root) => _resolve().IsValidLostHpOwnedGift(root);
    internal ProgramSkillFrame? LostHpOwnedGiftObserverRoot() => _resolve().LostHpOwnedGiftObserverRoot();
    internal SkillProgramStepOutcome PublishLostHpOwnedGift(ProgramSkillFrame frame) => _resolve().PublishLostHpOwnedGift(frame);
    internal void ResolveLostHpOwnedGift(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice choice) => _resolve().ResolveLostHpOwnedGift(frame, effect, choice);
    internal SkillProgramStepOutcome RunLostHpOwnedGift(ProgramSkillFrame frame) => _resolve().RunLostHpOwnedGift(frame);
    internal PromptChoice SelectAiLostHpOwnedGift(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiLostHpOwnedGift(decision, frame);
    internal long PaidTargetMovementSequence { get => _resolve().PaidTargetMovementSequence; }
}
