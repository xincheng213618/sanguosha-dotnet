#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingMouMobileEntries
{
    internal bool AdvanceMouGnChildren() => _resolve().AdvanceMouGnChildren();
    internal void AnswerMouGn(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouGn(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? AppendMouGanNingWei(int seat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? ordinary) => _resolve().AppendMouGanNingWei(seat, ordinary);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouGn(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMouGn(supplied, op);
    internal bool CanOfferMouGnWei(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferMouGnWei(owner, c);
    internal global::CardGame.Core.PromptChoice ChooseMouGnAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMouGnAi(p, f);
    internal void ContinueMouGn(long id) => _resolve().ContinueMouGn(id);
    internal bool DrainMouGn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouGn(f);
    internal void DrawMouGn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrawMouGn(f);
    internal void FinishMouGn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouGn(f);
    internal bool IsIssuedMouGanNingPile(global::CardGame.Core.PublicPersistentPileSource p) => _resolve().IsIssuedMouGanNingPile(p);
    internal bool IsMouGnDying() => _resolve().IsMouGnDying();
    internal bool KeepMouGnAccepted(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMouGnAccepted(f);
    internal global::CardGame.Core.PromptChoice MouGnChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string description, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default, string? suit = default) => _resolve().MouGnChoice(f, branch, description, cards, targets, suit);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouGnChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouGnChoices(f);
    internal bool MouGnFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouGnFirstChild(f, child);
    internal bool MouGnLionChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouGnLionChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MouGnObserverRoot() => _resolve().MouGnObserverRoot();
    internal global::CardGame.Core.CardUseFrame? MouGnParent(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().MouGnParent(c);
    internal bool MouGnRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouGnRecoveryParent(f, recovery);
    internal bool MouGnSuffix(int index) => _resolve().MouGnSuffix(index);
    internal global::CardGame.Core.MouGanNingWeiPlacedEvent? MouGnWei(int target, int? owner = default, string? instance = default) => _resolve().MouGnWei(target, owner, instance);
    internal void NextMouGnWei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NextMouGnWei(f);
    internal void PrepareMouGnMove(ref global::CardGame.Core.ProgramSkillFrame f, string purpose) => _resolve().PrepareMouGnMove(ref f, purpose);
    internal void PublishMouGn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouGn(f);
    internal bool ResumeMouGn(long id) => _resolve().ResumeMouGn(id);
    internal void ReturnMouGnDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnMouGnDraw(f, o);
    internal bool ReturnMouGnMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouGnMovement(f);
    internal void SealMouGn(long id) => _resolve().SealMouGn(id);
    internal bool ValidMouGn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouGn(f);
    internal bool HasMouGanNingMobile { get => _resolve().HasMouGanNingMobile; }
}
