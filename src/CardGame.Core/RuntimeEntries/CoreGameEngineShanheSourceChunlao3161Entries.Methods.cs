#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceChunlao3161Entries
{
    internal PromptChoice AiShanheSourceChunlaoStorage(PendingDecision p, ProgramSkillFrame f) => _resolve().AiShanheSourceChunlaoStorage(p, f);
    internal void AnswerShanheSourceChunlaoStorage(PromptChoice choice) => _resolve().AnswerShanheSourceChunlaoStorage(choice);
    internal SkillProgramStepOutcome BeginShanheSourceChunlaoStorage(ProgramSkillFrame supplied) => _resolve().BeginShanheSourceChunlaoStorage(supplied);
    internal int[] MatchingShanheSourceChunlaoIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingShanheSourceChunlaoIndexes(batch, candidate);
    internal bool ResumeShanheSourceChunlaoStorage(long id) => _resolve().ResumeShanheSourceChunlaoStorage(id);
    internal int[]? SelectWindowIndexes_MatchingShanheSourceChunlaoIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingShanheSourceChunlaoIndexes(batch, candidate, trigger);
}
