#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunZiLiuFangMobileEntries
{
    internal void CaptureMobileJiyuInvocation(ProgramSkillStartedEvent started) => _resolve().CaptureMobileJiyuInvocation(started);
    internal void CompleteMobileJiyuInvocation(ProgramSkillResolvedEvent resolved) => _resolve().CompleteMobileJiyuInvocation(resolved);
    internal bool HasMobileJiyuOriginalSelection(ProgramSkillFrame frame) => _resolve().HasMobileJiyuOriginalSelection(frame);
    internal bool KeepMobileJiyuInitialSelection(ProgramSkillFrame frame) => _resolve().KeepMobileJiyuInitialSelection(frame);
    internal SkillGrantPhaseExpiry? MobileJiyuCurrentPlay(int owner) => _resolve().MobileJiyuCurrentPlay(owner);
    internal IEnumerable<MobileJiyuInvokedEvent> MobileJiyuInvocations(int owner) => _resolve().MobileJiyuInvocations(owner);
    internal int MobileJiyuLimit(int owner) => _resolve().MobileJiyuLimit(owner);
    internal bool MobileJiyuMaySelect(int owner, int target, int usesBefore) => _resolve().MobileJiyuMaySelect(owner, target, usesBefore);
    internal bool ValidMobileJiyuInvocation(MobileJiyuInvokedEvent invoked) => _resolve().ValidMobileJiyuInvocation(invoked);
}
