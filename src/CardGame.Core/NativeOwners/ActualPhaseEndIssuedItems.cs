#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualPhaseEndIssuedItems(Func<RuntimeEntries.CharacterYaDanOlEntries> getCharacterYaDanOlRuntime, Func<RuntimeEntries.CharacterSimaShiMouEntries> getCharacterSimaShiMouRuntime, Func<RuntimeEntries.CharacterHuangZuMobileShiEntries> getCharacterHuangZuMobileShiRuntime, Func<RuntimeEntries.CharacterLuYuShengMobileShiEntries> getCharacterLuYuShengMobileShiRuntime) : IActualPhaseEndIssuedItems
{
    public IReadOnlyList<ActualPhaseEndItem> Qingya(ActualDiscardRecoveryPhaseKey phase) => getCharacterYaDanOlRuntime().CollectQingyaPhaseEndCandidates(phase);
    public IReadOnlyList<ActualPhaseEndItem> MouSimaShi(ActualDiscardRecoveryPhaseKey phase) => getCharacterSimaShiMouRuntime().CollectMouSimaShiPhaseEndCandidates(phase);
    public IReadOnlyList<ActualPhaseEndItem> ShiHuangZu(ActualDiscardRecoveryPhaseKey phase) => getCharacterHuangZuMobileShiRuntime().CollectShiHzPhaseEndCandidates(phase);
    public IReadOnlyList<ActualPhaseEndItem> ShiLuYuSheng(ActualDiscardRecoveryPhaseKey phase) => getCharacterLuYuShengMobileShiRuntime().CollectShiLysPhaseEndCandidates(phase);
}
