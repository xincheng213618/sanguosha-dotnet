#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramAlcoholConversionsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    internal CoreGameEngineProgramAlcoholConversionsRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
    }
}
