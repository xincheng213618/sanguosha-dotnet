#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramAlcoholConversionsRuntime
{
    private readonly ICoreProgramAlcoholConversionsCharacterGuanYuWumiaoOperations _characterGuanYuWumiaoOperations;
    private readonly ICoreProgramAlcoholConversionsCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreProgramAlcoholConversionsCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreProgramAlcoholConversionsCoreCardConversionsOperations _coreCardConversionsOperations;
    internal CoreGameEngineProgramAlcoholConversionsRuntime(ICoreProgramAlcoholConversionsCharacterGuanYuWumiaoOperations characterGuanYuWumiaoOperations, ICoreProgramAlcoholConversionsCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreProgramAlcoholConversionsCoreActionQueriesOperations coreActionQueriesOperations, ICoreProgramAlcoholConversionsCoreCardConversionsOperations coreCardConversionsOperations)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
    }
}
