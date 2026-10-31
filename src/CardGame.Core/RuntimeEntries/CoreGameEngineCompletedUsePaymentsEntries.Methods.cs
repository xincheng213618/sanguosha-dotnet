#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUsePaymentsEntries
{
    internal void AssertCompletedUsePayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertCompletedUsePayment(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCompletedUsePayment(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginCompletedUsePayment(input);
    internal void PublishCompletedUsePayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCompletedUsePayment(f);
    internal void ResolveCompletedUsePayment(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveCompletedUsePayment(choice);
    internal bool ResumeCompletedUsePayment(long id) => _resolve().ResumeCompletedUsePayment(id);
    internal global::CardGame.Core.PromptChoice SelectAiCompletedUsePayment(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiCompletedUsePayment(decision);
}
