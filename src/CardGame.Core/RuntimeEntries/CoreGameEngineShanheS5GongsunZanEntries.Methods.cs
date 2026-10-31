#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5GongsunZanEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheS5Qiaomeng(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheS5Qiaomeng(supplied);
    internal bool CanOfferShanheS5Gongsun(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheS5Gongsun(candidate, trigger, context);
    internal void PublishShanheS5Qiaomeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShanheS5Qiaomeng(f);
    internal void ResolveShanheS5QiaomengChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveShanheS5QiaomengChoice(choice);
    internal bool ResumeShanheS5Qiaomeng(long id) => _resolve().ResumeShanheS5Qiaomeng(id);
    internal bool ReturnShanheS5QiaomengMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShanheS5QiaomengMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiShanheS5Qiaomeng(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiShanheS5Qiaomeng(decision, f);
    internal global::CardGame.Core.Card[] ShanheS5QiaomengCards(int owner, int target) => _resolve().ShanheS5QiaomengCards(owner, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShanheS5QiaomengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShanheS5QiaomengChoices(f);
}
