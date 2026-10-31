#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignSelectedCardSlashEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSelectedForeignCardSlash(global::CardGame.Core.ProgramSkillFrame supplied, int actorSeat) => _resolve().BeginSelectedForeignCardSlash(supplied, actorSeat);
    internal void BindSelectedForeignCardSlashUse(long useId, long? parentId) => _resolve().BindSelectedForeignCardSlashUse(useId, parentId);
    internal bool CanStartSelectedForeignCardSlash(global::CardGame.Core.CharacterState owner, int actorSeat) => _resolve().CanStartSelectedForeignCardSlash(owner, actorSeat);
    internal bool CanUseSelectedForeignSlashMaterial(int issuer, int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().CanUseSelectedForeignSlashMaterial(issuer, actor, card, from);
    internal void CompleteSelectedForeignCardSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteSelectedForeignCardSlash(completion);
    internal void CompleteSelectedForeignCardSlashPayment(long useId) => _resolve().CompleteSelectedForeignCardSlashPayment(useId);
    internal void FinishSelectedForeignCardSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishSelectedForeignCardSlash(frame);
    internal bool IsSelectedForeignCardSlashSource(long? parentId, int actor, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int physicalOwner, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? materials, bool countsTowardLimit) => _resolve().IsSelectedForeignCardSlashSource(parentId, actor, target, card, kind, physicalOwner, from, materials, countsTowardLimit);
    internal bool IsSelectedForeignSlashActivation(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsSelectedForeignSlashActivation(frame);
    internal void PublishSelectedForeignCardSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishSelectedForeignCardSlash(frame);
    internal void ResolveSelectedForeignCardSlashChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveSelectedForeignCardSlashChoice(selected);
    internal bool ResumeSelectedForeignCardSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResumeSelectedForeignCardSlash(frame);
    internal bool ResumeSelectedForeignCardSlash2(long id) => _resolve().ResumeSelectedForeignCardSlash(id);
    internal bool ReturnRegisteredCompleteSelectedForeignCardSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteSelectedForeignCardSlash(completion);
    internal bool ReturnSelectedForeignCardSlashMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnSelectedForeignCardSlashMovement(frame);
    internal global::CardGame.Core.PromptChoice SelectAiSelectedForeignCardSlashChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiSelectedForeignCardSlashChoice(decision, frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SelectedForeignCardSlashChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectedForeignCardSlashChoices(frame);
    internal bool SelectedForeignSlashEnded { get => _resolve().SelectedForeignSlashEnded; }
}
