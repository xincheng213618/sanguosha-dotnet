#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramPairedColorResponseAiCoreDamageCounterpartAcquisitionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDamageCounterpartAcquisition(int owner, string skillId, string stateId, int counterpart);
}

internal interface ICoreProgramPairedColorResponseAiCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramPairedColorResponseAiCorePairedColorResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PairedColorResponseLinkedEvent? ExactPairedColorResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}
