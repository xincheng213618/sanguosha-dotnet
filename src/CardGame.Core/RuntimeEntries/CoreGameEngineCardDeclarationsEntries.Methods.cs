#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardDeclarationsEntries
{
    internal void AddDeclarationSlashActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playable) => _resolve().AddDeclarationSlashActions(actions, actor, playable);
    internal void AnswerCardDeclaration(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCardDeclaration(choice);
    internal bool AssertCardDeclarationInvariant() => _resolve().AssertCardDeclarationInvariant();
    internal global::CardGame.Core.Card? AvailableDeclarationCard(global::CardGame.Core.CharacterState owner) => _resolve().AvailableDeclarationCard(owner);
    internal bool CanChallengeDeclaration(int seat, int owner) => _resolve().CanChallengeDeclaration(seat, owner);
    internal bool CanDeclareCard(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().CanDeclareCard(owner, skillId);
    internal global::CardGame.Core.CardLocation CapturedDeclarationOrigin(int cardId, global::CardGame.Core.CardLocation actual) => _resolve().CapturedDeclarationOrigin(cardId, actual);
    internal bool ClaimDeclarationPayment(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().ClaimDeclarationPayment(card, from, to);
    internal void ContinueCardDeclaration(long id) => _resolve().ContinueCardDeclaration(id);
    internal void ContinueCardDeclarationChallenge(long id) => _resolve().ContinueCardDeclarationChallenge(id);
    internal global::CardGame.Core.CardDeclarationParentReturn DeclarationReturn(global::CardGame.Core.CardDeclarationPurpose purpose, int actor, global::CardGame.Core.LegalAction? action = default, int? target = default, int recovery = 1, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource>? policies = default, global::CardGame.Core.DyingResponseEvent? dying = default, bool fan = false, global::CardGame.Core.CardKind? finalKind = default) => _resolve().DeclarationReturn(purpose, actor, action, target, recovery, policies, dying, fan, finalKind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardDeclarationSnapshot>? GetCardDeclarationSnapshots(int viewer, bool revealAll) => _resolve().GetCardDeclarationSnapshots(viewer, revealAll);
    internal global::CardGame.Core.DeclaredCardPayment? LiveDeclarationPayment(int cardId) => _resolve().LiveDeclarationPayment(cardId);
    internal global::CardGame.Core.CardConversionSource? PeekDeclarationConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool use, global::CardGame.Core.CardConversionSource? explicitSource = default) => _resolve().PeekDeclarationConversion(owner, card, kind, use, explicitSource);
    internal string PublicDeclarationDescription(global::CardGame.Core.Card card, string ordinaryDescription) => _resolve().PublicDeclarationDescription(card, ordinaryDescription);
    internal void ReturnCardDeclaration(global::CardGame.Core.CardDeclarationFrame frame) => _resolve().ReturnCardDeclaration(frame);
    internal global::CardGame.Core.DeclaredCardPayment? TransferDeclarationToCardUse(long useId) => _resolve().TransferDeclarationToCardUse(useId);
    internal bool TryBeginCardDeclaration(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().TryBeginCardDeclaration(provider, card, kind, source, parent, targets);
    internal global::CardGame.Core.DeclaredCardPayment? UnclaimedDeclarationPayment(int provider, int cardId) => _resolve().UnclaimedDeclarationPayment(provider, cardId);
}
