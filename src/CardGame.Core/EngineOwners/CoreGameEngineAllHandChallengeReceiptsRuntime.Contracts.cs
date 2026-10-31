#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAllHandChallengeReceiptsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreAllHandChallengeReceiptsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreAllHandChallengeReceiptsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreAllHandChallengeReceiptsCoreSequentialDiscardProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource SequentialDiscardSource(global::CardGame.Core.ProgramSkillFrame f);
    global::System.Collections.Generic.IReadOnlyList<int> SequentialDiscardOrder(int owner, int start, bool ring);
}

internal interface ICoreAllHandChallengeReceiptsCoreSequentialDiscardReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidSequentialDiscardPayment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramSequentialDiscardDraft d, global::CardGame.Core.ProgramSequentialDiscardPayment p);
}

internal interface ICoreAllHandChallengeReceiptsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreAllHandChallengeReceiptsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}
