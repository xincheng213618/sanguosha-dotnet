#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityObserversEntries
{
    internal ProgramSkillFrame? AnyLastDamageSourceReciprocityObserverRoot() => _resolve().AnyLastDamageSourceReciprocityObserverRoot();
    internal bool IsLastDamageSourceReciprocityChoice(ProgramSkillFrame f, PendingDecision decision) => _resolve().IsLastDamageSourceReciprocityChoice(f, decision);
    internal bool LastDamageSourceFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LastDamageSourceFirstChild(f, child);
    internal GameEngine.IDamageAttempt? LastDamageSourceOriginalAttack(long id) => _resolve().LastDamageSourceOriginalAttack(id);
    internal ProgramSkillFrame? LastDamageSourceReciprocityObserverRoot(long damageWindowId) => _resolve().LastDamageSourceReciprocityObserverRoot(damageWindowId);
    internal bool TryAdvanceLastDamageSourceReciprocitySubtree() => _resolve().TryAdvanceLastDamageSourceReciprocitySubtree();
    internal bool ValidLastDamageSourceReceipt(ProgramSkillFrame f) => _resolve().ValidLastDamageSourceReceipt(f);
}
