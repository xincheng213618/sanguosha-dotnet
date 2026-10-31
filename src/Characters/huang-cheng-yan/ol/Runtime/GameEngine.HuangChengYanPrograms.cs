namespace CardGame.Core;
public sealed partial class GameEngine
{
    // 解阵 conversions survive across turns until the owner's next turn start or
    // the converted character's 八卦阵 judgment; replay rebuilds the map while the
    // accepted commands re-execute the converting op.
    private ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.JiezhenConversionState> _jiezhenConversions => ref _characterHuangChengYanOl.ReferenceToJiezhenConversions;
}
