#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingMouMobileEntries
{
    internal bool AdvanceMouGnChildren() => _resolve().AdvanceMouGnChildren();
    internal void AnswerMouGn(PromptChoice choice) => _resolve().AnswerMouGn(choice);
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendMouGanNingWei(int seat, IReadOnlyList<PlayerMarkerSnapshot>? ordinary) => _resolve().AppendMouGanNingWei(seat, ordinary);
    internal SkillProgramStepOutcome BeginMouGn(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMouGn(supplied, op);
    internal bool CanOfferMouGnWei(int owner, ProgramSkillWindowContext c) => _resolve().CanOfferMouGnWei(owner, c);
    internal PromptChoice ChooseMouGnAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMouGnAi(p, f);
    internal void ContinueMouGn(long id) => _resolve().ContinueMouGn(id);
    internal bool DrainMouGn(ProgramSkillFrame f) => _resolve().DrainMouGn(f);
    internal void DrawMouGn(ProgramSkillFrame f) => _resolve().DrawMouGn(f);
    internal void FinishMouGn(ProgramSkillFrame f) => _resolve().FinishMouGn(f);
    internal bool IsIssuedMouGanNingPile(PublicPersistentPileSource p) => _resolve().IsIssuedMouGanNingPile(p);
    internal bool IsMouGnDying() => _resolve().IsMouGnDying();
    internal bool KeepMouGnAccepted(ProgramSkillFrame f) => _resolve().KeepMouGnAccepted(f);
    internal PromptChoice MouGnChoice(ProgramSkillFrame f, string branch, string description, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default, string? suit = default) => _resolve().MouGnChoice(f, branch, description, cards, targets, suit);
    internal IReadOnlyList<PromptChoice> MouGnChoices(ProgramSkillFrame f) => _resolve().MouGnChoices(f);
    internal bool MouGnFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouGnFirstChild(f, child);
    internal bool MouGnLionChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouGnLionChild(f, child);
    internal ProgramSkillFrame? MouGnObserverRoot() => _resolve().MouGnObserverRoot();
    internal CardUseFrame? MouGnParent(ProgramSkillWindowContext c) => _resolve().MouGnParent(c);
    internal bool MouGnRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouGnRecoveryParent(f, recovery);
    internal bool MouGnSuffix(int index) => _resolve().MouGnSuffix(index);
    internal MouGanNingWeiPlacedEvent? MouGnWei(int target, int? owner = default, string? instance = default) => _resolve().MouGnWei(target, owner, instance);
    internal void NextMouGnWei(ProgramSkillFrame f) => _resolve().NextMouGnWei(f);
    internal void PrepareMouGnMove(ref ProgramSkillFrame f, string purpose) => _resolve().PrepareMouGnMove(ref f, purpose);
    internal void PublishMouGn(ProgramSkillFrame f) => _resolve().PublishMouGn(f);
    internal bool ResumeMouGn(long id) => _resolve().ResumeMouGn(id);
    internal void ReturnMouGnDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnMouGnDraw(f, o);
    internal bool ReturnMouGnMovement(ProgramSkillFrame f) => _resolve().ReturnMouGnMovement(f);
    internal void SealMouGn(long id) => _resolve().SealMouGn(id);
    internal bool ValidMouGn(ProgramSkillFrame f) => _resolve().ValidMouGn(f);
    internal bool HasMouGanNingMobile { get => _resolve().HasMouGanNingMobile; }
}
