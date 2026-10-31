#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnedCardDistributionsEntries
{
    internal SkillProgramStepOutcome DistributeProgramOwnedCards(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, string sourceBind, SkillProgramTargetKind targetKind, bool allowDeclineBeforeFirst, CardMoveReason reason) => _resolve().DistributeProgramOwnedCards(frame, zones, sourceBind, targetKind, allowDeclineBeforeFirst, reason);
    internal void PublishProgramOwnedCardDistribution(ProgramSkillFrame frame, CardMoveReason reason) => _resolve().PublishProgramOwnedCardDistribution(frame, reason);
    internal void ResolveProgramOwnedCardDistribution(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected) => _resolve().ResolveProgramOwnedCardDistribution(frame, effect, selected);
    internal PromptChoice SelectAiProgramOwnedCardDistribution(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramOwnedCardDistribution(decision, frame);
}
