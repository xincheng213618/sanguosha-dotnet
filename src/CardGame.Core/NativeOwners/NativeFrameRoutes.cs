#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class NativeFrameRoutes(global::CardGame.Core.NativeOwners.GetNativeTryAdvanceTarget getTryAdvanceTarget) : INativeFrameRoutes
    {
        public bool TryAdvance(ResolutionFrame frame) => getTryAdvanceTarget()(frame);
    }
