#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePindianRevealWindowsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePindianRevealWindowsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICorePindianRevealWindowsCorePindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompletePindian(global::CardGame.Core.PindianFrame frame);
}

internal interface ICorePindianRevealWindowsCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> AnniversaryCuiLieIssuedPindianCandidates(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> cards);
}

internal interface ICorePindianRevealWindowsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICorePindianRevealWindowsCharacterZhangLiaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMobileZhitiPindianPrograms(global::CardGame.Core.PindianFrame parent);
}

internal interface ICorePindianRevealWindowsCoreSimultaneousPindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource SimultaneousSource(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePindianRevealWindowsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICorePindianRevealWindowsCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDouyuPairAssignment(global::CardGame.Core.PindianFrame pair, global::CardGame.Core.PindianResult result);
}

internal interface ICorePindianRevealWindowsCorePindianClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BeginPindianClaims(global::CardGame.Core.PindianFrame frame);
}

internal interface ICorePindianRevealWindowsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICorePindianRevealWindowsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICorePindianRevealWindowsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePindianRevealWindowsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePindianRevealWindowsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICorePindianRevealWindowsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuLuKangAssignedPair(global::CardGame.Core.PindianFrame f, global::CardGame.Core.PindianResult result);
}

internal interface ICorePindianRevealWindowsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}
