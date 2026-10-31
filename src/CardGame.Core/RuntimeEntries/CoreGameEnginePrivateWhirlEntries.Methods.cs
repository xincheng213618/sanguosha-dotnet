#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateWhirlEntries
{
    internal SkillProgramStepOutcome BeginPrivateWhirl(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginPrivateWhirl(supplied, effect);
    internal bool CanContinuePrivateWhirl(ProgramSkillFrame f) => _resolve().CanContinuePrivateWhirl(f);
    internal bool CanOfferPrivateWhirl(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferPrivateWhirl(candidate, trigger, context);
    internal void CleanupLostPrivateWhirlPiles() => _resolve().CleanupLostPrivateWhirlPiles();
    internal void ContinueWhirl(ProgramSkillFrame f) => _resolve().ContinueWhirl(f);
    internal bool DrainWhirl(ProgramSkillFrame f) => _resolve().DrainWhirl(f);
    internal void DrawWhirlSegment(ProgramSkillFrame f, int amount, PrivateWhirlStage stage) => _resolve().DrawWhirlSegment(f, amount, stage);
    internal void FinishWhirl(ProgramSkillFrame f) => _resolve().FinishWhirl(f);
    internal void IssueWhirlSegment(ProgramSkillFrame f, int count) => _resolve().IssueWhirlSegment(f, count);
    internal PrivateWhirlDeposit[] LiveWhirl(int owner, string? skill = default, string? instance = default) => _resolve().LiveWhirl(owner, skill, instance);
    internal void PayWhirlRemoval(ProgramSkillFrame f, IReadOnlyList<PrivateWhirlDeposit> deposits, string reason) => _resolve().PayWhirlRemoval(f, deposits, reason);
    internal IReadOnlyList<PrivateWhirlPileSnapshot>? ProjectPrivateWhirlPiles(int owner, int viewer, bool revealAll) => _resolve().ProjectPrivateWhirlPiles(owner, viewer, revealAll);
    internal void PublishWhirlChoice(ProgramSkillFrame f) => _resolve().PublishWhirlChoice(f);
    internal void RequirePrivateWhirl(ProgramSkillFrame f) => _resolve().RequirePrivateWhirl(f);
    internal void ResolvePrivateWhirlChoice(PromptChoice choice) => _resolve().ResolvePrivateWhirlChoice(choice);
    internal bool ResumePrivateWhirl(long id) => _resolve().ResumePrivateWhirl(id);
    internal bool ReturnPrivateWhirlMovement(ProgramSkillFrame f) => _resolve().ReturnPrivateWhirlMovement(f);
    internal PromptChoice WhirlChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = default) => _resolve().WhirlChoice(f, branch, label, cards);
    internal IReadOnlyList<PromptChoice> WhirlChoices(ProgramSkillFrame f) => _resolve().WhirlChoices(f);
    internal bool WhirlSourceCurrent(ProgramSkillFrame f) => _resolve().WhirlSourceCurrent(f);
    internal bool HasPrivateWhirlCapability { get => _resolve().HasPrivateWhirlCapability; }
}
