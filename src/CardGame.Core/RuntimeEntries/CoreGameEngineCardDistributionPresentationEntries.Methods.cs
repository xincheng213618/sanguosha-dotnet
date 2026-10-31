#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardDistributionPresentationEntries
{
    internal IReadOnlyList<PromptChoice> PresentBoundCardDistribution(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string sourceBind, SkillProgramTargetKind targetKind) => _resolve().PresentBoundCardDistribution(frame, choices, sourceBind, targetKind);
    internal IReadOnlyList<PromptChoice> PresentCardDistribution(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string stage, int maximum = 2147483647, Func<PromptChoice, string>? destination = default, Func<PromptChoice, string>? label = default, int? scopeInstruction = default) => _resolve().PresentCardDistribution(frame, choices, stage, maximum, destination, label, scopeInstruction);
}
