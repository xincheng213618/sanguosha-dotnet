#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26810)]
    internal void RequireCaoXi(ProgramSkillFrame f)
    {
        if (f.CaoXiGangshu is not null && !ValidCaoXiGangshu(f) || f.CaoXiJianxuan is not null && !ValidCaoXiJianxuan(f))
            throw new InvalidOperationException("Cao Xi lost its original source, actual use/damage window, single recipient or paid per-draw cursor.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && (f.CaoXiGangshu is not null || f.CaoXiJianxuan?.Stage == CaoXiJianxuanStage.ChoosingRecipient) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } p || p.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, CaoXiChoices(f))))
            throw new InvalidOperationException("Cao Xi lost its exact unissued owner choice.");
    }
}
