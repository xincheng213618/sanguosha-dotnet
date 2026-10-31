#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignSelectedCardSlashEntries
{
    internal SkillProgramStepOutcome BeginSelectedForeignCardSlash(ProgramSkillFrame supplied, int actorSeat) => _resolve().BeginSelectedForeignCardSlash(supplied, actorSeat);
    internal void BindSelectedForeignCardSlashUse(long useId, long? parentId) => _resolve().BindSelectedForeignCardSlashUse(useId, parentId);
    internal bool CanStartSelectedForeignCardSlash(CharacterState owner, int actorSeat) => _resolve().CanStartSelectedForeignCardSlash(owner, actorSeat);
    internal bool CanUseSelectedForeignSlashMaterial(int issuer, int actor, Card card, CardLocation from) => _resolve().CanUseSelectedForeignSlashMaterial(issuer, actor, card, from);
    internal void CompleteSelectedForeignCardSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteSelectedForeignCardSlash(completion);
    internal void CompleteSelectedForeignCardSlashPayment(long useId) => _resolve().CompleteSelectedForeignCardSlashPayment(useId);
    internal void FinishSelectedForeignCardSlash(ProgramSkillFrame frame) => _resolve().FinishSelectedForeignCardSlash(frame);
    internal bool IsSelectedForeignCardSlashSource(long? parentId, int actor, int target, Card card, CardKind kind, int physicalOwner, CardLocation from, IReadOnlyList<Card>? materials, bool countsTowardLimit) => _resolve().IsSelectedForeignCardSlashSource(parentId, actor, target, card, kind, physicalOwner, from, materials, countsTowardLimit);
    internal bool IsSelectedForeignSlashActivation(ProgramSkillFrame frame) => _resolve().IsSelectedForeignSlashActivation(frame);
    internal void PublishSelectedForeignCardSlash(ProgramSkillFrame frame) => _resolve().PublishSelectedForeignCardSlash(frame);
    internal void ResolveSelectedForeignCardSlashChoice(PromptChoice selected) => _resolve().ResolveSelectedForeignCardSlashChoice(selected);
    internal bool ResumeSelectedForeignCardSlash(ProgramSkillFrame frame) => _resolve().ResumeSelectedForeignCardSlash(frame);
    internal bool ResumeSelectedForeignCardSlash2(long id) => _resolve().ResumeSelectedForeignCardSlash(id);
    internal bool ReturnRegisteredCompleteSelectedForeignCardSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteSelectedForeignCardSlash(completion);
    internal bool ReturnSelectedForeignCardSlashMovement(ProgramSkillFrame frame) => _resolve().ReturnSelectedForeignCardSlashMovement(frame);
    internal PromptChoice SelectAiSelectedForeignCardSlashChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiSelectedForeignCardSlashChoice(decision, frame);
    internal IReadOnlyList<PromptChoice> SelectedForeignCardSlashChoices(ProgramSkillFrame frame) => _resolve().SelectedForeignCardSlashChoices(frame);
    internal bool SelectedForeignSlashEnded { get => _resolve().SelectedForeignSlashEnded; }
}
