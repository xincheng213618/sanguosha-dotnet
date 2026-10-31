#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5GongsunZanEntries
{
    internal SkillProgramStepOutcome BeginShanheS5Qiaomeng(ProgramSkillFrame supplied) => _resolve().BeginShanheS5Qiaomeng(supplied);
    internal bool CanOfferShanheS5Gongsun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheS5Gongsun(candidate, trigger, context);
    internal void PublishShanheS5Qiaomeng(ProgramSkillFrame f) => _resolve().PublishShanheS5Qiaomeng(f);
    internal void ResolveShanheS5QiaomengChoice(PromptChoice choice) => _resolve().ResolveShanheS5QiaomengChoice(choice);
    internal bool ResumeShanheS5Qiaomeng(long id) => _resolve().ResumeShanheS5Qiaomeng(id);
    internal bool ReturnShanheS5QiaomengMovement(ProgramSkillFrame f) => _resolve().ReturnShanheS5QiaomengMovement(f);
    internal PromptChoice SelectAiShanheS5Qiaomeng(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiShanheS5Qiaomeng(decision, f);
    internal Card[] ShanheS5QiaomengCards(int owner, int target) => _resolve().ShanheS5QiaomengCards(owner, target);
    internal IReadOnlyList<PromptChoice> ShanheS5QiaomengChoices(ProgramSkillFrame f) => _resolve().ShanheS5QiaomengChoices(f);
}
