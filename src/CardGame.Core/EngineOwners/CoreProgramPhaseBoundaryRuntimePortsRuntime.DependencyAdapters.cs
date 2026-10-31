#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCorePrepDiscardEndingPromisesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCorePrepDiscardEndingPromisesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> _targetFactory = targetFactory;
    public void AppendPrepDiscardEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendPrepDiscardEndingItems(items);
    public void ConsumeSkippedPrepDiscardEnding(global::CardGame.Core.TurnEndingBoundaryFrame frame) => this._targetFactory().ConsumeSkippedPrepDiscardEnding(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterLvQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterLvQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.WeiluPhaseKey? CaptureWeiluStartingPhase(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureWeiluStartingPhase(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterYanPuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterYanPuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> _targetFactory = targetFactory;
    public void AppendYanPuEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendYanPuEndingItems(items);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCoreEarnedActualEndingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCoreEarnedActualEndingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> _targetFactory = targetFactory;
    public void AppendEarnedActualEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendEarnedActualEndingItems(items);
    public void ConsumeSkippedEarnedActualEnding(global::CardGame.Core.TurnEndingBoundaryFrame frame) => this._targetFactory().ConsumeSkippedEarnedActualEnding(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterRuanYuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterRuanYuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> _targetFactory = targetFactory;
    public void AppendRuanYuXingzuoEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendRuanYuXingzuoEndingItems(items);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void FinalizeEndTurn(global::CardGame.Core.CharacterState previous) => this._targetFactory().FinalizeEndTurn(previous);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterZhangJiaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterZhangJiaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryTianjieDeadOwnerEnding(int actualOwner) => this._targetFactory().HasAnniversaryTianjieDeadOwnerEnding(actualOwner);
    public bool IsAnniversaryTianjieDeadOwnerEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, int actualOwner) => this._targetFactory().IsAnniversaryTianjieDeadOwnerEndingCandidate(candidate, actualOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateQushiEndingContext(global::CardGame.Core.TurnEndingBoundaryFrame parent, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateQushiEndingContext(parent, c, context);
    public void AppendQushiEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendQushiEndingItems(items);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterLuZhiWeiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterLuZhiWeiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.QingzhongPlayPhaseKey? CaptureQingzhongStartingPhase(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureQingzhongStartingPhase(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterWenYangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterWenYangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime> _targetFactory = targetFactory;
    public void AppendWenYangEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendWenYangEndingItems(items);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCoreConvertingGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCoreConvertingGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> _targetFactory = targetFactory;
    public void AppendGiftRetentionEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendGiftRetentionEndingItems(items);
    public bool ConsumeGiftRetentionEndingItem(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.TurnEndingBoundaryItem item) => this._targetFactory().ConsumeGiftRetentionEndingItem(frame, item);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCoreFixedDistanceOneDebtAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCoreFixedDistanceOneDebtOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> _targetFactory = targetFactory;
    public void AppendFixedDistanceOneEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendFixedDistanceOneEndingItems(items);
    public void ConsumeSkippedFixedDistanceDebt(global::CardGame.Core.TurnEndingBoundaryFrame f) => this._targetFactory().ConsumeSkippedFixedDistanceDebt(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWenheActivityProgramsRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCoreWenheActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWenheActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCoreWenheActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWenheActivityProgramsRuntime> _targetFactory = targetFactory;
    public bool HasWenheDeadOwnerEnding(int actualOwner) => this._targetFactory().HasWenheDeadOwnerEnding(actualOwner);
    public bool IsWenheDeadOwnerEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, int actualOwner) => this._targetFactory().IsWenheDeadOwnerEndingCandidate(candidate, actualOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCoreNeighborDiscardOpportunitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCoreNeighborDiscardOpportunitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime> _targetFactory = targetFactory;
    public int PreviousLivingSeatFor(int owner) => this._targetFactory().PreviousLivingSeatFor(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterLiuBaOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterLiuBaOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime> _targetFactory = targetFactory;
    public void AppendLiuBaCoinEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendLiuBaCoinEndingItems(items);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiWanOlRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCharacterLiWanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiWanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCharacterLiWanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiWanOlRuntime> _targetFactory = targetFactory;
    public void AppendLianjuEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => this._targetFactory().AppendLianjuEndingItems(items);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreProgramPhaseBoundaryRuntimePortsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseBoundaryRuntimePortsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool UsesTreatedWounded { get => this._targetFactory().UsesTreatedWounded; }

    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}
