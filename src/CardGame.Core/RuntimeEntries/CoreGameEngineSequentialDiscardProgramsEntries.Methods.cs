#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome FinishSequentialDiscard(global::CardGame.Core.ProgramSkillFrame frame, int remaining) => _resolve().FinishSequentialDiscard(frame, remaining);
    internal void PaySequentialDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> sources) => _resolve().PaySequentialDiscard(frame, ids, sources);
    internal global::CardGame.Core.SkillProgramStepOutcome PublishSequentialDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().PublishSequentialDiscard(frame, effect);
    internal void ResolveSequentialDiscardChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSequentialDiscardChoice(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome RunSequentialDiscard(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect, int selectedTarget) => _resolve().RunSequentialDiscard(supplied, effect, selectedTarget);
    internal global::CardGame.Core.PromptChoice SelectAiSequentialDiscard(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiSequentialDiscard(decision, frame);
    internal bool SequentialDiscardCanContinue(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SequentialDiscardCanContinue(frame);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation Location)> SequentialDiscardCards(global::CardGame.Core.ProgramSkillFrame frame, int chooser, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SequentialDiscardCards(frame, chooser, effect);
    internal global::CardGame.Core.SkillProgramEffect SequentialDiscardEffect(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SequentialDiscardEffect(frame);
    internal global::System.Collections.Generic.IReadOnlyList<int> SequentialDiscardOrder(int owner, int start, bool ring) => _resolve().SequentialDiscardOrder(owner, start, ring);
    internal string SequentialDiscardReason(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SequentialDiscardReason(frame);
    internal global::CardGame.Core.CardConversionSource SequentialDiscardSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SequentialDiscardSource(f);
    internal bool TryReturnSequentialDiscardMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().TryReturnSequentialDiscardMovement(frame);
    internal long SequentialDiscardMovementSequence { get => _resolve().SequentialDiscardMovementSequence; }
}
