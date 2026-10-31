#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreMatchSetupKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreMatchSetupModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreMatchSetupModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreMatchSetupCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreMatchSetupCoreModePresentationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ModePresentationContext CaptureModePresentationContext();
}

internal interface ICoreMatchSetupKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreMatchSetupCoreOpeningCustomizationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequestOpeningHandConfirmation();
}
