#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunZiLiuFangMobileEntries
{
    internal void CaptureMobileJiyuInvocation(global::CardGame.Core.ProgramSkillStartedEvent started) => _resolve().CaptureMobileJiyuInvocation(started);
    internal void CompleteMobileJiyuInvocation(global::CardGame.Core.ProgramSkillResolvedEvent resolved) => _resolve().CompleteMobileJiyuInvocation(resolved);
    internal bool HasMobileJiyuOriginalSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasMobileJiyuOriginalSelection(frame);
    internal bool KeepMobileJiyuInitialSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepMobileJiyuInitialSelection(frame);
    internal global::CardGame.Core.SkillGrantPhaseExpiry? MobileJiyuCurrentPlay(int owner) => _resolve().MobileJiyuCurrentPlay(owner);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MobileJiyuInvokedEvent> MobileJiyuInvocations(int owner) => _resolve().MobileJiyuInvocations(owner);
    internal int MobileJiyuLimit(int owner) => _resolve().MobileJiyuLimit(owner);
    internal bool MobileJiyuMaySelect(int owner, int target, int usesBefore) => _resolve().MobileJiyuMaySelect(owner, target, usesBefore);
    internal bool ValidMobileJiyuInvocation(global::CardGame.Core.MobileJiyuInvokedEvent invoked) => _resolve().ValidMobileJiyuInvocation(invoked);
}
