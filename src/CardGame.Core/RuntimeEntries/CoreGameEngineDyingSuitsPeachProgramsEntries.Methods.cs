#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingSuitsPeachProgramsEntries
{
    internal SkillProgramStepOutcome BeginDyingSuits(ProgramSkillFrame supplied) => _resolve().BeginDyingSuits(supplied);
    internal bool CanRunDyingSuitsAndEndingHistory(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunDyingSuitsAndEndingHistory(candidate, trigger, context);
    internal bool ContinueDyingSuitsPeach(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueDyingSuitsPeach(id, continuation);
    internal IReadOnlyList<PromptChoice> DyingSuitsChoices(ProgramSkillFrame f) => _resolve().DyingSuitsChoices(f);
    internal Card[] DyingSuitsDiscardCandidates(ProgramSkillFrame f, int payer) => _resolve().DyingSuitsDiscardCandidates(f, payer);
    internal int[] DyingSuitsRecipients(int owner, int victim) => _resolve().DyingSuitsRecipients(owner, victim);
    internal bool HasIssuedDyingSuitsRound(int owner, string skill) => _resolve().HasIssuedDyingSuitsRound(owner, skill);
    internal bool IsDyingSuitsMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsDyingSuitsMovement(f, effect, pending);
    internal void IssueDyingSuitsPeach(ProgramSkillFrame f) => _resolve().IssueDyingSuitsPeach(f);
    internal void PublishDyingSuitsChoice(ProgramSkillFrame f) => _resolve().PublishDyingSuitsChoice(f);
    internal void ResolveDyingSuitsChoice(PromptChoice choice) => _resolve().ResolveDyingSuitsChoice(choice);
    internal bool ResumeDyingSuits(long id) => _resolve().ResumeDyingSuits(id);
    internal bool ReturnDyingSuitsMovement(ProgramSkillFrame f) => _resolve().ReturnDyingSuitsMovement(f);
    internal void ReturnDyingSuitsPeach(CardUseFrame use) => _resolve().ReturnDyingSuitsPeach(use);
    internal PromptChoice SelectAiDyingSuits(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiDyingSuits(decision, f);
    internal bool TryDrainDyingSuits(ProgramSkillFrame f) => _resolve().TryDrainDyingSuits(f);
}
