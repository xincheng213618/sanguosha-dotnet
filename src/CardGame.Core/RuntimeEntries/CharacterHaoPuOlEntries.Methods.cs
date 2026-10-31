#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHaoPuOlEntries
{
    internal bool AdvanceZhenyingSubtree() => _resolve().AdvanceZhenyingSubtree();
    internal void AnswerZhenying(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerZhenying(choice);
    internal void AssertZhenyingDuels(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertZhenyingDuels(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhenying(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginZhenying(supplied);
    internal global::CardGame.Core.PromptChoice ChooseZhenyingAi(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseZhenyingAi(prompt, f);
    internal bool ContinuePaidZhenying(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidZhenying(f);
    internal bool DrainZhenyingChildren(long id) => _resolve().DrainZhenyingChildren(id);
    internal void FinishZhenying(global::CardGame.Core.ProgramSkillFrame f, bool duel) => _resolve().FinishZhenying(f, duel);
    internal global::CardGame.Core.ZhenyingHandPlan FreezeZhenyingHand(int seat) => _resolve().FreezeZhenyingHand(seat);
    internal bool HasZhenyingBeforeDamageObserver(long id) => _resolve().HasZhenyingBeforeDamageObserver(id);
    internal bool HasZhenyingDamageObserver(long id) => _resolve().HasZhenyingDamageObserver(id);
    internal bool IsZhenyingDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsZhenyingDuelAttackConsistent(attack, processing);
    internal bool IsZhenyingDuelUse(long id) => _resolve().IsZhenyingDuelUse(id);
    internal bool IsZhenyingDying() => _resolve().IsZhenyingDying();
    internal bool IsZhenyingMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsZhenyingMovement(f, effect, p);
    internal bool MatchesZhenyingDuel(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ZhenyingDuelOrigin d) => _resolve().MatchesZhenyingDuel(f, d);
    internal void PayZhenyingAdjustment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayZhenyingAdjustment(f);
    internal void PublishZhenyingChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhenyingChoice(f);
    internal void RequireZhenying(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireZhenying(f);
    internal bool ResumeZhenying(long id) => _resolve().ResumeZhenying(id);
    internal void ReturnZhenyingDuel(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnZhenyingDuel(use);
    internal bool ReturnZhenyingDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnZhenyingDuelAttack(completion);
    internal bool ReturnZhenyingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhenyingMovement(f);
    internal bool ValidZhenying(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhenying(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhenyingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhenyingChoices(f);
    internal bool ZhenyingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhenyingFirstChild(f, child);
    internal int[] ZhenyingHandAt(long sequence, int seat) => _resolve().ZhenyingHandAt(sequence, seat);
    internal global::CardGame.Core.ProgramSkillFrame? ZhenyingObserverRoot() => _resolve().ZhenyingObserverRoot();
}
