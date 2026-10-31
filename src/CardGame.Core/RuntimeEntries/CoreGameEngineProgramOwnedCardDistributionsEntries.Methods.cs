#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnedCardDistributionsEntries
{
    internal void AssertProgramOwnedCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertProgramOwnedCardDistribution(frame, paused);
    internal global::CardGame.Core.SkillProgramStepOutcome DistributeProgramOwnedCards(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, string sourceBind, global::CardGame.Core.SkillProgramTargetKind targetKind, bool allowDeclineBeforeFirst, global::CardGame.Core.CardMoveReason reason) => _resolve().DistributeProgramOwnedCards(frame, zones, sourceBind, targetKind, allowDeclineBeforeFirst, reason);
    internal void PublishProgramOwnedCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMoveReason reason) => _resolve().PublishProgramOwnedCardDistribution(frame, reason);
    internal void ResolveProgramOwnedCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramOwnedCardDistribution(frame, effect, selected);
    internal global::CardGame.Core.PromptChoice SelectAiProgramOwnedCardDistribution(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiProgramOwnedCardDistribution(decision, frame);
}
