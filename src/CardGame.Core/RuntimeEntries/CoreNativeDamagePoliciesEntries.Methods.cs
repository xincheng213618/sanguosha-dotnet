#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamagePoliciesEntries
{
    internal int FinalizeAttackDamageAmount(GameEngine.IDamageAttempt attack) => _resolve().FinalizeAttackDamageAmount(attack);
    internal IReadOnlyList<(CardUseEffectSource Source, int Amount)> GetActualPassiveProgramDamageModifiers(GameEngine.IDamageAttempt attack) => _resolve().GetActualPassiveProgramDamageModifiers(attack);
    internal DamageNature GetDamageNatureBeforeMobileMaChao(GameEngine.IDamageAttempt attack) => _resolve().GetDamageNatureBeforeMobileMaChao(attack);
    internal IReadOnlyList<(CardUseEffectSource Source, int Amount)> GetPassiveProgramDamageModifiers(GameEngine.IDamageAttempt attack, bool cardDamageOnly = false) => _resolve().GetPassiveProgramDamageModifiers(attack, cardDamageOnly);
    internal ProgramDamageInterception InterceptWuyanDamage(GameEngine.IDamageAttempt attack, int amount, DamageNature nature) => _resolve().InterceptWuyanDamage(attack, amount, nature);
    internal bool IsCardTargetProhibited(CharacterState target, CardKind cardKind, Suit? suit = default, bool? effectiveIsRed = default) => _resolve().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
    internal bool IsSlashProhibited(CharacterState target) => _resolve().IsSlashProhibited(target);
    internal bool IsSlashProhibited2(CharacterState source, CharacterState target, Card slashCard) => _resolve().IsSlashProhibited(source, target, slashCard);
    internal bool TryPreventWuyanDamage(GameEngine.IDamageAttempt attack, int amount) => _resolve().TryPreventWuyanDamage(attack, amount);
}
