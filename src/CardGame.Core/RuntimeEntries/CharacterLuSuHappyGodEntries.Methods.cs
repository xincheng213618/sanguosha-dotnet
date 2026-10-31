#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuHappyGodEntries
{
    internal global::CardGame.Core.PromptChoice AiHappyDingzhou(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AiHappyDingzhou(decision, frame);
    internal void AnswerHappyDingzhou(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerHappyDingzhou(answer);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyDingzhou(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHappyDingzhou(supplied);
    internal void ContinueHappyDingzhou(long id) => _resolve().ContinueHappyDingzhou(id);
    internal void FinishHappyDingzhou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishHappyDingzhou(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyDingzhouChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HappyDingzhouChoices(frame);
    internal bool HappyDingzhouHandDestination(global::CardGame.Core.ShenLuSuMoveInvoice invoice, int recipient, bool allowGeneralWeapon = false) => _resolve().HappyDingzhouHandDestination(invoice, recipient, allowGeneralWeapon);
    internal bool HappyDingzhouMovementProof(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().HappyDingzhouMovementProof(frame, effect, pending);
    internal bool KeepHappyDingzhouPaid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepHappyDingzhouPaid(frame);
    internal void MoveHappyDingzhou(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<int> cards, global::CardGame.Core.HappyDingzhouStage stage, global::CardGame.Core.CardLocation destination) => _resolve().MoveHappyDingzhou(frame, cards, stage, destination);
    internal void PublishHappyDingzhou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishHappyDingzhou(frame);
    internal bool ResumeHappyDingzhou(long id) => _resolve().ResumeHappyDingzhou(id);
    internal bool ReturnHappyDingzhouMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnHappyDingzhouMovement(frame);
}
