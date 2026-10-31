#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterHuangfuSongWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterHuangfuSongWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeWuHfEndingItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => this._targetFactory().IncludeWuHfEndingItems(parent, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterCaiFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterCaiFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateOrdinaryCaiFuRenQietingContext(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateOrdinaryCaiFuRenQietingContext(parent, candidate, context);
    public bool IsOrdinaryCaiFuRenQietingTrigger(global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().IsOrdinaryCaiFuRenQietingTrigger(trigger);
    public bool IsEligibleOrdinaryCaiFuRenQietingCandidate(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().IsEligibleOrdinaryCaiFuRenQietingCandidate(parent, candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCoreActualEndedTurnEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCoreActualEndedTurnEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> _targetFactory = targetFactory;
    public bool IsActualEndedEquipmentTrigger(global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().IsActualEndedEquipmentTrigger(trigger);
    public global::CardGame.Core.ActualEndedTurnEquipmentContext? CaptureActualEndedEquipmentContext(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().CaptureActualEndedEquipmentContext(parent);
    public bool MatchesActualEndedEquipmentContext(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.SkillProgramTrigger trigger, bool requireLivingTurnOwner = true) => this._targetFactory().MatchesActualEndedEquipmentContext(parent, candidate, context, trigger, requireLivingTurnOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterHeQiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterHeQiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsIssuedShiHqCleanupCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => this._targetFactory().IsIssuedShiHqCleanupCandidate(c, w);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeShiHqAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => this._targetFactory().IncludeShiHqAfterTurnEndedItems(parent, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterZhangFeiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterZhangFeiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeShenZfAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame p, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().IncludeShenZfAfterTurnEndedItems(p, items);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterAHuiNanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterAHuiNanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> _targetFactory = targetFactory;
    public bool IsAHuiNanJuemanEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().IsAHuiNanJuemanEndingCandidate(c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterXunYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterXunYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime> _targetFactory = targetFactory;
    public bool IsIssuedMouXyAllocationCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().IsIssuedMouXyAllocationCandidate(c, context);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeMouXyAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => this._targetFactory().IncludeMouXyAfterTurnEndedItems(parent, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterCaoYuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterCaoYuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateXiangxuAfterTurnEndedContext(global::CardGame.Core.DeferredTurnEndFrame p, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateXiangxuAfterTurnEndedContext(p, c, context);
    public bool IsIssuedXiangxuEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().IsIssuedXiangxuEndingCandidate(c, context);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeXiangxuAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => this._targetFactory().IncludeXiangxuAfterTurnEndedItems(parent, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public bool IsIssuedXumingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().IsIssuedXumingCandidate(c, context);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeXumingAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => this._targetFactory().IncludeXumingAfterTurnEndedItems(parent, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts) => this._targetFactory().CollectEligibleProgramTriggerCandidates(owner, window, facts);
    public bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanRunProgramTrigger(candidate, context);
    public void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().ExposeProgramTriggerDecision(candidate, context);
    public void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().BeginProgramBinding(candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterWenYangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterWenYangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime> _targetFactory = targetFactory;
    public bool IsWenYangChoujueCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => this._targetFactory().IsWenYangChoujueCandidate(candidate, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterCaoZhenMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterCaoZhenMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsIssuedShiCaoZEndCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().IsIssuedShiCaoZEndCandidate(c, context);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeShiCaoZAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => this._targetFactory().IncludeShiCaoZAfterTurnEndedItems(parent, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCoreDeferredHandAlignmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCoreDeferredHandAlignmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _targetFactory = targetFactory;
    public void ContinueDeferredTurnEnd() => this._targetFactory().ContinueDeferredTurnEnd();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCharacterQinLangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCharacterQinLangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _targetFactory = targetFactory;
    public bool IsIssuedQinLangXianyingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().IsIssuedQinLangXianyingCandidate(c, context);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeQinLangXianyingAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => this._targetFactory().IncludeQinLangXianyingAfterTurnEndedItems(parent, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreAfterTurnEndedProgramsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAfterTurnEndedProgramsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}
