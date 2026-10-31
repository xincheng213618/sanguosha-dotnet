#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoEntries
{
    internal void AnswerSourceMou(PromptChoice choice) => _resolve().AnswerSourceMou(choice);
    internal void ContinueSourceMou(long id) => _resolve().ContinueSourceMou(id);
    internal bool DrainSourceMou(ProgramSkillFrame f) => _resolve().DrainSourceMou(f);
    internal void FinishSourceMou(ProgramSkillFrame f) => _resolve().FinishSourceMou(f);
    internal void GiveSourceMou(ProgramSkillFrame f, int recipient) => _resolve().GiveSourceMou(f, recipient);
    internal bool KeepPaidSourceMou(ProgramSkillFrame f) => _resolve().KeepPaidSourceMou(f);
    internal void LoseSourceMouGuanbian(ProgramSkillFrame f) => _resolve().LoseSourceMouGuanbian(f);
    internal void OfferSourceMouPeer(ProgramSkillFrame f, int cursor) => _resolve().OfferSourceMouPeer(f, cursor);
    internal void PaySourceMouDiscard(ProgramSkillFrame f, ShanheSourceMouStage stage) => _resolve().PaySourceMouDiscard(f, stage);
    internal void PublishSourceMou(ProgramSkillFrame f) => _resolve().PublishSourceMou(f);
    internal void ReduceSourceMouMaximum(ProgramSkillFrame f, int targetSeat) => _resolve().ReduceSourceMouMaximum(f, targetSeat);
    internal bool ResumeSourceMou(long id) => _resolve().ResumeSourceMou(id);
    internal bool ReturnSourceMouMovement(ProgramSkillFrame f) => _resolve().ReturnSourceMouMovement(f);
    internal bool SourceMouActivation(CharacterState owner) => _resolve().SourceMouActivation(owner);
    internal PromptChoice SourceMouAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().SourceMouAi(decision, f);
    internal ShanheSourceMouCard SourceMouCard(int seat, Card card, CardLocation from, bool effective) => _resolve().SourceMouCard(seat, card, from, effective);
    internal PromptChoice SourceMouChoice(ProgramSkillFrame f, string branch, string label, int card = 0, int target = -1) => _resolve().SourceMouChoice(f, branch, label, card, target);
    internal IReadOnlyList<PromptChoice> SourceMouChoices(ProgramSkillFrame f) => _resolve().SourceMouChoices(f);
    internal int SourceMouChooser(ProgramSkillFrame f) => _resolve().SourceMouChooser(f);
    internal ShanheSourceMouCard[] SourceMouDiscardPile() => _resolve().SourceMouDiscardPile();
    internal ShanheSourceMouCard[] SourceMouDiscardable(int seat, bool handOnly = false) => _resolve().SourceMouDiscardable(seat, handOnly);
    internal bool SourceMouMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().SourceMouMovement(f, effect, pending);
    internal bool SourceMouParent(ProgramSkillFrame f) => _resolve().SourceMouParent(f);
    internal CardConversionSource SourceMouSource(ProgramSkillFrame f) => _resolve().SourceMouSource(f);
    internal bool SourceMouTrigger(ProgramTriggerLegalityContext c) => _resolve().SourceMouTrigger(c);
    internal SkillProgramStepOutcome StartSourceMou(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().StartSourceMou(supplied, operation);
    internal bool ValidSourceMou(ProgramSkillFrame f) => _resolve().ValidSourceMou(f);
    internal bool IsSourceMouMode { get => _resolve().IsSourceMouMode; }
}
