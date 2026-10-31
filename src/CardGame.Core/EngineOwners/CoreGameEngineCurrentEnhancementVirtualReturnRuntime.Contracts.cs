#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCurrentEnhancementVirtualReturnCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCurrentEnhancementVirtualReturnCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreCurrentEnhancementVirtualReturnCoreCurrentEnhancementSlashRedirectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCurrentEnhancementVirtualProducer(global::CardGame.Core.CardUseFrame use);
    bool IsCurrentEnhancementSlashRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CurrentEnhancementSlashRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
}

internal interface ICoreCurrentEnhancementVirtualReturnContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCurrentEnhancementVirtualReturnCharacterLuBuHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyLuBuTrueVirtualProducer(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreCurrentEnhancementVirtualReturnResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}
