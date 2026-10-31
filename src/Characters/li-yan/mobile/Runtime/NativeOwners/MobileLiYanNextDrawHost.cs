#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileLiYanNextDrawHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiYanMobileEntries> getCharacterLiYanMobileRuntime) : IMobileLiYanNextDrawHost
{
    public void Issue(ProgramSkillFrame frame, int targetSeat, int amount) => getCharacterLiYanMobileRuntime().IssueMobileLiYanNextDrawBonus(frame, targetSeat, amount);
}
