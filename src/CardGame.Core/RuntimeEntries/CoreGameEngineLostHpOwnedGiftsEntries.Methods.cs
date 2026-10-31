#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLostHpOwnedGiftsEntries
{
    internal void AssertLostHpOwnedGifts() => _resolve().AssertLostHpOwnedGifts();
    internal global::CardGame.Core.SkillProgramStepOutcome AwaitLostHpOwnedGiftMovements(long frameId) => _resolve().AwaitLostHpOwnedGiftMovements(frameId);
    internal void FinishLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishLostHpOwnedGift(frame);
    internal bool IsLostHpOwnedGiftProgramDying() => _resolve().IsLostHpOwnedGiftProgramDying();
    internal bool IsValidLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame root) => _resolve().IsValidLostHpOwnedGift(root);
    internal global::CardGame.Core.ProgramSkillFrame? LostHpOwnedGiftObserverRoot() => _resolve().LostHpOwnedGiftObserverRoot();
    internal global::CardGame.Core.SkillProgramStepOutcome PublishLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishLostHpOwnedGift(frame);
    internal void ResolveLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice choice) => _resolve().ResolveLostHpOwnedGift(frame, effect, choice);
    internal global::CardGame.Core.SkillProgramStepOutcome RunLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RunLostHpOwnedGift(frame);
    internal global::CardGame.Core.PromptChoice SelectAiLostHpOwnedGift(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiLostHpOwnedGift(decision, frame);
    internal long PaidTargetMovementSequence { get => _resolve().PaidTargetMovementSequence; }
}
