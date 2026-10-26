# 陆逊·移动版

> 此版本与 `classic:lu-xun` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。普通移动身份，完整引用既有技能，不复制运行时或新增重复人物夹具。

### 陆逊：完整复用依据

- [官网14](https://www.sanguosha.cn/pc/hero-detail-14.html)、[人物34839](https://wiki.biligame.com/msgs/index.php?title=陆逊&oldid=34839)（当前页面2025-11-10）、[谦逊32675](https://wiki.biligame.com/msgs/index.php?title=谦逊/陆逊&oldid=32675)、[连营24417](https://wiki.biligame.com/msgs/index.php?title=连营/陆逊&oldid=24417)均保留普通经典规则。国战当前“节/度势”另有正文，不能替代普通版。
- 既有谦逊是 `prohibitTarget`，准确禁止顺手牵羊/乐不思蜀的选目标资格，不是先成为目标再取消，也不是界版收手牌。
- 连营在真实 `cardsMoved` 窗口观察Hand由正数到0，`perBatch`摸1；没有“失去几张就摸几张”，没有仅弃牌原因过滤、仅出牌阶段限制或次数上限。实体移动、逻辑丢弃、使用和响应仍走原生移动窗口，不能合成一个空手事实代替。
- 已有 `HuBanChecks.HuiyunNaturalHandUseQuotaAndNativeKindsCold` 验证原生火攻初始失去最后手牌及火攻弃牌再次失去最后手牌的两个真实连营子结算，且在原生火攻付款窗cold；`QuHuangChecks.QiejianActualLastHandSelfForeignRoundAndRefillCold` 验证连营实际补牌使后续空手资格失效；`CardMovementProgramChecks.AtomicBatchRunsPerCardAndPerBatchAndReplays` 覆盖按batch与card区别。纯引用不需复制这一矩阵。
- 普通图：[1400.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/1400.jpg)，HEAD 200/image/jpeg/293996字节。官网首张31000是界陆逊普通画，不可拿来作本普通身份立绘。建议portraitKey `mobile-lu-xun`，group `standard`。


本地立绘为 `official-mobile-lu-xun.jpg`，图鉴 `standard`；集成定向验证不表示全量通过。
