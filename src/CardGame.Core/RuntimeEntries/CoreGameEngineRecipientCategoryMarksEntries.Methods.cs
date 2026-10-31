#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategoryMarksEntries
{
    internal bool CanContinueRecipientCategoryMark(ProgramSkillFrame f) => _resolve().CanContinueRecipientCategoryMark(f);
    internal bool CanOfferRecipientCategoryMarkProgram(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferRecipientCategoryMarkProgram(c, trigger, context);
    internal void ChangeRecipientCategoryMarker(ProgramSkillFrame f, int holder, RecipientCategoryMarkKind kind, int delta) => _resolve().ChangeRecipientCategoryMarker(f, holder, kind, delta);
    internal void CompleteRecipientCategoryMark(ProgramSkillFrame f) => _resolve().CompleteRecipientCategoryMark(f);
    internal void ContinueRecipientCategoryPublicReveal(ProgramSkillFrame f) => _resolve().ContinueRecipientCategoryPublicReveal(f);
    internal SkillProgramStepOutcome ExecuteRecipientCategoryMark(ProgramSkillFrame supplied, SkillProgramEffect e) => _resolve().ExecuteRecipientCategoryMark(supplied, e);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeRecipientCategoryGiftObservers(CharacterState drawOwner, IReadOnlyList<ProgramTriggerCandidate> existing) => _resolve().IncludeRecipientCategoryGiftObservers(drawOwner, existing);
    internal int IndependentRecipientCategoryMarkerCount(CharacterState holder, PlayerMarkerKind marker, int source) => _resolve().IndependentRecipientCategoryMarkerCount(holder, marker, source);
    internal void PayRecipientCategoryAreaDiscard(ProgramSkillFrame f) => _resolve().PayRecipientCategoryAreaDiscard(f);
    internal void PayRecipientCategoryGift(ProgramSkillFrame f, int id) => _resolve().PayRecipientCategoryGift(f, id);
    internal void PublishRecipientCategoryMarkChoice(ProgramSkillFrame f) => _resolve().PublishRecipientCategoryMarkChoice(f);
    internal IReadOnlyList<RecipientCategoryMarkMaterial> RecipientCategoryAreaMaterials(int chooser, int target) => _resolve().RecipientCategoryAreaMaterials(chooser, target);
    internal IReadOnlyList<PromptChoice> RecipientCategoryMarkChoices(ProgramSkillFrame f) => _resolve().RecipientCategoryMarkChoices(f);
    internal bool RecipientCategoryMaterialStillLegal(ProgramSkillFrame f, RecipientCategoryMarkMaterial m) => _resolve().RecipientCategoryMaterialStillLegal(f, m);
    internal void ResolveRecipientCategoryMarkChoice(PromptChoice choice) => _resolve().ResolveRecipientCategoryMarkChoice(choice);
    internal bool ResumeRecipientCategoryMark(long id) => _resolve().ResumeRecipientCategoryMark(id);
    internal bool ReturnRecipientCategoryMarkMovement(ProgramSkillFrame f) => _resolve().ReturnRecipientCategoryMarkMovement(f);
    internal PromptChoice SelectAiRecipientCategoryMarkChoice(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiRecipientCategoryMarkChoice(p, f);
    internal long RecipientMarkSequence { get => _resolve().RecipientMarkSequence; }
    internal bool TracksRecipientCategoryMarks { get => _resolve().TracksRecipientCategoryMarks; }
}
