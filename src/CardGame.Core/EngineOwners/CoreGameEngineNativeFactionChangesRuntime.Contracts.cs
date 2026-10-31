#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeFactionChangesCharacterSunShangxiangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouSunShangxiangMobile { get; }
}

internal interface ICoreNativeFactionChangesCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeFactionChangesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNativeFactionChangesCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreNativeFactionChangesCharacterCaoTengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCaoTengFactionChange(global::CardGame.Core.ProgramSkillFrame f, string faction);
}

internal interface ICoreNativeFactionChangesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeFactionChangesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeFactionChangesCoreModeAllegianceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}

internal interface ICoreNativeFactionChangesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeFactionChangesCorePrivateGeneralLibrariesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PrivateGeneralLibrary? EffectiveGeneralLibrary(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeFactionChangesCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeFactionChangesCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}
