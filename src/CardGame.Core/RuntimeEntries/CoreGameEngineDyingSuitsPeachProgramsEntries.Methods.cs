#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingSuitsPeachProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDyingSuits(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginDyingSuits(supplied);
    internal bool CanRunDyingSuitsAndEndingHistory(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunDyingSuitsAndEndingHistory(candidate, trigger, context);
    internal bool ContinueDyingSuitsPeach(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueDyingSuitsPeach(id, continuation);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DyingSuitsChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DyingSuitsChoices(f);
    internal global::CardGame.Core.Card[] DyingSuitsDiscardCandidates(global::CardGame.Core.ProgramSkillFrame f, int payer) => _resolve().DyingSuitsDiscardCandidates(f, payer);
    internal int[] DyingSuitsRecipients(int owner, int victim) => _resolve().DyingSuitsRecipients(owner, victim);
    internal bool HasIssuedDyingSuitsRound(int owner, string skill) => _resolve().HasIssuedDyingSuitsRound(owner, skill);
    internal bool IsDyingSuitsMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsDyingSuitsMovement(f, effect, pending);
    internal void IssueDyingSuitsPeach(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueDyingSuitsPeach(f);
    internal void PublishDyingSuitsChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDyingSuitsChoice(f);
    internal void ResolveDyingSuitsChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDyingSuitsChoice(choice);
    internal bool ResumeDyingSuits(long id) => _resolve().ResumeDyingSuits(id);
    internal bool ReturnDyingSuitsMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnDyingSuitsMovement(f);
    internal void ReturnDyingSuitsPeach(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnDyingSuitsPeach(use);
    internal global::CardGame.Core.PromptChoice SelectAiDyingSuits(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiDyingSuits(decision, f);
    internal bool TryDrainDyingSuits(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TryDrainDyingSuits(f);
}
