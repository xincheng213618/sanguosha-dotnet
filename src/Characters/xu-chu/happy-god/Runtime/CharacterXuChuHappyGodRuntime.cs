#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterXuChuHappyGodRuntime
{
    private readonly ICharacterXuChuHappyGodCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    internal CharacterXuChuHappyGodRuntime(ICharacterXuChuHappyGodCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
    }
}
