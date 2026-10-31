#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiYanMobileEntries
{
    internal void AssignMobileLiYanNextDrawBonus(ActualDiscardRecoveryPhaseStartedEvent e) => _resolve().AssignMobileLiYanNextDrawBonus(e);
    internal void IssueMobileLiYanNextDrawBonus(ProgramSkillFrame supplied, int targetSeat, int amount) => _resolve().IssueMobileLiYanNextDrawBonus(supplied, targetSeat, amount);
    internal int MobileLiYanNextDrawBonus(CharacterState player) => _resolve().MobileLiYanNextDrawBonus(player);
}
