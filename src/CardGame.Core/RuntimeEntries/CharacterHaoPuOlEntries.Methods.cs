#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHaoPuOlEntries
{
    internal bool AdvanceZhenyingSubtree() => _resolve().AdvanceZhenyingSubtree();
    internal void AnswerZhenying(PromptChoice choice) => _resolve().AnswerZhenying(choice);
    internal SkillProgramStepOutcome BeginZhenying(ProgramSkillFrame supplied) => _resolve().BeginZhenying(supplied);
    internal PromptChoice ChooseZhenyingAi(PendingDecision prompt, ProgramSkillFrame f) => _resolve().ChooseZhenyingAi(prompt, f);
    internal bool ContinuePaidZhenying(ProgramSkillFrame f) => _resolve().ContinuePaidZhenying(f);
    internal bool DrainZhenyingChildren(long id) => _resolve().DrainZhenyingChildren(id);
    internal void FinishZhenying(ProgramSkillFrame f, bool duel) => _resolve().FinishZhenying(f, duel);
    internal ZhenyingHandPlan FreezeZhenyingHand(int seat) => _resolve().FreezeZhenyingHand(seat);
    internal bool IsZhenyingDuelUse(long id) => _resolve().IsZhenyingDuelUse(id);
    internal bool MatchesZhenyingDuel(ProgramSkillFrame f, ZhenyingDuelOrigin d) => _resolve().MatchesZhenyingDuel(f, d);
    internal void PayZhenyingAdjustment(ProgramSkillFrame f) => _resolve().PayZhenyingAdjustment(f);
    internal void PublishZhenyingChoice(ProgramSkillFrame f) => _resolve().PublishZhenyingChoice(f);
    internal bool ResumeZhenying(long id) => _resolve().ResumeZhenying(id);
    internal void ReturnZhenyingDuel(CardUseFrame use) => _resolve().ReturnZhenyingDuel(use);
    internal bool ReturnZhenyingDuelAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnZhenyingDuelAttack(completion);
    internal bool ReturnZhenyingMovement(ProgramSkillFrame f) => _resolve().ReturnZhenyingMovement(f);
    internal bool ValidZhenying(ProgramSkillFrame f) => _resolve().ValidZhenying(f);
    internal IReadOnlyList<PromptChoice> ZhenyingChoices(ProgramSkillFrame f) => _resolve().ZhenyingChoices(f);
    internal bool ZhenyingFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhenyingFirstChild(f, child);
    internal int[] ZhenyingHandAt(long sequence, int seat) => _resolve().ZhenyingHandAt(sequence, seat);
    internal ProgramSkillFrame? ZhenyingObserverRoot() => _resolve().ZhenyingObserverRoot();
}
