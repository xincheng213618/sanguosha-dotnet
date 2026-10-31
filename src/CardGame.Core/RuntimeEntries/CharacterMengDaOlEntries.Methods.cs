#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMengDaOlEntries
{
    internal bool AdvanceMengDaSubtree() => _resolve().AdvanceMengDaSubtree();
    internal void AnswerGoude(PromptChoice choice) => _resolve().AnswerGoude(choice);
    internal SkillProgramStepOutcome BeginGoude(ProgramSkillFrame input) => _resolve().BeginGoude(input);
    internal PromptChoice ChooseGoudeAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseGoudeAi(p, f);
    internal void ContinueMengDa(long id) => _resolve().ContinueMengDa(id);
    internal bool ContinuePaidMengDa(ProgramSkillFrame f) => _resolve().ContinuePaidMengDa(f);
    internal bool ExactGoudeCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ExactGoudeCandidate(candidate, context);
    internal void FinishGoude(ProgramSkillFrame f) => _resolve().FinishGoude(f);
    internal IReadOnlyList<GoudeOption> GoudeAvailableOptions(int owner, int mask) => _resolve().GoudeAvailableOptions(owner, mask);
    internal IReadOnlyList<PromptChoice> GoudeChoices(ProgramSkillFrame f) => _resolve().GoudeChoices(f);
    internal IReadOnlyList<GoudeHandMaterial> GoudeDiscardMaterials(int owner, int target) => _resolve().GoudeDiscardMaterials(owner, target);
    internal IReadOnlyList<int> GoudeDiscardTargets(int owner) => _resolve().GoudeDiscardTargets(owner);
    internal int GoudeExecutedMask(int turn, IReadOnlyList<int> sameFaction, int cutoff) => _resolve().GoudeExecutedMask(turn, sameFaction, cutoff);
    internal IReadOnlyList<string> GoudeFactionChoices(int owner) => _resolve().GoudeFactionChoices(owner);
    internal IReadOnlyList<int> GoudeFactionSeats(int owner) => _resolve().GoudeFactionSeats(owner);
    internal IReadOnlyList<int> GoudeSlashTargets(int owner) => _resolve().GoudeSlashTargets(owner);
    internal int GoudeZeroSlashActor(GoudeZeroSlashDeclaredEvent observed, IReadOnlyList<IGameEvent> history) => _resolve().GoudeZeroSlashActor(observed, history);
    internal bool IsGoudeSlashUse(long useId) => _resolve().IsGoudeSlashUse(useId);
    internal void IssueGoudeSlashUse(long useId, ProgramSkillFrame parent, CardActionContext action) => _resolve().IssueGoudeSlashUse(useId, parent, action);
    internal bool MengDaFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MengDaFirstChild(f, child);
    internal ProgramSkillFrame? MengDaObserverRoot() => _resolve().MengDaObserverRoot();
    internal void ObserveGoudeZeroSlash(IGameEvent fact) => _resolve().ObserveGoudeZeroSlash(fact);
    internal void PublishGoude(ProgramSkillFrame f) => _resolve().PublishGoude(f);
    internal bool QualifyGoude(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().QualifyGoude(candidate, trigger, context);
    internal void RecordGoudePayment(long id, long before, int count, GoudeHandMaterial? material, long? sequence) => _resolve().RecordGoudePayment(id, before, count, material, sequence);
    internal bool ResumeMengDa(long id) => _resolve().ResumeMengDa(id);
    internal bool ReturnGoudeAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnGoudeAttack(completion);
    internal void ReturnGoudeNativeUse(CardUseFrame use) => _resolve().ReturnGoudeNativeUse(use);
    internal bool ReturnMengDaMovement(ProgramSkillFrame f) => _resolve().ReturnMengDaMovement(f);
    internal void ValidateGoudeNativeUse(CardUseFrame use) => _resolve().ValidateGoudeNativeUse(use);
    internal bool HasGoudeCapability { get => _resolve().HasGoudeCapability; }
}
