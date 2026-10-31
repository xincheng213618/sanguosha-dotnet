#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicDamagePredictionEntries
{
    internal PlayerSkillContext CreateAiSkillContext(CharacterState player, int viewerSeat, GameSnapshot? snapshot = default) => _resolve().CreateAiSkillContext(player, viewerSeat, snapshot);
    internal Func<int, int, int>? CreatePublicSourceOccurrenceDamageBonus(int viewerSeat) => _resolve().CreatePublicSourceOccurrenceDamageBonus(viewerSeat);
    internal bool HasPredictionHpPolicy(CharacterState owner, SkillProgramCardPolicyKind kind, PlayerSkillContext context, IReadOnlySet<string>? visibleSkillIds = default) => _resolve().HasPredictionHpPolicy(owner, kind, context, visibleSkillIds);
    internal PublicDamagePrediction PredictPublicActionDamage(GameSnapshot view, int viewerSeat, LegalAction action, int targetSeat) => _resolve().PredictPublicActionDamage(view, viewerSeat, action, targetSeat);
    internal PublicDamagePrediction PredictPublicIncomingDamage(GameEngine.IDamageAttempt attack, int viewerSeat, int? sourceSeat = default, int? targetSeat = default, int? damageAmount = default) => _resolve().PredictPublicIncomingDamage(attack, viewerSeat, sourceSeat, targetSeat, damageAmount);
    internal PublicDamagePrediction PredictVisibleDamage(GameSnapshot view, int sourceSeat, int targetSeat, int amount, CardKind? kind, bool cardUse, bool sourceLess, bool chain, bool finalized, int paidHandCards, bool ignoresEquipmentArmor, DamageNature? damageNature = default, GameEngine.IDamageAttempt? issuedAttack = default, bool wushuangTactics = false, Suit prospectiveSuit = (Suit)500, int additionalIncomingSegments = 0, int priorTargetSegments = 0) => _resolve().PredictVisibleDamage(view, sourceSeat, targetSeat, amount, kind, cardUse, sourceLess, chain, finalized, paidHandCards, ignoresEquipmentArmor, damageNature, issuedAttack, wushuangTactics, prospectiveSuit, additionalIncomingSegments, priorTargetSegments);
}
