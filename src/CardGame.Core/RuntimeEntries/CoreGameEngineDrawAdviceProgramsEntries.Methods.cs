#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawAdviceProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDrawAdvice(global::CardGame.Core.ProgramSkillFrame input, int recipient) => _resolve().BeginDrawAdvice(input, recipient);
    internal bool CanContinueIssuedDrawAdvice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueIssuedDrawAdvice(frame);
    internal bool DrainDrawAdviceChildren(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainDrawAdviceChildren(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DrawAdviceChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrawAdviceChoices(frame);
    internal int DrawAdviceDiscardSeat(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrawAdviceDiscardSeat(frame);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> DrawAdviceDiscardableCards(int seat) => _resolve().DrawAdviceDiscardableCards(seat);
    internal bool DrawAdviceFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().DrawAdviceFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? DrawAdviceObserverRoot() => _resolve().DrawAdviceObserverRoot();
    internal bool ExactDrawAdviceParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactDrawAdviceParent(frame);
    internal bool IsDrawAdviceProgramDying() => _resolve().IsDrawAdviceProgramDying();
    internal void PublishDrawAdviceDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishDrawAdviceDiscard(frame);
    internal void ResolveDrawAdviceChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDrawAdviceChoice(selected);
    internal bool ResumeDrawAdvice(long id) => _resolve().ResumeDrawAdvice(id);
    internal bool ReturnDrawAdviceMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnDrawAdviceMovement(frame);
    internal bool ValidDrawAdviceReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidDrawAdviceReceipt(frame);
    internal long DrawAdviceSequence { get => _resolve().DrawAdviceSequence; }
}
