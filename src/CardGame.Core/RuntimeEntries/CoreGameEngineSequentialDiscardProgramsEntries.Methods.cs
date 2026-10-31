#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardProgramsEntries
{
    internal SkillProgramStepOutcome FinishSequentialDiscard(ProgramSkillFrame frame, int remaining) => _resolve().FinishSequentialDiscard(frame, remaining);
    internal void PaySequentialDiscard(ProgramSkillFrame frame, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> sources) => _resolve().PaySequentialDiscard(frame, ids, sources);
    internal SkillProgramStepOutcome PublishSequentialDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().PublishSequentialDiscard(frame, effect);
    internal void ResolveSequentialDiscardChoice(PromptChoice choice) => _resolve().ResolveSequentialDiscardChoice(choice);
    internal SkillProgramStepOutcome RunSequentialDiscard(ProgramSkillFrame supplied, SkillProgramEffect effect, int selectedTarget) => _resolve().RunSequentialDiscard(supplied, effect, selectedTarget);
    internal PromptChoice SelectAiSequentialDiscard(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiSequentialDiscard(decision, frame);
    internal bool SequentialDiscardCanContinue(ProgramSkillFrame frame) => _resolve().SequentialDiscardCanContinue(frame);
    internal IEnumerable<(Card Card, CardLocation Location)> SequentialDiscardCards(ProgramSkillFrame frame, int chooser, SkillProgramEffect effect) => _resolve().SequentialDiscardCards(frame, chooser, effect);
    internal SkillProgramEffect SequentialDiscardEffect(ProgramSkillFrame frame) => _resolve().SequentialDiscardEffect(frame);
    internal IReadOnlyList<int> SequentialDiscardOrder(int owner, int start, bool ring) => _resolve().SequentialDiscardOrder(owner, start, ring);
    internal string SequentialDiscardReason(ProgramSkillFrame frame) => _resolve().SequentialDiscardReason(frame);
    internal CardConversionSource SequentialDiscardSource(ProgramSkillFrame f) => _resolve().SequentialDiscardSource(f);
    internal bool TryReturnSequentialDiscardMovement(ProgramSkillFrame frame) => _resolve().TryReturnSequentialDiscardMovement(frame);
    internal long SequentialDiscardMovementSequence { get => _resolve().SequentialDiscardMovementSequence; }
}
