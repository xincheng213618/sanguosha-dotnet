#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoEntries
{
    internal void AnswerSourceMou(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerSourceMou(choice);
    internal void ContinueSourceMou(long id) => _resolve().ContinueSourceMou(id);
    internal bool DrainSourceMou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainSourceMou(f);
    internal void FinishSourceMou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSourceMou(f);
    internal void GiveSourceMou(global::CardGame.Core.ProgramSkillFrame f, int recipient) => _resolve().GiveSourceMou(f, recipient);
    internal bool KeepPaidSourceMou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidSourceMou(f);
    internal void LoseSourceMouGuanbian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LoseSourceMouGuanbian(f);
    internal void OfferSourceMouPeer(global::CardGame.Core.ProgramSkillFrame f, int cursor) => _resolve().OfferSourceMouPeer(f, cursor);
    internal void PaySourceMouDiscard(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShanheSourceMouStage stage) => _resolve().PaySourceMouDiscard(f, stage);
    internal void PublishSourceMou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSourceMou(f);
    internal void ReduceSourceMouMaximum(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().ReduceSourceMouMaximum(f, targetSeat);
    internal bool ResumeSourceMou(long id) => _resolve().ResumeSourceMou(id);
    internal bool ReturnSourceMouMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSourceMouMovement(f);
    internal bool SourceMouActivation(global::CardGame.Core.CharacterState owner) => _resolve().SourceMouActivation(owner);
    internal global::CardGame.Core.PromptChoice SourceMouAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceMouAi(decision, f);
    internal global::CardGame.Core.ShanheSourceMouCard SourceMouCard(int seat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, bool effective) => _resolve().SourceMouCard(seat, card, from, effective);
    internal global::CardGame.Core.PromptChoice SourceMouChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int card = 0, int target = -1) => _resolve().SourceMouChoice(f, branch, label, card, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SourceMouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceMouChoices(f);
    internal int SourceMouChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceMouChooser(f);
    internal global::CardGame.Core.ShanheSourceMouCard[] SourceMouDiscardPile() => _resolve().SourceMouDiscardPile();
    internal global::CardGame.Core.ShanheSourceMouCard[] SourceMouDiscardable(int seat, bool handOnly = false) => _resolve().SourceMouDiscardable(seat, handOnly);
    internal bool SourceMouMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().SourceMouMovement(f, effect, pending);
    internal bool SourceMouParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceMouParent(f);
    internal global::CardGame.Core.CardConversionSource SourceMouSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceMouSource(f);
    internal bool SourceMouTrigger(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().SourceMouTrigger(c);
    internal global::CardGame.Core.SkillProgramStepOutcome StartSourceMou(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().StartSourceMou(supplied, operation);
    internal bool ValidSourceMou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSourceMou(f);
    internal bool IsSourceMouMode { get => _resolve().IsSourceMouMode; }
}
