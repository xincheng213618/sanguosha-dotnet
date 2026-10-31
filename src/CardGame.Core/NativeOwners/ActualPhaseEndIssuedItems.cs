#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualPhaseEndIssuedItems(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYaDanOlEntries> getCharacterYaDanOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaShiMouEntries> getCharacterSimaShiMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries> getCharacterHuangZuMobileShiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuYuShengMobileShiEntries> getCharacterLuYuShengMobileShiRuntime) : IActualPhaseEndIssuedItems
{
    public IReadOnlyList<ActualPhaseEndItem> Qingya(ActualDiscardRecoveryPhaseKey phase) => getCharacterYaDanOlRuntime().CollectQingyaPhaseEndCandidates(phase);
    public IReadOnlyList<ActualPhaseEndItem> MouSimaShi(ActualDiscardRecoveryPhaseKey phase) => getCharacterSimaShiMouRuntime().CollectMouSimaShiPhaseEndCandidates(phase);
    public IReadOnlyList<ActualPhaseEndItem> ShiHuangZu(ActualDiscardRecoveryPhaseKey phase) => getCharacterHuangZuMobileShiRuntime().CollectShiHzPhaseEndCandidates(phase);
    public IReadOnlyList<ActualPhaseEndItem> ShiLuYuSheng(ActualDiscardRecoveryPhaseKey phase) => getCharacterLuYuShengMobileShiRuntime().CollectShiLysPhaseEndCandidates(phase);
}
