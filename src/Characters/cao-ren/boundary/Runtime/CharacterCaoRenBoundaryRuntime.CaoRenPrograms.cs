#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoRenBoundaryRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ICaoRenProgramHost.DiscardHandOrUseEquipment(ProgramSkillFrame frame) => BeginDiscardHandOrUseEquipment(frame);
    SkillProgramStepOutcome global::CardGame.Core.ICaoRenProgramHost.MoveFieldEquipment(ProgramSkillFrame frame) => BeginMoveFieldEquipment(frame);
}
