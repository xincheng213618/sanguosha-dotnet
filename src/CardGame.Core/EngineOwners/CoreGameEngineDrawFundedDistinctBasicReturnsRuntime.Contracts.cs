#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDrawFundedDistinctBasicReturnsCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetChanshuangSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongFollowupHandle? ActiveQinglongFollowup { get; set; }
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WuJingHejiAdapterActor(global::CardGame.Core.ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CaoChunInitialAdapterActor(global::CardGame.Core.ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request);
}

internal interface ICoreDrawFundedDistinctBasicReturnsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDrawFundedDistinctBasicReturnsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreDrawFundedDistinctBasicObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawFundedDistinctBasicPaidSuffix(int paidIndex);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreDrawFundedDistinctBasicUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDrawFundedDistinctBasicUse(long frameId);
    global::CardGame.Core.Card DrawFundedDistinctBasicRepresentation(long frameId);
    bool IsIssuedDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame use);
    bool IsDrawFundedDistinctBasicIssued(global::CardGame.Core.DrawFundedDistinctBasicUseReceipt receipt);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ReadTieredRoundUseAppearance(long ownerId, global::CardGame.Core.CardAppearanceReference appearance);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidDrawFundedDistinctBasicPayment(global::CardGame.Core.DrawFundedDistinctBasicPayment p);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool QigongAdapterActor(global::CardGame.Core.ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request);
}

internal interface ICoreDrawFundedDistinctBasicReturnsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ParticipantHandRescueObserverRide(global::CardGame.Core.ResolutionFrame frame, global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardUseFrame rescue);
}

internal interface ICoreDrawFundedDistinctBasicReturnsCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId);
}
