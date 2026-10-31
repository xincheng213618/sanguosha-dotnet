#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuPiOlEntries
{
    internal bool AdvanceLiuPiSubtree() => _resolve().AdvanceLiuPiSubtree();
    internal global::CardGame.Core.PromptChoice AiLiuPiExchange(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiLiuPiExchange(p, f);
    internal void AnswerLiuPiExchange(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerLiuPiExchange(choice);
    internal void AssertLiuPiExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertLiuPiExchange(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiuPiExchange(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginLiuPiExchange(supplied);
    internal void CleanupLiuPiExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CleanupLiuPiExchange(f);
    internal bool ContinueIssuedLiuPiExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedLiuPiExchange(f);
    internal void ContinueLiuPiExchange(long id) => _resolve().ContinueLiuPiExchange(id);
    internal bool DrainLiuPiChildren(long id) => _resolve().DrainLiuPiChildren(id);
    internal bool HasLiuPiDamageObserver(long id) => _resolve().HasLiuPiDamageObserver(id);
    internal bool IsLiuPiDying() => _resolve().IsLiuPiDying();
    internal void IssueLiuPiExchange(global::CardGame.Core.ProgramSkillFrame f, bool full) => _resolve().IssueLiuPiExchange(f, full);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LiuPiTableEntry> LiuPiAvailableTable(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuPiAvailableTable(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LiuPiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuPiChoices(f);
    internal global::CardGame.Core.LiuPiTableEntry LiuPiEntry(int id) => _resolve().LiuPiEntry(id);
    internal bool LiuPiEntryAvailable(global::CardGame.Core.LiuPiTableEntry e) => _resolve().LiuPiEntryAvailable(e);
    internal bool LiuPiEntryHistoryMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LiuPiTableEntry e) => _resolve().LiuPiEntryHistoryMatches(f, e);
    internal bool LiuPiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().LiuPiFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? LiuPiObserverRoot() => _resolve().LiuPiObserverRoot();
    internal global::CardGame.Core.CardConversionSource LiuPiSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuPiSource(f);
    internal void PublishLiuPiExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLiuPiExchange(f);
    internal global::CardGame.Core.ProgramSkillFrame ReplaceLiuPiPublicTable(long id, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().ReplaceLiuPiPublicTable(id, ids);
    internal bool ResumeLiuPiExchange(long id) => _resolve().ResumeLiuPiExchange(id);
}
