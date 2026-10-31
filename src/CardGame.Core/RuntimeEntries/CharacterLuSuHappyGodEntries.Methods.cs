#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuHappyGodEntries
{
    internal PromptChoice AiHappyDingzhou(PendingDecision decision, ProgramSkillFrame frame) => _resolve().AiHappyDingzhou(decision, frame);
    internal void AnswerHappyDingzhou(PromptChoice answer) => _resolve().AnswerHappyDingzhou(answer);
    internal SkillProgramStepOutcome BeginHappyDingzhou(ProgramSkillFrame supplied) => _resolve().BeginHappyDingzhou(supplied);
    internal void ContinueHappyDingzhou(long id) => _resolve().ContinueHappyDingzhou(id);
    internal void FinishHappyDingzhou(ProgramSkillFrame frame) => _resolve().FinishHappyDingzhou(frame);
    internal IReadOnlyList<PromptChoice> HappyDingzhouChoices(ProgramSkillFrame frame) => _resolve().HappyDingzhouChoices(frame);
    internal bool KeepHappyDingzhouPaid(ProgramSkillFrame frame) => _resolve().KeepHappyDingzhouPaid(frame);
    internal void MoveHappyDingzhou(ProgramSkillFrame frame, IReadOnlyList<int> cards, HappyDingzhouStage stage, CardLocation destination) => _resolve().MoveHappyDingzhou(frame, cards, stage, destination);
    internal void PublishHappyDingzhou(ProgramSkillFrame frame) => _resolve().PublishHappyDingzhou(frame);
    internal bool ResumeHappyDingzhou(long id) => _resolve().ResumeHappyDingzhou(id);
    internal bool ReturnHappyDingzhouMovement(ProgramSkillFrame frame) => _resolve().ReturnHappyDingzhouMovement(frame);
}
