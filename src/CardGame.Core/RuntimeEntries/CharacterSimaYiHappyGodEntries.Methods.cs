#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiHappyGodEntries
{
    internal bool AllowsHappySimaActivation(ProgramActivationLegalityContext g) => _resolve().AllowsHappySimaActivation(g);
    internal bool AllowsHappySimaPeach(int actor) => _resolve().AllowsHappySimaPeach(actor);
    internal bool AllowsHappySimaTrigger(ProgramTriggerLegalityContext g) => _resolve().AllowsHappySimaTrigger(g);
    internal void AnswerHappySimaFirstDraw(PromptChoice choice) => _resolve().AnswerHappySimaFirstDraw(choice);
    internal SkillProgramStepOutcome BeginHappySimaFirstDraw(ProgramSkillFrame supplied) => _resolve().BeginHappySimaFirstDraw(supplied);
    internal PromptChoice ChooseHappySimaFirst(PendingDecision p) => _resolve().ChooseHappySimaFirst(p);
    internal void CompleteHappySimaFirst(ProgramSkillFrame f) => _resolve().CompleteHappySimaFirst(f);
    internal SkillProgramStepOutcome GainHappySimaRen(ProgramSkillFrame f) => _resolve().GainHappySimaRen(f);
    internal int HappySimaDiscardCount(int owner, ProgramSkillWindowContext? context) => _resolve().HappySimaDiscardCount(owner, context);
    internal long HappySimaFirstAcceptedFrame(int owner) => _resolve().HappySimaFirstAcceptedFrame(owner);
    internal IReadOnlyList<PromptChoice> HappySimaFirstChoices(ProgramSkillFrame f) => _resolve().HappySimaFirstChoices(f);
    internal PlayerMarkerChangedEvent? HappySimaPaidRen(ProgramSkillFrame f) => _resolve().HappySimaPaidRen(f);
    internal void PublishHappySimaFirstDraw(ProgramSkillFrame f) => _resolve().PublishHappySimaFirstDraw(f);
    internal bool ResumeHappySimaFirst(long id) => _resolve().ResumeHappySimaFirst(id);
    internal bool RetainHappySimaFirst(ProgramSkillFrame f) => _resolve().RetainHappySimaFirst(f);
    internal void ReturnHappySimaFirstDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnHappySimaFirstDraw(f, outcome);
    internal bool ReturnHappySimaFirstMovement(ProgramSkillFrame f) => _resolve().ReturnHappySimaFirstMovement(f);
}
