#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRiceJudgmentKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreRiceJudgmentCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreRiceJudgmentCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreRiceJudgmentCorePublicPileSourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICoreRiceJudgmentCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreRiceJudgmentCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreRiceJudgmentCoreJudgmentReplacementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CommitProgramJudgmentReplacement(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card oldJudgmentCard, global::CardGame.Core.Card replacement, global::CardGame.Core.Suit committedSuit, global::CardGame.Core.SkillProgramOldJudgmentCardDestination oldDestination, global::CardGame.Core.CardLocation? exactRiceLocation = default);
}

internal interface ICoreRiceJudgmentCoreRicePileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PublicPersistentPileSource> RiceSources(int owner, global::CardGame.Core.SkillProgramEffect effect, string instance);
    bool ExactRiceParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op);
    global::CardGame.Core.CardConversionSource RiceIssuer(global::CardGame.Core.ProgramSkillFrame f);
    bool DrainRiceChildren(global::CardGame.Core.ProgramSkillFrame f);
    bool ReturnRiceMovement(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRiceJudgmentCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreRiceJudgmentCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreRiceJudgmentCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}
