#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRevealRecastProgramsEntries
{
    internal void DrawProgramCompletedCardParticipants(ProgramSkillFrame frame, string stateId, int threshold) => _resolve().DrawProgramCompletedCardParticipants(frame, stateId, threshold);
    internal IReadOnlyList<PromptChoice> PairedHandRevealChoices(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().PairedHandRevealChoices(frame, effect);
    internal SkillProgramStepOutcome RecastProgramSelectedCards(ProgramSkillFrame frame, string stateId, int threshold) => _resolve().RecastProgramSelectedCards(frame, stateId, threshold);
    internal void RecordCompletedCardDamageParticipant(long cardUseFrameId, int targetSeat) => _resolve().RecordCompletedCardDamageParticipant(cardUseFrameId, targetSeat);
    internal void ResolvePairedHandRevealChoice(PromptChoice selected) => _resolve().ResolvePairedHandRevealChoice(selected);
    internal SkillProgramStepOutcome RevealProgramSelectedHandAgainstTarget(ProgramSkillFrame frame, string resultBind) => _resolve().RevealProgramSelectedHandAgainstTarget(frame, resultBind);
}
