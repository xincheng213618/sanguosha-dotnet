#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKongRongMouOlEntries
{
    internal void BindMouKrUse(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().BindMouKrUse(fact);
    internal bool IsMouKrLiwenPaymentFrame(global::CardGame.Core.ChainedStateBasicFrame paid) => _resolve().IsMouKrLiwenPaymentFrame(paid);
    internal bool IsMouKrLiwenPaymentFrame2(global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => _resolve().IsMouKrLiwenPaymentFrame(paid);
    internal bool IsMouKrLiwenUseQuery(global::CardGame.Core.CharacterState actor) => _resolve().IsMouKrLiwenUseQuery(actor);
    internal void IssueMouKrUse(global::CardGame.Core.ProgramSkillFrame f, int actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().IssueMouKrUse(f, actor, action, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> MouKrActions(global::CardGame.Core.ProgramSkillFrame f, int actor) => _resolve().MouKrActions(f, actor);
    internal bool MouKrLiwenPaymentParent(long paymentId, long? parentId, long? requestId, int cursor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().MouKrLiwenPaymentParent(paymentId, parentId, requestId, cursor, source, kind, targets);
    internal global::CardGame.Core.ProgramSkillFrame? MouKrLiwenSlotParent(global::CardGame.Core.EquipmentSlotNamedUseDraft slot) => _resolve().MouKrLiwenSlotParent(slot);
    internal bool MouKrNativeMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouKrNativeMatches(use, f);
    internal bool MouKrNoSlashDebit(long id, int actor) => _resolve().MouKrNoSlashDebit(id, actor);
    internal bool MouKrParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouKrParentMatches(f);
    internal bool MouKrRequestedQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().MouKrRequestedQuota(q);
    internal global::CardGame.Core.ProgramSkillFrame? MouKrUseQueryFrame(int actor) => _resolve().MouKrUseQueryFrame(actor);
    internal void ObserveMouKrAction(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouKrAction(payload);
    internal bool ReturnMouKrAttack() => _resolve().ReturnMouKrAttack();
    internal void ReturnMouKrUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouKrUse(use);
    internal void SettleMouKrUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleMouKrUse(f);
    internal bool TryCancelMouKrLiwenPayment(long id) => _resolve().TryCancelMouKrLiwenPayment(id);
    internal void ValidateMouKrUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMouKrUse(use);
    internal bool HasMouKongRong { get => _resolve().HasMouKongRong; }
    internal global::CardGame.Core.MouKongRongRuntime MouKongRong { get => _resolve().MouKongRong; }
}
