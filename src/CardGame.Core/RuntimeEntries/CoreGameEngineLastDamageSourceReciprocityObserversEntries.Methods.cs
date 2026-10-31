#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityObserversEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? AnyLastDamageSourceReciprocityObserverRoot() => _resolve().AnyLastDamageSourceReciprocityObserverRoot();
    internal void AssertLastDamageSourceReciprocity(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertLastDamageSourceReciprocity(f);
    internal bool HasLastDamageSourceReciprocityDamageObserver(long windowId) => _resolve().HasLastDamageSourceReciprocityDamageObserver(windowId);
    internal bool IsLastDamageSourceReciprocityChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PendingDecision decision) => _resolve().IsLastDamageSourceReciprocityChoice(f, decision);
    internal bool IsLastDamageSourceReciprocityDying() => _resolve().IsLastDamageSourceReciprocityDying();
    internal bool LastDamageSourceFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LastDamageSourceFirstChild(f, child);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? LastDamageSourceOriginalAttack(long id) => _resolve().LastDamageSourceOriginalAttack(id);
    internal global::CardGame.Core.ProgramSkillFrame? LastDamageSourceReciprocityObserverRoot(long damageWindowId) => _resolve().LastDamageSourceReciprocityObserverRoot(damageWindowId);
    internal bool TryAdvanceLastDamageSourceReciprocitySubtree() => _resolve().TryAdvanceLastDamageSourceReciprocitySubtree();
    internal bool ValidLastDamageSourceReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidLastDamageSourceReceipt(f);
}
