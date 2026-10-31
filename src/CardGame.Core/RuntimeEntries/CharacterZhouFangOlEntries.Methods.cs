#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFangOlEntries
{
    internal bool CanStartDuanfaRecycle(CharacterState owner, string skillId) => _resolve().CanStartDuanfaRecycle(owner, skillId);
    internal int DuanfaDiscardedThisPhase(int ownerSeat, string skillId) => _resolve().DuanfaDiscardedThisPhase(ownerSeat, skillId);
    internal SkillProgramStepOutcome DuanfaProgramDiscardAndDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().DuanfaProgramDiscardAndDraw(frame, effect);
    internal void PresentYoudiBaitPrompt(ProgramSkillFrame frame, int chooserSeat) => _resolve().PresentYoudiBaitPrompt(frame, chooserSeat);
    internal void ResolveYoudiBaitChoice(PromptChoice selected) => _resolve().ResolveYoudiBaitChoice(selected);
    internal SkillProgramStepOutcome YoudiProgramBaitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YoudiProgramBaitDiscard(frame, effect);
}
