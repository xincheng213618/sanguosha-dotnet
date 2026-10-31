#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanHaiBridgesEntries
{
    internal bool IsXieweiMultiTargetAttack(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsXieweiMultiTargetAttack(pending, attack, processing);
    internal bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().ProgramMultiCardColor(owner, selection);
    internal global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().ProgramMultiCardSuit(owner, selection);
}
