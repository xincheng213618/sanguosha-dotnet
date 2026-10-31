#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiYiNativeQueriesEntries
{
    internal bool HasLiYiProspectiveWeaponSlashTarget(CharacterState actor, CharacterState target) => _resolve().HasLiYiProspectiveWeaponSlashTarget(actor, target);
    internal bool HasLiYiWeaponPaymentRange(CharacterState actor, int targetSeat, IReadOnlyList<Card> cards) => _resolve().HasLiYiWeaponPaymentRange(actor, targetSeat, cards);
    internal int? LiYiPaidSlashWeapon(CharacterState actor, IReadOnlyList<int> materialIds) => _resolve().LiYiPaidSlashWeapon(actor, materialIds);
}
