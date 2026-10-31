#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategoryMarksEntries
{
    internal bool CanContinueRecipientCategoryMark(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueRecipientCategoryMark(f);
    internal bool CanOfferRecipientCategoryMarkProgram(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferRecipientCategoryMarkProgram(c, trigger, context);
    internal void ChangeRecipientCategoryMarker(global::CardGame.Core.ProgramSkillFrame f, int holder, global::CardGame.Core.RecipientCategoryMarkKind kind, int delta) => _resolve().ChangeRecipientCategoryMarker(f, holder, kind, delta);
    internal void CompleteRecipientCategoryMark(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteRecipientCategoryMark(f);
    internal void ContinueRecipientCategoryPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueRecipientCategoryPublicReveal(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteRecipientCategoryMark(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect e) => _resolve().ExecuteRecipientCategoryMark(supplied, e);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeRecipientCategoryGiftObservers(global::CardGame.Core.CharacterState drawOwner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => _resolve().IncludeRecipientCategoryGiftObservers(drawOwner, existing);
    internal int IndependentRecipientCategoryMarkerCount(global::CardGame.Core.CharacterState holder, global::CardGame.Core.PlayerMarkerKind marker, int source) => _resolve().IndependentRecipientCategoryMarkerCount(holder, marker, source);
    internal void PayRecipientCategoryAreaDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayRecipientCategoryAreaDiscard(f);
    internal void PayRecipientCategoryGift(global::CardGame.Core.ProgramSkillFrame f, int id) => _resolve().PayRecipientCategoryGift(f, id);
    internal void PublishRecipientCategoryMarkChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishRecipientCategoryMarkChoice(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RecipientCategoryMarkMaterial> RecipientCategoryAreaMaterials(int chooser, int target) => _resolve().RecipientCategoryAreaMaterials(chooser, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RecipientCategoryMarkChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecipientCategoryMarkChoices(f);
    internal bool RecipientCategoryMaterialStillLegal(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecipientCategoryMarkMaterial m) => _resolve().RecipientCategoryMaterialStillLegal(f, m);
    internal void ResolveRecipientCategoryMarkChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveRecipientCategoryMarkChoice(choice);
    internal bool ResumeRecipientCategoryMark(long id) => _resolve().ResumeRecipientCategoryMark(id);
    internal bool ReturnRecipientCategoryMarkMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnRecipientCategoryMarkMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiRecipientCategoryMarkChoice(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiRecipientCategoryMarkChoice(p, f);
    internal long RecipientMarkSequence { get => _resolve().RecipientMarkSequence; }
    internal bool TracksRecipientCategoryMarks { get => _resolve().TracksRecipientCategoryMarks; }
}
