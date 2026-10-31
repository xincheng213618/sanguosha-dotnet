#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiYanMobileEntries
{
    internal void AssignMobileLiYanNextDrawBonus(global::CardGame.Core.ActualDiscardRecoveryPhaseStartedEvent e) => _resolve().AssignMobileLiYanNextDrawBonus(e);
    internal void IssueMobileLiYanNextDrawBonus(global::CardGame.Core.ProgramSkillFrame supplied, int targetSeat, int amount) => _resolve().IssueMobileLiYanNextDrawBonus(supplied, targetSeat, amount);
    internal int MobileLiYanNextDrawBonus(global::CardGame.Core.CharacterState player) => _resolve().MobileLiYanNextDrawBonus(player);
}
