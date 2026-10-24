# 移动版界虞翻

## 规则来源

- [三国杀移动版官网：界·虞翻，386](https://www.sanguosha.cn/index.php/pc/hero-detail-386.html)，核对日期：2026-10-25。纵玄包括自己的牌因弃置进入弃牌堆后的任意张置顶，以及出牌阶段限一次摸一张后将一张牌置顶。官网有直言标题，但未提供其正文。
- [移动版 BWIKI：界虞翻](https://wiki.biligame.com/msgs/%E7%95%8C%E8%99%9E%E7%BF%BB)（页面标注更新日期 2024-10-08）、[直言/界虞翻](https://wiki.biligame.com/msgs/%E7%9B%B4%E8%A8%80/%E7%95%8C%E8%99%9E%E7%BF%BB)（2023-02-01）：同版直言在自己的结束阶段，令任意一名角色摸一张并展示，若为装备牌，则该角色使用它并回复一点体力。
- [官方规则集 3.0 镜像：游戏牌的操作](https://gltjk.com/sanguosha/rules/glossary/gamecard.html)、[区域](https://gltjk.com/sanguosha/rules/glossary/zone.html)：手牌与装备是角色拥有的牌；判定区内的牌不是该角色拥有的牌。同版纵玄未找到包含判定区牌的特殊裁定，因此被动的来源和主动放置范围均为手牌、装备区。

独立武将 `boundary:yu-fan-mobile`，纵玄 `boundary:zongxuan-mobile`、直言 `boundary:zhiyan-mobile`，变体 `boundary-mobile`、规则集 `sanguosha-mobile`，吴势力、男性、三体力。本版不包含现有 OL 版的上家首次弃牌、上家结束阶段和非装备体力不等时扣血。

## 复用实现

纵玄被动直接使用 `cardsMoved` 的原生 `perOwnerBatch`、`movementDiscardOnly` 和 `putDiscardedCardsOnDrawPileTop`。只处理本人的真实手牌、装备弃置，保留原弃置来源及 Processing 中转证明；使用或打出后的通常清理并不等于弃置。候选必须仍在弃牌堆，按下一张先摸的顺序暂选，确认后移动相应实体。

主动纵玄以 `usesPerPhase: 1` 计数。先执行原生 `draw` 并等待移动子窗，再通过 `selectOwnedCards` 从当前手牌和装备中选择一张，以 `moveBoundCards` 原生置顶并等待后续移动时机。可以放回刚摸到的牌，也可以选择原有手牌或装备；启动前不预收这张牌。所有绑定、原区域及付款延续保留在原程序帧。

直言复用原有通用链：选择任一存活角色、实际原生摸一张并绑定、公开展示、筛出真实装备牌、由该目标使用原实体、回复一点体力。非装备没有额外效果。实际装备使用、替换与子窗沿现有 `useBoundCardByTarget`，没有直接安装或制造额外实体。

没有新增运行时、操作枚举或共享 hook。复用时修正了弃牌置顶候选未遵守触发器 `sourceZones` 的缺口：同一真实批次同时弃置手牌和判定区牌时，原批次继续保留两种来源，候选按本版 HE 过滤；其他已配置区域的版本仍遵守其原配置。

现有弃牌来源、置顶顺序和装备使用机制检查继续承担通用覆盖。接入检查验证新增主动入口先摸后选原有手牌或装备、每阶段一次，以及装备选择处代表性冷恢复和下一次真实摸牌得到原置顶实体；混合批次回归则保留真实闪电使用和手牌、判定区牌的同批弃置，确认读档后只能收回符合配置的手牌。`BoundaryYuFanScenario` 使用公开 API，界面若需复用，可从 `CreateScene()` 的出牌阶段调用 `BeginZongxuan()` 到达真实区域选牌。
