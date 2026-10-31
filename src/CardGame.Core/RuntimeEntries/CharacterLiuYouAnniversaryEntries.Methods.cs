#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuYouAnniversaryEntries
{
    internal bool CanStartLiuYou(CharacterState owner) => _resolve().CanStartLiuYou(owner);
    internal void CaptureAnniversaryLiuYouNextSlash(long cardUseFrameId) => _resolve().CaptureAnniversaryLiuYouNextSlash(cardUseFrameId);
    internal void CaptureLiuYouPindian(PindianResultDeterminedEvent fact) => _resolve().CaptureLiuYouPindian(fact);
    internal bool KeepLiuYouPaid(ProgramSkillFrame f) => _resolve().KeepLiuYouPaid(f);
    internal bool LiuYouPhaseAvailable(int owner) => _resolve().LiuYouPhaseAvailable(owner);
    internal AnniversaryLiuYouStartedEvent[] LiuYouPhaseStarts(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().LiuYouPhaseStarts(owner, phase);
    internal int LiuYouProjectedUses(int owner, string skill, int fallback) => _resolve().LiuYouProjectedUses(owner, skill, fallback);
    internal bool LiuYouTarget(int owner, int target) => _resolve().LiuYouTarget(owner, target);
    internal AnniversaryLiuYouBonusIssuedEvent[] LiuYouUnconsumed(int beneficiary) => _resolve().LiuYouUnconsumed(beneficiary);
    internal void PrepareLiuYou(ProgramSkillFrame supplied) => _resolve().PrepareLiuYou(supplied);
    internal void ResolveLiuYou(ProgramSkillFrame supplied) => _resolve().ResolveLiuYou(supplied);
    internal bool ValidLiuYou(ProgramSkillFrame f) => _resolve().ValidLiuYou(f);
}
