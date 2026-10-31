#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardDistributionPresentationEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PresentBoundCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string sourceBind, global::CardGame.Core.SkillProgramTargetKind targetKind) => _resolve().PresentBoundCardDistribution(frame, choices, sourceBind, targetKind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PresentCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string stage, int maximum = 2147483647, global::System.Func<global::CardGame.Core.PromptChoice, string>? destination = default, global::System.Func<global::CardGame.Core.PromptChoice, string>? label = default, int? scopeInstruction = default) => _resolve().PresentCardDistribution(frame, choices, stage, maximum, destination, label, scopeInstruction);
}
