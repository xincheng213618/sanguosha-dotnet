#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileLiYanNextDrawHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiYanMobileRuntime>> getCharacterLiYanMobileRuntime) : IMobileLiYanNextDrawHost
{
    public void Issue(ProgramSkillFrame frame, int targetSeat, int amount) => getCharacterLiYanMobileRuntime().Value.IssueMobileLiYanNextDrawBonus(frame, targetSeat, amount);
}
