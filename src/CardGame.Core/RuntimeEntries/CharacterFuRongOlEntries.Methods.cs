#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFuRongOlEntries
{
    internal bool AdvanceFuRongSubtree() => _resolve().AdvanceFuRongSubtree();
    internal bool AllowsFuRongActivation(CharacterState actor) => _resolve().AllowsFuRongActivation(actor);
    internal void AnswerFuRongPair(PromptChoice selected) => _resolve().AnswerFuRongPair(selected);
    internal void AnswerFuRongUse(ProgramSkillFrame f, PromptChoice selected) => _resolve().AnswerFuRongUse(f, selected);
    internal IReadOnlyList<Card> AppendFuRongDiscardCards(CharacterState actor, IReadOnlyList<Card> held) => _resolve().AppendFuRongDiscardCards(actor, held);
    internal SkillProgramStepOutcome BeginFuRongPair(ProgramSkillFrame supplied, int target) => _resolve().BeginFuRongPair(supplied, target);
    internal void BindFuRongNativeUse(CardUseFrame use) => _resolve().BindFuRongNativeUse(use);
    internal bool ContinuePaidFuRongPair(ProgramSkillFrame f) => _resolve().ContinuePaidFuRongPair(f);
    internal bool DistancePhysicalSlash50HasIssuedFuRongDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash50HasIssuedFuRongDistance(q);
    internal bool DistancePhysicalSlash60HasFuRongDiscardDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash60HasFuRongDiscardDistance(q);
    internal bool DistanceVirtualSlash20HasFuRongDiscardDistance(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash20HasFuRongDiscardDistance(q);
    internal bool DrainFuRongChildren(ProgramSkillFrame f) => _resolve().DrainFuRongChildren(f);
    internal Card? FindFuRongDiscardCard(CharacterState actor, int id) => _resolve().FindFuRongDiscardCard(actor, id);
    internal void FinishFuRongPair(ProgramSkillFrame f) => _resolve().FinishFuRongPair(f);
    internal Dictionary<string, string> FuRongArgs(ProgramSkillFrame f, string branch) => _resolve().FuRongArgs(f, branch);
    internal FuRongPayment? FuRongCurrentCost(ProgramSkillFrame f) => _resolve().FuRongCurrentCost(f);
    internal ProgramSkillFrame? FuRongDiscardQueryOwner(int seat) => _resolve().FuRongDiscardQueryOwner(seat);
    internal bool FuRongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().FuRongFirstChild(f, child);
    internal bool FuRongHasCost(int seat) => _resolve().FuRongHasCost(seat);
    internal bool FuRongLegalCost(int seat, Card c, CardLocation from) => _resolve().FuRongLegalCost(seat, c, from);
    internal IReadOnlyList<LegalAction> FuRongNativeActions(ProgramSkillFrame f) => _resolve().FuRongNativeActions(f);
    internal ProgramSkillFrame? FuRongObserverRoot() => _resolve().FuRongObserverRoot();
    internal IReadOnlyList<PromptChoice> FuRongPaymentChoices(ProgramSkillFrame f) => _resolve().FuRongPaymentChoices(f);
    internal bool FuRongSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().FuRongSlashQuotaAfterLimit(query);
    internal CardConversionSource FuRongSource(ProgramSkillFrame f) => _resolve().FuRongSource(f);
    internal bool FuRongSourceCurrent(ProgramSkillFrame f) => _resolve().FuRongSourceCurrent(f);
    internal bool FuRongUnspentDiscard(ProgramSkillFrame f, int id) => _resolve().FuRongUnspentDiscard(f, id);
    internal IReadOnlyList<PromptChoice> FuRongUseChoices(ProgramSkillFrame f) => _resolve().FuRongUseChoices(f);
    internal bool HasFuRongDiscardDistance(CharacterState actor, CardKind kind, IReadOnlyList<int>? ids) => _resolve().HasFuRongDiscardDistance(actor, kind, ids);
    internal bool HasIssuedFuRongDistance(long? existingUseFrameId, int ownerSeat) => _resolve().HasIssuedFuRongDistance(existingUseFrameId, ownerSeat);
    internal bool HasIssuedFuRongUnlimitedUse(long id, int seat) => _resolve().HasIssuedFuRongUnlimitedUse(id, seat);
    internal bool IsFuRongDiscardQuery(CharacterState actor, int? id) => _resolve().IsFuRongDiscardQuery(actor, id);
    internal bool IsFuRongSelectedDiscard(int seat, int id) => _resolve().IsFuRongSelectedDiscard(seat, id);
    internal void PublishFuRongPayment(ProgramSkillFrame f) => _resolve().PublishFuRongPayment(f);
    internal void PublishFuRongUse(ProgramSkillFrame f) => _resolve().PublishFuRongUse(f);
    internal bool ResumeFuRongPair(long id) => _resolve().ResumeFuRongPair(id);
    internal bool ReturnFuRongMovement(ProgramSkillFrame f) => _resolve().ReturnFuRongMovement(f);
    internal bool ReturnFuRongNativeAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnFuRongNativeAttack(completion);
    internal void ReturnFuRongNativeUse(CardUseFrame use) => _resolve().ReturnFuRongNativeUse(use);
    internal PromptChoice SelectAiFuRongPair(PendingDecision prompt, ProgramSkillFrame f) => _resolve().SelectAiFuRongPair(prompt, f);
    internal void SettleFuRongNativeReturn(FuRongUseReturn ret) => _resolve().SettleFuRongNativeReturn(ret);
}
