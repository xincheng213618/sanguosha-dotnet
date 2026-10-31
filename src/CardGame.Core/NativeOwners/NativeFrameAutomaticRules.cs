#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class NativeFrameAutomaticRules(global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : INativeFrameAutomaticRules
    {
        public void AdvanceRulesAndPublishState() => advanceRulesAndPublishState();
    }
