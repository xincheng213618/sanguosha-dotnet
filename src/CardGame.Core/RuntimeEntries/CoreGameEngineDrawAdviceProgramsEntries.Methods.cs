#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawAdviceProgramsEntries
{
    internal SkillProgramStepOutcome BeginDrawAdvice(ProgramSkillFrame input, int recipient) => _resolve().BeginDrawAdvice(input, recipient);
    internal bool CanContinueIssuedDrawAdvice(ProgramSkillFrame frame) => _resolve().CanContinueIssuedDrawAdvice(frame);
    internal bool DrainDrawAdviceChildren(ProgramSkillFrame frame) => _resolve().DrainDrawAdviceChildren(frame);
    internal IReadOnlyList<PromptChoice> DrawAdviceChoices(ProgramSkillFrame frame) => _resolve().DrawAdviceChoices(frame);
    internal int DrawAdviceDiscardSeat(ProgramSkillFrame frame) => _resolve().DrawAdviceDiscardSeat(frame);
    internal IEnumerable<Card> DrawAdviceDiscardableCards(int seat) => _resolve().DrawAdviceDiscardableCards(seat);
    internal bool DrawAdviceFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().DrawAdviceFirstChild(frame, child);
    internal ProgramSkillFrame? DrawAdviceObserverRoot() => _resolve().DrawAdviceObserverRoot();
    internal bool ExactDrawAdviceParent(ProgramSkillFrame frame) => _resolve().ExactDrawAdviceParent(frame);
    internal bool IsDrawAdviceProgramDying() => _resolve().IsDrawAdviceProgramDying();
    internal void PublishDrawAdviceDiscard(ProgramSkillFrame frame) => _resolve().PublishDrawAdviceDiscard(frame);
    internal void ResolveDrawAdviceChoice(PromptChoice selected) => _resolve().ResolveDrawAdviceChoice(selected);
    internal bool ResumeDrawAdvice(long id) => _resolve().ResumeDrawAdvice(id);
    internal bool ReturnDrawAdviceMovement(ProgramSkillFrame frame) => _resolve().ReturnDrawAdviceMovement(frame);
    internal bool ValidDrawAdviceReceipt(ProgramSkillFrame frame) => _resolve().ValidDrawAdviceReceipt(frame);
    internal long DrawAdviceSequence { get => _resolve().DrawAdviceSequence; }
}
