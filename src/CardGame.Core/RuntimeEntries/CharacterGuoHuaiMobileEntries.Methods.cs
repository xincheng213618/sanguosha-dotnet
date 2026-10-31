#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiMobileEntries
{
    internal bool CanRunMobileGuoHuaiJingce(int ownerSeat) => _resolve().CanRunMobileGuoHuaiJingce(ownerSeat);
    internal bool QualifyMobileGuoHuaiJingce(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().QualifyMobileGuoHuaiJingce(candidate, context);
}
