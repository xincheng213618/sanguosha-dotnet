#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHalfHandPhaseDebtReceiptsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreHalfHandPhaseDebtReceiptsCoreHalfHandPhaseDebtProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource HalfHandDebtSource(global::CardGame.Core.ProgramSkillFrame f);
    bool HalfHandDebtLifecycleMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreHalfHandPhaseDebtReceiptsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreHalfHandPhaseDebtReceiptsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreHalfHandPhaseDebtReceiptsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreHalfHandPhaseDebtReceiptsCoreHalfHandTargetSupportOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandSupportPaymentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ActualUseTargetWindowFrame parent, bool requirePaid);
}
