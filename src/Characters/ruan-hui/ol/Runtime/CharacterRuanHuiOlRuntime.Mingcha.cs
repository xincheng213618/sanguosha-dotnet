#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanHuiOlRuntime
{
    internal IReadOnlyList<Card> MingchaRandomCards(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat)).OrderBy(c => c.Id).ToArray());
    internal bool IsMingchaNormalDrawReplacement(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context && _coreProgramEventHistoryOperations.ProgramEventHistory<RuanHuiStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == RuanHuiSource(f) && e.GameplayHash == f.GameplayHash && e.ParentId == context.ParentFrameId && e.Op == SkillProgramEffectOp.ResolveMingchaRevealDrawChoice) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<RuanHuiDecisionEvent>().Count(e => e.FrameId == f.Id && e.ReplaceDraw) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<RuanHuiCompletedEvent>().Count(e => e.FrameId == f.Id) == 1;
}
