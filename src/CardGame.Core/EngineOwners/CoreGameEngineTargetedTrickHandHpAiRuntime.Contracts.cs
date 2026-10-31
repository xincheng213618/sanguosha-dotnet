#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTargetedTrickHandHpAiCoreTargetedOrdinaryTrickOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetTargetedOrdinaryTrickUse(global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TargetedTrickChoices(long frameId, global::CardGame.Core.CardUseFrame use, bool removeTargets = false);
}

internal interface ICoreTargetedTrickHandHpAiCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
