#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFanChouOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFanChou(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginFanChou(supplied, effect);
    internal bool CanContinueFanChou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueFanChou(f);
    internal bool CanOfferFanChou(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferFanChou(c, t, w);
    internal global::CardGame.Core.PromptChoice FanChouAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FanChouAiChoice(f);
    internal global::CardGame.Core.PromptChoice FanChouChoice(global::CardGame.Core.ProgramSkillFrame f, string step, string branch, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default, string? label = default) => _resolve().FanChouChoice(f, step, branch, cards, targets, label);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FanChouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FanChouChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> FanChouFieldCards(int owner) => _resolve().FanChouFieldCards(owner);
    internal bool FanChouFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().FanChouFinalizedAfterCategoryTargetWindow(query);
    internal bool FanChouFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().FanChouFirstChild(root, child);
    internal bool FanChouNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().FanChouNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.ProgramSkillFrame? FanChouObserverRoot() => _resolve().FanChouObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> FanChouPrivateDeckCards(int viewer) => _resolve().FanChouPrivateDeckCards(viewer);
    internal bool FanChouSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FanChouSourceCurrent(f);
    internal void FinishFanChou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishFanChou(f);
    internal bool HasFanChouBeforeDamageObserver(long id) => _resolve().HasFanChouBeforeDamageObserver(id);
    internal bool HasFanChouDamageObserver(long id) => _resolve().HasFanChouDamageObserver(id);
    internal bool IsFanChouMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsFanChouMovement(f, e, p);
    internal void IssueFanChouPaid(long id, global::CardGame.Core.FanChouReceipt r, int subject) => _resolve().IssueFanChouPaid(id, r, subject);
    internal void PayFanChouDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayFanChouDraw(f);
    internal void PublishFanChou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishFanChou(f);
    internal void RequireFanChou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireFanChou(f);
    internal void ResolveFanChouChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveFanChouChoice(choice);
    internal bool ResumeFanChou(long id) => _resolve().ResumeFanChou(id);
    internal bool ReturnFanChouMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFanChouMovement(f);
    internal bool TryAdvanceFanChouSubtree() => _resolve().TryAdvanceFanChouSubtree();
    internal bool TryFanChouUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.FanChouUseProof proof) => _resolve().TryFanChouUse(owner, context, out proof);
    internal bool HasFanChouCapability { get => _resolve().HasFanChouCapability; }
}
