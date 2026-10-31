#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class NativeFrameAutomaticRules(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : INativeFrameAutomaticRules
    {
        public void AdvanceRulesAndPublishState() => advanceRulesAndPublishState();
    }
