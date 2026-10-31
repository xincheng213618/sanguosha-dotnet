#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientConsequencesObserversEntries
{
    internal bool IsRecipientConsequencesProgramDying() => _resolve().IsRecipientConsequencesProgramDying();
    internal bool RecipientConsequencesFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().RecipientConsequencesFirstChild(f, child);
    internal ProgramSkillFrame? RecipientConsequencesObserverRoot() => _resolve().RecipientConsequencesObserverRoot();
    internal bool RecipientConsequencesPrefix(ProgramSkillFrame f, int last) => _resolve().RecipientConsequencesPrefix(f, last);
    internal bool TryAdvanceRecipientConsequencesSubtree() => _resolve().TryAdvanceRecipientConsequencesSubtree();
}
