#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramNativeSlashQueriesCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuJingHejiRequest(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramNativeSlashQueriesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramNativeSlashQueriesCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramNativeSlashQueriesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }
}

internal interface ICoreProgramNativeSlashQueriesCoreRoundOnceCardlessProgramRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RoundOnceCardlessProgramActor(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op, int actor);
}

internal interface ICoreProgramNativeSlashQueriesCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCaoChunInitialRequest(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramNativeSlashQueriesCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQigongRequest(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramNativeSlashQueriesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}
