#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuPiOlEntries
{
    internal bool AdvanceLiuPiSubtree() => _resolve().AdvanceLiuPiSubtree();
    internal PromptChoice AiLiuPiExchange(PendingDecision p, ProgramSkillFrame f) => _resolve().AiLiuPiExchange(p, f);
    internal void AnswerLiuPiExchange(PromptChoice choice) => _resolve().AnswerLiuPiExchange(choice);
    internal SkillProgramStepOutcome BeginLiuPiExchange(ProgramSkillFrame supplied) => _resolve().BeginLiuPiExchange(supplied);
    internal void CleanupLiuPiExchange(ProgramSkillFrame f) => _resolve().CleanupLiuPiExchange(f);
    internal bool ContinueIssuedLiuPiExchange(ProgramSkillFrame f) => _resolve().ContinueIssuedLiuPiExchange(f);
    internal void ContinueLiuPiExchange(long id) => _resolve().ContinueLiuPiExchange(id);
    internal bool DrainLiuPiChildren(long id) => _resolve().DrainLiuPiChildren(id);
    internal void IssueLiuPiExchange(ProgramSkillFrame f, bool full) => _resolve().IssueLiuPiExchange(f, full);
    internal IReadOnlyList<LiuPiTableEntry> LiuPiAvailableTable(ProgramSkillFrame f) => _resolve().LiuPiAvailableTable(f);
    internal IReadOnlyList<PromptChoice> LiuPiChoices(ProgramSkillFrame f) => _resolve().LiuPiChoices(f);
    internal LiuPiTableEntry LiuPiEntry(int id) => _resolve().LiuPiEntry(id);
    internal bool LiuPiEntryAvailable(LiuPiTableEntry e) => _resolve().LiuPiEntryAvailable(e);
    internal bool LiuPiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().LiuPiFirstChild(root, child);
    internal ProgramSkillFrame? LiuPiObserverRoot() => _resolve().LiuPiObserverRoot();
    internal CardConversionSource LiuPiSource(ProgramSkillFrame f) => _resolve().LiuPiSource(f);
    internal void PublishLiuPiExchange(ProgramSkillFrame f) => _resolve().PublishLiuPiExchange(f);
    internal ProgramSkillFrame ReplaceLiuPiPublicTable(long id, IReadOnlyList<int> ids) => _resolve().ReplaceLiuPiPublicTable(id, ids);
    internal bool ResumeLiuPiExchange(long id) => _resolve().ResumeLiuPiExchange(id);
}
