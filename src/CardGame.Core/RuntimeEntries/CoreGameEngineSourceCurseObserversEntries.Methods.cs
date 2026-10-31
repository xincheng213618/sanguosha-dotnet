#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceCurseObserversEntries
{
    internal bool HasSourceCurseDamageObserver(long windowId) => _resolve().HasSourceCurseDamageObserver(windowId);
    internal bool SourceCurseFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().SourceCurseFirstChild(root, child);
    internal bool SourceCurseMovementMatches(global::CardGame.Core.SourceCurseProgramReceipt r, int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, string reason) => _resolve().SourceCurseMovementMatches(r, cardId, from, to, reason);
    internal global::CardGame.Core.ProgramSkillFrame? SourceCurseObserverRoot() => _resolve().SourceCurseObserverRoot();
    internal bool ValidSourceCurseReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSourceCurseReceipt(f);
}
