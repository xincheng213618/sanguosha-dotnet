#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMengDaOlEntries
{
    internal bool AdvanceMengDaSubtree() => _resolve().AdvanceMengDaSubtree();
    internal void AnswerGoude(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerGoude(choice);
    internal void AssertMengDa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMengDa(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGoude(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginGoude(input);
    internal global::CardGame.Core.PromptChoice ChooseGoudeAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseGoudeAi(p, f);
    internal void ContinueMengDa(long id) => _resolve().ContinueMengDa(id);
    internal bool ContinuePaidMengDa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMengDa(f);
    internal bool ExactGoudeCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ExactGoudeCandidate(candidate, context);
    internal void FinishGoude(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishGoude(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GoudeOption> GoudeAvailableOptions(int owner, int mask) => _resolve().GoudeAvailableOptions(owner, mask);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GoudeChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GoudeChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GoudeHandMaterial> GoudeDiscardMaterials(int owner, int target) => _resolve().GoudeDiscardMaterials(owner, target);
    internal global::System.Collections.Generic.IReadOnlyList<int> GoudeDiscardTargets(int owner) => _resolve().GoudeDiscardTargets(owner);
    internal int GoudeExecutedMask(int turn, global::System.Collections.Generic.IReadOnlyList<int> sameFaction, int cutoff) => _resolve().GoudeExecutedMask(turn, sameFaction, cutoff);
    internal global::System.Collections.Generic.IReadOnlyList<string> GoudeFactionChoices(int owner) => _resolve().GoudeFactionChoices(owner);
    internal global::System.Collections.Generic.IReadOnlyList<int> GoudeFactionSeats(int owner) => _resolve().GoudeFactionSeats(owner);
    internal global::System.Collections.Generic.IReadOnlyList<int> GoudeSlashTargets(int owner) => _resolve().GoudeSlashTargets(owner);
    internal int GoudeZeroSlashActor(global::CardGame.Core.GoudeZeroSlashDeclaredEvent observed, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().GoudeZeroSlashActor(observed, history);
    internal bool HasGoudeCompletedSlashChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().HasGoudeCompletedSlashChild(window);
    internal bool HasMengDaBeforeDamageObserver(long id) => _resolve().HasMengDaBeforeDamageObserver(id);
    internal bool HasMengDaDamageObserver(long id) => _resolve().HasMengDaDamageObserver(id);
    internal bool IsGoudeSlashUse(long useId) => _resolve().IsGoudeSlashUse(useId);
    internal bool IsMengDaDying() => _resolve().IsMengDaDying();
    internal bool IsMengDaMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMengDaMovement(f, e, p);
    internal void IssueGoudeSlashUse(long useId, global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.CardActionContext action) => _resolve().IssueGoudeSlashUse(useId, parent, action);
    internal bool MengDaFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MengDaFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MengDaObserverRoot() => _resolve().MengDaObserverRoot();
    internal void ObserveGoudeZeroSlash(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveGoudeZeroSlash(fact);
    internal void PublishGoude(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishGoude(f);
    internal bool QualifyGoude(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyGoude(candidate, trigger, context);
    internal void RecordGoudePayment(long id, long before, int count, global::CardGame.Core.GoudeHandMaterial? material, long? sequence) => _resolve().RecordGoudePayment(id, before, count, material, sequence);
    internal bool ResumeMengDa(long id) => _resolve().ResumeMengDa(id);
    internal bool ReturnGoudeAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnGoudeAttack(completion);
    internal void ReturnGoudeNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnGoudeNativeUse(use);
    internal bool ReturnMengDaMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMengDaMovement(f);
    internal void ValidateGoudeNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateGoudeNativeUse(use);
    internal bool HasGoudeCapability { get => _resolve().HasGoudeCapability; }
}
