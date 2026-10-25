# 曹植（移动版）

核对日期：2026-10-26。独立移动身份 `mobile:cao-zhi`，魏、男性、3体力。

采用[移动版官网101](https://www.sanguosha.cn/pc/hero-detail-101.html)，并与[移动版 BWIKI 曹植](https://wiki.biligame.com/msgs/曹植)的普通身份核对。落英完整引用 `classic:luoying`：其他角色的梅花牌因弃置或判定进入弃牌堆时可获得；不引入界版落英实体标记、距离和响应特效。

酒诗完整引用 `classic:jiushi`。本批修复其既有文案承诺但此前缺失的出牌阶段入口：复用 `useVirtualAlcohol`，先支付真实翻面成本，等待翻面原生子结算，再使用一张零实体酒；资料同时声明主动形式。原濒死酒与伤后翻回分支保留，不复制一个同规则技能或升级包版本。

来源差异：官网酒诗只要求伤害后当前背面朝上；BWIKI 普通身份进一步要求受到该次伤害时也背面朝上。此处明确采用官方优先口径，与原经典酒诗伤后条件一致，没有暗中声称这条限定双源一致。

`ClassicCaoZhiWineChecks.ActiveJiushiPaysFaceChildThenUsesNativeZeroMaterialWineCold` 聚焦修复：已付翻面子窗暂停与恢复、原帧零实体酒、实际使用一次且手牌未被当作材料、背面不再次提供未付费主动入口。落英与既有濒死/伤害流程继承共享检查。

立绘使用官网轮播内普通身份的[经典原图15100](https://www.sanguosha.cn/storage/uploads/images/skins/15100.jpg)，`portraitKey=mobile-cao-zhi`；该页首项50100不作为本普通身份画像。
