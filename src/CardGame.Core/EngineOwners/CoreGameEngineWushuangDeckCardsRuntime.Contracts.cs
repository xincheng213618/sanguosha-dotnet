#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreWushuangDeckCardsCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}
