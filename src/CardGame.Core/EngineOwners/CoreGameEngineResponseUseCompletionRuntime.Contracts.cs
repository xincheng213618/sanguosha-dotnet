#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreResponseUseCompletionCharacterZouShiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeZouShiCapability { get; }
}

internal interface ICoreResponseUseCompletionCoreWholeActualTurnUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksWholeActualTurnUseOrdinal { get; }
}

internal interface ICoreResponseUseCompletionCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueFinishedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreResponseUseCompletionCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreResponseUseCompletionCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreResponseUseCompletionCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreResponseUseCompletionCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteCaoXiNativeCounterspellBeforeUseCompleted(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreResponseUseCompletionCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreResponseUseCompletionCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiLsAutonomousResponse(global::CardGame.Core.CardActionContext a);
}

internal interface ICoreResponseUseCompletionCharacterMaMiDiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaMiDiCapability { get; }
}

internal interface ICoreResponseUseCompletionCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardResponseCompleted(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation originalContinuation);
}

internal interface ICoreResponseUseCompletionCharacterLiFengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJiyunResponseCompletion(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    bool HasLiFengCapability { get; }
}

internal interface ICoreResponseUseCompletionCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
    long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreResponseUseCompletionContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreResponseUseCompletionCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSiruiCapability { get; }
}

internal interface ICoreResponseUseCompletionCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame ClearTieredRoundCounterspellResponse(global::CardGame.Core.NullificationWindowFrame input);
}

internal interface ICoreResponseUseCompletionCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreResponseUseCompletionProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreResponseUseCompletionCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreResponseUseCompletionCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreResponseUseCompletionCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareMouZhaJiangResponse(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardKind kind);
}

internal interface ICoreResponseUseCompletionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreResponseUseCompletionCharacterWuAnGuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuAnGuoCapability { get; }
}

internal interface ICoreResponseUseCompletionCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLiuChengSuspendedCounterspell(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreResponseUseCompletionCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCuiYanCapability { get; }
}

internal interface ICoreResponseUseCompletionCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksCurrentTurnUseKinds { get; }

    bool TracksActualTurnUseOrdinal { get; }
}

internal interface ICoreResponseUseCompletionCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHengwuCapability { get; }
}
