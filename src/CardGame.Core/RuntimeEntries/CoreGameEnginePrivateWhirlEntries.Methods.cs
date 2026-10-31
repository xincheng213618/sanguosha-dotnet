#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateWhirlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPrivateWhirl(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginPrivateWhirl(supplied, effect);
    internal bool CanContinuePrivateWhirl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePrivateWhirl(f);
    internal bool CanOfferPrivateWhirl(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferPrivateWhirl(candidate, trigger, context);
    internal void CleanupLostPrivateWhirlPiles() => _resolve().CleanupLostPrivateWhirlPiles();
    internal void ContinueWhirl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueWhirl(f);
    internal bool DrainWhirl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainWhirl(f);
    internal void DrawWhirlSegment(global::CardGame.Core.ProgramSkillFrame f, int amount, global::CardGame.Core.PrivateWhirlStage stage) => _resolve().DrawWhirlSegment(f, amount, stage);
    internal void FinishWhirl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWhirl(f);
    internal bool IsPrivateWhirlMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPrivateWhirlMovement(f, effect, pending);
    internal void IssueWhirlSegment(global::CardGame.Core.ProgramSkillFrame f, int count) => _resolve().IssueWhirlSegment(f, count);
    internal global::CardGame.Core.PrivateWhirlDeposit[] LiveWhirl(int owner, string? skill = default, string? instance = default) => _resolve().LiveWhirl(owner, skill, instance);
    internal void PayWhirlRemoval(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrivateWhirlDeposit> deposits, string reason) => _resolve().PayWhirlRemoval(f, deposits, reason);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrivateWhirlPileSnapshot>? ProjectPrivateWhirlPiles(int owner, int viewer, bool revealAll) => _resolve().ProjectPrivateWhirlPiles(owner, viewer, revealAll);
    internal void PublishWhirlChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWhirlChoice(f);
    internal void RequirePrivateWhirl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequirePrivateWhirl(f);
    internal void ResolvePrivateWhirlChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePrivateWhirlChoice(choice);
    internal bool ResumePrivateWhirl(long id) => _resolve().ResumePrivateWhirl(id);
    internal bool ReturnPrivateWhirlMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPrivateWhirlMovement(f);
    internal global::CardGame.Core.PromptChoice WhirlChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default) => _resolve().WhirlChoice(f, branch, label, cards);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WhirlChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WhirlChoices(f);
    internal bool WhirlSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WhirlSourceCurrent(f);
    internal bool HasPrivateWhirlCapability { get => _resolve().HasPrivateWhirlCapability; }
}
