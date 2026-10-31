#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiYiNativeQueriesEntries
{
    internal bool HasLiYiProspectiveWeaponSlashTarget(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target) => _resolve().HasLiYiProspectiveWeaponSlashTarget(actor, target);
    internal bool HasLiYiWeaponPaymentRange(global::CardGame.Core.CharacterState actor, int targetSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().HasLiYiWeaponPaymentRange(actor, targetSeat, cards);
    internal int? LiYiPaidSlashWeapon(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> materialIds) => _resolve().LiYiPaidSlashWeapon(actor, materialIds);
}
