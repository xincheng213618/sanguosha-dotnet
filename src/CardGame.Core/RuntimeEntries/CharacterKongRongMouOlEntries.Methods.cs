#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKongRongMouOlEntries
{
    internal void BindMouKrUse(CardUseDeclaredEvent fact) => _resolve().BindMouKrUse(fact);
    internal bool IsMouKrLiwenPaymentFrame(ChainedStateBasicFrame paid) => _resolve().IsMouKrLiwenPaymentFrame(paid);
    internal bool IsMouKrLiwenPaymentFrame2(DrawFundedDistinctBasicFrame paid) => _resolve().IsMouKrLiwenPaymentFrame(paid);
    internal bool IsMouKrLiwenUseQuery(CharacterState actor) => _resolve().IsMouKrLiwenUseQuery(actor);
    internal void IssueMouKrUse(ProgramSkillFrame f, int actor, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().IssueMouKrUse(f, actor, action, cards, targets);
    internal IReadOnlyList<LegalAction> MouKrActions(ProgramSkillFrame f, int actor) => _resolve().MouKrActions(f, actor);
    internal bool MouKrLiwenPaymentParent(long paymentId, long? parentId, long? requestId, int cursor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets) => _resolve().MouKrLiwenPaymentParent(paymentId, parentId, requestId, cursor, source, kind, targets);
    internal ProgramSkillFrame? MouKrLiwenSlotParent(EquipmentSlotNamedUseDraft slot) => _resolve().MouKrLiwenSlotParent(slot);
    internal bool MouKrNativeMatches(CardUseFrame use, ProgramSkillFrame f) => _resolve().MouKrNativeMatches(use, f);
    internal bool MouKrNoSlashDebit(long id, int actor) => _resolve().MouKrNoSlashDebit(id, actor);
    internal bool MouKrParentMatches(ProgramSkillFrame f) => _resolve().MouKrParentMatches(f);
    internal bool MouKrRequestedQuota(ProgramSlashQuotaQuery q) => _resolve().MouKrRequestedQuota(q);
    internal ProgramSkillFrame? MouKrUseQueryFrame(int actor) => _resolve().MouKrUseQueryFrame(actor);
    internal void ObserveMouKrAction(IGameEvent payload) => _resolve().ObserveMouKrAction(payload);
    internal bool ReturnMouKrAttack() => _resolve().ReturnMouKrAttack();
    internal void ReturnMouKrUse(CardUseFrame use) => _resolve().ReturnMouKrUse(use);
    internal void SettleMouKrUse(ProgramSkillFrame f) => _resolve().SettleMouKrUse(f);
    internal bool TryCancelMouKrLiwenPayment(long id) => _resolve().TryCancelMouKrLiwenPayment(id);
    internal void ValidateMouKrUse(CardUseFrame use) => _resolve().ValidateMouKrUse(use);
    internal bool HasMouKongRong { get => _resolve().HasMouKongRong; }
    internal MouKongRongRuntime MouKongRong { get => _resolve().MouKongRong; }
}
