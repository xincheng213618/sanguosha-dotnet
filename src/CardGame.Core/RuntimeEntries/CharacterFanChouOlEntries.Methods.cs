#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFanChouOlEntries
{
    internal SkillProgramStepOutcome BeginFanChou(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginFanChou(supplied, effect);
    internal bool CanContinueFanChou(ProgramSkillFrame f) => _resolve().CanContinueFanChou(f);
    internal bool CanOfferFanChou(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferFanChou(c, t, w);
    internal PromptChoice FanChouAiChoice(ProgramSkillFrame f) => _resolve().FanChouAiChoice(f);
    internal PromptChoice FanChouChoice(ProgramSkillFrame f, string step, string branch, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default, string? label = default) => _resolve().FanChouChoice(f, step, branch, cards, targets, label);
    internal IReadOnlyList<PromptChoice> FanChouChoices(ProgramSkillFrame f) => _resolve().FanChouChoices(f);
    internal IReadOnlyList<Card> FanChouFieldCards(int owner) => _resolve().FanChouFieldCards(owner);
    internal bool FanChouFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().FanChouFinalizedAfterCategoryTargetWindow(query);
    internal bool FanChouFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().FanChouFirstChild(root, child);
    internal bool FanChouNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().FanChouNormalizeImplicitDrawTwoTargetWindow(query);
    internal ProgramSkillFrame? FanChouObserverRoot() => _resolve().FanChouObserverRoot();
    internal IReadOnlyList<CardSnapshot> FanChouPrivateDeckCards(int viewer) => _resolve().FanChouPrivateDeckCards(viewer);
    internal bool FanChouSourceCurrent(ProgramSkillFrame f) => _resolve().FanChouSourceCurrent(f);
    internal void FinishFanChou(ProgramSkillFrame f) => _resolve().FinishFanChou(f);
    internal void IssueFanChouPaid(long id, FanChouReceipt r, int subject) => _resolve().IssueFanChouPaid(id, r, subject);
    internal void PayFanChouDraw(ProgramSkillFrame f) => _resolve().PayFanChouDraw(f);
    internal void PublishFanChou(ProgramSkillFrame f) => _resolve().PublishFanChou(f);
    internal void RequireFanChou(ProgramSkillFrame f) => _resolve().RequireFanChou(f);
    internal void ResolveFanChouChoice(PromptChoice choice) => _resolve().ResolveFanChouChoice(choice);
    internal bool ResumeFanChou(long id) => _resolve().ResumeFanChou(id);
    internal bool ReturnFanChouMovement(ProgramSkillFrame f) => _resolve().ReturnFanChouMovement(f);
    internal bool TryAdvanceFanChouSubtree() => _resolve().TryAdvanceFanChouSubtree();
    internal bool TryFanChouUse(int owner, ProgramSkillWindowContext context, scoped out FanChouUseProof proof) => _resolve().TryFanChouUse(owner, context, out proof);
    internal bool HasFanChouCapability { get => _resolve().HasFanChouCapability; }
}
