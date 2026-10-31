#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUsePaymentsEntries
{
    internal void AssertCompletedUsePayment(ProgramSkillFrame f) => _resolve().AssertCompletedUsePayment(f);
    internal SkillProgramStepOutcome BeginCompletedUsePayment(ProgramSkillFrame input) => _resolve().BeginCompletedUsePayment(input);
    internal void PublishCompletedUsePayment(ProgramSkillFrame f) => _resolve().PublishCompletedUsePayment(f);
    internal void ResolveCompletedUsePayment(PromptChoice choice) => _resolve().ResolveCompletedUsePayment(choice);
    internal bool ResumeCompletedUsePayment(long id) => _resolve().ResumeCompletedUsePayment(id);
    internal PromptChoice SelectAiCompletedUsePayment(PendingDecision decision) => _resolve().SelectAiCompletedUsePayment(decision);
}
