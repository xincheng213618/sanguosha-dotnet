#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFangOlEntries
{
    internal bool CanStartDuanfaRecycle(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().CanStartDuanfaRecycle(owner, skillId);
    internal int DuanfaDiscardedThisPhase(int ownerSeat, string skillId) => _resolve().DuanfaDiscardedThisPhase(ownerSeat, skillId);
    internal global::CardGame.Core.SkillProgramStepOutcome DuanfaProgramDiscardAndDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().DuanfaProgramDiscardAndDraw(frame, effect);
    internal void PresentYoudiBaitPrompt(global::CardGame.Core.ProgramSkillFrame frame, int chooserSeat) => _resolve().PresentYoudiBaitPrompt(frame, chooserSeat);
    internal void ResolveYoudiBaitChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveYoudiBaitChoice(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome YoudiProgramBaitDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YoudiProgramBaitDiscard(frame, effect);
}
