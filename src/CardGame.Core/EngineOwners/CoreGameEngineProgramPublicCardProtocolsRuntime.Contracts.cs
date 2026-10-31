#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramPublicCardProtocolsCoreRecipientCategoryMarkObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidRecipientCategoryMarkReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramPublicCardProtocolsCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouLuSuAnniversaryRuntime MouLuSuAnniversary { get; }
}
