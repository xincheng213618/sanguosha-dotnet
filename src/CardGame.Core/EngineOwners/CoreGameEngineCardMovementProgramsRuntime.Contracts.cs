#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardMovementProgramsCardsMovedWindowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardsMovedWindowRuntime Get();
}

internal interface ICoreCardMovementProgramsCardMovementBatchCoordinatorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementBatchCoordinator Get();
}

internal interface ICoreCardMovementProgramsProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}

internal interface ICoreCardMovementProgramsCoreGainGiftProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksRedOwnedLoss { get; }
}

internal interface ICoreCardMovementProgramsKernelCoreGameEngineProgramTriggerWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost();
}

internal interface ICoreCardMovementProgramsCoreProgramShanheSourceZhaoranOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheSourceZhaoranRules { get; }
}

internal interface ICoreCardMovementProgramsCharacterFaZhengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouFaZhengCapability { get; }
}

internal interface ICoreCardMovementProgramsCharacterZhouFeiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeZhouFeiLingkongCapability { get; }
}

internal interface ICoreCardMovementProgramsCharacterZhaoZhongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhaoZhongCapability { get; }
}

internal interface ICoreCardMovementProgramsCharacterSimaYiOlGod2016Operations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOl2016SimaRenjie { get; }
}

internal interface ICoreCardMovementProgramsCoreActualHandGainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasActualHandGainCapability { get; }
}

internal interface ICoreCardMovementProgramsCharacterQiaoRuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryQiaoRuiCapability { get; }
}

internal interface ICoreCardMovementProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardMovementProgramsProgramPhaseScheduleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule { get; }
}

internal interface ICoreCardMovementProgramsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreCardMovementProgramsCoreJudgedRankSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOtherActualBasicDiscardCapability { get; }
}

internal interface ICoreCardMovementProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCardMovementProgramsCharacterHuCheErAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHuCheErRules { get; }
}

internal interface ICoreCardMovementProgramsCoreShanheChanrangEdictOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheChanrangCapability { get; }
}

internal interface ICoreCardMovementProgramsCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceWenhePoisonLoss(global::CardGame.Core.CardsMovedTriggerWindowFrame supplied);
}
