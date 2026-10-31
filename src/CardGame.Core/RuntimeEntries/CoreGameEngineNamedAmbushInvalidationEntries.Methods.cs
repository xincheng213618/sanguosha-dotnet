#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedAmbushInvalidationEntries
{
    internal void BindNamedAmbushCounterspell(long id, CardActionContext action) => _resolve().BindNamedAmbushCounterspell(id, action);
    internal NullificationWindowFrame CompleteNamedAmbushCounterspell(NullificationWindowFrame pending) => _resolve().CompleteNamedAmbushCounterspell(pending);
    internal bool ContinueNamedAmbushIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().ContinueNamedAmbushIneffectiveDodge(attack, action, continuation);
    internal bool HasEnabledMatchingNamedAmbush(int actor, CardKind kind) => _resolve().HasEnabledMatchingNamedAmbush(actor, kind);
    internal void InvalidateNamedAmbushUse(ProgramSkillFrame f) => _resolve().InvalidateNamedAmbushUse(f);
    internal bool IsDeferredZeroCounterspell(NullificationWindowFrame pending, CardActionContext action, NamedAmbushCounterspellResponse response) => _resolve().IsDeferredZeroCounterspell(pending, action, response);
    internal bool IsExactNamedAmbushInvalidation(NamedAmbushUseInvalidatedEvent fact, CardActionContext action, long parent) => _resolve().IsExactNamedAmbushInvalidation(fact, action, parent);
    internal bool IsNamedAmbushUseIneffective(CardUseFrame use) => _resolve().IsNamedAmbushUseIneffective(use);
    internal NullificationWindowFrame PrepareNamedAmbushCounterspell(NullificationWindowFrame pending, int responder, int cardId) => _resolve().PrepareNamedAmbushCounterspell(pending, responder, cardId);
}
