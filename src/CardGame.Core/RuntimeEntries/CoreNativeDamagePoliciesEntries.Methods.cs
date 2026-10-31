#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamagePoliciesEntries
{
    internal int FinalizeAttackDamageAmount(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().FinalizeAttackDamageAmount(attack);
    internal global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> GetActualPassiveProgramDamageModifiers(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().GetActualPassiveProgramDamageModifiers(attack);
    internal global::CardGame.Core.DamageNature GetDamageNatureBeforeMobileMaChao(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().GetDamageNatureBeforeMobileMaChao(attack);
    internal global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> GetPassiveProgramDamageModifiers(global::CardGame.Core.GameEngine.IDamageAttempt attack, bool cardDamageOnly = false) => _resolve().GetPassiveProgramDamageModifiers(attack, cardDamageOnly);
    internal global::CardGame.Core.ProgramDamageInterception InterceptWuyanDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature) => _resolve().InterceptWuyanDamage(attack, amount, nature);
    internal bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => _resolve().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
    internal bool IsSlashProhibited(global::CardGame.Core.CharacterState target) => _resolve().IsSlashProhibited(target);
    internal bool IsSlashProhibited2(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard) => _resolve().IsSlashProhibited(source, target, slashCard);
    internal bool TryPreventWuyanDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => _resolve().TryPreventWuyanDamage(attack, amount);
}
