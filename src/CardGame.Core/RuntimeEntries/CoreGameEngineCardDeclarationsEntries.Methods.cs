#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardDeclarationsEntries
{
    internal void AddDeclarationSlashActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable) => _resolve().AddDeclarationSlashActions(actions, actor, playable);
    internal void AnswerCardDeclaration(PromptChoice choice) => _resolve().AnswerCardDeclaration(choice);
    internal Card? AvailableDeclarationCard(CharacterState owner) => _resolve().AvailableDeclarationCard(owner);
    internal bool CanChallengeDeclaration(int seat, int owner) => _resolve().CanChallengeDeclaration(seat, owner);
    internal bool CanDeclareCard(CharacterState owner, string skillId) => _resolve().CanDeclareCard(owner, skillId);
    internal CardLocation CapturedDeclarationOrigin(int cardId, CardLocation actual) => _resolve().CapturedDeclarationOrigin(cardId, actual);
    internal bool ClaimDeclarationPayment(Card card, CardLocation from, CardLocation to) => _resolve().ClaimDeclarationPayment(card, from, to);
    internal void ContinueCardDeclaration(long id) => _resolve().ContinueCardDeclaration(id);
    internal void ContinueCardDeclarationChallenge(long id) => _resolve().ContinueCardDeclarationChallenge(id);
    internal CardDeclarationParentReturn DeclarationReturn(CardDeclarationPurpose purpose, int actor, LegalAction? action = default, int? target = default, int recovery = 1, IReadOnlyList<ProgramRecoveryPolicySource>? policies = default, DyingResponseEvent? dying = default, bool fan = false, CardKind? finalKind = default) => _resolve().DeclarationReturn(purpose, actor, action, target, recovery, policies, dying, fan, finalKind);
    internal IReadOnlyList<CardDeclarationSnapshot>? GetCardDeclarationSnapshots(int viewer, bool revealAll) => _resolve().GetCardDeclarationSnapshots(viewer, revealAll);
    internal DeclaredCardPayment? LiveDeclarationPayment(int cardId) => _resolve().LiveDeclarationPayment(cardId);
    internal CardConversionSource? PeekDeclarationConversion(CharacterState owner, Card card, CardKind kind, bool use, CardConversionSource? explicitSource = default) => _resolve().PeekDeclarationConversion(owner, card, kind, use, explicitSource);
    internal string PublicDeclarationDescription(Card card, string ordinaryDescription) => _resolve().PublicDeclarationDescription(card, ordinaryDescription);
    internal void ReturnCardDeclaration(CardDeclarationFrame frame) => _resolve().ReturnCardDeclaration(frame);
    internal DeclaredCardPayment? TransferDeclarationToCardUse(long useId) => _resolve().TransferDeclarationToCardUse(useId);
    internal bool TryBeginCardDeclaration(CharacterState provider, Card card, CardKind kind, CardConversionSource? source, CardDeclarationParentReturn parent, IReadOnlyList<int> targets) => _resolve().TryBeginCardDeclaration(provider, card, kind, source, parent, targets);
    internal DeclaredCardPayment? UnclaimedDeclarationPayment(int provider, int cardId) => _resolve().UnclaimedDeclarationPayment(provider, cardId);
}
