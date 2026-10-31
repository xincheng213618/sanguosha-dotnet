#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardContinuationCursorsEntries
{
    internal NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => _resolve().ActiveBorrowedSword; set => _resolve().ActiveBorrowedSword = value; }
    internal NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords { get => _resolve().ActiveCixiongDoubleSwords; set => _resolve().ActiveCixiongDoubleSwords = value; }
    internal NativeOwners.DuelHandle? ActiveDuel { get => _resolve().ActiveDuel; set => _resolve().ActiveDuel = value; }
    internal NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => _resolve().ActiveFactionCardRequest; set => _resolve().ActiveFactionCardRequest = value; }
    internal NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => _resolve().ActiveFactionDefense; set => _resolve().ActiveFactionDefense = value; }
    internal NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get => _resolve().ActiveFangtianHalberd; set => _resolve().ActiveFangtianHalberd = value; }
    internal CardUseFrame? ActiveFireAttack { get => _resolve().ActiveFireAttack; }
    internal NativeOwners.GroupCardHandle? ActiveGroupCard { get => _resolve().ActiveGroupCard; set => _resolve().ActiveGroupCard = value; }
    internal NativeOwners.IceSwordHandle? ActiveIceSword { get => _resolve().ActiveIceSword; set => _resolve().ActiveIceSword = value; }
    internal NullificationWindowFrame? ActiveNullificationWindow { get => _resolve().ActiveNullificationWindow; }
    internal NativeOwners.QilinBowHandle? ActiveQilinBow { get => _resolve().ActiveQilinBow; set => _resolve().ActiveQilinBow = value; }
    internal NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get => _resolve().ActiveQinglongCrescentBlade; set => _resolve().ActiveQinglongCrescentBlade = value; }
    internal NativeOwners.QinglongFollowupHandle? ActiveQinglongFollowup { get => _resolve().ActiveQinglongFollowup; set => _resolve().ActiveQinglongFollowup = value; }
    internal NativeOwners.StoneAxeHandle? ActiveStoneAxe { get => _resolve().ActiveStoneAxe; set => _resolve().ActiveStoneAxe = value; }
    internal TargetCardSelectionFrame? ActiveTargetCardSelection { get => _resolve().ActiveTargetCardSelection; }
}
