#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientConsequencesObserversEntries
{
    internal bool IsRecipientConsequencesProgramDying() => _resolve().IsRecipientConsequencesProgramDying();
    internal bool RecipientConsequencesFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().RecipientConsequencesFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? RecipientConsequencesObserverRoot() => _resolve().RecipientConsequencesObserverRoot();
    internal bool RecipientConsequencesPrefix(global::CardGame.Core.ProgramSkillFrame f, int last) => _resolve().RecipientConsequencesPrefix(f, last);
    internal bool TryAdvanceRecipientConsequencesSubtree() => _resolve().TryAdvanceRecipientConsequencesSubtree();
}
