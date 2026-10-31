#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramLifecycleProgramPhaseSchedulingOperations : IProgramRuntimeOperations
{
    ProgramPhaseSchedulingRuntime Get();
}

internal interface ICoreProgramLifecycleCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreProgramLifecycleProgramBindingOperations : IProgramRuntimeOperations
{
    ProgramBindingRuntime Get();
}

internal interface ICoreProgramLifecycleProgramTargetSelectionOperations : IProgramRuntimeOperations
{
    ProgramTargetSelectionRuntime Get();
}

internal interface ICoreProgramLifecycleProgramOwnedZoneDiscardOperations : IProgramRuntimeOperations
{
    ProgramOwnedZoneDiscardRuntime Get();
}

internal interface ICoreProgramLifecycleCorePublicDamagePredictionOperations : IProgramRuntimeOperations
{
    PlayerSkillContext CreateAiSkillContext(CharacterState player, int viewerSeat, GameSnapshot? snapshot = default);
    Func<int, int, int>? CreatePublicSourceOccurrenceDamageBonus(int viewerSeat);
    PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, LegalAction action, int targetSeat);
}

internal interface ICoreProgramLifecycleProgramTriggerOperations : IProgramRuntimeOperations
{
    ProgramTriggerRuntime Get();
}

internal interface ICoreProgramLifecycleProgramRandomCardAcquisitionOperations : IProgramRuntimeOperations
{
    ProgramRandomCardAcquisitionRuntime Get();
}

internal interface ICoreProgramLifecycleProgramDamageCardClaimOperations : IProgramRuntimeOperations
{
    ProgramDamageCardClaimRuntime Get();
}

internal interface ICoreProgramLifecycleProgramWindowCursorOperations : IProgramRuntimeOperations
{
    ProgramWindowCursorRuntime Get();
}

internal interface ICoreProgramLifecycleProgramPindianOperations : IProgramRuntimeOperations
{
    ProgramPindianRuntime Get();
}

internal interface ICoreProgramLifecycleProgramTurnWindowEntryOperations : IProgramRuntimeOperations
{
    ProgramTurnWindowEntryRuntime Get();
}

internal interface ICoreProgramLifecycleProgramTriggerFactProjectorOperations : IProgramRuntimeOperations
{
    ProgramTriggerFactProjector Get();
}

internal interface ICoreProgramLifecycleProgramJudgmentOperations : IProgramRuntimeOperations
{
    ProgramJudgmentRuntime Get();
}

internal interface ICoreProgramLifecycleProgramDrawOperations : IProgramRuntimeOperations
{
    ProgramDrawRuntime Get();
}

internal interface ICoreProgramLifecycleProgramPhaseBoundaryOperations : IProgramRuntimeOperations
{
    ProgramPhaseBoundaryRuntime Get();
}

internal interface ICoreProgramLifecycleProgramDrawDiscardWindowEntryOperations : IProgramRuntimeOperations
{
    ProgramDrawDiscardWindowEntryRuntime Get();
}

internal interface ICoreProgramLifecycleProgramHealthStateOperations : IProgramRuntimeOperations
{
    ProgramHealthStateRuntime Get();
}

internal interface ICoreProgramLifecycleCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreProgramLifecycleProgramBoundCardOperations : IProgramRuntimeOperations
{
    ProgramBoundCardRuntime Get();
}

internal interface ICoreProgramLifecycleProgramActivationWindowOperations : IProgramRuntimeOperations
{
    ProgramActivationWindowRuntime Get();
}
