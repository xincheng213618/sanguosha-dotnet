#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeCardContinuationCursorsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeCardContinuationCursorsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNativeCardContinuationCursorsKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeCardContinuationCursorsKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(long ownerId) => engine.ComposeNativeDuelHandle(ownerId);
        public global::CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId) => engine.ComposeNativeGroupCardHandle(ownerId);
        public global::CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId) => engine.ComposeNativeBorrowedSwordHandle(ownerId);
        public global::CardGame.Core.NativeOwners.StoneAxeHandle CreateStoneAxeHandle(long ownerId) => engine.ComposeNativeStoneAxeHandle(ownerId);
        public global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(long ownerId) => engine.ComposeNativeCixiongDoubleSwordsHandle(ownerId);
        public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(long ownerId) => engine.ComposeNativeQinglongCrescentBladeHandle(ownerId);
        public global::CardGame.Core.NativeOwners.QinglongFollowupHandle CreateQinglongFollowupHandle(long ownerId) => engine.ComposeNativeQinglongFollowupHandle(ownerId);
        public global::CardGame.Core.NativeOwners.IceSwordHandle CreateIceSwordHandle(long ownerId) => engine.ComposeNativeIceSwordHandle(ownerId);
        public global::CardGame.Core.NativeOwners.QilinBowHandle CreateQilinBowHandle(long ownerId) => engine.ComposeNativeQilinBowHandle(ownerId);
        public global::CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId) => engine.ComposeNativeFangtianHalberdHandle(ownerId);
        public global::CardGame.Core.NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(long ownerId) => engine.ComposeNativeFactionDefenseHandle(ownerId);
        public global::CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId) => engine.ComposeNativeFactionCardRequestHandle(ownerId);
    }
}
