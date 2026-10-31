#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedAmbushInvalidationEntries
{
    internal void BindNamedAmbushCounterspell(long id, global::CardGame.Core.CardActionContext action) => _resolve().BindNamedAmbushCounterspell(id, action);
    internal global::CardGame.Core.NullificationWindowFrame CompleteNamedAmbushCounterspell(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().CompleteNamedAmbushCounterspell(pending);
    internal bool ContinueNamedAmbushIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().ContinueNamedAmbushIneffectiveDodge(attack, action, continuation);
    internal bool HasEnabledMatchingNamedAmbush(int actor, global::CardGame.Core.CardKind kind) => _resolve().HasEnabledMatchingNamedAmbush(actor, kind);
    internal void InvalidateNamedAmbushUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().InvalidateNamedAmbushUse(f);
    internal bool IsDeferredZeroCounterspell(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action, global::CardGame.Core.NamedAmbushCounterspellResponse response) => _resolve().IsDeferredZeroCounterspell(pending, action, response);
    internal bool IsExactNamedAmbushInvalidation(global::CardGame.Core.NamedAmbushUseInvalidatedEvent fact, global::CardGame.Core.CardActionContext action, long parent) => _resolve().IsExactNamedAmbushInvalidation(fact, action, parent);
    internal bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsNamedAmbushUseIneffective(use);
    internal global::CardGame.Core.NullificationWindowFrame PrepareNamedAmbushCounterspell(global::CardGame.Core.NullificationWindowFrame pending, int responder, int cardId) => _resolve().PrepareNamedAmbushCounterspell(pending, responder, cardId);
}
