#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPublicCardProtocolsRuntime
{
    string global::CardGame.Core.IProgramPublicCardProtocolQueries.CompletedUndamagedRevealBind => "completed-undamaged-target-reveal";

    bool global::CardGame.Core.IProgramPublicCardProtocolQueries.ValidMouLuSuReceipt(ProgramSkillFrame frame) => _characterLuSuMouOperations.MouLuSuAnniversary.Valid(frame);
    bool global::CardGame.Core.IProgramPublicCardProtocolQueries.ValidRecipientCategoryMarkReceipt(ProgramSkillFrame frame) => _coreRecipientCategoryMarkObserversOperations.ValidRecipientCategoryMarkReceipt(frame);
}
