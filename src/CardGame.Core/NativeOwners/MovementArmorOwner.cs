#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementArmorOwner(Func<RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries> getCharacterDaQiaoXiaoQiaoHappyGodRuntime, Func<RuntimeEntries.CoreGameEngineExtendedViewAsEntries> getCoreGameEngineExtendedViewAsRuntime, Func<RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, CharacterState owner) : IMovementArmorOwner
{
    public bool Alive => owner.IsAlive;
    public bool HasHpValue => owner.HasHpValue;
    public bool Wounded => getCharacterDaQiaoXiaoQiaoHappyGodRuntime().HappyQiaoAnyWounded(owner.Seat);
    public bool ArmorIneffective => getCoreGameEngineExtendedViewAsRuntime().IsArmorIneffectiveForTurn(owner);
    public bool EquipmentDisabled => getCharacterHuCheErMouRuntime().MouHuCheErEquipmentDisabled(owner.Seat);
    public int Hp => owner.Hp;
    public string Name => owner.Name;

    public void RecoverOne() => owner.Hp++;
}
