#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiHappyGodEntries
{
    internal bool AllowsHappySimaActivation(global::CardGame.Core.ProgramActivationLegalityContext g) => _resolve().AllowsHappySimaActivation(g);
    internal bool AllowsHappySimaPeach(int actor) => _resolve().AllowsHappySimaPeach(actor);
    internal bool AllowsHappySimaTrigger(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().AllowsHappySimaTrigger(g);
    internal void AnswerHappySimaFirstDraw(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHappySimaFirstDraw(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappySimaFirstDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHappySimaFirstDraw(supplied);
    internal global::CardGame.Core.PromptChoice ChooseHappySimaFirst(global::CardGame.Core.PendingDecision p) => _resolve().ChooseHappySimaFirst(p);
    internal void CompleteHappySimaFirst(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteHappySimaFirst(f);
    internal global::CardGame.Core.SkillProgramStepOutcome GainHappySimaRen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GainHappySimaRen(f);
    internal int HappySimaDiscardCount(int owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().HappySimaDiscardCount(owner, context);
    internal long HappySimaFirstAcceptedFrame(int owner) => _resolve().HappySimaFirstAcceptedFrame(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappySimaFirstChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappySimaFirstChoices(f);
    internal global::CardGame.Core.PlayerMarkerChangedEvent? HappySimaPaidRen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappySimaPaidRen(f);
    internal bool ProveHappySimaFirst(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation continuation) => _resolve().ProveHappySimaFirst(f, effect, continuation);
    internal void PublishHappySimaFirstDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHappySimaFirstDraw(f);
    internal bool ResumeHappySimaFirst(long id) => _resolve().ResumeHappySimaFirst(id);
    internal bool RetainHappySimaFirst(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHappySimaFirst(f);
    internal void ReturnHappySimaFirstDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappySimaFirstDraw(f, outcome);
    internal bool ReturnHappySimaFirstMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappySimaFirstMovement(f);
    internal bool ValidHappySimaDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappySimaDraw(f);
}
