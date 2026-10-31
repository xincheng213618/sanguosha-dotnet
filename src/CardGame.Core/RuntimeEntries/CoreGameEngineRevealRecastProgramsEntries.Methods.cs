#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRevealRecastProgramsEntries
{
    internal void DrawProgramCompletedCardParticipants(global::CardGame.Core.ProgramSkillFrame frame, string stateId, int threshold) => _resolve().DrawProgramCompletedCardParticipants(frame, stateId, threshold);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PairedHandRevealChoices(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().PairedHandRevealChoices(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome RecastProgramSelectedCards(global::CardGame.Core.ProgramSkillFrame frame, string stateId, int threshold) => _resolve().RecastProgramSelectedCards(frame, stateId, threshold);
    internal void RecordCompletedCardDamageParticipant(long cardUseFrameId, int targetSeat) => _resolve().RecordCompletedCardDamageParticipant(cardUseFrameId, targetSeat);
    internal void ResolvePairedHandRevealChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolvePairedHandRevealChoice(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome RevealProgramSelectedHandAgainstTarget(global::CardGame.Core.ProgramSkillFrame frame, string resultBind) => _resolve().RevealProgramSelectedHandAgainstTarget(frame, resultBind);
}
