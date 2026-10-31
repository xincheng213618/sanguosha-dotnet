#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuYouAnniversaryEntries
{
    internal void AssertLiuYou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertLiuYou(f);
    internal bool CanStartLiuYou(global::CardGame.Core.CharacterState owner) => _resolve().CanStartLiuYou(owner);
    internal void CaptureAnniversaryLiuYouNextSlash(long cardUseFrameId) => _resolve().CaptureAnniversaryLiuYouNextSlash(cardUseFrameId);
    internal void CaptureLiuYouPindian(global::CardGame.Core.PindianResultDeterminedEvent fact) => _resolve().CaptureLiuYouPindian(fact);
    internal bool KeepLiuYouPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepLiuYouPaid(f);
    internal bool LiuYouPhaseAvailable(int owner) => _resolve().LiuYouPhaseAvailable(owner);
    internal global::CardGame.Core.AnniversaryLiuYouStartedEvent[] LiuYouPhaseStarts(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().LiuYouPhaseStarts(owner, phase);
    internal int LiuYouProjectedUses(int owner, string skill, int fallback) => _resolve().LiuYouProjectedUses(owner, skill, fallback);
    internal bool LiuYouTarget(int owner, int target) => _resolve().LiuYouTarget(owner, target);
    internal global::CardGame.Core.AnniversaryLiuYouBonusIssuedEvent[] LiuYouUnconsumed(int beneficiary) => _resolve().LiuYouUnconsumed(beneficiary);
    internal void PrepareLiuYou(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PrepareLiuYou(supplied);
    internal void ResolveLiuYou(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveLiuYou(supplied);
    internal bool ValidLiuYou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidLiuYou(f);
}
