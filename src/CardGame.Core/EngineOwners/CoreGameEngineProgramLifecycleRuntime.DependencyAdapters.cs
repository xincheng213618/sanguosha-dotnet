#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramPhaseSchedulingRuntime))]
internal sealed class CoreProgramLifecycleProgramPhaseSchedulingAdapter(Func<ProgramPhaseSchedulingRuntime> targetFactory) : ICoreProgramLifecycleProgramPhaseSchedulingOperations
{
    private readonly Func<ProgramPhaseSchedulingRuntime> _targetFactory = targetFactory;
    public ProgramPhaseSchedulingRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CoreProgramLifecycleCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICoreProgramLifecycleCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(ProgramBindingRuntime))]
internal sealed class CoreProgramLifecycleProgramBindingAdapter(Func<ProgramBindingRuntime> targetFactory) : ICoreProgramLifecycleProgramBindingOperations
{
    private readonly Func<ProgramBindingRuntime> _targetFactory = targetFactory;
    public ProgramBindingRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramTargetSelectionRuntime))]
internal sealed class CoreProgramLifecycleProgramTargetSelectionAdapter(Func<ProgramTargetSelectionRuntime> targetFactory) : ICoreProgramLifecycleProgramTargetSelectionOperations
{
    private readonly Func<ProgramTargetSelectionRuntime> _targetFactory = targetFactory;
    public ProgramTargetSelectionRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramOwnedZoneDiscardRuntime))]
internal sealed class CoreProgramLifecycleProgramOwnedZoneDiscardAdapter(Func<ProgramOwnedZoneDiscardRuntime> targetFactory) : ICoreProgramLifecycleProgramOwnedZoneDiscardOperations
{
    private readonly Func<ProgramOwnedZoneDiscardRuntime> _targetFactory = targetFactory;
    public ProgramOwnedZoneDiscardRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreGameEnginePublicDamagePredictionRuntime))]
internal sealed class CoreProgramLifecycleCorePublicDamagePredictionAdapter(Func<CoreGameEnginePublicDamagePredictionRuntime> targetFactory) : ICoreProgramLifecycleCorePublicDamagePredictionOperations
{
    private readonly Func<CoreGameEnginePublicDamagePredictionRuntime> _targetFactory = targetFactory;
    public PlayerSkillContext CreateAiSkillContext(CharacterState player, int viewerSeat, GameSnapshot? snapshot = default) => this._targetFactory().CreateAiSkillContext(player, viewerSeat, snapshot);
    public Func<int, int, int>? CreatePublicSourceOccurrenceDamageBonus(int viewerSeat) => this._targetFactory().CreatePublicSourceOccurrenceDamageBonus(viewerSeat);
    public PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, LegalAction action, int targetSeat) => this._targetFactory().PredictPublicActionDamage(viewerSeat, action, targetSeat);
}

[Core.ProgramCapability(typeof(ProgramTriggerRuntime))]
internal sealed class CoreProgramLifecycleProgramTriggerAdapter(Func<ProgramTriggerRuntime> targetFactory) : ICoreProgramLifecycleProgramTriggerOperations
{
    private readonly Func<ProgramTriggerRuntime> _targetFactory = targetFactory;
    public ProgramTriggerRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramRandomCardAcquisitionRuntime))]
internal sealed class CoreProgramLifecycleProgramRandomCardAcquisitionAdapter(Func<ProgramRandomCardAcquisitionRuntime> targetFactory) : ICoreProgramLifecycleProgramRandomCardAcquisitionOperations
{
    private readonly Func<ProgramRandomCardAcquisitionRuntime> _targetFactory = targetFactory;
    public ProgramRandomCardAcquisitionRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramDamageCardClaimRuntime))]
internal sealed class CoreProgramLifecycleProgramDamageCardClaimAdapter(Func<ProgramDamageCardClaimRuntime> targetFactory) : ICoreProgramLifecycleProgramDamageCardClaimOperations
{
    private readonly Func<ProgramDamageCardClaimRuntime> _targetFactory = targetFactory;
    public ProgramDamageCardClaimRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramWindowCursorRuntime))]
internal sealed class CoreProgramLifecycleProgramWindowCursorAdapter(Func<ProgramWindowCursorRuntime> targetFactory) : ICoreProgramLifecycleProgramWindowCursorOperations
{
    private readonly Func<ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramPindianRuntime))]
internal sealed class CoreProgramLifecycleProgramPindianAdapter(Func<ProgramPindianRuntime> targetFactory) : ICoreProgramLifecycleProgramPindianOperations
{
    private readonly Func<ProgramPindianRuntime> _targetFactory = targetFactory;
    public ProgramPindianRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramTurnWindowEntryRuntime))]
internal sealed class CoreProgramLifecycleProgramTurnWindowEntryAdapter(Func<ProgramTurnWindowEntryRuntime> targetFactory) : ICoreProgramLifecycleProgramTurnWindowEntryOperations
{
    private readonly Func<ProgramTurnWindowEntryRuntime> _targetFactory = targetFactory;
    public ProgramTurnWindowEntryRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramTriggerFactProjector))]
internal sealed class CoreProgramLifecycleProgramTriggerFactProjectorAdapter(Func<ProgramTriggerFactProjector> targetFactory) : ICoreProgramLifecycleProgramTriggerFactProjectorOperations
{
    private readonly Func<ProgramTriggerFactProjector> _targetFactory = targetFactory;
    public ProgramTriggerFactProjector Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramJudgmentRuntime))]
internal sealed class CoreProgramLifecycleProgramJudgmentAdapter(Func<ProgramJudgmentRuntime> targetFactory) : ICoreProgramLifecycleProgramJudgmentOperations
{
    private readonly Func<ProgramJudgmentRuntime> _targetFactory = targetFactory;
    public ProgramJudgmentRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramDrawRuntime))]
internal sealed class CoreProgramLifecycleProgramDrawAdapter(Func<ProgramDrawRuntime> targetFactory) : ICoreProgramLifecycleProgramDrawOperations
{
    private readonly Func<ProgramDrawRuntime> _targetFactory = targetFactory;
    public ProgramDrawRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramPhaseBoundaryRuntime))]
internal sealed class CoreProgramLifecycleProgramPhaseBoundaryAdapter(Func<ProgramPhaseBoundaryRuntime> targetFactory) : ICoreProgramLifecycleProgramPhaseBoundaryOperations
{
    private readonly Func<ProgramPhaseBoundaryRuntime> _targetFactory = targetFactory;
    public ProgramPhaseBoundaryRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramDrawDiscardWindowEntryRuntime))]
internal sealed class CoreProgramLifecycleProgramDrawDiscardWindowEntryAdapter(Func<ProgramDrawDiscardWindowEntryRuntime> targetFactory) : ICoreProgramLifecycleProgramDrawDiscardWindowEntryOperations
{
    private readonly Func<ProgramDrawDiscardWindowEntryRuntime> _targetFactory = targetFactory;
    public ProgramDrawDiscardWindowEntryRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramHealthStateRuntime))]
internal sealed class CoreProgramLifecycleProgramHealthStateAdapter(Func<ProgramHealthStateRuntime> targetFactory) : ICoreProgramLifecycleProgramHealthStateOperations
{
    private readonly Func<ProgramHealthStateRuntime> _targetFactory = targetFactory;
    public ProgramHealthStateRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramLifecycleCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreProgramLifecycleCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[Core.ProgramCapability(typeof(ProgramBoundCardRuntime))]
internal sealed class CoreProgramLifecycleProgramBoundCardAdapter(Func<ProgramBoundCardRuntime> targetFactory) : ICoreProgramLifecycleProgramBoundCardOperations
{
    private readonly Func<ProgramBoundCardRuntime> _targetFactory = targetFactory;
    public ProgramBoundCardRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramActivationWindowRuntime))]
internal sealed class CoreProgramLifecycleProgramActivationWindowAdapter(Func<ProgramActivationWindowRuntime> targetFactory) : ICoreProgramLifecycleProgramActivationWindowOperations
{
    private readonly Func<ProgramActivationWindowRuntime> _targetFactory = targetFactory;
    public ProgramActivationWindowRuntime Get() => this._targetFactory();
}
